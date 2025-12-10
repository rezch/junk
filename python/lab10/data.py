tasks = []
task_id_counter = 1

def add_task(title, description=None, status='pending'):
    global task_id_counter
    task = {
        'id': task_id_counter,
        'title': title,
        'description': description,
        'status': status
    }
    tasks.append(task)
    task_id_counter += 1
    return task

def get_all_tasks(status=None):
    if status:
        return [task for task in tasks if task['status'] == status]
    return tasks

def get_task_by_id(task_id):
    return next((task for task in tasks if task['id'] == task_id), None)

def delete_task(task_id):
    global tasks
    task = get_task_by_id(task_id)
    if task:
        tasks = [t for t in tasks if t['id'] != task_id]
        return True
    return False

def update_task(task_id, title=None, description=None, status=None):
    task = get_task_by_id(task_id)
    if not task:
        return None
    if title is not None:
        task['title'] = title
    if description is not None:
        task['description'] = description
    if status is not None:
        task['status'] = status
    return task

def get_status_stats():
    stats = {'pending': 0, 'done': 0}
    for t in tasks:
        if t['status'] in stats:
            stats[t['status']] += 1
    return stats
