const API_URL = 'http://localhost:5084';

document.getElementById('name').addEventListener('input', function() {
    this.classList.remove('error');
});
document.getElementById('password').addEventListener('input', function() {
    this.classList.remove('error');
});

document.getElementById('loginForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    
    const name = document.getElementById('name').value;
    const password = document.getElementById('password').value;

    const response = await fetch(`${API_URL}/Auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: name, password: password })
    });

    if (!response.ok) {
    document.getElementById('name').classList.add('error');
    document.getElementById('password').classList.add('error');
    return;
}


    const data = await response.json();
localStorage.setItem('token', data.token);

const payload = JSON.parse(atob(data.token.split('.')[1]));
const role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

if (role === 'Admin') {
    window.location.href = 'admin.html';
} else {
    window.location.href = 'readings.html';
}
});