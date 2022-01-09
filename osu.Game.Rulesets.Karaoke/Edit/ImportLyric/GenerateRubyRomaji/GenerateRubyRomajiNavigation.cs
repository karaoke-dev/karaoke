// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRubyRomaji
{
    public class GenerateRubyRomajiNavigation : TopNavigation<GenerateRubyRomajiStepScreen>
    {
        private const string auto_generate_ruby = "AUTO_GENERATE_RUBY";
        private const string auto_generate_romaji = "AUTO_GENERATE_ROMAJI";

        public GenerateRubyRomajiNavigation(GenerateRubyRomajiStepScreen screen)
            : base(screen)
        {
        }

        protected override NavigationTextContainer CreateTextContainer()
            => new GenerateRubyTextFlowContainer(Screen);

        protected override NavigationState GetState(Lyric[] lyrics)
        {
            // technically, all non-english lyric should have romaji.
            if (lyrics.All(hasRomaji))
                return NavigationState.Done;

            // not all (japanese) lyric contains ruby, so it's ok with that.
            if (lyrics.Any(hasRuby) || lyrics.Any(hasRomaji))
                return NavigationState.Working;

            return NavigationState.Initial;

            static bool hasRuby(Lyric lyric)
                => lyric.RubyTags != null && lyric.RubyTags.Any();

            static bool hasRomaji(Lyric lyric)
                => lyric.RubyTags != null && lyric.RubyTags.Any();
        }

        protected override string GetNavigationText(NavigationState value) =>
            value switch
            {
                NavigationState.Initial => $"Lazy to typing ruby? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] to auto-generate ruby and romaji. It's very easy.",
                NavigationState.Working => $"Go to next step to generate time-tag. Messing around? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] again.",
                NavigationState.Done => $"Go to next step to generate time-tag. Messing around? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] again.",
                NavigationState.Error => "Oops, seems cause some error in here.",
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };

        protected override bool AbleToNextStep(NavigationState value)
            => value is NavigationState.Initial or NavigationState.Working or NavigationState.Done;

        private class GenerateRubyTextFlowContainer : NavigationTextContainer
        {
            public GenerateRubyTextFlowContainer(GenerateRubyRomajiStepScreen screen)
            {
                AddLinkFactory(auto_generate_ruby, "auto generate ruby", screen.AskForAutoGenerateRuby);
                AddLinkFactory(auto_generate_romaji, "auto generate romaji", screen.AskForAutoGenerateRomaji);
            }
        }
    }
}
