// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class AssignLanguageNavigation : TopNavigation<AssignLanguageStepScreen>
    {
        private const string auto_assign_language = "AUTO_ASSIGN_LANGUAGE";

        public AssignLanguageNavigation(AssignLanguageStepScreen screen)
            : base(screen)
        {
        }

        protected override NavigationTextContainer CreateTextContainer()
            => new AssignLanguageTextFlowContainer(Screen);

        protected override NavigationState GetState(Lyric[] lyrics)
        {
            if (lyrics.All(x => x.Language != null))
                return NavigationState.Done;

            if (lyrics.Any(x => x.Language != null))
                return NavigationState.Working;

            return NavigationState.Initial;
        }

        protected override string GetNavigationText(NavigationState value) =>
            value switch
            {
                NavigationState.Initial => $"Try to select left side to mark lyric's language, or click [{auto_assign_language}] to let system auto detect lyric language.",
                NavigationState.Working => $"Almost there, you can still click [{auto_assign_language}] to re-detect each lyric's language.",
                NavigationState.Done => "Cool! Seems all lyric has it's own language. Go to next step to generate ruby.",
                NavigationState.Error => "Oops, seems cause some error in here.",
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };

        private class AssignLanguageTextFlowContainer : NavigationTextContainer
        {
            public AssignLanguageTextFlowContainer(AssignLanguageStepScreen screen)
            {
                AddLinkFactory(auto_assign_language, "language detector", screen.AskForAutoAssignLanguage);
            }
        }
    }
}
