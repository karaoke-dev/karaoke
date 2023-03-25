// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    public partial class ConfigScreen : KaraokeSkinEditorScreen
    {
        [Cached]
        protected readonly LyricFontInfoManager LyricFontInfoManager;

        public ConfigScreen(ISkin skin)
            : base(skin, KaraokeSkinEditorScreenMode.Config)
        {
            AddInternal(LyricFontInfoManager = new LyricFontInfoManager());
        }

        protected override Section[] CreateSelectionContainer()
            => Array.Empty<Section>();

        protected override Section[] CreatePropertiesContainer()
            => new Section[]
            {
                new IntervalSection(),
                new PositionSection(),
                new RubyRomajiSection(),
            };

        protected override Container CreatePreviewArea()
            => new();
    }
}
