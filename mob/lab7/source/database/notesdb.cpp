#include "notesdb.h"
#include "singleton.h"
#include "utils.h"

namespace database {

std::string generateUid()
{
    // TODO
    static uint64_t mod{};
    auto uid = utils::timestamp() + ++mod;
    return std::to_string(uid);
}

NotesDB::NotesDB()
{ }

void NotesDB::init(const std::string& dbFile)
{
    storage_ = std::make_unique<models::noteDBType>(
        sqlite_orm::make_storage(dbFile, models::createTableFornote()));
    storage_->sync_schema();
}

std::string NotesDB::add(const std::string& title, const std::string& text)
{
    auto uid = generateUid();
    auto note = models::note{
        .id=uid,
        .text=text,
        .title=title,
        .createdAt=utils::timestamp(),
        .updatedAt=0 };

    storage_->replace(note);
    return uid;
}

models::note NotesDB::get(const std::string& id)
{
    auto note = storage_->get_pointer<models::note>(id);
    if (note == nullptr)
        throw NotFoundException("id not found");

    return *note;
}

void NotesDB::update(const std::string& id, const std::string& title, const std::string& text)
{
    get(id); // existing check

    auto note = models::note{
        .id=id,
        .text=text,
        .title=title,
        .createdAt=utils::timestamp(),
        .updatedAt=0 };

    storage_->replace(note);
}

NotesDB* db()
{
    return &utils::singleton<NotesDB>();
}

}
