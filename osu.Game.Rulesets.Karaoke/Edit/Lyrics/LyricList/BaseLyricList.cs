// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
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
    public abstract class BaseLyricList : CompositeDrawable
    {
        public const float LYRIC_LIST_PADDING = 10;

        [Resolved]
        private ILyricsChangeHandler? lyricsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private LyricEditorColourProvider colourProvider { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<bool> bindableSelecting = new Bindable<bool>();
        private readonly IBindable<float> bindableFontSize = new Bindable<float>();

        private readonly GridContainer lyricEditorGridContainer;
        private readonly LyricEditorSkin skin;
        private readonly DrawableLyricList container;

        private Drawable? background;

        protected BaseLyricList()
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
                            Child = container = CreateDrawableLyricList().With(x =>
                            {
                                x.RelativeSizeAxes = Axes.Both;
                            })
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
                Schedule(redrawBackground);
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

        protected override void LoadComplete()
        {
            base.LoadComplete();

            redrawBackground();
        }

        private void redrawBackground()
        {
            if (background != null)
                RemoveInternal(background, true);

            background = CreateBackground(colourProvider, bindableMode.Value);
            if (background == null)
                return;

            AddInternal(background.With(x =>
            {
                x.RelativeSizeAxes = Axes.Both;
                x.Depth = int.MaxValue;
            }));
        }

        protected abstract DrawableLyricList CreateDrawableLyricList();

        protected virtual Drawable? CreateBackground(LyricEditorColourProvider colourProvider, LyricEditorMode mode) => null;

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
