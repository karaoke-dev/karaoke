// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Integration.Formats;

public interface IEncoder<in TSourceFormat> : IEncoder<TSourceFormat, string>;

public interface IEncoder<in TSourceFormat, out TTargetFormat>
{
    public TTargetFormat Encode(TSourceFormat source);
}
