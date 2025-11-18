# Сформировать матрицу размера n X 3n, составленную из трех единичных квадратных матриц
# размера n X n.
def getE(n):
    return [
        [int(i == j) for j in range(n)]
        for i in range(n)
    ]

def printM(m):
    for l in m:
        print(l)

n = 4
e = getE(n)

e3 = []
for i in range(n):
    e3.append(e[i] * 3)

printM(e3)
