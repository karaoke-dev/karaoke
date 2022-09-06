﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.IO.Stores;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    public class FontSelector : CompositeDrawable, IHasCurrentValue<FontUsage>
    {
        private readonly SpriteText previewText;
        private readonly FontFamilyPropertyList familyProperty;
        private readonly FontPropertyList<string> weightProperty;
        private readonly FontPropertyList<float> fontSizeProperty;
        private readonly OsuCheckbox fixedWidthCheckbox;

        private readonly BindableWithCurrent<FontUsage> current = new();
        private readonly BindableList<FontInfo> fonts = new();

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

        public FontSelector()
        {
            InternalChild = new GridContainer
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
                        previewText = new OsuSpriteText
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
                                        familyProperty = new FontFamilyPropertyList
                                        {
                                            Name = "Font family selection area",
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        weightProperty = new FontPropertyList<string>
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
                                            },
                                            Content = new[]
                                            {
                                                new Drawable[]
                                                {
                                                    fontSizeProperty = new FontPropertyList<float>
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

            fonts.BindCollectionChanged((_, b) =>
            {
                // re-calculate if source changed.
                Schedule(() =>
                {
                    string[] oldFamilies = b.OldItems?.OfType<FontInfo>().Select(x => x.Family).Distinct().ToArray();
                    string[] newFamilies = b.NewItems?.OfType<FontInfo>().Select(x => x.Family).Distinct().ToArray();

                    if (oldFamilies != null)
                    {
                        familyProperty.Items.RemoveAll(x => oldFamilies.Contains(x));
                    }

                    if (newFamilies != null)
                    {
                        familyProperty.Items.AddRange(newFamilies);
                    }

                    // should reset family selection if user select the font that will be removed or added.
                    string currentFamily = familyProperty.Current.Value;
                    bool resetFamily = oldFamilies?.Contains(currentFamily) ?? false;

                    if (resetFamily)
                    {
                        familyProperty.Current.Value = familyProperty.Items.FirstOrDefault();
                    }
                });
            });

            familyProperty.Current.BindValueChanged(x =>
            {
                performChange();

                // re-calculate if family changed.
                string[] weight = fonts.Where(f => f.Family == x.NewValue).Select(f => f.Weight).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToArray();
                weightProperty.Items.Clear();
                weightProperty.Items.AddRange(weight);

                // set to first or empty if change new family.
                weightProperty.Current.Value = weight.FirstOrDefault();
            });
            weightProperty.Current.BindValueChanged(_ => performChange());
            fontSizeProperty.Current.BindValueChanged(_ => performChange());
            fixedWidthCheckbox.Current.BindValueChanged(_ => performChange());
        }

        [BackgroundDependencyLoader]
        private void load(FontManager fontManager, IRenderer renderer)
        {
            fonts.BindTo(fontManager.Fonts);

            // create local font store and import those files
            localFontStore = new KaraokeLocalFontStore(fontManager, renderer);
            fontStore.AddStore(localFontStore);

            Current.BindValueChanged(e =>
            {
                var newFont = e.NewValue;
                familyProperty.Current.Value = newFont.Family;
                weightProperty.Current.Value = newFont.Weight;
                fontSizeProperty.Current.Value = newFont.Size;
                fixedWidthCheckbox.Current.Value = newFont.FixedWidth;
            }, true);
        }

        private void performChange()
        {
            var fontUsage = generateFontUsage();

            // add font to local font store for preview purpose.
            localFontStore.ClearFont();
            localFontStore.AddFont(fontUsage);

            previewText.Font = fontUsage;

            // write-back the value.
            Current.Value = fontUsage;
        }

        private FontUsage generateFontUsage()
        {
            string family = familyProperty.Current.Value;
            string weight = weightProperty.Current.Value;
            float size = fontSizeProperty.Current.Value;
            bool fixedWidth = fixedWidthCheckbox.Current.Value;
            return new FontUsage(family, size, weight, false, fixedWidth);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            fontStore?.RemoveStore(localFontStore);
        }

        internal class FontFamilyPropertyList : FontPropertyList<string>
        {
            protected override RearrangeableTextFlowListContainer<string> CreateRearrangeableListContainer()
                => new RearrangeableFontFamilyListContainer();

            private class RearrangeableFontFamilyListContainer : RearrangeableTextFlowListContainer<string>
            {
                protected override DrawableTextListItem CreateDrawable(string item)
                    => new DrawableFontFamilyListItem(item);

                private class DrawableFontFamilyListItem : DrawableTextListItem
                {
                    [Resolved]
                    private FontManager fontManager { get; set; }

                    public DrawableFontFamilyListItem(string item)
                        : base(item)
                    {
                    }

                    protected override void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, string model)
                    {
                        textFlowContainer.TextAnchor = Anchor.BottomLeft;
                        textFlowContainer.AddText(model);

                        var matchedFormat = fontManager.Fonts
                                                       .Where(x => x.Family == Model).Select(x => x.FontFormat)
                                                       .Distinct()
                                                       .ToArray();

                        foreach (var format in matchedFormat)
                        {
                            textFlowContainer.AddText(" ");
                            textFlowContainer.AddArbitraryDrawable(new FontFormatBadge(format));
                        }
                    }
                }
            }

            private class FontFormatBadge : Container
            {
                private readonly FontFormat fontFormat;
                private readonly Box box;
                private readonly OsuSpriteText badgeText;

                public FontFormatBadge(FontFormat fontFormat)
                {
                    this.fontFormat = fontFormat;

                    AutoSizeAxes = Axes.Both;
                    Masking = true;
                    CornerRadius = 3;
                    Children = new Drawable[]
                    {
                        box = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        badgeText = new OsuSpriteText
                        {
                            Font = OsuFont.Default.With(size: 10),
                            Margin = new MarginPadding
                            {
                                Vertical = 1,
                                Horizontal = 3
                            },
                        }
                    };
                }

                [BackgroundDependencyLoader]
                private void load(OsuColour colour)
                {
                    box.Colour = fontFormat switch
                    {
                        FontFormat.Internal => colour.Gray7,
                        FontFormat.Fnt => colour.Pink,
                        FontFormat.Ttf => colour.Blue,
                        FontFormat.Ttc => colour.BlueDark,
                        _ => throw new ArgumentOutOfRangeException(nameof(fontFormat))
                    };

                    // todo : might apply translate.
                    badgeText.Text = fontFormat.ToString();
                }
            }
        }

        internal class FontPropertyList<T> : CompositeDrawable
        {
            private readonly CornerBackground background;
            private readonly TextPropertySearchTextBox filter;
            private readonly RearrangeableTextFlowListContainer<T> propertyFlowList;

            private readonly BindableWithCurrent<T> current = new();

            public Bindable<T> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public BindableList<T> Items => propertyFlowList.Items;

            public FontPropertyList()
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
                                    propertyFlowList = CreateRearrangeableListContainer().With(x =>
                                    {
                                        x.RelativeSizeAxes = Axes.Both;
                                        x.RequestSelection = item =>
                                        {
                                            Current.Value = item;
                                        };
                                    })
                                }
                            }
                        }
                    }
                };

                filter.Current.BindValueChanged(e => propertyFlowList.Filter(e.NewValue));
                Current.BindValueChanged(e => propertyFlowList.SelectedSet.Value = e.NewValue);
            }

            protected virtual RearrangeableTextFlowListContainer<T> CreateRearrangeableListContainer()
                => new();

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
