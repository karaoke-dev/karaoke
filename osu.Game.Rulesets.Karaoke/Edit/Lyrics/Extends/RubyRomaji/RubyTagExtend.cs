// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagExtend : TextTagExtend
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            EditMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case TextTagEditMode.Generate:
                        Children = new Drawable[]
                        {
                            new TextTagEditModeSection(),
                            new RubyTagAutoGenerateSection(),
                        };
                        break;

                    case TextTagEditMode.Edit:
                        Children = new Drawable[]
                        {
                            new TextTagEditModeSection(),
                            new RubyTagEditSection(),
                        };
                        break;

                    case TextTagEditMode.Verify:
                        Children = new Drawable[]
                        {
                            new TextTagEditModeSection(),
                            new RubyTagIssueSection(),
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }
    }
}
