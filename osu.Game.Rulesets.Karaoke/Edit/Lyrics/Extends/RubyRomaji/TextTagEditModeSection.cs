// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class TextTagEditModeSection : Section
    {
        private EditModeButton<TextTagEditMode>[] buttons;
        private OsuMarkdownContainer description;

        [Cached]
        private readonly OverlayColourProvider overlayColourProvider = OverlayColourProvider.Pink;

        protected override string Title => "Edit mode";

        [BackgroundDependencyLoader]
        private void load(Bindable<TextTagEditMode> bindableEditMode, OsuColour colour)
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
                            new EditModeButton<TextTagEditMode>(TextTagEditMode.Edit)
                            {
                                Text = "Create",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            },
                            new EditModeButton<TextTagEditMode>(TextTagEditMode.Management)
                            {
                                Text = "Recording",
                                Action = updateEditMode,
                                Padding = new MarginPadding { Horizontal = 5 },
                            },
                        }
                    }
                },
                description = new OsuMarkdownContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                }
            };

            updateEditMode(bindableEditMode.Value);

            void updateEditMode(TextTagEditMode mode)
            {
                bindableEditMode.Value = mode;

                // update button style.
                foreach (var child in buttons)
                {
                    var highLight = child.Mode == mode;
                    child.Alpha = highLight ? 0.8f : 0.4f;

                    switch (child.Mode)
                    {
                        case TextTagEditMode.Edit:
                            child.BackgroundColour = highLight ? colour.Blue : colour.BlueDarker;
                            break;

                        case TextTagEditMode.Management:
                            child.BackgroundColour = highLight ? colour.Red : colour.RedDarker;
                            break;
                    }
                }

                // update description text.
                switch (mode)
                {
                    case TextTagEditMode.Edit:
                        description.Text = "Create / delete and edit lyric text tag in here.";
                        break;

                    case TextTagEditMode.Management:
                        description.Text = "Auto-generate and check invalid text tag in here.";
                        break;
                }
            }
        }
    }
}
