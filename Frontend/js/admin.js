const API_URL = 'http://159.194.205.124:8080';
const token = localStorage.getItem('token');

if (!token) {
    window.location.href = 'index.html';
}

// Названия месяцев на русском
const months = ['январь','февраль','март','апрель','май','июнь',
                 'июль','август','сентябрь','октябрь','ноябрь','декабрь'];

function getMeterName(meterType, tariff) {
    const types = { 0: 'Общее', 1: 'Вода горячая', 2: 'Вода холодная' };
    const tariffs = { 0: 'T1', 1: 'T2', 2: 'T3' };
    const type = types[meterType] || 'Счётчик';
    const t = tariff !== null ? ` (${tariffs[tariff]})` : '';
    return type + t;
}

async function loadTable() {
    const response = await fetch(`${API_URL}/Readings/latest`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (!response.ok) {
        window.location.href = 'index.html';
        return;
    }

    const readings = await response.json();

    // Определяем актуальный месяц из данных
    if (readings.length > 0) {
        const date = new Date(readings[0].readingDate);
        document.getElementById('monthTitle').textContent = 
            `Показания за ${months[date.getMonth()]} ${date.getFullYear()}`;
    }

    // Группируем по помещению
    const byPremise = {};
    readings.forEach(r => {
        const premiseId = r.meter.premiseId;
        if (!byPremise[premiseId]) {
            byPremise[premiseId] = {
                premise: r.meter.premise,
                submittedBy: r.submittedBy,
                date: r.readingDate,
                readings: []
            };
        }
        byPremise[premiseId].readings.push(r);
    });

    const tbody = document.getElementById('tableBody');
    tbody.innerHTML = '';

    Object.values(byPremise).forEach(group => {
        const hotWater = group.readings.find(r => r.meter.meterType === 1);
        const coldWater = group.readings.find(r => r.meter.meterType === 2);
        const electricity = group.readings.filter(r => r.meter.meterType === 0);

        const elText = electricity.map(r => 
            `${getMeterName(r.meter.meterType, r.meter.tariff)}: ${r.value}`
        ).join('<br>');

        const date = new Date(group.date);
        const dateStr = `${date.getDate().toString().padStart(2,'0')}.${(date.getMonth()+1).toString().padStart(2,'0')}`;

        const row = document.createElement('tr');
        row.innerHTML = `
                <td>
                    <strong>${group.premise.tenantName || 'свободно'}</strong><br>
                    <small>${group.premise.address}</small>
                </td>
                <td>${hotWater?.value ?? '—'}</td>
                <td>${coldWater?.value ?? '—'}</td>
                <td>${elText || '—'}</td>
                <td>${dateStr}</td>
                <td>${group.submittedBy?.name || '—'}</td>
            `;
        tbody.appendChild(row);
    });
}


loadTable();