// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using J2N.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
{
    public abstract class TextTagAutoGenerateSection : LyricEditorSection
    {
        protected sealed override LocalisableString Title => "Auto generate";

        protected abstract class TextTagAutoGenerateSubsection : AutoGenerateSubsection
        {
            private const string language_mode = "LANGUAGE_MODE";

            protected TextTagAutoGenerateSubsection(LyricAutoGenerateProperty autoGenerateProperty)
                : base(autoGenerateProperty)
            {
            }

            protected override DescriptionFormat CreateInvalidLyricDescriptionFormat()
                => new()
                {
                    Text = $"Seems some lyric missing language, go to [{DescriptionFormat.LINK_KEY_EDIT_MODE}]({language_mode}) to fill the language.",
                    EditModes = new Dictionary<string, SwitchMode>
                    {
                        {
                            language_mode, new SwitchMode
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
