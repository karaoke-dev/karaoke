// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.TimeTags
{
    public class TimeTagSelectionBlueprint : TagSelectionBlueprint<TimeTag>
    {
        private const float time_tag_size = 10;

        [Resolved]
        private EditorLyricPiece editorLyricPiece { get; set; }

        public TimeTagSelectionBlueprint(TimeTag item)
            : base(item)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            updatePosition();
        }

        private void updatePosition()
        {
            var size = new Vector2(time_tag_size);
            var position = editorLyricPiece.GetTimeTagPosition(Item) - size / 2;
            var rectangle = new RectangleF(position, size);
            UpdatePositionAndSize(rectangle);
        }
    }
}
