![](assets/logo.png)

# karaoke --dev
[![CodeFactor](https://www.codefactor.io/repository/github/karaoke-dev/karaoke/badge)](https://www.codefactor.io/repository/github/karaoke-dev/karaoke)
[![Build status](https://ci.appveyor.com/api/projects/status/07ytm0sei6l5oy08?svg=true)](https://ci.appveyor.com/project/andy840119/karaoke)
[![Crowdin](https://badges.crowdin.net/karaoke-dev/localized.svg)](https://crowdin.com/project/karaoke-dev)
[![Waifu](https://img.shields.io/badge/月子我婆-passed-ff69b4.svg)](https://github.com/karaoke-dev/karaoke)
[![GitHub last commit](https://img.shields.io/github/last-commit/karaoke-dev/karaoke)](https://github.com/karaoke-dev/karaoke/releases)
[![Tagged Release](https://github.com/karaoke-dev/karaoke/workflows/Tagged%20Release/badge.svg)](https://github.com/karaoke-dev/karaoke/releases)
[![dev chat](https://discordapp.com/api/guilds/299006062323826688/widget.png?style=shield)](https://discord.gg/ga2xZXk)
[![Total lines](https://tokei.rs/b1/github/karaoke-dev/karaoke)](https://github.com/karaoke-dev/karaoke)
[![Dashboard](https://img.shields.io/badge/Dashboard-stonks!-informational)](https://www.repotrends.com/karaoke-dev/karaoke)
[![Star History Chart](https://img.shields.io/badge/Stars-0.1k-yellow.svg)](https://seladb.github.io/StarTrack-js/#/preload?r=karaoke-dev,karaoke)
[![airpods pro](https://img.shields.io/badge/Andy's%20airpods%20pro-missing-red.svg)](https://github.com/karaoke-dev/karaoke/issues/1514)
[![airpods 2](https://img.shields.io/badge/Andy's%20airpods%202-missing-red)](https://github.com/karaoke-dev/karaoke/issues/1513)


The source code of the `karaoke` ruleset, running on [osu!lazer](https://github.com/ppy/osu).

## Status

Some features are still under heavy development, but existing features should work fine.

Please note that this project doesn't have much of a [demo](https://github.com/karaoke-dev/sample-beatmap) currently available, and most of the lyrics are written in Japanese, so we recommend looking around this project to find new features instead of actually using it, unless you understand / speak Japanese.

Also, another reason for looking around is that this project is not stable. I hope users don't start using it until most of the bugs are fixed, and the editor has been implemented.

If you run into any problems, you can shoot us an email (support@karaoke.dev) or send me a message on [Discord](https://discord.gg/ga2xZXk). I will typically reply faster on Discord.

And feel free to report any bugs if found.

## How to run this project

See [this tutorial](https://karaoke-dev.github.io/how-to-install/) to get the ruleset from the existing build.

Or you can compile it yourself: `release build` then copy `Packed/osu.Game.Rulesets.Karaoke.dll` into your [ruleset folder](https://github.com/LumpBloom7/sentakki/wiki/Ruleset-installation-guide)

## License

This repo is covered under the [GPL V3](LICENSE) license.
If you plan on using this repo for commercial purposes, please contact us at (support@karaoke.dev) to get permission first.

Using this repo to create, use or using the beatmap format to distribution any `PIRATED`, `unauthorized` karaoke songs/beatmaps is absolutely forbidden.
This repo is trying to make song author(or people has copyright) ability to distribution their songs with karaoke format without any restriction, not for copycat to make thing with copyright issue.

## Thanks to

- [osu!](https://github.com/ppy/osu) and it's [framework](https://github.com/ppy/osu-framework) for karaoke!

- [RhythmKaTTE](http://juna-idler.blogspot.com/2016/05/rhythmkatte-version-01.html), [RhythmicaLyrics](http://suwa.pupu.jp/RhythmicaLyrics.html) and [Aegisub](https://github.com/Aegisub/Aegisub), an open-source software used to create lyrics with time tags.
Parts of the lyrics editor in this ruleset were inspired by them.

- [ニコカラメーカー](http://shinta0806be.ldblog.jp/tag/%E3%83%8B%E3%82%B3%E3%82%AB%E3%83%A9%E3%83%A1%E3%83%BC%E3%82%AB%E3%83%BC), a software to convert `.lrc` files into karaoke video with beautiful text effects.

- [Jetbrain](https://www.jetbrains.com/?from=osu-karaoke), for contributing a free [rider](https://www.jetbrains.com/rider/) license used to developing.

- [Appveyor](https://www.appveyor.com/), [CodeFactor](https://www.codefactor.io/) and [Github action](https://github.com/features/actions) for providing free `CI`/`CD` service.

- [Figma](https://www.figma.com/), for quick creation of assets like logos or icon.

- [Miro](https://miro.com/). Used for flow-charts and deciding how to structure some parts.
