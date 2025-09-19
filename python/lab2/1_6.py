# Вычислить длину вектора размера 5

from math import sqrt

def vec_len(vec: list) -> float:
    return sqrt(sum(
        map(lambda x: x * x, vec)))


l = [4, -2, 3.2, 5, 10]
print(vec_len(l))
