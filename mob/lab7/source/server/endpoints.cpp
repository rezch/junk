#include "endpoints.h"
#include "logging.h"
#include "notesdb.h"

namespace endpoints {

#define NOT_FOUND(response, msg)                        \
    do {                                                \
        (response).status = httplib::NotFound_404;      \
        (response).set_content((msg), "text/plain");    \
        return;                                         \
    } while (0);

void setExceptionHandler(httplib::Server& server)
{
    server.set_exception_handler(
        [](const auto& req, auto& res, std::exception_ptr ep)
        {
            ;
            auto fmt = "<h1>Error 500</h1><p>%s</p>";
            char buf[BUFSIZ];

            try {
                std::rethrow_exception(ep);
            } catch (const database::NotFoundException& ex) {
                NOT_FOUND(res, ex.what())
            } catch (std::exception& e) {
                snprintf(buf, sizeof(buf), fmt, e.what());
            } catch (...) {
                snprintf(buf, sizeof(buf), fmt, "Unknown Exception");
            }

            res.set_content(buf, "text/html");
            res.status = httplib::StatusCode::InternalServerError_500;
        });
}

void hi(httplib::Server& server)
{
    server.Get("/hi",
        [](const httplib::Request&, httplib::Response& res)
        {
            res.set_content("Hello World!\n", "text/plain");
        });
}

void CreateNote(httplib::Server& server)
{
    server.Get("/add",
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto title = req.get_param_value("title");
            auto text = req.get_param_value("text");
            LOG("ADD:", title, text);

            auto id = database::db()->add(title, text);

            res.set_content(id, "text/plain");
        });
}

void GetNoteById(httplib::Server& server)
{
    server.Get("/get",
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto id = req.get_param_value("id");
            LOG("GET:", id);

            auto note = database::db()->get(id);

            res.set_content(
                nlohmann::json(note).dump(),
                "application/json");
        });
}

void EditNote(httplib::Server& server)
{
    server.Get("/put",
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto id = req.get_param_value("id");
            auto title = req.get_param_value("title");
            auto text = req.get_param_value("text");
            LOG("PUT:", id, title, text);

            database::db()->update(id, title, text);
        });
}

void create(httplib::Server& server)
{
    setExceptionHandler(server);

    hi(server);
    CreateNote(server);
    GetNoteById(server);
    EditNote(server);
}

} // namespace endpoints
