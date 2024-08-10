// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Integration.Formats;

public interface IDecoder<out TTargetFormat> : IDecoder<string, TTargetFormat>;

public interface IDecoder<in TSourceFormat, out TTargetFormat>
{
    public TTargetFormat Decode(TSourceFormat source);
}
