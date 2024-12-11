// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands.Lyrics;

public class LyricStyleCommand : StageCommand<LyricStyle>
{
    public LyricStyleCommand(Easing easing, double startTime, double endTime, LyricStyle startValue, LyricStyle endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => nameof(LyricStyle);

    public override void ApplyInitialValue<TDrawable>(TDrawable d)
    {
        if (d is not DrawableLyric drawableLyric)
            throw new InvalidOperationException();

        drawableLyric.ApplyToLyricPieces(l =>
        {
            l.UpdateStyle(StartValue);
        });
    }

    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
    {
        // note: because update shader cost lots of effect, if the duration is 0, we just use the initial value.
        if (Duration == 0)
            return d.Delay(0);

        return d.TransformTo(d.PopulateTransform(new ApplyLyricFontTransform(), StartValue))
                .Delay(Duration)
                .Append(o => o.TransformTo(d.PopulateTransform(new ApplyLyricFontTransform(), EndValue)));
    }

    private class ApplyLyricFontTransform : Transform<LyricStyle, Drawable>
    {
        public override string TargetMember => nameof(LyricStyle);

        protected override void Apply(Drawable d, double time)
        {
            if (d is not DrawableLyric drawableLyric)
                throw new InvalidOperationException();

            drawableLyric.ApplyToLyricPieces(l =>
            {
                l.UpdateStyle(EndValue);
            });
        }

        protected override void ReadIntoStartValue(Drawable d)
        {
            // there's no start value for it.
        }
    }
}
