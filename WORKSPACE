workspace(
    name = "libhokchew_wksp",
    managed_directories = {"@npm": ["node_modules"]},
)

load("@bazel_tools//tools/build_defs/repo:git.bzl", "git_repository")

# Python
git_repository(
    name = "rules_python",
    commit = "38f86fb55b698c51e8510c807489c9f4e047480e",
    remote = "https://github.com/bazelbuild/rules_python.git",
)

load("@rules_python//python:repositories.bzl", "py_repositories")

py_repositories()

# For Python packaging rules.
load("@rules_python//python:pip.bzl", "pip_repositories")

pip_repositories()

# JavaScript
load("@bazel_tools//tools/build_defs/repo:http.bzl", "http_archive")

RULES_NODEJS_VERSION = "1.6.0"

RULES_NODEJS_SHA256 = "f9e7b9f42ae202cc2d2ce6d698ccb49a9f7f7ea572a78fd451696d03ef2ee116"

http_archive(
    name = "build_bazel_rules_nodejs",
    sha256 = RULES_NODEJS_SHA256,
    url = "https://github.com/bazelbuild/rules_nodejs/releases/download/%s/rules_nodejs-%s.tar.gz" % (RULES_NODEJS_VERSION, RULES_NODEJS_VERSION),
)

load("@build_bazel_rules_nodejs//:index.bzl", "node_repositories", "yarn_install")

node_repositories()

yarn_install(
    name = "npm",
    package_json = "//:package.json",
    yarn_lock = "//:yarn.lock",
)

load("@npm//:install_bazel_dependencies.bzl", "install_bazel_dependencies")

install_bazel_dependencies()

# Set up TypeScript toolchain
load("@npm_bazel_typescript//:index.bzl", "ts_setup_workspace")

ts_setup_workspace()
