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
