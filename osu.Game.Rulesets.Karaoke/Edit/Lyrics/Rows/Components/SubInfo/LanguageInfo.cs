// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public class LanguageInfo : SubInfo
    {
        [Resolved]
        private LanguageSelectionDialog languageSelectionDialog { get; set; }

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
                => BadgeText = language?.DisplayName ?? "None";
        }

        protected override bool OnClick(ClickEvent e)
        {
            languageSelectionDialog.Current = Lyric.LanguageBindable;
            languageSelectionDialog.Show();

            return base.OnClick(e);
        }
    }
}
