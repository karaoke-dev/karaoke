// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.Rubies
{
    public class RubySelectionBlueprint : SelectionBlueprint<ITextTag>
    {
        public RubySelectionBlueprint(ITextTag item)
            : base(item)
        {
            RelativeSizeAxes = Axes.None;

            // todo : Make a demo on how to adjust blueprint with target position and size without creating child object.
            X = 100;
            Y = 30;
            Width = 20;
            Height = 20;
        }

        public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;
    }
}
