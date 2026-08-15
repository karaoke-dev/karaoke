# Tag releases

Pushing a tag to `karaoke-dev/karaoke` triggers [`.github/workflows/release.yml`](../.github/workflows/release.yml) (`Tagged Release`) — the only workflow in this repo that reacts to tags.

## What a release actually produces

**Nothing is published to nuget.org.** There is no `nuget push`, no `.nupkg`, and no publishing credential anywhere in this repo's workflows — unlike the sibling libraries ([osu-framework-font](https://github.com/karaoke-dev/osu-framework-font), [osu-framework-microphone](https://github.com/karaoke-dev/osu-framework-microphone), [LrcParser](https://github.com/karaoke-dev/LrcParser)), whose tagged releases *do* pack and publish to nuget.org. The karaoke ruleset isn't consumed as a library; it's a build people drop into their game.

What a tag produces instead:

1. `dotnet build osu.Game.Rulesets.Karaoke --configuration Release -p:version=<tag>` — the tag string becomes the assembly version.
2. The `CopyCustomContent` target gathers the ruleset dll plus its runtime dependencies into `bin/Release/net8.0/DLLs/` (see [upgrading-dependencies.md](upgrading-dependencies.md#4-dont-forget-copycustomcontent--the-failure-nothing-catches) — this list is hand-maintained and is the usual reason a release is broken while CI was green).
3. That folder is zipped into **`osu.Game.Rulesets.Karaoke.zip`** and attached to a [GitHub release](https://github.com/karaoke-dev/karaoke/releases) — ~10 MB, and the only artefact users get.
4. `gren release --override` regenerates the release notes.

Users download that zip and extract it into their lazer rulesets folder; the [install guide](https://karaoke-dev.github.io/how-to-install/) is the user-facing version of this.

## Tag naming convention

`YYYY.MMDD.PATCH`, no `v` prefix — check the existing history with `git tag -l | sort -V`:

```
2025.0509.0
2025.0615.0
2025.0828.0
2025.0828.1
2025.1030.0
2026.0418.0
```

- `YYYY` — 4-digit year.
- `MMDD` — zero-padded month + day (August 28th → `0828`, not `828`).
- `PATCH` — `0` for the day's first release; bump to `1`, `2`, … for another release the same day (`2025.0828.0` → `2025.0828.1`).

The bare version string matters because it's passed straight through as `-p:version=`.

## Which remote to push the tag to

Push tags to `karaoke` / `upstream` (`karaoke-dev/karaoke`), **not** `origin` (the personal fork) — the opposite of branches, which go to the fork ([opening-a-pull-request.md](opening-a-pull-request.md)). Two reasons:

1. A workflow only runs where the tag is pushed. A tag on the fork runs whatever that fork has, if Actions are even enabled there.
2. Every GitHub-touching step (`create_release`, the asset upload, `gren`) authenticates with `secrets.RELEASE_TOKEN`, which only exists on `karaoke-dev/karaoke`. On a fork those steps fail even with an identical workflow file.

## Tag the newest commit on `master`

The workflow does **not** use the tag that triggered it to pick a version. It computes:

```
CURRENT_TAG=$(git describe --abbrev=0 --tags $(git rev-list --tags --max-count=1))
```

which resolves to the tag on the most recent *tagged commit* in the repo. That equals the tag you just pushed only when you tag the tip of `master`. Tag an older commit — or push an out-of-order tag — and `-p:version=` and `gren --tags=` will silently use a different tag than the release itself, so the release named `2026.0815.0` can ship assemblies stamped with another version.

## Doing it

```
git fetch karaoke master
git tag 2026.0815.0 karaoke/master     # tip of master, name per the convention above
git push karaoke 2026.0815.0
gh run list --repo karaoke-dev/karaoke --workflow=release.yml --limit 1
```

Then confirm the artefact actually landed — a green run isn't enough, since the zip contents depend on the `DLLs/` folder:

```
gh release view 2026.0815.0 --repo karaoke-dev/karaoke --json isDraft,assets
```

You want `osu.Game.Rulesets.Karaoke.zip` present and roughly the size of the previous release. A zip that's suddenly much smaller means `CopyCustomContent` lost an assembly.

## Known quirks of the current workflow

- **The release ends up published, not a draft.** The upload step passes `draft: true`, but the release was already created by the earlier `create_release` step and `gren release --override` rewrites it afterwards. Assume the release is public the moment the run finishes.
- **`gren` overwrites the release body.** The friendly "Thank you for showing interest in this ruleset…" text in the upload step never survives. When `gren` finds nothing to summarise between tags, the published body is literally `*No changelog for this release.*` — which is what [2026.0418.0](https://github.com/karaoke-dev/karaoke/releases/tag/2026.0418.0) shipped with. Edit the release notes by hand afterwards if the release deserves better.
- **Re-pushing a deleted tag re-runs everything**, including `gren --override`, against whatever is then the newest tagged commit.
