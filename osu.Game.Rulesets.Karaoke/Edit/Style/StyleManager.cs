// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    public class StyleManager : Component
    {
        public readonly Bindable<KaraokeFont> EditStyle = new Bindable<KaraokeFont>();

        public readonly Bindable<NoteSkin> EditNoteStyle = new Bindable<NoteSkin>();

        [Resolved]
        private ISkinSource source { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {

        }

        public void ApplyCurrentStyleChange(Action<KaraokeFont> action)
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
