<?php

function check($file_data) {
    return array_all(
        explode("\n", $file_data),
        fn($line): bool => count(
                array_filter(
                    explode(':', trim($line))
                )
            ) === 3);
}

echo check('some:var:bar
    some:var:bar
    some:var:bar') . "\n";

echo !check('some:var:bar
    some::bar') . "\n";

echo !check('some:bar') . "\n";
echo !check('asd asd asd') . "\n";

?>
