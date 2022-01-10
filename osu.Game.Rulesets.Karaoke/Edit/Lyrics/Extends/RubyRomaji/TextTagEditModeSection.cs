// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagEditModeSection : EditModeSection<TextTagEditMode>
    {
        [Resolved]
        private Bindable<TextTagEditMode> bindableEditMode { get; set; }

        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;

        protected override TextTagEditMode DefaultMode()
            => bindableEditMode.Value;

        protected override Color4 GetColour(OsuColour colour, TextTagEditMode mode, bool active) =>
            mode switch
            {
                TextTagEditMode.Generate => active ? colour.Blue : colour.BlueDarker,
                TextTagEditMode.Edit => active ? colour.Red : colour.RedDarker,
                TextTagEditMode.Verify => active ? colour.Yellow : colour.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };

        protected override void UpdateEditMode(TextTagEditMode mode)
        {
            bindableEditMode.Value = mode;

            base.UpdateEditMode(mode);
        }
    }
}
