// Function to create a new customer
function createCustomer() {
    var customerName = document.getElementById("customerName").value;
    fetch('/create-customer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ customerName: customerName })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById("customerNameDisplay").innerText = data.customer.customerName;
                document.getElementById("message").innerText = "New customer created!";
            } else {
                document.getElementById("message").innerText = "Failed to create customer.";
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// Function to perform deposit
function deposit() {
    var amount = parseFloat(document.getElementById("amount").value);
    fetch('/deposit', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ amount: amount })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                var newBalance = data.account.balance.toFixed(2);
                document.getElementById("balance").innerText = newBalance;
                document.getElementById("message").innerText = "Deposit successful!";
            } else {
                document.getElementById("message").innerText = "Failed to perform deposit.";
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// Function to perform withdrawal
function withdraw() {
    var amount = parseFloat(document.getElementById("amount").value);
    fetch('/withdraw', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ amount: amount })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                var newBalance = data.account.balance.toFixed(2);
                document.getElementById("balance").innerText = newBalance;
                document.getElementById("message").innerText = "Withdrawal successful!";
            } else {
                document.getElementById("message").innerText = "Failed to perform withdrawal.";
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
