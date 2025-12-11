/**
 * Simple Toast Notification System
 * A vanilla JavaScript implementation for browser-based toast notifications
 */

(function() {
    'use strict';

    // Toast container
    let toastContainer = null;

    // Toast counter for unique IDs
    let toastCounter = 0;

    // Initialize the toast container
    function initToastContainer() {
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.id = 'sonner-toaster';
            toastContainer.setAttribute('data-sonner-toaster', '');
            toastContainer.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 999999;
                pointer-events: none;
            `;
            document.body.appendChild(toastContainer);
        }
        return toastContainer;
    }

    // Create a toast element
    function createToastElement(message, type = 'default') {
        const toast = document.createElement('div');
        const toastId = `toast-${++toastCounter}`;
        toast.id = toastId;
        toast.setAttribute('data-sonner-toast', '');
        toast.setAttribute('data-type', type);
        
        // Icon based on type
        const icons = {
            success: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" style="width: 20px; height: 20px;"><path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.857-9.809a.75.75 0 00-1.214-.882l-3.483 4.79-1.88-1.88a.75.75 0 10-1.06 1.061l2.5 2.5a.75.75 0 001.137-.089l4-5.5z" clip-rule="evenodd"></path></svg>',
            error: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" style="width: 20px; height: 20px;"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-8-5a.75.75 0 01.75.75v4.5a.75.75 0 01-1.5 0v-4.5A.75.75 0 0110 5zm0 10a1 1 0 100-2 1 1 0 000 2z" clip-rule="evenodd"></path></svg>',
            warning: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" style="width: 20px; height: 20px;"><path fill-rule="evenodd" d="M9.401 3.003c1.155-2 4.043-2 5.197 0l7.355 12.748c1.154 2-.29 4.5-2.599 4.5H4.645c-2.309 0-3.752-2.5-2.598-4.5L9.4 3.003zM12 8.25a.75.75 0 01.75.75v3.75a.75.75 0 01-1.5 0V9a.75.75 0 01.75-.75zm0 8.25a.75.75 0 100-1.5.75.75 0 000 1.5z" clip-rule="evenodd"></path></svg>',
            info: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" style="width: 20px; height: 20px;"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a.75.75 0 000 1.5h.253a.25.25 0 01.244.304l-.459 2.066A1.75 1.75 0 0010.747 15H11a.75.75 0 000-1.5h-.253a.25.25 0 01-.244-.304l.459-2.066A1.75 1.75 0 009.253 9H9z" clip-rule="evenodd"></path></svg>',
            default: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" style="width: 20px; height: 20px;"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a.75.75 0 000 1.5h.253a.25.25 0 01.244.304l-.459 2.066A1.75 1.75 0 0010.747 15H11a.75.75 0 000-1.5h-.253a.25.25 0 01-.244-.304l.459-2.066A1.75 1.75 0 009.253 9H9z" clip-rule="evenodd"></path></svg>'
        };

        // Colors based on type
        const colors = {
            success: { bg: '#10b981', text: '#ffffff' },
            error: { bg: '#ef4444', text: '#ffffff' },
            warning: { bg: '#f59e0b', text: '#ffffff' },
            info: { bg: '#3b82f6', text: '#ffffff' },
            default: { bg: '#6b7280', text: '#ffffff' }
        };

        const color = colors[type] || colors.default;
        const icon = icons[type] || icons.default;

        toast.style.cssText = `
            background-color: ${color.bg};
            color: ${color.text};
            padding: 12px 16px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 10px;
            min-width: 300px;
            max-width: 400px;
            pointer-events: auto;
            opacity: 0;
            transform: translateX(100%);
            transition: all 0.3s ease;
            font-size: 14px;
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
        `;

        toast.innerHTML = `
            <div style="flex-shrink: 0;">${icon}</div>
            <div style="flex: 1; word-break: break-word;">${message}</div>
            <button onclick="window.sonner._removeToast('${toastId}')" style="
                background: transparent;
                border: none;
                color: ${color.text};
                cursor: pointer;
                padding: 0;
                margin: 0;
                width: 20px;
                height: 20px;
                display: flex;
                align-items: center;
                justify-content: center;
                opacity: 0.7;
                transition: opacity 0.2s;
            " onmouseover="this.style.opacity='1'" onmouseout="this.style.opacity='0.7'">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <line x1="18" y1="6" x2="6" y2="18"></line>
                    <line x1="6" y1="6" x2="18" y2="18"></line>
                </svg>
            </button>
        `;

        return toast;
    }

    // Show a toast
    function showToast(message, type = 'default', duration = 4000) {
        const container = initToastContainer();
        const toast = createToastElement(message, type);
        
        container.appendChild(toast);

        // Trigger animation
        setTimeout(() => {
            toast.style.opacity = '1';
            toast.style.transform = 'translateX(0)';
        }, 10);

        // Auto remove after duration
        if (duration > 0) {
            setTimeout(() => {
                removeToast(toast.id);
            }, duration);
        }

        return toast.id;
    }

    // Remove a toast
    function removeToast(toastId) {
        const toast = document.getElementById(toastId);
        if (toast) {
            toast.style.opacity = '0';
            toast.style.transform = 'translateX(100%)';
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 300);
        }
    }

    // Public API
    const toast = function(message, options = {}) {
        return showToast(message, 'default', options.duration || 4000);
    };

    toast.success = function(message, options = {}) {
        return showToast(message, 'success', options.duration || 4000);
    };

    toast.error = function(message, options = {}) {
        return showToast(message, 'error', options.duration || 4000);
    };

    toast.warning = function(message, options = {}) {
        return showToast(message, 'warning', options.duration || 4000);
    };

    toast.info = function(message, options = {}) {
        return showToast(message, 'info', options.duration || 4000);
    };

    // Expose to window
    window.sonner = {
        toast: toast,
        _removeToast: removeToast
    };

    // Initialize container on load
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initToastContainer);
    } else {
        initToastContainer();
    }
})();
