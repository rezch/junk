<?php

session_start();
session_unset();

function read_users_from_file() {
    $file_data = @file("passw.txt");
    $users = [];
    foreach ($file_data as $line) {
        $line = trim($line);
        list($username, $password, $keyword) = explode(':', $line);
        $users[$username] = [
            'password' => $password,
            'keyword' => $keyword];
    }
    return $users;
}

if (!isset($_GET['go'])){
    echo "<form>
        Login: <input type=text name=login>
        Password: <input type=password name=passwd>
        <input type=submit name=go value=Go>
        </form>";
}
else {
    $_SESSION['users'] = read_users_from_file();
    $_SESSION['login'] = $_GET['login'];
    $_SESSION['passwd'] = $_GET['passwd'];


    if (!isset($_SESSION['users'][$_GET['login']])) {
        echo "Неправильный логин или пароль";
        echo '<form action="authorize.php"><input type=submit value="Назад"></form>';
        exit;
    }
    if ($_SESSION['users'][$_GET['login']]['password'] === $_GET['passwd']) {
        Header("Location: secret_info.php");
        exit();
    }
    Header('Location: recovery.php');
    exit();
}
?>
