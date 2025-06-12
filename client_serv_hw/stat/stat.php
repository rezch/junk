<?php
require __DIR__ . '/vendor/autoload.php';
$dotenv = Dotenv\Dotenv::createImmutable(__DIR__);
$dotenv->load();

$DBHost     = $_ENV['DB_HOST'];
$DBUser     = $_ENV['DB_USER'];
$DBPassword = $_ENV['DB_PASSWORD'];
$DBName     = $_ENV['DB_NAME'];

// timestamp in us
$timestamp = (int)(new DateTime('now', new DateTimeZone('UTC')))->format('Uu');
$user_agent = $_SERVER['HTTP_USER_AGENT'];
$ipv4 = $_SERVER['REMOTE_ADDR'];
$host = gethostbyaddr($_SERVER['REMOTE_ADDR']);
$referer = $_SERVER['HTTP_REFERER'];
$datetime = date("Y-m-d H:i:s");
$uri = $_SERVER['REQUEST_URI'];

$DBLink = mysqli_connect($DBHost, $DBUser, $DBPassword, $DBName);
$Query = "
INSERT
    INTO STAT
VALUES (
    $timestamp,
    '$user_agent',
    INET_ATON('$ipv4'),
    '$host',
    '$referer',
    '$datetime',
    '$uri'
);
";

$Result = mysqli_query($DBLink, $Query);
mysqli_close($DBLink);
?>
