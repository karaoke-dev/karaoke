// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
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

        [Cached]
        private readonly TimeTagManager timeTagManager;

        public GenerateTimeTagSubScreen()
        {
            AddInternal(timeTagManager = new TimeTagManager());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateTimeTagNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(x =>
            {
                LyricEditor.Mode = Mode.TimeTagEditMode;
                LyricEditor.LyricFastEditMode = LyricFastEditMode.Language;
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

        protected void AskForAutoGenerateTimeTag()
        {
            DialogOverlay.Push(new UseAutoGenerateTimeTagPopupDialog(ok =>
            {
                if (ok)
                {
                    timeTagManager.AutoGenerateTimeTags();
                    // todo : should moving cursor to first
                    // timeTagManager.MoveCursor(CursorAction.First);
                }
            }));
        }

        public class GenerateTimeTagNavigation : TopNavigation
        {
            public GenerateTimeTagNavigation(ImportLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = "Press button to auto-generate time tag. It's very easy.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        NavigationText = "Cool";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;
                }
            }

            protected override bool AbleToNextStep(NavigationState value)
                => value == NavigationState.Working || value == NavigationState.Done;
        }
    }
}
