// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menu;
using osu.Game.Rulesets.Karaoke.Edit.Export;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Translate;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit.Compose;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    [Cached(Type = typeof(IPlacementHandler))]
    public class KaraokeEditor : GenericEditor<KaraokeEditorScreenMode>, IPlacementHandler
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Green);

        [Cached]
        private readonly KaraokeRulesetEditConfigManager editConfigManager;

        [Cached]
        private readonly KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditCheckerConfigManager checkerConfigManager;

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached]
        private readonly FontManager fontManager;

        [Cached]
        private readonly ExportLyricManager exportLyricManager;

        [Cached]
        private readonly NoteManager noteManager;

        [Cached]
        private readonly LyricManager lyricManager;

        [Cached]
        private readonly RubyRomajiManager rubyRomajiManager;

        [Cached]
        private readonly LyricCheckerManager lyricCheckerManager;

        [Cached]
        private LanguageSelectionDialog languageSelectionDialog;

        [Resolved]
        private EditorBeatmap editorBeatmap { get; set; }

        public KaraokeEditor()
        {
            editConfigManager = new KaraokeRulesetEditConfigManager();
            lyricEditorConfigManager = new KaraokeRulesetLyricEditorConfigManager();
            generatorConfigManager = new KaraokeRulesetEditGeneratorConfigManager();
            checkerConfigManager = new KaraokeRulesetEditCheckerConfigManager();

            // Duplicated registration because selection handler need to use it.
            AddInternal(notePositionInfo = new NotePositionInfo());
            AddInternal(fontManager = new FontManager());

            AddInternal(exportLyricManager = new ExportLyricManager());
            AddInternal(noteManager = new NoteManager());
            AddInternal(lyricManager = new LyricManager());
            AddInternal(rubyRomajiManager = new RubyRomajiManager());
            AddInternal(lyricCheckerManager = new LyricCheckerManager());
            AddInternal(languageSelectionDialog = new LanguageSelectionDialog());
        }

        [BackgroundDependencyLoader(true)]
        private void load()
        {
        }

        protected override GenericEditorScreen<KaraokeEditorScreenMode> GenerateScreen(KaraokeEditorScreenMode screenMode)
        {
            switch (screenMode)
            {
                case KaraokeEditorScreenMode.Lyric:
                    return new LyricEditorScreen();

                case KaraokeEditorScreenMode.Singer:
                    return new SingerScreen();

                case KaraokeEditorScreenMode.Translate:
                    return new TranslateScreen();

                default:
                    throw new InvalidOperationException("Editor menu bar switched to an unsupported mode");
            }
        }

        protected override MenuItem[] GenerateMenuItems(KaraokeEditorScreenMode screenMode)
        {
            switch (screenMode)
            {
                case KaraokeEditorScreenMode.Lyric:
                    return new MenuItem[]
                    {
                        new("File")
                        {
                            Items = new MenuItem[]
                            {
                                new ImportLyricMenu(this, "Import from text"),
                                new ImportLyricMenu(this, "Import from .lrc file"),
                                new EditorMenuItemSpacer(),
                                new EditorMenuItem("Export to .lrc", MenuItemType.Standard, () => exportLyricManager.ExportToLrc()),
                                new EditorMenuItem("Export to text", MenuItemType.Standard, () => exportLyricManager.ExportToText()),
                                new EditorMenuItem("Export to json", MenuItemType.Destructive, () => exportLyricManager.ExportToJson()),
                            }
                        },
                        new("View")
                        {
                            Items = new MenuItem[]
                            {
                                new LyricEditorModeMenu(lyricEditorConfigManager, "Lyric editor mode"),
                                new LyricEditorTextSizeMenu(lyricEditorConfigManager, "Text size"),
                                new AutoFocusToEditLyricMenu(lyricEditorConfigManager, "Auto focus to edit lyric"),
                                new EditorMenuItemSpacer(),
                                new NoteEditorPreviewMenu(editConfigManager, "Note editor"),
                            }
                        },
                        new("Tools")
                        {
                            Items = new MenuItem[]
                            {
                                // todo: maybe place menu item for navigate to skin editor.
                            }
                        },
                        new("Config")
                        {
                            Items = new MenuItem[]
                            {
                                new EditorMenuItem("Lyric editor"),
                                new GeneratorConfigMenu("Generator"),
                                new LockStateMenu(lyricEditorConfigManager, "Lock"),
                            }
                        }
                    };

                default:
                    return null;
            }
        }

        #region IPlacementHandler

        public void BeginPlacement(HitObject hitObject)
        {
            editorBeatmap.PlacementObject.Value = hitObject;
        }

        public void EndPlacement(HitObject hitObject, bool commit)
        {
            editorBeatmap.PlacementObject.Value = null;

            if (commit)
            {
                editorBeatmap.Add(hitObject);
            }
        }

        public void Delete(HitObject hitObject) => editorBeatmap.Remove(hitObject);

        #endregion
    }
}
