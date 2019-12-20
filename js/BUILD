load("@npm_bazel_jasmine//:index.bzl", "jasmine_node_test")
load("@npm_bazel_typescript//:index.bzl", "ts_library")

ts_library(
    name = "yngping",
    srcs = glob(
        ["**/*.ts"],
        exclude = ["**/*.spec.ts"],
    ),
    visibility = ["//visibility:public"],
    module_name="@hokchewjs"
)

ts_library(
    name = "tests_ts",
    srcs = glob(["**/*.spec.ts"]),
    deps = [
        ":yngping",
        "@npm//:node_modules",
    ],
)

jasmine_node_test(
    name = "tests",
    srcs = glob(["**/*.spec.js"]),
    deps = [
        ":tests_ts",
    ],
)
