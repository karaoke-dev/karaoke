// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints
{
    public class LyricSelectionBlueprint : SelectionBlueprint
    {
        public LyricSelectionBlueprint(DrawableLyricLine hitObject)
            : base(hitObject)
        {
            RelativeSizeAxes = Axes.None;
        }
    }
}
