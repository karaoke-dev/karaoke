// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class AssignLanguageSubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Language";

        public override string ShortTitle => "Language";

        public override ImportLyricStep Step => ImportLyricStep.AssignLanguage;

        public override IconUsage Icon => FontAwesome.Solid.Globe;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            var clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };
            dependencies.CacheAs<IAdjustableClock>(clock);
            dependencies.CacheAs<IFrameBasedClock>(clock);

            return dependencies;
        }

        public AssignLanguageSubScreen()
        {
            InternalChild = new LyricEditor
            {
                RelativeSizeAxes = Axes.Both,
                Mode = Mode.EditMode,
                LyricFastEditMode = LyricFastEditMode.Language,
                FontSize = 26
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AskForAutoAssignLanguage();
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateRuby);
        }

        protected void AskForAutoAssignLanguage()
        {
            DialogOverlay.Push(new UseLanguageDetectorPopupDialog(ok =>
            {
                // todo : call manager to do that.
            }));
        }
    }
}
