const API_URL = 'http://localhost:5084';
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
    console.log('Ошибка входа');
    return;
    }

    const data = await response.json();
    localStorage.setItem('token', data.token);
    console.log(data);
    
    window.location.href = 'readings.html'
});