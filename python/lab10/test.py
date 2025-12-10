import requests

base_url = 'http://localhost:5000/tasks'

# Добавление задачи
task = {"title": "Test Task"}
resp = requests.post(base_url, json=task)
print("POST:", resp.json())

# Получение задач
resp = requests.get(base_url)
print("GET:", resp.json())
