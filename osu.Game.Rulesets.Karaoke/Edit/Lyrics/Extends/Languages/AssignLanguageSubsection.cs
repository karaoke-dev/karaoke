// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class AssignLanguageSubsection : SelectLyricButton, IHasPopover
    {
        protected override LocalisableString StandardText => "Change language";

        protected override LocalisableString SelectingText => $"Cancel change language({CultureInfoUtils.GetLanguageDisplayText(bindableLanguage.Value)})";

        private readonly Bindable<CultureInfo> bindableLanguage = new();

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, ILyricLanguageChangeHandler lyricLanguageChangeHandler)
        {
            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                {
                    bindableLanguage.Value = null;
                    return;
                }

                lyricLanguageChangeHandler.SetLanguage(bindableLanguage.Value);
                bindableLanguage.Value = null;
            };

            bindableLanguage.BindValueChanged(e =>
            {
                var language = e.NewValue;
                if (language == null)
                    return;

                this.HidePopover();
                StartSelectingLyrics();
            });
        }

        protected override void StartSelectingLyrics()
        {
            // before start selecting, we should make sure that language has been assigned.
            if (bindableLanguage.Value == null)
            {
                this.ShowPopover();
                return;
            }

            base.StartSelectingLyrics();
        }

        public Popover GetPopover()
            => new LanguageSelectorPopover(bindableLanguage);
    }
}
