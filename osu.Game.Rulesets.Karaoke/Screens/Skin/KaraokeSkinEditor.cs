// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Config;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Layout;
using osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin
{
    public class KaraokeSkinEditor : GenericEditor<KaraokeSkinEditorScreenMode>
    {
        protected override GenericEditorScreen<KaraokeSkinEditorScreenMode> GenerateScreen(KaraokeSkinEditorScreenMode screenMode)
        {
            switch (screenMode)
            {
                case KaraokeSkinEditorScreenMode.Config:
                    return new ConfigScreen();

                case KaraokeSkinEditorScreenMode.Layout:
                    return new LayoutScreen();

                case KaraokeSkinEditorScreenMode.Style:
                    return new StyleScreen();

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
