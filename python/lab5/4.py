# Написать функцию season, принимающую 1 аргумент — номер месяца (от 1 до 12), и
# возвращающую время года, которому этот месяц принадлежит (зима, весна, лето или осень).


def season(month):
    # 12, 1, 2 - winter
    # 3, 4, 5 - spring
    # 6, 7, 8 - summer
    # 9, 10, 11 - autumn
    month = (month % 12) // 3
    seasons = [
        "winter",
        "spring",
        "summer",
        "autumn",
    ]
    return seasons[month]

print(season(1))
print(season(2))
print(season(3))
print(season(4))
print(season(5))
print(season(6))
print(season(7))
print(season(8))
print(season(9))
print(season(10))
print(season(11))
print(season(12))
