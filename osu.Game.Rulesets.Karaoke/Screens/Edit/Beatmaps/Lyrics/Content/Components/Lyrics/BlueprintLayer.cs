// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Blueprints;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class BlueprintLayer : BaseLayer
{
    private readonly IBindable<EditorModeWithEditStep> bindableModeWithEditStep = new Bindable<EditorModeWithEditStep>();

    // todo: make better way to handle this.
    private readonly IBindable<RubyTagEditMode> bindableRubyTagEditMode = new Bindable<RubyTagEditMode>();

    // should block all blueprint action if not editable.
    public override bool PropagatePositionalInputSubTree => base.PropagatePositionalInputSubTree && editable;

    private bool editable = true;

    public BlueprintLayer(Lyric lyric)
        : base(lyric)
    {
        bindableModeWithEditStep.BindValueChanged(_ =>
        {
            // Initial blueprint container.
            InitializeBlueprint();
        });

        bindableRubyTagEditMode.BindValueChanged(_ =>
        {
            // Initial blueprint container.
            InitializeBlueprint();
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, IEditRubyModeState editRubyModeState)
    {
        bindableModeWithEditStep.BindTo(state.BindableModeWithEditStep);

        bindableRubyTagEditMode.BindTo(editRubyModeState.BindableRubyTagEditMode);
    }

    protected void InitializeBlueprint()
    {
        // remove all exist blueprint container
        ClearInternal();

        // create preview and real caret
        var modeWithEditStep = bindableModeWithEditStep.Value;
        var rubyTagEditMode = bindableRubyTagEditMode.Value;

        var blueprintContainer = createBlueprintContainer(modeWithEditStep, rubyTagEditMode, Lyric);
        if (blueprintContainer == null)
            return;

        AddInternal(blueprintContainer);

        static Drawable? createBlueprintContainer(EditorModeWithEditStep modeWithEditStep, RubyTagEditMode rubyTagEditMode, Lyric lyric) =>
            modeWithEditStep.Mode switch
            {
                LyricEditorMode.EditRuby => rubyTagEditMode == RubyTagEditMode.Create ? null : new RubyBlueprintContainer(lyric),
                LyricEditorMode.EditTimeTag => modeWithEditStep.EditStep is TimeTagEditStep.Adjust ? new TimeTagBlueprintContainer(lyric) : null,
                _ => null,
            };
    }

    public override void UpdateDisableEditState(bool editable)
    {
        this.editable = editable;
    }
}
