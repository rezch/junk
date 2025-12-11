import grpc
import social_pb2
import social_pb2_grpc

def run():
    channel = grpc.insecure_channel('localhost:50051')

    try:
        # Тестируем подключение
        auth_stub = social_pb2_grpc.AuthServiceStub(channel)

        # 1. Регистрируем пользователя
        print("Регистрируем пользователя...")
        register_response = auth_stub.Register(social_pb2.RegisterRequest(
            username="test_user",
            password="test_password"
        ))
        print(f"Зарегистрирован: {register_response.username}")

        # 2. Логинимся
        print("\nЛогинимся...")
        login_response = auth_stub.Login(social_pb2.LoginRequest(
            username="test_user",
            password="test_password"
        ))
        print(f"Получен токен: {login_response.token}")

        # Профиль
        print("\nПолучаем профиль пользователя через GetUserProfile ...")
        profile_response = auth_stub.GetUserProfile(social_pb2.ProfileRequest(
            token=login_response.token
        ))
        print(f"ID пользователя: {profile_response.user_id}, username: {profile_response.username}")

        # 3. Создаем пост
        print("\nСоздаем пост...")
        wall_stub = social_pb2_grpc.WallServiceStub(channel)
        metadata = [('authorization', f'Bearer {login_response.token}')]
        post_response = wall_stub.CreatePost(social_pb2.CreatePostRequest(
            text="Мой первый пост через gRPC!"
        ), metadata=metadata)
        print(f"Создан пост: {post_response.post.text}")

        # 4. Получаем все посты
        print("\nПолучаем все посты...")
        posts_response = wall_stub.GetPosts(social_pb2.GetPostsRequest())
        for post in posts_response.posts:
            print(f"- {post.text} (автор: {post.user_id})")

    except grpc.RpcError as e:
        print(f"Ошибка gRPC: {e.code()} - {e.details()}")


if __name__ == "__main__":
    run()
