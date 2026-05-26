#!/usr/bin/env python3
import argparse
import json
from pathlib import Path

FILE_TEMPLATE = """// Auto-generated file for {model_name} by source/models_gen/generate_models.py
#pragma once

#include <string>
#include <cstdint>
#include <sqlite_orm/sqlite_orm.h>
#include <nlohmann/json.hpp>

namespace models {{

struct {model_name} {{
"""

JSON_TEMPLATE = """
    NLOHMANN_DEFINE_TYPE_INTRUSIVE({model_name},
        {fields_names});
"""

CREATE_FUNC_INIT_TEMPLATE = """
inline auto createTableFor{model_name}()
{{
    return sqlite_orm::make_table(\"{table_name}\",
"""
TABLE_ROW_TEMPLATE = "        sqlite_orm::make_column(\"{name}\", &{model_name}::{name}"

DECLTYPE_TEMPLATE = """
using {model_name}DBType = decltype(sqlite_orm::make_storage("foo_name", models::createTableFor{model_name}()));
"""


def parse_args():
    parser = argparse.ArgumentParser(description="Generate C++ headers for sqlite_orm models")
    parser.add_argument("--config", "-c", required=True, help="Path to config JSON file")
    parser.add_argument("--output", "-o", required=True, help="Output path for generated file")
    return parser.parse_args()


def model_to_filename(model_name):
    result = []
    for i, char in enumerate(model_name):
        if char.isupper() and i > 0:
            result.append('_')
        result.append(char.lower())
    return ''.join(result) + '.h'


def gen(model_data, output_dir):
    model_name = model_data['name']
    table_name = model_data['table_name']
    output_path = output_dir / model_to_filename(model_name)

    with open(output_path, 'w') as f:
        f.write(FILE_TEMPLATE.format(model_name=model_name))

        for field in model_data['fields']:
            f.write(f"    {field['type']} {field['name']};\n")

        if "from_json" in model_data and model_data["from_json"]:
            fields_names = [field['name'] for field in model_data['fields']]
            f.write(JSON_TEMPLATE.format(
                model_name=model_name,
                fields_names=',\n        '.join(fields_names)))

        f.write("};\n")

        f.write(CREATE_FUNC_INIT_TEMPLATE.format(model_name=model_name, table_name=table_name))
        for i, field in enumerate(model_data['fields']):
            if i > 0:
                f.write(",\n")
            f.write(TABLE_ROW_TEMPLATE.format(name=field['name'], model_name=model_name))
            if "primary_key" in field and field["primary_key"]:
                f.write(", sqlite_orm::primary_key())")
            else:
                f.write(")")
        f.write(");\n}\n")

        f.write(DECLTYPE_TEMPLATE.format(model_name=model_name))

        f.write("\n} // namespace models\n")

    print(f"Generated {output_path}")


def main():
    args = parse_args()

    output_dir = Path(args.output)
    output_dir.mkdir(parents=True, exist_ok=True)

    with open(args.config) as f:
        config = json.load(f)

    for old_file in output_dir.glob("*.h"):
        old_file.unlink()

    dummy_file = output_dir / "dummy.h"
    dummy_file.write_text("// Dummy header file for CMake\n")

    for model in config['models']:
        gen(model, output_dir)

if __name__ == "__main__":
    main()
