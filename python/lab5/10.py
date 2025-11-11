# Два треугольника заданы длинами своих сторон a, b и с. Определить при помощи треугольник с
# большей площадью, вычисляя площади треугольников по формуле Герона
from math import sqrt


def p(a, b, c):
    return (a + b + c) / 2

def S(a, b, c):
    px = p(a, b, c)
    return sqrt(
        px * (px - a) * (px - b) * (px - c)
    )

def comp(a1, b1, c1, a2, b2, c2):
    print(S(a1, b1, c1), S(a2, b2, c2))
    return 1 if S(a1, b1, c1) > S(a2, b2, c2) else 2

print(comp(8, 13, 9, 14, 17, 5))
print(comp(8, 2, 9, 14, 17, 5))
