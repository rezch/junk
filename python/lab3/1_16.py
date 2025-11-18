# В каждой строке матрицы А размера n × m максимальный элемент поместить в конец строки,
# сохранив порядок остальных элементов.
from random import randint

def getM(n, m):
    return [[randint(-10, 10) for _ in range(n)] for _ in range(m)]

def printM(m):
    for l in m:
        print(l)

a = getM(4, 5)
printM(a)

for i, l in enumerate(a):
    mxi, mx = 0, l[0]
    for j, x in enumerate(l):
        if x > mx:
            mx = x
            mxi = j
    for j, x in enumerate(l):
        if j > mxi:
            l[j], l[j - 1] = l[j - 1], l[j]

print()
printM(a)
