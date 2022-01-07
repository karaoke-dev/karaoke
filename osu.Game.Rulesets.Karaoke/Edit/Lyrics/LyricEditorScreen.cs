// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorScreen : KaraokeEditorScreen
    {
        private readonly Bindable<LyricEditorMode> bindableLyricEditorMode = new();

        [Cached(typeof(ILyricsChangeHandler))]
        private readonly LyricsChangeHandler lyricsChangeHandler;

        [Cached(typeof(ILyricTextChangeHandler))]
        private readonly LyricTextChangeHandler lyricTextChangeHandler;

        [Cached(typeof(ILyricLanguageChangeHandler))]
        private readonly LyricLanguageChangeHandler lyricLanguageChangeHandler;

        [Cached(typeof(ILyricRubyTagsChangeHandler))]
        private readonly LyricRubyTagsChangeHandler lyricRubyTagsChangeHandler;

        [Cached(typeof(ILyricRomajiChangeHandler))]
        private readonly LyricRomajiChangeHandler lyricRomajiChangeHandler;

        [Cached(typeof(ILyricTimeTagsChangeHandler))]
        private readonly LyricTimeTagsChangeHandler lyricTimeTagsChangeHandler;

        [Cached(typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached(typeof(INotesChangeHandler))]
        private readonly NotesChangeHandler notesChangeHandler;

        [Cached(typeof(ILyricLayoutChangeHandler))]
        private readonly LyricLayoutChangeHandler lyricLayoutChangeHandler;

        [Cached(typeof(ILyricSingerChangeHandler))]
        private readonly LyricSingerChangeHandler lyricSingerChangeHandler;

        [Cached(typeof(ILockChangeHandler))]
        private readonly LockChangeHandler lockChangeHandler;

        public LyricEditorScreen()
            : base(KaraokeEditorScreenMode.Lyric)
        {
            AddInternal(lyricsChangeHandler = new LyricsChangeHandler());
            AddInternal(lyricTextChangeHandler = new LyricTextChangeHandler());
            AddInternal(lyricLanguageChangeHandler = new LyricLanguageChangeHandler());
            AddInternal(lyricRubyTagsChangeHandler = new LyricRubyTagsChangeHandler());
            AddInternal(lyricRomajiChangeHandler = new LyricRomajiChangeHandler());
            AddInternal(lyricTimeTagsChangeHandler = new LyricTimeTagsChangeHandler());
            AddInternal(notePositionInfo = new NotePositionInfo());
            AddInternal(notesChangeHandler = new NotesChangeHandler());
            AddInternal(lyricLayoutChangeHandler = new LyricLayoutChangeHandler());
            AddInternal(lyricSingerChangeHandler = new LyricSingerChangeHandler());
            AddInternal(lockChangeHandler = new LockChangeHandler());

            LyricEditor lyricEditor;
            Add(new KaraokeEditInputManager(new KaraokeRuleset().RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(10),
                Child = lyricEditor = new LyricEditor
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });
            bindableLyricEditorMode.BindValueChanged(e =>
            {
                lyricEditor.Mode = e.NewValue;
            });
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.LyricEditorMode, bindableLyricEditorMode);
        }
    }
}
