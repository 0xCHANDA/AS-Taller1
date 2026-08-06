#!/usr/bin/env bash
set -euo pipefail

SOURCE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
TARGET_DIR="${1:-}"

if [[ -z "$TARGET_DIR" ]]; then
  echo "Uso: $0 /ruta/al/repositorio" >&2
  exit 2
fi

if [[ ! -d "$TARGET_DIR" ]]; then
  echo "La ruta no existe: $TARGET_DIR" >&2
  exit 2
fi

mkdir -p "$TARGET_DIR/.opencode" "$TARGET_DIR/docs/solid"

copy_with_backup() {
  local src="$1"
  local dst="$2"
  if [[ -e "$dst" ]]; then
    cp -a "$dst" "$dst.bak.$(date +%Y%m%d%H%M%S)"
  fi
  cp -a "$src" "$dst"
}

copy_with_backup "$SOURCE_DIR/AGENTS.md" "$TARGET_DIR/AGENTS.md"
copy_with_backup "$SOURCE_DIR/opencode.jsonc" "$TARGET_DIR/opencode.jsonc"
cp -a "$SOURCE_DIR/.opencode/." "$TARGET_DIR/.opencode/"
cp -a "$SOURCE_DIR/docs/solid/." "$TARGET_DIR/docs/solid/"

echo "Instalado en: $TARGET_DIR"
echo "Revise opencode.jsonc y ejecute /models para confirmar el identificador del modelo."
