// Auto-generated file for category by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {

struct category {
    uint64_t id;
    std::string name;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(category,
        id,
        name);
};

inline auto createTableForcategory()
{
    return sqlite_orm::make_table("categories",
        sqlite_orm::make_column("id", &category::id, sqlite_orm::primary_key()),
        sqlite_orm::make_column("name", &category::name));
}

using categoryDBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableForcategory()));

} // namespace models
