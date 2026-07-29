const API_URL = 'http://localhost:5084';
const token = localStorage.getItem('token');

if (!token) {
    window.location.href = 'html/index.html';
}
let toastTimer;
function showToast(message, type = 'success') {
    const toast = document.getElementById('toast');
    document.getElementById('toastMessage').textContent = message;
    document.getElementById('toastIcon').textContent = type === 'error' ? '✕' : '✓';
    toast.classList.toggle('toast-error', type === 'error');
    toast.classList.add('show');
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => toast.classList.remove('show'), 2200);
}

function showScreen(id) {
    document.getElementById('screenPremises').classList.add('hidden');
    document.getElementById('screenMeters').classList.add('hidden');
    document.getElementById('screenForm').classList.add('hidden');
    document.getElementById(id).classList.remove('hidden');
}
async function loadPremises() {
    const response = await fetch(`${API_URL}/Premises`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        console.log('Ошибка загрузки помещений');
        return;
    }

    const premises = await response.json();
    const list = document.getElementById('premisesList');
    if (list.children.length > 0) return;

premises.forEach(premise => {
    const row = document.createElement('div');
    row.className = 'row';
    row.innerHTML = `
        <div class="row-title">${premise.tenantName || 'свободно'}</div>
        <div class="row-sub">${premise.address }</div>
    `;
    row.addEventListener('click', () => {
    loadMeters(premise.id, premise.tenantName || premise.address);
    });
    list.appendChild(row);
});
}

loadPremises();
async function loadMeters(premiseId, address) {
    console.log('loadMeters вызван:', premiseId);
    showScreen('screenMeters');
    document.getElementById('premiseTitle').textContent = address;

    const response = await fetch(`${API_URL}/Meters/by-premise?premiseId=${premiseId}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });

    const meters = await response.json();
    console.log('Количество счётчиков:', meters.length);
    const list = document.getElementById('metersList');
    list.innerHTML = '';

    meters.forEach(meter => {
        const row = document.createElement('div');
        row.className = 'row';
        row.innerHTML = `
            <div class="row-title">${getMeterName(meter)}</div>
        `;
        row.addEventListener('click', () => openForm(meter));
        list.appendChild(row);
    });
}

function getMeterName(meter) {
    const types = { 0: 'Электричество', 1: 'Вода горячая', 2: 'Вода холодная' };
    const tariffs = { 0: 'T1', 1: 'T2', 2: 'T3' };
    const type = types[meter.meterType] || 'Счётчик';
    const tariff = meter.tariff !== null ? ` (${tariffs[meter.tariff]})` : '';
    return type + tariff;
}
document.getElementById('backToPremises').addEventListener('click', () => {
    showScreen('screenPremises');
});
document.getElementById('photo').addEventListener('change', function() {
    const preview = document.getElementById('photoPreview');
    const file = this.files[0];
    if (!file) {
        preview.style.backgroundImage = '';
        preview.textContent = 'превью фото';
        return;
    }
    const reader = new FileReader();
    reader.onload = e => {
        preview.style.backgroundImage = `url(${e.target.result})`;
        preview.textContent = '';
    };
    reader.readAsDataURL(file);
});

async function openForm(meter) {
    showScreen('screenForm');
    document.getElementById('meterTitle').textContent = getMeterName(meter);
    document.getElementById('value').value = '';
    document.getElementById('photo').value = '';
    document.getElementById('photoPreview').style.backgroundImage = '';
    document.getElementById('photoPreview').textContent = 'превью фото';
    document.getElementById('submitReading').dataset.meterId = meter.id;

    // Загружаем последнее показание
    const response = await fetch(`${API_URL}/Readings/last-by-meter/${meter.id}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const last = await response.json();
        document.getElementById('lastValue').textContent = `Прошлое: ${last.value}`;
    } else {
        document.getElementById('lastValue').textContent = 'Первое показание';
    }
}

document.getElementById('backToMeters').addEventListener('click', () => {
    showScreen('screenMeters');
});
document.getElementById('submitReading').addEventListener('click', async function() {
    const meterId = parseInt(this.dataset.meterId);
    const value = parseFloat(document.getElementById('value').value);
    const photo = document.getElementById('photo').files[0];

    if (!value) {
        showToast('Введите показание', 'error');
        return;
    }

    // Шаг 1: создать показание
    const response = await fetch(`${API_URL}/Readings`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ meterId, value, photoUrl: '' })
    });

    if (!response.ok) {
        const error = await response.text();
        showToast(error, 'error');
        return;
    }

    const reading = await response.json();

    // Шаг 2: загрузить фото если выбрано
    if (photo) {
        const formData = new FormData();
        formData.append('file', photo);

        await fetch(`${API_URL}/Readings/${reading.id}/photo`, {
            method: 'POST',
            headers: { 'Authorization': `Bearer ${token}` },
            body: formData
        });
    }

    showToast('Показание сохранено');
    showScreen('screenMeters');
});