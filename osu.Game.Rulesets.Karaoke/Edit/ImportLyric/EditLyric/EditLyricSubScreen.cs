// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.EditLyric
{
    public class EditLyricSubScreen : ImportLyricSubScreenWithLyricEditor
    {
        public override string Title => "Edit lyric";

        public override string ShortTitle => "Edit";

        public override ImportLyricStep Step => ImportLyricStep.EditLyric;

        public override IconUsage Icon => FontAwesome.Solid.Globe;

        [Cached]
        protected readonly LyricManager LyricManager;

        public EditLyricSubScreen()
        {
            AddInternal(LyricManager = new LyricManager());
        }

        protected override TopNavigation CreateNavigation()
            => new EditLyricNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(x =>
            {
                LyricEditor.Mode = Mode.EditMode;
                LyricEditor.LyricFastEditMode = LyricFastEditMode.None;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.AssignLanguage);
        }

        public class EditLyricNavigation : TopNavigation<EditLyricSubScreen>
        {
            public EditLyricNavigation(EditLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override TextFlowContainer CreateTextContainer()
                => new EditLyricTextFlowContainer(Screen);

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = "Check and edit lyric if needed.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        NavigationText = "Cool!";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;
                }
            }

            private class EditLyricTextFlowContainer : CustomizableTextContainer
            {
                public EditLyricTextFlowContainer(EditLyricSubScreen screen)
                {
                    AddIconFactory("Hello", () => null);
                }
            }
        }
    }
}
