// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Config;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Layout;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Style;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin
{
    public class KaraokeSkinEditor : GenericEditor<KaraokeSkinEditorScreenMode>
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private readonly ISkin skin;

        public KaraokeSkinEditor(ISkin skin)
        {
            this.skin = skin;
        }

        protected override GenericEditorScreen<KaraokeSkinEditorScreenMode> GenerateScreen(KaraokeSkinEditorScreenMode screenMode) =>
            screenMode switch
            {
                KaraokeSkinEditorScreenMode.Config => new ConfigScreen(skin),
                KaraokeSkinEditorScreenMode.Layout => new LayoutScreen(skin),
                KaraokeSkinEditorScreenMode.Style => new StyleScreen(skin),
                _ => throw new InvalidOperationException("Editor menu bar switched to an unsupported mode")
            };

        protected override MenuItem[] GenerateMenuItems(KaraokeSkinEditorScreenMode screenMode)
        {
            // todo: waiting for implementation.
            return null;
        }
    }
}
