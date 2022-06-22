// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class BarLine : KaraokeHitObject, IBarLine
    {
        public bool Major
        {
            get => MajorBindable.Value;
            set => MajorBindable.Value = value;
        }

        public readonly Bindable<bool> MajorBindable = new BindableBool();

        public override Judgement CreateJudgement() => new IgnoreJudgement();
    }
}
