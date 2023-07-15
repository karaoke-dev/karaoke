// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Components.Timeline;

public partial class PageSelectionBlueprint : EditableTimelineSelectionBlueprint<Page>
{
    private const float body_width = 4;

    private readonly IBindable<double> startTime;

    private readonly PageInfoPiece pageInfoPiece;
    private readonly PageBodyPiece pageBodyPiece;

    public PageSelectionBlueprint(Page item)
        : base(item)
    {
        startTime = item.TimeBindable.GetBoundCopy();
        RelativeSizeAxes = Axes.None;

        Width = body_width;

        // todo: not really sure why it fix the issue. should have more checks about this.
        Height = PagesTimeLine.TIMELINE_HEIGHT - 1;

        AddRangeInternal(new Drawable[]
        {
            pageInfoPiece = new PageInfoPiece(item)
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.BottomCentre,
            },
            pageBodyPiece = new PageBodyPiece(item)
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
            },
        });

        startTime.BindValueChanged(_ => X = (float)Item.Time, true);
    }

    [BackgroundDependencyLoader]
    private void load(IPageEditorVerifier pageEditorVerifier)
    {
    }

    private partial class PageInfoPiece : CompositeDrawable
    {
        private readonly Page page;
        private readonly IBindable<int> pagesVersion = new Bindable<int>();

        public PageInfoPiece(Page page)
        {
            this.page = page;

            AutoSizeAxes = Axes.X;
            Height = 16;
            Margin = new MarginPadding(4);

            Masking = true;
            CornerRadius = Height / 2;

            Origin = Anchor.TopCentre;
            Anchor = Anchor.TopCentre;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, IPageStateProvider pageStateProvider)
        {
            OsuSpriteText spriteText;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = colours.Yellow,
                    RelativeSizeAxes = Axes.Both,
                },
                spriteText = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Padding = new MarginPadding(3),
                    Font = OsuFont.Default.With(size: 14, weight: FontWeight.SemiBold),
                    Colour = colours.B5,
                },
            };

            pagesVersion.BindTo(pageStateProvider.PageInfo.PagesVersion);
            pagesVersion.BindValueChanged(x =>
            {
                int? order = pageStateProvider.PageInfo.GetPageOrder(page);
                spriteText.Text = $" #{order} ";
            }, true);
        }
    }

    private partial class PageBodyPiece : CompositeDrawable
    {
        private readonly Page page;

        public PageBodyPiece(Page page)
        {
            this.page = page;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = colours.Yellow,
                    RelativeSizeAxes = Axes.Both,
                },
            };
        }
    }
}
