// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    public class StyleManager : Component
    {
        public readonly Bindable<LyricStyle> EditStyle = new();

        public readonly Bindable<NoteSkin> EditNoteStyle = new();

        [Resolved]
        private ISkinSource source { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
        }

        public void ApplyCurrentStyleChange(Action<LyricStyle> action)
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
