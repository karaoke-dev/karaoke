// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class BlueprintLayer : CompositeDrawable
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        private readonly Lyric lyric;

        public BlueprintLayer(Lyric lyric)
        {
            this.lyric = lyric;

            bindableMode.BindValueChanged(e =>
            {
                // Initial blueprint container.
                InitializeBlueprint(e.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);
        }

        protected void InitializeBlueprint(LyricEditorMode mode)
        {
            // remove all exist blueprint container
            ClearInternal();

            // create preview and real caret
            var blueprintContainer = createBlueprintContainer(mode, lyric);
            if (blueprintContainer == null)
                return;

            AddInternal(blueprintContainer);

            static Drawable createBlueprintContainer(LyricEditorMode mode, Lyric lyric) =>
                mode switch
                {
                    LyricEditorMode.EditRuby => new RubyBlueprintContainer(lyric),
                    LyricEditorMode.EditRomaji => new RomajiBlueprintContainer(lyric),
                    LyricEditorMode.AdjustTimeTag => new TimeTagBlueprintContainer(lyric),
                    _ => null
                };
        }
    }
}
