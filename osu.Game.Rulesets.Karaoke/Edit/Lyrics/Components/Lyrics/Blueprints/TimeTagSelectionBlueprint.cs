// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Blueprints
{
    public class TimeTagSelectionBlueprint : SelectionBlueprint<TimeTag>
    {
        private const float time_tag_size = 10;

        [Resolved]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        public TimeTagSelectionBlueprint(TimeTag item)
            : base(item)
        {
            RelativeSizeAxes = Axes.None;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            updatePosition();
        }

        private void updatePosition()
        {
            var size = new Vector2(time_tag_size);
            var position = karaokeSpriteText.GetTimeTagPosition(Item) - size / 2;

            X = position.X;
            Y = position.Y;
            Width = time_tag_size;
            Height = time_tag_size;
        }

        public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;
    }
}
