// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings
{
    public abstract partial class LyricEditorAutoGenerateSubsection : AutoGenerateSubsection
    {
        private readonly LyricAutoGenerateProperty autoGenerateProperty;

        protected LyricEditorAutoGenerateSubsection(LyricAutoGenerateProperty autoGenerateProperty)
        {
            this.autoGenerateProperty = autoGenerateProperty;
        }

        protected override EditorSectionButton CreateGenerateButton()
            => new AutoGenerateButton(autoGenerateProperty);

        protected sealed override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
            => new LyricEditorDescriptionTextFlowContainer();

        private partial class AutoGenerateButton : SelectLyricButton
        {
            [Resolved, AllowNull]
            private ILyricAutoGenerateChangeHandler lyricAutoGenerateChangeHandler { get; set; }

            private readonly LyricAutoGenerateProperty autoGenerateProperty;

            public AutoGenerateButton(LyricAutoGenerateProperty autoGenerateProperty)
            {
                this.autoGenerateProperty = autoGenerateProperty;
            }

            protected override LocalisableString StandardText => "Generate";

            protected override LocalisableString SelectingText => "Cancel generate";

            protected override IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
            {
                return lyricAutoGenerateChangeHandler.GetNotGeneratableLyrics(autoGenerateProperty);
            }

            protected override void Apply()
            {
                lyricAutoGenerateChangeHandler.AutoGenerate(autoGenerateProperty);
            }
        }
    }
}
