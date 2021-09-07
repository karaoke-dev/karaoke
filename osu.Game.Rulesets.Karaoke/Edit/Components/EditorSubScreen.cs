// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Components
{
    /// <summary>
    /// todo : will inherit <see cref="EditorScreen"/> eventually
    /// </summary>
    public abstract class EditorSubScreen : Container
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        protected EditorSubScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            InternalChild = content = new PopoverContainer { RelativeSizeAxes = Axes.Both };
        }
    }
}
