// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Graphics;
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

        protected override TopNavigation CreateNavigation()
            => new EditLyricNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(x =>
            {
                LyricEditor.Mode = LyricEditorMode.Manage;
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

        internal void SwitchLyricEditorMode(LyricEditorMode mode)
        {
            // todo : will cause text update because has ScheduleAfterChildren in lyric editor.
            LyricEditor.Mode = mode;
            Navigation.State = NavigationState.Working;
        }

        public class EditLyricNavigation : TopNavigation<EditLyricSubScreen>
        {
            private const string cutting_mode = "CUTTING_MODE";
            private const string typing_mode = "TYPING_MODE";

            public EditLyricNavigation(EditLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override NavigationTextContainer CreateTextContainer()
                => new EditLyricTextFlowContainer(Screen);

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = $"Does something looks weird? Try switching [{cutting_mode}] or [{typing_mode}] to edit your lyric.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        var mode = Screen.LyricEditor.Mode;

                        NavigationText = mode switch
                        {
                            LyricEditorMode.Manage => $"Cool! Try switching to [{typing_mode}] if you wants to edit lyric.",
                            LyricEditorMode.Typing => $"Cool! Try switching to [{cutting_mode}] if you wants to cut or combine lyric.",
                            _ => throw new InvalidEnumArgumentException(nameof(mode))
                        };

                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;

                    default:
                        throw new InvalidEnumArgumentException(nameof(value));
                }
            }

            protected override bool AbleToNextStep(NavigationState value)
                => true;

            private class EditLyricTextFlowContainer : NavigationTextContainer
            {
                public EditLyricTextFlowContainer(EditLyricSubScreen screen)
                {
                    AddLinkFactory(cutting_mode, "cutting mode", () => screen.SwitchLyricEditorMode(LyricEditorMode.Manage));
                    AddLinkFactory(typing_mode, "typing mode", () => screen.SwitchLyricEditorMode(LyricEditorMode.Typing));
                }
            }
        }
    }
}
