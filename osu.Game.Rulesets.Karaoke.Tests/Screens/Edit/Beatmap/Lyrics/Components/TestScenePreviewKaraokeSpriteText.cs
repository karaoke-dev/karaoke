// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Components;

public partial class TestScenePreviewKaraokeSpriteText : OsuTestScene
{
    private PreviewKaraokeSpriteText karaokeSpriteText = null!;
    private Container mask = null!;
    private OsuSpriteText spriteText = null!;

    private Action? updateAction;

    private readonly Lyric lyric;

    public TestScenePreviewKaraokeSpriteText()
    {
        lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
            RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
        };
    }

    protected override void Update()
    {
        updateAction?.Invoke();
        base.Update();
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
                karaokeSpriteText = new PreviewKaraokeSpriteText(lyric),
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
                },
                spriteText = new OsuSpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Font = OsuFont.GetFont(size: 20),
                }
            }
        };
    }

    [SetUp]
    public void Setup()
    {
        Schedule(() =>
        {
            updateAction = null;

            mask.Hide();
            spriteText.Hide();
        });
    }

    #region Text char index

    [Test]
    public void TestGetCharIndexByPosition()
    {
        AddStep("Show Char index Position", () =>
        {
            triggerUpdate(() =>
            {
                var mousePosition = getMousePosition();
                int? charIndex = karaokeSpriteText.GetCharIndexByPosition(mousePosition.X);
                updateText(charIndex.ToString());

                if (charIndex == null)
                {
                    hidePosition();
                }
                else
                {
                    var position = karaokeSpriteText.GetRectByCharIndex(charIndex.Value);
                    showPosition(position);
                }
            });
        });
    }

    [Test]
    public void TestGetRectByCharIndex()
    {
        for (int i = 0; i < lyric.Text.Length; i++)
        {
            int charIndex = i;
            AddStep($"Show Char index Position {i}", () =>
            {
                var position = karaokeSpriteText.GetRectByCharIndex(charIndex);
                showPosition(position);
            });
        }
    }

    #endregion

    #region Text indicator

    [Test]
    public void TestGetCharIndicatorByPosition()
    {
        AddStep("Show char indicator position", () =>
        {
            triggerUpdate(() =>
            {
                var mousePosition = getMousePosition();
                int charIndex = karaokeSpriteText.GetCharIndicatorByPosition(mousePosition.X);
                updateText(charIndex.ToString());

                var position = karaokeSpriteText.GetRectByCharIndicator(charIndex);
                showPosition(position);
            });
        });
    }

    [Test]
    public void TestGetRectByCharIndicator()
    {
        for (int i = 0; i <= lyric.Text.Length; i++)
        {
            int charIndex = i;
            AddStep($"Show char indicator position: {i}", () =>
            {
                var position = karaokeSpriteText.GetRectByCharIndicator(charIndex);
                showPosition(position);
            });
        }
    }

    #endregion

    #region Ruby tag

    [Test]
    public void TestGetRubyPosition()
    {
        foreach (var rubyTag in lyric.RubyTags)
        {
            AddStep($"Show ruby-tag position: {TextTagUtils.PositionFormattedString(rubyTag)}", () =>
            {
                var position = karaokeSpriteText.GetTextTagByPosition(rubyTag);
                showPosition(position);
            });
        }
    }

    #endregion

    #region Romaji tag

    [Test]
    public void TestGetRomajiTagPosition()
    {
        foreach (var romajiTag in lyric.RomajiTags)
        {
            AddStep($"Show romaji-tag position: {TextTagUtils.PositionFormattedString(romajiTag)}", () =>
            {
                var position = karaokeSpriteText.GetTextTagByPosition(romajiTag);
                showPosition(position);
            });
        }
    }

    #endregion

    #region Time tag

    [Test]
    public void TestGetTimeTagByPosition()
    {
        AddStep("Show time-tag position", () =>
        {
            triggerUpdate(() =>
            {
                var mousePosition = getMousePosition();
                var timeTag = karaokeSpriteText.GetTimeTagByPosition(mousePosition.X);
                updateText(timeTag != null ? TimeTagUtils.FormattedString(timeTag) : null);

                if (timeTag == null)
                {
                    hidePosition();
                }
                else
                {
                    var position = karaokeSpriteText.GetPositionByTimeTag(timeTag);
                    showPosition(position);
                }
            });
        });
    }

    [Test]
    public void TestGetPositionByTimeTag()
    {
        foreach (var timeTag in lyric.TimeTags)
        {
            AddStep($"Show time-tag position {TimeTagUtils.FormattedString(timeTag)}", () =>
            {
                var position = karaokeSpriteText.GetPositionByTimeTag(timeTag);
                showPosition(position);
            });
        }
    }

    #endregion

    [TearDown]
    public void TearDown()
    {
        Schedule(() =>
        {
            updateAction = null;
        });
    }

    private Vector2 getMousePosition()
    {
        var position = GetContainingInputManager().CurrentState.Mouse.Position;
        return karaokeSpriteText.ToLocalSpace(position);
    }

    private void triggerUpdate(Action action)
    {
        updateAction = action;
    }

    private void updateText(string? text)
    {
        spriteText.Text = text ?? "-";

        spriteText.Show();
    }

    private void hidePosition()
    {
        mask.Hide();
    }

    private void showPosition(Vector2 position)
    {
        const float sizing = 5;
        showPosition(new RectangleF(position.X - sizing / 2, position.Y - sizing / 2, sizing, sizing));
    }

    private void showPosition(RectangleF position)
    {
        mask.Position = position.TopLeft;
        mask.Size = position.Size;

        mask.Show();
    }
}
