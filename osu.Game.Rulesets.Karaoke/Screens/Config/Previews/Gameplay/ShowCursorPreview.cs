// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.Cursor;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class ShowCursorPreview : SettingsSubsectionPreview
    {
        private readonly Bindable<bool> bindableShowCursor = new Bindable<bool>();
        private readonly MenuCursor.Cursor cursor;

        public ShowCursorPreview()
        {
            Size = new Vector2(0.3f);
            Children = new Drawable[]
            {
                cursor = new MenuCursor.Cursor
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = 30,
                    Text = "Wanna show this while gameplay?"
                }
            };

            bindableShowCursor.BindValueChanged(e =>
            {
                var showCursor = e.NewValue;

                if (showCursor)
                {
                    cursor.FadeTo(1, 200);
                }
                else
                {
                    cursor.FadeTo(0.5f, 200);
                }
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Config.BindWith(KaraokeRulesetSetting.ShowCursor, bindableShowCursor);
        }
    }
}
