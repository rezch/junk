<?php
session_start();

if (!isset($_SESSION['authorized']) || !$_SESSION['authorized']) {
    echo "Для просмотра данной страницы необходимо авторизоваться";
    echo '<form action="auth.php"><input type=submit value="Назад"></form>';
    exit;
}

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
;";

$Result = mysqli_query($DBLink, $Query);
mysqli_close($DBLink);

if ($Result->num_rows === 0) {
    echo "Ничего не найдено.";
}

echo "
<table>
    <thead><tr>
        <th>id</th>
        <th>ФИО</th>
        <th>Телефон</th>
        <th>Адрес</th>
    </tr></thead>
<tbody>
";
while ($Rows = mysqli_fetch_array($Result, MYSQLI_ASSOC)) {
    printf("<tr> <td>%s</td>  <td>%s %s %s</td> <td>%s</td> <td>%s</td> </tr>",
        $Rows['id'],
        $Rows['surname'], $Rows['firstname'], $Rows['patronymic'],
        $Rows['phone'],
        $Rows['address']);
}
echo "</tbody></table><br>";

echo '<form action="change_db.php" method="POST">';
echo '<select name="choice">
    <option value="add" selected>Добавить</option>';
for ($i = 1; $i <= $Result->num_rows; ++$i) {
    echo "<option value='edit_$i'>Изменить $i</option>";
}
echo '</select><br>';

echo '<label>Фамилия:</label>
    <input type=text name="surname" placeholder="Иванов"><br>
    <label>Имя:</label>
    <input type=text name="name" placeholder="Иван"><br>
    <label>Отчество:</label>
    <input type=text name="patronymic" placeholder="Иванович"><br>
    <label>Телефон:</label>
    <input type=text name="phone" placeholder="+71234567890"><br>
    <label>Адрес:</label>
    <input type=text name="address" placeholder="Иваново"><br>
    <input type=submit value="Изменить"> <input type=reset value="Отменить">
</form>';

echo '<form action="index.html"><input type=submit value="Назад"></form>';
?>
