// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeBlueprintContainer : ComposeBlueprintContainer
    {
        public KaraokeBlueprintContainer(IEnumerable<DrawableHitObject> drawableHitObjects)
            : base(drawableHitObjects)
        {
        }

        public override OverlaySelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableNote note:
                    return new NoteSelectionBlueprint(note);

                case DrawableLyricLine lyric:
                    return new LyricSelectionBlueprint(lyric);
            }

            return base.CreateBlueprintFor(hitObject);
        }

        protected override SelectionHandler CreateSelectionHandler() => new KaraokeSelectionHandler();
    }
}
