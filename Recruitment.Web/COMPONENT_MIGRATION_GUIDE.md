# Shadcn UI Component Migration Guide

This guide explains how to update remaining views to use the new shadcn UI component system.

## Component Library

All component styles are in `/wwwroot/css/shadcn-components.css`. The layout includes:
- **Sidebar** navigation (color: HSL 197, 78.9%, 14.9%)
- **Breadcrumb** navigation
- **Toast notifications** using Sonner

## Component Classes Reference

### Cards
```html
<div class="card-component">
    <div class="card-header">
        <h2 class="card-title">Title</h2>
        <p class="card-description">Description</p>
    </div>
    <div class="card-content">
        <!-- Content -->
    </div>
    <div class="card-footer">
        <!-- Footer -->
    </div>
</div>
```

### Scrollable Tables
For tables that may overflow on smaller screens:
```html
<div class="table-scroll-wrapper">
    <div class="table-component">
        <table>
            <thead>
                <tr>
                    <th style="min-width: 150px;">Column 1</th>
                    <th style="min-width: 120px;">Column 2</th>
                </tr>
            </thead>
            <tbody>...</tbody>
        </table>
    </div>
</div>
```

### Stat/Info Boxes
```html
<div class="card-stat-box">
    <strong class="card-stat-label">Label</strong>
    <span class="card-stat-value">Value</span>
</div>
```

### Tables
```html
<div class="table-component">
    <table>
        <thead>
            <tr>
                <th>Column 1</th>
                <th>Column 2</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Data 1</td>
                <td>Data 2</td>
            </tr>
        </tbody>
    </table>
</div>
```

### Buttons
```html
<!-- Primary Button -->
<button class="btn-component btn-primary">
    <i class="bi bi-plus-circle me-2"></i>
    Add New
</button>

<!-- Secondary Button -->
<button class="btn-component btn-secondary">Cancel</button>

<!-- Outline Button -->
<button class="btn-component btn-outline">View</button>

<!-- Destructive Button -->
<button class="btn-component btn-destructive">Delete</button>

<!-- Small Button -->
<button class="btn-component btn-sm btn-primary">Edit</button>
```

### Badges
```html
<span class="badge-component badge-default">Default</span>
<span class="badge-component badge-success">Active</span>
<span class="badge-component badge-destructive">Rejected</span>
<span class="badge-component badge-warning">Pending</span>
<span class="badge-component badge-secondary">Inactive</span>
```

### Form Fields (Legacy)
```html
<div class="form-field">
    <label for="name" class="form-label">
        Name <span class="text-danger">*</span>
    </label>
    <input type="text" 
           id="name"
           name="name" 
           class="form-input" 
           placeholder="Enter name"
           required />
    <p class="form-description">Helper text</p>
    <p class="form-error">Error message</p>
</div>
```

### Field Components (Recommended for New Forms)
The Field component system provides a more accessible and consistent form structure:

#### Basic Field
```html
<div class="field">
    <label class="field-label" for="name">
        Name <span class="text-danger">*</span>
        <span class="tooltip-trigger">
            <i>i</i>
            <span class="tooltip-content multiline">Enter the full name</span>
        </span>
    </label>
    <input type="text" 
           id="name"
           class="form-field-input" 
           required />
    <span class="field-description">This will be displayed in the system</span>
    <span class="field-error" style="display:none;">Name is required</span>
</div>
```

#### Field with Textarea
```html
<div class="field">
    <label class="field-label" for="description">Description</label>
    <textarea id="description" 
              class="form-field-textarea" 
              rows="4"></textarea>
    <span class="field-description">Optional description</span>
</div>
```

#### Field with Select
```html
<div class="field">
    <label class="field-label" for="category">Category</label>
    <select id="category" class="form-field-select">
        <option value="">Select category</option>
        <option value="1">Category 1</option>
    </select>
    <span class="field-description">Choose a category</span>
</div>
```

#### Horizontal Field (for checkboxes/switches)
```html
<div class="field orientation-horizontal">
    <input type="checkbox" id="active" class="form-check-input" />
    <div class="field-content">
        <label class="field-label mb-0" for="active">Active</label>
        <span class="field-description">Enable or disable</span>
    </div>
</div>
```

#### Field Group
```html
<div class="field-group">
    <div class="field">...</div>
    <div class="field">...</div>
    <div class="field-separator"></div>
    <div class="field">...</div>
</div>
```

#### Field Set
```html
<fieldset class="field-set">
    <legend class="field-legend">Personal Information</legend>
    <div class="field-group">
        <div class="field">...</div>
        <div class="field">...</div>
    </div>
</fieldset>
```

### Textarea
```html
<textarea class="textarea-component" 
          name="description" 
          rows="4"
          placeholder="Enter description"></textarea>
```

### Select Dropdown
```html
<select class="form-select">
    <option value="">Select option</option>
    <option value="1">Option 1</option>
</select>
```

### Alerts
```html
<!-- Success Alert -->
<div class="alert-component alert-success">
    <i class="bi bi-check-circle-fill alert-icon"></i>
    <div>
        <div class="alert-title">Success!</div>
        <div class="alert-description">Operation completed successfully</div>
    </div>
</div>

<!-- Destructive Alert -->
<div class="alert-component alert-destructive">
    <i class="bi bi-exclamation-triangle-fill alert-icon"></i>
    <div>
        <div class="alert-title">Are you sure?</div>
        <div class="alert-description">This action cannot be undone.</div>
    </div>
</div>
```

### Pagination
```html
<nav class="pagination-component" aria-label="Pagination">
    <a class="pagination-button disabled">
        <i class="bi bi-chevron-left"></i>
    </a>
    <a class="pagination-button active" href="?page=1">1</a>
    <a class="pagination-button" href="?page=2">2</a>
    <a class="pagination-button" href="?page=3">3</a>
    <a class="pagination-button" href="?page=2">
        <i class="bi bi-chevron-right"></i>
    </a>
</nav>
```

## Migration Patterns

### Pattern 1: Simple CRUD Pages (Country, Currency, Location, Title, Project)

**Reference:** `Views/Department/Index.cshtml`

Structure:
1. Card wrapper with header (title, description, add button)
2. Table component with data
3. Modals for Create, Edit, Delete
4. Toast notifications in scripts section

Key changes:
- Replace `<h2>` with card header
- Replace Bootstrap tables with `table-component`
- Replace Bootstrap buttons with `btn-component`
- Add alert components in delete modals
- Add toast notification scripts

### Pattern 2: List Pages with Search (UserManagement, Applicant, Interview)

**Reference:** `Views/Vacancy/Index.cshtml` or `Views/Application/Index.cshtml`

Structure:
1. Page header with title and actions
2. Search/Filter card with form
3. Table card with data
4. Pagination in card footer

Key changes:
- Search form uses `form-field`, `form-label`, `form-input`
- Add filter/clear buttons
- Use badge components for status
- Add action buttons with icons
- Implement pagination component

### Pattern 3: Dashboard/Stats Pages

**Reference:** `Views/Home/Index.cshtml`

Structure:
1. Stats cards in grid layout
2. Alert/notification cards
3. Quick action cards

Already well-styled, minimal changes needed.

### Pattern 4: Forms (Create/Edit Pages)

Structure:
```html
<div class="card-component">
    <div class="card-header">
        <h2 class="card-title">Add/Edit Item</h2>
    </div>
    <div class="card-content">
        <form method="post">
            <div class="row g-3">
                <div class="col-md-6">
                    <div class="form-field">
                        <label class="form-label">Field Name</label>
                        <input class="form-input" type="text" name="field" />
                    </div>
                </div>
            </div>
            <div class="mt-4 d-flex gap-2">
                <button type="submit" class="btn-component btn-primary">Save</button>
                <a href="..." class="btn-component btn-secondary">Cancel</a>
            </div>
        </form>
    </div>
</div>
```

## Toast Notifications

Add to scripts section for CRUD operations:

```javascript
@section Scripts {
    <script>
        // Show success toast if operation succeeded
        @if (TempData["SuccessMessage"] != null)
        {
            <text>
            showToast('@TempData["SuccessMessage"]', 'success');
            </text>
        }

        // Show error toast if operation failed
        @if (TempData["ErrorMessage"] != null)
        {
            <text>
            showToast('@TempData["ErrorMessage"]', 'error');
            </text>
        }

        // Or trigger on specific action
        function confirmDelete(id, name) {
            if (confirm(`Are you sure you want to delete "${name}"?`)) {
                // Perform delete
                fetch(`/Controller/Delete/${id}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                })
                .then(response => {
                    if (response.ok) {
                        showToast(`"${name}" deleted successfully`, 'success');
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        showToast('Failed to delete item', 'error');
                    }
                })
                .catch(error => {
                    showToast('An error occurred', 'error');
                });
            }
        }
    </script>
}
```

## Modal Updates

Replace Bootstrap modal headers/footers with new component styles:

**Destructive Modal (Delete):**
```html
<div class="modal-header" style="background-color: hsl(0, 84%, 60%); color: white;">
    <h5 class="modal-title">
        <i class="bi bi-exclamation-triangle-fill me-2"></i>
        Confirm Delete
    </h5>
    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
</div>
<div class="modal-body">
    <div class="alert-component alert-destructive mb-0">
        <i class="bi bi-exclamation-triangle-fill alert-icon"></i>
        <div>
            <div class="alert-title">Are you sure?</div>
            <div class="alert-description">
                You are about to delete <strong>Item Name</strong>.
                This action cannot be undone.
            </div>
        </div>
    </div>
</div>
<div class="modal-footer">
    <button type="button" class="btn-component btn-secondary" data-bs-dismiss="modal">
        Cancel
    </button>
    <button type="submit" class="btn-component btn-destructive">
        <i class="bi bi-trash me-2"></i>
        Delete Item
    </button>
</div>
```

**Form Modal (Create/Edit):**
```html
<div class="modal-header">
    <h5 class="modal-title">
        <i class="bi bi-plus-circle me-2"></i>
        Add New Item
    </h5>
    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
</div>
<div class="modal-body">
    <div class="form-field">
        <label class="form-label">Field Name</label>
        <input class="form-input" type="text" name="field" />
    </div>
</div>
<div class="modal-footer">
    <button type="button" class="btn-component btn-secondary" data-bs-dismiss="modal">
        Cancel
    </button>
    <button type="submit" class="btn-component btn-primary">
        <i class="bi bi-check-circle me-2"></i>
        Save Item
    </button>
</div>
```

## Icon Usage

Use Bootstrap Icons for visual indicators:
- `bi-plus-circle` - Add/Create actions
- `bi-pencil` - Edit actions
- `bi-trash` - Delete actions
- `bi-eye` - View/Details actions
- `bi-search` - Search
- `bi-funnel` - Filter
- `bi-check-circle` - Success/Active
- `bi-x-circle` - Cancel/Inactive
- `bi-exclamation-triangle-fill` - Warning/Delete
- `bi-person-circle` - User
- `bi-building` - Department
- `bi-briefcase-fill` - Vacancy
- `bi-calendar-event-fill` - Interview

## Utility Classes

```css
.flex-between { display: flex; justify-content: space-between; align-items: center; }
.flex-center { display: flex; justify-content: center; align-items: center; }
.gap-2 { gap: 0.5rem; }
.gap-4 { gap: 1rem; }
```

## Quick Checklist for Each View

- [ ] Replace page title with card header (title + description)
- [ ] Wrap content in `card-component`
- [ ] Update table to `table-component`
- [ ] Replace buttons with `btn-component`
- [ ] Replace badges with `badge-component`
- [ ] Update forms to use `form-field`, `form-label`, `form-input`
- [ ] Add icons to buttons and headings
- [ ] Update modals with new styles
- [ ] Add alert components to destructive modals
- [ ] Add toast notifications
- [ ] Test responsiveness and functionality

## Files Completed

✅ Completed:
- Views/Shared/_Layout.cshtml
- Views/UserManagement/Index.cshtml
- Views/UserManagement/Profile.cshtml (with Field components)
- Views/Permission/Index.cshtml (enhanced styling)
- Views/Permission/Setup.cshtml (with Field components)
- Views/Vacancy/Index.cshtml (with scrollable table)
- Views/Vacancy/Details.cshtml (enhanced with stat boxes and delete dialog)
- Views/Vacancy/GetSubmissions.cshtml (with scrollable table and improved styling)
- Views/Vacancy/Create.cshtml (with Field components and tooltips)
- Views/Vacancy/Edit.cshtml (with Field components and tooltips)
- Views/AppSetup/Index.cshtml
- Views/Application/Index.cshtml
- Views/Department/Index.cshtml
- wwwroot/css/shadcn-components.css (with Field components)

⏳ Remaining (Apply patterns above):
- Views/Country/Index.cshtml (Pattern 1)
- Views/Currency/Index.cshtml (Pattern 1)
- Views/Location/Index.cshtml (Pattern 1)
- Views/Title/Index.cshtml (Pattern 1)
- Views/Project/Index.cshtml (Pattern 1)
- Views/DepartmentTitle/Index.cshtml (Pattern 1)
- Views/Role/Index.cshtml (Pattern 1)
- Views/Applicant/Index.cshtml (Pattern 2)
- Views/Interview/Index.cshtml (Pattern 2)
- Various other Create/Edit/Details views (Pattern 4 - consider using Field components)

## Testing

After migration, test:
1. All CRUD operations (Create, Read, Update, Delete)
2. Search and filter functionality
3. Pagination
4. Modal interactions
5. Toast notifications
6. Responsive design on mobile
7. Sidebar navigation
8. Breadcrumb navigation
