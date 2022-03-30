// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag
{
    public class GenerateTimeTagStepScreen : LyricImporterStepScreenWithLyricEditor
    {
        public override string Title => "Generate time tag";

        public override string ShortTitle => "Generate time tag";

        public override LyricImporterStep Step => LyricImporterStep.GenerateTimeTag;

        public override IconUsage Icon => FontAwesome.Solid.Tag;

        [Cached(typeof(ILyricTimeTagsChangeHandler))]
        private readonly LyricTimeTagsChangeHandler lyricTimeTagsChangeHandler;

        public GenerateTimeTagStepScreen()
        {
            AddInternal(lyricTimeTagsChangeHandler = new LyricTimeTagsChangeHandler());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateTimeTagNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(_ =>
            {
                SwitchLyricEditorMode(LyricEditorMode.EditTimeTag);
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AskForAutoGenerateTimeTag();
        }

        public override void Complete()
        {
            ScreenStack.Push(LyricImporterStep.Success);
        }

        internal void AskForAutoGenerateTimeTag()
        {
            var lyrics = Beatmap.Value.Beatmap.HitObjects.OfType<Lyric>();

            if (LyricsUtils.HasTimedTimeTags(lyrics))
            {
                // do not touch user's lyric if already contains valid time-tag with time.
                DialogOverlay.Push(new AlreadyContainTimeTagPopupDialog(ok =>
                {
                    // do nothing if already contains valid tags.
                }));
            }
            else
            {
                DialogOverlay.Push(new UseAutoGenerateTimeTagPopupDialog(ok =>
                {
                    if (!ok)
                        return;

                    PrepareAutoGenerate();
                }));
            }
        }
    }
}
