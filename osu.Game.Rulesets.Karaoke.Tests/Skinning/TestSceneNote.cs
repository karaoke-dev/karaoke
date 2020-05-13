// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public class TestSceneNote : KaraokeHitObjectTestScene
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
            var note = new Note
            {
                StartTime = 100,
                EndTime = 900,
                Text = "カラオケ"
            };
            note.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

            return new DrawableNote(note);
        }
    }
}
