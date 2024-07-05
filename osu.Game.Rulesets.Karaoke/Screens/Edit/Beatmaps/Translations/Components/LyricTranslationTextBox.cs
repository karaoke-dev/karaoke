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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translations.Components;

public partial class LyricTranslationTextBox : OsuTextBox
{
    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    [Resolved]
    private ILyricTranslationChangeHandler lyricTranslationChangeHandler { get; set; } = null!;

    [Resolved]
    private ITranslationInfoProvider translationInfoProvider { get; set; } = null!;

    private readonly IBindable<CultureInfo?> currentLanguage = new Bindable<CultureInfo?>();

    private readonly Lyric lyric;

    public LyricTranslationTextBox(Lyric lyric)
    {
        this.lyric = lyric;

        currentLanguage.BindValueChanged(v =>
        {
            var cultureInfo = v.NewValue;

            // disable and clear text box if contains no language in language list.
            Text = cultureInfo != null ? translationInfoProvider.GetLyricTranslation(lyric, cultureInfo) : null;
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

            lyricTranslationChangeHandler.UpdateTranslation(cultureInfo, text);
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
