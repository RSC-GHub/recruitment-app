# Recruitment System — VPS Deployment Guide

> **Target:** `163.245.208.94` (AlmaLinux 9.7) — alongside existing RSC website  
> **Domain:** `recruitment.redseaconstruct.com`  
> **Stack:** Docker (SQL Server 2022 + ASP.NET Core 8.0)  
> **API:** `recruitment.redseaconstruct.com/api/`  
> **RAM:** 8 GB total

---

## Architecture Overview

```
Internet
   │
   ▼
┌──────────────────────────────────────────────────────┐
│  Nginx (host)  — ports 80 / 443                      │
│  ├─ redseaconstruct.com     → Strapi :1337 (PM2)     │
│  ├─ recruitment.redseaconstruct.com                   │
│  │    ├─ /api/*  → recruitment-api  :5011 (Docker)    │
│  │    └─ /*      → recruitment-web  :5010 (Docker)    │
│  └─ Static frontend         → /var/www/rsc/dist/      │
├──────────────────────────────────────────────────────┤
│  Docker Compose                                       │
│  ├─ recruitment-web   (ASP.NET MVC)     → :5010       │
│  ├─ recruitment-api   (ASP.NET API)     → :5011       │
│  └─ recruitment-db    (SQL Server 2022) → :1433       │
│       └─ Volume: recruitment_sqlserver_data            │
│       └─ Volume: recruitment_cv_uploads                │
├──────────────────────────────────────────────────────┤
│  Existing services (untouched)                        │
│  ├─ Strapi (PM2) :1337                                │
│  ├─ PostgreSQL :5432                                  │
│  ├─ Apache :8081/8444                                 │
│  └─ DirectAdmin :2222                                 │
└──────────────────────────────────────────────────────┘
```

---

## Pre-Deployment Checklist

- [ ] SSH access to `163.245.208.94` as root
- [ ] DNS A record: `recruitment.redseaconstruct.com` → `163.245.208.94`
- [ ] GitHub token ready for private repo clone
- [ ] Port 1433 NOT exposed in CSF firewall (Docker internal only)

---

## Step 1 — DNS Record

Add an **A record** in your DNS provider (wherever `redseaconstruct.com` is managed):

```
Type: A
Name: recruitment
Value: 163.245.208.94
TTL: 300
```

Wait a few minutes for propagation. Verify:
```bash
dig recruitment.redseaconstruct.com +short
# Should return: 163.245.208.94
```

---

## Step 2 — SSH into VPS

```bash
ssh root@163.245.208.94
```

---

## Step 3 — Install Docker

```bash
# Install Docker Engine on AlmaLinux 9
dnf install -y yum-utils --disableexcludes=main
yum-config-manager --add-repo https://download.docker.com/linux/rhel/docker-ce.repo

dnf install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin --disableexcludes=main

# Start and enable Docker
systemctl start docker
systemctl enable docker

# Verify
docker --version
docker compose version
```

---

## Step 4 — Open Firewall Ports (if needed)

Docker manages its own iptables rules for container networking. The containers bind to `127.0.0.1` only, so no external ports are exposed. However, ensure ports 80 and 443 are open for Nginx (they should already be):

```bash
# Verify CSF allows 80 and 443 (should already be open for your website)
grep "TCP_IN" /etc/csf/csf.conf | head -1
# Should contain: 80,443

# Do NOT add port 1433 — SQL Server stays internal to Docker
```

---

## Step 5 — Clone the Repository

```bash
# Create project directory
mkdir -p /var/www/recruitment
cd /var/www/recruitment

# Clone private repo using token
git clone https://YOUR_GITHUB_TOKEN@github.com/RSC-GHub/recruitment-app.git .
```

> **Replace `YOUR_GITHUB_TOKEN` with the actual GitHub PAT token.**
> After cloning, the token is no longer needed (unless you `git pull` later).

---

## Step 6 — Configure Environment

```bash
# Create .env from template
cp .env.example .env

# Edit the password (or keep the default)
nano .env
```

Contents of `.env`:
```env
DB_SA_PASSWORD=P@ssw0rd@RSC2026!
```

> **Password requirements:** At least 8 characters, must include uppercase, lowercase, digit, and special character.

---

## Step 7 — Build and Start Docker Containers

```bash
cd /var/www/recruitment

# Build images and start all containers (first time takes 3-5 minutes)
docker compose up -d --build

# Watch the build progress
docker compose logs -f --tail=50
# Press Ctrl+C to stop following logs

# Verify all 3 containers are running
docker compose ps
```

Expected output:
```
NAME               STATUS          PORTS
recruitment-db     running (healthy)  127.0.0.1:1433->1433/tcp
recruitment-web    running            127.0.0.1:5010->8080/tcp
recruitment-api    running            127.0.0.1:5011->8080/tcp
```

### Wait for SQL Server to be healthy

```bash
# Check SQL Server health (may take 30-60 seconds on first start)
docker compose logs sqlserver --tail=20

# Verify database was created by EF Core migrations
docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
  -Q "SELECT name FROM sys.databases WHERE name = 'RecruitmentDB'"
```

> **Note:** On the very first start, the Recruitment.Web container will automatically run EF Core migrations and create all tables + seed the admin user. This may take 20-30 seconds.

---

## Step 8 — Configure Nginx

Create the Nginx server block for the recruitment subdomain:

```bash
nano /etc/nginx/conf.d/recruitment.conf
```

Paste the following:

```nginx
# ============================================================
#  recruitment.redseaconstruct.com — Nginx Reverse Proxy
#  Recruitment.Web  → 127.0.0.1:5010
#  Recruitment.Api  → 127.0.0.1:5011
# ============================================================

server {
    listen 80;
    listen [::]:80;
    server_name recruitment.redseaconstruct.com;

    # Redirect all HTTP to HTTPS (after SSL is set up)
    location / {
        return 301 https://$host$request_uri;
    }
}

server {
    listen 443 ssl http2;
    listen [::]:443 ssl http2;
    server_name recruitment.redseaconstruct.com;

    # --- SSL (will be filled by Certbot, or set manually) ---
    ssl_certificate     /etc/letsencrypt/live/recruitment.redseaconstruct.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/recruitment.redseaconstruct.com/privkey.pem;
    include             /etc/letsencrypt/options-ssl-nginx.conf;
    ssl_dhparam         /etc/letsencrypt/ssl-dhparams.pem;

    # --- Max upload size (50 MB for CVs) ---
    client_max_body_size 50M;

    # --- API routes (must be BEFORE the catch-all) ---
    location /api/ {
        proxy_pass         http://127.0.0.1:5011/api/;
        proxy_http_version 1.1;
        proxy_set_header   Host              $host;
        proxy_set_header   X-Real-IP         $remote_addr;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_set_header   Upgrade           $http_upgrade;
        proxy_set_header   Connection        "upgrade";
        proxy_buffering    off;
        proxy_read_timeout 120s;
    }

    # Swagger (API documentation) — uncomment if you want it accessible
    # location /swagger {
    #     proxy_pass         http://127.0.0.1:5011/swagger;
    #     proxy_http_version 1.1;
    #     proxy_set_header   Host              $host;
    #     proxy_set_header   X-Real-IP         $remote_addr;
    #     proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
    #     proxy_set_header   X-Forwarded-Proto $scheme;
    # }

    # --- Web app (catch-all) ---
    location / {
        proxy_pass         http://127.0.0.1:5010;
        proxy_http_version 1.1;
        proxy_set_header   Host              $host;
        proxy_set_header   X-Real-IP         $remote_addr;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_set_header   Upgrade           $http_upgrade;
        proxy_set_header   Connection        "upgrade";
        proxy_buffering    off;
        proxy_read_timeout 120s;
    }
}
```

**Test and reload Nginx:**

```bash
nginx -t
# If the test fails because SSL certs don't exist yet, temporarily comment out
# the entire HTTPS server block and the "return 301" line, then get SSL first.

systemctl reload nginx
```

---

## Step 9 — SSL Certificate

```bash
# Get SSL certificate for the recruitment subdomain
certbot --nginx -d recruitment.redseaconstruct.com

# Verify auto-renewal is active
systemctl status certbot-renew.timer
```

> **If Certbot `--nginx` plugin modifies your config:** That's fine — it will update the SSL paths automatically. You can compare with the config above to make sure nothing broke.

### Alternative: If Certbot fails with the HTTPS block

Comment out the HTTPS `server` block first, keep only the HTTP block without the redirect:

```bash
nano /etc/nginx/conf.d/recruitment.conf
```

Temporary HTTP-only config:
```nginx
server {
    listen 80;
    server_name recruitment.redseaconstruct.com;

    location /api/ {
        proxy_pass http://127.0.0.1:5011/api/;
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    location / {
        proxy_pass http://127.0.0.1:5010;
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Then:
```bash
nginx -t && systemctl reload nginx
certbot --nginx -d recruitment.redseaconstruct.com
# Certbot will add the SSL config automatically
```

---

## Step 10 — Verify Everything Works

```bash
# 1. Check containers are running
docker compose ps

# 2. Test Web app locally
curl -s -o /dev/null -w "%{http_code}" http://127.0.0.1:5010
# Expected: 302 (redirect to login)

# 3. Test API locally
curl -s -o /dev/null -w "%{http_code}" http://127.0.0.1:5011/api/Countries
# Expected: 200

# 4. Test via domain (after DNS + SSL)
curl -s -o /dev/null -w "%{http_code}" https://recruitment.redseaconstruct.com
# Expected: 302 → login page

# 5. Open in browser
# https://recruitment.redseaconstruct.com
# Login: admin@rsc.com.eg / Admin@123
```

---

## Daily Operations

### View logs

```bash
cd /var/www/recruitment

# All containers
docker compose logs --tail=50

# Specific container
docker compose logs recruitment-web --tail=30
docker compose logs recruitment-api --tail=30
docker compose logs sqlserver --tail=30

# Follow logs live
docker compose logs -f recruitment-web
```

### Restart services

```bash
cd /var/www/recruitment

# Restart everything
docker compose restart

# Restart single service
docker compose restart recruitment-web
docker compose restart recruitment-api

# Full stop and start
docker compose down
docker compose up -d
```

### Redeploy after code changes (pushed to GitHub)

```bash
cd /var/www/recruitment

# Pull latest code
git pull

# Rebuild and restart (only rebuilds changed images)
docker compose up -d --build

# Verify
docker compose ps
docker compose logs --tail=20
```

### Database backup

```bash
# Create backup
docker exec recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
  -Q "BACKUP DATABASE RecruitmentDB TO DISK='/var/opt/mssql/backup/RecruitmentDB_$(date +%Y%m%d_%H%M%S).bak'"

# Create backup directory first (one time)
docker exec recruitment-db mkdir -p /var/opt/mssql/backup

# List backups
docker exec recruitment-db ls -la /var/opt/mssql/backup/

# Copy backup to host
docker cp recruitment-db:/var/opt/mssql/backup/ /var/www/recruitment/backups/
```

### Database restore

```bash
# Copy backup file into container
docker cp /path/to/RecruitmentDB.bak recruitment-db:/var/opt/mssql/backup/

# Restore
docker exec recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
  -Q "RESTORE DATABASE RecruitmentDB FROM DISK='/var/opt/mssql/backup/RecruitmentDB.bak' WITH REPLACE"
```

### Access SQL Server directly

```bash
# Interactive SQL shell
docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C

# Run a query
# > SELECT COUNT(*) FROM RecruitmentDB.dbo.Countries;
# > GO
# > EXIT
```

### Check disk and RAM usage

```bash
# Docker disk usage
docker system df

# Container resource usage (live)
docker stats --no-stream

# Host RAM
free -h

# Host disk
df -h
```

---

## CV Uploads — Data Safety

CV files are stored in a **Docker named volume** (`recruitment_cv_uploads`). This volume:

- **Survives** `docker compose down` (stop + remove containers)
- **Survives** `docker compose up -d --build` (rebuild images)
- **Survives** `docker compose restart`
- **Survives** server reboot

It is **only deleted** if you explicitly run:
```bash
docker compose down -v          # ← the -v flag deletes volumes!
docker volume rm recruitment_cv_uploads  # ← explicit volume removal
```

> **NEVER use `docker compose down -v` in production** unless you want to wipe everything.

### Backup CV files

```bash
# Copy CVs from volume to host
docker cp recruitment-web:/app/uploads/cv/ /var/www/recruitment/backups/cv_$(date +%Y%m%d)/

# Or mount additional backup location
# Add to docker-compose.yml under recruitment-web volumes:
#   - /var/www/recruitment/backups/cv:/app/uploads/cv-backup
```

---

## Stored Procedure — ExportApplicantsReport

The application uses a stored procedure `dbo.ExportApplicantsReport` for the applicant export feature. After the first deployment, you must create this stored procedure manually:

```bash
# Connect to SQL Server
docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -d RecruitmentDB -C

# Then paste your stored procedure SQL:
# CREATE PROCEDURE dbo.ExportApplicantsReport ...
# GO
```

> If the stored procedure doesn't exist yet, the Export Applicants feature will throw an error. All other features work normally.

---

## Integrating API with Main Website (Careers Page)

The Recruitment API is available at:
```
https://recruitment.redseaconstruct.com/api/
```

### Available API Endpoints:

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Vacancies` | List open vacancies |
| GET | `/api/Countries` | List countries |
| GET | `/api/Currencies` | List currencies |
| POST | `/api/Applicant` | Submit new applicant |
| POST | `/api/Application` | Submit job application |

### Example: Fetch vacancies from your React website

```javascript
// In your React frontend at redseaconstruct.com
const response = await fetch('https://recruitment.redseaconstruct.com/api/Vacancies');
const vacancies = await response.json();
```

CORS is already configured to allow requests from:
- `https://redseaconstruct.com`
- `https://www.redseaconstruct.com`
- `https://recruitment.redseaconstruct.com`

---

## Troubleshooting

### Container won't start

```bash
# Check logs for the failing container
docker compose logs recruitment-web --tail=50
docker compose logs sqlserver --tail=50

# Common: SQL Server not ready yet — web/api will retry
# Wait 30-60 seconds, then check again
```

### 502 Bad Gateway

```bash
# Check if containers are running
docker compose ps

# If stopped, start them
docker compose up -d

# Check Nginx can reach the containers
curl -v http://127.0.0.1:5010
```

### SQL Server out of memory

```bash
# Check container resource usage
docker stats --no-stream

# Reduce SQL Server memory limit in docker-compose.yml:
# MSSQL_MEMORY_LIMIT_MB=512 (instead of 1024)
docker compose up -d
```

### EF Core migration error on startup

```bash
# Check web app logs for migration errors
docker compose logs recruitment-web --tail=50

# If needed, manually apply migrations
docker exec -it recruitment-web dotnet Recruitment.Web.dll --migrate
# Or rebuild with updated code
docker compose up -d --build
```

### Cannot pull from private GitHub repo

```bash
# Verify token works
git ls-remote https://YOUR_TOKEN@github.com/RSC-GHub/recruitment-app.git

# Store credentials so git pull works without token in URL
cd /var/www/recruitment
git remote set-url origin https://YOUR_TOKEN@github.com/RSC-GHub/recruitment-app.git
```

### SELinux blocking Docker or Nginx

```bash
# Check for SELinux denials
ausearch -m avc --start recent

# If Nginx can't connect to Docker ports
setsebool -P httpd_can_network_connect 1

# If Docker has issues
setsebool -P container_manage_cgroup 1
```

### Check what's using RAM

```bash
# Overall system
free -h

# Per-container
docker stats --no-stream

# Per-process
htop
```

---

## Complete Quick-Reference Commands

```bash
# === Deploy (first time) ===
ssh root@163.245.208.94
cd /var/www/recruitment
git clone https://TOKEN@github.com/RSC-GHub/recruitment-app.git .
cp .env.example .env
docker compose up -d --build

# === Redeploy (after code push) ===
cd /var/www/recruitment
git pull
docker compose up -d --build

# === Logs ===
docker compose logs -f                    # All logs (live)
docker compose logs recruitment-web -f    # Web only
docker compose logs recruitment-api -f    # API only
docker compose logs sqlserver -f          # DB only

# === Status ===
docker compose ps                         # Container status
docker stats --no-stream                  # Resource usage

# === Restart ===
docker compose restart                    # Restart all
docker compose restart recruitment-web    # Restart web only

# === Stop / Start ===
docker compose down                       # Stop all (keeps data)
docker compose up -d                      # Start all

# === Rebuild single service ===
docker compose up -d --build recruitment-web

# === Database ===
docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C

# === Nginx ===
nginx -t
systemctl reload nginx

# === SSL ===
certbot --nginx -d recruitment.redseaconstruct.com
certbot renew --dry-run
```

---

## Credentials Reference

| Item | Value |
|------|-------|
| **VPS IP** | `163.245.208.94` |
| **Domain** | `recruitment.redseaconstruct.com` |
| **SSH** | `ssh root@163.245.208.94` |
| **Admin Login** | `admin@rsc.com.eg` / `Admin@123` |
| **SQL Server SA** | `sa` / `P@ssw0rd@RSC2026!` |
| **DB Name** | `RecruitmentDB` |
| **Web Port (internal)** | `127.0.0.1:5010` |
| **API Port (internal)** | `127.0.0.1:5011` |
| **SQL Port (internal)** | `127.0.0.1:1433` |
| **GitHub Repo** | `github.com/RSC-GHub/recruitment-app.git` (private) |
| **Project Path** | `/var/www/recruitment/` |

---

## Port Map (Full VPS)

| Service | Port | Managed By |
|---------|------|------------|
| Nginx | 80, 443 | systemd |
| Strapi (website) | 1337 | PM2 |
| PostgreSQL (website) | 5432 | systemd |
| Apache | 8081, 8444 | systemd |
| DirectAdmin | 2222 | systemd |
| Recruitment Web | 5010 | Docker |
| Recruitment API | 5011 | Docker |
| SQL Server | 1433 | Docker |
