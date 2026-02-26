#include "triangle.h"
#include "vector.h"
#include <matrix.h>

#include <thread>
#include <utils.h>
#include <vector>

#include <SDL.h>


int main()
{
    SDL_Init( SDL_INIT_EVERYTHING );
    SDL_Window* window = SDL_CreateWindow(
        "SDL",
        SDL_WINDOWPOS_UNDEFINED,
        SDL_WINDOWPOS_UNDEFINED,
        800, 600,
        SDL_WINDOW_SHOWN);
    SDL_Renderer* renderer = SDL_CreateRenderer(
        window,
        -1,
        SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);

    auto t = Triangle(Matrix({
        400, 150, 10, 0,
        200, 450, 10, 0,
        600, 450, 10, 0,
        0, 0, 0, 0
    }));

    bool running = true;
    bool isRotating = true;
    while (running) {
        SDL_Event ev;
        while (SDL_PollEvent(&ev)) {
            if ((SDL_QUIT == ev.type) ||
                 (SDL_KEYDOWN == ev.type && SDL_SCANCODE_ESCAPE == ev.key.keysym.scancode)) {
                running = false;
                break;
            }
            if (SDL_KEYDOWN == ev.type && SDL_SCANCODE_SPACE == ev.key.keysym.scancode) {
                isRotating = !isRotating;
            }
        }

        auto verts = t.toSDL();
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, SDL_ALPHA_OPAQUE);
        SDL_RenderClear(renderer);
        SDL_RenderGeometry(renderer, nullptr, verts.data(), verts.size(), nullptr, 0);
        SDL_RenderPresent(renderer);

        if (isRotating)
            t.rotX(5);

        std::this_thread::sleep_for(
            std::chrono::milliseconds(100));
    }

    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();
}
