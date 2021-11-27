// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    public class LyricConfigManager : Component
    {
        public readonly Bindable<LyricConfig> LoadedLyricConfig = new();

        public void ApplyCurrentLyricConfigChange(Action<LyricConfig> action)
        {
            // todo: implement.
        }
    }
}
