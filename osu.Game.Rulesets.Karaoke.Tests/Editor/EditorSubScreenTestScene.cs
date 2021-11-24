// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    public abstract class EditorSubScreenTestScene<T> : ScreenTestScene where T : EditorSubScreen
    {
        protected T Editor { get; private set; }

        public override void SetUpSteps()
        {
            base.SetUpSteps();

            AddStep("load editor", LoadEditor);
            AddUntilStep("wait for editor to load", () => Editor.IsLoaded);
        }

        protected virtual void LoadEditor()
        {
            LoadScreen(Editor = CreateEditor());
        }

        protected abstract T CreateEditor();
    }
}
