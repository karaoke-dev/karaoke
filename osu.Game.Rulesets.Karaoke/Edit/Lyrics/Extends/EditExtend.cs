// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public abstract class EditExtend : EditorRoundedScreenSettings
    {
        public abstract ExtendDirection Direction { get; }

        public abstract float ExtendWidth { get; }

        protected override IReadOnlyList<Drawable> CreateSections() => Array.Empty<Drawable>();

        protected IReadOnlyList<Drawable> Children
        {
            get => this.ChildrenOfType<FillFlowContainer>().First().Children;
            set
            {
                // should delay assign the children after loaded.
                Schedule(() =>
                {
                    this.ChildrenOfType<FillFlowContainer>().First().Children = value;
                });
            }
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            // change the background colour to the lighter one.
            this.ChildrenOfType<Box>().First().Colour = colourProvider.Background3(state.Mode);
        }
    }
}
