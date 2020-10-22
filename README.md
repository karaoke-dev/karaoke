# karaoke!
[![CodeFactor](https://www.codefactor.io/repository/github/karaoke-dev/karaoke/badge)](https://www.codefactor.io/repository/github/karaoke-dev/karaoke)
[![Build status](https://ci.appveyor.com/api/projects/status/07ytm0sei6l5oy08?svg=true)](https://ci.appveyor.com/project/andy840119/karaoke)
[![NuGet](https://img.shields.io/badge/月子我婆-passed-ff69b4.svg)](https://github.com/karaoke-dev/karaoke)
[![GitHub last commit](https://img.shields.io/github/last-commit/karaoke-dev/karaoke)](https://github.com/karaoke-dev/karaoke/releases)
[![Tagged Release](https://github.com/karaoke-dev/karaoke/workflows/Tagged%20Release/badge.svg)](https://github.com/karaoke-dev/karaoke/releases)
[![dev chat](https://discordapp.com/api/guilds/299006062323826688/widget.png?style=shield)](https://discord.gg/ga2xZXk)


The source code of the `karaoke!` ruleset, for [osu!lazer](https://github.com/ppy/osu).

## Status

Some features are still under heavy development, but existing features should work fine.

If you run into any problems, you can shoot me an email (andy840119@gmail.com) or send me a message on [discord](https://discord.gg/ga2xZXk). I will typically reply faster on discord.

And feel free to report any bugs if found.

## How to run this project

See [this tutorial](https://karaoke-dev.github.io/how-to-install/README.html) to get the ruleset from the existing build.

Or you can compile it yourself: `release build` then copy `osu.Game.Rulesets.Karaoke.Packed.dll` into your [ruleset folder](https://github.com/LumpBloom7/sentakki/wiki/Ruleset-installation-guide)

If you want to try the ruleset on another platform like android or ios, you would use [launcher](https://github.com/karaoke-dev/launcher).    
Notice : The launcher is still under development, so it might not work as intended.

## License

This repo is covered under the [GPL V3](LICENSE) license.
If you plan on using this repo for commercial purposes, please contact me at (andy840119@gmail.com) to get permission first.
Using this repo to create a `PIRATED` karaoke ruleset is absolutely forbidden.

## Thanks to

- [osu!](https://github.com/ppy/osu) as it's [framework](https://github.com/ppy/osu-framework) for karaoke!

- [RhythmKaTTE](http://juna-idler.blogspot.com/2016/05/rhythmkatte-version-01.html) and [RhythmicaLyrics](http://suwa.pupu.jp/RhythmicaLyrics.html), an open-source software used to create lyrics with time tags.
Parts of the lyrics editor in this ruleset were inspired by them.

- [ニコカラメーカー](http://shinta0806be.ldblog.jp/tag/%E3%83%8B%E3%82%B3%E3%82%AB%E3%83%A9%E3%83%A1%E3%83%BC%E3%82%AB%E3%83%BC), a software to convert `.lrc` files into karaoke video with beautiful text effects.

- [Jetbrain](https://www.jetbrains.com/?from=osu-karaoke), for contributing a free [rider](https://www.jetbrains.com/rider/) license used to clean-up the code.

- [Appveyor](https://www.appveyor.com/), [CodeFactor](https://www.codefactor.io/) and [Github action](https://github.com/features/actions) for providing free `CI`/`CD` service.
