// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

public partial class EditableLyricTimelineSelectionBlueprint : SelectionBlueprint<Lyric>
{
    private const float lyric_size = 20;

    private bool selectable = true;

    public EditableLyricTimelineSelectionBlueprint(Lyric item)
        : base(item)
    {
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.CentreLeft;

        X = (float)Item.LyricStartTime;

        RelativePositionAxes = Axes.X;
        RelativeSizeAxes = Axes.X;
        Height = lyric_size;

        AddInternal(new Container
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            RelativeSizeAxes = Axes.Both,
            Masking = true,
            CornerRadius = 5,
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray,
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Left = 5 },
                    RelativeSizeAxes = Axes.X,
                    Truncate = true,
                    Text = item.Text
                }
            }
        });
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

    protected override void OnSelected()
    {
        // base logic hides selected blueprints when not selected, but timeline doesn't do that.
    }

    protected override void OnDeselected()
    {
        // base logic hides selected blueprints when not selected, but timeline doesn't do that.
    }

    // prevent selection.
    public override Vector2 ScreenSpaceSelectionPoint => selectable ? ScreenSpaceDrawQuad.TopLeft : new Vector2(int.MinValue);

    // prevent single select.
    public override Quad SelectionQuad => selectable ? base.SelectionQuad : new Quad();

    protected override void Update()
    {
        base.Update();

        // no bindable so we perform this every update
        float duration = (float)Item.LyricDuration;

        if (Width != duration)
        {
            Width = duration;
        }
    }
}
