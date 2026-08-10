#!/usr/bin/env bash
set -euo pipefail

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || exit 2
cd "$repo_root"
mode="${1:-run}"
case "$mode" in run|--smoke) ;; *) echo "Uso: $0 [--smoke]" >&2; exit 64 ;; esac
branch="$(git branch --show-current)"
[[ "$branch" == agent/phase4-overnight-* ]] || {
  echo "RUN_BLOCKED: branch debe ser agent/phase4-overnight-*" >&2
  exit 2
}
[ -z "$(git status --porcelain=v1 --untracked-files=all)" ] || {
  echo "RUN_BLOCKED: el worktree debe iniciar limpio" >&2
  exit 3
}

scripts/validate-phase4-workbench.sh
[ -f .opencode/phase4-overnight.md ] && [ -f .opencode/commands/phase4-overnight.md ] || {
  echo "RUN_BLOCKED: falta prompt/command versionado" >&2
  exit 3
}

scripts/phase4-preflight.sh

if [ "$mode" = "--smoke" ]; then
  echo "RUNNER SMOKE: PASS"
  echo "Branch, clean baseline, resolved config, models and prompts validated."
  exit 0
fi

timestamp="$(date +%Y%m%d-%H%M%S)"
log_dir="04-evidencia/logs/$timestamp"
mkdir -p "$log_dir"
stdout_log="$log_dir/opencode.stdout.log"
stderr_log="$log_dir/opencode.stderr.log"
status_file="$log_dir/exit-code.txt"

set +e
opencode run --pure --command phase4-overnight --agent solid-orchestrator --auto \
  >"$stdout_log" 2>"$stderr_log"
exit_code=$?
set -e
printf '%s\n' "$exit_code" > "$status_file"

if opencode stats --help >/dev/null 2>&1; then
  opencode stats --pure --project "$repo_root" --models > "$log_dir/opencode-stats.txt" 2>&1 || true
fi

echo "PHASE4_EXIT_CODE=$exit_code"
echo "PHASE4_LOG_DIR=$log_dir"
echo "No push performed."
exit "$exit_code"
