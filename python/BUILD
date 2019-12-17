load("@rules_python//python:defs.bzl", "py_library")
load("@rules_python//experimental/python:wheel.bzl", "py_wheel")

py_library(
    name = "pyhokchew",
    srcs = glob(
        ["pyhokchew/**/*.py"],
        exclude = ["pyhokchew/tests/*"],
    ),
    visibility = ["//visibility:public"],
)

py_library(
    name = "pyhokchew_with_readme",
    srcs = glob(
        ["pyhokchew/**/*.py"],
        exclude = ["pyhokchew/tests/*"],
    ),
    data = ["README.md"],
)

py_wheel(
    name = "wheel",
    description_file = "README.md",
    distribution = "pyhokchew",
    python_tag = "py3",
    strip_path_prefixes = [
        "python",
    ],
    version = "0.1.1.dev",
    deps = [":pyhokchew_with_readme"],
)
