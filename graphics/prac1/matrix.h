#pragma once

#include <array>
#include <cstddef>


class Matrix {
public:
    static constexpr size_t SIZE = 4;

    Matrix() = default;

    Matrix(std::array<std::array<float, 4>, 4> data)
        : data_(data)
    { }

    std::array<float, 4>& operator [] (size_t idx)
    {
        return data_[idx];
    }

    const std::array<float, 4>& operator [] (size_t idx) const
    {
        return data_[idx];
    }

    Matrix& operator *= (float rhs)
    {
        for (size_t i = 0; i < SIZE; ++i)
            for (size_t j = 0; j < SIZE; ++j)
                data_[i][j] *= rhs;
        return *this;
    }

    Matrix operator * (float rhs) const
    {
        auto copy = *this;
        return copy *= rhs;
    }

    Matrix& operator *= (const Matrix& rhs)
    {
        *this = *this * rhs;
        return *this;
    }

    Matrix operator * (const Matrix& rhs) const
    {
        auto result = Matrix{};
        for (size_t i = 0; i < SIZE; ++i)
            for (size_t j = 0; j < SIZE; ++j)
                for (size_t k = 0; k < SIZE; ++k)
                    result[i][j] += data_[i][k] * rhs.data_[k][j];
        return result;
    }

    Matrix& T()
    {
        for (size_t i = 0; i < SIZE; ++i)
            for (size_t j = i + 1; j < SIZE; ++j)
                std::swap(data_[i][j], data_[j][i]);
        return *this;
    }

    Matrix T() const
    {
        auto result = *this;
        return result.T();
    }

private:
    std::array<std::array<float, 4>, 4> data_{};
};
