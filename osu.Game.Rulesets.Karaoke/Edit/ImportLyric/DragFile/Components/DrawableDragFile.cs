// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile.Components
{
    public class DrawableDragFile : Container
    {
        private const float button_height = 50;
        private const float button_vertical_margin = 15;

        private FileSelector fileSelector;
        private TextFlowContainer currentFileText;

        private TriangleButton importButton;

        [BackgroundDependencyLoader(true)]
        private void load(OsuColour colours)
        {
            Masking = true;
            CornerRadius = 10;
            Children = new Drawable[]
            {
                new Box
                {
                    Colour = colours.GreySeafoamDark,
                    RelativeSizeAxes = Axes.Both,
                },
                fileSelector = new FileSelector(validFileExtensions: ImportLyricManager.LyricFormatExtensions)
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.6f
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.4f,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = colours.GreySeafoamDarker,
                            RelativeSizeAxes = Axes.Both
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding { Bottom = button_height + button_vertical_margin * 2 },
                            Child = new OsuScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Child = currentFileText = new TextFlowContainer(t => t.Font = OsuFont.Default.With(size: 30))
                                {
                                    AutoSizeAxes = Axes.Y,
                                    RelativeSizeAxes = Axes.X,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    TextAnchor = Anchor.Centre
                                },
                                ScrollContent =
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                }
                            },
                        },
                        importButton = new TriangleButton
                        {
                            Text = "Import",
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativeSizeAxes = Axes.X,
                            Height = button_height,
                            Width = 0.9f,
                            Margin = new MarginPadding { Vertical = button_vertical_margin },
                            Action = () => Import?.Invoke(fileSelector.CurrentFile.Value?.FullName)
                        }
                    }
                }
            };

            fileSelector.CurrentFile.BindValueChanged(fileChanged, true);
            fileSelector.CurrentPath.BindValueChanged(directoryChanged);
        }

        private void fileChanged(ValueChangedEvent<FileInfo> selectedFile)
        {
            importButton.Enabled.Value = selectedFile.NewValue != null;
            currentFileText.Text = selectedFile.NewValue?.Name ?? "Select a file or drag to import.";
        }

        private void directoryChanged(ValueChangedEvent<DirectoryInfo> _)
        {
            // this should probably be done by the selector itself, but let's do it here for now.
            fileSelector.CurrentFile.Value = null;
        }

        public Action<string> Import { get; set; }
    }
}
