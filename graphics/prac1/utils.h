#pragma once

#include <iostream>


namespace ut {

template <class... Args>
void write(Args&&... args)
{
    ((std::cout << args << ' '), ...) << std::endl;
}

inline void write()
{ }

template <class Container>
void print(Container&& c)
{
    for (const auto& x : c) {
        write(x);
    }
}

} // namespace ut
