# Из списка размера 12 удалить все отрицательные элементы.

from random import randint

def f(l: list) -> list:
    return list(filter(
        lambda x: x >= 0,
        l))

l = [randint(-10, 10) for _ in range(12)]
print(l)
print(f(l))
