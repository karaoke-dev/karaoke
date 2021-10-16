// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class PlaybackSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>
    {
        private readonly ClickablePlayerSliderBar playBackSliderBar;

        public PlaybackSettings()
            : base("Playback")
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

        public bool OnPressed(KeyBindingPressEvent<KaraokeAction> e)
        {
            switch (e.Action)
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

        public void OnReleased(KeyBindingReleaseEvent<KaraokeAction> e)
        {
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSessionStatics session)
        {
            playBackSliderBar.Current = session.GetBindable<int>(KaraokeRulesetSession.PlaybackSpeed);
        }
    }
}
