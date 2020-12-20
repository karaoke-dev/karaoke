// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        private EditorBeatmap beatmap { get; }

        public Bindable<Mode> BindableMode { get; } = new Bindable<Mode>();

        public Bindable<LyricFastEditMode> BindableFastEditMode { get; } = new Bindable<LyricFastEditMode>();

        public Mode Mode => BindableMode.Value;

        public LyricFastEditMode FastEditMode => BindableFastEditMode.Value;

        public LyricEditorStateManager(EditorBeatmap beatmap)
        {
            this.beatmap = beatmap;
        }

        public void SetMode(Mode mode)
        {
            BindableMode.Value = mode;

            switch (mode)
            {
                case Mode.ViewMode:
                case Mode.EditMode:
                    return;

                case Mode.RecordMode:
                    MoveCursor(CursorAction.First);
                    return;

                case Mode.TimeTagEditMode:
                    return;

                default:
                    throw new IndexOutOfRangeException(nameof(Mode));
            }
        }

        public void SetFastEditMode(LyricFastEditMode fastEditMode)
        {
            BindableFastEditMode.Value = fastEditMode;
        }

        public bool MoveCursor(CursorAction action)
        {
            switch (Mode)
            {
                case Mode.ViewMode:
                    return false;

                case Mode.RecordMode:
                    return moveRecordCursor(action);

                case Mode.EditMode:
                case Mode.TimeTagEditMode:
                    return moveCursor(action);

                default:
                    throw new IndexOutOfRangeException(nameof(Mode));
            }
        }
    }

    public enum Mode
    {
        /// <summary>
        /// Cannot edit anything except each lyric's left-side part.
        /// </summary>
        ViewMode,

        /// <summary>
        /// Can create/delete/mode/split/combine lyric.
        /// </summary>
        EditMode,

        /// <summary>
        /// Click white-space to set current time into time-tag.
        /// </summary>
        RecordMode,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        TimeTagEditMode
    }

    public enum LyricFastEditMode
    {
        /// <summary>
        /// User can only see start and end time.
        /// </summary>
        None,

        /// <summary>
        /// Can edit each lyric's layout.
        /// </summary>
        Layout,

        /// <summary>
        /// Can edit each lyric's singer.
        /// </summary>
        Singer,

        /// <summary>
        /// Can edit each lyric's language.
        /// </summary>
        Language,
    }
}
