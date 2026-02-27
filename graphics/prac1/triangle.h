#pragma once

#include "utils.h"
#include <calc.h>
#include <vector.h>
#include <matrix.h>

#include <SDL.h>


class Triangle {
public:
    Triangle() = default;
    Triangle(Matrix verts)
        : verts_(verts)
    { }

    static uint8_t castToDepth(float value)
    {
        if (value < 0 || value > std::numeric_limits<uint8_t>().max())
            return 0;
        return std::numeric_limits<uint8_t>().max() - static_cast<uint8_t>(value);
    }

    std::array<SDL_Vertex, 3> toSDL() const {
        const auto tex_coord = SDL_FPoint{0};
        const std::array<SDL_Vertex, 3> verts =
        {
            SDL_Vertex{ SDL_FPoint{ verts_[0][0], verts_[0][1] }, SDL_Color{ 0, 0, castToDepth(verts_[0][2]), 255 }, tex_coord },
            SDL_Vertex{ SDL_FPoint{ verts_[1][0], verts_[1][1] }, SDL_Color{ 0, 0, castToDepth(verts_[1][2]), 255 }, tex_coord },
            SDL_Vertex{ SDL_FPoint{ verts_[2][0], verts_[2][1] }, SDL_Color{ 0, 0, castToDepth(verts_[2][2]), 255 }, tex_coord },
        };
        return verts;
    }

    static float degreesToRadians(float degrees)
    {
        return degrees * M_PI / 180;
    }

    void rotZ(float degrees)
    {
        float angle = degreesToRadians(degrees);
        auto c = center();
        moveToPoint(c);
        auto rotM = Matrix({
            std::cos(angle), -std::sin(angle), 0, 0,
            std::sin(angle), std::cos(angle), 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        });
        verts_ *= rotM;
        moveToPoint(c.neg());
    }

    void rotX(float degrees)
    {
        float angle = degreesToRadians(degrees);
        auto c = center();
        moveToPoint(c);
        auto rotM = Matrix({
            1, 0, 0, 0,
            0, std::cos(angle), -std::sin(angle), 0,
            0, std::sin(angle), std::cos(angle), 0,
            0, 0, 0, 1,
        });
        verts_ *= rotM;
        moveToPoint(c.neg());
    }

    Vector center() const
    {
        auto cV = Vector({ 1.f / 3, 1.f / 3, 1.f / 3, 1 });
        return mult(cV, verts_);
    }

    void moveToPoint(const Vector& p)
    {
        verts_[0] = (Vector({verts_[0]}) - p).data();
        verts_[1] = (Vector({verts_[1]}) - p).data();
        verts_[2] = (Vector({verts_[2]}) - p).data();
    }

private:
    Matrix verts_;
};
