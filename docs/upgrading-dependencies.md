# Upgrading dependencies

The recurring "update the important packages to the latest, fix whatever breaks, one commit per category" task. This repo has **no `.github/dependabot.yml`** — every bump is done by hand, which is why the process is written down.

## 1. Where versions live

| File | What it holds |
| --- | --- |
| [`osu.Game.Rulesets.Karaoke.csproj`](../osu.Game.Rulesets.Karaoke/osu.Game.Rulesets.Karaoke.csproj) | runtime dependencies (`ppy.osu.Game`, the karaoke-dev packages, Lucene, SixLabors, …) |
| [`osu.Game.Rulesets.Karaoke.Tests.csproj`](../osu.Game.Rulesets.Karaoke.Tests/osu.Game.Rulesets.Karaoke.Tests.csproj) | `NUnit`, `NUnit3TestAdapter`, `Microsoft.NET.Test.Sdk`, `Appveyor.TestLogger` |
| [`osu.Game.Rulesets.Karaoke.Architectures.csproj`](../osu.Game.Rulesets.Karaoke.Architectures/osu.Game.Rulesets.Karaoke.Architectures.csproj) | `TngTech.ArchUnitNET` + its own `Microsoft.NET.Test.Sdk` — **bump this in lockstep with the Tests project**, a version split between the two is easy to miss |
| [`Directory.Build.props`](../Directory.Build.props) | `Microsoft.CodeAnalysis.BannedApiAnalyzers`, plus the repo-wide `TreatWarningsAsErrors` / `Nullable` settings |
| [`.config/dotnet-tools.json`](../.config/dotnet-tools.json) | `jetbrains.resharper.globaltools` (`jb`), `codefilesanity`, `nvika`, `ppy.localisationanalyser.tools` |

The tool versions are not cosmetic: the format workflow does `dotnet tool restore` and runs those exact versions, so bumping `jetbrains.resharper.globaltools` can light up brand-new inspections that fail CI without a single line of product code changing.

## 2. Commit grouping

One commit per category, each building and testing clean before starting the next. The convention the history follows (branch `update-package-to-latest`, e.g. PRs #2334, #2336, #2338, #2339):

- `ppy.osu.Game` on its own — *"Update osu.game to the latest."*
- test tooling — *"Upgrade test framework to the latest."*, *"Upgrade the test SDK to the latest."*
- drawing libraries (`SixLabors.*`) — *"Upgrade drawing library to the latest."*
- localisation analyser + tool — *"Update localisation library to the latest."*
- leftovers on their own commit — *"Upgrade Encoding.web package. to the latest."*

Splitting this way keeps a bisect useful when a bump turns out to be the thing that broke gameplay or the editor.

## 3. `ppy.osu.Game` is the one that breaks things

There is **no direct `ppy.osu.Framework` reference** — the framework arrives transitively through `ppy.osu.Game`. Bump `ppy.osu.Game` and let it pull the matching framework; don't add a separate pin that can drift out of sync with what osu! itself was built against.

A "latest" bump can cross many months of upstream churn. Before assuming a compile error is *your* bug, go read what upstream did:

```
gh api "search/code?q=repo:ppy/osu+filename:<Name>.cs"     # where did the type move to?
gh api "repos/ppy/osu/commits?path=<file>"                 # why did it change?
```

Sometimes the answer is that there is nothing to port. In the `2026.408.0` bump, the osu!-side base components behind the karaoke beatmap-info wedge and beatmap-info graph were deleted upstream, so the karaoke subclasses **and their visual tests** were deleted too (`ea52110`, `4b732b8`) rather than re-implemented. Deleting is the correct fix when the upstream base is gone — don't reconstruct a component that osu! no longer has a place for.

### karaoke-dev's own packages

These come from sibling repos, so "latest on nuget.org" is only as new as the last release *there*. If you need a fix, release it in that repo first, then bump the version here:

| Package | Repo |
| --- | --- |
| `osu.Framework.KaraokeFont` | [karaoke-dev/osu-framework-font](https://github.com/karaoke-dev/osu-framework-font) |
| `osu.Framework.Microphone` | [karaoke-dev/osu-framework-microphone](https://github.com/karaoke-dev/osu-framework-microphone) |
| `LrcParser` | [karaoke-dev/LrcParser](https://github.com/karaoke-dev/LrcParser) |
| `osu.Game.Rulesets.Karaoke.Resources` | [karaoke-dev/karaoke-resources](https://github.com/karaoke-dev/karaoke-resources) |
| `LanguageDetection.karaoke-dev` | [karaoke-dev/language-detection](https://github.com/karaoke-dev/language-detection) |

Note the asymmetry with this repo: those publish `.nupkg` to nuget.org from their own tagged releases, whereas a tag here produces a downloadable build instead — see [tag-releases.md](tag-releases.md).

## 4. Don't forget `CopyCustomContent` — the failure nothing catches

`osu.Game.Rulesets.Karaoke.csproj` ends with a `CopyCustomContent` target that runs after a **Release** build and copies a *hand-maintained list* of assemblies into `bin/Release/net8.0/DLLs/`. That folder is exactly what [`release.yml`](../.github/workflows/release.yml) zips and ships to users.

If an upgrade adds, removes, or renames a runtime assembly — a package splitting out a new dll, or a transitive one like `NWaves`, `J2N`, or `Lucene.Net.Analysis.Common` disappearing — **nothing in build or test notices**, because both run against `bin/`, not `DLLs/`. The first symptom is a user unzipping the release into their rulesets folder and lazer failing to load the ruleset.

So after any dependency change that touches the main project:

```
dotnet build osu.Game.Rulesets.Karaoke --configuration Release
ls osu.Game.Rulesets.Karaoke/bin/Release/net8.0/DLLs
```

and reconcile the `InputAssemblies` list against what the packages actually bring in.

## 5. Verification checklist

```
dotnet restore
dotnet build --configuration Release   # TreatWarningsAsErrors is on — a new warning is a failure
dotnet test                            # full suite; confirm N/N passing, not just "build succeeded"
dotnet list package --outdated         # re-run after all bumps; should be empty aside from deliberate skips
```

Then run the format/inspection commands from [opening-a-pull-request.md](opening-a-pull-request.md) — an upgraded ReSharper or analyzer version routinely produces new inspections that only that job catches, and it's the most common reason an otherwise-working upgrade PR goes red.

**NUnit:** this repo is already on NUnit 4 (`4.5.1`) with `NUnit3TestAdapter` 6.x, and the classic-assert migration is finished — every assertion in the repo uses the constraint model (`Assert.That(actual, Is.EqualTo(expected))`), with no `ClassicAssert` shim anywhere. Keep it that way; don't reintroduce `Assert.AreEqual` / `Assert.Throws<T>` on the way through an upgrade.

## 6. Committing

One commit per category (section 2), each verified before the next, then open the PR — see [opening-a-pull-request.md](opening-a-pull-request.md).
