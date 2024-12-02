document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('loginForm');
    const resultDiv = document.getElementById('result');

    loginForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;

        try {
            const response = await fetch('https://localhost:7114/api/APIuser/auth', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                throw new Error(response.status);
            }

            const data = await response.json();
            if (data.token) {
                resultDiv.innerHTML = `<p class="text-success">Token: ${data.token}</p>`;
            } 
        } catch (error) {
            console.error(error);
            resultDiv.innerHTML = `<p class="text-danger">Login failed</p>`;
        }
    });
});
