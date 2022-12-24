// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using J2N.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji
{
    public abstract partial class TextTagAutoGenerateSection : AutoGenerateSection
    {
        protected abstract partial class TextTagAutoGenerateSubsection : LyricEditorAutoGenerateSubsection
        {
            private const string language_mode = "LANGUAGE_MODE";

            protected TextTagAutoGenerateSubsection(LyricAutoGenerateProperty autoGenerateProperty)
                : base(autoGenerateProperty)
            {
            }

            protected override DescriptionFormat CreateInvalidDescriptionFormat()
                => new()
                {
                    Text = $"Seems some lyric missing language, go to [{DescriptionFormat.LINK_KEY_ACTION}]({language_mode}) to fill the language.",
                    Actions = new Dictionary<string, IDescriptionAction>
                    {
                        {
                            language_mode, new SwitchModeDescriptionAction
                            {
                                Text = "edit language mode",
                                Mode = LyricEditorMode.Language
                            }
                        }
                    }
                };
        }
    }
}
