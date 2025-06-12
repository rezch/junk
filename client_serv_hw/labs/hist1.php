<?php
$width = 1500;
$height = 800;
$image = imagecreatetruecolor($width, $height);

$white = imagecolorallocate($image, 255, 255, 255);
$black = imagecolorallocate($image, 0, 0, 0);
$bar_canvas_width = 1200;
$bar_canvas_height = 700;
$bars_count = rand(2, 10);

imagefill($image, 0, 0, $white);

$bars = [];
for ($i = 0; $i < $bars_count; $i++) {
    $bars[] = rand(-100, 100);
}

$max_size = max(
    abs(max($bars)),
    abs(min($bars)));
$bar_width = floor(($bar_canvas_width - 100) / (2 * $bars_count - 1));

for ($i = 0; $i < $bars_count; $i++) {
    $barColor = imagecolorallocate(
        $image,
        rand(0, 255),
        rand(0, 255),
        rand(0, 255));

    $scaled_height = round(($bars[$i]) / $max_size * $bar_canvas_height);

    $x1 = (2 * $i) * $bar_width + 100;
    $y1 = round($height / 2) - round($scaled_height / 2);

    $x2 = $x1 + $bar_width;
    $y2 = round($height / 2);

    imagefilledrectangle($image, $x1, $y1, $x2, $y2, $barColor);
    $label_y_diff = ($bars[$i] > 0) ? 20 : -20;
    imagestring($image, 50, $x1 + round($bar_width / 4), $y1 + $label_y_diff, $bars[$i], $black);
}

imageline($image, $bar_canvas_width, round($height / 2), 50, round($height / 2), $black);
imageline($image,
    100, $height - round($height / 2) - round($bar_canvas_height / 2),
    100, $height - round($height / 2) + round($bar_canvas_height / 2),
    $black);

imagestring($image, 50, 50, round($height / 2), 0, $black);
imagestring($image, 50,
    50, $height - round($height / 2) - round($bar_canvas_height / 2),
    100, $black);
imagestring($image, 50,
    50, $height - 50,
    -100, $black);

header("Content-Type: image/png");
imagepng($image);
imagedestroy($image);
?>
