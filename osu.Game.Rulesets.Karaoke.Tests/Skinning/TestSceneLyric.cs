// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning;

[TestFixture]
public partial class TestSceneLyric : KaraokeSkinnableTestScene
{
    private static CultureInfo cultureInfo { get; } = new("en-US");

    public TestSceneLyric()
    {
        AddStep("Default Lyric", () => SetContents(_ => testSingle()));
    }

    private Drawable testSingle(double timeOffset = 0)
    {
        double startTime = Time.Current + 1000 + timeOffset;

        var lyric = new Lyric
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

        lyric.Translations.Add(cultureInfo, "karaoke");

        lyric.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

        var drawable = CreateDrawableLyric(lyric);
        foreach (var mod in SelectedMods.Value.OfType<IApplicableToDrawableHitObject>())
            mod.ApplyToDrawableHitObject(drawable);

        return drawable;
    }

    private int depthIndex;

    protected virtual TestDrawableLyric CreateDrawableLyric(Lyric lyric)
        => new(lyric)
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Depth = depthIndex++,
        };

    protected partial class TestDrawableLyric : DrawableLyric
    {
        public TestDrawableLyric(Lyric h)
            : base(h)
        {
        }
    }
}
