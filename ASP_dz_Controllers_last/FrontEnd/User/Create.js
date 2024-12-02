document.addEventListener('DOMContentLoaded', () => {
    const registerForm = document.getElementById('registerForm');
    const resultDiv = document.getElementById('result');

    registerForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;

        try {
            const response = await fetch('https://localhost:7114/api/APIuser/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            const contentType = response.headers.get('content-type');

            if (response.ok) {
                if (contentType && contentType.includes('application/json')) {
                    const data = await response.json();
                    resultDiv.innerHTML = `<p class="text-info">${data.description}</p>`;
                } 
            }
            else {
                const text = await response.text();
                resultDiv.innerHTML = `<p class="text-success">${text}</p>`;
            }
        } catch (error) {
            console.error(error);
            resultDiv.innerHTML = `<p class="text-danger">Registration failed</p>`;
        }
    });
});
