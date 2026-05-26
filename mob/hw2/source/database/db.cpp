#include "db.h"
#include "jwt.h"
#include "singleton.h"
#include "utils.h"

namespace database {

uint64_t generateUid()
{
    // TODO
    static uint64_t mod{};
    auto uid = utils::timestamp() + ++mod;
    return uid;
}

DB::DB()
{ }

void DB::init(const std::string& dbFile)
{
    storage_ = std::make_unique<DBType>(
        sqlite_orm::make_storage(dbFile,
        models::createTableForuser(),
        models::createTableForcategory(),
        models::createTableForreport(),
        models::createTableForsession(),
        models::createTableFortransaction()));

    storage_->sync_schema();
}

void DB::addUser(const std::string& name, const std::string& email, const std::string& hash)
{
    auto uid = generateUid();
    auto note = models::user{
        .id=uid,
        .name=name,
        .email=email,
        .password_hash=hash,
    };

    storage_->replace(note);
}

models::user DB::getUserById(uint64_t id)
{
    auto user = storage_->get_pointer<models::user>(id);
    if (user == nullptr)
        throw NotFoundException("id not found");
    return *user;
}

models::user DB::getUserByEmail(const std::string& email)
{
    auto user = storage_->get_all<models::user>(
        sqlite_orm::where(sqlite_orm::c(&models::user::email) == email));
    if (user.empty())
        throw NotFoundException("id not found");
    return user.front();
}

void DB::createSession(
    uint64_t user_id,
    std::string token)
{
    auto session = models::session{
        .user_id=user_id,
        .token=token,
    };
    storage_->replace(session);
}

void DB::checkSessionToken(
    uint64_t user_id,
    std::string token)

{
    auto session = storage_->get_pointer<models::session>(user_id);
    if (session == nullptr)
        throw NotFoundException("id not found");
    if (token != session->token)
        throw server::jwt::AuthException("Bad token");
}

void DB::addCategory(std::string name)
{
    auto id = generateUid();
    auto category = models::category{
        .id=id,
        .name=name,
    };
    storage_->replace(category);
}

uint64_t DB::getCategoryId(std::string name)
{
    auto category = storage_->get_all<models::category>(
        sqlite_orm::where(sqlite_orm::c(&models::category::name) == name));
    if (category.empty())
        throw NotFoundException("id not found");
    return category.front().id;
}

void DB::addTransaction(
    uint64_t uid,
    std::string title,
    std::string date,
    uint64_t amount,
    uint64_t category_id)
{
    auto id = generateUid();
    auto transaction = models::transaction{
        .id=id,
        .user_id=uid,
        .title=title,
        .date=date,
        .amount=amount,
        .category_id=category_id,
    };

    storage_->replace(transaction);
}

std::vector<models::transaction> DB::getTransactions(uint64_t uid)
{
    auto transactions = storage_->get_all<models::transaction>(
        sqlite_orm::where(
            sqlite_orm::c(&models::transaction::user_id) == uid),
        sqlite_orm::order_by(&models::transaction::date));
    return transactions;
}

std::vector<models::transaction> DB::getTransactions(uint64_t uid, std::string date)
{
    auto transactions = storage_->get_all<models::transaction>(
        sqlite_orm::where(
            sqlite_orm::c(&models::transaction::user_id) == uid
            and sqlite_orm::c(&models::transaction::date) == date),
        sqlite_orm::order_by(&models::transaction::date));
    return transactions;
}

std::vector<models::transaction> DB::getTransactions(uint64_t uid, std::string from, std::string to)
{
    auto transactions = storage_->get_all<models::transaction>(
        sqlite_orm::where(
            sqlite_orm::c(&models::transaction::user_id) == uid
            and sqlite_orm::c(&models::transaction::date) >= from
            and sqlite_orm::c(&models::transaction::date) <= to),
        sqlite_orm::order_by(&models::transaction::date));
    return transactions;
}

DB* db()
{
    return &utils::singleton<DB>();
}

}
