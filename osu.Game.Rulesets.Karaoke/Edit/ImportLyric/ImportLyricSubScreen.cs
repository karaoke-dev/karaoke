// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreen : OsuScreen, IImportLyricSubScreen
    {
        [Resolved]
        protected ImportLyricSubScreenStack ScreenStack { get; private set; }

        public abstract string ShortTitle { get; }

        public abstract ImportLyricStep Step { get; }

        public ImportLyricSubScreen()
        {
            InternalChildren = new Drawable[]
            {
                new OsuButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 240,
                    Text = $"{Title}, Click to next step.",
                    Action = Complete
                }
            };
        }

        public abstract void Complete();
    }
}
