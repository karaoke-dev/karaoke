// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Overlays;
using osu.Game.Overlays.OSD;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorScreen : KaraokeEditorScreen
    {
        [Cached(typeof(ILyricsChangeHandler))]
        private readonly LyricsChangeHandler lyricsChangeHandler;

        [Cached(typeof(ILyricAutoGenerateChangeHandler))]
        private readonly LyricAutoGenerateChangeHandler lyricAutoGenerateChangeHandler;

        [Cached(typeof(ILyricTextChangeHandler))]
        private readonly LyricTextChangeHandler lyricTextChangeHandler;

        [Cached(typeof(ILyricReferenceChangeHandler))]
        private readonly LyricReferenceChangeHandler lyricReferenceChangeHandler;

        [Cached(typeof(ILyricLanguageChangeHandler))]
        private readonly LyricLanguageChangeHandler lyricLanguageChangeHandler;

        [Cached(typeof(ILyricRubyTagsChangeHandler))]
        private readonly LyricRubyTagsChangeHandler lyricRubyTagsChangeHandler;

        [Cached(typeof(ILyricRomajiTagsChangeHandler))]
        private readonly LyricRomajiTagsChangeHandler lyricRomajiTagsChangeHandler;

        [Cached(typeof(ILyricTimeTagsChangeHandler))]
        private readonly LyricTimeTagsChangeHandler lyricTimeTagsChangeHandler;

        [Cached(typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached(typeof(INotesChangeHandler))]
        private readonly NotesChangeHandler notesChangeHandler;

        [Cached(typeof(INotePropertyChangeHandler))]
        private readonly NotePropertyChangeHandler notePropertyChangeHandler;

        [Cached(typeof(ILyricSingerChangeHandler))]
        private readonly LyricSingerChangeHandler lyricSingerChangeHandler;

        [Cached(typeof(ISingersChangeHandler))]
        private readonly SingersChangeHandler singersChangeHandler;

        [Cached(typeof(ILockChangeHandler))]
        private readonly LockChangeHandler lockChangeHandler;

        private readonly FullScreenLyricEditor lyricEditor;

        public LyricEditorScreen()
            : base(KaraokeEditorScreenMode.Lyric)
        {
            AddInternal(lyricsChangeHandler = new LyricsChangeHandler());
            AddInternal(lyricAutoGenerateChangeHandler = new LyricAutoGenerateChangeHandler());
            AddInternal(lyricTextChangeHandler = new LyricTextChangeHandler());
            AddInternal(lyricReferenceChangeHandler = new LyricReferenceChangeHandler());
            AddInternal(lyricLanguageChangeHandler = new LyricLanguageChangeHandler());
            AddInternal(lyricRubyTagsChangeHandler = new LyricRubyTagsChangeHandler());
            AddInternal(lyricRomajiTagsChangeHandler = new LyricRomajiTagsChangeHandler());
            AddInternal(lyricTimeTagsChangeHandler = new LyricTimeTagsChangeHandler());
            AddInternal(notePositionInfo = new NotePositionInfo());
            AddInternal(notesChangeHandler = new NotesChangeHandler());
            AddInternal(notePropertyChangeHandler = new NotePropertyChangeHandler());
            AddInternal(lyricSingerChangeHandler = new LyricSingerChangeHandler());
            AddInternal(singersChangeHandler = new SingersChangeHandler());
            AddInternal(lockChangeHandler = new LockChangeHandler());

            Add(new KaraokeEditInputManager(new KaraokeRuleset().RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Child = lyricEditor = new FullScreenLyricEditor
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(Bindable<LyricEditorMode> lyricEditorMode)
        {
            lyricEditor.BindableMode.BindTo(lyricEditorMode);
        }

        protected override void PopIn()
        {
            base.PopIn();

            // should reset the selection because selected hitobject in the editor beatmap might not sync with the selection in lyric editor.
            lyricEditor.ResetSelectedHitObject();
        }

        private class FullScreenLyricEditor : LyricEditor
        {
            private ILyricCaretState lyricCaretState { get; set; }

            [Resolved(canBeNull: true)]
            private OnScreenDisplay onScreenDisplay { get; set; }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var dependencies = base.CreateChildDependencies(parent);
                lyricCaretState = dependencies.Get<ILyricCaretState>();
                return dependencies;
            }

            public void ResetSelectedHitObject() => lyricCaretState.SyncSelectedHitObjectWithCaret();

            public override bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
            {
                switch (e.Action)
                {
                    case KaraokeEditAction.PreviousEditMode:
                        SwitchMode(EnumUtils.GetPreviousValue(Mode));
                        onScreenDisplay?.Display(new LyricEditorEditModeToast(Mode));
                        return true;

                    case KaraokeEditAction.NextEditMode:
                        SwitchMode(EnumUtils.GetNextValue(Mode));
                        onScreenDisplay?.Display(new LyricEditorEditModeToast(Mode));
                        return true;

                    default:
                        return base.OnPressed(e);
                }
            }
        }

        public class LyricEditorEditModeToast : Toast
        {
            public LyricEditorEditModeToast(LyricEditorMode mode)
                : base(getDescription(), getValue(mode), getShortcut(mode))
            {
            }

            private static LocalisableString getDescription()
                => "Lyric editor";

            private static LocalisableString getValue(LyricEditorMode mode)
                => $"{mode.GetDescription()} Mode";

            private static LocalisableString getShortcut(LyricEditorMode mode)
                => $"Switch to the {mode.GetDescription()} mode";
        }
    }
}
