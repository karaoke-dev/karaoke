// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorAutoGenerateSubsection : AutoGenerateSubsection
{
    private readonly AutoGenerateType autoGenerateType;

    protected LyricEditorAutoGenerateSubsection(AutoGenerateType generateType)
    {
        this.autoGenerateType = generateType;
    }

    protected override EditorSectionButton CreateGenerateButton()
        => new AutoGenerateButton(autoGenerateType);

    protected sealed override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
        => new LyricEditorDescriptionTextFlowContainer();

    private partial class AutoGenerateButton : SelectLyricButton
    {
        [Resolved]
        private ILyricPropertyAutoGenerateChangeHandler lyricPropertyAutoGenerateChangeHandler { get; set; } = null!;

        private readonly AutoGenerateType autoGenerateType;

        public AutoGenerateButton(AutoGenerateType generateType)
        {
            autoGenerateType = generateType;
        }

        protected override LocalisableString StandardText => "Generate";

        protected override LocalisableString SelectingText => "Cancel generate";

        protected override IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
        {
            return lyricPropertyAutoGenerateChangeHandler.GetGeneratorNotSupportedLyrics(autoGenerateType);
        }

        protected override void Apply()
        {
            lyricPropertyAutoGenerateChangeHandler.AutoGenerate(autoGenerateType);
        }
    }
}
