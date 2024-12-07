// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

public partial class LabelledRealTimeSliderBar<TNumber> : LabelledSliderBar<TNumber>
    where TNumber : struct, INumber<TNumber>, IMinMaxValue<TNumber>
{
    protected override SettingsSlider<TNumber> CreateComponent()
        => base.CreateComponent().With(x => x.TransferValueOnCommit = false);
}
