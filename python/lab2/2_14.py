# Поменять местами максимальный и первый отрицательный элементы списка.


def f(l: list) -> list:
    mx = max(l)
    mxi = l.index(mx)

    for i, val in enumerate(l):
        if i != mxi and val < 0:
            l[i], l[mxi] = l[mxi], l[i]
            break

    return l


l = [4, -1, 2, 10, 5, -3, -10]
print(f(l))
