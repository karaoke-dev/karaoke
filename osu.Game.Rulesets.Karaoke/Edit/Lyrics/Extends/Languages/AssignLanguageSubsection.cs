// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class AssignLanguageSubsection : SelectLyricButton, IHasPopover
    {
        protected override string StandardText => "Change language";

        protected override string SelectingText => "Cancel change language";

        private readonly Bindable<CultureInfo> bindableLanguage = new();

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, ILyricLanguageChangeHandler lyricLanguageChangeHandler)
        {
            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                this.ShowPopover();
            };

            bindableLanguage.BindValueChanged(e =>
            {
                var language = e.NewValue;
                if (language == null)
                    return;

                lyricLanguageChangeHandler.SetLanguage(language);

                this.HidePopover();
                bindableLanguage.Value = null;
            });
        }

        public Popover GetPopover()
            => new LanguageSelectorPopover(bindableLanguage);
    }
}
