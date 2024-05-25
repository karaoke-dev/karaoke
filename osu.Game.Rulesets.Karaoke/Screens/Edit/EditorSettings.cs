// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Graphics.Containers;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

[Cached]
public abstract partial class EditorSettings : EditorRoundedScreenSettings
{
    protected override void LoadComplete()
    {
        base.LoadComplete();

        var newSettingHeader = CreateSettingHeader();
        if (newSettingHeader == null)
            return;

        AddInternal(newSettingHeader);
    }

    public void ReloadSections()
    {
        // reload section after header ready.
        Schedule(() =>
        {
            // adjust the scroll position.
            var settingsHeader = this.ChildrenOfType<EditorSettingsHeader>().FirstOrDefault();

            if (settingsHeader != null)
            {
                var scrollContainer = this.ChildrenOfType<OsuScrollContainer>().First();
                scrollContainer.Margin = new MarginPadding { Top = settingsHeader.DrawHeight };
            }

            // re-create the content.
            var content = this.ChildrenOfType<FillFlowContainer>().First();
            content.Children = CreateSections();
        });
    }

    protected void ChangeBackgroundColour(Colour4 colour4)
    {
        this.ChildrenOfType<Box>().First().Colour = colour4;

        // apply colour after header ready.
        Schedule(() =>
        {
            var settingsHeader = this.ChildrenOfType<EditorSettingsHeader>().FirstOrDefault();
            if (settingsHeader != null)
                settingsHeader.BackgroundColour = colour4.Darken(0.4f);
        });
    }

    protected sealed override IReadOnlyList<Drawable> CreateSections()
    {
        return CreateEditorSections();
    }

    protected virtual EditorSettingsHeader? CreateSettingHeader() => null;

    protected abstract IReadOnlyList<EditorSection> CreateEditorSections();
}
