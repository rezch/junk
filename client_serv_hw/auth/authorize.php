<?php

session_start();
session_unset();

include('find_file.php');
$_SESSION['file_path'] = find_file();
if (!$_SESSION['file_path']) {
    echo "Не удалось найти файл с информацией о паролях.";
    exit;
}

function read_users_from_file() {
    $file_data = @file($_SESSION['file_path'], FILE_SKIP_EMPTY_LINES);
    $users = [];
    foreach ($file_data as $line) {
        list($username, $password, $keyword) = explode(':', trim($line));
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
