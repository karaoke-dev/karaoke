// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class PlaybackSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>
    {
        protected override string Title => "Playback";

        private readonly ClickablePlayerSliderBar playBackSliderBar;

        public PlaybackSettings()
        {
            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "Playback:"
                },
                playBackSliderBar = new ClickablePlayerSliderBar(),
            };
        }

        public bool OnPressed(KaraokeAction action)
        {
            switch (action)
            {
                case KaraokeAction.IncreaseTempo:
                    playBackSliderBar.TriggerIncrease();
                    break;

                case KaraokeAction.DecreaseTempo:
                    playBackSliderBar.TriggerDecrease();
                    break;

                case KaraokeAction.ResetTempo:
                    playBackSliderBar.ResetToDefaultValue();
                    break;

                default:
                    return false;
            }

            return true;
        }

        public void OnReleased(KaraokeAction action)
        {
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSessionStatics session)
        {
            playBackSliderBar.Bindable = session.GetBindable<int>(KaraokeRulesetSession.PlaybackSpeed);
        }
    }
}
