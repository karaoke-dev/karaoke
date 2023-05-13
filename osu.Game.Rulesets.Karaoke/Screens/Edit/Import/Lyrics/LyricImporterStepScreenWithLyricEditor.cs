// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public abstract partial class LyricImporterStepScreenWithLyricEditor : LyricImporterStepScreenWithTopNavigation
{
    // it's a tricky way to let navigation bar able to get the lyric state.
    // not a good solution, but have no better way now.
    [Cached(typeof(ILyricEditorState))]
    private ImportLyricEditor lyricEditor { get; set; } = null!;

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
        => lyricEditor.SwitchSubMode(mode);

    protected void PrepareAutoGenerate()
    {
        lyricEditor.PrepareAutoGenerate();
    }

    private partial class ImportLyricEditor : LyricEditor
    {
        [Resolved]
        private LyricImporterSubScreenStack screenStack { get; set; } = null!;

        private ILyricSelectionState lyricSelectionState { get; set; } = null!;

        public void PrepareAutoGenerate()
        {
            // then open the selecting mode and select all lyrics.
            lyricSelectionState.StartSelecting();
            lyricSelectionState.SelectAll();

            // for some mode, we need to switch to generate section.
            SwitchSubMode(LanguageEditMode.Generate);
            SwitchSubMode(RubyTagEditMode.Generate);
            SwitchSubMode(RomajiTagEditMode.Generate);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = base.CreateChildDependencies(parent);
            lyricSelectionState = dependencies.Get<ILyricSelectionState>();
            return dependencies;
        }

        public T GetLyricEditorModeState<T>() where T : Enum =>
            BindableModeAndSubMode.Value.GetSubMode<T>();

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
