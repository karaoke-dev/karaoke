// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

[Cached(typeof(IEditableLyricState))]
public partial class EditableLyric : InteractableLyric, IEditableLyricState
{
    public EditableLyric(Lyric lyric)
        : base(lyric)
    {
        CornerRadius = 5;
        Padding = new MarginPadding { Bottom = 10 };

        Layers = new Layer[]
        {
            new LyricLayer(lyric),
            new EditLyricLayer(lyric),
            new TimeTagLayer(lyric),
            new CaretLayer(lyric),
            new BlueprintLayer(lyric),
        };
    }

    public void TriggerDisallowEditEffect()
    {
        InternalChildren.OfType<Layer>().ForEach(x => x.TriggerDisallowEditEffect(BindableMode.Value));
    }
}
