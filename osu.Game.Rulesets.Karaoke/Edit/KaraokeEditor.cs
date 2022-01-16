// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menus;
using osu.Game.Rulesets.Karaoke.Edit.Export;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Translate;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit.Compose;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    [Cached(typeof(IPlacementHandler))]
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

        [Cached]
        private readonly FontManager fontManager;

        [Cached]
        private readonly ExportLyricManager exportLyricManager;

        [Cached]
        private readonly LyricCheckerManager lyricCheckerManager;

        [Cached]
        private LanguageSelectionDialog languageSelectionDialog;

        [Resolved]
        private EditorBeatmap editorBeatmap { get; set; }

        [Resolved]
        private IEditorChangeHandler changeHandler { get; set; }

        public KaraokeEditor()
        {
            editConfigManager = new KaraokeRulesetEditConfigManager();
            lyricEditorConfigManager = new KaraokeRulesetLyricEditorConfigManager();
            generatorConfigManager = new KaraokeRulesetEditGeneratorConfigManager();
            checkerConfigManager = new KaraokeRulesetEditCheckerConfigManager();

            // Duplicated registration because selection handler need to use it.
            AddInternal(fontManager = new FontManager());

            AddInternal(exportLyricManager = new ExportLyricManager());
            AddInternal(lyricCheckerManager = new LyricCheckerManager());
            AddInternal(languageSelectionDialog = new LanguageSelectionDialog());
        }

        protected override GenericEditorScreen<KaraokeEditorScreenMode> GenerateScreen(KaraokeEditorScreenMode screenMode) =>
            screenMode switch
            {
                KaraokeEditorScreenMode.Lyric => new LyricEditorScreen(),
                KaraokeEditorScreenMode.Singer => new SingerScreen(),
                KaraokeEditorScreenMode.Translate => new TranslateScreen(),
                _ => throw new InvalidOperationException("Editor menu bar switched to an unsupported mode")
            };

        protected override MenuItem[] GenerateMenuItems(KaraokeEditorScreenMode screenMode)
        {
            return screenMode switch
            {
                KaraokeEditorScreenMode.Lyric => new MenuItem[]
                {
                    new("File")
                    {
                        Items = new MenuItem[]
                        {
                            new ImportLyricMenu(this, "Import from text", importLyric),
                            new ImportLyricMenu(this, "Import from .lrc file", importLyric),
                            new EditorMenuItemSpacer(),
                            new EditorMenuItem("Export to .lrc", MenuItemType.Standard, () => exportLyricManager.ExportToLrc()),
                            new EditorMenuItem("Export to text", MenuItemType.Standard, () => exportLyricManager.ExportToText()),
                            new EditorMenuItem("Export to json", MenuItemType.Destructive, () => exportLyricManager.ExportToJson()),
                        }
                    },
                    new LyricEditorModeMenu(lyricEditorConfigManager, "Mode"),
                    new("View")
                    {
                        Items = new MenuItem[]
                        {
                            new LyricEditorTextSizeMenu(lyricEditorConfigManager, "Text size"), new AutoFocusToEditLyricMenu(lyricEditorConfigManager, "Auto focus to edit lyric"),
                        }
                    },
                    new("Config")
                    {
                        Items = new MenuItem[] { new EditorMenuItem("Lyric editor"), new GeneratorConfigMenu("Auto-generator"), new LockStateMenu(lyricEditorConfigManager, "Lock"), }
                    },
                    new("Tools") { Items = new MenuItem[] { new KaraokeSkinEditorMenu(this, null, "Skin editor"), } },
                },
                _ => null
            };
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

        private void importLyric(IBeatmap beatmap)
        {
            changeHandler.BeginChange();

            editorBeatmap.Clear();

            if (beatmap.HitObjects.Any())
                editorBeatmap.AddRange(beatmap.HitObjects);

            changeHandler.EndChange();
        }
    }
}
