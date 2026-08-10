#!/usr/bin/env bash
set -euo pipefail

scope="${1:-}"
gate_file="${2:-}"
if [ "$#" -ne 2 ]; then
  echo "Uso: $0 {production|evidence|workbench} 04-evidencia/gates/<gate>.md" >&2
  exit 64
fi
case "$scope" in production|evidence|workbench) ;; *) exit 64 ;; esac
if [[ "$gate_file" != 04-evidencia/gates/* ]] || [[ "$gate_file" == *".."* ]]; then
  echo "CHECKPOINT_BLOCKED: gate inválido" >&2
  exit 2
fi

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || exit 2
cd "$repo_root"
branch="$(git branch --show-current)"
[[ "$branch" == agent/phase4-overnight-* ]] || {
  echo "CHECKPOINT_BLOCKED: branch debe ser agent/phase4-overnight-*" >&2
  exit 2
}
[ -f opencode.jsonc ] && [ -d 03-src/redisenado/HaciendaNEW ] || {
  echo "CHECKPOINT_BLOCKED: workspace inesperado" >&2
  exit 2
}
[ -f "$gate_file" ] && rg -q '(^|:[[:space:]]*)PASS([[:space:]]|$)' "$gate_file" || {
  echo "CHECKPOINT_BLOCKED: falta evidencia explícita PASS" >&2
  exit 2
}

allowed_for_scope() {
  case "$scope:$1" in
    production:03-src/redisenado/HaciendaNEW/Bib_Hacienda/*|production:03-src/redisenado/HaciendaNEW/p_mvcHacienda/*) return 0 ;;
    evidence:03-src/*Verification/*|evidence:03-src/*Characterization/*|evidence:04-evidencia/*) return 0 ;;
    workbench:.opencode/*|workbench:scripts/*|workbench:docs/solid/*|workbench:opencode.jsonc|workbench:README.md) return 0 ;;
  esac
  [ "$1" = "$gate_file" ]
}

changed=0
while IFS= read -r -d '' entry; do
  path="${entry:3}"
  case "$path" in
    *'/logs/'*)
      # Runtime logs are intentionally never staged in a checkpoint.
      continue
      ;;
    *'/bin/'*|*'/obj/'*|*.env|*.env.*|*secrets.json|*.pem|*.key)
      echo "CHECKPOINT_BLOCKED: ruta prohibida $path" >&2
      exit 3
      ;;
  esac
  allowed_for_scope "$path" || {
    echo "CHECKPOINT_BLOCKED: cambio fuera del scope: $path" >&2
    exit 3
  }
  changed=$((changed + 1))
done < <(git status --porcelain=v1 -z --untracked-files=all)
[ "$changed" -gt 0 ] || { echo "CHECKPOINT_BLOCKED: no hay cambios" >&2; exit 2; }

case "$scope" in
  production) git add -- 03-src/redisenado/HaciendaNEW/Bib_Hacienda 03-src/redisenado/HaciendaNEW/p_mvcHacienda "$gate_file" ;;
  evidence) git add -- ':(glob)03-src/**/*Verification/**' ':(glob)03-src/**/*Characterization/**' 04-evidencia ;;
  workbench) git add -- .opencode scripts docs/solid opencode.jsonc README.md "$gate_file" ;;
esac
git diff --cached --check
git -c core.hooksPath=/dev/null -c commit.gpgsign=false commit -m "phase4 checkpoint: $scope"
echo "Local checkpoint created on $branch; no push performed."
