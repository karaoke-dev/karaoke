// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class DrawableKaraokeRuleset : DrawableScrollingRuleset<KaraokeHitObject>
    {
        public new KaraokePlayfield Playfield => (KaraokePlayfield)base.Playfield;

        public IEnumerable<BarLine> BarLines;

        protected new KaraokeRulesetConfigManager Config => (KaraokeRulesetConfigManager)base.Config;

        private readonly Bindable<KaraokeScrollingDirection> configDirection = new Bindable<KaraokeScrollingDirection>();

        private readonly BindableBool translate = new BindableBool();
        private readonly Bindable<string> translateLanguage = new Bindable<string>();

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        public DrawableKaraokeRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
            positionCalculator = new PositionCalculator(9);

            // TODO : it should be moved into NotePlayfield
            BarLines = new BarLineGenerator<BarLine>(Beatmap).BarLines;

            // Change need to translate
            translate.BindValueChanged(x => updateLyricTranslate());
            translateLanguage.BindValueChanged(x => updateLyricTranslate());
        }

        private void updateLyricTranslate()
        {
            var isTranslate = translate.Value;
            var translateLanguage = this.translateLanguage.Value;

            var lyric = Beatmap.HitObjects.OfType<LyricLine>().ToList();
            var translateDictionary = Beatmap.HitObjects.OfType<TranslateDictionary>().FirstOrDefault();

            // Clear exist translate
            lyric.ForEach(x => x.TranslateText = null);

            // If contain target language
            if (isTranslate && translateLanguage != null
                            && translateDictionary != null && translateDictionary.Translates.ContainsKey(translateLanguage))
            {
                var language = translateDictionary.Translates[translateLanguage];

                // Apply translate
                for (int i = 0; i < Math.Min(lyric.Count, language.Count); i++)
                {
                    lyric[i].TranslateText = language[i];
                }
            }
        }

        protected override Playfield CreatePlayfield() => new KaraokePlayfield();

        protected override PassThroughInputManager CreateInputManager() =>
            new KaraokeInputManager(Ruleset.RulesetInfo);

        [BackgroundDependencyLoader]
        private void load()
        {
            BarLines.ForEach(Playfield.Add);

            Config.BindWith(KaraokeRulesetSetting.ScrollDirection, configDirection);
            configDirection.BindValueChanged(direction => Direction.Value = (ScrollingDirection)direction.NewValue, true);

            Config.BindWith(KaraokeRulesetSetting.ScrollTime, TimeRange);

            // Translate
            Config.BindWith(KaraokeRulesetSetting.UseTranslate, translate);
            Config.BindWith(KaraokeRulesetSetting.PreferLanguage, translateLanguage);
        }

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
        {
            switch (h)
            {
                case LyricLine lyric:
                    return new DrawableLyricLine(lyric);

                case Note note:
                    if (note.Display)
                        return new DrawableNote(note);

                    break;
            }

            return null;
        }

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new KaraokeFramedReplayInputHandler(replay);
    }
}
