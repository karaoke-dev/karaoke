// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.IO;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Edit.Layout;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneLayout : EditorClockTestScene
    {
        private readonly KaraokeLayoutTestSkin skin = new KaraokeLayoutTestSkin();

        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        public TestSceneLayout()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);
            Child = new SkinProvidingContainer(skin)
            {
                RelativeSizeAxes = Axes.Both,
                Child = new LayoutScreen(),
            };
        }

        public class KaraokeLayoutTestSkin : KaraokeLegacySkinTransformer
        {
            public KaraokeLayoutTestSkin()
                : base(null)
            {
            }
        }
    }

    [Ignore("Will be removed.")]
    public class TestSceneLyricLineLayout : OsuTestScene
    {
        private readonly OsuTextBox nameTextBox;
        private readonly OsuDropdown<Anchor> alignmentDropdown;
        private readonly OsuSliderBar<int> horizontalMarginSliderBar;
        private readonly OsuSliderBar<int> verticalMarginSliderBar;
        private readonly OsuCheckbox continuousCheckbox;
        private readonly OsuDropdown<KaraokeTextSmartHorizon> smartHorizonDropdown;
        private readonly OsuSliderBar<int> lyricIntervalSliderBar;
        private readonly OsuSliderBar<int> rubyIntervalSliderBar;
        private readonly OsuSliderBar<int> romajiIntervalSliderBar;
        private readonly OsuDropdown<LyricTextAlignment> rubyAlignmentDropdown;
        private readonly OsuDropdown<LyricTextAlignment> romajiAlignmentDropdown;
        private readonly OsuSliderBar<int> rubyMarginSliderBar;
        private readonly OsuSliderBar<int> romajiMarginSliderBar;

        private TestDrawableLyricLine drawableLyricLine;
        private readonly SkinProvidingContainer layoutArea;

        public TestSceneLyricLineLayout()
        {
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 210f)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new Container
                        {
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
                                            Name = "Name section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Name" },
                                                nameTextBox = new OsuTextBox
                                                {
                                                    Width = 200
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Anchor section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Anchor" },
                                                alignmentDropdown = new OsuDropdown<Anchor>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Items = (Anchor[])Enum.GetValues(typeof(Anchor))
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Horizontal margin section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Horizontal margin" },
                                                horizontalMarginSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 500,
                                                        Value = 30,
                                                        Default = 30
                                                    }
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Vertical margin section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Vertical margin" },
                                                verticalMarginSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 500,
                                                        Value = 30,
                                                        Default = 30
                                                    }
                                                }
                                            }
                                        },
                                        continuousCheckbox = new OsuCheckbox
                                        {
                                            Name = "Continuous section",
                                            LabelText = "Continuous"
                                        },
                                        new EditSection
                                        {
                                            Name = "Smart horizon section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Smart horizon" },
                                                smartHorizonDropdown = new OsuDropdown<KaraokeTextSmartHorizon>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Items = (KaraokeTextSmartHorizon[])Enum.GetValues(typeof(KaraokeTextSmartHorizon))
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Lyrics interval section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Lyrics interval" },
                                                lyricIntervalSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 30,
                                                        Value = 10,
                                                        Default = 10
                                                    }
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Ruby interval section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Ruby interval" },
                                                rubyIntervalSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 30,
                                                        Value = 10,
                                                        Default = 10
                                                    }
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Romaji interval section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Romaji interval" },
                                                romajiIntervalSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 30,
                                                        Value = 10,
                                                        Default = 10
                                                    }
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Ruby alignment section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Ruby alignment" },
                                                rubyAlignmentDropdown = new OsuDropdown<LyricTextAlignment>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Items = (LyricTextAlignment[])Enum.GetValues(typeof(LyricTextAlignment))
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Romaji alignment section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Romaji alignment" },
                                                romajiAlignmentDropdown = new OsuDropdown<LyricTextAlignment>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Items = (LyricTextAlignment[])Enum.GetValues(typeof(LyricTextAlignment))
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Ruby margin section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Ruby margin" },
                                                rubyMarginSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 30,
                                                        Value = 10,
                                                        Default = 10
                                                    }
                                                }
                                            }
                                        },
                                        new EditSection
                                        {
                                            Name = "Romaji margin section",
                                            Children = new Drawable[]
                                            {
                                                new OsuSpriteText { Text = "Romaji margin" },
                                                romajiMarginSliderBar = new OsuSliderBar<int>
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Current = new BindableNumber<int>
                                                    {
                                                        MinValue = 0,
                                                        MaxValue = 30,
                                                        Value = 10,
                                                        Default = 10
                                                    }
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    Name = "Setting background",
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.WhiteSmoke
                                },
                                layoutArea = new SkinProvidingContainer(new KaraokeLayoutEditorSkin())
                                {
                                    RelativeSizeAxes = Axes.Both,
                                }
                            }
                        }
                    }
                }
            };

            // Initial bindable
            nameTextBox.Current.BindValueChanged(x =>
            {
                /*TODO : maybe do something in the future.*/
            });
            alignmentDropdown.Current.BindValueChanged(x => applyChange(l => l.Alignment = x.NewValue));
            horizontalMarginSliderBar.Current.BindValueChanged(x => applyChange(l => l.HorizontalMargin = x.NewValue));
            verticalMarginSliderBar.Current.BindValueChanged(x => applyChange(l => l.VerticalMargin = x.NewValue));
            continuousCheckbox.Current.BindValueChanged(x => applyChange(l => l.Continuous = x.NewValue));
            smartHorizonDropdown.Current.BindValueChanged(x => applyChange(l => l.SmartHorizon = x.NewValue));
            lyricIntervalSliderBar.Current.BindValueChanged(x => applyChange(l => l.LyricsInterval = x.NewValue));
            rubyIntervalSliderBar.Current.BindValueChanged(x => applyChange(l => l.RubyInterval = x.NewValue));
            romajiIntervalSliderBar.Current.BindValueChanged(x => applyChange(l => l.RomajiInterval = x.NewValue));
            rubyAlignmentDropdown.Current.BindValueChanged(x => applyChange(l => l.RubyAlignment = x.NewValue));
            romajiAlignmentDropdown.Current.BindValueChanged(x => applyChange(l => l.RomajiAlignment = x.NewValue));
            rubyMarginSliderBar.Current.BindValueChanged(x => applyChange(l => l.RubyMargin = x.NewValue));
            romajiMarginSliderBar.Current.BindValueChanged(x => applyChange(l => l.RomajiMargin = x.NewValue));

            AddStep("Small size lyric layout",
                () => initialLyricLine(createDefaultLyricLine("@カラオケ",
                    new[]
                    {
                        "@Ruby1=カ,か",
                        "@Ruby2=ラ,ら",
                        "@Ruby3=オ,お",
                        "@Ruby4=ケ,け"
                    },
                    new[]
                    {
                        "@Romaji1=カ,ka",
                        "@Romaji2=ラ,ra",
                        "@Romaji3=オ,o",
                        "@Romaji4=ケ,ke"
                    }
                    , "karaoke")));
            AddStep("Medium size lyric layout",
                () => initialLyricLine(createDefaultLyricLine("@[00:18:58]た[00:18:81]だ[00:19:36]風[00:20:09]に[00:20:29]揺[00:20:49]ら[00:20:68]れ[00:20:89]て[00:20:93]",
                    new[]
                    {
                        "@Ruby1=風,かぜ",
                        "@Ruby2=揺,ゆ"
                    },
                    new[]
                    {
                        "@Romaji1=た,ta",
                        "@Romaji2=だ,da",
                        "@Romaji3=風,kaze",
                        "@Romaji4=に,ni",
                        "@Romaji5=揺,yu",
                        "@Romaji6=ら,ra",
                        "@Romaji7=れ,re",
                        "@Romaji8=て,te"
                    }
                    , "karaoke")));
            AddStep("Large size lyric layout", () => initialLyricLine(createDefaultLyricLine("@灰色(いろ)(いろ)の景色(いろ)(いろ)さえ色づき始める",
                Array.Empty<string>(), Array.Empty<string>(), "karaoke")));
        }

        private void initialLyricLine(LyricLine lyricLine) => layoutArea.Child = drawableLyricLine = new TestDrawableLyricLine(this, lyricLine);

        private LyricLine createDefaultLyricLine(string text, string[] ruby, string[] romaji, string translate)
        {
            var startTime = Time.Current;
            const double duration = 1000000;

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new LineBufferedReader(stream))
            {
                writer.WriteLine("karaoke file format v1");
                writer.WriteLine("[HitObjects]");

                writer.WriteLine(text);
                ruby?.ForEach(x => writer.WriteLine(x));
                romaji?.ForEach(x => writer.WriteLine(x));

                writer.WriteLine("end");
                writer.Flush();
                stream.Position = 0;

                var lyric = new KaraokeLegacyBeatmapDecoder().Decode(reader).HitObjects.OfType<LyricLine>().FirstOrDefault();

                // Check is not null
                Assert.IsTrue(lyric != null);

                // Apply property
                lyric.StartTime = startTime;
                lyric.Duration = duration;
                lyric.Translates.Add(0, translate);
                lyric.ApplyDisplayTranslate(0);
                lyric.TimeTags = new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(0), startTime },
                    { new TimeTagIndex(4), startTime + duration },
                };

                return lyric;
            }
        }

        private void applyChange(Action<KaraokeLayout> layoutAction)
        {
            var layout = drawableLyricLine.TestLayout;
            layoutAction.Invoke(layout);
            drawableLyricLine.TestLayout = layout;
        }

        public class TestDrawableLyricLine : DrawableLyricLine
        {
            private readonly TestSceneLyricLineLayout testScene;

            private KaraokeLayout layout;
            private bool defaultValueAssigned;

            public TestDrawableLyricLine(TestSceneLyricLineLayout testScene, LyricLine hitObject)
                : base(hitObject)
            {
                this.testScene = testScene;
            }

            protected override void ApplySkin(ISkinSource skin, bool allowFallback)
            {
                // Get layout
                layout = skin?.GetConfig<KaraokeSkinLookup, KaraokeLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, HitObject.LayoutIndex))?.Value;

                base.ApplySkin(skin, allowFallback);
            }

            public KaraokeLayout TestLayout
            {
                get => layout;
                set
                {
                    layout = value;
                    ApplyLayout(layout);
                }
            }

            protected override void ApplyLayout(KaraokeLayout layout)
            {
                base.ApplyLayout(layout);

                if (defaultValueAssigned)
                    return;

                defaultValueAssigned = true;

                // It's a lazy way to initial test case's component
                testScene.nameTextBox.Current.Value = layout.Name;
                testScene.alignmentDropdown.Current.Value = layout.Alignment;
                testScene.horizontalMarginSliderBar.Current.Value = layout.HorizontalMargin;
                testScene.verticalMarginSliderBar.Current.Value = layout.VerticalMargin;
                testScene.continuousCheckbox.Current.Value = layout.Continuous;
                testScene.smartHorizonDropdown.Current.Value = layout.SmartHorizon;
                testScene.lyricIntervalSliderBar.Current.Value = layout.LyricsInterval;
                testScene.rubyIntervalSliderBar.Current.Value = layout.RubyInterval;
                testScene.romajiIntervalSliderBar.Current.Value = layout.RomajiInterval;
                testScene.rubyAlignmentDropdown.Current.Value = layout.RubyAlignment;
                testScene.romajiAlignmentDropdown.Current.Value = layout.RomajiAlignment;
                testScene.rubyMarginSliderBar.Current.Value = layout.RubyMargin;
                testScene.romajiMarginSliderBar.Current.Value = layout.RomajiMargin;
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

        public class KaraokeLayoutEditorSkin : KaraokeInternalSkin
        {
            protected override string ResourceName => @"osu.Game.Rulesets.Karaoke.Resources.Skin.editor.skin";
        }
    }
}
