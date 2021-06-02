// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag
{
    public class GenerateTimeTagSubScreen : ImportLyricSubScreenWithLyricEditor
    {
        public override string Title => "Generate time tag";

        public override string ShortTitle => "Generate time tag";

        public override ImportLyricStep Step => ImportLyricStep.GenerateTimeTag;

        public override IconUsage Icon => FontAwesome.Solid.Tag;

        protected override TopNavigation CreateNavigation()
            => new GenerateTimeTagNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(x =>
            {
                LyricEditor.Mode = LyricEditorMode.EditTimeTag;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
            AskForAutoGenerateTimeTag();
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.Success);
        }

        internal void AskForAutoGenerateTimeTag()
        {
            if (LyricManager.HasTimedTimeTags())
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

                    LyricManager.AutoGenerateTimeTags();
                    Navigation.State = NavigationState.Done;
                }));
            }
        }

        public class GenerateTimeTagNavigation : TopNavigation<GenerateTimeTagSubScreen>
        {
            private const string auto_generate_time_tag = "AUTO_GENERATE_TIME_TAG";

            public GenerateTimeTagNavigation(GenerateTimeTagSubScreen screen)
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
                public GenerateTimeTagTextFlowContainer(GenerateTimeTagSubScreen screen)
                {
                    AddLinkFactory(auto_generate_time_tag, "auto generate time tag", screen.AskForAutoGenerateTimeTag);
                }
            }
        }
    }
}
