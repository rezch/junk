<?php session_start();

if (!isset($_SESSION['users'])
    || !isset($_SESSION['users'][$_SESSION['login']])
    || $_SESSION['users'][$_SESSION['login']]['password'] != $_SESSION['passwd'] ) {
    Header("Location: authorize.php");
    exit();
}
?>

<html>
<head>
    <title>Secret info</title>
</head>
    <body>
        <p>Секретная страница<hr>
        <a href="authorize.php">На главную</a>
    </body>
</html>
