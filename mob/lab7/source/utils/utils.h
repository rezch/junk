#pragma once

#include <filesystem>


namespace utils {

static void EmptyDeleter(void* /*ptr*/) noexcept // NOLINT
{ }

using EmptyDeleterT = decltype(*EmptyDeleter);

inline std::string datetimeNow()
{
    auto now = std::chrono::system_clock::now();
    auto time = std::chrono::system_clock::to_time_t(now);
    std::tm tm{};
    gmtime_r(&time, &tm);

    std::stringstream ss;
    ss << std::put_time(&tm, "%Y-%m-%d %H:%M:%S");
    return ss.str();
}

inline uint64_t timestamp()
{
    const auto clock = std::chrono::system_clock::now();

    return std::chrono::duration_cast<std::chrono::seconds>(
               clock.time_since_epoch()).count();
}

} // namespace utils

#define CURRENT_PATH() \
    std::filesystem::path(__FILE__).parent_path()
