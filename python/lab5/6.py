# Написать функцию is_prime, принимающую 1 аргумент — число от 0 до 1000, и возвращающую
# True, если оно простое, и False - иначе.

def is_prime(x):
    if x < 2:
        return False

    for i in range(2, int(x ** 0.5) + 1):
        if x % i == 0:
            return False

    return True


print(1, is_prime(1))
print(2, is_prime(2))
print(3, is_prime(3))
print(4, is_prime(4))
print(5, is_prime(5))
print(6, is_prime(6))
print(7, is_prime(7))
print(8, is_prime(8))
print(9, is_prime(9))
print(10, is_prime(10))
print(11, is_prime(11))
print(12, is_prime(12))
print(13, is_prime(13))
print(14, is_prime(14))
print(15, is_prime(15))
print(16, is_prime(16))
