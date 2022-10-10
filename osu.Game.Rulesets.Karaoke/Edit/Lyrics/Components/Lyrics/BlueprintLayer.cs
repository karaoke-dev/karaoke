// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Blueprints;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public class BlueprintLayer : BaseLayer
    {
        private readonly IBindable<ModeWithSubMode> bindableModeAndSubMode = new Bindable<ModeWithSubMode>();

        // should block all blueprint action if not editable.
        public override bool PropagatePositionalInputSubTree => base.PropagatePositionalInputSubTree && editable;

        private bool editable = true;

        public BlueprintLayer(Lyric lyric)
            : base(lyric)
        {
            bindableModeAndSubMode.BindValueChanged(_ =>
            {
                // Initial blueprint container.
                InitializeBlueprint();
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ITimeTagModeState timeTagModeState)
        {
            bindableModeAndSubMode.BindTo(state.BindableModeAndSubMode);
        }

        protected void InitializeBlueprint()
        {
            // remove all exist blueprint container
            ClearInternal();

            // create preview and real caret
            var modeWithSubMode = bindableModeAndSubMode.Value;
            var blueprintContainer = createBlueprintContainer(modeWithSubMode, Lyric);
            if (blueprintContainer == null)
                return;

            AddInternal(blueprintContainer);

            static Drawable? createBlueprintContainer(ModeWithSubMode modeWithSubMode, Lyric lyric) =>
                modeWithSubMode.Mode switch
                {
                    LyricEditorMode.EditRuby => new RubyBlueprintContainer(lyric),
                    LyricEditorMode.EditRomaji => new RomajiBlueprintContainer(lyric),
                    LyricEditorMode.EditTimeTag => modeWithSubMode.SubMode is TimeTagEditMode.Adjust ? new TimeTagBlueprintContainer(lyric) : null,
                    _ => null
                };
        }

        public override void UpdateDisableEditState(bool editable)
        {
            this.editable = editable;
        }
    }
}
