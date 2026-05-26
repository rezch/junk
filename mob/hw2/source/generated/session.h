// Auto-generated file for session by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {

struct session {
    uint64_t user_id;
    std::string token;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(session,
        user_id,
        token);
};

inline auto createTableForsession()
{
    return sqlite_orm::make_table("sessions",
        sqlite_orm::make_column("user_id", &session::user_id, sqlite_orm::primary_key()),
        sqlite_orm::make_column("token", &session::token));
}

using sessionDBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableForsession()));

} // namespace models
