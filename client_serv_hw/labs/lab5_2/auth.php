<?php
session_start();

if (!isset($_POST['go'])){
    echo "
    <form method='POST'>
        Login: <input type=text name=login>
        Password: <input type=password name=password>
        <input type=submit name=go value=Go>
    </form>";
}
else {
    include('check_user.php');
    if (!check_user($_POST['login'], $_POST['password'])) {
        echo "Неправильный логин или пароль";
        echo '<form action="auth.php"><input type=submit value="Назад"></form>';
        exit;
    }

    $_SESSION['authorized'] = true;
    Header("Location: edit.php");
    exit();
}
?>
