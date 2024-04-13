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

    public void Push(LyricImporterStep step)
    {
        if (CurrentScreen != null)
        {
            if (CurrentScreen is not ILyricImporterStepScreen lyricSubScreen)
                throw new NotImportStepScreenException();

            if (step == lyricSubScreen.Step)
                throw new ScreenNotCurrentException("Cannot push same screen.");

            if (step <= lyricSubScreen.Step)
                throw new ScreenNotCurrentException("Cannot push previous then current screen.");

            if (lyricSubScreen.Step == LyricImporterStep.AssignLanguage && step > LyricImporterStep.GenerateTimeTag)
                throw new ScreenNotCurrentException("Only generate ruby step can be skipped.");
        }

        Push(getScreenByStep(step));
        return;

        static ILyricImporterStepScreen getScreenByStep(LyricImporterStep step) =>
            step switch
            {
                LyricImporterStep.ImportLyric => new DragFileStepScreen(),
                LyricImporterStep.EditLyric => new EditLyricStepScreen(),
                LyricImporterStep.AssignLanguage => new AssignLanguageStepScreen(),
                LyricImporterStep.GenerateRuby => new GenerateRubyStepScreen(),
                LyricImporterStep.GenerateTimeTag => new GenerateTimeTagStepScreen(),
                LyricImporterStep.Success => new SuccessStepScreen(),
                _ => throw new ScreenNotCurrentException("Screen is not in the lyric import step.")
            };
    }

    public new void Push(IScreen screen)
    {
        if (screen is not ILyricImporterStepScreen lyricSubScreen)
            throw new NotImportStepScreenException();

        stack.Push(lyricSubScreen);
        base.Push(screen);
    }

    public void Pop(LyricImporterStep step)
    {
        var screen = stack.FirstOrDefault(x => x.Step == step);
        if (screen == null)
            throw new ScreenNotCurrentException("Screen is not in the lyric import step.");

        screen.MakeCurrent();

        // pop to target stack.
        while (stack.Peek().Step != step)
        {
            stack.Pop();
        }
    }
}
