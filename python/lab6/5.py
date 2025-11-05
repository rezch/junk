# Написать функцию square, принимающую 1 аргумент — сторону квадрата, и
# возвращающую 3 значения (например, с помощью кортежа): периметр квадрата,
# площадь квадрата и диагональ квадрата. Использовать методы функционального
# программирования.


def square(x):
    sqrt2 = 1.41421356

    lamd = [
        lambda x: x * 4,
        lambda x: x * x,
        lambda x: x * sqrt2,
    ]

    return tuple(map(lambda l: l(x), lamd))


print(square(2.5))
print(square(5))
print(square(10))
