// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
                LyricEditor.Mode = LyricEditorMode.CreateTimeTag;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
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
                    Navigation.State = NavigationState.Done;
                }));
            }
            else
            {
                DialogOverlay.Push(new UseAutoGenerateTimeTagPopupDialog(ok =>
                {
                    if (!ok)
                        return;

                    // todo: select all lyrics or switch to select mode.

                    lyricTimeTagsChangeHandler.AutoGenerate();
                    Navigation.State = NavigationState.Done;
                }));
            }
        }

        public class GenerateTimeTagNavigation : TopNavigation<GenerateTimeTagStepScreen>
        {
            private const string auto_generate_time_tag = "AUTO_GENERATE_TIME_TAG";

            public GenerateTimeTagNavigation(GenerateTimeTagStepScreen screen)
                : base(screen)
            {
            }

            protected override NavigationTextContainer CreateTextContainer()
                => new GenerateTimeTagTextFlowContainer(Screen);

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = $"Press [{auto_generate_time_tag}] to auto-generate time tag. It's very easy.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        NavigationText = $"Cool, you can reset your time-tag by pressing [{auto_generate_time_tag}]";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }

            protected override bool AbleToNextStep(NavigationState value)
                => value == NavigationState.Working || value == NavigationState.Done;

            private class GenerateTimeTagTextFlowContainer : NavigationTextContainer
            {
                public GenerateTimeTagTextFlowContainer(GenerateTimeTagStepScreen screen)
                {
                    AddLinkFactory(auto_generate_time_tag, "auto generate time tag", screen.AskForAutoGenerateTimeTag);
                }
            }
        }
    }
}
