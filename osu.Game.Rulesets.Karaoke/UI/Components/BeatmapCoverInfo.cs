// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Resources.Localisation.Web;
using osu.Game.Rulesets.Karaoke.UI.Stages;

namespace osu.Game.Rulesets.Karaoke.UI.Components;

public partial class BeatmapCoverInfo : CompositeDrawable, IStageComponent
{
    public BeatmapCoverInfo()
    {
        Masking = true;
        CornerRadius = 10;
    }

    [BackgroundDependencyLoader]
    private void load(IBindable<WorkingBeatmap> beatmap, OsuColour colours)
    {
        var metadata = beatmap.Value.Metadata;

        InternalChildren = new Drawable[]
        {
            new BeatmapCover
            {
                RelativeSizeAxes = Axes.Both,
            },
            new Box
            {
                Colour = colours.Gray1,
                Alpha = 0.6f,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Height = 64,
            },
            new FillFlowContainer
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Height = 64,
                Padding = new MarginPadding
                {
                    Horizontal = 10,
                },
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new TruncatingSpriteText
                    {
                        Text = new RomanisableString(metadata.TitleUnicode, metadata.Title),
                        Font = OsuFont.Default.With(size: 22.5f, weight: FontWeight.SemiBold),
                        RelativeSizeAxes = Axes.X,
                    },
                    new TruncatingSpriteText
                    {
                        Text = createArtistText(metadata),
                        Font = OsuFont.Default.With(size: 17.5f, weight: FontWeight.SemiBold),
                        RelativeSizeAxes = Axes.X,
                    },
                    new LinkFlowContainer(s =>
                    {
                        s.Shadow = false;
                        s.Font = OsuFont.GetFont(size: 14, weight: FontWeight.SemiBold);
                    }).With(d =>
                    {
                        d.AutoSizeAxes = Axes.Both;
                        d.Margin = new MarginPadding { Top = 2 };
                        d.AddText("mapped by ", t => t.Colour = colours.GrayB);
                        d.AddUserLink(metadata.Author);
                    }),
                },
            },
        };
    }

    private static LocalisableString createArtistText(IBeatmapMetadataInfo beatmapMetadata)
    {
        var romanisableArtist = new RomanisableString(beatmapMetadata.ArtistUnicode, beatmapMetadata.Artist);
        return BeatmapsetsStrings.ShowDetailsByArtist(romanisableArtist);
    }

    private partial class BeatmapCover : CompositeDrawable
    {
        private const string fallback_texture_name = @"Backgrounds/bg1";

        private readonly Sprite sprite;

        public BeatmapCover()
        {
            AddInternal(sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fill,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
        }

        [BackgroundDependencyLoader]
        private void load(IBindable<WorkingBeatmap> beatmap, LargeTextureStore textures)
        {
            sprite.Texture = beatmap.Value?.GetBackground() ?? textures.Get(fallback_texture_name);
        }
    }
}
