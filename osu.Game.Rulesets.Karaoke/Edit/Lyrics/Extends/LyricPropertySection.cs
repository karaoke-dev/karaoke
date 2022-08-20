// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public abstract class LyricPropertySection : Section
    {
        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

        protected bool IsRebinding { get; private set; }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            bindableCaretPosition.BindValueChanged(x =>
            {
                var lyric = x.NewValue?.Lyric;

                IsRebinding = true;

                OnLyricChanged(lyric);

                IsRebinding = false;
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        }

        protected abstract void OnLyricChanged(Lyric? lyric);
    }
}
