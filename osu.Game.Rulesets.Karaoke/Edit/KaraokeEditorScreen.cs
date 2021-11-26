// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    /// <summary>
    /// TODO: eventually make this inherit Screen and add a local screen stack inside the Editor.
    /// </summary>
    public class KaraokeEditorScreen : GenericEditorScreen<KaraokeEditorScreenMode>
    {
        public KaraokeEditorScreen(KaraokeEditorScreenMode type)
            : base(type)
        {
        }
    }
}
