// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




const body = document.querySelector("body"),
    dashboard = body.querySelector(".dashboard"),
    toggle = body.querySelector(".toggle");

toggle.addEventListener("click", () => {
    dashboard.classList.toggle("close");
});


//--------------------
//Get Users

// Assuming the API is hosted at https://localhost:7005/api/User
const apiUrl = 'https://localhost:7005/api/User';

fetch(apiUrl)
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to fetch users');
        }
        return response.json();
    })
    .then(users => {
        displayUsersInTable(users);
    })
    .catch(error => {
        console.error('Error fetching users:', error);
        // Display an error message on the frontend
    });


function displayUsersInTable(users) {
    const tableBody = document.getElementById('userTableBody');

    users.forEach(user => {
        const row = tableBody.insertRow();
        row.insertCell(0).textContent = user.id;
        row.insertCell(1).textContent = user.userName;
        // Add more cells for additional user properties
    });
}


//--------------------
