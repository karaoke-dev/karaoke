// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.RomajiTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class RubyRomajiBlueprintContainer : ExtendBlueprintContainer<ITextTag>
    {
        [Resolved]
        private ILyricEditorState state { get; set; }

        [UsedImplicitly]
        private readonly Bindable<RubyTag[]> rubyTags;

        [UsedImplicitly]
        private readonly Bindable<RomajiTag[]> romajiTags;

        protected readonly Lyric Lyric;

        public RubyRomajiBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
            rubyTags = lyric.RubyTagsBindable.GetBoundCopy();
            romajiTags = lyric.RomajiTagsBindable.GetUnboundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(state.SelectedTextTags);

            // Add ruby and romaji tag into blueprint container
            RegistBindable(rubyTags);
            RegistBindable(romajiTags);
        }

        protected override bool ApplySnapResult(SelectionBlueprint<ITextTag>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            // todo : handle lots of ruby / romaji drag position changed.
            return true;
        }

        protected override SelectionHandler<ITextTag> CreateSelectionHandler()
            => new RubyRomajiSelectionHandler();

        protected override SelectionBlueprint<ITextTag> CreateBlueprintFor(ITextTag item)
        {
            switch (item)
            {
                case RubyTag rubyTag:
                    return new RubySelectionBlueprint(rubyTag);

                case RomajiTag romajiTag:
                    return new RomajiSelectionBlueprint(romajiTag);

                default:
                    throw new ArgumentOutOfRangeException(nameof(item));
            }
        }

        protected override void DeselectAll()
        {
            state.ClearSelectedTextTags();
        }

        protected class RubyRomajiSelectionHandler : ExtendSelectionHandler<ITextTag>
        {
            [Resolved]
            private ILyricEditorState state { get; set; }

            [Resolved]
            private LyricManager lyricManager { get; set; }

            [Resolved]
            private EditorLyricPiece editorLyricPiece { get; set; }

            [BackgroundDependencyLoader]
            private void load()
            {
                SelectedItems.BindTo(state.SelectedTextTags);
            }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<ITextTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<ITextTag> items)
            {
                // todo : delete ruby or romaji
            }

            public override bool HandleScale(Vector2 scale, Anchor anchor)
            {
                // 0. notice that this feature only works if only select one ruby / romaji tag.
                var selectedTextTag = SelectedItems.FirstOrDefault();
                if (selectedTextTag == null)
                    return false;

                // 1. we should get real left-side and right-side position
                var leftPosition = 50;
                var rightPosition = 70;

                // 2. get updated text-tag index
                var startIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(leftPosition));
                var endIndex = TextIndexUtils.ToStringIndex(editorLyricPiece.GetHoverIndex(rightPosition));

                // apply new index and note that should change lyric property also.
                selectedTextTag.StartIndex = startIndex;
                selectedTextTag.EndIndex = endIndex;

                return true;
            }

            protected override void OnSelectionChanged()
            {
                base.OnSelectionChanged();

                // only select one ruby / romaji tag can let user drag to change start and end index.
                SelectionBox.CanScaleX = SelectedItems.Count == 1;
            }
        }
    }
}
