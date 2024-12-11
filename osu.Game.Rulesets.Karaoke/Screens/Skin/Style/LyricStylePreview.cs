// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

internal partial class LyricStylePreview : CompositeDrawable
{
    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider)
    {
        Masking = true;
        CornerRadius = 15;
        FillMode = FillMode.Fit;
        FillAspectRatio = 1.25f;
        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = colourProvider.Background1,
            },
            new PreviewDrawableLyricLine(createDefaultLyricLine()),
        };
    }

    private Lyric createDefaultLyricLine()
    {
        double startTime = Time.Current;

        return new Lyric
        {
            Text = "カラオケ！",
            TimeTags = new List<TimeTag>
            {
                new(new TextIndex(0), startTime + 500),
                new(new TextIndex(1), startTime + 600)
                {
                    FirstSyllable = true,
                    RomanisedSyllable = "ra",
                },
                new(new TextIndex(2), startTime + 1000),
                new(new TextIndex(3), startTime + 1500)
                {
                    FirstSyllable = true,
                    RomanisedSyllable = "ke",
                },
                new(new TextIndex(4), startTime + 2000),
            },
            RubyTags = new[]
            {
                new RubyTag
                {
                    StartIndex = 0,
                    EndIndex = 0,
                    Text = "か",
                },
                new RubyTag
                {
                    StartIndex = 2,
                    EndIndex = 2,
                    Text = "お",
                },
            },
        };
    }

    private partial class PreviewDrawableLyricLine : DrawableLyric
    {
        public PreviewDrawableLyricLine(Lyric hitObject)
            : base(hitObject)
        {
        }
    }
}
