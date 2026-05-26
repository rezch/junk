#----------------------- PROJECT CONFIGURATION --------------------------------
cmake_minimum_required(VERSION 4.0)
project(hw2 VERSION 0.0.1)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

if ("${CMAKE_BUILD_TYPE}" STREQUAL "")
    set(CMAKE_BUILD_TYPE RELEASE)
else()
    string(TOUPPER ${CMAKE_BUILD_TYPE} CMAKE_BUILD_TYPE)
endif()
message(STATUS "Building ${PROJECT_NAME} in ${CMAKE_BUILD_TYPE} mode")

#----------------------- Libraries --------------------------------------------
set(THIRD_PARTY_BUILD_FOLDER third_party_build)
include(FetchContent)

find_package(Patch)
if(Patch_FOUND)
    execute_process(
        COMMAND ${Patch_EXECUTABLE} -p1 -i patches/jwt-cpp.patch
    )
endif()

FetchContent_Declare(httplib
    GIT_REPOSITORY https://github.com/yhirose/cpp-httplib
    GIT_TAG master
)
FetchContent_MakeAvailable(httplib)

set(JSON_BuildTests OFF CACHE INTERNAL "")
add_subdirectory(third_party/nlohmann_json ${THIRD_PARTY_BUILD_FOLDER}/nlohmann_json)

add_subdirectory(third_party/sqlite_orm
    ${THIRD_PARTY_BUILD_FOLDER}/sqlite_orm)

add_subdirectory(third_party/jwt-cpp
    ${THIRD_PARTY_BUILD_FOLDER}/jwt-cpp)

#----------------------- Result path ------------------------------------------
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/bin)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/lib)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/lib)

#----------------------- Moldes generate --------------------------------------
set(MODELS_CONFIG ${CMAKE_SOURCE_DIR}/source/models_gen/models_config.json)
set(GENERATOR_SCRIPT ${CMAKE_SOURCE_DIR}/source/models_gen/generate_models.py)
set(GENERATED_MODELS ${CMAKE_SOURCE_DIR}/source/generated)
set(DUMMY_SOURCE ${GENERATED_MODELS}/dummy)

file(WRITE ${DUMMY_SOURCE}.cpp "// Dummy file for CMake\n")
file(WRITE ${DUMMY_SOURCE}.h "#pragma once\n// Dummy file for CMake\n")

find_package(Python3 REQUIRED)
add_custom_command(
    OUTPUT ${GENERATED_MODELS}
    COMMAND ${Python3_EXECUTABLE}
        ${GENERATOR_SCRIPT}
        --config ${MODELS_CONFIG}
        --output ${GENERATED_MODELS}
    DEPENDS
        ${GENERATOR_SCRIPT}
        ${MODELS_CONFIG}
    COMMENT "Generating database models from ${MODELS_CONFIG}"
    VERBATIM)

add_library(generated_models
    OBJECT ${DUMMY_SOURCE}.cpp)

target_include_directories(generated_models
    PUBLIC ${GENERATED_DIR})

#----------------------- Executable -------------------------------------------
add_executable(${PROJECT_NAME}
    main.cpp
    ${GENERATED_MODELS})

target_compile_features(${PROJECT_NAME}
    PRIVATE cxx_std_20)

target_link_libraries(${PROJECT_NAME}
    PUBLIC
        nlohmann_json::nlohmann_json
    PRIVATE
        generated_models
        sqlite_orm::sqlite_orm
        httplib::httplib
        jwt-cpp::jwt-cpp)

target_sources(${PROJECT_NAME}
    PRIVATE
        source/server/endpoints.cpp
        source/database/db.cpp
        source/server/jwt.cpp)

target_include_directories(${PROJECT_NAME}
    PRIVATE
        ${PROJECT_SOURCE_DIR}
        ${CMAKE_CURRENT_SOURCE_DIR}/source/utils
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/source/include>
        $<BUILD_INTERFACE:${GENERATED_MODELS}>
        $<INSTALL_INTERFACE:${CMAKE_INSTALL_INCLUDEDIR}>)

#----------------------- Build ------------------------------------------------
if ("${CMAKE_BUILD_TYPE}" STREQUAL "DEBUG")
    target_compile_options(${PROJECT_NAME} PRIVATE
        -g -Wall -O0)
else()
    target_compile_options(${PROJECT_NAME} PRIVATE
        -O3)
endif()
