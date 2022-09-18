// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public class SingerSettings : LyricEditorSettings
    {
        public override ExtendDirection Direction => ExtendDirection.Left;

        public override float ExtendWidth => 300;

        protected override IReadOnlyList<Drawable> CreateSections() => new[]
        {
            new SingerEditSection(),
        };
    }
}
