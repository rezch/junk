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
    $bars[] = rand(10, 100);
}

$max_size = max($bars);
$bar_width = floor(($bar_canvas_width - 100) / (2 * $bars_count - 1));

for ($i = 0; $i < $bars_count; $i++) {
    $barColor = imagecolorallocate(
        $image,
        rand(0, 255),
        rand(0, 255),
        rand(0, 255));

    $scaled_height = round($bars[$i] / $max_size * $bar_canvas_height);
    $x1 = (2 * $i) * $bar_width + 100;
    $y1 = $height - $scaled_height;
    $x2 = $x1 + $bar_width;
    $y2 = $height - 50;

    imagefilledrectangle($image, $x1, $y1, $x2, $y2, $barColor);
    imagestring($image, 50, $x1 + round($bar_width / 4), $y1 - 20, $bars[$i], $black);
}

imageline($image, $bar_canvas_width, $height - 50, 50, $height - 50, $black);
imageline($image, 100, $height - $bar_canvas_height, 100, $height - 20, $black);

imagestring($image, 50, 50, $height - 70, 0, $black);
imagestring($image, 50, 50, $height - $bar_canvas_height, 100, $black);

header("Content-Type: image/png");
imagepng($image);
imagedestroy($image);
?>
