from flask import Flask, jsonify, request
from data import (
    add_task, get_all_tasks, get_task_by_id,
    delete_task, update_task, get_status_stats
)

app = Flask(__name__)

@app.route('/tasks', methods=['GET'])
def list_tasks():
    status = request.args.get('status')
    result = get_all_tasks(status)
    return jsonify(result), 200

@app.route('/tasks', methods=['POST'])
def create_task():
    data = request.get_json()
    if not data or not data.get('title'):
        return jsonify({'error': 'Title is required'}), 400
    title = data['title']
    description = data.get('description')
    status = data.get('status', 'pending')
    if status not in ['pending', 'done']:
        return jsonify({'error': 'Invalid status'}), 400
    task = add_task(title, description, status)
    return jsonify(task), 201

@app.route('/tasks/<int:task_id>', methods=['GET'])
def get_task(task_id):
    task = get_task_by_id(task_id)
    if not task:
        return jsonify({'error': 'Task not found'}), 404
    return jsonify(task)

@app.route('/tasks/<int:task_id>', methods=['DELETE'])
def remove_task(task_id):
    result = delete_task(task_id)
    if not result:
        return jsonify({'error': 'Task not found'}), 404
    return '', 204

@app.route('/tasks/<int:task_id>', methods=['PUT'])
def modify_task(task_id):
    data = request.get_json()
    if not data:
        return jsonify({'error': 'No data provided'}), 400
    if not any(k in data for k in ('title', 'description', 'status')):
        return jsonify({'error': 'Nothing to update'}), 400
    title = data.get('title')
    description = data.get('description')
    status = data.get('status')
    if status and status not in ['pending', 'done']:
        return jsonify({'error': 'Invalid status'}), 400
    task = update_task(task_id, title, description, status)
    if not task:
        return jsonify({'error': 'Task not found'}), 404
    return jsonify(task)

@app.route('/tasks/stats', methods=['GET'])
def stats():
    return jsonify(get_status_stats())
