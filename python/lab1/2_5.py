# Определить частное и остаток от деления двух целых чисел N и M, используя
# операцию вычитания.


n = 123456
m = 4321

quotient = 0
remainder = n

while remainder >= m:
    remainder -= m
    quotient += 1

print(quotient, n // m)
print(remainder, n % m)
