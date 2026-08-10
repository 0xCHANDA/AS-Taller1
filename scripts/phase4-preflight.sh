#!/usr/bin/env bash
set -euo pipefail

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || exit 2
cd "$repo_root"

old_project="03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj"
new_project="03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj"
failed=0

gate() {
  local label="$1"
  local action="$2"
  local target="$3"
  if scripts/phase4-safe-dotnet.sh "$action" "$target"; then
    echo "$label=PASS"
  else
    echo "$label=FAIL" >&2
    failed=1
  fi
}

gate OLD_CAN_BUILD build "$old_project"
gate OLD_CAN_EXECUTE web-smoke "$old_project"
gate NEW_CAN_BUILD build "$new_project"
gate NEW_CAN_EXECUTE web-smoke "$new_project"

if [ "$failed" -ne 0 ]; then
  echo "PHASE4_PREFLIGHT=BLOCKED" >&2
  exit 1
fi
echo "PHASE4_PREFLIGHT=PASS"
