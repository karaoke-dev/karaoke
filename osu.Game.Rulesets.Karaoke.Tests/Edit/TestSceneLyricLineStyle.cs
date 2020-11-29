// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneLyricLineStyle : OsuTestScene
    {
        private ColorArea selectedColorArea => colorAreaDropdown.Current.Value;
        private FontArea selectedFontArea => fontAreaDropdown.Current.Value;

        private readonly OsuDropdown<ColorArea> colorAreaDropdown;
        private readonly OsuDropdown<BrushType> brushTypeDropdown;
        private readonly ColorPicker colorPicker;

        private readonly FillFlowContainer fontSection;
        private readonly OsuDropdown<FontArea> fontAreaDropdown;
        private readonly OsuDropdown<Font> fontDropdown;
        private readonly OsuCheckbox boldCheckbox;
        private readonly OsuSliderBar<float> fontSizeSliderBar;
        private readonly OsuSliderBar<int> borderSliderBar;
        private readonly OsuCheckbox displayShaderCheckbox;
        private readonly OsuSliderBar<float> shadowXSliderBar;
        private readonly OsuSliderBar<float> shadowYSliderBar;

        private TestDrawableLyricLine drawableLyricLine;
        private readonly SkinProvidingContainer layoutArea;

        public TestSceneLyricLineStyle()
        {
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 410f)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new GridContainer
                        {
                            Name = "Edit container",
                            RelativeSizeAxes = Axes.Both,
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        Name = "Color section",
                                        RelativeSizeAxes = Axes.Both,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                Name = "Setting background",
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Gray
                                            },
                                            new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Padding = new MarginPadding(5),
                                                Spacing = new Vector2(10),
                                                Children = new Drawable[]
                                                {
                                                    new EditSection
                                                    {
                                                        Name = "Color",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Color" },
                                                            colorAreaDropdown = new OsuDropdown<ColorArea>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Items = (ColorArea[])Enum.GetValues(typeof(ColorArea))
                                                            }
                                                        }
                                                    },
                                                    new EditSection
                                                    {
                                                        Name = "Brush type",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Brush type" },
                                                            brushTypeDropdown = new OsuDropdown<BrushType>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Items = (BrushType[])Enum.GetValues(typeof(BrushType))
                                                            }
                                                        }
                                                    },
                                                    new EditSection
                                                    {
                                                        Name = "Color picker",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Color picker" },
                                                            colorPicker = new ColorPicker
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                            }
                                                        }
                                                    },
                                                }
                                            }
                                        }
                                    }
                                },
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        Name = "Font section",
                                        RelativeSizeAxes = Axes.Both,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                Name = "Setting background",
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Gray
                                            },
                                            fontSection = new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Padding = new MarginPadding(5),
                                                Spacing = new Vector2(10),
                                                Children = new Drawable[]
                                                {
                                                    new EditSection
                                                    {
                                                        Name = "Font area",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Font area" },
                                                            fontAreaDropdown = new OsuDropdown<FontArea>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Items = (FontArea[])Enum.GetValues(typeof(FontArea))
                                                            }
                                                        }
                                                    },
                                                    new EditSection
                                                    {
                                                        // TODO : implement
                                                        Name = "Font",
                                                        Alpha = 0,
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Font" },
                                                            fontDropdown = new OsuDropdown<Font>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Items = (Font[])Enum.GetValues(typeof(Font))
                                                            }
                                                        }
                                                    },
                                                    boldCheckbox = new OsuCheckbox
                                                    {
                                                        LabelText = "Bold"
                                                    },
                                                    new EditSection
                                                    {
                                                        Name = "Font size",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Font size" },
                                                            fontSizeSliderBar = new OsuSliderBar<float>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Current = new BindableFloat
                                                                {
                                                                    Value = 30,
                                                                    MinValue = 10,
                                                                    MaxValue = 70
                                                                }
                                                            }
                                                        }
                                                    },
                                                    new EditSection
                                                    {
                                                        Name = "Border size",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Border size" },
                                                            borderSliderBar = new OsuSliderBar<int>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Current = new BindableInt
                                                                {
                                                                    Value = 10,
                                                                    MinValue = 0,
                                                                    MaxValue = 20
                                                                }
                                                            }
                                                        }
                                                    },
                                                    displayShaderCheckbox = new OsuCheckbox
                                                    {
                                                        LabelText = "Display shadow"
                                                    },
                                                    new EditSection
                                                    {
                                                        Name = "Shadow X",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Shadow X" },
                                                            shadowXSliderBar = new OsuSliderBar<float>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Current = new BindableFloat
                                                                {
                                                                    Value = 10,
                                                                    MinValue = 0,
                                                                    MaxValue = 20
                                                                }
                                                            }
                                                        }
                                                    },
                                                    new EditSection
                                                    {
                                                        Name = "Shadow Y",
                                                        Children = new Drawable[]
                                                        {
                                                            new OsuSpriteText { Text = "Shadow Y" },
                                                            shadowYSliderBar = new OsuSliderBar<float>
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Current = new BindableFloat
                                                                {
                                                                    Value = 10,
                                                                    MinValue = 0,
                                                                    MaxValue = 20
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new Container
                        {
                            Name = "Preview container",
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    Name = "Preview background",
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.WhiteSmoke
                                },
                                layoutArea = new SkinProvidingContainer(new KaraokeStyleEditorSkin())
                                {
                                    RelativeSizeAxes = Axes.Both,
                                }
                            }
                        }
                    },
                }
            };

            colorAreaDropdown.Current.BindValueChanged(value => { brushTypeDropdown.Current.Value = drawableLyricLine.GetBrushInfo(value.NewValue).Type; });

            brushTypeDropdown.Current.BindValueChanged(value => drawableLyricLine.ApplyProperty(selectedColorArea, x => x.Type = value.NewValue));

            fontAreaDropdown.Current.BindValueChanged(value =>
            {
                var fontInfo = drawableLyricLine.GetFontInfo(value.NewValue);
                boldCheckbox.Current.Value = fontInfo.Bold;
                fontSizeSliderBar.Current.Value = fontInfo.CharSize;
                borderSliderBar.Current.Value = fontInfo.EdgeSize;
            });

            boldCheckbox.Current.BindValueChanged(value => drawableLyricLine.ApplyProperty(selectedFontArea, x => x.Bold = value.NewValue));
            fontSizeSliderBar.Current.BindValueChanged(value => drawableLyricLine.ApplyProperty(selectedFontArea, x => x.CharSize = value.NewValue));
            borderSliderBar.Current.BindValueChanged(value => drawableLyricLine.ApplyProperty(selectedFontArea, x => x.EdgeSize = value.NewValue));

            displayShaderCheckbox.Current.BindValueChanged(value =>
            {
                var karaokeFont = drawableLyricLine.Font;
                shadowXSliderBar.Current.Value = karaokeFont.ShadowOffset.X;
                shadowYSliderBar.Current.Value = karaokeFont.ShadowOffset.Y;

                // Update view
                fontSection.Children.Where(x => x.Name.StartsWith("Shadow ")).ForEach(x => x.Alpha = value.NewValue ? 1 : 0);

                // Update property
                drawableLyricLine.ApplyProperty(x => x.UseShadow = value.NewValue);
            });

            shadowXSliderBar.Current.BindValueChanged(value => drawableLyricLine.ApplyProperty(x => x.ShadowOffset = new Vector2(value.NewValue, x.ShadowOffset.Y)));
            shadowYSliderBar.Current.BindValueChanged(value => drawableLyricLine.ApplyProperty(x => x.ShadowOffset = new Vector2(x.ShadowOffset.X, value.NewValue)));

            AddStep("Test", () => initialLyricLine(createDefaultLyricLine()));
        }

        private void initialLyricLine(Lyric lyricLine) => layoutArea.Child = drawableLyricLine = new TestDrawableLyricLine(this, lyricLine);

        private Lyric createDefaultLyricLine()
        {
            var startTime = Time.Current;
            const double duration = 1000000;

            return new Lyric
            {
                StartTime = startTime,
                Duration = duration,
                Text = "カラオケ！",
                TimeTags = TimeTagsUtils.ToTimeTagList(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(0), startTime + 500 },
                    { new TimeTagIndex(1), startTime + 600 },
                    { new TimeTagIndex(2), startTime + 1000 },
                    { new TimeTagIndex(3), startTime + 1500 },
                    { new TimeTagIndex(4), startTime + 2000 },
                }),
                RubyTags = new[]
                {
                    new RubyTag
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Text = "か"
                    },
                    new RubyTag
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Text = "お"
                    }
                },
                RomajiTags = new[]
                {
                    new RomajiTag
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ra"
                    },
                    new RomajiTag
                    {
                        StartIndex = 3,
                        EndIndex = 4,
                        Text = "ke"
                    }
                }
            };
        }

        public class TestDrawableLyricLine : DrawableLyric
        {
            private readonly TestSceneLyricLineStyle testScene;

            public KaraokeFont Font { get; private set; }
            private bool defaultValueAssigned;

            public TestDrawableLyricLine(TestSceneLyricLineStyle testCase, Lyric hitObject)
                : base(hitObject)
            {
                testScene = testCase;
            }

            protected override void ApplySkin(ISkinSource skin, bool allowFallback)
            {
                // Get layout
                Font = skin?.GetConfig<KaraokeSkinLookup, KaraokeFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.Singers))?.Value;
                base.ApplySkin(skin, allowFallback);
            }

            protected override void ApplyFont(KaraokeFont font)
            {
                base.ApplyFont(font);

                if (defaultValueAssigned)
                    return;

                defaultValueAssigned = true;

                // Assign default values here
                testScene.colorAreaDropdown.Current.TriggerChange();
                testScene.fontAreaDropdown.Current.TriggerChange();
                testScene.displayShaderCheckbox.Current.Value = Font.UseShadow;
            }

            protected override void ApplyLayout(KaraokeLayout layout)
            {
                // use my own layout
                base.ApplyLayout(new KaraokeLayout
                {
                    Name = "Skin layout",
                    Alignment = Anchor.Centre
                });
            }

            public BrushInfo GetBrushInfo(ColorArea area)
            {
                switch (area)
                {
                    case ColorArea.Front_Text:
                        return Font.FrontTextBrushInfo.TextBrush;

                    case ColorArea.Front_Border:
                        return Font.FrontTextBrushInfo.BorderBrush;

                    case ColorArea.Front_Shadow:
                        return Font.FrontTextBrushInfo.ShadowBrush;

                    case ColorArea.Back_Text:
                        return Font.BackTextBrushInfo.TextBrush;

                    case ColorArea.Back_Border:
                        return Font.BackTextBrushInfo.BorderBrush;

                    case ColorArea.Back_Shadow:
                        return Font.BackTextBrushInfo.ShadowBrush;
                }

                return null;
            }

            public FontInfo GetFontInfo(FontArea area)
            {
                switch (area)
                {
                    case FontArea.Lyric:
                        return Font.LyricTextFontInfo.LyricTextFontInfo;

                    case FontArea.Ruby:
                        return Font.RubyTextFontInfo.LyricTextFontInfo;

                    case FontArea.Romaji:
                        return Font.RomajiTextFontInfo.LyricTextFontInfo;
                }

                return null;
            }

            public void ApplyProperty(Action<KaraokeFont> action)
            {
                action.Invoke(Font);
                ApplyFont(Font);
            }

            public void ApplyProperty(ColorArea area, Action<BrushInfo> action)
            {
                action.Invoke(GetBrushInfo(area));
                ApplyFont(Font);
            }

            public void ApplyProperty(FontArea info, Action<FontInfo> action)
            {
                action.Invoke(GetFontInfo(info));
                ApplyFont(Font);
            }
        }

        public class EditSection : FillFlowContainer
        {
            public EditSection()
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                Spacing = new Vector2(0, 5);
            }
        }

        public class ColorPicker : Box
        {
            public Bindable<Color4> CurrentColor { get; set; } = new Bindable<Color4>();

            public ColorPicker()
            {
                Height = 200;
                Colour = Color4.Blue;
            }
        }

        public enum ColorArea
        {
            Front_Text,

            Front_Border,

            Front_Shadow,

            Back_Text,

            Back_Border,

            Back_Shadow
        }

        public enum FontArea
        {
            Lyric,

            Ruby,

            Romaji
        }

        public enum Font
        {
            F001,

            F002,

            F003
        }

        public class KaraokeStyleEditorSkin : KaraokeInternalSkin
        {
            protected override string ResourceName => @"osu.Game.Rulesets.Karaoke.Resources.Skin.editor.skin";
        }
    }
}
