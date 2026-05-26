#include "endpoints.h"
#include "logging.h"
#include "db.h"

#include <httplib.h>
#include <sqlite_orm/sqlite_orm.h>


constexpr auto dbFile = "db.sqlite";

int main()
{
    httplib::Server server;
    endpoints::create(server);
    database::db()->init(dbFile);
    database::db()->addCategory("income");
    database::db()->addCategory("expense");

    LOG("Starting server...");
    server.listen("0.0.0.0", 8080);
}
