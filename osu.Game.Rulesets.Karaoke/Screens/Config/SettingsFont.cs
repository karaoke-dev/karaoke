// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class SettingsFont : SettingsItem<FontUsage>
    {
        protected override Drawable CreateControl() => new FontSelectionButton
        {
            Margin = new MarginPadding { Top = 5 },
            RelativeSizeAxes = Axes.X,
        };

        internal class FontSelectionButton : CompositeDrawable, IHasCurrentValue<FontUsage>
        {
            private const float height = 30;

            [Resolved(canBeNull: true)]
            protected OsuGame Game { get; private set; }

            private readonly BindableWithCurrent<FontUsage> current = new BindableWithCurrent<FontUsage>();
            private readonly TriangleButton fontButton;

            public Bindable<FontUsage> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public FontSelectionButton()
            {
                AutoSizeAxes = Axes.Y;
                InternalChild = new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Distributed),
                        new Dimension(GridSizeMode.Absolute, 5),
                        new Dimension(GridSizeMode.Absolute, height),
                        new Dimension(GridSizeMode.Absolute, 5),
                        new Dimension(GridSizeMode.Absolute, height),
                    },
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        new []
                        {
                            fontButton = new TriangleButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = height,
                                Action = () =>
                                {
                                    try
                                    {
                                        var displayContainer = Game.GetDisplayContainer();
                                        if (displayContainer == null)
                                            return;

                                        // Should only has one instance.
                                        var dialog = displayContainer.Children.OfType<FontSelectionDialog>().FirstOrDefault();

                                        if (dialog == null)
                                        {
                                            displayContainer.Add(dialog = new FontSelectionDialog());
                                        }

                                        dialog.Current = Current;
                                        dialog?.Show();
                                    }
                                    catch
                                    {
                                        // maybe this overlay has been moved into internal.
                                    }
                                }
                            },
                            null,
                            new TriangleButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = height,
                                Text = "-",
                                Action = () =>
                                {

                                }
                            },
                            null,
                            new TriangleButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = height,
                                Text = "+",
                                Action = () =>
                                {

                                }
                            }
                        },
                    }
                };
                

                Current.BindValueChanged(e =>
                {
                    var font = e.NewValue;
                    var family = font.Family ?? "[Unknown font]";
                    var weight = string.IsNullOrEmpty(font.Weight) ? $"-{font.Weight}" : "";
                    var size = $"{font.Size}px";
                    var fixedWidthText = font.FixedWidth ? "(fixed width)" : "";
                    var displayText = $"{family}{weight}, {size} {fixedWidthText}";
                    fontButton.Text = displayText;
                });
            }
        }
    }
}
