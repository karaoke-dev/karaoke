// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menu;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneEditorMenuBar : OsuTestScene
    {
        public TestSceneEditorMenuBar()
        {
            Add(new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                Height = 50,
                Y = 50,
                Child = new EditorMenuBar
                {
                    RelativeSizeAxes = Axes.Both,
                    Items = new[]
                    {
                        new MenuItem("File")
                        {
                            Items = new[]
                            {
                                new EditorMenuItem("Import from text"),
                                new EditorMenuItem("Import from .lrc file"),
                                new EditorMenuItemSpacer(),
                                new EditorMenuItem("Export to .lrc"),
                                new EditorMenuItem("Export to text"),
                            }
                        },
                        new MenuItem("View")
                        {
                            Items = new MenuItem[]
                            {
                                new EditModeMenu(),
                                new EditorMenuItemSpacer(),
                                new LyricEditorEditModeMenu(),
                                new LyricEditorLeftSideModeMenu(),
                                new LyricEditorTextSizeMenu(),
                            }
                        },
                        new MenuItem("Tools")
                        {
                            Items = new MenuItem[]
                            {
                                new EditorMenuItem("Singer manager"),
                                new EditorMenuItem("Translate manager"),
                                new EditorMenuItem("Layout manager"),
                                new EditorMenuItem("Style manager"),
                            }
                        },
                        new MenuItem("Options")
                        {
                            Items = new MenuItem[]
                            {
                                new EditorMenuItem("Lyric editor"),
                                new GeneratorConfigMenu(),
                            }
                        }
                    }
                }
            });
        }
    }
}
