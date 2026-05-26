#pragma once

#include <cstdint>
#include <string>

namespace utils {

inline std::string djb2_hash(const std::string& password)
{
    uint32_t hash = 5381;

    for (char c : password)
        hash = ((hash << 5) + hash) + c;

    return std::to_string(hash);
}

}
