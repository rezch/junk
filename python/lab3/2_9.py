# В матрице А размера 6 × 7 расположить элементы строк в обратном порядке.
from random import randint

def getM(n, m):
    return [[randint(-10, 10) for _ in range(n)] for _ in range(m)]

def printM(m):
    for l in m:
        print(l)

a = getM(6, 7)
printM(a)

n = len(a[0])
for l in a:
    for i in range(len(l) // 2):
        l[i], l[n - i - 1] = l[n - i - 1], l[i]

print()
printM(a)
