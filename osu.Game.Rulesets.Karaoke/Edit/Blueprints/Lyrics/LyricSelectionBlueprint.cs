// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics
{
    public class LyricSelectionBlueprint : OverlaySelectionBlueprint
    {
        public LyricSelectionBlueprint(DrawableLyricLine hitObject)
            : base(hitObject)
        {
            RelativeSizeAxes = Axes.None;
        }
    }
}
