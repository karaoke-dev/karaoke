// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning;

public partial class TestSceneNote : KaraokeHitObjectTestScene
{
    public TestSceneNote()
    {
        AddToggleStep("toggle hitting", v =>
        {
            foreach (var holdNote in CreatedDrawables.SelectMany(d => d.ChildrenOfType<DrawableNote>()))
            {
                ((Bindable<bool>)holdNote.IsHitting).Value = v;
            }
        });
    }

    protected override DrawableHitObject CreateHitObject()
    {
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "カラオケ", 100, 800);
        var note = new Note
        {
            Text = "カラオケ",
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        };
        note.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

        return new DrawableNote(note);
    }
}
