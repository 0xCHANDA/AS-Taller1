#!/usr/bin/env bash
set -euo pipefail

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || exit 2
cd "$repo_root"

agents=(solid-orchestrator codebase-cartographer srp-auditor ocp-auditor lsp-auditor isp-auditor dip-auditor architecture-auditor refactor-planner refactor-implementer refactor-implementer-fallback phase4-evidence-engineer phase4-evidence-engineer-fallback phase4-readonly-fallback test-guardian adversarial-reviewer)
tmp_dir="$(mktemp -d /tmp/as-taller1-config.XXXXXX)"
cleanup() { case "$tmp_dir" in /tmp/as-taller1-config.*) rm -rf -- "$tmp_dir" ;; esac; }
trap cleanup EXIT

opencode debug config --pure > "$tmp_dir/config.json"
for agent in "${agents[@]}"; do
  opencode debug agent "$agent" --pure > "$tmp_dir/$agent.json"
done

python3 - "$tmp_dir" <<'PY'
import json, pathlib, sys

root = pathlib.Path(sys.argv[1])
resolved_agents = json.loads((root / "config.json").read_text())["agent"]
required_steps = {
    "solid-orchestrator": 160, "codebase-cartographer": 30,
    "srp-auditor": 35, "ocp-auditor": 40, "lsp-auditor": 45,
    "isp-auditor": 35, "dip-auditor": 45, "architecture-auditor": 45,
    "refactor-planner": 45, "refactor-implementer": 60,
    "refactor-implementer-fallback": 60,
    "phase4-evidence-engineer": 65,
    "phase4-evidence-engineer-fallback": 65,
    "phase4-readonly-fallback": 45, "test-guardian": 45,
    "adversarial-reviewer": 50,
}
required_models = {
    "refactor-implementer": ("opencode-go", "kimi-k2.7-code"),
    "refactor-implementer-fallback": ("openai", "gpt-5.6-sol"),
    "phase4-evidence-engineer": ("opencode-go", "deepseek-v4-pro"),
    "phase4-evidence-engineer-fallback": ("openai", "gpt-5.6-sol"),
    "phase4-readonly-fallback": ("openai", "gpt-5.6-sol"),
    "test-guardian": ("opencode-go", "deepseek-v4-pro"),
    "adversarial-reviewer": ("opencode-go", "glm-5.2"),
}

def rules(agent):
    return json.loads((root / f"{agent}.json").read_text())["permission"]

def ordered_structure(value):
    if isinstance(value, dict):
        return ("dict", tuple((key, ordered_structure(item)) for key, item in value.items()))
    if isinstance(value, list):
        return ("list", tuple(ordered_structure(item) for item in value))
    return ("value", value)

def resolved_permission(agent):
    return ordered_structure(resolved_agents[agent]["permission"])

def last_general(rs, permission):
    matches = [r for r in rs if r["permission"] == permission and r.get("pattern") == "*"]
    return matches[-1]["action"] if matches else None

errors = []
for agent, steps in required_steps.items():
    data = json.loads((root / f"{agent}.json").read_text())
    rs = data["permission"]
    if data.get("steps") != steps:
        errors.append(f"{agent}: steps {data.get('steps')} != {steps}")
    if agent in required_models:
        model = data.get("model") or {}
        actual = (model.get("providerID"), model.get("modelID"))
        if actual != required_models[agent]:
            errors.append(f"{agent}: model {actual} != {required_models[agent]}")
    for permission in ("question", "external_directory", "doom_loop", "webfetch", "websearch"):
        if last_general(rs, permission) != "deny":
            errors.append(f"{agent}: {permission} no termina en deny")
    if agent != "solid-orchestrator" and last_general(rs, "task") != "deny":
        errors.append(f"{agent}: puede delegar")

for agent in ("codebase-cartographer", "srp-auditor", "ocp-auditor", "lsp-auditor", "isp-auditor", "dip-auditor", "architecture-auditor", "refactor-planner", "phase4-readonly-fallback", "test-guardian", "adversarial-reviewer"):
    if last_general(rules(agent), "edit") != "deny":
        errors.append(f"{agent}: no es read-only")

impl = rules("refactor-implementer")
impl_fallback = rules("refactor-implementer-fallback")
evid = rules("phase4-evidence-engineer")
evid_fallback = rules("phase4-evidence-engineer-fallback")
def final_rules(rs, permission):
    return [(r.get("pattern"), r["action"]) for r in rs if r["permission"] == permission]

impl_edit = final_rules(impl, "edit")
evid_edit = final_rules(evid, "edit")
if impl_edit[-3:] != [
    ("*", "deny"),
    ("03-src/redisenado/HaciendaNEW/Bib_Hacienda/**", "allow"),
    ("03-src/redisenado/HaciendaNEW/p_mvcHacienda/**", "allow"),
]:
    errors.append("refactor-implementer: scope edit inesperado")
if not evid_edit or evid_edit[-5] != ("*", "deny"):
    errors.append("phase4-evidence-engineer: falta default deny edit")
if any("Bib_Hacienda/**" in (p or "") or "p_mvcHacienda/**" in (p or "") for p, a in evid_edit if a == "allow"):
    errors.append("phase4-evidence-engineer: puede editar producción")
if resolved_permission("refactor-implementer") != resolved_permission("refactor-implementer-fallback"):
    errors.append("refactor-implementer-fallback: permisos no son idénticos al agente preferido")
if resolved_permission("phase4-evidence-engineer") != resolved_permission("phase4-evidence-engineer-fallback"):
    errors.append("phase4-evidence-engineer-fallback: permisos no son idénticos al agente preferido")

readonly_fallback = rules("phase4-readonly-fallback")
guardian = rules("test-guardian")
if final_rules(readonly_fallback, "skill") != final_rules(guardian, "skill"):
    errors.append("phase4-readonly-fallback: skills exceden test-guardian")
if final_rules(readonly_fallback, "bash") != final_rules(guardian, "bash"):
    errors.append("phase4-readonly-fallback: bash excede test-guardian")

orchestrator_tasks = final_rules(rules("solid-orchestrator"), "task")
for fallback in ("refactor-implementer-fallback", "phase4-evidence-engineer-fallback", "phase4-readonly-fallback"):
    if (fallback, "allow") not in orchestrator_tasks:
        errors.append(f"solid-orchestrator: no puede invocar {fallback}")

for agent in required_steps:
    rs = rules(agent)
    bash = final_rules(rs, "bash")
    last_star = max((i for i, (p, _) in enumerate(bash) if p == "*"), default=-1)
    if last_star < 0 or bash[last_star][1] != "deny":
        errors.append(f"{agent}: bash sin default deny")
    if any(action == "ask" for _, action in bash[last_star + 1:]):
        errors.append(f"{agent}: ASK bash efectivo")

if errors:
    print("CONFIG VALIDATION: FAIL")
    for error in errors:
        print("-", error)
    raise SystemExit(1)
print(f"CONFIG VALIDATION: PASS ({len(required_steps)} agents)")
PY

echo "Models configured:"
opencode models opencode-go --pure | awk '/opencode-go\/(deepseek-v4-pro|kimi-k2.7-code|glm-5.2)$/ {print}'
opencode models openai --pure | awk '/openai\/gpt-5.6-sol$/ {print}'
echo "No ASK blockers in validated effective rules."
