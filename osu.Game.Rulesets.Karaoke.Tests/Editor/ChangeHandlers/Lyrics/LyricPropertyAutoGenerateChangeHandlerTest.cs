// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

/// <summary>
/// This test is focus on make sure that:
/// If the <see cref="Lyric.ReferenceLyric"/> in the <see cref="Lyric"/> is not empty.
/// <see cref="ILyricPropertyAutoGenerateChangeHandler"/> should be able to change the property.
/// </summary>
/// <typeparam name="TChangeHandler"></typeparam>
[TestFixture(typeof(LyricReferenceChangeHandler))]
[TestFixture(typeof(LyricLanguageChangeHandler))]
[TestFixture(typeof(LyricRubyTagsChangeHandler))]
[TestFixture(typeof(LyricRomajiTagsChangeHandler))]
[TestFixture(typeof(LyricTimeTagsChangeHandler))]
[TestFixture(typeof(LyricNotesChangeHandler))]
public partial class LyricPropertyAutoGenerateChangeHandlerTest<TChangeHandler> : LyricPropertyChangeHandlerTest<TChangeHandler>
    where TChangeHandler : LyricPropertyChangeHandler, ILyricPropertyAutoGenerateChangeHandler, new()
{
    protected override bool IncludeAutoGenerator => true;

    # region With reference lyric

    [Test]
    public void TestCanGenerateWithReferenceLyric()
    {
        bool lyricReferenceChangeHandler = isLyricReferenceChangeHandler();

        if (lyricReferenceChangeHandler)
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
            Assert.AreEqual(lyricReferenceChangeHandler, c.CanGenerate());
        });
    }

    [Test]
    public void TestGeneratorNotSupportedLyricsWithReferenceLyric()
    {
        bool lyricReferenceChangeHandler = isLyricReferenceChangeHandler();

        if (lyricReferenceChangeHandler)
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
            bool hasNotSupportedLyrics = c.GetGeneratorNotSupportedLyrics().Any();
            Assert.AreEqual(lyricReferenceChangeHandler, !hasNotSupportedLyrics);
        });
    }

    [Test]
    public void TestAutoGenerate()
    {
        bool lyricReferenceChangeHandler = isLyricReferenceChangeHandler();

        if (lyricReferenceChangeHandler)
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

        if (lyricReferenceChangeHandler)
        {
            TriggerHandlerChanged(c => c.AutoGenerate());
        }
        else
        {
            TriggerHandlerChangedWithChangeForbiddenException(c => c.AutoGenerate());
        }
    }

    private bool isLyricReferenceChangeHandler()
        => typeof(TChangeHandler) == typeof(LyricReferenceChangeHandler);

    #endregion
}
