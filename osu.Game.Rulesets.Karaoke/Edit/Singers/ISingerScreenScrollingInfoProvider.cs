// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public interface ISingerScreenScrollingInfoProvider
    {
        BindableFloat BindableZoom { get; }

        BindableFloat BindableCurrent { get; }
    }
}
