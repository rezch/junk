# Удалить строку матрицы А размера 5 × 7, содержащую минимальный элемент в 1-м столбце.
from random import randint

def getM(n, m):
    return [[randint(-10, 10) for _ in range(n)] for _ in range(m)]

def printM(m):
    for l in m:
        print(l)

a = getM(5, 7)
printM(a)

for i, l in enumerate(a):
    mn = l[0]
    for x in l:
        mn = min(mn, x)
    if l[0] == mn:
        a.pop(i)

print()
printM(a)
