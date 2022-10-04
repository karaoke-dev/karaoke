// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.RecordingTimeTags;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor
{
    public class RecordingTimeTagBottomEditor : BaseBottomEditor
    {
        public override float ContentHeight => 60;

        protected override Drawable CreateInfo()
        {
            // todo : waiting for implementation.
            return new Container();
        }

        protected override Drawable CreateContent()
        {
            return new RecordingTimeTagScrollContainer
            {
                RelativeSizeAxes = Axes.X,
                Height = 60,
            };
        }
    }
}
