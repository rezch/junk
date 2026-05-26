#include "jwt.h"

namespace server::jwt {

const auto algo = ::jwt::algorithm::hs256{"secret"};

std::string createToken(uint64_t uid)
{
    auto claim = ::jwt::claim(std::to_string(uid));

    auto token = ::jwt::create()
        .set_type("JWT")
        .set_issuer("auth0")
        .set_expires_in(std::chrono::hours{1})
        .set_payload_claim("uid", claim)
        .sign(algo);

    return token;
}

void verifyToken(const std::string& token)
{
    auto decoded = ::jwt::decode(token);

    ::jwt::verify()
        .allow_algorithm(algo)
        .with_issuer("auth0")
        .verify(decoded);
}

}
