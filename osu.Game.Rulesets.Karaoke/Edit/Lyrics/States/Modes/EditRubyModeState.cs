// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class EditRubyModeState : ModeStateWithBlueprintContainer<RubyTag>, IEditRubyModeState
    {
        private readonly Bindable<TextTagEditMode> bindableEditMode = new();

        public IBindable<TextTagEditMode> BindableEditMode => bindableEditMode;

        public void ChangeEditMode(TextTagEditMode mode)
            => bindableEditMode.Value = mode;

        protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags));
    }
}
