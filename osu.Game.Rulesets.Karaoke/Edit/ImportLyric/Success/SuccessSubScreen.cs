// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.Success
{
    public class SuccessSubScreen : ImportLyricSubScreen
    {
        public override string Title => "Success";

        public override string ShortTitle => "Success";

        public override ImportLyricStep Step => ImportLyricStep.GenerateTimeTag;

        public override IconUsage Icon => FontAwesome.Regular.CheckCircle;

        [Resolved]
        private ImportLyricScreen lyricImporter { get; set; }

        public override void Complete()
        {
            lyricImporter.Exit();
        }
    }
}
