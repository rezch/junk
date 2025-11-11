# Написать функцию is_year_leap, принимающую 1 аргумент — год, и
# возвращающую True, если год високосный, и False иначе. Использовать методы
# функционального программирования.


def is_year_leap(year):
    return (year % 4 == 0) and (year % 100 != 0 or year % 400 == 0)


print(is_year_leap(2020)) # True
print(is_year_leap(2000)) # True
print(is_year_leap(1900)) # False
print(is_year_leap(2023)) # False
print(is_year_leap(2024)) # True
