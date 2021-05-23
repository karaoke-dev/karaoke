// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers
{
    public class SingerEditSection : Section
    {
        private readonly Bindable<int[]> singerIndexes = new Bindable<int[]>();
        protected override string Title => "Singer";

        public SingerEditSection()
        {
            singerIndexes.BindValueChanged(e =>
            {
                foreach (var singerLabel in Content.OfType<LabelledSingerSwitchButton>())
                {
                    // should mark singer as selected/unselected.
                    var singerId = singerLabel.Singer.ID;
                    var selected = singerIndexes.Value?.Contains(singerId) ?? false;

                    // update singer label selection.
                    singerLabel.Current.Value = selected;
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, ILyricEditorState state)
        {
            // update singer
            if (beatmap?.PlayableBeatmap is KaraokeBeatmap karaokeBeatmap)
            {
                var singers = karaokeBeatmap.Singers;
                Content.AddRange(singers.Select(x =>
                {
                    var singerName = x.Name;
                    var description = x.Description;
                    return new LabelledSingerSwitchButton(x)
                    {
                        Label = singerName,
                        Description = description,
                    };
                }));
            }

            // update lyric.
            state.BindableCaretPosition.BindValueChanged(e =>
            {
                e.OldValue?.Lyric?.SingersBindable.UnbindFrom(singerIndexes);
                e.NewValue?.Lyric?.SingersBindable.BindTo(singerIndexes);
            });
        }

        public class LabelledSingerSwitchButton : LabelledSwitchButton
        {
            protected const float AVATAR_CORNER_RADIUS = 40f;

            public ISinger Singer { get; }

            public LabelledSingerSwitchButton(ISinger singer)
            {
                Singer = singer;

                if (InternalChildren[1] is FillFlowContainer fillflowContainer)
                {
                    fillflowContainer.Padding
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
                    Size = new Vector2(AVATAR_CORNER_RADIUS),
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
