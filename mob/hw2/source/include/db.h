#pragma once

#include "category.h"
#include "report.h"
#include "session.h"
#include "transaction.h"
#include "user.h"

#include "exceptions.h"

#include <string>

#include <sqlite_orm/sqlite_orm.h>


namespace database {

class DBException : public utils::Exception {
public:
    using Exception::Exception;
};

class NotFoundException : public utils::Exception {
public:
    using Exception::Exception;
};

class DB {
public:
    using DBType = decltype(sqlite_orm::make_storage("",
        models::createTableForuser(),
        models::createTableForcategory(),
        models::createTableForreport(),
        models::createTableForsession(),
        models::createTableFortransaction()));

    DB();

    void init(const std::string& dbFile);

    void addUser(
        const std::string& name,
        const std::string& email,
        const std::string& hash);
    models::user getUserById(uint64_t id);
    models::user getUserByEmail(const std::string& email);

    void createSession(
        uint64_t user_id,
        std::string token);
    void checkSessionToken(
        uint64_t user_id,
        std::string token);

    void addCategory(std::string name);
    uint64_t getCategoryId(std::string name);

    void addTransaction(
        uint64_t uid,
        std::string title,
        std::string date,
        uint64_t amount,
        uint64_t category_id);

    std::vector<models::transaction> getTransactions(uint64_t uid);
    std::vector<models::transaction> getTransactions(uint64_t uid, std::string date);
    std::vector<models::transaction> getTransactions(uint64_t uid, std::string from, std::string to);

private:
    std::unique_ptr<DBType> storage_;
};

DB* db();

} // namespace database
