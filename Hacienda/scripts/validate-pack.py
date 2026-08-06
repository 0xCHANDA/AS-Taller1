#!/usr/bin/env python3
from pathlib import Path
import json, re, sys

root = Path(__file__).resolve().parents[1]
errors = []

try:
    json.loads((root / "opencode.jsonc").read_text())
except Exception as exc:
    errors.append(f"opencode.jsonc inválido: {exc}")

for skill in (root / ".opencode/skills").glob("*/SKILL.md"):
    text = skill.read_text()
    match = re.match(r"^---\n(.*?)\n---\n", text, re.S)
    if not match:
        errors.append(f"Sin frontmatter: {skill}")
        continue
    front = match.group(1)
    name_match = re.search(r"^name:\s*(.+)$", front, re.M)
    desc_match = re.search(r"^description:\s*(.+)$", front, re.M)
    if not name_match or not desc_match:
        errors.append(f"Falta name/description: {skill}")
        continue
    name = name_match.group(1).strip()
    if name != skill.parent.name:
        errors.append(f"Nombre no coincide con carpeta: {skill}")
    if not re.fullmatch(r"[a-z0-9]+(?:-[a-z0-9]+)*", name):
        errors.append(f"Nombre inválido: {name}")

for agent in (root / ".opencode/agents").glob("*.md"):
    text = agent.read_text()
    if not text.startswith("---\n") or "\n---\n" not in text[4:]:
        errors.append(f"Frontmatter de agente inválido: {agent}")
    if "description:" not in text.split("---", 2)[1]:
        errors.append(f"Agente sin description: {agent}")

for command in (root / ".opencode/commands").glob("*.md"):
    text = command.read_text()
    if not text.startswith("---\n") or "description:" not in text:
        errors.append(f"Comando inválido: {command}")

if errors:
    print("FAIL")
    for error in errors:
        print("-", error)
    sys.exit(1)

print("PASS")
print(f"Agents: {len(list((root / '.opencode/agents').glob('*.md')))}")
print(f"Skills: {len(list((root / '.opencode/skills').glob('*/SKILL.md')))}")
print(f"Commands: {len(list((root / '.opencode/commands').glob('*.md')))}")
