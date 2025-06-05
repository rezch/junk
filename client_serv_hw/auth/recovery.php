<?php
session_start();


if (!isset($_SESSION['users']) || !isset($_SESSION['users'][$_SESSION['login']])) {
    Header("Location: authorize.php", true, 301);
    exit();
}


function internal_error() {
    echo "Возникла проблема, при попытке сохранить пароль. Попробуйте снова.";
    exit();
}


function update_file() {
    $file = @fopen("passw.txt", "w");
    if (!$file) {
        internal_error();
    }

    foreach ($_SESSION['users'] as $name => $user) {
       if (!fwrite($file, $name . ":" . $user['password'] . ":" . $user['keyword'] . "\n")) {
            internal_error();
        }
    }

    while (!fclose($file)) { }
}


if (!isset($_GET['go_key'])){
    echo "<form>
        Key phrase: <input type=text name=key>
        New password: <input type=password name=new_passwd>
        <input type=submit name=go_key value=Go>
        </form>";
}
else {
    $key = trim($_GET['key']);
    if ($_SESSION['users'][$_SESSION['login']]['keyword'] === $key) {
        $_SESSION['users'][$_SESSION['login']]['password'] = $_GET['new_passwd'];
        update_file();
        echo "Пароль успешно изменен.";
    }
    else {
        echo "Неверная ключевая фраза.";
    }
    echo '<form action="authorize.php"><input type=submit value="Назад"></form>';
    exit;
}
?>
