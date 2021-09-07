// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneLyricEditor : EditorClockTestScene
    {
        [Cached]
        private readonly KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;
        private LanguageSelectionDialog languageSelectionDialog;
        private ImportLyricManager importManager;
        private LyricManager lyricManager;
        private LyricCheckerManager lyricCheckManager;

        private LyricEditor editor;

        public TestSceneLyricEditor()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();

            lyricEditorConfigManager = new KaraokeRulesetLyricEditorConfigManager();
        }

        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap =
            new(new TestKaraokeBeatmap(null));

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            Dependencies.CacheAs(editorBeatmap);
            Dependencies.CacheAs<IBeatSnapProvider>(editorBeatmap);

            base.Content.AddRange(new Drawable[]
            {
                new KaraokeEditInputManager(new KaraokeRuleset().RulesetInfo)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = Content,
                },
                dialogOverlay = new DialogOverlay(),
                languageSelectionDialog = new LanguageSelectionDialog(),
                importManager = new ImportLyricManager(),
                lyricManager = new LyricManager(),
                lyricCheckManager = new LyricCheckerManager(),
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(languageSelectionDialog);
            Dependencies.Cache(importManager);
            Dependencies.Cache(lyricManager);
            Dependencies.Cache(lyricCheckManager);
        }

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Margin = new MarginPadding(10),
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(10),
                            Children = new Drawable[]
                            {
                                new OsuDropdown<LyricEditorMode>
                                {
                                    Width = 150,
                                    Items = EnumUtils.GetValues<LyricEditorMode>(),
                                }.With(x =>
                                {
                                    x.Current.BindValueChanged(mode =>
                                    {
                                        editor.Mode = mode.NewValue;
                                    });
                                }),
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "+",
                                    Action = () =>
                                    {
                                        var fontSize = lyricEditorConfigManager.Get<float>(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize);
                                        lyricEditorConfigManager.SetValue(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, fontSize + 3);
                                    }
                                },
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "-",
                                    Action = () =>
                                    {
                                        var fontSize = lyricEditorConfigManager.Get<float>(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize);
                                        lyricEditorConfigManager.SetValue(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, fontSize - 3);
                                    }
                                },
                            }
                        }
                    },
                    new Drawable[]
                    {
                        editor = new LyricEditor
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    }
                }
            };
        });
    }
}
