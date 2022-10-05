// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public abstract class BaseLayer : CompositeDrawable
    {
        protected readonly Lyric Lyric;

        protected BaseLayer(Lyric lyric)
        {
            Lyric = lyric;

            RelativeSizeAxes = Axes.Both;
        }

        public abstract void UpdateDisableEditState(bool editable);

        public virtual void TriggerDisallowEditEffect(LyricEditorMode editorMode)
        {
        }
    }
}
