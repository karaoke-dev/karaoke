// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public abstract class ModeStateWithBlueprintContainer<TObject> : Component, IHasBlueprintSelection<TObject> where TObject : class
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();
        private readonly IBindable<int> bindableLyricPropertyWritableVersion = new Bindable<int>();

        public BindableList<TObject> SelectedItems { get; } = new();

        protected ModeStateWithBlueprintContainer()
        {
            bindableMode.BindValueChanged(e =>
            {
                TriggerDisableStateChanged();
            });

            bindableCaretPosition.BindValueChanged(e =>
            {
                bindableLyricPropertyWritableVersion.UnbindBindings();

                var lyric = e.NewValue?.Lyric;

                if (lyric == null)
                    return;

                bindableLyricPropertyWritableVersion.BindTo(lyric.LyricPropertyWritableVersion);
                TriggerDisableStateChanged();
            });

            bindableLyricPropertyWritableVersion.BindValueChanged(_ =>
            {
                TriggerDisableStateChanged();
            });
        }

        protected virtual void TriggerDisableStateChanged()
        {
            var caret = bindableCaretPosition.Value;
            if (caret == null)
                return;

            var lyric = caret.Lyric;
            var generateType = caret.GenerateType;

            SelectedItems.Clear();
            bool locked = IsWriteLyricPropertyLocked(lyric);
            if (locked)
                return;

            switch (generateType)
            {
                case CaretGenerateType.Action:
                    if (SelectFirstProperty(lyric))
                        SelectedItems.Add(SelectableProperties(lyric).FirstOrDefault());
                    break;

                case CaretGenerateType.TargetLyric:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract bool IsWriteLyricPropertyLocked(Lyric lyric);

        protected abstract bool SelectFirstProperty(Lyric lyric);

        protected abstract IEnumerable<TObject> SelectableProperties(Lyric lyric);

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ILyricCaretState lyricCaretState)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        }
    }
}
