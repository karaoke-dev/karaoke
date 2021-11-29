// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public abstract class KaraokeEditorScreen : GenericEditorScreen<KaraokeEditorScreenMode>
    {
        protected KaraokeEditorScreen(KaraokeEditorScreenMode type)
            : base(type)
        {
        }
    }
}
