// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class LyricImporterStepScreenWithLyricEditor : LyricImporterStepScreenWithTopNavigation
    {
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

            public void PrepareAutoGenerate()
            {
                // todo: open the selection
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
