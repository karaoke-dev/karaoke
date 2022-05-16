// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public class LanguageInfo : SubInfo, IHasPopover
    {
        private readonly Bindable<CultureInfo> languageBindable;

        public LanguageInfo(Lyric lyric)
            : base(lyric)
        {
            languageBindable = lyric.LanguageBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(ILyricLanguageChangeHandler lyricLanguageChangeHandler, ILyricSelectionState lyricSelectionState, EditorBeatmap beatmap, OsuColour colours)
        {
            languageBindable.BindValueChanged(value =>
            {
                var language = value.NewValue;
                updateBadgeText(language);

                if (lyricSelectionState.Selecting.Value)
                    return;

                // todo: it's a temp way for applying the lyric.
                // because assign language does not have the caret algorithm, so cannot apply the selected hit object by changing the caret.
                beatmap.SelectedHitObjects.Add(Lyric);

                lyricLanguageChangeHandler.SetLanguage(language);

                beatmap.SelectedHitObjects.Remove(Lyric);
            });
            updateBadgeText(Lyric.Language);

            BadgeColour = colours.BlueDarker;

            void updateBadgeText(CultureInfo language)
                => BadgeText = CultureInfoUtils.GetLanguageDisplayText(language);
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.ShowPopover();

            return base.OnClick(e);
        }

        public Popover GetPopover()
            => new LanguageSelectorPopover(languageBindable);
    }
}
