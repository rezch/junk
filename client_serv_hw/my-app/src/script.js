const mysql = require('mysql2');
const ipcRenderer = require('electron').ipcRenderer;


function parse_response(err, results, fields) {
    if (!!err) {
        let err_msg = "Ошибка во время запроса<br>Проверьте логин и пароль";
        document.getElementById('results').innerHTML = err_msg;
        return;
    }
    table = `
    <table>
        <thead><tr>
            <th>user_agent</th>
            <th>ipv4</th>
            <th>host</th>
            <th>referer</th>
            <th>visit_datetime</th>
            <th>uri</th>
        </tr></thead>
    <tbody>`;
    for (const item of results) {
        table += `
        <tr>
            <td>${item.user_agent}</td>
            <td>${item.ipv4}</td>
            <td>${item.host}</td>
            <td>${item.referer}</td>
            <td>${item.visit_datetime}</td>
            <td>${item.uri}</td>
        </tr>`
    }
    document.getElementById('results').innerHTML = table;
}


function request(event) {
    event.preventDefault()
    const username  = document.getElementById("username").value;
    const passwd    = document.getElementById("passwd").value;
    const date      = document.getElementById("dateid").value;
    const time      = document.getElementById("timeid").value;
    let datetime    = new Date(date + 'T' + time);
    const timestamp = datetime.setHours(datetime.getHours() + 3) + '000';

    const connection = mysql.createConnection({
        host     : "mc.cherry-berry.pro",
        user     : username,
        database : "stat",
        password : passwd
    });

    const query = `
    SELECT
        ts_id,
        user_agent,
        inet_ntoa(ipv4_net) as ipv4,
        host,
        referer,
        visit_datetime,
        uri
    FROM
        STAT
    WHERE
        ts_id >= ${timestamp};
    `;

    connection.query(
        query,
        parse_response);

    connection.end();
}
