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
        private readonly IBindable<int> bindableReferenceLyricOrder = new Bindable<int>();
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

            bindableReferenceLyricOrder.BindValueChanged(e =>
            {
                BadgeText = $"Ref: #{e.NewValue}";
            });

            bindableReferenceLyric.BindValueChanged(e =>
            {
                bindableReferenceLyricOrder.UnbindBindings();

                if (e.NewValue == null)
                {
                    Hide();
                }
                else
                {
                    Show();
                    bindableReferenceLyricOrder.BindTo(e.NewValue.OrderBindable);
                }
            }, true);
        }
    }
}
