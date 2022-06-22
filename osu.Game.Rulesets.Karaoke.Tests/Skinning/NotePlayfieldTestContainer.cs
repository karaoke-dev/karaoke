// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    /// <summary>
    /// A container to be used in a <see cref="KaraokeSkinnableColumnTestScene"/> to provide a resolvable <see cref="NotePlayfield"/> dependency.
    /// </summary>
    public class NotePlayfieldTestContainer : Container
    {
        protected override Container<Drawable> Content => content;

        private readonly Container content;

        [Cached]
        private readonly NotePlayfield playfield;

        public NotePlayfieldTestContainer(int column)
        {
            playfield = new NotePlayfield(column);
            InternalChild = content = new KaraokeInputManager(new KaraokeRuleset().RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
