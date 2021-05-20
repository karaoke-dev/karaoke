// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.RomajiTags
{
    public class RomajiSelectionBlueprint : TagSelectionBlueprint<ITextTag>
    {
        [UsedImplicitly]
        private readonly Bindable<string> text;

        [UsedImplicitly]
        private readonly BindableNumber<int> startIndex;

        [UsedImplicitly]
        private readonly BindableNumber<int> endIndex;

        [Resolved]
        private EditorLyricPiece editorLyricPiece { get; set; }

        public RomajiSelectionBlueprint(ITextTag item)
            : base(item)
        {
            if (!(item is RomajiTag romajiTag))
                throw new InvalidCastException(nameof(item));

            text = romajiTag.TextBindable.GetBoundCopy();
            startIndex = romajiTag.StartIndexBindable.GetBoundCopy();
            endIndex = romajiTag.EndIndexBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            updatePosition();
            text.BindValueChanged(e => updatePosition());
            startIndex.BindValueChanged(e => updatePosition());
            endIndex.BindValueChanged(e => updatePosition());
        }

        private void updatePosition()
        {
            // wait until lyric update romaji position.
            ScheduleAfterChildren(() =>
            {
                var position = editorLyricPiece.GetTextTagPosition(Item);
                UpdatePositionAndSize(position);
            });
        }
    }
}
