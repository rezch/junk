// Auto-generated file for note by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {

struct note {
    std::string id;
    std::string text;
    std::string title;
    uint64_t createdAt;
    uint64_t updatedAt;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(note,
        id,
        text,
        title,
        createdAt,
        updatedAt);
};

inline auto createTableFornote()
{
    return sqlite_orm::make_table("notes",
        sqlite_orm::make_column("id", &note::id, sqlite_orm::primary_key()),
        sqlite_orm::make_column("text", &note::text),
        sqlite_orm::make_column("title", &note::title),
        sqlite_orm::make_column("createdAt", &note::createdAt),
        sqlite_orm::make_column("updatedAt", &note::updatedAt));
}

using noteDBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableFornote()));

} // namespace models
