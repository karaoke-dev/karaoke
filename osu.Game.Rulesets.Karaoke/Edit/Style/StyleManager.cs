// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    public class StyleManager : Component
    {
        public readonly Bindable<LyricFont> EditStyle = new Bindable<LyricFont>();

        public readonly Bindable<NoteSkin> EditNoteStyle = new Bindable<NoteSkin>();

        [Resolved]
        private ISkinSource source { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
        }

        public void ApplyCurrentStyleChange(Action<LyricFont> action)
        {
            action?.Invoke(EditStyle.Value);
            EditStyle.TriggerChange();
        }

        public void ApplyCurrentNoteStyle(Action<NoteSkin> action)
        {
            action?.Invoke(EditNoteStyle.Value);
            EditNoteStyle.TriggerChange();
        }
    }
}
