# Преобразовать матрицу А размера 5 × 7, умножив максимальный элемент каждой строки на
# номер этой строки
from random import randint

def getM(n, m):
    return [[randint(-10, 10) for _ in range(n)] for _ in range(m)]

def printM(m):
    for l in m:
        print(l)

a = getM(5, 7)

printM(a)

for i, l in enumerate(a):
    mx = l[0]
    for x in l:
        mx = max(mx, x)
    for j, x in enumerate(l):
        if x == mx:
            l[j] *= (i + 1)

print()
printM(a)
