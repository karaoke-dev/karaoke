// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricNotesChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricNotesChangeHandler>
{
    protected override bool IncludeAutoGenerator => true;

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
            }
        });

        TriggerHandlerChanged(c => c.AutoGenerate());

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

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate());
    }

    private Note[] getMatchedNotes(Lyric lyric)
    {
        var editorBeatmap = Dependencies.Get<EditorBeatmap>();
        return EditorBeatmapUtils.GetNotesByLyric(editorBeatmap, lyric).ToArray();
    }

    #endregion
}
