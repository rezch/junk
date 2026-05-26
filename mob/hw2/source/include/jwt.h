#pragma once

#include "exceptions.h"
#include <jwt-cpp/jwt.h>

namespace server::jwt {

class AuthException : public utils::Exception {
public:
    using Exception::Exception;
};

std::string createToken(uint64_t uid);

void verifyToken(const std::string& token);

}
