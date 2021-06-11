// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagEditModeSection : Section
    {
        protected override string Title => "Edit mode";

        private EditModeButton[] buttons;

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, ILyricEditorState state)
        {
            Child = new GridContainer
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
                        new EditModeButton(LyricEditorMode.EditTimeTag)
                        {
                            Text = "Edit",
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
            };

            updateEditMode(state.Mode);

            void updateEditMode(LyricEditorMode mode)
            {
                foreach (var child in buttons)
                {
                    var highLight = child.Mode == mode;
                    child.Alpha = highLight ? 0.8f : 0.4f;

                    switch (child.Mode)
                    {
                        case LyricEditorMode.EditTimeTag:
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

                state.BindableMode.Value = mode;
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
