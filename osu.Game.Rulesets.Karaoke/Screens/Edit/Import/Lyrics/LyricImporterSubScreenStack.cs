// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Screens;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.AssignLanguage;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.DragFile;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.EditLyric;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateRuby;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateTimeTag;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.Success;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class LyricImporterSubScreenStack : OsuScreenStack
{
    private readonly Stack<ILyricImporterStepScreen> stack = new();

    public LyricImporterStep CurrentStep => getStepByScreen(stack.Peek());

    public void Push(LyricImporterStep step)
    {
        Push(getScreenByStep(step));
    }

    public new void Push(IScreen screen)
    {
        if (screen is not ILyricImporterStepScreen newStepScreen)
            throw new NotImportStepScreenException();

        if (CurrentScreen is ILyricImporterStepScreen currentScreen)
        {
            var currentStep = getStepByScreen(currentScreen);
            var newStep = getStepByScreen(newStepScreen);

            checkStep(currentStep, newStep);
        }

        stack.Push(newStepScreen);
        base.Push(screen);
        return;

        static void checkStep(LyricImporterStep currentStep, LyricImporterStep newStep)
        {
            if (newStep == currentStep)
                throw new ScreenNotCurrentException("Cannot push same screen.");

            if (newStep <= currentStep)
                throw new ScreenNotCurrentException("Cannot push previous then current screen.");

            if (currentStep == LyricImporterStep.AssignLanguage && newStep > LyricImporterStep.GenerateTimeTag)
                throw new ScreenNotCurrentException("Only generate ruby step can be skipped.");
        }
    }

    public void Pop(LyricImporterStep step)
    {
        Pop(getScreenByStep(step));
    }

    public void Pop(IScreen screen)
    {
        if (screen is not ILyricImporterStepScreen newStepScreen)
            throw new NotImportStepScreenException();

        var targetScreen = stack.FirstOrDefault(x => x == newStepScreen);
        if (targetScreen == null)
            throw new ScreenNotCurrentException("Screen is not in the lyric import step.");

        targetScreen.MakeCurrent();

        // pop to target stack.
        while (stack.Peek() != targetScreen)
        {
            stack.Pop();
        }
    }

    public bool IsFirstStep()
    {
        return CurrentStep == LyricImporterStep.ImportLyric;
    }

    private static ILyricImporterStepScreen getScreenByStep(LyricImporterStep step) =>
        step switch
        {
            LyricImporterStep.ImportLyric => new DragFileStepScreen(),
            LyricImporterStep.EditLyric => new EditLyricStepScreen(),
            LyricImporterStep.AssignLanguage => new AssignLanguageStepScreen(),
            LyricImporterStep.GenerateRuby => new GenerateRubyStepScreen(),
            LyricImporterStep.GenerateTimeTag => new GenerateTimeTagStepScreen(),
            LyricImporterStep.Success => new SuccessStepScreen(),
            _ => throw new ScreenNotCurrentException("Screen is not in the lyric import step."),
        };

    private static LyricImporterStep getStepByScreen(ILyricImporterStepScreen screen) =>
        screen switch
        {
            DragFileStepScreen => LyricImporterStep.ImportLyric,
            EditLyricStepScreen => LyricImporterStep.EditLyric,
            AssignLanguageStepScreen => LyricImporterStep.AssignLanguage,
            GenerateRubyStepScreen => LyricImporterStep.GenerateRuby,
            GenerateTimeTagStepScreen => LyricImporterStep.GenerateTimeTag,
            SuccessStepScreen => LyricImporterStep.Success,
            _ => throw new ScreenNotCurrentException("Screen is not in the lyric import step."),
        };
}
