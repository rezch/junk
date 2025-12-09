import numpy as np
import matplotlib.pyplot as plt

a, b, c = -2, 4.3, 5


def f1(rng):
    return a * rng**2 + b * rng + c


def f2(rng):
    return a * np.sin(b * rng) + c


def f3(rng):
    return (a / (b * rng)) + c**2


def graph1(x, y):
    plt.plot(x, y, c='g')
    plt.show()


def graph2(x, y):
    plt.scatter(x, y, marker='x', c='r')
    plt.show()


def graph3(x, y):
    plt.bar(x, y)
    plt.show()


rng = np.linspace(-10, 10, 100)
g = int(input('Введите номер графика (1-3): '))
f = int(input('Введите номер функции (1-3): '))

func = None
if f == 1:
    func = f1
elif f == 2:
    func = f2
elif f == 3:
    func = f3

graph = None
if g == 1:
    graph = graph1
elif g == 2:
    graph = graph2
elif g == 3:
    graph = graph3

graph(rng, func(rng))
