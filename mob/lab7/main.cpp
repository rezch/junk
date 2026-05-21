#include "endpoints.h"
#include "logging.h"
#include "notesdb.h"

#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>
#include <httplib.h>

constexpr auto dbFile = "notes.sqlite";

int main()
{
    httplib::Server server;
    endpoints::create(server);
    database::db()->init(dbFile);

    LOG("Starting server...");
    server.listen("0.0.0.0", 8080);
}
