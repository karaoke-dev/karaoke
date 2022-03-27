// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
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
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class AssignLanguageSubsection : SelectLyricButton, IHasPopover
    {
        protected override string StandardText => "Change language";

        protected override string SelectingText => "Cancel change language";

        private readonly Bindable<CultureInfo> bindableLanguage = new();

        private readonly List<HitObject> selectedLyrics = new();

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, ILyricLanguageChangeHandler lyricLanguageChangeHandler, ILyricCaretState lyricCaretState, EditorBeatmap beatmap)
        {
            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                selectedLyrics.Clear();
                selectedLyrics.AddRange(beatmap.SelectedHitObjects);

                this.ShowPopover();
            };

            bindableLanguage.BindValueChanged(e =>
            {
                var language = e.NewValue;
                if (language == null)
                    return;

                // Note: because selected hit objects was cleared after end of selection.
                // So should re-add them into the selection list again.
                beatmap.SelectedHitObjects.AddRange(selectedLyrics);
                lyricLanguageChangeHandler.SetLanguage(language);

                this.HidePopover();
                bindableLanguage.Value = null;

                // after apply the language, should make sure that should sync the selected hit object again.
                lyricCaretState.SyncSelectedHitObjectWithCaret();
            });
        }

        public Popover GetPopover()
            => new LanguageSelectorPopover(bindableLanguage);
    }
}
