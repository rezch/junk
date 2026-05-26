#include "db.h"
#include "endpoints.h"
#include "hash.h"
#include "json_verify.h"
#include "jwt.h"
#include "logging.h"

namespace endpoints {

void setExceptionHandler(httplib::Server& server)
{
    server.set_exception_handler(
        [](const auto& /*req*/, auto& res, std::exception_ptr ep)
        {
            auto fmt = "<h1>Error</h1><p>%s</p>";
            char buf[BUFSIZ];

            res.status = httplib::StatusCode::InternalServerError_500;

            try {
                std::rethrow_exception(ep);
            } catch (const database::NotFoundException& e) {
                res.status = httplib::NotFound_404;
                snprintf(buf, sizeof(buf), fmt, e.what());
            } catch (const server::jwt::AuthException& e) {
                res.status = httplib::Forbidden_403;
                snprintf(buf, sizeof(buf), fmt, e.what());
            } catch (std::exception& e) {
                snprintf(buf, sizeof(buf), fmt, e.what());
            } catch (...) {
                snprintf(buf, sizeof(buf), fmt, "Unknown Exception");
            }

            res.set_content(buf, "text/html");
        });
}

uint64_t checkAuth(const httplib::Request& req)
{
    auto token = req.get_header_value("Authorization");

    try {
        server::jwt::verifyToken(token);
        auto decoded = jwt::decode(token).get_payload_json();
        auto uid = std::stoull(decoded.at("uid").get<std::string>());
        database::db()->checkSessionToken(uid, token);
        return uid;
    }
    catch (const std::exception&) {
        throw server::jwt::AuthException("Invalid token");
    }
}

void hi(httplib::Server& server)
{
    server.Get("/hi",
        [](const httplib::Request&, httplib::Response& res)
        {
            res.set_content("Hello World!\n", "text/plain");
        });
}

void Register(httplib::Server& server) {
    static constexpr auto handle = "/register";

    server.Post(handle,
        [](const httplib::Request& req, httplib::Response& res)
        {
            LOG(handle, req.body);
            auto json = nlohmann::json::parse(req.body);
            utils::verify(json, { "name", "email", "password" });

            try {
                database::db()->getUserByEmail(json.at("email"));
                res.status = httplib::Conflict_409;
                res.set_content("this email is already used", "text/html");
                return;
            }
            catch (const database::NotFoundException&) { }

            auto hash = utils::djb2_hash(json.at("password"));

            database::db()->addUser(
                json.at("name"),
                json.at("email"),
                hash);

            res.set_content("{\"message\":\"User created\"}", "application/json");
        });
}

void Login(httplib::Server& server) {
    static constexpr auto handle = "/login";

    server.Post(handle,
        [](const httplib::Request& req, httplib::Response& res)
        {
            LOG(handle, req.body);
            auto json = nlohmann::json::parse(req.body);
            utils::verify(json, { "email", "password" });

            auto hash = utils::djb2_hash(json.at("password"));
            auto user = database::db()->getUserByEmail(json.at("email"));
            if (user.password_hash != hash) {
                res.status = httplib::Forbidden_403;
                res.set_content("wrong password", "text/html");
                return;
            }

            auto token = server::jwt::createToken(user.id);
            database::db()->createSession(user.id, token);

            auto response = nlohmann::json();
            response["accessToken"] = token;
            response["user"]["id"] = user.id;
            response["user"]["name"] = user.name;
            response["user"]["email"] = user.email;

            res.set_content(
                nlohmann::json(response).dump(),
                "application/json");
        });
}

void Me(httplib::Server& server) {
    static constexpr auto handle = "/users/me";

    server.Get(handle,
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto uid = checkAuth(req);
            LOG(handle, uid);

            auto user = database::db()->getUserById(uid);
            auto response = nlohmann::json();
            response["id"] = user.id;
            response["name"] = user.name;
            response["email"] = user.email;

            res.set_content(
                nlohmann::json(response).dump(),
                "application/json");
        });
}

nlohmann::json transactionsToJson(
    const std::vector<models::transaction>& transactions)
{
    nlohmann::json items = nlohmann::json::array();
    for (const auto& product : transactions) {
        items.push_back(product);
    }
    return items;
}

void Transactions(httplib::Server& server) {
    static constexpr std::string handle = "/transactions";

    server.Post(handle,
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto uid = checkAuth(req);
            LOG(handle, "POST", uid, req.body);

            auto json = nlohmann::json::parse(req.body);
            utils::verify(json, { "date", "title", "category", "amount" });

            auto cid = database::db()->getCategoryId(json.at("category"));
            database::db()->addTransaction(
                uid,
                json.at("title"),
                json.at("date"),
                json.at("amount").get<uint64_t>(),
                cid);

            res.set_content(
                "{\"message\":\"Transaction created\"}",
                "application/json");
        });

    server.Get(handle,
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto uid = checkAuth(req);
            LOG(handle, "GET", uid);

            auto transactions = transactionsToJson(
                database::db()->getTransactions(uid));

            res.set_content(
                nlohmann::json(transactions).dump(),
                "application/json");
        });

    server.Get(handle + "/day",
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto uid = checkAuth(req);
            auto date = req.get_param_value("date");
            LOG(handle + "/day", "GET", uid, date);

            auto transactions = transactionsToJson(
                database::db()->getTransactions(uid, date));

            res.set_content(
                nlohmann::json(transactions).dump(),
                "application/json");
        });

    server.Get(handle + "/period",
        [](const httplib::Request& req, httplib::Response& res)
        {
            auto uid = checkAuth(req);
            auto start = req.get_param_value("start");
            auto end = req.get_param_value("end");
            LOG(handle + "/period", "GET", uid, start, end);

            auto transactions = transactionsToJson(
                database::db()->getTransactions(uid, start, end));

            res.set_content(
                nlohmann::json(transactions).dump(),
                "application/json");
        });
}


void create(httplib::Server& server)
{
    setExceptionHandler(server);

    hi(server);
    Register(server);
    Login(server);
    Me(server);
    Transactions(server);
}

} // namespace endpoints
