// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menu;
using osu.Game.Rulesets.Karaoke.Edit.Export;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit.Components.TernaryButtons;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeHitObjectComposer : HitObjectComposer<KaraokeHitObject>
    {
        private readonly Bindable<EditMode> bindableEditMode = new();

        private DrawableKaraokeEditorRuleset drawableRuleset;

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached]
        private readonly FontManager fontManager;

        [Cached]
        private readonly KaraokeRulesetEditConfigManager editConfigManager;

        [Cached]
        private readonly KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditCheckerConfigManager checkerConfigManager;

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
        private readonly SingerManager singerManager;

        [Cached]
        private LanguageSelectionDialog languageSelectionDialog;

        [Resolved]
        private Editor editor { get; set; }

        public KaraokeHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
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
            AddInternal(singerManager = new SingerManager());
            LayerBelowRuleset.Add(languageSelectionDialog = new LanguageSelectionDialog());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            CreateMenuBar();
            AddInternal(new KaraokeLyricEditor(Ruleset)
            {
                RelativeSizeAxes = Axes.Both
            });

            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                {
                    InternalChildren[0].Hide(); // hide content
                    InternalChildren[1].Hide(); // hide left-side toggle.
                    InternalChildren[2].Show(); // show lyric editor.
                }
                else
                {
                    InternalChildren[0].Show();
                    InternalChildren[1].Show();
                    InternalChildren[2].Hide();
                }
            }, true);
            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
        }

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public new KaraokePlayfield Playfield => drawableRuleset.Playfield;

        public IScrollingInfo ScrollingInfo => drawableRuleset.ScrollingInfo;

        protected override Playfield PlayfieldAtScreenSpacePosition(Vector2 screenSpacePosition)
        {
            // Only note and lyric playfield can interact with mouse input.
            if (Playfield.NotePlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.NotePlayfield;
            if (Playfield.LyricPlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.LyricPlayfield;

            return null;
        }

        public override SnapResult SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition)
        {
            var result = base.SnapScreenSpacePositionToValidTime(screenSpacePosition);

            // should not affect x position and time if dragging object in note playfield.
            return result.Playfield is EditorNotePlayfield
                ? new SnapResult(screenSpacePosition, null, result.Playfield)
                : result;
        }

        protected override DrawableRuleset<KaraokeHitObject> CreateDrawableRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
        {
            drawableRuleset = new DrawableKaraokeEditorRuleset(ruleset, beatmap, mods);

            // This is the earliest we can cache the scrolling info to ourselves, before masks are added to the hierarchy and inject it
            dependencies.CacheAs(drawableRuleset.ScrollingInfo);

            return drawableRuleset;
        }

        protected override ComposeBlueprintContainer CreateBlueprintContainer()
            => new KaraokeBlueprintContainer(this);

        protected void CreateMenuBar()
        {
            // It's a tricky way to place menu bar in here, will be removed eventually.
            var prop = typeof(Editor).GetField("menuBar", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop == null)
                return;

            var menuBar = (EditorMenuBar)prop.GetValue(editor);

            Schedule(() =>
            {
                menuBar.Items = new[]
                {
                    new MenuItem("File")
                    {
                        Items = new MenuItem[]
                        {
                            new ImportLyricMenu(editor, "Import from text"),
                            new ImportLyricMenu(editor, "Import from .lrc file"),
                            new EditorMenuItemSpacer(),
                            new EditorMenuItem("Export to .lrc", MenuItemType.Standard, () => exportLyricManager.ExportToLrc()),
                            new EditorMenuItem("Export to text", MenuItemType.Standard, () => exportLyricManager.ExportToText()),
                            new EditorMenuItem("Export to json", MenuItemType.Destructive, () => exportLyricManager.ExportToJson()),
                        }
                    },
                    new MenuItem("View")
                    {
                        Items = new MenuItem[]
                        {
                            new EditModeMenu(editConfigManager, "Edit mode"),
                            new EditorMenuItemSpacer(),
                            new LyricEditorModeMenu(lyricEditorConfigManager, "Lyric editor mode"),
                            new LyricEditorTextSizeMenu(lyricEditorConfigManager, "Text size"),
                            new AutoFocusToEditLyricMenu(lyricEditorConfigManager, "Auto focus to edit lyric"),
                            new EditorMenuItemSpacer(),
                            new NoteEditorPreviewMenu(editConfigManager, "Note editor"),
                        }
                    },
                    new MenuItem("Tools")
                    {
                        Items = new MenuItem[]
                        {
                            new ManagerMenu(editor, "Manage"),
                            new GeneratorMenu("Generator"),
                        }
                    },
                    new MenuItem("Config")
                    {
                        Items = new MenuItem[]
                        {
                            new EditorMenuItem("Lyric editor"),
                            new GeneratorConfigMenu("Generator"),
                            new LockStateMenu(lyricEditorConfigManager, "Lock"),
                        }
                    }
                };
            });
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();

        protected override IEnumerable<TernaryButton> CreateTernaryButtons() => Array.Empty<TernaryButton>();
    }
}
