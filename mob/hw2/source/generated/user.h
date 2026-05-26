// Auto-generated file for user by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {

struct user {
    uint64_t id;
    std::string name;
    std::string email;
    std::string password_hash;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(user,
        id,
        name,
        email,
        password_hash);
};

inline auto createTableForuser()
{
    return sqlite_orm::make_table("users",
        sqlite_orm::make_column("id", &user::id, sqlite_orm::primary_key()),
        sqlite_orm::make_column("name", &user::name),
        sqlite_orm::make_column("email", &user::email),
        sqlite_orm::make_column("password_hash", &user::password_hash));
}

using userDBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableForuser()));

} // namespace models
