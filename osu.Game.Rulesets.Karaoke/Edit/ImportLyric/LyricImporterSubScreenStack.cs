// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Screens;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.EditLyric;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.Success;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public class LyricImporterSubScreenStack : OsuScreenStack
    {
        private readonly Stack<ILyricImporterStepScreen> stack = new();

        public void Push(LyricImporterStep step)
        {
            if (CurrentScreen is ILyricImporterStepScreen lyricSubScreen)
            {
                if (step == lyricSubScreen.Step)
                    throw new ScreenNotCurrentException("Cannot push same screen.");

                if (step <= lyricSubScreen.Step)
                    throw new ScreenNotCurrentException("Cannot push previous then current screen.");

                if (lyricSubScreen.Step == LyricImporterStep.AssignLanguage && step > LyricImporterStep.GenerateTimeTag)
                    throw new ScreenNotCurrentException("Only generate ruby step can be skipped.");
            }

            switch (step)
            {
                case LyricImporterStep.ImportLyric:
                    Push(new DragFileStepScreen());
                    return;

                case LyricImporterStep.EditLyric:
                    Push(new EditLyricStepScreen());
                    return;

                case LyricImporterStep.AssignLanguage:
                    Push(new AssignLanguageStepScreen());
                    return;

                case LyricImporterStep.GenerateRuby:
                    Push(new GenerateRubyRomajiStepScreen());
                    return;

                case LyricImporterStep.GenerateTimeTag:
                    Push(new GenerateTimeTagStepScreen());
                    return;

                case LyricImporterStep.Success:
                    Push(new SuccessStepScreen());
                    return;

                default:
                    throw new ScreenNotCurrentException("Screen is not in the lyric import step.");
            }
        }

        public void Push(ILyricImporterStepScreen screen)
        {
            stack.Push(screen);
            Push(screen as IScreen);
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
}
