﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Utils;

public class ValueChangedEventUtilsTest
{
    [Test]
    public void TestLyricChangedWithSameLyric()
    {
        var lyric1 = new Lyric
        {
            Text = "lyric 1",
        };

        var oldCaret = new ClickingCaretPosition(lyric1);
        var newCaret = new ClickingCaretPosition(lyric1);

        Assert.That(ValueChangedEventUtils.LyricChanged(new ValueChangedEvent<ICaretPosition?>(oldCaret, newCaret)), Is.False);
    }

    [Test]
    public void TestLyricChangedWithDifferentLyric()
    {
        var lyric1 = new Lyric
        {
            Text = "lyric 1",
        };

        var lyric2 = new Lyric
        {
            Text = "lyric 2",
        };

        var oldCaret = new ClickingCaretPosition(lyric1);
        var newCaret = new ClickingCaretPosition(lyric2);

        Assert.That(ValueChangedEventUtils.LyricChanged(new ValueChangedEvent<ICaretPosition?>(oldCaret, newCaret)));
    }

    [Test]
    public void TestLyricChangedWithSameLyricButDifferentCaretPosition()
    {
        var lyric1 = new Lyric
        {
            Text = "lyric 1",
        };

        var oldCaret = new ClickingCaretPosition(lyric1);
        var newCaret = new RecordingTimeTagCaretPosition(lyric1, new TimeTag(new TextIndex(1)));

        Assert.That(ValueChangedEventUtils.LyricChanged(new ValueChangedEvent<ICaretPosition?>(oldCaret, newCaret)), Is.False);
    }

    [Test]
    public void TestEditModeChangedWithDefaultValue()
    {
        var oldMode = default(EditorModeWithEditStep);
        var newMode = new EditorModeWithEditStep
        {
            Mode = LyricEditorMode.View,
        };

        Assert.That(ValueChangedEventUtils.EditModeChanged(new ValueChangedEvent<EditorModeWithEditStep>(oldMode, newMode)));
    }

    [Test]
    public void TestEditModeChanged()
    {
        var oldMode = new EditorModeWithEditStep
        {
            Mode = LyricEditorMode.View,
        };
        var newMode = new EditorModeWithEditStep
        {
            Mode = LyricEditorMode.View,
        };

        Assert.That(ValueChangedEventUtils.EditModeChanged(new ValueChangedEvent<EditorModeWithEditStep>(oldMode, newMode)), Is.False);
    }
}
