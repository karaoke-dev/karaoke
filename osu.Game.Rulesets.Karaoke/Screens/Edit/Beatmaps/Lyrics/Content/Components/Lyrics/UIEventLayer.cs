// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public abstract partial class UIEventLayer : Layer
{
    protected UIEventLayer(Lyric lyric)
        : base(lyric)
    {
    }

    public sealed override void UpdateDisableEditState(bool editable)
    {
        // todo: should have some effect
    }
}
