// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.SubInfo
{
    public class LanguageInfo : SubInfo
    {
        [Resolved]
        private LanguageSelectionDialog languageSelectionDialog { get; set; }

        [Resolved]
        private LyricManager lyricManager { get; set; }

        private readonly Bindable<CultureInfo> languageBindable = new Bindable<CultureInfo>();

        public LanguageInfo(Lyric lyric)
            : base(lyric)
        {
            languageBindable.BindValueChanged(value =>
            {
                var language = value.NewValue;
                lyricManager?.SetLanguage(lyric, language);

                BadgeText = language == null ? "None" : language.DisplayName;
            }, true);
            languageBindable.BindTo(lyric.LanguageBindable);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.BlueDarker;
        }

        protected override bool OnClick(ClickEvent e)
        {
            languageSelectionDialog.Current.UnbindAll();
            languageSelectionDialog.Current.BindTo(Lyric.LanguageBindable);
            languageSelectionDialog.Show();

            return base.OnClick(e);
        }
    }
}
