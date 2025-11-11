# Написать функцию date, принимающую 3 аргумента — день, месяц и год. Вернуть True, если
# такая дата есть в нашем календаре, и False иначе.


def date(day, month, year):
    if month < 1 or month > 12:
        return False

    # См. задание 2
    def is_year_leap(year):
        return (year % 4 == 0) and (year % 100 != 0 or year % 400 == 0)

    days_in_month = [
        31,
        29 if is_year_leap(year) else 28,
        31, 30, 31, 30, 31,
        31, 30, 31, 30, 31
    ]

    max_day = days_in_month[month - 1]

    return day >= 1 and day <= max_day


print(date(29, 2, 2024))  # True
print(date(29, 2, 2023))  # False
print(date(31, 4, 2022))  # False
print(date(30, 4, 2022))  # True
print(date(1, 1, 1))      # True
print(date(31, 12, 9999)) # True
