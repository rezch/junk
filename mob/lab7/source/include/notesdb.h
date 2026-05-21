#pragma once

#include "note.h"
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

class NotesDB {
public:
    NotesDB();

    void init(const std::string& dbFile);

    std::string add(const std::string& title, const std::string& text);

    models::note get(const std::string& id);

    void update(const std::string& id, const std::string& title, const std::string& text);

private:
    std::unique_ptr<models::noteDBType> storage_;
};

NotesDB* db();

} // namespace database
