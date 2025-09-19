# Заданный список преобразовать таким образом, чтобы все его элементы принадлежали отрезку [-1, 1].
# Предусмотреть возможность обратного преобразования.

from random import randint

def scale(l: list) -> tuple[int, list]:
    # для того чтобы все эл-ты x попали в отрезок [-1, 1]
    # можно нормализовать этот список
    max_abs = max(
            max(l),
            -min(l))
    return max_abs, list(map(
        lambda x: x / max_abs,
        l))

l = [randint(-10, 10) for _ in range(12)]
print(l)
size, result = scale(l)
print(size)
print(result)

# для восстановления списка в исходный размер можно просто умножить все его элементы
# на то число которое использовалось для нормализации
# то есть максимальный по абс. значению элемент
print(list(map(lambda x: x * size, result)))
