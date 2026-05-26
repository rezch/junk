#pragma once

#include <exception>
#include <string>


namespace utils {

class Exception : public std::exception {
public:
    Exception(const char* msg)
        : message(msg)
    { }

    const char* what() const noexcept
    {
        return message.c_str();
    }

private:
    std::string message;
};

} // namespace utils
