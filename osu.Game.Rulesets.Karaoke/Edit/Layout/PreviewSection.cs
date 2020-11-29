// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.IO;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class PreviewSection : LayoutSection
    {
        private LabelledDropdown<PreviewRatio> previewRatioDropdown;
        private LabelledDropdown<PreviewSample> previewSampleDropdown;
        private StyleLabelledDropdown previewStyleDropdown;

        protected override string Title => "Preview(Won't be saved)";

        [BackgroundDependencyLoader]
        private void load(LayoutManager manager)
        {
            Children = new Drawable[]
            {
                previewRatioDropdown = new LabelledDropdown<PreviewRatio>
                {
                    Label = "Ratio",
                    Description = "Adjust to see different preview ratio.",
                    Items = (PreviewRatio[])Enum.GetValues(typeof(PreviewRatio)),
                },
                previewSampleDropdown = new LabelledDropdown<PreviewSample>
                {
                    Label = "Lyric",
                    Description = "Select different lyric to check layout is valid.",
                    Items = (PreviewSample[])Enum.GetValues(typeof(PreviewSample)),
                },
                previewStyleDropdown = new StyleLabelledDropdown
                {
                    Label = "Style",
                    Description = "Select different style to check layout is valid.",
                    Items = manager.PreviewFontSelections,
                },
            };

            previewRatioDropdown.Current.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case PreviewRatio.WideScreen:
                        manager.PreviewScreenRatio.Value = new DisplayRatio
                        {
                            Width = 16,
                            Height = 9
                        };
                        break;

                    case PreviewRatio.LegacyScreen:
                        manager.PreviewScreenRatio.Value = new DisplayRatio
                        {
                            Width = 4,
                            Height = 3
                        };
                        break;
                }
            }, true);

            previewSampleDropdown.Current.BindValueChanged(e => { manager.PreviewLyric.Value = getLyricSampleBySelection(e.NewValue); }, true);

            previewStyleDropdown.Current.BindValueChanged(e =>
            {
                // todo : might use dropdown to assign singer, not style.
                var singer = SingerUtils.GetSingersIndex(e.NewValue.Key);
                manager.ChangePreviewSinger(singer);
            }, true);
        }

        private Lyric getLyricSampleBySelection(PreviewSample previewSample)
        {
            switch (previewSample)
            {
                case PreviewSample.SampeSmall:
                    return createDefaultLyric("@カラオケ",
                        new[]
                        {
                            "@Ruby1=カ,か",
                            "@Ruby2=ラ,ら",
                            "@Ruby3=オ,お",
                            "@Ruby4=ケ,け"
                        },
                        new[]
                        {
                            "@Romaji1=カ,ka",
                            "@Romaji2=ラ,ra",
                            "@Romaji3=オ,o",
                            "@Romaji4=ケ,ke"
                        }
                        , "karaoke");

                case PreviewSample.SampleMedium:
                    return createDefaultLyric("@[00:18:58]た[00:18:81]だ[00:19:36]風[00:20:09]に[00:20:29]揺[00:20:49]ら[00:20:68]れ[00:20:89]て[00:20:93]",
                        new[]
                        {
                            "@Ruby1=風,かぜ",
                            "@Ruby2=揺,ゆ"
                        },
                        new[]
                        {
                            "@Romaji1=た,ta",
                            "@Romaji2=だ,da",
                            "@Romaji3=風,kaze",
                            "@Romaji4=に,ni",
                            "@Romaji5=揺,yu",
                            "@Romaji6=ら,ra",
                            "@Romaji7=れ,re",
                            "@Romaji8=て,te"
                        }
                        , "karaoke");

                case PreviewSample.SampleLarge:
                    return createDefaultLyric("@灰色(いろ)(いろ)の景色(いろ)(いろ)さえ色づき始める",
                        Array.Empty<string>(), Array.Empty<string>(), "karaoke");

                default:
                    return null;
            }
        }

        private Lyric createDefaultLyric(string text, string[] ruby, string[] romaji, string translate)
        {
            var startTime = Time.Current;
            const double duration = 1000000;

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new LineBufferedReader(stream))
            {
                writer.WriteLine("karaoke file format v1");
                writer.WriteLine("[HitObjects]");

                writer.WriteLine(text);
                ruby?.ForEach(x => writer.WriteLine(x));
                romaji?.ForEach(x => writer.WriteLine(x));

                writer.WriteLine("end");
                writer.Flush();
                stream.Position = 0;

                var lyric = new KaraokeLegacyBeatmapDecoder().Decode(reader).HitObjects.OfType<Lyric>().FirstOrDefault();

                // Check if not null
                if (lyric == null)
                    throw new ArgumentNullException();

                // Apply property
                lyric.StartTime = startTime;
                lyric.Duration = duration;
                lyric.Translates.Add(0, translate);
                lyric.ApplyDisplayTranslate(0);
                lyric.TimeTags = TimeTagsUtils.ToTimeTagList(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(0), startTime },
                    { new TimeTagIndex(4), startTime + duration },
                });

                return lyric;
            }
        }

        internal enum PreviewRatio
        {
            [Description("16:9")]
            WideScreen,

            [Description("4:3")]
            LegacyScreen,
        }

        internal enum PreviewSample
        {
            [Description("Small lyric")]
            SampeSmall,

            [Description("Medium lyric")]
            SampleMedium,

            [Description("Large lyric")]
            SampleLarge
        }

        private class StyleLabelledDropdown : LabelledDropdown<KeyValuePair<int, string>>
        {
            protected override SettingsDropdown<KeyValuePair<int, string>> CreateComponent()
                => new StyleDropdown
                {
                    RelativeSizeAxes = Axes.X,
                };

            private class StyleDropdown : SettingsDropdown<KeyValuePair<int, string>>
            {
                private class StyleDropdownControl : DropdownControl
                {
                    protected override string GenerateItemText(KeyValuePair<int, string> item)
                        => item.Value ?? $"Style{item.Key}";
                }
            }
        }
    }
}
