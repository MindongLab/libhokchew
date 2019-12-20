load("@npm_bazel_typescript//:index.bzl", "ts_library","ts_devserver")

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
