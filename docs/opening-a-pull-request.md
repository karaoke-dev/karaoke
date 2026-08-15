# Opening a pull request

The canonical repo is [`karaoke-dev/karaoke`](https://github.com/karaoke-dev/karaoke). **Every PR targets `karaoke-dev/karaoke` as its base**, regardless of where the branch itself lives.

## Remotes

Contributors work from a fork. A maintainer clone typically ends up with more than one remote pointing at the same place — check `git remote -v` before pushing rather than assuming a name:

| Remote | Repo | Used for |
| --- | --- | --- |
| `karaoke` / `upstream` | `karaoke-dev/karaoke` | fetching `master`, pushing **release tags** (see [tag-releases.md](tag-releases.md)) |
| `origin` | `andy840119/karaoke` (personal fork) | pushing **branches** |

Don't confuse "push to `origin`" with "PR against `origin`" — those are different repos. A PR whose **base repo** is the fork is wrong: CI defined on `karaoke-dev/karaoke` won't gate it, and merging it only updates the fork. The base repo must always be `karaoke-dev/karaoke`; only the head branch lives on the fork.

Push access to the canonical repo doesn't change this — recent maintainer PRs (#2334, #2336, #2338, #2339) all come from `andy840119/<branch>`.

## Workflow

Branch off `master`, never PR from `master` itself:

```
git fetch karaoke master
git checkout -b <branch> karaoke/master
# ...make changes, commit...
git push -u origin <branch>
gh pr create --repo karaoke-dev/karaoke --base master --head andy840119:<branch>
```

The `--head <owner>:<branch>` form is required for a cross-repo PR — a bare `--head <branch>` looks for the branch inside `karaoke-dev/karaoke` itself and fails (or worse, silently targets a same-named branch that happens to exist there).

Branch names are kebab-case and describe the change (`update-package-to-latest`, `patch/ruleset-icon`). Keep *Allow edits from maintainers* checked. Unlike the osu! project, force-push is allowed here — use it to clean up a branch, and rebase onto `master` if the PR goes stale or conflicts.

## What CI will check

Two workflows run on every PR, and both must be green:

- **`.NET Core`** ([`ci.yml`](../.github/workflows/ci.yml)) — `dotnet build --configuration Release` then `dotnet test`. Triggers on `pull_request: branches: [master]`, so it only runs once the base is correctly `master` on the canonical repo.
- **`Format check on pull request`** ([`dotnet-format.yml`](../.github/workflows/dotnet-format.yml)) — `dotnet build -c Debug -warnaserror`, then `CodeFileSanity`, then ReSharper `InspectCode` piped through `NVika --treatwarningsaserrors`. This is the step that usually fails on an otherwise-working PR.

`Directory.Build.props` sets `TreatWarningsAsErrors` and `Nullable=enable` repo-wide, so a new warning is a build failure, not a note. Reproduce the format job locally before pushing:

```
dotnet tool restore
dotnet build -c Debug -warnaserror osu.Game.Rulesets.Karaoke.sln
dotnet codefilesanity
dotnet jb inspectcode osu.Game.Rulesets.Karaoke.sln --no-build -f=xml --output=inspectcodereport.xml --caches-home=inspectcode --verbosity=WARN
dotnet nvika parsereport inspectcodereport.xml --treatwarningsaserrors
```

The first `inspectcode` run is slow; the `--caches-home=inspectcode` directory (gitignored, and cached in CI) makes later runs much faster.

## If CI doesn't run on the PR

Check the PR's base repo first — `gh pr view <n> --json baseRepository`. Actions being disabled on a personal fork doesn't matter when the base is `karaoke-dev/karaoke`, because the workflows run there.
