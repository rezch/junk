<?php
require __DIR__ . '/vendor/autoload.php';
$dotenv = Dotenv\Dotenv::createImmutable(__DIR__);
$dotenv->load();

$DBHost     = $_ENV['DB_HOST'];
$DBUser     = $_ENV['DB_USER'];
$DBPassword = $_ENV['DB_PASSWORD'];
$DBName     = $_ENV['DB_NAME'];

$DBLink = mysqli_connect($DBHost, $DBUser, $DBPassword, $DBName);

$Query =
"SELECT *
FROM BOOK
WHERE
    surname LIKE '%".$_POST['surname']."%'
    AND phone LIKE '%".$_POST['phone']."%'
LIMIT 30
;";

$Result = mysqli_query($DBLink, $Query);

if ($Result->num_rows === 0) {
    echo "Ничего не найдено.";
}

echo "
<table>
    <thead><tr>
        <th>ФИО</th>
        <th>Телефон</th>
        <th>Адрес</th>
    </tr></thead>
<tbody>
";
while ($Rows = mysqli_fetch_array($Result, MYSQLI_ASSOC)) {
    printf("<tr> <td>%s %s %s</td> <td>%s</td> <td>%s</td> </tr>",
        $Rows['surname'], $Rows['firstname'], $Rows['patronymic'],
        $Rows['phone'],
        $Rows['address']);
}
echo "</tbody></table>";

mysqli_close($DBLink);
?>
