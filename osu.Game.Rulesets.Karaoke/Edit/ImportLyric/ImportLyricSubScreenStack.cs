// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
    public class ImportLyricSubScreenStack : OsuScreenStack
    {
        private readonly Stack<IImportLyricSubScreen> stack = new();

        public void Push(ImportLyricStep step)
        {
            if (CurrentScreen is IImportLyricSubScreen lyricSubScreen)
            {
                if (step == lyricSubScreen.Step)
                    throw new ScreenNotCurrentException("Cannot push same screen.");

                if (step <= lyricSubScreen.Step)
                    throw new ScreenNotCurrentException("Cannot push previous then current screen.");

                if (step != ImportLyricStep.GenerateRuby && step - lyricSubScreen.Step > 1)
                    throw new ScreenNotCurrentException("Only generate ruby step can be skipped.");
            }

            switch (step)
            {
                case ImportLyricStep.ImportLyric:
                    Push(new DragFileSubScreen());
                    return;

                case ImportLyricStep.EditLyric:
                    Push(new EditLyricSubScreen());
                    return;

                case ImportLyricStep.AssignLanguage:
                    Push(new AssignLanguageSubScreen());
                    return;

                case ImportLyricStep.GenerateRuby:
                    Push(new GenerateRubyRomajiSubScreen());
                    return;

                case ImportLyricStep.GenerateTimeTag:
                    Push(new GenerateTimeTagSubScreen());
                    return;

                case ImportLyricStep.Success:
                    Push(new SuccessSubScreen());
                    return;

                default:
                    throw new ScreenNotCurrentException("Screen is not in the lyric import step.");
            }
        }

        public void Push(IImportLyricSubScreen screen)
        {
            stack.Push(screen);
            Push(screen as IScreen);
        }

        public void Pop(ImportLyricStep step)
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
