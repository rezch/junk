<?php
require __DIR__ . '/vendor/autoload.php';
$dotenv = Dotenv\Dotenv::createImmutable(__DIR__);
$dotenv->load();

function check_user($username, $password) {
    $DBHost     = $_ENV['DB_HOST'];
    $DBUser     = $_ENV['DB_USER'];
    $DBPassword = $_ENV['DB_PASSWORD'];
    $DBName     = $_ENV['DB_NAME'];

    $DBLink = mysqli_connect($DBHost, $DBUser, $DBPassword, $DBName);

    $Query = "
    SELECT *
    FROM ADMINS
    WHERE
        username = '$username'
        AND password_hash = UNHEX(SHA2('$password', 256))
    ;";
    $Result = mysqli_query($DBLink, $Query);
    mysqli_close($DBLink);
    return $Result->num_rows > 0; // В бд есть админ с такими именем и паролем
}
?>
