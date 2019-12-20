load("@npm_bazel_typescript//:index.bzl", "ts_devserver", "ts_library")
load("@build_bazel_rules_nodejs//:index.bzl", "pkg_web")
load("@npm_bazel_rollup//:index.bzl", "rollup_bundle")
load("@npm_bazel_terser//:index.bzl", "terser_minified")

ts_library(
    name = "app_ts",
    srcs = [
        "app.ts",
    ],
    deps = ["//js:yngping"],
)

ts_devserver(
    name = "devserver",
    entry_module =
        "libhokchew_wksp/examples/js/app",
    serving_path = "/bundle.js",
    static_files = [
        ":index.html",
    ],
    deps = [
        ":app_ts",
    ],
)

rollup_bundle(
    name = "bundle",
    config_file = "rollup.config.js",
    entry_point = ":app.ts",
    format = "cjs",
    deps = [
        ":app_ts",
        "//js:yngping",
        "@npm//rollup-plugin-commonjs",
        "@npm//rollup-plugin-node-resolve",
    ],
)

terser_minified(
    name = "bundle.min",
    src = "bundle",
)

pkg_web(
    name = "app_package",
    srcs = [
        ":bundle.min",
        ":index.html",
    ],
)
