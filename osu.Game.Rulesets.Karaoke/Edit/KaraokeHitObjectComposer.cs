// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Screens.Edit.Components.TernaryButtons;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeHitObjectComposer : HitObjectComposer<KaraokeHitObject>
    {
        private DrawableKaraokeEditRuleset drawableRuleset;

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator;

        public KaraokeHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
            // Duplicated registration because selection handler needs to use it.
            positionCalculator = new PositionCalculator(9);
        }

        public new KaraokePlayfield Playfield => drawableRuleset.Playfield;

        public IScrollingInfo ScrollingInfo => drawableRuleset.ScrollingInfo;

        protected override Playfield PlayfieldAtScreenSpacePosition(Vector2 screenSpacePosition)
        {
            // Only note and lyric playfields can interact with mouse input.
            if (Playfield.NotePlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.NotePlayfield;
            if (Playfield.LyricPlayfield.ReceivePositionalInputAt(screenSpacePosition))
                return Playfield.LyricPlayfield;

            return null;
        }

        public override SnapResult SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition)
        {
            var result = base.SnapScreenSpacePositionToValidTime(screenSpacePosition);

            if (result.Playfield is NotePlayfield)
            {
                // Apply Y value because it's disappeared.
                result.ScreenSpacePosition.Y = screenSpacePosition.Y;
                // then disable time change by moving x
                result.Time = null;
            }

            return result;
        }

        protected override DrawableRuleset<KaraokeHitObject> CreateDrawableRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            => drawableRuleset = new DrawableKaraokeEditRuleset(ruleset, beatmap, mods);

        protected override ComposeBlueprintContainer CreateBlueprintContainer()
            => new KaraokeBlueprintContainer(this);

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();

        private readonly Bindable<TernaryState> displayRubyToggle = new Bindable<TernaryState>();
        private readonly Bindable<TernaryState> displayRomajiToggle = new Bindable<TernaryState>();
        private readonly Bindable<TernaryState> displayTranslateToggle = new Bindable<TernaryState>();

        protected override IEnumerable<TernaryButton> CreateTernaryButtons()
            => new[]
            {
                new TernaryButton(displayRubyToggle, "Ruby", () => new SpriteIcon { Icon = FontAwesome.Solid.Ruler }),
                new TernaryButton(displayRomajiToggle, "Romaji", () => new SpriteIcon { Icon = FontAwesome.Solid.Ruler }),
                new TernaryButton(displayTranslateToggle, "Translate", () => new SpriteIcon { Icon = FontAwesome.Solid.Ruler }),
            };

        [BackgroundDependencyLoader]
        private void load()
        {
            var karaokeSessionStatics = drawableRuleset.Session;
            displayRubyToggle.Value = karaokeSessionStatics.Get<bool>(KaraokeRulesetSession.DisplayRuby) ? TernaryState.True : TernaryState.False;
            displayRomajiToggle.Value = karaokeSessionStatics.Get<bool>(KaraokeRulesetSession.DisplayRomaji) ? TernaryState.True : TernaryState.False;
            displayTranslateToggle.Value = karaokeSessionStatics.Get<bool>(KaraokeRulesetSession.UseTranslate) ? TernaryState.True : TernaryState.False;
            displayRubyToggle.BindValueChanged(x => { karaokeSessionStatics.GetBindable<bool>(KaraokeRulesetSession.DisplayRuby).Value = x.NewValue == TernaryState.True; });
            displayRomajiToggle.BindValueChanged(x => { karaokeSessionStatics.GetBindable<bool>(KaraokeRulesetSession.DisplayRomaji).Value = x.NewValue == TernaryState.True; });
            displayTranslateToggle.BindValueChanged(x => { karaokeSessionStatics.GetBindable<bool>(KaraokeRulesetSession.UseTranslate).Value = x.NewValue == TernaryState.True; });
        }
    }
}
