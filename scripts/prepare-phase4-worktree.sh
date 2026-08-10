#!/usr/bin/env bash
set -euo pipefail

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || {
  echo "WORKTREE_BLOCKED: no es un repositorio Git" >&2
  exit 2
}
cd "$repo_root"

current_branch="$(git branch --show-current)"
[ -n "$current_branch" ] || { echo "WORKTREE_BLOCKED: HEAD separado" >&2; exit 2; }
[[ "$current_branch" != agent/phase4-overnight-* ]] || {
  echo "WORKTREE_BLOCKED: ya está en una rama nocturna" >&2
  exit 2
}
[ -z "$(git status --porcelain=v1 --untracked-files=all)" ] || {
  echo "WORKTREE_BLOCKED: baseline sucio; no se hará stash ni copia silenciosa" >&2
  exit 3
}

timestamp="$(date +%Y%m%d-%H%M)"
branch="agent/phase4-overnight-$timestamp"
parent_dir="$(dirname "$repo_root")"
repo_name="$(basename "$repo_root")"
worktree_path="$parent_dir/${repo_name}-phase4-$timestamp"
[ ! -e "$worktree_path" ] || { echo "WORKTREE_BLOCKED: destino existente" >&2; exit 3; }
git show-ref --verify --quiet "refs/heads/$branch" && {
  echo "WORKTREE_BLOCKED: branch existente" >&2
  exit 3
}

git worktree add -b "$branch" "$worktree_path" HEAD
echo "WORKTREE_PATH=$worktree_path"
echo "BRANCH=$branch"
echo "No stash, commit or push performed."
