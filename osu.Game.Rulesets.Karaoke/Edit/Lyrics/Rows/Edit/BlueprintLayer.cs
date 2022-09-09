// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit
{
    public class BlueprintLayer : CompositeDrawable
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();

        private readonly Lyric lyric;

        public BlueprintLayer(Lyric lyric)
        {
            this.lyric = lyric;

            bindableMode.BindValueChanged(_ =>
            {
                // Initial blueprint container.
                InitializeBlueprint();
            });

            bindableTimeTagEditMode.BindValueChanged(_ =>
            {
                // Initial blueprint container.
                InitializeBlueprint();
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ITimeTagModeState timeTagModeState)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);
        }

        protected void InitializeBlueprint()
        {
            // remove all exist blueprint container
            ClearInternal();

            // create preview and real caret
            var mode = bindableMode.Value;
            var timeTagEditMode = bindableTimeTagEditMode.Value;
            var blueprintContainer = createBlueprintContainer(mode, timeTagEditMode, lyric);
            if (blueprintContainer == null)
                return;

            AddInternal(blueprintContainer);

            static Drawable createBlueprintContainer(LyricEditorMode mode, TimeTagEditMode timeTagEditMode, Lyric lyric) =>
                mode switch
                {
                    LyricEditorMode.EditRuby => new RubyBlueprintContainer(lyric),
                    LyricEditorMode.EditRomaji => new RomajiBlueprintContainer(lyric),
                    LyricEditorMode.EditTimeTag => timeTagEditMode == TimeTagEditMode.Adjust ? new TimeTagBlueprintContainer(lyric) : null,
                    _ => null
                };
        }
    }
}
