# Определить индексы элементов списка, меньших среднего.
# Результат получить в виде списка.

def f(l: list) -> list:
    mean = sum(l) / len(l)
    return list(map(
        lambda x: x[0],
            filter(
                lambda x: x[1] > mean,
                enumerate(l))))

l = [4, 1, -1, 3, 8, 7, 10, 2]
print(l)
print(sum(l) / len(l))
print(f(l))
