// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Humanizer;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    // todo : move to other place
    /*
    public class DeleteLanguageDialog : PopupDialog
    {
        public DeleteLanguageDialog(BeatmapSetOnlineLanguage language, Action deleteAction)
        {
            // todo : got real number
            var languageTranslateCount = 10;

            HeaderText = "Confirm deletion of";
            BodyText = $"{language.Name} ({"language".ToQuantity(languageTranslateCount)})";

            Icon = FontAwesome.Regular.TrashAlt;

            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes. Go for it.",
                    Action = deleteAction
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!",
                },
            };
        }
    }
    */
}
