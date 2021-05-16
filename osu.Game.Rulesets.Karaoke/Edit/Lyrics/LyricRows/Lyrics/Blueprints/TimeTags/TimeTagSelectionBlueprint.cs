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
            updatePosition();
        }

        private void updatePosition()
        {
            var position = editorLyricPiece.GetTimeTagPosition(Item);
            var rectangle = new RectangleF(position, new Vector2(time_tag_size));
            UpdatePositionAndSize(rectangle);
        }
    }
}
