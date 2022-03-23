// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public class SettingsFont : SettingsItem<FontUsage>
    {
        protected override Drawable CreateControl() => new FontSelectionButton
        {
            RelativeSizeAxes = Axes.X,
        };

        internal class FontSelectionButton : CompositeDrawable, IHasCurrentValue<FontUsage>, IHasPopover
        {
            private const float height = 30;

            [Resolved(canBeNull: true)]
            protected OsuGame Game { get; private set; }

            private readonly BindableWithCurrent<FontUsage> current = new();
            private BindableFontUsage bindableFontUsage;

            private readonly GridContainer grid;
            private readonly SettingsButton fontButton;
            private readonly SettingsButton decreaseFontSizeButton;
            private readonly SettingsButton increaseFontSizeButton;

            private float[] availableSizes = FontUtils.DefaultFontSize();

            public Bindable<FontUsage> Current
            {
                get => current.Current;
                set
                {
                    current.Current = value;
                    bindableFontUsage = value as BindableFontUsage;

                    availableSizes = bindableFontUsage != null
                        ? FontUtils.DefaultFontSize(bindableFontUsage.MinFontSize, bindableFontUsage.MaxFontSize)
                        : FontUtils.DefaultFontSize();

                    bool showSizeButton = availableSizes.Length > 1;
                    decreaseFontSizeButton.Alpha = showSizeButton ? 1 : 0;
                    increaseFontSizeButton.Alpha = showSizeButton ? 1 : 0;

                    int spacing = showSizeButton ? 5 : 0;
                    float buttonWidth = showSizeButton ? height : 0;
                    grid.ColumnDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, spacing),
                        new Dimension(GridSizeMode.Absolute, buttonWidth),
                        new Dimension(GridSizeMode.Absolute, spacing),
                        new Dimension(GridSizeMode.Absolute, buttonWidth),
                    };
                }
            }

            public FontSelectionButton()
            {
                AutoSizeAxes = Axes.Y;
                InternalChild = grid = new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        new[]
                        {
                            fontButton = new SettingsButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Padding = new MarginPadding { Left = SettingsPanel.CONTENT_MARGINS },
                                Height = height,
                                Action = this.ShowPopover
                            },
                            null,
                            decreaseFontSizeButton = new SettingsButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Padding = new MarginPadding(),
                                Height = height,
                                Text = "-",
                                Action = () =>
                                {
                                    float currentSize = current.Value.Size;
                                    float nextSize = availableSizes.Reverse().FirstOrDefault(x => x < currentSize);
                                    if (nextSize == 0)
                                        return;

                                    current.Value = current.Value.With(size: nextSize);
                                }
                            },
                            null,
                            increaseFontSizeButton = new SettingsButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Padding = new MarginPadding(),
                                Height = height,
                                Text = "+",
                                Action = () =>
                                {
                                    float currentSize = current.Value.Size;
                                    float nextSize = availableSizes.FirstOrDefault(x => x > currentSize);
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
                    string fontName = font.FontName;
                    string size = FontUtils.GetText(font.Size);
                    string fixedWidthText = font.FixedWidth ? "(fixed width)" : "";
                    string displayText = $"{fontName}, {size} {fixedWidthText}";
                    fontButton.Text = displayText;
                });
            }

            public Popover GetPopover()
                => new FontSelectorPopover(bindableFontUsage ?? Current);
        }

        internal class FontSelectorPopover : OsuPopover
        {
            public FontSelectorPopover(Bindable<FontUsage> bindableFontUsage)
            {
                Child = new FontSelector
                {
                    Width = 1000,
                    Height = 600,
                    Current = bindableFontUsage
                };
            }
        }
    }
}
