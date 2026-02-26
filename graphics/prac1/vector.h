#pragma once

#include <array>
#include <cstddef>


class Vector {
public:
    static constexpr size_t SIZE = 4;

    Vector() = default;

    Vector(std::array<float, 4> data)
        : data_(data)
    { }

    float& operator [] (size_t idx)
    {
        return data_[idx];
    }

    float operator [] (size_t idx) const
    {
        return data_[idx];
    }

    std::array<float, 4> data() const
    {
        return data_;
    }

    Vector& operator *= (const Vector& rhs)
    {
        for (int i = 0; i < SIZE; ++i)
            data_[i] *= rhs.data_[i];
        return *this;
    }

    Vector operator * (const Vector& rhs) const
    {
        auto result = *this;
        return result *= rhs;
    }

    Vector& operator -= (const Vector& rhs)
    {
        for (int i = 0; i < SIZE; ++i)
            data_[i] -= rhs.data_[i];
        return *this;
    }

    Vector operator - (const Vector& rhs) const
    {
        auto result = *this;
        return result -= rhs;
    }

    Vector& neg()
    {
        for (int i = 0; i < SIZE; ++i)
            data_[i] = -data_[i];
        return *this;
    }

private:
    std::array<float, SIZE> data_;
};

inline float scalar_mult(const Vector& lhs, const Vector& rhs)
{
    float result{};
    for (int i = 0; i < Vector::SIZE; ++i)
        result += lhs[i] * rhs[i];
    return result;
}
