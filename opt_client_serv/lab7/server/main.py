import grpc
from concurrent import futures
import social_pb2_grpc
from server.services.auth_service import AuthService
from server.services.wall_service import WallService

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    social_pb2_grpc.add_AuthServiceServicer_to_server(AuthService(), server)
    social_pb2_grpc.add_WallServiceServicer_to_server(WallService(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    print("Сервер запущен на порту 50051")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()
