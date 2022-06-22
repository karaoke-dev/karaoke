// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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
        private void load(ILyricLanguageChangeHandler lyricLanguageChangeHandler, ILyricSelectionState lyricSelectionState, OsuColour colours)
        {
            languageBindable.BindValueChanged(value =>
            {
                var language = value.NewValue;
                updateBadgeText(language);

                if (lyricSelectionState.Selecting.Value)
                    return;

                lyricLanguageChangeHandler.SetLanguage(language);
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
