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
            Assert.That(h.ReferenceLyric, Is.Not.Null);
            Assert.That(h.ReferenceLyricConfig, Is.InstanceOf<SyncLyricConfig>());
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
            Assert.That(h.Language, Is.EqualTo(new CultureInfo("ja")));
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
            Assert.That(h.Language, Is.Null);
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
            Assert.That(rubyTags.Count, Is.EqualTo(1));
            Assert.That(rubyTags[0].Text, Is.EqualTo("かぜ"));
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
            Assert.That(h.TimeTags.Count, Is.EqualTo(5));
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

    #region Romanisation

    [Test]
    public void TestAutoGenerateRomanisation()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]", "[3,end]" }),
        });

        TriggerHandlerChanged(c => c.AutoGenerate(AutoGenerateType.AutoGenerateRomanisation));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags[0].RomanisedSyllable, Is.EqualTo("karaoke"));
        });
    }

    [Test]
    public void TestAutoGenerateRomanisationWithNonSupportedLyric()
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

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate(AutoGenerateType.AutoGenerateRomanisation));
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
            Assert.That(actualNotes.Length, Is.EqualTo(4));
            Assert.That(actualNotes[0].Text, Is.EqualTo("カ"));
            Assert.That(actualNotes[1].Text, Is.EqualTo("ラ"));
            Assert.That(actualNotes[2].Text, Is.EqualTo("オ"));
            Assert.That(actualNotes[3].Text, Is.EqualTo("ケ"));
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
    public void TestChangeWithNormalLyric([Values] AutoGenerateType type)
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
            Language = new CultureInfo(17), // for auto-generate ruby and romanisation.
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
            Assert.That(c.CanGenerate(type), Is.True);
        });

        TriggerHandlerChanged(c =>
        {
            Assert.That(c.GetGeneratorNotSupportedLyrics(type), Is.Empty);
        });

        TriggerHandlerChanged(c =>
        {
            Assert.That(() => c.AutoGenerate(type), Throws.Nothing);
        });
    }

    [Test]
    [Description("Should not be able to generate the property if the lyric is missing detectable property.")]
    public void TestChangeWithMissingPropertyLyric([Values] AutoGenerateType type)
    {
        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c =>
        {
            Assert.That(c.CanGenerate(type), Is.False);
        });

        TriggerHandlerChanged(c =>
        {
            Assert.That(c.GetGeneratorNotSupportedLyrics(type), Is.Not.Empty);
        });

        TriggerHandlerChanged(c =>
        {
            var exception = Assert.Catch(() => c.AutoGenerate(type));
            Assert.That(new[] { typeof(GeneratorNotSupportedException), typeof(DetectorNotSupportedException) }, Does.Contain(exception?.GetType()));
        });
    }

    [Test]
    [Description("Should not be able to generate the property if the lyric is reference to other lyric.")]
    public void TestCheckWithReferencedLyric([Values] AutoGenerateType type)
    {
        if (isLyricReferenceChangeHandler(type))
            return;

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romanisation.
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
            Assert.That(c.CanGenerate(type), Is.False);
        });

        TriggerHandlerChanged(c =>
        {
            Assert.That(c.GetGeneratorNotSupportedLyrics(type), Is.Not.Empty);
        });

        TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.AutoGenerate(type));
    }

    private bool isLyricReferenceChangeHandler(AutoGenerateType type)
        => type == AutoGenerateType.DetectReferenceLyric;

    #endregion
}
