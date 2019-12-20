load("@npm_bazel_typescript//:index.bzl", "ts_library","ts_devserver")
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
    deps=[
        ":app_ts"
    ],
    serving_path="/bundle.js",
    static_files = [
        ":index.html"
    ],
    entry_module = 
        "libhokchew_wksp/examples/js/app"
    
)

rollup_bundle(
    name = "bundle",
    deps = [":app_ts"],
    entry_point = ":app.ts",
    output_dir = True,
)

terser_minified(
    name = "bundle.min",
    src = "bundle",
)

pkg_web(
    name = "app_package",
    srcs = [":bundle.min", ":index.html"],
)
