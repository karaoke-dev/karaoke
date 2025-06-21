// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics;

public class LyricEditorTest
{
    [TestCase(LyricEditorLayout.List, LyricEditorLayout.List, LyricEditorLayout.List)]
    [TestCase(LyricEditorLayout.Compose, LyricEditorLayout.Compose, LyricEditorLayout.Compose)]
    [TestCase(LyricEditorLayout.List | LyricEditorLayout.Compose, LyricEditorLayout.List, LyricEditorLayout.List)]
    [TestCase(LyricEditorLayout.List | LyricEditorLayout.Compose, LyricEditorLayout.Compose, LyricEditorLayout.Compose)]
    [TestCase(LyricEditorLayout.List, LyricEditorLayout.Compose, LyricEditorLayout.List)] // should use the support layout if prefer layout is not matched.
    [TestCase(LyricEditorLayout.Compose, LyricEditorLayout.List, LyricEditorLayout.Compose)]
    public void TestGetSuitableLayout(LyricEditorLayout supportedLayout, LyricEditorLayout preferLayout, LyricEditorLayout actualLayout)
    {
        var expectedLayout = LyricEditor.GetSuitableLayout(supportedLayout, preferLayout);
        Assert.That(actualLayout, Is.EqualTo(expectedLayout));
    }
}
