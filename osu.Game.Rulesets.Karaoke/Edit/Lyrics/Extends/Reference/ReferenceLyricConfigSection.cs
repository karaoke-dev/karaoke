// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Reference
{
    public class ReferenceLyricConfigSection : LyricPropertySection
    {
        private const string sync = "sync";
        private const string reference = "reference";

        protected override LocalisableString Title => "Config";

        [Resolved, AllowNull]
        private ILyricReferenceChangeHandler lyricReferenceChangeHandler { get; set; }

        private readonly IBindable<IReferenceLyricPropertyConfig?> bindableReferenceLyricPropertyConfig = new Bindable<IReferenceLyricPropertyConfig?>();

        private readonly LabelledDropdown<string> labelledReferenceLyricConfig;
        private readonly LabelledSwitchButton labelledSyncEverything;
        private readonly LabelledSwitchButton labelledSyncSinger;
        private readonly LabelledSwitchButton labelledSyncTimeTag;

        private bool isConfigChanging;

        public ReferenceLyricConfigSection()
        {
            Children = new Drawable[]
            {
                labelledReferenceLyricConfig = new LabelledDropdown<string>
                {
                    Label = "Config",
                    Description = "Select the similar lyric that want to reference or sync the property.",
                    Items = new[]
                    {
                        sync,
                        reference
                    }
                },
                labelledSyncEverything = new LabelledSwitchButton
                {
                    Label = "Sync",
                    Description = "Sync most property.",
                    Current =
                    {
                        Disabled = true
                    }
                },
                labelledSyncSinger = new LabelledSwitchButton
                {
                    Label = "Sync singer.",
                    Description = "Un-select the selection if want to customize the singer.",
                },
                labelledSyncTimeTag = new LabelledSwitchButton
                {
                    Label = "Sync time-tags.",
                    Description = "Un-select the selection if want to customize the time-tag.",
                }
            };

            bindableReferenceLyricPropertyConfig.BindValueChanged(e =>
            {
                onConfigChanged();
            }, true);

            labelledReferenceLyricConfig.Current.BindValueChanged(x =>
            {
                if (IsRebinding || isConfigChanging)
                    return;

                switch (x.NewValue)
                {
                    case sync:
                        lyricReferenceChangeHandler.SwitchToSyncLyricConfig();
                        break;

                    case reference:
                        lyricReferenceChangeHandler.SwitchToReferenceLyricConfig();
                        break;

                    default:
                        throw new IndexOutOfRangeException();
                }
            });

            labelledSyncSinger.Current.BindValueChanged(x =>
            {
                if (!IsRebinding && !isConfigChanging)
                    lyricReferenceChangeHandler.AdjustLyricConfig<SyncLyricConfig>(config => config.SyncSingerProperty = x.NewValue);
            });

            labelledSyncTimeTag.Current.BindValueChanged(x =>
            {
                if (!IsRebinding && !isConfigChanging)
                    lyricReferenceChangeHandler.AdjustLyricConfig<SyncLyricConfig>(config => config.SyncTimeTagProperty = x.NewValue);
            });
        }

        protected override void OnLyricChanged(Lyric? lyric)
        {
            bindableReferenceLyricPropertyConfig.UnbindBindings();

            if (lyric != null)
                bindableReferenceLyricPropertyConfig.BindTo(lyric.ReferenceLyricConfigBindable);
        }

        private void onConfigChanged()
        {
            isConfigChanging = true;

            Children.ForEach(x => x.Hide());
            Show();

            var config = bindableReferenceLyricPropertyConfig.Value;

            switch (config)
            {
                case ReferenceLyricConfig:
                    labelledReferenceLyricConfig.Current.Value = reference;
                    labelledReferenceLyricConfig.Show();
                    break;

                case SyncLyricConfig syncLyricConfig:
                    labelledReferenceLyricConfig.Current.Value = sync;
                    labelledSyncSinger.Current = syncLyricConfig.SyncSingerPropertyBindable;
                    labelledSyncTimeTag.Current = syncLyricConfig.SyncTimeTagPropertyBindable;

                    labelledReferenceLyricConfig.Show();
                    labelledSyncEverything.Show();
                    labelledSyncSinger.Show();
                    labelledSyncTimeTag.Show();
                    break;

                case null:
                    Hide();
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }

            isConfigChanging = false;
        }
    }
}
