import pymysql
from decouple import config
from dotenv import load_dotenv

import random
import string


load_dotenv()
PASSWORD = config("PASSWORD", default="")

connection = pymysql.connect(host='localhost', user='root', password=PASSWORD, database='python_lab7')

with connection.cursor() as cursor:
    cursor.execute("TRUNCATE TABLE Audit;")
    cursor.execute("TRUNCATE TABLE AuditType;")
    cursor.execute("TRUNCATE TABLE Fond;")
connection.commit()

with connection.cursor() as cursor:
    # Заполнение типов аудиторий
    for i in range(1, 11):
        first_letter = random.choice(string.ascii_uppercase)
        digits = f"{random.randint(0, 999):03}"
        name = f"{first_letter}{digits}"

        query = """
            INSERT INTO AuditType (name)
            VALUES (%s)
        """

        cursor.execute(query, (
            name
            ))
    connection.commit()

    # Заполнение аудиторного фонда
    for i in range(1, 11):
        name = random.choice(string.ascii_uppercase)
        qa = 15

        query = """
            INSERT INTO Fond (name, qa)
            VALUES (%s, %s)
        """

        cursor.execute(query, (
            name, qa
            ))
    connection.commit()

    # Заполнение аудиторий
    for i in range(1, 151):
        typeV   = random.randint(1, 10)
        id_f    = (i - 1) % 10 + 1
        num     = random.randint(0, 10_000)
        comp    = random.randint(0, 40)
        video   = random.choice(['YES', 'NO'])
        vmest   = random.randint(0, 200)

        query = """
            INSERT INTO Audit (type, id_f, num, comp, video, vmest)
            VALUES (%s, %s, %s, %s, %s, %s)
        """

        cursor.execute(query, (
            typeV, id_f, num,
            comp, video, vmest
            ))
    connection.commit()


with connection.cursor() as cursor:
    # список всех корпусов, отсортированных по алфавиту
    cursor.execute("SELECT * FROM Fond ORDER BY name;")
    rows = cursor.fetchall()
    for row in rows:
        print(row)
    print('---------------')

    # список всех типов аудиторий, отсортированных по коду в обратном порядке
    cursor.execute("SELECT * FROM AuditType ORDER BY kod DESC;")
    rows = cursor.fetchall()
    for row in rows:
        print(row)
    print('---------------')

    # список всех аудиторий, отсортированных по номеру аудитории (первые пять)
    cursor.execute("SELECT * FROM Audit ORDER BY num LIMIT 5;")
    rows = cursor.fetchall()
    for row in rows:
        print(row)
    print('---------------')

    # Количество аудиторий
    cursor.execute("SELECT COUNT(*) FROM Audit;")
    count = cursor.fetchone()[0]
    print("Количество аудиторий:", count)

    # Уникальные вместимости
    cursor.execute("SELECT DISTINCT vmest FROM Audit ORDER BY vmest;")
    vmest_list = cursor.fetchall()
    print("Уникальные вместимости:", [v[0] for v in vmest_list])

    # Номер аудитории с минимальной вместимостью
    cursor.execute("SELECT num FROM Audit WHERE vmest = (SELECT MIN(vmest) FROM Audit);")
    min_vmest_list = cursor.fetchall()
    print("Аудитории с минимальной вместимостью:", [n[0] for n in min_vmest_list])

    # Информация по номеру аудитории
    required_num = int(input("Введите номер аудитории (num): "))
    cursor.execute("SELECT * FROM Audit WHERE num = %s;", (required_num,))
    row = cursor.fetchone()
    if row:
        print("Информация об аудитории с номером", required_num, ":", row)
    else:
        print("Аудитория с таким номером не найдена.")

connection.close()
