// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics
{
    public class LyricEditorTest
    {
        [TestCase(LyricEditorLayout.Preview, LyricEditorLayout.Preview, LyricEditorLayout.Preview)]
        [TestCase(LyricEditorLayout.Detail, LyricEditorLayout.Detail, LyricEditorLayout.Detail)]
        [TestCase(LyricEditorLayout.Preview | LyricEditorLayout.Detail, LyricEditorLayout.Preview, LyricEditorLayout.Preview)]
        [TestCase(LyricEditorLayout.Preview | LyricEditorLayout.Detail, LyricEditorLayout.Detail, LyricEditorLayout.Detail)]
        [TestCase(LyricEditorLayout.Preview, LyricEditorLayout.Detail, LyricEditorLayout.Preview)] // should use the support layout if prefer layout is not matched.
        [TestCase(LyricEditorLayout.Detail, LyricEditorLayout.Preview, LyricEditorLayout.Detail)]
        public void TestGetSuitableLayout(LyricEditorLayout supportedLayout, LyricEditorLayout preferLayout, LyricEditorLayout actualLayout)
        {
            var expectedLayout = LyricEditor.GetSuitableLayout(supportedLayout, preferLayout);
            Assert.AreEqual(expectedLayout, actualLayout);
        }
    }
}
