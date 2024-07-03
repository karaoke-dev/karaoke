// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content;

public abstract partial class LyricList : CompositeDrawable
{
    public const float LYRIC_LIST_PADDING = 10;

    [Resolved]
    private ILyricsChangeHandler? lyricsChangeHandler { get; set; }

    [Resolved]
    private LyricEditorColourProvider colourProvider { get; set; } = null!;

    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
    private readonly IBindable<bool> bindableSelecting = new Bindable<bool>();

    private readonly GridContainer lyricEditorGridContainer;
    private readonly LyricEditorSkin skin;
    private readonly DrawableLyricList container;
    private readonly ApplySelectingArea applySelectingArea;

    private Drawable? background;

    protected LyricList()
    {
        InternalChild = lyricEditorGridContainer = new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            RowDimensions = new[]
            {
                new Dimension(),
                new Dimension(GridSizeMode.AutoSize),
            },
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
                        }),
                    },
                },
                new Drawable[]
                {
                    applySelectingArea = new ApplySelectingArea(),
                },
            },
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
            updateApplySelectingArea();
        }, true);
    }

    protected void AdjustSkin(Action<LyricEditorSkin> action)
    {
        action(skin);
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

    private void updateApplySelectingArea()
    {
        if (bindableSelecting.Value)
        {
            applySelectingArea.Show();
        }
        else
        {
            applySelectingArea.Hide();
        }
    }

    private void updateAddLyricState()
    {
        // display add new lyric only with edit mode.
        bool disableBottomDrawable = bindableMode.Value == LyricEditorMode.EditText && !bindableSelecting.Value;
        container.DisplayBottomDrawable = disableBottomDrawable;
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ILyricEditorState state,
                      ILyricSelectionState lyricSelectionState, ILyricsProvider lyricsProvider)
    {
        bindableMode.BindTo(state.BindableMode);
        bindableSelecting.BindTo(lyricSelectionState.Selecting);

        container.Items.BindTo(lyricsProvider.BindableLyrics);
    }
}
