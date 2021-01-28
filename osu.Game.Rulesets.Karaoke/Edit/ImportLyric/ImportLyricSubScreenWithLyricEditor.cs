// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreenWithLyricEditor : ImportLyricSubScreenWithTopNavigation
    {
        protected LyricEditor LyricEditor { get; private set; }

        [Cached]
        protected readonly LyricManager LyricManager;

        protected ImportLyricSubScreenWithLyricEditor()
        {
            AddInternal(LyricManager = new LyricManager());
        }

        protected override Drawable CreateContent()
            => LyricEditor = new LyricEditor
            {
                RelativeSizeAxes = Axes.Both,
            };
    }
}
