// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets
{
    public interface IDrawableCursor : IDrawable
    {
        /// <summary>
        /// Is previewing cursor
        /// </summary>
        bool Preview { get; set; }
    }
}
