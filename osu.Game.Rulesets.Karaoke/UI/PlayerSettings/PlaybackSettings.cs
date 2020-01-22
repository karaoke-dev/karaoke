// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
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

        public bool OnReleased(KaraokeAction action) => true;

        [BackgroundDependencyLoader]
        private void load(KaroakeSessionStatics session)
        {
            playBackSliderBar.Bindable = session.GetBindable<int>(KaraokeRulesetSession.PlaybackSpeed);
        }
    }
}
