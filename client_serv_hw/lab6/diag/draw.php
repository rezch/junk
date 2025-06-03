<?php
$WIDTH = 800;
$HEIGHT = 800;

$DIAMETER = 600;
$X_CENTER = $WIDTH / 2;
$Y_CENTER = $HEIGHT / 2;

$MAX_SECTORS_COUNT = 10;
$MAX_SECTORS_WEIGHT = 100;

$image = imagecreatetruecolor($WIDTH, $HEIGHT);

$WHITE = imagecolorallocate($image, 255, 255, 255);
$BLACK = imagecolorallocate($image, 0, 0, 0);

imagefill($image, 0, 0, $WHITE);

include('config.php');

$weights_sum = array_sum($sectors_weight);
$sectors_weight = array_map(
    fn($weight): int => round(360 * $weight / $weights_sum),
    $sectors_weight
);

$diff = array_sum($sectors_weight) - 360;
for ($i = 0; $i < $sectors_count; ++$i) {
    if ($sectors_weight[$i] - $diff > 0) {
        $sectors_weight[$i] -= $diff;
        break;
    }
}

$curr_angle = 0;
for ($i = 0; $i < $sectors_count; ++$i) {
    // sector
    imagefilledarc(
        $image,
        /*center  =*/$X_CENTER, $Y_CENTER,
        /*size x/y=*/$DIAMETER, $DIAMETER,
        /*angle   =*/$curr_angle, $curr_angle + $sectors_weight[$i],
        /*color   =*/$sectors_color[$i],
        /*flags   =*/0);

    // label
    $label_x = ($DIAMETER / 2 + 30) * cos(deg2rad($curr_angle + $sectors_weight[$i] / 2)) + $X_CENTER;
    $label_y = ($DIAMETER / 2 + 30) * sin(deg2rad($curr_angle + $sectors_weight[$i] / 2)) + $Y_CENTER;
    $orig_weight = round($sectors_weight[$i] * $weights_sum / 360);
    imagestring($image, 50, $label_x, $label_y, $orig_weight, $BLACK);

    $curr_angle += $sectors_weight[$i];
}

header("Content-Type: image/png");
imagepng($image);
imagedestroy($image);
?>
