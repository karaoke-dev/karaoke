// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag
{
    public class GenerateTimeTagNavigation : TopNavigation<GenerateTimeTagStepScreen>
    {
        private const string auto_generate_time_tag = "AUTO_GENERATE_TIME_TAG";

        public GenerateTimeTagNavigation(GenerateTimeTagStepScreen screen)
            : base(screen)
        {
        }

        protected override NavigationTextContainer CreateTextContainer()
            => new GenerateTimeTagTextFlowContainer(Screen);

        protected override void UpdateState(NavigationState value)
        {
            base.UpdateState(value);

            switch (value)
            {
                case NavigationState.Initial:
                    NavigationText = $"Press [{auto_generate_time_tag}] to auto-generate time tag. It's very easy.";
                    break;

                case NavigationState.Working:
                case NavigationState.Done:
                    NavigationText = $"Cool, you can reset your time-tag by pressing [{auto_generate_time_tag}]";
                    break;

                case NavigationState.Error:
                    NavigationText = "Oops, seems cause some error in here.";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        protected override bool AbleToNextStep(NavigationState value)
            => value is NavigationState.Working or NavigationState.Done;

        private class GenerateTimeTagTextFlowContainer : NavigationTextContainer
        {
            public GenerateTimeTagTextFlowContainer(GenerateTimeTagStepScreen screen)
            {
                AddLinkFactory(auto_generate_time_tag, "auto generate time tag", screen.AskForAutoGenerateTimeTag);
            }
        }
    }
}
