#!/usr/bin/env bash
set -euo pipefail

action="${1:-}"
target="${2:-}"

case "$action" in
  build|test|run|web-smoke) ;;
  *) echo "Uso: $0 {build|test|run|web-smoke} 03-src/ruta/proyecto.csproj" >&2; exit 64 ;;
esac

if [ "$#" -ne 2 ] || [[ "$target" = /* ]] || [[ "$target" == *".."* ]] || [[ "$target" != 03-src/* ]]; then
  echo "TOOL_BLOCKED: target debe ser una ruta relativa segura dentro de 03-src" >&2
  exit 64
fi
case "$target" in
  *.csproj|*.sln) ;;
  *) echo "TOOL_BLOCKED: target debe ser .csproj o .sln" >&2; exit 64 ;;
esac
if [[ "$action" == run || "$action" == web-smoke ]] && [[ "$target" != *.csproj ]]; then
  echo "TOOL_BLOCKED: run/web-smoke requiere .csproj" >&2
  exit 64
fi

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || {
  echo "TOOL_BLOCKED: no es un repositorio Git" >&2
  exit 2
}
cd "$repo_root"
[ -f "$target" ] || { echo "TOOL_BLOCKED: no existe $target" >&2; exit 2; }

before_status="$(git status --porcelain=v1 --untracked-files=all)"
tracked_generated_count="$(git ls-files | awk '/(^|\/)(bin|obj)(\/|$)/ { count++ } END { print count+0 }')"
echo "Tracked generated files detected: $tracked_generated_count"

tmp_root="$(mktemp -d /tmp/as-taller1-phase4.XXXXXX)"
cleanup() {
  case "$tmp_root" in
    /tmp/as-taller1-phase4.*) rm -rf -- "$tmp_root" ;;
  esac
}
trap cleanup EXIT

mkdir -p "$tmp_root/repo"
cp -a --reflink=auto 03-src "$tmp_root/repo/"
tmp_target="$tmp_root/repo/$target"
mkdir -p "$tmp_root/home" "$tmp_root/dotnet-home" "$tmp_root/tmp" "$tmp_root/nuget-http"

command -v bwrap >/dev/null 2>&1 || {
  echo "TOOL_BLOCKED: bubblewrap es obligatorio para aislar MSBuild" >&2
  exit 2
}
global_packages="$(dotnet nuget locals global-packages --list | sed -n 's/^global-packages: //p')"
[ -n "$global_packages" ] && [ -d "$global_packages" ] || {
  echo "TOOL_BLOCKED: no se pudo resolver el cache local de NuGet" >&2
  exit 2
}
sandbox=(
  bwrap --die-with-parent --unshare-net
  --ro-bind / /
  --tmpfs /tmp
  --dir "$tmp_root"
  --bind "$tmp_root" "$tmp_root"
  --dev /dev --proc /proc
  --setenv HOME "$tmp_root/home"
  --setenv DOTNET_CLI_HOME "$tmp_root/dotnet-home"
  --setenv NUGET_PACKAGES "$global_packages"
  --setenv NUGET_HTTP_CACHE_PATH "$tmp_root/nuget-http"
  --setenv TMPDIR "$tmp_root/tmp"
  --chdir "$tmp_root/repo"
)

export DOTNET_ROLL_FORWARD=Major
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

nuget_config="$tmp_root/NuGet.Config"
printf '%s\n' '<?xml version="1.0" encoding="utf-8"?>' '<configuration><packageSources><clear /></packageSources></configuration>' > "$nuget_config"
# Resolves only SDK/project references and packages already present in the local
# NuGet cache. A missing package fails because all remote sources are cleared.
"${sandbox[@]}" dotnet restore "$tmp_target" --configfile "$nuget_config" --ignore-failed-sources

case "$action" in
  build)
    "${sandbox[@]}" dotnet build "$tmp_target" --no-restore
    ;;
  test)
    "${sandbox[@]}" dotnet test "$tmp_target" --no-restore
    ;;
  run)
    timeout 120s "${sandbox[@]}" dotnet run --project "$tmp_target" --no-restore
    ;;
  web-smoke)
    "${sandbox[@]}" dotnet build "$tmp_target" --no-restore
    web_log="$tmp_root/web-smoke.log"
    set +e
    timeout 30s "${sandbox[@]}" dotnet run --project "$tmp_target" --no-build --no-restore -- --urls http://127.0.0.1:0 >"$web_log" 2>&1
    web_exit=$?
    set -e
    sed -n '1,160p' "$web_log"
    if ! rg -q 'Now listening on:|Application started\.' "$web_log"; then
      echo "TOOL_BLOCKED: la aplicación no demostró inicio observable (exit $web_exit)" >&2
      exit 1
    fi
    echo "Application startup observable: PASS"
    ;;
esac

after_status="$(git status --porcelain=v1 --untracked-files=all)"
if [ "$before_status" != "$after_status" ]; then
  echo "TOOL_BLOCKED: el build/test alteró el workspace real" >&2
  exit 3
fi
echo "Workspace source/status unchanged: PASS"
