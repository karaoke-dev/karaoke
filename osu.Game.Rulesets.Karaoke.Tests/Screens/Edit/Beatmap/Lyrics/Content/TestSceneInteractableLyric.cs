// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Content;

public partial class TestSceneInteractableLyric : EditorClockTestScene
{
    private const int border = 36;

    private static readonly Lyric lyric = new()
    {
        Text = "カラオケ",
        TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#o", "[3,start]:4000#ke", "[3,end]:5000" }),
        RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }),
    };

    [Resolved]
    private OsuColour colour { get; set; } = null!;

    [Cached(typeof(EditorBeatmap))]
    private readonly EditorBeatmap editorBeatmap = new(new KaraokeBeatmap
    {
        BeatmapInfo =
        {
            Ruleset = new KaraokeRuleset().RulesetInfo,
        },
    });

    public TestSceneInteractableLyric()
    {
        editorBeatmap.Add(lyric);
        editorBeatmap.SelectedHitObjects.Add(lyric);
    }

    [Test]
    public void TestGridLayer()
    {
        AddToggleStep("Add/remove the grid layer", value =>
        {
            if (value)
            {
                addLoader(new LayerLoader<GridLayer>
                {
                    OnLoad = layer =>
                    {
                        layer.Spacing = 10;
                    },
                });
            }
            else
            {
                removeLoader<GridLayer>();
            }
        });

        AddSliderStep("Change the spacing of the grid", 0, 100, 10, value =>
        {
            changeLayerProperty<GridLayer>(layer =>
            {
                layer.Spacing = value;
            });
        });
    }

    [Test]
    public void TestEditLyricLayer()
    {
        AddToggleStep("Add/remove the edit lyric layer", value =>
        {
            if (value)
            {
                addLoader<EditLyricLayer>();
            }
            else
            {
                removeLoader<EditLyricLayer>();
            }
        });
    }

    [Test]
    public void TestTimeTagLayer()
    {
        AddToggleStep("Add/remove the time-tag layer", value =>
        {
            if (value)
            {
                addLoader<TimeTagLayer>();
            }
            else
            {
                removeLoader<TimeTagLayer>();
            }
        });
    }

    [Test]
    public void TestCaretLayer()
    {
        AddToggleStep("Add/remove the caret layer", value =>
        {
            if (value)
            {
                addLoader<CaretLayer>();
            }
            else
            {
                removeLoader<CaretLayer>();
            }
        });

        AddStep("View mode", () => switchMode(LyricEditorMode.View));
        AddStep("Typing mode", () => switchMode(LyricEditorMode.EditText, TextEditStep.Typing));
        AddStep("Cutting mode", () => switchMode(LyricEditorMode.EditText, TextEditStep.Split));
        AddStep("Edit ruby mode", () => switchMode(LyricEditorMode.EditRuby, RubyTagEditMode.Create));
        AddStep("Edit time-tag mode", () => switchMode(LyricEditorMode.EditTimeTag, TimeTagEditStep.Create));
        AddStep("Record time-tag mode", () => switchMode(LyricEditorMode.EditTimeTag, TimeTagEditStep.Recording));
    }

    [Test]
    public void TestBlueprintLayer()
    {
        AddToggleStep("Add/remove the blueprint layer", value =>
        {
            if (value)
            {
                addLoader<BlueprintLayer>();
            }
            else
            {
                removeLoader<BlueprintLayer>();
            }
        });

        AddStep("Ruby blueprint", () =>
        {
            switchMode(LyricEditorMode.EditRuby, RubyTagEditMode.Create);
            switchEditRubyModeState(RubyTagEditMode.Modify);
        });
        AddStep("Time-tag adjust blueprint", () =>
        {
            switchMode(LyricEditorMode.EditTimeTag, TimeTagEditStep.Adjust);
        });
    }

    [BackgroundDependencyLoader]
    private void load(OsuGameBase game)
    {
        Dependencies.CacheAs<ILyricsProvider>(new LyricsProvider().With(Add));
        Dependencies.CacheAs<ILyricsChangeHandler>(new LyricsChangeHandler().With(Add));
        Dependencies.CacheAs<ILyricTextChangeHandler>(new LyricTextChangeHandler().With(Add));
        Dependencies.CacheAs<ILyricRubyTagsChangeHandler>(new LyricRubyTagsChangeHandler().With(Add));
        Dependencies.CacheAs<ILyricTimeTagsChangeHandler>(new LyricTimeTagsChangeHandler().With(Add));
        Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());

        game.Resources.AddStore(new NamespacedResourceStore<byte[]>(new ShaderResourceStore(), "Resources"));
    }

    #region Testing tools

    private void switchMode(LyricEditorMode mode, Enum? step = null)
    {
        var editorState = this.ChildrenOfType<ILyricEditorState>().First();
        editorState.SwitchMode(mode);

        if (step != null)
        {
            editorState.SwitchEditStep(step);
        }
    }

    private void switchEditRubyModeState(RubyTagEditMode mode)
    {
        var editRubyModeState = this.ChildrenOfType<IEditRubyModeState>().First();
        editRubyModeState.BindableRubyTagEditMode.Value = mode;
    }

    private readonly List<LayerLoader> loaders = new()
    {
        // note: lyric layer should always in the loader and never be removed.
        new LayerLoader<LyricLayer>
        {
            OnLoad = layer =>
            {
                layer.LyricPosition = new Vector2(border);
            },
        },
    };

    private LayerLoader? getLoader<TLayer>()
        => loaders.FirstOrDefault(l => l.GetType().GenericTypeArguments.First() == typeof(TLayer));

    private void addLoader<TLayer>(bool reloadView = true) where TLayer : Layer
    {
        addLoader(new LayerLoader<TLayer>(), reloadView);
    }

    private void addLoader<TLayer>(LayerLoader<TLayer> instance, bool reloadView = true) where TLayer : Layer
    {
        if (getLoader<TLayer>() != null)
        {
            return;
        }

        loaders.Add(instance);

        if (reloadView)
        {
            updateInteractableLyric();
        }
    }

    private void removeLoader<TLoader>(bool reloadView = true)
    {
        var loader = getLoader<TLoader>();

        if (loader != null)
        {
            loaders.Remove(loader);
        }

        if (reloadView)
        {
            updateInteractableLyric();
        }
    }

    private void changeLayerProperty<TLayer>(Action<TLayer> action, bool reloadView = true) where TLayer : Layer
    {
        if (getLoader<TLayer>() == null)
        {
            return;
        }

        removeLoader<TLayer>(false);
        addLoader(new LayerLoader<TLayer>
        {
            OnLoad = action,
        }, false);

        if (reloadView)
        {
            updateInteractableLyric();
        }
    }

    private void updateInteractableLyric()
    {
        RemoveAll(x => x is PopoverContainer, true);
        Add(new PopoverContainer
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            AutoSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colour.BlueDark,
                },
                new MockLyricEditorState
                {
                    AutoSizeAxes = Axes.Both,
                    Padding = new MarginPadding(48),
                    Child = new SkinProvidingContainer(new LyricEditorSkin(null)
                    {
                        FontSize = 48,
                    })
                    {
                        RelativeSizeAxes = Axes.None,
                        AutoSizeAxes = Axes.Both,
                        Child = createInteractableLyric(loaders.ToArray()),
                    },
                },
            },
        });
    }

    private static InteractableLyric createInteractableLyric(LayerLoader[] loaders)
    {
        return new InteractableLyric(lyric)
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            TextSizeChanged = (self, size) =>
            {
                self.Width = size.X + border * 2;
                self.Height = size.Y + border * 2;
            },
            Loaders = reorderLoaders(loaders),
        };
    }

    private static LayerLoader[] reorderLoaders(LayerLoader[] loaders)
    {
        // follow how LyricEditor to change the sort from input loaders
        return loaders.OrderBy(x =>
        {
            var type = x.GetType().GenericTypeArguments.First();
            return type.Name switch
            {
                nameof(GridLayer) => 0,
                nameof(LyricLayer) => 1,
                nameof(EditLyricLayer) => 2,
                nameof(TimeTagLayer) => 3,
                nameof(CaretLayer) => 4,
                nameof(BlueprintLayer) => 5,
                _ => throw new InvalidOperationException(),
            };
        }).ToArray();
    }

    #endregion

    /// <summary>
    /// Follow <see cref="LyricEditor"/> to create the component with necessary DI.
    /// </summary>
    [Cached(typeof(ILyricEditorState))]
    private partial class MockLyricEditorState : Container, ILyricEditorState
    {
        private readonly Bindable<LyricEditorMode> bindableMode = new();
        private readonly Bindable<EditorModeWithEditStep> bindableModeWithEditStep = new();

        public IBindable<LyricEditorMode> BindableMode => bindableMode;

        public IBindable<EditorModeWithEditStep> BindableModeWithEditStep => bindableModeWithEditStep;
        public LyricEditorMode Mode => LyricEditorMode.View;

        [Cached]
        private readonly LyricEditorColourProvider colourProvider = new();

        [Cached(typeof(ILyricCaretState))]
        private readonly LyricCaretState lyricCaretState = new();

        [Cached(typeof(IEditRubyModeState))]
        private readonly EditRubyModeState editRubyModeState = new();

        [Cached(typeof(IEditTimeTagModeState))]
        private readonly EditTimeTagModeState editTimeTagModeState = new();

        /// <summary>
        /// Add the DI into children here for prevent child is removed if call Children = [...] outside.
        /// </summary>
        protected override void LoadComplete()
        {
            base.LoadComplete();

            // global state
            AddInternal(lyricCaretState);

            // state for target mode only.
            AddInternal(editRubyModeState);
            AddInternal(editTimeTagModeState);
        }

        public void SwitchMode(LyricEditorMode mode)
            => bindableMode.Value = mode;

        public void SwitchEditStep<TEditStep>(TEditStep editStep) where TEditStep : Enum
        {
            bindableModeWithEditStep.Value = new EditorModeWithEditStep
            {
                Mode = bindableMode.Value,
                EditStep = editStep,
                Default = false,
            };
        }

        public void NavigateToFix(LyricEditorMode mode)
            => bindableMode.Value = mode;
    }
}
