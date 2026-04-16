# Prompt for Claude Chat — Recruitment System Deployment

Copy everything below this line into a new Claude chat:

---

You are helping me deploy a recruitment system (ASP.NET Core 8.0) to my VPS using Docker. You will guide me step by step — give me ONE command at a time, wait for me to paste the output, then diagnose and give the next command. Do NOT dump all steps at once. Be interactive like a senior DevOps engineer sitting next to me.

If any command fails, help me fix it before moving on. If the output looks good, confirm it and move to the next step.

---

## EXISTING VPS INFO

- **IP:** `163.245.208.94`
- **OS:** AlmaLinux 9.7
- **SSH:** `ssh root@163.245.208.94`
- **Nginx:** already running on ports 80/443 (serving main website)
- **Existing website:** `redseaconstruct.com` (React + Strapi + PostgreSQL)
- **Strapi:** port 1337 (managed by PM2)
- **PostgreSQL:** port 5432 (for Strapi, NOT for this project)
- **Apache:** moved to 8081/8444
- **DirectAdmin:** port 2222
- **CSF firewall** (not firewalld)
- **SELinux:** enforcing, `httpd_can_network_connect` already enabled
- **Swap:** 2GB at `/swapfile`
- **RAM:** 8 GB total
- **Node:** 20.20.1 via NVM
- **Existing Nginx config:** `/etc/nginx/conf.d/redseaconstruct.conf`
- **SSL certs (existing site):** `/etc/letsencrypt/live/redseaconstruct.com/`
- **DirectAdmin DNF exclusions:** may need `--disableexcludes=main` for package installs

---

## WHAT WE'RE DEPLOYING

A recruitment system built with:
- **ASP.NET Core 8.0** (Clean Architecture)
- **Two apps:** Recruitment.Web (MVC frontend, port 5010) + Recruitment.Api (REST API, port 5011)
- **SQL Server 2022** in Docker (port 1433, localhost only)
- **Docker Compose** orchestrating all 3 containers
- **Domain:** `recruitment.redseaconstruct.com`

**GitHub repo (PRIVATE):** `https://github.com/RSC-GHub/recruitment-app.git`
**GitHub token:** `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`

---

## ARCHITECTURE

```
Internet → Nginx (host, 80/443)
              ├─ redseaconstruct.com         → Strapi :1337 (existing, don't touch)
              └─ recruitment.redseaconstruct.com
                   ├─ /api/*  → recruitment-api  container :5011
                   └─ /*      → recruitment-web  container :5010

Docker Compose:
  ├─ recruitment-db    (mcr.microsoft.com/mssql/server:2022-latest) → 127.0.0.1:1433
  ├─ recruitment-web   (ASP.NET MVC)                                → 127.0.0.1:5010
  └─ recruitment-api   (ASP.NET API)                                → 127.0.0.1:5011

Volumes:
  - recruitment_sqlserver_data  (SQL Server data — persists across restarts)
  - recruitment_cv_uploads      (CV file uploads — persists across restarts)
```

---

## CREDENTIALS

| Item | Value |
|------|-------|
| VPS SSH | `ssh root@163.245.208.94` |
| App Admin Login | `admin@rsc.com.eg` / `Admin@123` |
| SQL Server SA | `sa` / `P@ssw0rd@RSC2026!` |
| DB Name | `RecruitmentDB` |
| Web Port | `127.0.0.1:5010` |
| API Port | `127.0.0.1:5011` |
| SQL Port | `127.0.0.1:1433` (internal only) |
| Project Path on VPS | `/var/www/recruitment/` |

---

## DEPLOYMENT STEPS (follow this order)

### Phase 1: Prerequisites
1. I need to add a DNS A record: `recruitment` → `163.245.208.94` (you'll ask me to verify with `dig`)
2. SSH into VPS

### Phase 2: Install Docker
3. Install Docker Engine + Docker Compose plugin on AlmaLinux 9 (use `--disableexcludes=main` because DirectAdmin blocks packages)
4. Start and enable Docker
5. Verify docker and docker compose work

### Phase 3: Clone & Configure
6. Create `/var/www/recruitment/` directory
7. Clone the private repo using the GitHub token: `git clone https://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@github.com/RSC-GHub/recruitment-app.git .`
8. Set the remote URL with token so future `git pull` works: `git remote set-url origin https://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@github.com/RSC-GHub/recruitment-app.git`
9. Create `.env` from `.env.example` (contains `DB_SA_PASSWORD=P@ssw0rd@RSC2026!`)

### Phase 4: Build & Start Docker
10. Run `docker compose up -d --build` (first time takes 3-5 minutes for .NET SDK + SQL Server image download)
11. Watch logs with `docker compose logs -f --tail=50` — wait for SQL Server healthcheck to pass
12. Verify all 3 containers are running with `docker compose ps`
13. Verify SQL Server is healthy and database was created:
    ```
    docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
      -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
      -Q "SELECT name FROM sys.databases WHERE name = 'RecruitmentDB'"
    ```
14. Test web app responds: `curl -s -o /dev/null -w "%{http_code}" http://127.0.0.1:5010` (expect 302)
15. Test API responds: `curl -s -o /dev/null -w "%{http_code}" http://127.0.0.1:5011/api/Countries` (expect 200)

### Phase 5: Nginx Configuration
16. Create `/etc/nginx/conf.d/recruitment.conf` — BUT start with HTTP-only config first (no SSL yet):

```nginx
server {
    listen 80;
    listen [::]:80;
    server_name recruitment.redseaconstruct.com;

    client_max_body_size 50M;

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

17. Test nginx config: `nginx -t`
18. Reload nginx: `systemctl reload nginx`
19. Test via domain: `curl -s -o /dev/null -w "%{http_code}" http://recruitment.redseaconstruct.com` (expect 302)

### Phase 6: SSL
20. Run Certbot: `certbot --nginx -d recruitment.redseaconstruct.com`
21. Verify HTTPS works: `curl -s -o /dev/null -w "%{http_code}" https://recruitment.redseaconstruct.com` (expect 302)
22. Verify auto-renewal: `certbot renew --dry-run`

### Phase 7: Database Restore (Stored Procedures)
23. I have a database backup (.bak file) from my old SQL Server that contains stored procedures. Help me restore it:
    - I'll upload the .bak file to the VPS using scp or FileZilla
    - Copy it into the Docker container: `docker cp /var/www/recruitment/RecruitmentDB.bak recruitment-db:/var/opt/mssql/backup/`
    - First check the logical file names: 
      ```
      docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
        -Q "RESTORE FILELISTONLY FROM DISK='/var/opt/mssql/backup/RecruitmentDB.bak'"
      ```
    - Then restore with MOVE (using the logical names from the output above):
      ```
      docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
        -Q "RESTORE DATABASE RecruitmentDB FROM DISK='/var/opt/mssql/backup/RecruitmentDB.bak' WITH MOVE '<LogicalDataName>' TO '/var/opt/mssql/data/RecruitmentDB.mdf', MOVE '<LogicalLogName>' TO '/var/opt/mssql/data/RecruitmentDB_log.ldf', REPLACE"
      ```
    - **IMPORTANT:** Replace `<LogicalDataName>` and `<LogicalLogName>` with the actual values from FILELISTONLY output
    - After restore, the EF Core database will be replaced with the backup (including all stored procedures)
    - The admin seed user might already exist in the backup — if not, restart the web container to re-seed: `docker compose restart recruitment-web`

### Phase 8: Final Verification
24. Open browser: `https://recruitment.redseaconstruct.com`
25. Login with: `admin@rsc.com.eg` / `Admin@123`
26. Test API: `https://recruitment.redseaconstruct.com/api/Countries`
27. Check resource usage: `docker stats --no-stream`
28. Check overall RAM: `free -h`

---

## TROUBLESHOOTING KNOWLEDGE

**If `docker compose up` fails with port conflict:**
Port 1433 shouldn't conflict (bound to 127.0.0.1). If it does: `ss -tlnp | grep 1433`

**If SQL Server container keeps restarting:**
Check logs: `docker compose logs sqlserver --tail=50`
Common cause: password doesn't meet complexity requirements (needs uppercase + lowercase + digit + special char, min 8)

**If web/api container crashes on startup:**
Check logs: `docker compose logs recruitment-web --tail=50`
Common cause: SQL Server not ready yet — wait for healthcheck, then restart: `docker compose restart recruitment-web`

**If 502 Bad Gateway after Nginx setup:**
Containers might be down: `docker compose ps`
Or SELinux blocking: `setsebool -P httpd_can_network_connect 1`

**If Certbot fails:**
Make sure DNS is propagated: `dig recruitment.redseaconstruct.com +short` must return `163.245.208.94`
Make sure port 80 is open: `csf -l | grep "80"`

**If Docker install fails with DNF:**
DirectAdmin blocks packages. Use: `--disableexcludes=main` flag

**If `docker compose` command not found:**
Install the plugin: `dnf install -y docker-compose-plugin --disableexcludes=main`

**If image build runs out of memory:**
The VPS has 8GB + 2GB swap, should be fine. If not: `free -h` to check, and the Dockerfile already limits SQL Server to 1GB.

**NEVER run `docker compose down -v`** — the `-v` flag deletes all volumes including database data and CV uploads.

---

## AFTER DEPLOYMENT — USEFUL COMMANDS

```bash
# Redeploy after code push
cd /var/www/recruitment && git pull && docker compose up -d --build

# View logs
docker compose logs -f recruitment-web
docker compose logs -f recruitment-api
docker compose logs -f sqlserver

# Restart
docker compose restart

# Database shell
docker exec -it recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -d RecruitmentDB -C

# Backup database
docker exec recruitment-db mkdir -p /var/opt/mssql/backup
docker exec recruitment-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'P@ssw0rd@RSC2026!' -C \
  -Q "BACKUP DATABASE RecruitmentDB TO DISK='/var/opt/mssql/backup/RecruitmentDB.bak' WITH FORMAT"

# Backup CVs
docker cp recruitment-web:/app/uploads/cv/ /var/www/recruitment/backups/cv/

# Check status
docker compose ps
docker stats --no-stream
```

---

Now start guiding me. I'm ready to SSH into the VPS. Give me the first command and tell me what output to expect. One step at a time.
