// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Edit
{
    public class BlueprintLayer : BaseLayer
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();

        // should block all blueprint action if not editable.
        public override bool PropagatePositionalInputSubTree => base.PropagatePositionalInputSubTree && editable;

        private bool editable = true;

        public BlueprintLayer(Lyric lyric)
            : base(lyric)
        {
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
            var blueprintContainer = createBlueprintContainer(mode, timeTagEditMode, Lyric);
            if (blueprintContainer == null)
                return;

            AddInternal(blueprintContainer);

            static Drawable? createBlueprintContainer(LyricEditorMode mode, TimeTagEditMode timeTagEditMode, Lyric lyric) =>
                mode switch
                {
                    LyricEditorMode.EditRuby => new RubyBlueprintContainer(lyric),
                    LyricEditorMode.EditRomaji => new RomajiBlueprintContainer(lyric),
                    LyricEditorMode.EditTimeTag => timeTagEditMode == TimeTagEditMode.Adjust ? new TimeTagBlueprintContainer(lyric) : null,
                    _ => null
                };
        }

        public override void UpdateDisableEditState(bool editable)
        {
            this.editable = editable;
        }
    }
}
