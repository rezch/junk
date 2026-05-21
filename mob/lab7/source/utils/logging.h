#pragma once

#include <iostream>

namespace {

enum class LOG_LEVEL {
    OFF   = -1,

    UNDEF = 0,
    ERROR = 1,
    INFO  = 2,
    DEBUG = 3,
};

inline LOG_LEVEL parseStr(const std::string& log_level)
{
    if (log_level == "ERROR") {
        return LOG_LEVEL::ERROR;
    }
    if (log_level == "INFO") {
        return LOG_LEVEL::INFO;
    }
    if (log_level == "DEBUG"
        || log_level == "ALL") {
        return LOG_LEVEL::DEBUG;
    }
    return LOG_LEVEL::OFF;
}

[[ maybe_unused ]] inline LOG_LEVEL getLogLevel()
{
    // static auto log_level = parseStr(dotenv::env["LOG_LEVEL"]);
    return LOG_LEVEL::DEBUG;
}

template <class... Args>
inline void log(Args&&...args)
{
    ((std::cerr << args << ' '), ...) << std::endl;
}

template <class Tag, class... Args>
inline void logTag(Tag&& tag, Args&&... args)
{
    std::cerr << "LOG (" << tag << "): ";
    log(args...);
}

} // namespace

#define LOG_ERROR(...)                          \
    do {                                        \
        if (LOG_LEVEL::ERROR <= getLogLevel())  \
            logTag("ERROR", __VA_ARGS__);       \
    } while (0)

#define LOG_INFO(...)                           \
    do {                                        \
        if (LOG_LEVEL::INFO <= getLogLevel())   \
            logTag("INFO", __VA_ARGS__);        \
    } while (0)

#define LOG_DEBUG(...)                          \
    do {                                        \
        if (LOG_LEVEL::DEBUG <= getLogLevel())  \
            logTag("DEBUG", __VA_ARGS__);       \
    } while (0)

#define LOG(...)                                \
    do {                                        \
        if (LOG_LEVEL::UNDEF <= getLogLevel())  \
            log("LOG:", __VA_ARGS__);           \
    } while (0)
