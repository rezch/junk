<?php
function check_file($path) {
    return array_all(
        @file($path, FILE_SKIP_EMPTY_LINES),
        fn($line): bool => count(
                array_filter(
                    explode(':', trim($line))
                )
            ) === 3);
}

function find_file() {
    $iterator = new RecursiveDirectoryIterator('./');
    $iterator = new RecursiveIteratorIterator(
        $iterator,
        RecursiveIteratorIterator::SELF_FIRST);
    foreach ($iterator as $fileInfo) {
        if ($fileInfo->isFile()) {
            if (check_file($fileInfo->getPathname())) {
                return $fileInfo->getPathname();
            }
        }
    }
    return false;
}
?>
