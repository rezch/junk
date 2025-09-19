# Элементы одномерного списка размера 7, большие среднего значения элементов списка, заменить на 0

def f(l: list) -> list:
    mean = sum(l) / len(l)
    return list(map(
        lambda x: 0 if x > mean else x,
        l))

l = [-2, 3, 1, 6, 3, 10, 11]
print(sum(l) / len(l))
print(f(l))
