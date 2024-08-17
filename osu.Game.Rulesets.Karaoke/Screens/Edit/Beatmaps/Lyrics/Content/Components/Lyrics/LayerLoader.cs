// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public class LayerLoader<TLayer> : LayerLoader where TLayer : Layer
{
    public Action<TLayer>? OnLoad { get; init; }

    public override Layer CreateLayer(Lyric lyric)
    {
        var layer = ActivatorUtils.CreateInstance<TLayer>(lyric);
        OnLoad?.Invoke(layer);
        return layer;
    }
}

public abstract class LayerLoader
{
    public abstract Layer CreateLayer(Lyric lyric);
}
