// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers
{
    public class SingerEditSection : Section
    {
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();
        private readonly IBindableList<Singer> bindableSingers = new BindableList<Singer>();
        private readonly IBindableList<int> singerIndexes = new BindableList<int>();
        protected override string Title => "Singer";

        [Resolved]
        private ISingersChangeHandler singersChangeHandler { get; set; }

        [Resolved]
        private ILyricSingerChangeHandler lyricSingerChangeHandler { get; set; }

        public SingerEditSection()
        {
            // update singer selections from lyric.
            bindableCaretPosition.BindValueChanged(e =>
            {
                singerIndexes.UnbindBindings();

                var lyric = e.NewValue?.Lyric;
                if (lyric == null)
                    return;

                // should bind from lyric.
                // singer index might be able to change from other place like singer editor.
                singerIndexes.BindTo(lyric.SingersBindable);
            });

            // update singer list.
            bindableSingers.BindCollectionChanged((_, _) =>
            {
                initialSingerList();
            });

            // update singer toggle state from lyric.
            singerIndexes.BindCollectionChanged((_, _) =>
            {
                initialSingerList();
            });
        }

        private void initialSingerList()
        {
            Content.Clear();
            Content.AddRange(bindableSingers.Select(x =>
            {
                var switchButton = new LabelledSingerSwitchButton(x);
                bool selected = singerIndexes.Contains(x.ID);

                switchButton.Current.Value = selected;
                switchButton.Current.BindValueChanged(e =>
                {
                    if (e.NewValue)
                    {
                        lyricSingerChangeHandler.Add(x);
                    }
                    else
                    {
                        lyricSingerChangeHandler.Remove(x);
                    }
                });

                return switchButton;
            }));
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            // update singer
            bindableSingers.BindTo(singersChangeHandler.Singers);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        }

        public class LabelledSingerSwitchButton : LabelledSwitchButton, IHasCustomTooltip<Singer>
        {
            private const float avatar_size = 48f;

            private readonly IBindable<string> bindableName = new Bindable<string>();
            private readonly IBindable<string> bindableEnglishName = new Bindable<string>();

            public LabelledSingerSwitchButton(Singer singer)
            {
                TooltipContent = singer;

                bindableName.BindTo(singer.NameBindable);
                bindableEnglishName.BindTo(singer.EnglishNameBindable);

                if (InternalChildren[1] is FillFlowContainer fillFlowContainer)
                {
                    fillFlowContainer.Spacing = new Vector2(0, 6);
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

                bindableName.BindValueChanged(e => Label = e.NewValue, true);
                bindableEnglishName.BindValueChanged(e => Description = string.IsNullOrEmpty(e.NewValue) ? "<No english name>" : e.NewValue, true);
            }

            public ITooltip<Singer> GetCustomTooltip() => new SingerToolTip();

            public Singer TooltipContent { get; }
        }
    }
}
