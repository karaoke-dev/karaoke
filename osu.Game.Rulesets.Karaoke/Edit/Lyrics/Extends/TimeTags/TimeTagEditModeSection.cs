// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagEditModeSection : Section
    {
        protected override string Title => "Edit mode";

        [Cached]
        private readonly OverlayColourProvider overlayColourProvider = OverlayColourProvider.Blue;

        private EditModeButton[] buttons;
        private OsuMarkdownContainer description;

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, ILyricEditorState state)
        {
            Children = new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        buttons = new[]
                        {
                            new EditModeButton(LyricEditorMode.CreateTimeTag)
                            {
                                Text = "Create",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            },
                            new EditModeButton(LyricEditorMode.RecordTimeTag)
                            {
                                Text = "Recording",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            },
                            new EditModeButton(LyricEditorMode.AdjustTimeTag)
                            {
                                Text = "Adjust",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            }
                        }
                    }
                },
                description = new OsuMarkdownContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                }
            };

            updateEditMode(state.Mode);

            void updateEditMode(LyricEditorMode mode)
            {
                state.Mode = mode;

                // update button style.
                foreach (var child in buttons)
                {
                    var highLight = child.Mode == mode;
                    child.Alpha = highLight ? 0.8f : 0.4f;

                    switch (child.Mode)
                    {
                        case LyricEditorMode.CreateTimeTag:
                            child.BackgroundColour = highLight ? colour.Blue : colour.BlueDarker;
                            break;

                        case LyricEditorMode.RecordTimeTag:
                            child.BackgroundColour = highLight ? colour.Green : colour.GreenDarker;
                            break;

                        case LyricEditorMode.AdjustTimeTag:
                            child.BackgroundColour = highLight ? colour.Yellow : colour.YellowDarker;
                            break;
                    }
                }

                // update description text.
                switch (mode)
                {
                    case LyricEditorMode.CreateTimeTag:
                        description.Text = "Use keyboard to control caret position, press `N` to create new time-tag and press `D` to delete exist time-tag.";
                        break;

                    case LyricEditorMode.RecordTimeTag:
                        description.Text = "Press spacing button at the right time to set current time to time-tag.";
                        break;

                    case LyricEditorMode.AdjustTimeTag:
                        description.Text = "Drag to adjust time-tag time precisely.";
                        break;
                }
            }
        }

        public class EditModeButton : OsuButton
        {
            public new Action<LyricEditorMode> Action;

            public LyricEditorMode Mode { get; }

            public EditModeButton(LyricEditorMode mode)
            {
                Mode = mode;
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;

                base.Action = () => Action.Invoke(mode);
            }
        }
    }
}
