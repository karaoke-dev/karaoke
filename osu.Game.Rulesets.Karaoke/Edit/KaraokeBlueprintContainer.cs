// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeBlueprintContainer : ComposeBlueprintContainer
    {
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos)
        {
            if (bindableEditMode.Value == EditMode.LyricEditor)
                return false;

            return base.ReceivePositionalInputAt(screenSpacePos);
        }

        public KaraokeBlueprintContainer(HitObjectComposer composer)
            : base(composer)
        {
            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }, true);
        }

        public override OverlaySelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableNote note:
                    return new NoteSelectionBlueprint(note);

                case DrawableLyric lyric:
                    return new LyricSelectionBlueprint(lyric);

                default:
                    throw new IndexOutOfRangeException(nameof(hitObject));
            }
        }

        protected override SelectionHandler CreateSelectionHandler() => new KaraokeSelectionHandler();

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
        }
    }
}
