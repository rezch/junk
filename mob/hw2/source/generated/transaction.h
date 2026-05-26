// Auto-generated file for transaction by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {

struct transaction {
    uint64_t id;
    uint64_t user_id;
    std::string title;
    std::string date;
    uint64_t amount;
    uint64_t category_id;

    NLOHMANN_DEFINE_TYPE_INTRUSIVE(transaction,
        id,
        user_id,
        title,
        date,
        amount,
        category_id);
};

inline auto createTableFortransaction()
{
    return sqlite_orm::make_table("transactions",
        sqlite_orm::make_column("id", &transaction::id, sqlite_orm::primary_key()),
        sqlite_orm::make_column("user_id", &transaction::user_id),
        sqlite_orm::make_column("title", &transaction::title),
        sqlite_orm::make_column("date", &transaction::date),
        sqlite_orm::make_column("amount", &transaction::amount),
        sqlite_orm::make_column("category_id", &transaction::category_id));
}

using transactionDBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableFortransaction()));

} // namespace models
