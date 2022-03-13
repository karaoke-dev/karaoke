// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        private readonly IBindable<NoteEditMode> bindableMode = new Bindable<NoteEditMode>();

        private readonly IBindable<NoteEditModeSpecialAction> bindableModeSpecialAction = new Bindable<NoteEditModeSpecialAction>();

        public NoteExtend()
        {
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case NoteEditMode.Generate:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteConfigSection(),
                            new NoteSwitchSpecialActionSection(),
                        };
                        updateActionArea();
                        break;

                    case NoteEditMode.Edit:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteEditPropertyModeSection(),
                            new NoteEditPropertySection(),
                        };
                        break;

                    case NoteEditMode.Verify:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteIssueSection()
                        };
                        break;

                    default:
                        return;
                }
            }, true);

            bindableModeSpecialAction.BindValueChanged(e =>
            {
                if (bindableMode.Value != NoteEditMode.Generate)
                    return;

                updateActionArea();
            }, true);

            void updateActionArea()
            {
                RemoveAll(x => x is NoteAutoGenerateSection or NoteClearSection);

                switch (bindableModeSpecialAction.Value)
                {
                    case NoteEditModeSpecialAction.AutoGenerate:
                        Add(new NoteAutoGenerateSection());
                        break;

                    case NoteEditModeSpecialAction.SyncTime:
                        // todo: implement
                        break;

                    case NoteEditModeSpecialAction.Clear:
                        Add(new NoteClearSection());
                        break;

                    default:
                        return;
                }
            }
        }

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            bindableMode.BindTo(editNoteModeState.BindableEditMode);
            bindableModeSpecialAction.BindTo(editNoteModeState.BindableSpecialAction);
        }
    }
}
