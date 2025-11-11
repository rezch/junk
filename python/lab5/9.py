# Определить, сколькими способами можно отобрать команду в составе пяти человек из восьми
# кандидатов; из 10 кандидатов; из 11 кандидатов. Использовать функцию для подсчета
# количества способов отбора по формуле


def factorial(n):
    result = 1

    for i in range(2, n + 1):
        result *= i

    return result

def Cnk(n, k):
    return factorial(n) // (factorial(k) * factorial(n - k))

print(Cnk(8, 5))
print(Cnk(10, 5))
print(Cnk(11, 5))
