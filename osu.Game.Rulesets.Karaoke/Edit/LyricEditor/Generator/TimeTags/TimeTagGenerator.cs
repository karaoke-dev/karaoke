// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.


namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.TimeTags
{
    public class TimeTagGenerator<T> where T : TimeTagGeneratorConfig
    {
        protected T Config { get; private set; }

        protected TimeTagGenerator(T config)
        {
            Config = config;
        }
    }
}
