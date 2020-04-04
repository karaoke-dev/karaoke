// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public class TestSceneNote : KaraokeHitObjectTestScene
    {
        protected override DrawableHitObject CreateHitObject()
        {
            var note = new Note
            {
                EndTime = 1000,
                Text = "カラオケ"
            };
            note.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

            return new DrawableNote(note);
        }
    }
}
