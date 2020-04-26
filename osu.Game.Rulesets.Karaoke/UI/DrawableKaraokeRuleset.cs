// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class DrawableKaraokeRuleset : DrawableScrollingRuleset<KaraokeHitObject>
    {
        public KaraokeSessionStatics Session { get; private set; }
        public new KaraokePlayfield Playfield => (KaraokePlayfield)base.Playfield;

        public IEnumerable<BarLine> BarLines;

        public new KaraokeRulesetConfigManager Config => (KaraokeRulesetConfigManager)base.Config;

        private readonly Bindable<KaraokeScrollingDirection> configDirection = new Bindable<KaraokeScrollingDirection>();

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        public DrawableKaraokeRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
            positionCalculator = new PositionCalculator(9);

            // TODO : it should be moved into NotePlayfield
            BarLines = new BarLineGenerator<BarLine>(Beatmap).BarLines;
        }

        protected override Playfield CreatePlayfield() => new KaraokePlayfield();

        protected override PassThroughInputManager CreateInputManager() =>
            new KaraokeInputManager(Ruleset.RulesetInfo);

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            dependencies.Cache(Session = new KaraokeSessionStatics(Config, Beatmap));
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            BarLines.ForEach(Playfield.Add);

            Config.BindWith(KaraokeRulesetSetting.ScrollDirection, configDirection);
            configDirection.BindValueChanged(direction => Direction.Value = (ScrollingDirection)direction.NewValue, true);

            Config.BindWith(KaraokeRulesetSetting.ScrollTime, TimeRange);
        }

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
        {
            switch (h)
            {
                case LyricLine lyric:
                    return new DrawableLyricLine(lyric);

                case TranslateDictionary _:
                    return null;

                default:
                    throw new NotSupportedException($"{nameof(DrawableKaraokeRuleset)} does not support type of {nameof(h)} hitobject.");
            }
        }

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new KaraokeFramedReplayInputHandler(replay);
    }
}
