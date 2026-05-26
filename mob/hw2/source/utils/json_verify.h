#pragma once

#include "exceptions.h"
#include <nlohmann/json.hpp>

namespace utils {

class VerifyException : public Exception {
public:
    using Exception::Exception;
};

inline void verify(const nlohmann::json& json, std::vector<std::string> keys)
{
    for (const auto& key : keys) {
        if (!json.contains(key)) {
            auto msg = "Key not found in request json: " + key;
            throw VerifyException(msg.c_str());
        }
    }
}

}
