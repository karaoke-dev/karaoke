// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers
{
    public class SingerEditSection : Section
    {
        private readonly IBindableList<Singer> bindableSingers = new BindableList<Singer>();
        private readonly BindableList<int> singerIndexes = new();
        protected override string Title => "Singer";

        [Resolved]
        private ISingersChangeHandler singersChangeHandler { get; set; }

        public SingerEditSection()
        {
            // update singer list.
            bindableSingers.BindCollectionChanged((_, args) =>
            {
                Content.Clear();
                Content.AddRange(bindableSingers.Select(x => new LabelledSingerSwitchButton(x)));
            });

            // update selection.
            singerIndexes.BindCollectionChanged((_, _) =>
            {
                foreach (var singerLabel in Content.OfType<LabelledSingerSwitchButton>())
                {
                    // should mark singer as selected/unselected.
                    int singerId = singerLabel.Singer.ID;
                    bool selected = singerIndexes?.Contains(singerId) ?? false;

                    // update singer label selection.
                    singerLabel.Current.Value = selected;
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            // update singer
            bindableSingers.BindTo(singersChangeHandler.Singers);

            // update lyric.
            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                e.OldValue?.Lyric?.SingersBindable.UnbindFrom(singerIndexes);
                e.NewValue?.Lyric?.SingersBindable.BindTo(singerIndexes);
            });
        }

        public class LabelledSingerSwitchButton : LabelledSwitchButton
        {
            private const float avatar_size = 40f;

            public Singer Singer { get; }

            public LabelledSingerSwitchButton(Singer singer)
            {
                Singer = singer;

                Label = singer.Name;
                Description = singer.Description;

                if (InternalChildren[1] is FillFlowContainer fillFlowContainer)
                {
                    fillFlowContainer.Padding
                        = new MarginPadding
                        {
                            Horizontal = CONTENT_PADDING_HORIZONTAL,
                            Vertical = CONTENT_PADDING_VERTICAL,
                            Left = CONTENT_PADDING_HORIZONTAL + 40 + CONTENT_PADDING_HORIZONTAL,
                        };
                }

                AddInternal(new DrawableCircleSingerAvatar
                {
                    Singer = singer,
                    Size = new Vector2(avatar_size),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding
                    {
                        Left = CONTENT_PADDING_HORIZONTAL,
                    }
                });
            }
        }
    }
}
