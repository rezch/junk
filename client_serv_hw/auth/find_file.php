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
    $dir_iter = new RecursiveDirectoryIterator('./');
    $dir_iter = new RecursiveIteratorIterator(
        $dir_iter,
        RecursiveIteratorIterator::SELF_FIRST);
    foreach ($dir_iter as $entity) {
        if ($entity->isFile()) {
            if (check_file($entity->getPathname())) {
                return $entity->getPathname();
            }
        }
    }
    return false;
}
?>
