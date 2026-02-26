#pragma once

#include <vector.h>
#include <matrix.h>

inline Vector mult(const Matrix& m, const Vector& v)
{
    auto result = Vector{};
    for (int i = 0; i < Matrix::SIZE; ++i)
        result[i] = scalar_mult(m[i], v);
    return result;
}

inline Vector mult(const Vector& v, const Matrix& m)
{
    auto result = Vector{};
    auto mt = m.T();
    for (int i = 0; i < Matrix::SIZE; ++i)
        result[i] = scalar_mult(v, m[i]);
    return result;
}
