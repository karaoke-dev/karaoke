﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public class TestSceneDrawableJudgement : KaraokeSkinnableTestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(DrawableJudgement),
            typeof(DrawableNoteJudgement)
        };

        public TestSceneDrawableJudgement()
        {
            foreach (var result in Enum.GetValues(typeof(HitResult)).OfType<HitResult>().Skip(1))
            {
                AddStep("Show " + result.GetDescription(), () => SetContents(() =>
                    new DrawableNoteJudgement(new JudgementResult(new HitObject(), new Judgement()) { Type = result }, null)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    }));
            }
        }
    }
}
