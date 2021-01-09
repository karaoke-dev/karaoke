// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeBlueprintContainer : ComposeBlueprintContainer
    {
        private EditMode mode;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos)
        {
            if (mode == EditMode.LyricEditor)
                return false;

            return base.ReceivePositionalInputAt(screenSpacePos);
        }

        public KaraokeBlueprintContainer(HitObjectComposer composer)
            : base(composer)
        {
        }

        public override OverlaySelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableNote note:
                    return new NoteSelectionBlueprint(note);

                case DrawableLyric lyric:
                    return new LyricSelectionBlueprint(lyric);
            }

            return base.CreateBlueprintFor(hitObject);
        }

        protected override SelectionHandler CreateSelectionHandler() => new KaraokeSelectionHandler();

        [BackgroundDependencyLoader]
        private void load(IBindable<EditMode> editMode)
        {
            editMode.BindValueChanged(e =>
            {
                mode = e.NewValue;

                if (mode == EditMode.LyricEditor)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            });
        }
    }
}
