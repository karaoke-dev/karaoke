// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Components
{
    public abstract class EditorSubScreen : EditorScreen
    {
        protected EditorSubScreen()
            : base(EditorScreenMode.Compose)
        {
            // todo : this is a temp fix and might be broken in next release.
            // See : https://github.com/ppy/osu/pull/14554/files
            Show();
        }
    }
}
