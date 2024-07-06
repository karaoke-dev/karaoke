// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Badge;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.FixedInfo;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.List;

public partial class InfoControl : CompositeDrawable, IHasContextMenu
{
    private const int max_height = 120;

    private readonly Box background;
    private readonly Box headerBackground;
    private readonly OsuSpriteText timeRange;
    private readonly Container subInfoContainer;

    private readonly IBindable<EditorModeWithEditStep> bindableModeWithEditStep = new Bindable<EditorModeWithEditStep>();

    [Resolved]
    private IDialogOverlay? dialogOverlay { get; set; }

    [Resolved]
    private ILyricsChangeHandler lyricsChangeHandler { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    [Resolved]
    private LyricEditorColourProvider colourProvider { get; set; } = null!;

    public Lyric Lyric { get; }

    public InfoControl(Lyric lyric)
    {
        Lyric = lyric;

        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.X,
                Height = max_height,
            },
            new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Spacing = new Vector2(5),
                Children = new Drawable[]
                {
                    new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 36,
                        Children = new Drawable[]
                        {
                            headerBackground = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                            timeRange = new OsuSpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = OsuFont.GetFont(size: 16, fixedWidth: true),
                                Margin = new MarginPadding(10),
                            },
                            new InvalidInfo(lyric)
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Margin = new MarginPadding(10),
                                Scale = new Vector2(1.3f),
                                Y = 1,
                            },
                        },
                    },
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        ColumnDimensions = new[]
                        {
                            new Dimension(),
                            new Dimension(GridSizeMode.Absolute, 28),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                subInfoContainer = new Container
                                {
                                    RelativeSizeAxes = Axes.X,
                                },
                                new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(5),
                                    Children = new Drawable[]
                                    {
                                        new OrderInfo(lyric),
                                        new LockInfo(lyric),
                                    },
                                },
                            },
                        },
                    },
                },
            },
        };

        timeRange.Text = LyricUtils.LyricTimeFormattedString(lyric);

        bindableModeWithEditStep.BindValueChanged(e =>
        {
            initializeBadge(e.NewValue);

            if (ValueChangedEventUtils.EditModeChanged(e) || !IsLoaded)
                Schedule(() => updateColour(e.NewValue.Mode));
        }, true);
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state)
    {
        bindableModeWithEditStep.BindTo(state.BindableModeWithEditStep);
    }

    private void updateColour(LyricEditorMode mode)
    {
        background.Colour = colourProvider.Background2(mode);
        headerBackground.Colour = colourProvider.Background5(mode);
    }

    private void initializeBadge(EditorModeWithEditStep editorMode)
    {
        subInfoContainer.Clear();
        var subInfo = createSubInfo();
        if (subInfo == null)
            return;

        subInfo.Margin = new MarginPadding { Right = 15 };
        subInfo.Anchor = Anchor.TopRight;
        subInfo.Origin = Anchor.TopRight;
        subInfoContainer.Add(subInfo);

        Drawable? createSubInfo()
        {
            switch (editorMode.Mode)
            {
                case LyricEditorMode.View:
                case LyricEditorMode.EditText:
                    return null;

                case LyricEditorMode.EditReferenceLyric:
                    return new ReferenceLyricInfo(Lyric);

                case LyricEditorMode.EditLanguage:
                    return new LanguageInfo(Lyric);

                case LyricEditorMode.EditRuby:
                    return new LanguageInfo(Lyric);

                case LyricEditorMode.EditTimeTag:
                    return createTimeTagModeSubInfo(editorMode.GetEditStep<TimeTagEditStep>(), Lyric);

                case LyricEditorMode.EditRomanisation:
                    return new LanguageInfo(Lyric);

                case LyricEditorMode.EditNote:
                    return null;

                case LyricEditorMode.EditSinger:
                    return new SingerInfo(Lyric);

                default:
                    throw new ArgumentOutOfRangeException(nameof(editorMode));
            }

            static Drawable createTimeTagModeSubInfo(TimeTagEditStep editMode, Lyric lyric)
            {
                switch (editMode)
                {
                    case TimeTagEditStep.Create:
                        return new LanguageInfo(lyric);

                    case TimeTagEditStep.Recording:
                    case TimeTagEditStep.Adjust:
                        return new TimeTagInfo(lyric);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(editMode));
                }
            }
        }
    }

    public MenuItem[] ContextMenuItems
    {
        get
        {
            var editMode = bindableModeWithEditStep.Value.Mode;
            if (editMode != LyricEditorMode.EditText)
                return Array.Empty<MenuItem>();

            // should select lyric if trying to interact with context menu.
            lyricCaretState.MoveCaretToTargetPosition(Lyric);

            var menuItems = new List<MenuItem>
            {
                new OsuMenuItem("Create new lyric", MenuItemType.Standard, () =>
                {
                    lyricsChangeHandler.CreateAtPosition();
                }),
            };

            // use lazy way to check lyric is not in first
            if (Lyric.Order > 1)
            {
                menuItems.Add(new OsuMenuItem("Combine with previous lyric", MenuItemType.Standard, () =>
                {
                    lyricsChangeHandler.Combine();
                }));
            }

            menuItems.Add(new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
            {
                if (dialogOverlay == null)
                {
                    // todo : remove lyric directly in test case because pop-up dialog is not registered.
                    lyricsChangeHandler.Remove();
                }
                else
                {
                    dialogOverlay.Push(new DeleteLyricDialog(isOk =>
                    {
                        if (isOk)
                            lyricsChangeHandler.Remove();
                    }));
                }
            }));

            return menuItems.ToArray();
        }
    }
}
