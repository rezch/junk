<?php
require __DIR__ . '/vendor/autoload.php';
$dotenv = Dotenv\Dotenv::createImmutable(__DIR__);
$dotenv->load();

$DBHost     = $_ENV['DB_HOST'];
$DBUser     = $_ENV['DB_USER'];
$DBPassword = $_ENV['DB_PASSWORD'];
$DBName     = $_ENV['DB_NAME'];

$action     = $_POST['choice'];
$surname    = $_POST['surname'];
$name       = $_POST['name'];
$patronymic = $_POST['patronymic'];
$phone      = $_POST['phone'];
$address    = $_POST['address'];

include('validation.php');

function close() {
    echo '<form action="edit.php"><input type=submit value="Назад"></form>';
    exit();
}

foreach ([ $surname, $name, $patronymic ] as $var) {
    if ($var != "" && !validate_name($var)) {
        echo "Не правильно указано ФИО";
        close();
    }
}

if ($phone != "" && !validate_phone($phone)) {
    echo "Не правильно указан номер телефона";
    close();
}

if ($address != "" && !validate_address($address)) {
    echo "Не правильно указан адрес";
    close();
}

$DBLink = mysqli_connect($DBHost, $DBUser, $DBPassword, $DBName);

$user_vars = [ $surname, $name, $patronymic, $phone, $address ];

$var_names = [ "surname", "firstname", "patronymic", "phone", "address" ];
if ($action === "add") {
    $var_names = implode(", ", $var_names);

    $Query = "
    INSERT INTO BOOK
        ($var_names)
    VALUES
        ('$surname', '$name', '$patronymic', '$phone', '$address');
    ";
}
else {
    $user_vars =
        implode(",\n",
        array_filter(
            array_map(
                fn($a, $b): string => $b != "" ? "BOOK.$a = '$b'" : "",
                $var_names, $user_vars)));

    $id = explode('edit_', $action)[1];
    $Query = "
    UPDATE
        BOOK
    SET
        $user_vars
    WHERE
        BOOK.id = $id;
    ;";
}

if (mysqli_query($DBLink, $Query)) {
    echo "Успешно";
}
else {
    echo "Ошибка во время операции. Попробуйте снова.";
}
mysqli_close($DBLink);

echo '<form action="edit.php"><input type=submit value="Назад"></form>';
?>
