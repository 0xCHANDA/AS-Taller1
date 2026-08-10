#!/usr/bin/env bash
set -euo pipefail

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || exit 2
cd "$repo_root"
echo "OpenCode version: $(opencode --version)"
opencode stats --pure --project "$repo_root" --models
