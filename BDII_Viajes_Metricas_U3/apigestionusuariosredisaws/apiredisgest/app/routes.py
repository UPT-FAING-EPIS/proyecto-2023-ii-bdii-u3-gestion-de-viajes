
from flask import Flask, request, jsonify
import redis
import json
import boto3
from botocore.exceptions import ClientError
from app.models import User #
from app import utils #
#
from prometheus_client import Counter, Histogram, make_wsgi_app
from werkzeug.middleware.dispatcher import DispatcherMiddleware
from time import time

app = Flask(__name__)

#
# Metricas Prometheus
#REQUEST_COUNT = Counter('api_request_count', 'Total Request Count', ['method', 'endpoint', 'http_status'])
#REQUEST_LATENCY = Histogram('api_request_latency', 'Histogram for latency of requests', ['method', 'endpoint'])
REQUEST_COUNT = Counter(
    'api_http_responses', 
    'HTTP Responses',
    ['method', 'endpoint', 'status']
)
REQUEST_LATENCY = Histogram(
    'api_request_latency', 
    'Request latency',
    ['method', 'endpoint'],
    buckets=[0.001, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 1.0, 2.0, 5.0]
)

# Middleware para capturar el inicio de cada request
@app.before_request
def start_timer():
    if request.path != '/metrics':
        if request.path.startswith('/user') or request.path.startswith('/users'):    
            request.start_time = time()

@app.after_request
def record_request_data(response):
    if request.path != '/metrics':
        if request.path.startswith('/user') or request.path.startswith('/users'):
            request_latency = time() - request.start_time
            REQUEST_COUNT.labels(request.method, request.path, response.status_code).inc()
            REQUEST_LATENCY.labels(request.method, request.path).observe(request_latency)
    return response


#

def get_secret():
    secret_name = "clavebd"
    region_name = "us-east-2"

    session = boto3.session.Session()
    client = session.client(
        service_name='secretsmanager',
        region_name=region_name
    )

    try:
        get_secret_value_response = client.get_secret_value(SecretId=secret_name)
    except ClientError as e:
        raise e
    else:
        if 'SecretString' in get_secret_value_response:
            secret = json.loads(get_secret_value_response['SecretString'])
            return secret.get('clavebd') 
        else:
            
            raise Exception("Secreto en formato binario no soportado.")

    return None

redis_password = get_secret()

db = redis.Redis(host='redis', port=6379, db=0, password=redis_password, decode_responses=True)

@app.route('/', methods=['GET'])
def home():
    return "Bienvenido a la API de Gestión de Usuarios", 200

@app.route('/user', methods=['POST'])
def create_user():
    try:
        user_data = request.json
        if not user_data or 'id' not in user_data or 'username' not in user_data or 'password' not in user_data:
            return jsonify({"error": "Datos de usuario inválidos"}), 400

        hashed_password = utils.hash_password(user_data['password'])
        user = User(user_data['id'], user_data['username'], hashed_password)
        db.set(user.id, json.dumps(user.to_dict()))
        return jsonify(user.to_dict()), 201

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/user/<id>', methods=['GET'])
def get_user(id):
    try:
        user_data = db.get(id)
        if user_data:
            user_data = json.loads(user_data)
            return jsonify(user_data), 200
        return jsonify({"error": "Usuario no encontrado"}), 404

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/users', methods=['GET'])
def list_users():
    user_ids = db.keys('*')
    users = [json.loads(db.get(user_id)) for user_id in user_ids]
    return jsonify(users), 200


@app.route('/user/<id>', methods=['PUT'])
def update_user(id):
    try:
        user_data = request.json
        if not user_data or 'username' not in user_data or 'password' not in user_data:
            return jsonify({"error": "Datos de usuario inválidos"}), 400

        hashed_password = utils.hash_password(user_data['password'])
        user = User(id, user_data['username'], hashed_password)
        db.set(user.id, json.dumps(user.to_dict()))
        return jsonify(user.to_dict()), 200

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/user/<id>', methods=['DELETE'])
def delete_user(id):
    try:
        result = db.delete(id)
        if result:
            return jsonify({"success": "Usuario eliminado"}), 200
        else:
            return jsonify({"error": "Usuario no encontrado"}), 404

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/test', methods=['GET'])
def test():
    return "API funcionando correctamente", 200

#

# Crea una aplicacion WSGI que incluya las metricas de Prometheus
app_dispatch = DispatcherMiddleware(app, {
    '/metrics': make_wsgi_app()
})

#if __name__ == '__main__':
#    from werkzeug.serving import run_simple
#    run_simple(hostname="0.0.0.0", port=5000, application=app_dispatch)# , use_reloader=True, use_debugger=True


#

#if __name__ == '__main__':
#    app.run(host='0.0.0.0', port=5000, debug=True)
