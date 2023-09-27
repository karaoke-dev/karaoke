// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;

public partial class ReferenceLyricAutoGenerateSection : AutoGenerateSection
{
    protected override AutoGenerateSubsection CreateAutoGenerateSubsection()
        => new ReferenceLyricAutoGenerateSubsection();

    private partial class ReferenceLyricAutoGenerateSubsection : LyricEditorAutoGenerateSubsection
    {
        public ReferenceLyricAutoGenerateSubsection()
            : base(AutoGenerateType.DetectReferenceLyric)
        {
        }

        protected override DescriptionFormat CreateInvalidDescriptionFormat()
            => new()
            {
                Text = "Seems every lyrics in the songs are unique. But don't worry, reference lyric can still link by hands.",
            };

        protected override ConfigButton CreateConfigButton()
            => new ReferenceLyricAutoGenerateConfigButton();

        protected partial class ReferenceLyricAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new GeneratorConfigPopover(KaraokeRulesetEditGeneratorSetting.ReferenceLyricDetectorConfig);
        }
    }
}
