#!/usr/bin/env bash
set -euo pipefail
# Block pushes to protected branches

cmd="$*"
if [[ "$cmd" =~ git[[:space:]]+push ]]; then
  for protected in main master develop; do
    if [[ "$cmd" =~ (^|[[:space:]/"'"])$protected([[:space:]/"'"]|$) ]]; then
      echo "ERROR: push to protected branch '$protected' is not allowed." >&2
      exit 1
    fi
  done
fi
