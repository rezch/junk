# Написать функцию XOR_cipher, принимающая 2 аргумента: строку, которую нужно
# зашифровать, и ключ шифрования, которая возвращает строку, зашифрованную путем
# применения функции XOR (^) над символами строки с ключом. Написать также
# функцию XOR_uncipher, которая по зашифрованной строке и ключу восстанавливает исходную
# строку.


def XOR_cipher(data, key):
    return ''.join(map(
        lambda c: chr(ord(c) ^ key),
        data))

def XOR_uncipher(data, key):
    return XOR_cipher(data, key)


x = XOR_cipher("abcdef", 1234)
print(x)
print(XOR_uncipher(x, 1234))
print()

x = XOR_cipher("xyz123yy", 52)
print(x)
print(XOR_uncipher(x, 52))
print(XOR_uncipher(x, 1234))
