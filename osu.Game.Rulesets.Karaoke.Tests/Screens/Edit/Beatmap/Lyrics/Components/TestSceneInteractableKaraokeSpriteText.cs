// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Components;

public partial class TestSceneInteractableKaraokeSpriteText : OsuTestScene
{
    private InteractableKaraokeSpriteText karaokeSpriteText = null!;
    private Container mask = null!;

    private readonly Lyric lyric;

    public TestSceneInteractableKaraokeSpriteText()
    {
        lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
            RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
        };
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colour)
    {
        Child = new Container
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Scale = new Vector2(2),
            Width = 200,
            Height = 100,
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colour.BlueDarker,
                },
                karaokeSpriteText = new InteractableKaraokeSpriteText(lyric),
                mask = new Container
                {
                    Masking = true,
                    BorderThickness = 1,
                    BorderColour = colour.RedDarker,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colour.RedDarker,
                        Alpha = 0.3f
                    }
                }
            }
        };
    }

    [SetUp]
    public void Setup()
    {
        Schedule(() =>
        {
            mask.Hide();
        });
    }

    [Test]
    public void TestGetRubyPosition()
    {
        foreach (var rubyTag in lyric.RubyTags)
        {
            AddStep($"Show Ruby Position {TextTagUtils.PositionFormattedString(rubyTag)}", () =>
            {
                var position = karaokeSpriteText.GetTextTagPosition(rubyTag);
                showPosition(position);
            });
        }
    }

    [Test]
    public void TestGetRomajiPosition()
    {
        foreach (var romajiTag in lyric.RomajiTags)
        {
            AddStep($"Show Romaji Position {TextTagUtils.PositionFormattedString(romajiTag)}", () =>
            {
                var position = karaokeSpriteText.GetTextTagPosition(romajiTag);
                showPosition(position);
            });
        }
    }

    private void showPosition(RectangleF position)
    {
        mask.Position = position.TopLeft;
        mask.Size = position.Size;

        mask.Show();
    }
}
