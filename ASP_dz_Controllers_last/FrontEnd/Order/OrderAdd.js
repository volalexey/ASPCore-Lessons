document.getElementById("orderForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const userId = document.getElementById("userId").value;
    const productId = document.getElementById("productId").value;

    const responseElement = document.getElementById("result");
    responseElement.innerHTML = "";

    try {
        const response = await fetch("https://localhost:7114/api/APIOrders", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                UserId: userId,
                ProductId: parseInt(productId),
            }),
        });

        if (response.ok) {
            const data = await response.json();
            responseElement.innerHTML = `<div class="alert alert-success">Order created</div>`;
        }
    } catch (error) {
        responseElement.innerHTML = `<div class="alert alert-danger">${error.message}</div>`;
    }
});