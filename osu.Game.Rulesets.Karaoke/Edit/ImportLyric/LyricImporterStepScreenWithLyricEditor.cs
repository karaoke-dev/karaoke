// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class LyricImporterStepScreenWithLyricEditor : LyricImporterStepScreenWithTopNavigation
    {
        // it's a tricky way to let navigation bar able to get the lyric state.
        // not a good solution, but have no better way now.
        [Cached(typeof(ILyricEditorState))]
        private ImportLyricEditor lyricEditor { get; set; }

        [Cached(typeof(ILockChangeHandler))]
        private readonly LockChangeHandler lockChangeHandler;

        protected LyricImporterStepScreenWithLyricEditor()
        {
            AddInternal(lockChangeHandler = new LockChangeHandler());
        }

        protected override Drawable CreateContent()
            => lyricEditor = new ImportLyricEditor
            {
                RelativeSizeAxes = Axes.Both,
            };

        public LyricEditorMode LyricEditorMode
            => lyricEditor.Mode;

        public T GetLyricEditorModeState<T>() where T : Enum
            => lyricEditor.GetLyricEditorModeState<T>();

        public virtual void SwitchLyricEditorMode(LyricEditorMode mode)
            => lyricEditor.SwitchMode(mode);

        public void SwitchToEditModeState<T>(T mode) where T : Enum
            => lyricEditor.SwitchToEditModeState(mode);

        protected void PrepareAutoGenerate()
        {
            lyricEditor.PrepareAutoGenerate();
        }

        private class ImportLyricEditor : LyricEditor
        {
            [Resolved]
            private LyricImporterSubScreenStack screenStack { get; set; }

            private ILyricSelectionState lyricSelectionState { get; set; }

            private IManageModeState manageModeState { get; set; }
            private ILanguageModeState languageModeState { get; set; }
            private IEditRubyModeState editRubyModeState { get; set; }
            private IEditRomajiModeState editRomajiModeState { get; set; }

            public void PrepareAutoGenerate()
            {
                // then open the selecting mode and select all lyrics.
                lyricSelectionState.StartSelecting();
                lyricSelectionState.SelectAll();

                // for some mode, we need to switch to generate section.
                languageModeState.ChangeEditMode(LanguageEditMode.Generate);
                editRubyModeState.ChangeEditMode(TextTagEditMode.Generate);
                editRomajiModeState.ChangeEditMode(TextTagEditMode.Generate);
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var dependencies = base.CreateChildDependencies(parent);
                lyricSelectionState = dependencies.Get<ILyricSelectionState>();
                manageModeState = dependencies.Get<IManageModeState>();
                languageModeState = dependencies.Get<ILanguageModeState>();
                editRubyModeState = dependencies.Get<IEditRubyModeState>();
                editRomajiModeState = dependencies.Get<IEditRomajiModeState>();
                return dependencies;
            }

            public T GetLyricEditorModeState<T>() where T : Enum
            {
                switch (typeof(T))
                {
                    case Type t when t == typeof(TextingEditMode):
                        return EnumUtils.Casting<T>(manageModeState.EditMode);

                    default:
                        throw new NotSupportedException();
                }
            }

            public void SwitchToEditModeState<T>(T mode) where T : Enum
            {
                switch (typeof(T))
                {
                    case Type t when t == typeof(TextingEditMode):
                        manageModeState.ChangeEditMode(EnumUtils.Casting<TextingEditMode>(mode));
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }

            public override void NavigateToFix(LyricEditorMode mode)
            {
                switch (mode)
                {
                    case LyricEditorMode.Texting:
                        screenStack.Pop(LyricImporterStep.EditLyric);
                        break;

                    case LyricEditorMode.Language:
                        screenStack.Pop(LyricImporterStep.AssignLanguage);
                        break;

                    case LyricEditorMode.EditTimeTag:
                        screenStack.Pop(LyricImporterStep.GenerateTimeTag);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }
    }
}
