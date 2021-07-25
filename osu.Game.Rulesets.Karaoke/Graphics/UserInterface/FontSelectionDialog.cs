// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Fonts;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class FontSelectionDialog : TitleFocusedOverlayContainer, IHasCurrentValue<FontUsage>
    {
        protected override string Title => "Select font";

        private readonly SpriteText previewText;
        private readonly TextPropertyList<string> familyProperty;
        private readonly TextPropertyList<string> weightProperty;
        private readonly TextPropertyList<float> fontSizeProperty;
        private readonly OsuCheckbox fixedWidthCheckbox;

        private readonly BindableWithCurrent<FontUsage> current = new BindableWithCurrent<FontUsage>();
        private readonly BindableList<FontInfo> fonts = new BindableList<FontInfo>();

        [Resolved]
        private FontStore fontStore { get; set; }

        private KaraokeLocalFontStore localFontStore;

        public Bindable<FontUsage> Current
        {
            get => current.Current;
            set
            {
                current.Current = value;

                // should calculate available size until has bindable text.
                fontSizeProperty.Items.Clear();

                if (value is BindableFontUsage bindableFontUsage)
                {
                    fontSizeProperty.Items.AddRange(FontUtils.DefaultFontSize(bindableFontUsage.MinFontSize, bindableFontUsage.MaxFontSize));
                }
                else
                {
                    fontSizeProperty.Items.AddRange(FontUtils.DefaultFontSize());
                }
            }
        }

        public FontSelectionDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.6f, 0.8f);

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.4f),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        previewText = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = "カラオケ, karaoke"
                        }
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            Padding = new MarginPadding(10),
                            RelativeSizeAxes = Axes.Both,
                            Child = new GridContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                ColumnDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.Relative, 0.5f),
                                    new Dimension(GridSizeMode.Relative, 0.3f),
                                    new Dimension(GridSizeMode.Relative, 0.2f),
                                },
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        familyProperty = new TextPropertyList<string>
                                        {
                                            Name = "Font family selection area",
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        weightProperty = new TextPropertyList<string>
                                        {
                                            Name = "Font widget selection area",
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        new GridContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            RowDimensions = new[]
                                            {
                                                new Dimension(),
                                                new Dimension(GridSizeMode.Absolute, 48),
                                                new Dimension(GridSizeMode.Absolute, 64),
                                            },
                                            Content = new[]
                                            {
                                                new Drawable[]
                                                {
                                                    fontSizeProperty = new TextPropertyList<float>
                                                    {
                                                        Name = "Font size selection area",
                                                        RelativeSizeAxes = Axes.Both,
                                                    },
                                                },
                                                new Drawable[]
                                                {
                                                    fixedWidthCheckbox = new OsuCheckbox
                                                    {
                                                        Name = "Font fixed width selection area",
                                                        RelativeSizeAxes = Axes.X,
                                                        Padding = new MarginPadding(10),
                                                        LabelText = "FixedWidth",
                                                    },
                                                },
                                                new Drawable[]
                                                {
                                                    // OK Button.
                                                    new TriangleButton
                                                    {
                                                        Name = "OK Button",
                                                        RelativeSizeAxes = Axes.X,
                                                        Padding = new MarginPadding(10),
                                                        Text = "OK",
                                                        Height = 64,
                                                        Action = () =>
                                                        {
                                                            // set to current value and hide.
                                                            var font = generateFontUsage();
                                                            Current.Value = font;
                                                            Hide();
                                                        }
                                                    },
                                                }
                                            }
                                        }
                                    },
                                }
                            }
                        }
                    }
                }
            };

            fonts.BindCollectionChanged((a, b) =>
            {
                // re-calculate if source changed.
                familyProperty.Items.Clear();
                familyProperty.Items.AddRange(fonts.Select(x => x.Family).Distinct());
            });

            familyProperty.Current.BindValueChanged(x =>
            {
                previewChange();

                // re-calculate if family changed.
                var weight = fonts.Where(f => f.Family == x.NewValue).Select(f => f.Weight).Where(s => !string.IsNullOrEmpty(s)).Distinct();
                weightProperty.Items.Clear();
                weightProperty.Items.AddRange(weight);

                // set to first or empty if change new family.
                weightProperty.Current.Value = weight.FirstOrDefault();
            });
            weightProperty.Current.BindValueChanged(x => previewChange());
            fontSizeProperty.Current.BindValueChanged(x => previewChange());
            fixedWidthCheckbox.Current.BindValueChanged(x => previewChange());
            Current.BindValueChanged(e =>
            {
                var newFont = e.NewValue;
                familyProperty.Current.Value = newFont.Family;
                weightProperty.Current.Value = newFont.Weight;
                fontSizeProperty.Current.Value = newFont.Size;
                fixedWidthCheckbox.Current.Value = newFont.FixedWidth;
            });
        }

        [BackgroundDependencyLoader]
        private void load(FontManager fontManager, GameHost host)
        {
            fonts.BindTo(fontManager.Fonts);

            // create local font store and import those files
            localFontStore = new KaraokeLocalFontStore(host);
            fontStore.AddStore(localFontStore);
        }

        private void previewChange()
        {
            var fontUsage = generateFontUsage();

            // add font to local font store for preview purpose.
            var fontInfo = FontUsageUtils.ToFontInfo(fontUsage);
            localFontStore.ClearFont();
            localFontStore.AddFont(fontInfo);

            previewText.Font = fontUsage;
        }

        private FontUsage generateFontUsage()
        {
            var family = familyProperty.Current.Value;
            var weight = weightProperty.Current.Value;
            var size = fontSizeProperty.Current.Value;
            var fixedWidth = fixedWidthCheckbox.Current.Value;
            return new FontUsage(family, size, weight, false, fixedWidth);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }

        internal class TextPropertyList<T> : CompositeDrawable
        {
            private readonly CornerBackground background;
            private readonly TextPropertySearchTextBox filter;
            private readonly RearrangeableTextListContainer<T> propertyList;

            private readonly BindableWithCurrent<T> current = new BindableWithCurrent<T>();

            public Bindable<T> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public BindableList<T> Items => propertyList.Items;

            public TextPropertyList()
            {
                InternalChild = new Container
                {
                    Padding = new MarginPadding(10),
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        background = new CornerBackground
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.Absolute, 40),
                                new Dimension()
                            },
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    filter = new TextPropertySearchTextBox
                                    {
                                        RelativeSizeAxes = Axes.X,
                                    }
                                },
                                new Drawable[]
                                {
                                    propertyList = new RearrangeableTextListContainer<T>
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        RequestSelection = item =>
                                        {
                                            Current.Value = item;
                                        },
                                    }
                                }
                            }
                        }
                    }
                };

                filter.Current.BindValueChanged(e => propertyList.Filter(e.NewValue));
                Current.BindValueChanged(e => propertyList.SelectedSet.Value = e.NewValue);
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.ContextMenuGray;
            }

            private class TextPropertySearchTextBox : SearchTextBox
            {
                protected override Color4 SelectionColour => Color4.Gray;

                public TextPropertySearchTextBox()
                {
                    PlaceholderText = @"Search...";
                }
            }
        }
    }
}
