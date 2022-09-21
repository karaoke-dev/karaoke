// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class LyricList : CompositeDrawable
    {
        public const float LYRIC_LIST_PADDING = 10;

        [Resolved(canBeNull: true)]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<bool> bindableSelecting = new Bindable<bool>();
        private readonly IBindable<float> bindableFontSize = new Bindable<float>();

        private readonly GridContainer lyricEditorGridContainer;
        private readonly LyricEditorSkin skin;
        private readonly DrawableLyricList container;

        public LyricList()
        {
            InternalChild = lyricEditorGridContainer = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                Content = new[]
                {
                    new Drawable[]
                    {
                        new SkinProvidingContainer(skin = new LyricEditorSkin(null))
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(LYRIC_LIST_PADDING),
                            Child = container = new DrawableLyricList
                            {
                                RelativeSizeAxes = Axes.Both,
                            }
                        },
                    },
                    new Drawable[]
                    {
                        new ApplySelectingArea(),
                    }
                }
            };

            container.OnOrderChanged += (x, nowOrder) =>
            {
                lyricsChangeHandler?.ChangeOrder(nowOrder);
            };

            bindableMode.BindValueChanged(e =>
            {
                updateAddLyricState();
            }, true);

            bindableSelecting.BindValueChanged(e =>
            {
                updateAddLyricState();
                initializeApplySelectingArea();
            }, true);

            bindableFontSize.BindValueChanged(e =>
            {
                skin.FontSize = e.NewValue;
            });
        }

        private void initializeApplySelectingArea()
        {
            lyricEditorGridContainer.RowDimensions = new[]
            {
                new Dimension(),
                new Dimension(GridSizeMode.AutoSize),
            };
        }

        private void updateAddLyricState()
        {
            // display add new lyric only with edit mode.
            bool disableBottomDrawable = bindableMode.Value == LyricEditorMode.Texting && !bindableSelecting.Value;
            container.DisplayBottomDrawable = disableBottomDrawable;
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ILyricEditorState state,
                          ILyricSelectionState lyricSelectionState, ILyricsProvider lyricsProvider)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableSelecting.BindTo(lyricSelectionState.Selecting);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, bindableFontSize);

            container.Items.BindTo(lyricsProvider.BindableLyrics);
        }
    }
}
