import social_pb2
import social_pb2_grpc
import grpc

from server.services.token import JwtToken

posts = []
next_post_id = 1


class WallService(social_pb2_grpc.WallServiceServicer):

    def CreatePost(self, request, context):
        global next_post_id

        token = JwtToken.decodeToken(
            dict(context.invocation_metadata())
            .get('authorization', '')
            .replace('Bearer ', ''))

        if token is None:
            context.set_code(grpc.StatusCode.UNAUTHENTICATED)
            context.set_details('Invalid or expired token')
            return social_pb2.CreatePostResponse()

        post_id = str(next_post_id)
        next_post_id += 1

        # Пока используем фиктивного пользователя
        post = {
            'id': post_id,
            'user_id': token['user_id'],
            'text': request.text
        }
        posts.append(post)

        return social_pb2.CreatePostResponse(
            post=social_pb2.Post(
                id=post['id'],
                user_id=post['user_id'],
                text=post['text']
            )
        )

    def GetPosts(self, request, context):
        # Этот метод публичный - не требует аутентификации
        response_posts = []
        for post in posts:
            response_posts.append(social_pb2.Post(
                id=post['id'],
                user_id=post['user_id'],
                text=post['text']
            ))

        return social_pb2.GetPostsResponse(posts=response_posts)
