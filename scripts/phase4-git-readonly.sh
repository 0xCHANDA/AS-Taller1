#!/usr/bin/env bash
set -euo pipefail

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || {
  echo "TOOL_BLOCKED: no es un repositorio Git" >&2
  exit 2
}
cd "$repo_root"

case "${1:-}" in
  status)
    [ "$#" -eq 1 ] || exit 64
    git status --short
    ;;
  branch)
    [ "$#" -eq 1 ] || exit 64
    git branch --show-current
    ;;
  log)
    [ "$#" -eq 1 ] || exit 64
    git log --oneline -10
    ;;
  diff)
    [ "$#" -eq 1 ] || exit 64
    git --no-pager diff --no-ext-diff --
    ;;
  diff-names)
    [ "$#" -eq 1 ] || exit 64
    git diff --name-status --no-ext-diff --
    ;;
  tracked-generated)
    [ "$#" -eq 1 ] || exit 64
    git ls-files | awk '/(^|\/)(bin|obj)(\/|$)/ { print }'
    ;;
  *)
    echo "Uso: $0 {status|branch|log|diff|diff-names|tracked-generated}" >&2
    exit 64
    ;;
esac
