# Написать функцию is_year_leap, принимающую 1 аргумент — год, и
# возвращающую True, если год високосный, и False иначе. Использовать методы
# функционального программирования.


def is_year_leap(year):
    conditions = [
        lambda x: x % 4 == 0,
        lambda x: x % 100 != 0 or x % 400 == 0
    ]

    return all(cond(year) for cond in conditions)


print(is_year_leap(2020)) # True
print(is_year_leap(2000)) # True
print(is_year_leap(1900)) # False
print(is_year_leap(2023)) # False
print(is_year_leap(2024)) # True
