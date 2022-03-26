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
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public class LanguageInfo : SubInfo, IHasPopover
    {
        private readonly Bindable<CultureInfo> languageBindable = new();

        public LanguageInfo(Lyric lyric)
            : base(lyric)
        {
            languageBindable.BindTo(Lyric.LanguageBindable);
        }

        [BackgroundDependencyLoader]
        private void load(ILyricLanguageChangeHandler lyricLanguageChangeHandler, OsuColour colours)
        {
            languageBindable.BindValueChanged(value =>
            {
                // todo : how to mark lyric as selected.
                var language = value.NewValue;
                lyricLanguageChangeHandler.SetLanguage(language);

                updateBadgeText(language);
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
