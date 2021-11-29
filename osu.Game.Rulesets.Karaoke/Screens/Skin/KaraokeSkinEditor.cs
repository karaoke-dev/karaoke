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
using osu.Game.Rulesets.Karaoke.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin
{
    public class KaraokeSkinEditor : GenericEditor<KaraokeSkinEditorScreenMode>
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private readonly KaraokeSkin skin;

        public KaraokeSkinEditor(KaraokeSkin skin)
        {
            this.skin = skin;
        }

        protected override GenericEditorScreen<KaraokeSkinEditorScreenMode> GenerateScreen(KaraokeSkinEditorScreenMode screenMode)
        {
            switch (screenMode)
            {
                case KaraokeSkinEditorScreenMode.Config:
                    return new ConfigScreen(skin);

                case KaraokeSkinEditorScreenMode.Layout:
                    return new LayoutScreen(skin);

                case KaraokeSkinEditorScreenMode.Style:
                    return new StyleScreen(skin);

                default:
                    throw new InvalidOperationException("Editor menu bar switched to an unsupported mode");
            }
        }

        protected override MenuItem[] GenerateMenuItems(KaraokeSkinEditorScreenMode screenMode)
        {
            // todo: waiting for implementation.
            return null;
        }
    }
}
