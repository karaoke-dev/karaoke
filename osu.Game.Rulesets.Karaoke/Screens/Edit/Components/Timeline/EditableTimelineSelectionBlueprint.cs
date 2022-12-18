// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

public abstract partial class EditableTimelineSelectionBlueprint<TItem> : SelectionBlueprint<TItem>
{
    private bool selectable = true;

    protected EditableTimelineSelectionBlueprint(TItem item)
        : base(item)
    {
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.CentreLeft;

        RelativePositionAxes = Axes.X;
    }

    protected bool Selectable
    {
        get => selectable;
        set
        {
            if (selectable == value)
                return;

            selectable = value;
            OnSelectableStatusChanged(selectable);
        }
    }

    protected virtual void OnSelectableStatusChanged(bool selectable)
    {
        if (selectable)
        {
            Show();
        }
        else
        {
            this.FadeTo(0.1f, 200);
        }
    }

    protected sealed override void OnSelected()
    {
        // base logic hides selected blueprints when not selected, but timeline doesn't do that.
    }

    protected sealed override void OnDeselected()
    {
        // base logic hides selected blueprints when not selected, but timeline doesn't do that.
    }

    public sealed override bool ReceivePositionalInputAt(Vector2 screenSpacePos)
    {
        var drawable = GetInteractDrawable();
        if (drawable == this)
            return base.ReceivePositionalInputAt(screenSpacePos);

        return drawable.ReceivePositionalInputAt(screenSpacePos);
    }

    // prevent selection.
    public sealed override Vector2 ScreenSpaceSelectionPoint => selectable ? GetInteractDrawable().ScreenSpaceDrawQuad.TopLeft : new Vector2(int.MinValue);

    // prevent single select.
    public sealed override Quad SelectionQuad => selectable ? GetInteractDrawable().ScreenSpaceDrawQuad : new Quad();

    protected virtual Drawable GetInteractDrawable() => this;
}
