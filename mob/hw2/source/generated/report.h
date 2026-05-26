// Auto-generated file for report by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {

struct report {
    uint64_t id;
    uint64_t user_id;
    std::string period_start;
    std::string period_end;
    std::string created_at;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(report,
        id,
        user_id,
        period_start,
        period_end,
        created_at);
};

inline auto createTableForreport()
{
    return sqlite_orm::make_table("reports",
        sqlite_orm::make_column("id", &report::id, sqlite_orm::primary_key()),
        sqlite_orm::make_column("user_id", &report::user_id),
        sqlite_orm::make_column("period_start", &report::period_start),
        sqlite_orm::make_column("period_end", &report::period_end),
        sqlite_orm::make_column("created_at", &report::created_at));
}

using reportDBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableForreport()));

} // namespace models
