// ElectroStore JavaScript

// Cart functionality
document.addEventListener('DOMContentLoaded', function() {
    // Update cart badge
    updateCartBadge();
    
    // Quantity controls
    const quantityInputs = document.querySelectorAll('.quantity-input');
    quantityInputs.forEach(input => {
        const minusBtn = input.previousElementSibling;
        const plusBtn = input.nextElementSibling;
        
        if (minusBtn && plusBtn) {
            minusBtn.addEventListener('click', () => {
                if (parseInt(input.value) > 1) {
                    input.value = parseInt(input.value) - 1;
                }
            });
            
            plusBtn.addEventListener('click', () => {
                input.value = parseInt(input.value) + 1;
            });
        }
    });
});

function updateCartBadge() {
    // This would typically fetch cart count from an API
    const cartBadge = document.getElementById('cartBadge');
    if (cartBadge) {
        // Placeholder - implement actual cart count logic
        const count = 0;
        if (count > 0) {
            cartBadge.textContent = count;
            cartBadge.style.display = 'block';
        } else {
            cartBadge.style.display = 'none';
        }
    }
}

// Form validation enhancement
function validateForm(form) {
    const inputs = form.querySelectorAll('input[required], textarea[required]');
    let isValid = true;
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });
    
    return isValid;
}

// Smooth scroll
function smoothScrollTo(element) {
    element.scrollIntoView({ behavior: 'smooth', block: 'start' });
}
