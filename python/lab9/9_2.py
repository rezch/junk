import pandas as pd

# Задание 1
data = {'Иванов': 5, 'Петрова': 15, 'Сидоров': 12, 'Григорьев': 11,
        'Маштаков': 1, 'Каберникова': 8, 'Трапезникова': 10}
s = pd.Series(data)

print(s[s > 10])
print()

print(s[s == s.max()])
print()

print(s[s < s.mean()])
print()
print('-----------------')

# Задание 2
df = pd.DataFrame({
    'Марка': ['Volkswagen', 'Volkswagen', 'Volkswagen', 'Lada', 'Lada', 'Lada', 'Mitsubishi', 'Toyota'],
    'Модель': ['Polo', 'Tiguan', 'Passat', 'Vesta', 'X-Ray', 'Kalina', 'Pajero', 'Camry'],
    'Пробег, км': [45800, 31200, 13100, 15700, 55400, 22000, 15000, 21900],
    'Год выпуска': [2018, 2018, 2020, 2021, 2019, 2022, 2022, 2019]
})

print(df[df['Год выпуска'] == 2019])
print()

print(df[df['Пробег, км'] > 30000])
print()

print(df[df['Пробег, км'] == df['Пробег, км'].min()])
print()
print('-----------------')

# Задание 3

df = pd.read_csv('Students_Performance_132b1e1ff9.csv')
print(df.groupby(['gender']).size())
print()

df['mean score'] = df[['math score', 'reading score', 'writing score']].mean(axis=1)
print(df.head())
