<?php
$sectors_count = 0;
$sectors_color = [];
$sectors_weight = [];

$file = fopen("colors.txt", "r");
if (!$file) { exit; }

while (($line = fgets($file)) !== false) {
    list($weight, $r, $g, $b) = explode(':', $line);
    array_push($sectors_weight, $weight);
    array_push($sectors_color, imagecolorallocate($image, $r, $g, $b));
    ++$sectors_count;
}
while (!fclose($file)) { }
?>
