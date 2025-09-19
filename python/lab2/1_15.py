# Задан список х размера 10.
# Вычислить значения функции у = 0.5*lnx при значениях аргумента, заданных в списке х.
# Вычисленные значения поместить в список y. Вывести списокы х и y в виде двух столбцов.

from math import log
from random import randint

def f(x: list) -> list:
    return list(map(
        lambda x: 0.5 * log(x), x))

# x в интервале [1, 10]
x = [randint(1, 100) / 10 for _ in range(10)]
y = f(x)

for xi, yi in zip(x, y):
    print(xi, yi)
