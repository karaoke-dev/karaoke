// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate.Components;

public partial class LyricTranslateTextBox : OsuTextBox
{
    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    [Resolved]
    private ILyricTranslateChangeHandler lyricTranslateChangeHandler { get; set; } = null!;

    [Resolved]
    private ITranslateInfoProvider translateInfoProvider { get; set; } = null!;

    private readonly IBindable<CultureInfo?> currentLanguage = new Bindable<CultureInfo?>();

    private readonly Lyric lyric;

    public LyricTranslateTextBox(Lyric lyric)
    {
        this.lyric = lyric;

        currentLanguage.BindValueChanged(v =>
        {
            var cultureInfo = v.NewValue;

            // disable and clear text box if contains no language in language list.
            Text = cultureInfo != null ? translateInfoProvider.GetLyricTranslate(lyric, cultureInfo) : null;
            ScheduleAfterChildren(() =>
            {
                Current.Disabled = cultureInfo == null;
            });
        }, true);

        OnCommit += (t, newText) =>
        {
            if (!newText)
                return;

            string text = t.Text.Trim();

            var cultureInfo = currentLanguage.Value;
            if (cultureInfo == null)
                return;

            lyricTranslateChangeHandler.UpdateTranslate(cultureInfo, text);
        };
    }

    [BackgroundDependencyLoader]
    private void load(IBindable<CultureInfo?> currentLanguage)
    {
        this.currentLanguage.BindTo(currentLanguage);
    }

    protected override void OnFocus(FocusEvent e)
    {
        base.OnFocus(e);
        beatmap.SelectedHitObjects.Add(lyric);
    }

    protected override void OnFocusLost(FocusLostEvent e)
    {
        base.OnFocusLost(e);
        Schedule(() =>
        {
            // should remove lyric until commit finished.
            beatmap.SelectedHitObjects.Remove(lyric);
        });
    }
}
