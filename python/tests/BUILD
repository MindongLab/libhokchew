load("@rules_python//python:defs.bzl", "py_test")

test_suite(
    name = "tests",
    tests = [
        ":ParserTest",
        ":YngPingParseTest",
    ],
)

py_test(
    name = "ParserTest",
    size = "small",
    srcs = ["ParserTest.py"],
    deps = [
        "//python:pyhokchew",
    ],
)

py_test(
    name = "YngPingParseTest",
    size = "small",
    srcs = ["YngPingParseTest.py"],
    deps = [
        "//python:pyhokchew",
    ],
)
