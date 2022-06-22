// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menus;
using osu.Game.Rulesets.Karaoke.Edit.Export;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit.Components.TernaryButtons;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeHitObjectComposer : HitObjectComposer<KaraokeHitObject>
    {
        private DrawableKaraokeEditorRuleset drawableRuleset;

        [Cached]
        private readonly KaraokeRulesetEditConfigManager editConfigManager;

        [Cached]
        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        [Cached]
        private readonly FontManager fontManager;

        [Cached(typeof(IKaraokeBeatmapResourcesProvider))]
        private KaraokeBeatmapResourcesProvider karaokeBeatmapResourcesProvider;

        [Cached(typeof(ILyricRubyTagsChangeHandler))]
        private readonly LyricRubyTagsChangeHandler lyricRubyTagsChangeHandler;

        [Cached(typeof(ILyricRomajiTagsChangeHandler))]
        private readonly LyricRomajiTagsChangeHandler lyricRomajiTagsChangeHandler;

        [Cached(typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached(typeof(INotesChangeHandler))]
        private readonly NotesChangeHandler notesChangeHandler;

        [Cached(typeof(ILyricSingerChangeHandler))]
        private readonly LyricSingerChangeHandler lyricSingerChangeHandler;

        [Cached(typeof(ISingersChangeHandler))]
        private readonly SingersChangeHandler singersChangeHandler;

        [Cached]
        private readonly ExportLyricManager exportLyricManager;

        [Resolved]
        private Editor editor { get; set; }

        public KaraokeHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
            editConfigManager = new KaraokeRulesetEditConfigManager();
            generatorConfigManager = new KaraokeRulesetEditGeneratorConfigManager();

            // Duplicated registration because selection handler need to use it.
            AddInternal(fontManager = new FontManager());
            AddInternal(karaokeBeatmapResourcesProvider = new KaraokeBeatmapResourcesProvider());

            AddInternal(lyricRubyTagsChangeHandler = new LyricRubyTagsChangeHandler());
            AddInternal(lyricRomajiTagsChangeHandler = new LyricRomajiTagsChangeHandler());
            AddInternal(notePositionInfo = new NotePositionInfo());
            AddInternal(notesChangeHandler = new NotesChangeHandler());
            AddInternal(lyricSingerChangeHandler = new LyricSingerChangeHandler());
            AddInternal(singersChangeHandler = new SingersChangeHandler());

            AddInternal(exportLyricManager = new ExportLyricManager());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            CreateMenuBar();
        }

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public new KaraokePlayfield Playfield => drawableRuleset.Playfield;

        protected override Playfield PlayfieldAtScreenSpacePosition(Vector2 screenSpacePosition)
        {
            // Only note and lyric playfield can interact with mouse input.
            if (Playfield.NotePlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.NotePlayfield;
            if (Playfield.LyricPlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.LyricPlayfield;

            return null;
        }

        public override SnapResult FindSnappedPositionAndTime(Vector2 screenSpacePosition, SnapType snapType = SnapType.All)
        {
            var result = base.FindSnappedPositionAndTime(screenSpacePosition, snapType);

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
            var editorMenuBar = editor.ChildrenOfType<EditorMenuBar>().FirstOrDefault();
            if (editorMenuBar == null)
                return;

            Schedule(() =>
            {
                editorMenuBar.Items = new List<MenuItem>(editorMenuBar.Items)
                {
                    new("Config")
                    {
                        Items = new MenuItem[]
                        {
                            new NoteEditorPreviewMenu(editConfigManager, "Note editor"),
                        }
                    },
                    new("Tools")
                    {
                        Items = new MenuItem[]
                        {
                            new KaraokeSkinEditorMenu(editor, null, "Skin editor"),
                            new KaraokeEditorMenu(editor, "Karaoke editor")
                        }
                    },
                    new("Debug")
                    {
                        Items = new MenuItem[]
                        {
                            new EditorMenuItem("Export to json beatmap", MenuItemType.Destructive, () => exportLyricManager.ExportToJsonBeatmap()),
                        }
                    }
                };
            });
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();

        protected override IEnumerable<TernaryButton> CreateTernaryButtons() => Array.Empty<TernaryButton>();
    }
}
