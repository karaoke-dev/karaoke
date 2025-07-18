﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Content;

public class SingleLyricEditorTest
{
    [Test]
    public void TestLockMessage()
    {
        var lyric = new Lyric();
        Assert.That(InteractableLyric.GetLyricPropertyLockedReason(lyric, LyricEditorMode.View), Is.Null);
    }
}
