// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

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
        {
            get => lyricEditor.Mode;
            protected set => lyricEditor.Mode = value;
        }

        protected void PrepareAutoGenerate()
        {
            lyricEditor.PrepareAutoGenerate();
        }

        private class ImportLyricEditor : LyricEditor
        {
            [Resolved]
            private LyricImporterSubScreenStack screenStack { get; set; }

            private ILyricSelectionState lyricSelectionState { get; set; }

            private ILyricEditorExtendAreaState lyricEditorExtendAreaState { get; set; }

            public void PrepareAutoGenerate()
            {
                // for some mode, we need to switch to generate section.
                lyricEditorExtendAreaState.ChangeLanguageEditMode(LanguageEditMode.Generate);
                lyricEditorExtendAreaState.ChangeRubyTagEditMode(TextTagEditMode.Generate);
                lyricEditorExtendAreaState.ChangeRomajiTagEditMode(TextTagEditMode.Generate);

                // then open the selecting mode and select all lyrics.
                lyricSelectionState.StartSelecting();
                lyricSelectionState.SelectAll();
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var dependencies = base.CreateChildDependencies(parent);
                lyricSelectionState = dependencies.Get<ILyricSelectionState>();
                lyricEditorExtendAreaState = dependencies.Get<ILyricEditorExtendAreaState>();
                return dependencies;
            }

            public override void NavigateToFix(LyricEditorMode mode)
            {
                switch (mode)
                {
                    case LyricEditorMode.Typing:
                        screenStack.Pop(LyricImporterStep.EditLyric);
                        break;

                    case LyricEditorMode.Language:
                        screenStack.Pop(LyricImporterStep.AssignLanguage);
                        break;

                    case LyricEditorMode.AdjustTimeTag:
                        screenStack.Pop(LyricImporterStep.GenerateTimeTag);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }
    }
}
