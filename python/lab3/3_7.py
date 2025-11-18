# Все отрицательные элементы переставить в конец списка с сохранением порядка их
# следования.
from random import randint

def getL(n):
    return [randint(-10, 10) for _ in range(n)]

a = [3, -10, 6, -4, -6, -10, 7, -1, 12] # getL(7)
print(a)

n = len(a)
neg_cnt = 0
i = 0
while i < n - neg_cnt:
    if a[i] >= 0:
        i += 1
        continue
    for j in range(i + 1, n):
        a[j], a[j - 1] = a[j - 1], a[j]
    neg_cnt += 1

print(a)
