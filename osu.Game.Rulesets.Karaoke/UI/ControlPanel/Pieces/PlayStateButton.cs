// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces
{
    public class PlayStateButton : ToolTipButton
    {
        private PlayState state;

        /// <summary>
        /// If paused , show pause icon
        /// </summary>
        public PlayState State
        {
            set
            {
                state = value;

                switch (value)
                {
                    case PlayState.Play:
                        playButton.Icon = FontAwesome.Regular.PlayCircle;
                        TooltipText = "Play";
                        break;
                    case PlayState.Pause:
                        playButton.Icon = FontAwesome.Regular.PauseCircle;
                        TooltipText = "Pause";
                        break;
                }
            }
            get => state;
        }

        private readonly IconButton playButton;

        public PlayStateButton()
        {
            Add(playButton = new IconButton
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(1.0f),
                IconScale = new Vector2(1.0f),
            });
        }
    }

    public enum PlayState
    {
        Play,
        Pause
    }
}
