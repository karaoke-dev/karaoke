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
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Language
{
    public class AssignLanguageSubsection : SelectLyricButton, IHasPopover
    {
        private readonly Bindable<CultureInfo> bindableLanguage = new();

        [Resolved]
        private ILyricLanguageChangeHandler lyricLanguageChangeHandler { get; set; }

        public AssignLanguageSubsection()
        {
            bindableLanguage.BindValueChanged(e =>
            {
                var language = e.NewValue;
                if (language == null)
                    return;

                this.HidePopover();
                StartSelectingLyrics();
            });
        }

        protected override LocalisableString StandardText => "Change language";

        protected override LocalisableString SelectingText => $"Cancel change language({CultureInfoUtils.GetLanguageDisplayText(bindableLanguage.Value)})";

        protected override void Apply()
        {
            lyricLanguageChangeHandler.SetLanguage(bindableLanguage.Value);
            bindableLanguage.Value = null;
        }

        protected override void Cancel()
        {
            bindableLanguage.Value = null;
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
