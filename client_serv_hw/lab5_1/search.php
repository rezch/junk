<?php

// DB vars
$DBHost     = "";
$DBUser     = "";
$DBPassword = "";
$DBName     = "";

$DBLink = mysqli_connect($DBHost, $DBUser, $DBPassword, $DBName);

$phone = trim($_POST["phone"]);

$Query = "SELECT
	full_name,
    address
FROM `contacts`
WHERE `phone` = '$phone';";

$Result = mysqli_query($DBLink, $Query);

if ($Result->num_rows === 0) {
    echo "Ничего не найдено.";
}

while ($Rows = mysqli_fetch_array($Result, MYSQLI_ASSOC))
{
    printf("ФИО:%s, Адрес:%s<br>",
        $Rows['full_name'], $Rows['address']);
}

mysqli_close($DBLink);
?>
