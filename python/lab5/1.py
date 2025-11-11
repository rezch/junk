# Написать функцию arithmetic, принимающую 3 аргумента: первые 2 - числа,
# третий - операция, которая должна быть произведена над ними. Если третий
# аргумент +, сложить их; если —, то вычесть; * — умножить; / — разделить (первое на
# второе). В остальных случаях вернуть строку "Неизвестная операция".


def arithmetic(a, b, op):
    operations = {
        '+': lambda x, y: x + y,
        '-': lambda x, y: x - y,
        '*': lambda x, y: x * y,
        '/': lambda x, y: x / y,
    }

    if op not in operations.keys():
        return "Неизвестная операция"

    return operations[op](a, b)


print(arithmetic(5, 3, '+'))
print(arithmetic(5, 3, '-'))
print(arithmetic(5, 3, '*'))
print(arithmetic(6, 3, '/'))
print(arithmetic(5, 3, '^'))
