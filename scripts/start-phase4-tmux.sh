#!/usr/bin/env bash
set -euo pipefail

command -v tmux >/dev/null 2>&1 || {
  echo "TMUX_BLOCKED: tmux no está instalado" >&2
  exit 2
}
repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || exit 2
cd "$repo_root"
branch="$(git branch --show-current)"
[[ "$branch" == agent/phase4-overnight-* ]] || {
  echo "TMUX_BLOCKED: branch incorrecta" >&2
  exit 2
}
session="phase4-$(date +%Y%m%d-%H%M%S)"
tmux new-session -d -s "$session" -c "$repo_root" "scripts/run-phase4-overnight.sh"
echo "TMUX_SESSION=$session"
echo "Adjuntar: tmux attach -t $session"
