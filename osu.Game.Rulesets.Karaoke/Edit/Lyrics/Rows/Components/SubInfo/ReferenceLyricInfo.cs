// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public class ReferenceLyricInfo : SubInfo
    {
        private readonly IBindable<Lyric?> bindableReferenceLyric;

        public ReferenceLyricInfo(Lyric lyric)
            : base(lyric)
        {
            bindableReferenceLyric = lyric.ReferenceLyricBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Red;

            bindableReferenceLyric.BindValueChanged(e =>
            {
                if (e.NewValue == null)
                {
                    Hide();
                }
                else
                {
                    Show();

                    // note: there's no need to worry about referenced lyric change the order because there's no possible to change hhe order in reference lyric mode.
                    BadgeText = $"Ref: #{e.NewValue.Order}";
                }
            }, true);
        }
    }
}
