// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

/// <summary>
/// This test is focus on make sure that:
/// If the <see cref="Lyric.ReferenceLyric"/> in the <see cref="Lyric"/> is not empty.
/// <see cref="ILyricPropertyAutoGenerateChangeHandler"/> should be able to change the property.
/// </summary>
public partial class LyricPropertyAutoGenerateChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricPropertyAutoGenerateChangeHandler>
{
    protected override bool IncludeAutoGenerator => true;

    #region Reference lyric

    [Test]
    public void TestDetectReferenceLyric()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        }, false);

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.DetectReferenceLyric));

        AssertSelectedHitObject(h =>
        {
            Assert.IsNotNull(h.ReferenceLyric);
            Assert.IsTrue(h.ReferenceLyricConfig is SyncLyricConfig);
        });
    }

    [Test]
    public void TestDetectReferenceLyricWithNonSupportedLyric()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        }, false);

        PrepareHitObject(() => new Lyric
        {
            Text = "???",
        });

        TriggerHandlerChangedWithException<DetectorNotSupportedException>(c => c.AutoGenerate(AutoGenerateType.DetectReferenceLyric));
    }

    #endregion

    #region Language

    [Test]
    public void TestDetectLanguage()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.DetectLanguage));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(new CultureInfo("ja"), h.Language);
        });
    }

    [Test]
    public void TestDetectLanguageWithNonSupportedLyric()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "???",
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.DetectLanguage));

        AssertSelectedHitObject(h =>
        {
            Assert.IsNull(h.Language);
        });
    }

    #endregion

    #region Ruby

    [Test]
    public void TestAutoGenerateRubyTags()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "風",
            Language = new CultureInfo(17),
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.AutoGenerateRubyTags));

        AssertSelectedHitObject(h =>
        {
            var rubyTags = h.RubyTags;
            Assert.AreEqual(1, rubyTags.Count);
            Assert.AreEqual("かぜ", rubyTags[0].Text);
        });
    }

    [Test]
    public void TestAutoGenerateRubyTagsWithNonSupportedLyric()
    {
        PrepareHitObjects(() => new[]
        {
            new Lyric
            {
                Text = "風",
            },
            new Lyric
            {
                Text = string.Empty,
            },
            new Lyric
            {
                Text = string.Empty,
                Language = new CultureInfo(17),
            },
        });

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(AutoGenerateType.AutoGenerateRubyTags));
    }

    #endregion

    #region Time-tag

    [Test]
    public void TestAutoGenerateTimeTags()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.AutoGenerateTimeTags));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(5, h.TimeTags.Count);
        });
    }

    [Test]
    public void TestAutoGenerateTimeTagsWithNonSupportedLyric()
    {
        PrepareHitObjects(() => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
            },
            new Lyric
            {
                Text = string.Empty,
            },
            new Lyric
            {
                Text = string.Empty,
                Language = new CultureInfo(17),
            },
        });

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(AutoGenerateType.AutoGenerateTimeTags));
    }

    #endregion

    #region Time-tag romaji

    [Test]
    public void TestAutoGenerateTimeTagRomaji()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]", "[3,end]" }),
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.AutoGenerateTimeTagRomaji));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual("karaoke", h.TimeTags[0].RomanisedSyllable);
        });
    }

    [Test]
    public void TestAutoGenerateTimeTagRomajiWithNonSupportedLyric()
    {
        PrepareHitObjects(() => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                // with no time-tag.
            },
        });

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(AutoGenerateType.AutoGenerateTimeTagRomaji));
    }

    #endregion

    #region Note

    [Test]
    public void TestAutoGenerateNotes()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            },
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.AutoGenerateNotes));

        AssertSelectedHitObject(h =>
        {
            var actualNotes = getMatchedNotes(h);
            Assert.AreEqual(4, actualNotes.Length);
            Assert.AreEqual("カ", actualNotes[0].Text);
            Assert.AreEqual("ラ", actualNotes[1].Text);
            Assert.AreEqual("オ", actualNotes[2].Text);
            Assert.AreEqual("ケ", actualNotes[3].Text);
        });
    }

    [Test]
    public void TestAutoGenerateNotesWithNonSupportedLyric()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(AutoGenerateType.AutoGenerateNotes));
    }

    private Note[] getMatchedNotes(Lyric lyric)
    {
        var editorBeatmap = Dependencies.Get<EditorBeatmap>();
        return EditorBeatmapUtils.GetNotesByLyric(editorBeatmap, lyric).ToArray();
    }

    #endregion

    #region Shared tests

    [Test]
    [Description("Should be able to generate the property if the lyric is not reference to other lyric.")]
    public void ChangeWithNormalLyric([Values] AutoGenerateType type)
    {
        // for detect reference lyric.
        if (isLyricReferenceChangeHandler(type))
        {
            PrepareHitObject(() => new Lyric
            {
                Text = "karaoke",
            }, false);
        }

        PrepareHitObject(() => new Lyric
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
            },
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsTrue(c.CanGenerate(type));
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsEmpty(c.GetGeneratorNotSupportedLyrics(type));
        });

        TriggerHandlerChanged(c =>
        {
            Assert.DoesNotThrow(() => c.AutoGenerate(type));
        });
    }

    [Test]
    [Description("Should not be able to generate the property if the lyric is missing detectable property.")]
    public void ChangeWithMissingPropertyLyric([Values] AutoGenerateType type)
    {
        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c =>
        {
            Assert.IsFalse(c.CanGenerate(type));
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsNotEmpty(c.GetGeneratorNotSupportedLyrics(type));
        });

        TriggerHandlerChanged(c =>
        {
            var exception = Assert.Catch(() => c.AutoGenerate(type));
            Assert.Contains(exception?.GetType(), new[] { typeof(GeneratorNotSupportedException), typeof(DetectorNotSupportedException) });
        });
    }

    [Test]
    [Description("Should not be able to generate the property if the lyric is reference to other lyric.")]
    public void CheckWithReferencedLyric([Values] AutoGenerateType type)
    {
        if (isLyricReferenceChangeHandler(type))
            return;

        PrepareHitObject(() => new Lyric
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
            },
            // has reference lyric.
            ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(1),
            ReferenceLyric = new Lyric().ChangeId(1),
            ReferenceLyricConfig = new SyncLyricConfig(),
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsFalse(c.CanGenerate(type));
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsNotEmpty(c.GetGeneratorNotSupportedLyrics(type));
        });

        TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.AutoGenerate(type));
    }

    private bool isLyricReferenceChangeHandler(AutoGenerateType type)
        => type == AutoGenerateType.DetectReferenceLyric;

    #endregion
}
