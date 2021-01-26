// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menu;
using osu.Game.Rulesets.Karaoke.Edit.Export;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Objects;
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
        private DrawableKaraokeEditRuleset drawableRuleset;

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        [Cached]
        private readonly KaraokeRulesetEditConfigManager editConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        [Cached]
        private readonly ExportLyricManager exportLyricManager;

        [Cached]
        private readonly NoteManager noteManager;

        [Cached]
        private readonly LyricManager lyricManager;

        [Cached]
        private readonly SingerManager singerManager;

        [Resolved]
        private Editor editor { get; set; }

        public KaraokeHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
            // Duplicated registration because selection handler need to use it.
            positionCalculator = new PositionCalculator(9);
            editConfigManager = new KaraokeRulesetEditConfigManager();
            generatorConfigManager = new KaraokeRulesetEditGeneratorConfigManager();

            AddInternal(exportLyricManager = new ExportLyricManager());
            AddInternal(noteManager = new NoteManager());
            AddInternal(lyricManager = new LyricManager());
            AddInternal(singerManager = new SingerManager());
            LayerBelowRuleset.Add(new KaraokeLyricEditor(ruleset)
            {
                RelativeSizeAxes = Axes.Both
            });
        }

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

            if (result.Playfield is NotePlayfield)
            {
                // Apply Y value because it's disappeared.
                result.ScreenSpacePosition.Y = screenSpacePosition.Y;
                // then disable time change by moving x
                result.Time = null;
            }

            return result;
        }

        protected override DrawableRuleset<KaraokeHitObject> CreateDrawableRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            => drawableRuleset = new DrawableKaraokeEditRuleset(ruleset, beatmap, mods);

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
                            new LyricEditorEditModeMenu(editConfigManager, "Lyric editor mode"),
                            new LyricEditorLeftSideModeMenu(editConfigManager, "Lyric editor left side mode"),
                            new LyricEditorMovingCursorModeMenu(editConfigManager, "Record cursor moving mode"),
                            new LyricEditorTextSizeMenu(editConfigManager, "Text size"),
                            new EditorMenuItemSpacer(),
                            new NoteEditorPreviewMenu(editConfigManager, "Note editor"),
                        }
                    },
                    new ToolsMenu(editor, "Tools"),
                    new MenuItem("Options")
                    {
                        Items = new MenuItem[]
                        {
                            new EditorMenuItem("Lyric editor"),
                            new GeneratorConfigMenu("Generator"),
                        }
                    }
                };
            });
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();

        protected override IEnumerable<TernaryButton> CreateTernaryButtons() => Array.Empty<TernaryButton>();

        [BackgroundDependencyLoader]
        private void load()
        {
            CreateMenuBar();
        }
    }
}
