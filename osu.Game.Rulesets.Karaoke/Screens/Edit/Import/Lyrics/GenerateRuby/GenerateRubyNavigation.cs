// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateRuby;

public partial class GenerateRubyNavigation : TopNavigation<GenerateRubyStepScreen>
{
    private const string auto_generate_ruby = "AUTO_GENERATE_RUBY";

    public GenerateRubyNavigation(GenerateRubyStepScreen screen)
        : base(screen)
    {
    }

    protected override NavigationTextContainer CreateTextContainer()
        => new GenerateRubyTextFlowContainer(Screen);

    protected override NavigationState GetState(Lyric[] lyrics)
    {
        // technically, all non-english lyric should have romanisation.
        if (lyrics.All(hasRomanisation))
            return NavigationState.Done;

        // not all (japanese) lyric contains ruby, so it's ok with that.
        if (lyrics.Any(hasRuby) || lyrics.Any(hasRomanisation))
            return NavigationState.Working;

        return NavigationState.Initial;

        static bool hasRuby(Lyric lyric)
            => lyric.RubyTags.Any();

        static bool hasRomanisation(Lyric lyric)
            => lyric.TimeTags.Any(x => !string.IsNullOrEmpty(x.RomanisedSyllable));
    }

    protected override LocalisableString GetNavigationText(NavigationState value) =>
        value switch
        {
            NavigationState.Initial => $"Lazy to typing ruby? Press [{auto_generate_ruby}].",
            NavigationState.Working => "Go to next step to generate time-tag.",
            NavigationState.Done => "Go to next step to generate time-tag.",
            NavigationState.Error => "Oops, seems cause some error in here.",
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };

    protected override bool AbleToNextStep(NavigationState value)
        => value is NavigationState.Initial or NavigationState.Working or NavigationState.Done;

    private partial class GenerateRubyTextFlowContainer : NavigationTextContainer
    {
        public GenerateRubyTextFlowContainer(GenerateRubyStepScreen screen)
        {
            AddLinkFactory(auto_generate_ruby, "auto generate ruby", screen.AskForAutoGenerateRuby);
        }
    }
}
