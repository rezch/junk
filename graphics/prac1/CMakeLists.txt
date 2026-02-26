cmake_minimum_required(VERSION 4.0)
project(prac1)

set(CMAKE_CXX_STANDARD 23)

find_package(SDL2 REQUIRED)
include_directories(SDL2Test ${SDL2_INCLUDE_DIRS})

add_compile_options(-fsanitize=address)
add_link_options(-fsanitize=address)

# ------------------------------ Srcs
add_executable(${PROJECT_NAME})

target_link_libraries(${PROJECT_NAME}
    PRIVATE ${SDL2_LIBRARIES})

target_sources(${PROJECT_NAME}
    PRIVATE main.cpp)

target_include_directories(${PROJECT_NAME}
    PRIVATE
        ${PROJECT_SOURCE_DIR}
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
        $<INSTALL_INTERFACE:${CMAKE_INSTALL_INCLUDEDIR}>)

# ------------------------------ Build
if ("${CMAKE_BUILD_TYPE}" STREQUAL "DEBUG")
    target_compile_options(${PROJECT_NAME} PRIVATE
        -g -Wall -O0
    )
else()
    target_compile_options(${PROJECT_NAME} PRIVATE
        -O3
    )
endif()
