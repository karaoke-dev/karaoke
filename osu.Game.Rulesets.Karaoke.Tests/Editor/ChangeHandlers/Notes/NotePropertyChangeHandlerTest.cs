// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Notes;

public partial class NotePropertyChangeHandlerTest : BaseHitObjectPropertyChangeHandlerTest<NotePropertyChangeHandler, Note>
{
    private static readonly ElementId referenced_lyric_id = TestCaseElementIdHelper.CreateElementIdByNumber(1);

    [Test]
    public void TestChangeText()
    {
        PrepareHitObject(() => new Note
        {
            ReferenceLyricId = referenced_lyric_id,
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.ChangeText("からおけ"));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Text, Is.EqualTo("からおけ"));
        });
    }

    [Test]
    public void TestChangeRubyText()
    {
        PrepareHitObject(() => new Note
        {
            ReferenceLyricId = referenced_lyric_id,
            RubyText = "からおけ",
        });

        TriggerHandlerChanged(c => c.ChangeRubyText("カラオケ"));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.RubyText, Is.EqualTo("カラオケ"));
        });
    }

    [Test]
    public void TestChangeDisplayStateToVisible()
    {
        PrepareHitObject(() => new Note
        {
            ReferenceLyricId = referenced_lyric_id,
        });

        TriggerHandlerChanged(c => c.ChangeDisplayState(true));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Display);
        });
    }

    [Test]
    public void TestChangeDisplayStateToNonVisible()
    {
        PrepareHitObject(() => new Note
        {
            ReferenceLyricId = referenced_lyric_id,
            Display = true,
            Tone = new Tone(3),
        });

        TriggerHandlerChanged(c => c.ChangeDisplayState(false));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Display, Is.False);
            Assert.That(h.Tone, Is.EqualTo(new Tone()));
        });
    }

    [Test]
    [Ignore("Waiting to implement the lock rules.")]
    public void TestWithReferenceLyric()
    {
        PrepareHitObject(() => new Note
        {
            Text = "カラオケ",
            ReferenceLyric = new Lyric
            {
                ReferenceLyric = new Lyric(),
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            },
        });

        TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.ChangeText("からおけ"));
    }

    [Test]
    public void TestOffsetTone()
    {
        PrepareHitObject(() => new Note
        {
            ReferenceLyricId = referenced_lyric_id,
            Display = true,
            Tone = new Tone(3),
        });

        TriggerHandlerChanged(c => c.OffsetTone(new Tone(-3)));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Tone, Is.EqualTo(new Tone()));
            Assert.That(h.Display);
        });
    }

    [Test]
    public void TestOffsetToneWithZeroValue()
    {
        PrepareHitObject(() => new Note
        {
            ReferenceLyricId = referenced_lyric_id,
            Display = true,
            Tone = new Tone(3),
        });

        // offset value should not be zero.
        TriggerHandlerChangedWithException<InvalidOperationException>(c => c.OffsetTone(new Tone()));
    }

    protected override void SetUpEditorBeatmap(Action<EditorBeatmap> action)
    {
        base.SetUpEditorBeatmap(editorBeatmap =>
        {
            action(editorBeatmap);

            editorBeatmap.Add(new Lyric
            {
                Text = "Referenced lyric",
            }.ChangeId(referenced_lyric_id));
        });
    }
}
