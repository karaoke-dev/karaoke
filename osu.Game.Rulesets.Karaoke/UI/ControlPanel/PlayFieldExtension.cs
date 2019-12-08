// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel
{
    /// <summary>
    /// Get the "state" of playField
    /// </summary>
    public static class PlayFieldExtension
    {
        /// <summary>
        /// If the number is larger , will have more preemp time
        /// </summary>
        public static double PrepareTime { get; set; } = 0;

        public static double Speed { get; set; } = 1;

        public static double Tone { get; set; } = 1;

        public static double Offset { get; set; }

        public static double Volumn { get; set; } = 1;

        /// <summary>
        /// NavigationToFirst
        /// </summary>
        /// <param name="karaokeField"></param>
        public static void NavigationToFirst(this KaraokePlayfield karaokeField)
        {
            var firstObject = karaokeField.FirstObjectTime();
            karaokeField.NavigateToTime(firstObject - PrepareTime);
        }

        /// <summary>
        /// NavigationToPrevious
        /// </summary>
        /// <param name="karaokeField"></param>
        public static void NavigationToPrevious(this KaraokePlayfield karaokeField)
        {
            var nowObjectIndex = karaokeField.FindObjectIndexByCurrentTime();

            if (nowObjectIndex > 1)
            {
                var list = karaokeField.GetListHitObjects();
                karaokeField.NavigateToTime(list[nowObjectIndex - 1].StartTime - PrepareTime);
            }
        }

        /// <summary>
        /// NavigationToNext
        /// </summary>
        /// <param name="karaokeField"></param>
        public static void NavigationToNext(this KaraokePlayfield karaokeField)
        {
            var nowObjectIndex = karaokeField.FindObjectIndexByCurrentTime();
            var list = karaokeField.GetListHitObjects();

            if (nowObjectIndex < list.Count - 2)
                karaokeField.NavigateToTime(list[nowObjectIndex + 2].StartTime - PrepareTime);
        }

        /// <summary>
        /// Play //TODO : still need to implement
        /// </summary>
        /// <param name="karaokeField"></param>
        public static void Play(this KaraokePlayfield karaokeField)
        {
            //karaokeField.WorkingBeatmap.Track.Start();
            karaokeField.WorkingBeatmap.Track.TempoAdjust = Speed;
            karaokeField.WorkingBeatmap.Track.Volume.Value = Volumn;
        }

        /// <summary>
        /// Check is playing //TODO : still need to implement
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <returns></returns>
        public static bool IsPlaying(this KaraokePlayfield karaokeField)
        {
            return karaokeField.WorkingBeatmap.Track.IsRunning;
        }

        /// <summary>
        /// Pause the song //TODO : still need to implement
        /// </summary>
        /// <param name="karaokeField"></param>
        public static void Pause(this KaraokePlayfield karaokeField)
        {
            //Play and pause are the same
            karaokeField.WorkingBeatmap.Track.Stop();

            //use stupid method instead;
            //karaokeField.WorkingBeatmap.Track.TempoAdjust = 0;
            //Volumn = karaokeField.WorkingBeatmap.Track.Volume.Value;
            //karaokeField.WorkingBeatmap.Track.Volume.Value = 0;
        }

        /// <summary>
        /// Navigatte to target time
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <param name="value"></param>
        public static void NavigateToTime(this KaraokePlayfield karaokeField, double value)
        {
            karaokeField?.WorkingBeatmap?.Track?.Seek(value);
        }

        /// <summary>
        /// Adjust offset
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <param name="value"></param>
        public static void AdjustlyricsOffset(this KaraokePlayfield karaokeField, double value)
        {
            //TODO : maybe use offset ?
            //1. adjust config.GetBindable<double>(OsuSetting.AudioOffset); ,but will change the offset to another modes,
            //2. get offsetClock from player
            Offset = value;
        }

        /// <summary>
        /// First Object's time
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <returns></returns>
        public static double FirstObjectTime(this KaraokePlayfield karaokeField)
        {
            //RulesetContainer.Objects;
            //Refrenca : SongProgress.cs
            return karaokeField.WorkingBeatmap.Beatmap.HitObjects.First().StartTime;
        }

        /// <summary>
        /// Last object's time
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <returns></returns>
        public static double LastObjectTime(this KaraokePlayfield karaokeField)
        {
            var hitObjects = karaokeField.GetListHitObjects();
            return hitObjects.Last().GetEndTime() + 1;
        }

        /// <summary>
        /// Use to get the current time
        /// </summary>
        /// <returns></returns>
        public static double GetCurrentTime(this KaraokePlayfield karaokeField)
        {
            return karaokeField.WorkingBeatmap.Track.CurrentTime;
        }

        /// <summary>
        /// FindObjectByCurrentTime
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <returns></returns>
        public static HitObject FindObjectByCurrentTime(this KaraokePlayfield karaokeField)
        {
            var currentTime = karaokeField.GetCurrentTime();
            var listObjects = karaokeField.GetListHitObjects();

            for (var i = 0; i < listObjects.Count; i++)
            {
                if (listObjects[i].StartTime >= currentTime + PrepareTime)
                {
                    if (i == 0)
                        return null;

                    return listObjects[i - 1];
                }
            }

            return null;
        }

        /// <summary>
        /// FindObjectIndexByCurrentTime
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <returns></returns>
        public static int FindObjectIndexByCurrentTime(this KaraokePlayfield karaokeField)
        {
            var hitObject = karaokeField.FindObjectByCurrentTime();
            if (hitObject == null)
                return -1;

            var listObjects = karaokeField.GetListHitObjects();

            for (var i = 0; i < listObjects.Count; i++)
            {
                if (listObjects[i] == hitObject)
                    return i;
            }

            //404
            return -1;
        }

        /// <summary>
        /// Get list HitObjects
        /// </summary>
        /// <param name="karaokeField"></param>
        /// <returns></returns>
        public static List<HitObject> GetListHitObjects(this KaraokePlayfield karaokeField)
        {
            return karaokeField.WorkingBeatmap.Beatmap.HitObjects.ToList();
        }
    }
}
