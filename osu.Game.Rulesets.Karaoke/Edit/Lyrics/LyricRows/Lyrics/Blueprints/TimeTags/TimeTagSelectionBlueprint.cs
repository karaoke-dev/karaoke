// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.TimeTags
{
    public class TimeTagSelectionBlueprint : TagSelectionBlueprint<TimeTag>
    {
        [Resolved]
        private EditorLyricPiece editorLyricPiece { get; set; }

        public TimeTagSelectionBlueprint(TimeTag item)
            : base(item)
        {
            updatePosition();
        }

        private void updatePosition()
        {
            // todo : should be able to get time-tag position.
            var position = editorLyricPiece.GetTimeTagPosition(Item);
            UpdatePositionAndSize(position);
        }
    }
}
