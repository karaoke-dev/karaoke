// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricPropertiesSection<TModel> : LyricPropertySection where TModel : class
{
    private readonly LyricPropertiesEditor itemsEditor;

    protected LyricPropertiesSection()
    {
        Add(itemsEditor = CreateLyricPropertiesEditor());
    }

    protected sealed override void OnLyricChanged(Lyric? lyric)
    {
        itemsEditor.OnLyricChanged(lyric);
    }

    protected abstract LyricPropertiesEditor CreateLyricPropertiesEditor();

    protected abstract partial class LyricPropertiesEditor : SectionItemsEditor<TModel>
    {
        private Lyric? currentLyric;

        protected Lyric CurrentLyric => currentLyric ?? throw new InvalidOperationException();

        public void OnLyricChanged(Lyric? lyric)
        {
            currentLyric = lyric;

            Items.UnbindBindings();

            if (lyric != null)
                Items.BindTo(GetItems(lyric));
        }

        protected abstract IBindableList<TModel> GetItems(Lyric lyric);
    }
}
