<meta charset="utf-8" />
<body>
    <form action="diag.php">
        <input type=submit value="Создать">
    </form>
    <form action="diag.php" method="GET">
        <input type="hidden" name="read" value="true" />
        <input type=submit value="Считать">
    </form>
    <?php
    if ($_GET['read']) {
        echo '<img src="draw.php?read=true">';
    }
    else {
        echo '<img src="draw.php">';
    }
    ?>
</body>
