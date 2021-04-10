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
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Utils;

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
            private BindableFontUsage bindableFontUsage;
            private readonly TriangleButton fontButton;

            private float[] availableSizes = FontUtils.DefaultFontSize();

            public Bindable<FontUsage> Current
            {
                get => current.Current;
                set
                {
                    current.Current = value;
                    bindableFontUsage = value as BindableFontUsage;

                    if (bindableFontUsage != null)
                    {
                        availableSizes = FontUtils.DefaultFontSize(bindableFontUsage.MinFontSize, bindableFontUsage.MaxFontSize);
                    }
                    else
                    {
                        availableSizes = FontUtils.DefaultFontSize();
                    }
                }
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
                        new[]
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

                                        // If has bindable font usage source, the bind with it first(for getter other property in bindable).
                                        dialog.Current = bindableFontUsage ?? Current;
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
                                    var currentSize = current.Value.Size;
                                    var nextSize = availableSizes.Reverse().FirstOrDefault(x => x < currentSize);
                                    if (nextSize == 0)
                                        return;

                                    current.Value = current.Value.With(size: nextSize);
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
                                    var currentSize = current.Value.Size;
                                    var nextSize = availableSizes.FirstOrDefault(x => x > currentSize);
                                    if (nextSize == 0)
                                        return;

                                    current.Value = current.Value.With(size: nextSize);
                                }
                            }
                        },
                    }
                };

                Current.BindValueChanged(e =>
                {
                    var font = e.NewValue;
                    var fontName = font.FontName ?? "[Unknown font]";
                    var size = FontUtils.GetText(font.Size);
                    var fixedWidthText = font.FixedWidth ? "(fixed width)" : "";
                    var displayText = $"{fontName}, {size} {fixedWidthText}";
                    fontButton.Text = displayText;
                });
            }
        }
    }
}
