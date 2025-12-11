import bcrypt
import social_pb2
import social_pb2_grpc
import grpc

from server.services.token import JwtToken

# Простая "база данных" в памяти
users = []
posts = []


class AuthService(social_pb2_grpc.AuthServiceServicer):
    def Register(self, request, context):
        # Проверяем, нет ли уже такого пользователя
        if any(u['username'] == request.username for u in users):
            context.set_code(grpc.StatusCode.ALREADY_EXISTS)
            context.set_details('User already exists')
            return social_pb2.RegisterResponse()

        # Хэшируем пароль
        hashed_password = bcrypt.hashpw(
            request.password.encode(), bcrypt.gensalt()).decode()

        # Сохраняем пользователя
        user_id = str(len(users) + 1)
        user = {
            'id': user_id,
            'username': request.username,
            'password': hashed_password
        }
        users.append(user)

        return social_pb2.RegisterResponse(
            user_id=user_id, username=request.username)

    def Login(self, request, context):
        # Находим пользователя
        user = next(
            (u for u in users if u['username'] == request.username), None)

        if not user or not bcrypt.checkpw(
                request.password.encode(), user['password'].encode()):
            context.set_code(grpc.StatusCode.UNAUTHENTICATED)
            context.set_details('Invalid username or password')
            return social_pb2.LoginResponse()

        token = JwtToken.createToken(user)
        return social_pb2.LoginResponse(token=token)

    def GetUserProfile(self, request, context):
        try:
            payload = JwtToken.decodeToken(request.token)
        except Exception:
            context.set_code(grpc.StatusCode.UNAUTHENTICATED)
            context.set_details('Invalid or expired token')
            return social_pb2.ProfileResponse()

        user_id = payload.get('user_id')
        username = payload.get('username')

        return social_pb2.ProfileResponse(
            user_id=user_id,
            username=username
        )
