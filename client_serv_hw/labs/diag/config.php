<?php
if (isset($_GET['read'])) {
    include('load_config.php');
}
else {
    include('generate_config.php');
}
?>
