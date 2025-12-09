import pandas as pd

data = {'Иванов': 5, 'Петрова': 15, 'Сидоров': 12, 'Григорьев': 11,
        'Маштаков': 1, 'Каберникова': 8, 'Трапезникова': 10}
s = pd.Series(data)

print(s[s > 10])
print()

print(s[s == s.max()])
print()

print(s[s < s.mean()])

print('-----------------')

print('-----------------')
