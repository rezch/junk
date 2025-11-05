# Вкладчик положил в банк 10 000 рублей под 8 % в месяц. Определить, через какое
# время сумма удвоится

start_money = 10_000
rate = 1.08

time = 0 # month
money = start_money
while money < start_money * 2:
    money *= rate
    time += 1

print(f'{time} - месяцев')
