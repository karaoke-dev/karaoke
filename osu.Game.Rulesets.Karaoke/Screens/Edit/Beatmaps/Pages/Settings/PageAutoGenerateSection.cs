// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;
using osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Beatmaps.Pages;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PageAutoGenerateSection : AutoGenerateSection
{
    protected override AutoGenerateSubsection CreateAutoGenerateSubsection()
        => new PageAutoGenerateSubsection();

    private partial class PageAutoGenerateSubsection : AutoGenerateSubsection
    {
        protected override EditorSectionButton CreateGenerateButton()
            => new PageAutoGenerateButton();

        protected override DescriptionFormat CreateInvalidDescriptionFormat()
            => new()
            {
                Text = "Seems have some time-related issues in the lyrics. Go to lyric editor to fix them."
            };

        protected override ConfigButton CreateConfigButton()
            => new PageAutoGenerateConfigButton();

        protected partial class PageAutoGenerateConfigButton : ConfigButton
        {
            public override Popover GetPopover()
                => new PageGeneratorConfigPopover();
        }

        private partial class PageAutoGenerateButton : EditorSectionButton
        {
            [Resolved, AllowNull]
            private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; }

            [Resolved, AllowNull]
            private IDialogOverlay dialogOverlay { get; set; }

            [Resolved, AllowNull]
            private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; }

            public PageAutoGenerateButton()
            {
                Text = "Generate";
                Action = () =>
                {
                    bool canGenerate = beatmapPagesChangeHandler.CanGenerate();

                    if (canGenerate)
                    {
                        dialogOverlay.Push(new OkPopupDialog
                        {
                            Icon = FontAwesome.Solid.ExclamationTriangle,
                            HeaderText = "Seems still have some issues need to be fixed.",
                            BodyText = beatmapPagesChangeHandler.GetNotGeneratableMessage()!.Value,
                        });
                        return;
                    }

                    bool clearPagesAfterGenerated = clearExistPagesAfterGenerated();

                    if (clearPagesAfterGenerated)
                    {
                        dialogOverlay.Push(new ConfirmReGeneratePageDialog(isOk =>
                        {
                            if (isOk)
                                beatmapPagesChangeHandler.AutoGenerate();
                        }));
                    }
                    else
                    {
                        beatmapPagesChangeHandler.AutoGenerate();
                    }
                };
            }

            private bool clearExistPagesAfterGenerated()
                => generatorConfigManager.Get<PageGeneratorConfig>(KaraokeRulesetEditGeneratorSetting.BeatmapPageGeneratorConfig).ClearExistPages;
        }
    }
}
