// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class DrawableKaraokeEditRuleset : DrawableKaraokeRuleset
    {
        public DrawableKaraokeEditRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
        }

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
        {
            switch (h)
            {
                case LyricLine lyric:
                    return new DrawableLyricLine(lyric);

                case Note note:
                    return new DrawableNote(note);
            }

            return null;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.S)
            {
                string directory = Path.Combine(Path.GetTempPath(), @"osu!");
                Directory.CreateDirectory(directory);
                var path = Path.Combine(directory, "karaoke.txt");

                using (var sw = new StreamWriter(path))
                {
                    var encoder = new KaraokeLegacyBeatmapEncoder();
                    sw.WriteLine(encoder.Encode(new Beatmap
                    {
                        HitObjects = Beatmap.HitObjects.OfType<HitObject>().ToList()
                    }));
                }

                return true;
            }

            return base.OnKeyDown(e);
        }

        protected override Playfield CreatePlayfield() => new KaraokeEditPlayfield();

        public class KaraokeEditPlayfield : KaraokePlayfield
        {
            public KaraokeEditPlayfield()
            {
                LyricPlayfield.Anchor = LyricPlayfield.Origin = Anchor.BottomCentre;
                LyricPlayfield.Margin = new MarginPadding { Top = 150, Bottom = -100 };
                LyricPlayfield.Scale = new Vector2(0.7f);
            }
        }
    }
}
