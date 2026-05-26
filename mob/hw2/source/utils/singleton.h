#pragma once

#include <memory>


namespace utils {

template <class Class>
Class& singleton() noexcept
{
    static Class obj;
    return obj;
};

template <class Dispatcher>
concept DispatcherConcept = requires(Dispatcher dispatcher) {
    dispatcher.stop();
};

template <class Dispatcher>
    requires DispatcherConcept<Dispatcher>
Dispatcher& dispatcher() noexcept
{
    static auto _destructor =
        std::unique_ptr<Dispatcher, void(*)(Dispatcher*)>(
            &singleton<Dispatcher>(),
            [](Dispatcher* obj) { obj->stop(); });

    return singleton<Dispatcher>();
};

} // namespace utils
