// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorScreen : KaraokeEditorScreen
    {
        private readonly Bindable<LyricEditorMode> bindableLyricEditorMode = new();

        [Cached(Type = typeof(ILyricsChangeHandler))]
        private readonly LyricsChangeHandler lyricsChangeHandler;

        [Cached(typeof(ILyricRubyChangeHandler))]
        private readonly LyricRubyChangeHandler lyricRubyChangeHandler;

        [Cached(typeof(ILyricRomajiChangeHandler))]
        private readonly LyricRomajiChangeHandler lyricRomajiChangeHandler;

        [Cached(typeof(ILyricTimeTagsChangeHandler))]
        private readonly LyricTimeTagsChangeHandler lyricTimeTagsChangeHandler;

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly NotePositionInfo notePositionInfo;

        [Cached(typeof(INotesChangeHandler))]
        private readonly NotesChangeHandler notesChangeHandler;

        [Cached(typeof(ILyricLayoutChangeHandler))]
        private readonly LyricLayoutChangeHandler lyricLayoutChangeHandler;

        [Cached(typeof(ILyricSingerChangeHandler))]
        private readonly LyricSingerChangeHandler lyricSingerChangeHandler;

        public LyricEditorScreen()
            : base(KaraokeEditorScreenMode.Lyric)
        {
            AddInternal(lyricsChangeHandler = new LyricsChangeHandler());
            AddInternal(lyricRubyChangeHandler = new LyricRubyChangeHandler());
            AddInternal(lyricRomajiChangeHandler = new LyricRomajiChangeHandler());
            AddInternal(lyricTimeTagsChangeHandler = new LyricTimeTagsChangeHandler());
            AddInternal(notePositionInfo = new NotePositionInfo());
            AddInternal(notesChangeHandler = new NotesChangeHandler());
            AddInternal(lyricLayoutChangeHandler = new LyricLayoutChangeHandler());
            AddInternal(lyricSingerChangeHandler = new LyricSingerChangeHandler());

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
