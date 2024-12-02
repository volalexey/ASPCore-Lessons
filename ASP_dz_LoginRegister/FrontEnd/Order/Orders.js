document.getElementById("loadOrders").addEventListener("click", async function () {
    const ordersTableBody = document.getElementById("ordersTableBody");
    const errorMessage = document.getElementById("errorMessage");

    ordersTableBody.innerHTML = "";
    errorMessage.classList.add("d-none");
    try {
        const response = await fetch("https://localhost:7114/api/APIOrders");
        if (response.ok) {
            const orders = await response.json();

            if (orders.length > 0) {
                orders.forEach(order => {
                    const row = `
                        <tr>
                            <td>${order.id}</td>
                            <td>${order.count}</td>
                            <td>${order.userId}</td>
                            <td>${order.productId}</td>
                        </tr>
                    `;
                    ordersTableBody.insertAdjacentHTML("beforeend", row);
                });
            }
        }
    } catch (error) {
        errorMessage.textContent = error.message;
        errorMessage.classList.remove("d-none");
    }
});