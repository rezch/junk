from os import environ
from jose import jwt

from datetime import datetime, timedelta


class JwtToken:
    SECRET_KEY: str = environ.get("SECRET_KEY", "somekey")
    ALGORITHM: str = environ.get("ALGORITHM", "HS256")
    ACCESS_TOKEN_EXPIRES_MIN: int = \
        int(environ.get("ACCESS_TOKEN_EXPIRES_MIN", 1440))

    @staticmethod
    def decodeToken(token):
        try:
            return jwt.decode(
                token,
                JwtToken.SECRET_KEY,
                algorithms=[JwtToken.ALGORITHM])
        except Exception:
            return None

    @staticmethod
    def createToken(user):
        payload = {
            "user_id": user['id'],
            "username": user['username'],
            "exp": datetime.utcnow() + timedelta(
                minutes=JwtToken.ACCESS_TOKEN_EXPIRES_MIN)
        }
        return jwt.encode(
            payload, JwtToken.SECRET_KEY, JwtToken.ALGORITHM)
