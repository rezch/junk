<?php
$sectors_count = rand(1, $MAX_SECTORS_COUNT);

$sectors_color = array_map(
    fn(): int => imagecolorallocate($image, rand(0, 255), rand(0, 255), rand(0, 255)),
    array_fill(0, $sectors_count, 0)
);

$sectors_weight = array_map(
    fn(): int => rand(1, $MAX_SECTORS_WEIGHT),
    array_fill(0, $sectors_count, 0)
);
?>
