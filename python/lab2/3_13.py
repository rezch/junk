# Из списка удалить повторяющиеся элементы.

def f(l: list) -> list:
    result = []

    for x in l:
        if x in result:
            continue
        result.append(x)

    return result

l = [4, 1, 10, 5, 1, 8, 4, 9, 5]
print(l)
print(f(l))
