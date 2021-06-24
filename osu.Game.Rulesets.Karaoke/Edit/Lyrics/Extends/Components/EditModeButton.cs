// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public class EditModeButton : EditModeButton<LyricEditorMode>
    {
        public EditModeButton(LyricEditorMode mode)
            : base(mode)
        {
        }
    }

    public class EditModeButton<T> : OsuButton
    {
        public new Action<T> Action;

        public T Mode { get; }

        public EditModeButton(T mode)
        {
            Mode = mode;
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;

            base.Action = () => Action.Invoke(mode);
        }
    }
}
