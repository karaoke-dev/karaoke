// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricAutoGenerateChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricAutoGenerateChangeHandler>
{
    protected override bool IncludeAutoGenerator => true;

    # region With reference lyric

    [TestCase(LyricAutoGenerateProperty.DetectReferenceLyric, true)]
    [TestCase(LyricAutoGenerateProperty.DetectLanguage, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateRubyTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateRomajiTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateTimeTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateNotes, false)]
    public void TestCanGenerateWithReferenceLyric(LyricAutoGenerateProperty autoGenerateProperty, bool canGenerate)
    {
        if (autoGenerateProperty == LyricAutoGenerateProperty.DetectReferenceLyric)
        {
            PrepareHitObject(() => new Lyric
            {
                Text = "karaoke"
            }, false);
        }

        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romaji.
            TimeTags = new[] // for auto-generate notes.
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            }
        });

        TriggerHandlerChanged(c =>
        {
            Assert.AreEqual(canGenerate, c.CanGenerate(autoGenerateProperty));
        });
    }

    [TestCase(LyricAutoGenerateProperty.DetectReferenceLyric, true)]
    [TestCase(LyricAutoGenerateProperty.DetectLanguage, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateRubyTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateRomajiTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateTimeTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateNotes, false)]
    public void TestGeneratorNotSupportedLyricsWithReferenceLyric(LyricAutoGenerateProperty autoGenerateProperty, bool canGenerate)
    {
        if (autoGenerateProperty == LyricAutoGenerateProperty.DetectReferenceLyric)
        {
            PrepareHitObject(() => new Lyric
            {
                Text = "karaoke"
            }, false);
        }

        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romaji.
            TimeTags = new[] // for auto-generate notes.
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            }
        });

        TriggerHandlerChanged(c =>
        {
            bool hasNotSupportedLyrics = c.GetGeneratorNotSupportedLyrics(autoGenerateProperty).Any();
            Assert.AreEqual(canGenerate, !hasNotSupportedLyrics);
        });
    }

    [TestCase(LyricAutoGenerateProperty.DetectReferenceLyric, true)]
    [TestCase(LyricAutoGenerateProperty.DetectLanguage, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateRubyTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateRomajiTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateTimeTags, false)]
    [TestCase(LyricAutoGenerateProperty.AutoGenerateNotes, false)]
    public void TestAutoGenerate(LyricAutoGenerateProperty autoGenerateProperty, bool canGenerate)
    {
        if (autoGenerateProperty == LyricAutoGenerateProperty.DetectReferenceLyric)
        {
            PrepareHitObject(() => new Lyric
            {
                Text = "karaoke"
            }, false);
        }

        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romaji.
            TimeTags = new[] // for auto-generate notes.
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            }
        });

        if (canGenerate)
        {
            TriggerHandlerChanged(c => c.AutoGenerate(autoGenerateProperty));
        }
        else
        {
            TriggerHandlerChangedWithChangeForbiddenException(c => c.AutoGenerate(autoGenerateProperty));
        }
    }

    #endregion
}
