// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

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

        protected override void UpdateState(NavigationState value)
        {
            base.UpdateState(value);

            switch (value)
            {
                case NavigationState.Initial:
                    NavigationText = $"Lazy to typing ruby? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] to auto-generate ruby and romaji. It's very easy.";
                    break;

                case NavigationState.Working:
                case NavigationState.Done:
                    NavigationText = $"Go to next step to generate time-tag. Messing around? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] again.";
                    break;

                case NavigationState.Error:
                    NavigationText = "Oops, seems cause some error in here.";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        protected override bool AbleToNextStep(NavigationState value)
            => value == NavigationState.Initial || value == NavigationState.Working || value == NavigationState.Done;

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
