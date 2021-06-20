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

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditModeSection : Section
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
                            new EditModeButton(LyricEditorMode.CreateNote)
                            {
                                Text = "Create",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            },
                            new EditModeButton(LyricEditorMode.CreateNotePosition)
                            {
                                Text = "Position",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            },
                            new EditModeButton(LyricEditorMode.AdjustNote)
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
                        case LyricEditorMode.CreateNote:
                            child.BackgroundColour = highLight ? colour.Blue : colour.BlueDarker;
                            break;

                        case LyricEditorMode.CreateNotePosition:
                            child.BackgroundColour = highLight ? colour.Red : colour.RedDarker;
                            break;

                        case LyricEditorMode.AdjustNote:
                            child.BackgroundColour = highLight ? colour.Yellow : colour.YellowDarker;
                            break;
                    }
                }

                // update description text.
                switch (mode)
                {
                    case LyricEditorMode.CreateNote:
                        description.Text = "Using time-tag to create default notes.";
                        break;

                    case LyricEditorMode.CreateNotePosition:
                        description.Text = "Using singer voice data to adjust note position.";
                        break;

                    case LyricEditorMode.AdjustNote:
                        description.Text = "If you are note satisfied this result, you can adjust this by hands.";
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
