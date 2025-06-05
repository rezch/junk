<?php
function validate_name($name) {
    $pattern = "/^[А-Я][\p{Cyrillic}]+$/u";
	return preg_match($pattern, $name);
}

function validate_phone($phone) {
    return str_starts_with($phone, "+7")
        || str_starts_with($phone, "8");
}

function validate_address($name) {
    $pattern = "/^[А-Я][\p{Cyrillic}\d\s]+$/u";
	return preg_match($pattern, $name);
}
?>
