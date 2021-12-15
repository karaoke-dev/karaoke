// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Cursor
{
    public class LayoutToolTip : BackgroundToolTip<Lyric>
    {
        private const float scale = 0.4f;

        private readonly DrawableLayoutPreview preview;

        [Resolved(canBeNull: true)]
        private ISkinSource skinSource { get; set; }

        public LayoutToolTip()
        {
            Child = preview = new DrawableLayoutPreview
            {
                Size = new Vector2(512 * scale, 384 * scale),
            };
        }

        private Lyric lastLyric;

        public override void SetContent(Lyric content)
        {
            if (content == lastLyric)
                return;

            lastLyric = content;

            // Get layout
            int layoutIndex = content.LayoutIndex;
            var layout = skinSource?.GetConfig<KaraokeSkinLookup, LyricLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, layoutIndex)).Value;

            // Display in content\
            preview.Layout = layout;
            preview.Lyric = content;
        }
    }
}
