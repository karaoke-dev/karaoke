// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class UseLanguageDetectorPopupDialog : PopupDialog
    {
        public UseLanguageDetectorPopupDialog(Action<bool> okAction = null)
        {
            Icon = FontAwesome.Solid.Globe;
            HeaderText = "LanguageDetector";
            BodyText = $"Would you like to use language detector to auto assign each lyric's language?";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"OK",
                    Action = () => okAction?.Invoke(true),
                },
                new PopupDialogCancelButton
                {
                    Text = @"Cancel",
                    Action = () => okAction?.Invoke(false),
                },
            };
        }
    }
}
