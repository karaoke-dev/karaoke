// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagExtend : TextTagExtend
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            EditMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case TextTagEditMode.Edit:
                        Children = new Drawable[]
                        {
                            new TextTagEditModeSection(),
                            new RomajiTagEditSection(),
                        };
                        break;

                    case TextTagEditMode.Generate:
                        Children = new Drawable[]
                        {
                            new TextTagEditModeSection(),
                            new RomajiTagAutoGenerateSection(),
                        };
                        break;

                    case TextTagEditMode.Verify:
                        Children = new Drawable[]
                        {
                            new TextTagEditModeSection(),
                            new RomajiTagIssueSection(),
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }
    }
}
