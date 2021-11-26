// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Screens.Edit.Components.Menus;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Menus
{
    public class GenericEditorMenuBar<TScreenMode> : EditorMenuBar
    {
        public new readonly Bindable<TScreenMode> Mode = new();

        public GenericEditorMenuBar()
        {
            // remove exit tab control
            removeEditorTabControl();

            GenericScreenSelectionTabControl<TScreenMode> tabControl;
            AddRangeInternal(new Drawable[]
            {
                tabControl = new GenericScreenSelectionTabControl<TScreenMode>
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    X = -15
                }
            });

            Mode.BindTo(tabControl.Current);

            void removeEditorTabControl()
            {
                var editorTabControl = InternalChildren.OfType<ScreenSelectionTabControl>().FirstOrDefault();
                RemoveInternal(editorTabControl);
                base.Mode.UnbindAll();
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Mode.TriggerChange();
        }
    }
}
