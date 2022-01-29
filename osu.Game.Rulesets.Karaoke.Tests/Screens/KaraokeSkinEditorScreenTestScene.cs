// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.IO;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private readonly KaraokeBeatmapSkin karaokeSkin = new TestKaraokeBeatmapSkin();

        protected override void LoadComplete()
        {
            Child = new SkinProvidingContainer(karaokeSkin)
            {
                RelativeSizeAxes = Axes.Both,
                Child = CreateEditorScreen(karaokeSkin).With(x =>
                {
                    x.State.Value = Visibility.Visible;
                })
            };
        }

        protected abstract T CreateEditorScreen(KaraokeSkin karaokeSkin);

        protected class TestKaraokeBeatmapSkin : KaraokeBeatmapSkin
        {
            public TestKaraokeBeatmapSkin()
                : base(new SkinInfo(), null)
            {
                // TODO : need a better way to load resource
                var assembly = Assembly.GetExecutingAssembly();
                const string resource_name = @"osu.Game.Rulesets.Karaoke.Tests.Resources.Testing.Skin.default.skin";

                using (var stream = assembly.GetManifestResourceStream(resource_name))
                using (var reader = new LineBufferedReader(stream))
                {
                    var karaokeSkin = new KaraokeSkinDecoder().Decode(reader);

                    // Default values
                    BindableDefaultLyricConfig.Value = karaokeSkin.DefaultLyricConfig;
                    BindableDefaultLyricStyle.Value = karaokeSkin.DefaultLyricStyle;
                    BindableDefaultNoteStyle.Value = karaokeSkin.DefaultNoteStyle;

                    // Create bindable
                    for (int i = 0; i < karaokeSkin.Layouts.Count; i++)
                        BindableLayouts.Add(i, new Bindable<LyricLayout>(karaokeSkin.Layouts[i]));
                    for (int i = 0; i < karaokeSkin.LyricStyles.Count; i++)
                        BindableLyricStyles.Add(i, new Bindable<LyricStyle>(karaokeSkin.LyricStyles[i]));
                    for (int i = 0; i < karaokeSkin.NoteStyles.Count; i++)
                        BindableNoteStyles.Add(i, new Bindable<NoteStyle>(karaokeSkin.NoteStyles[i]));

                    // Create lookups
                    BindableFontsLookup.Value = karaokeSkin.LyricStyles.ToDictionary(k => karaokeSkin.LyricStyles.IndexOf(k), y => y.Name);
                    BindableLayoutsLookup.Value = karaokeSkin.Layouts.ToDictionary(k => karaokeSkin.Layouts.IndexOf(k), y => y.Name);
                    BindableNotesLookup.Value = karaokeSkin.NoteStyles.ToDictionary(k => karaokeSkin.NoteStyles.IndexOf(k), y => y.Name);
                }
            }
        }
    }
}
