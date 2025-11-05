# Вычислить сумму s, прекращая суммирование, когда очередной член суммы по
# абсолютной величине станет меньше 0,0001, при изменении аргумента x в указанном
# диапазоне [а, b] c шагом h. Для сравнения в каждой точке вычислить также функцию y =
# f(x), являющуюся аналитическим выражением ряда.

from math import factorial, exp

def s(x):
    min_prec = .0001
    i = 0
    S = 0
    while True:
        xi = ((2 * x) ** i) / factorial(i)
        if xi < min_prec:
            break
        S += xi
        i += 1
    return S

def f(x):
    return exp(2 * x)

a, b = 0.1, 1
h = 0.05

x = a
while x <= b:
    Sx = s(x)
    fx = f(x)
    diff = Sx - fx
    print(f'x={x:.1f}: S(x)={Sx:.4f}, f(x)={fx:.4f}, diff={diff:.6f}')
    x += h
