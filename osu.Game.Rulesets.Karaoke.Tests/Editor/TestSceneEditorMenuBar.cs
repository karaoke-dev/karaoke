// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menu;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneEditorMenuBar : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var config = new KaraokeRulesetEditConfigManager();
            var lyricEditorConfig = new KaraokeRulesetLyricEditorConfigManager();
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
                            Items = new MenuItem[]
                            {
                                new ImportLyricMenu(null, "Import from text"),
                                new ImportLyricMenu(null, "Import from .lrc file"),
                                new EditorMenuItemSpacer(),
                                new EditorMenuItem("Export to .lrc", MenuItemType.Standard, () => { }),
                                new EditorMenuItem("Export to text", MenuItemType.Standard, () => { }),
                                new EditorMenuItem("Export to json", MenuItemType.Destructive, () => { }),
                            }
                        },
                        new MenuItem("View")
                        {
                            Items = new MenuItem[]
                            {
                                new EditorMenuItemSpacer(),
                                new LyricEditorModeMenu(lyricEditorConfig, "Lyric editor mode"),
                                new LyricEditorTextSizeMenu(lyricEditorConfig, "Text size"),
                                new EditorMenuItemSpacer(),
                                new NoteEditorPreviewMenu(config, "Note editor"),
                            }
                        },
                        new MenuItem("Tools")
                        {
                            Items = new MenuItem[]
                            {
                                // todo: maybe place menu item for navigate to skin editor.
                            }
                        },
                        new MenuItem("Config")
                        {
                            Items = new MenuItem[]
                            {
                                new EditorMenuItem("Lyric editor"),
                                new GeneratorConfigMenu("Generator"),
                            }
                        }
                    }
                }
            });
        }
    }
}
