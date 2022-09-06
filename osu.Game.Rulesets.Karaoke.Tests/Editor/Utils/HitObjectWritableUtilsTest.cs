// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Utils
{
    public class HitObjectWritableUtilsTest
    {
        #region Lyric property

        [Test]
        public void TestIsWriteLyricPropertyLocked()
        {
            // standard.
            test(new Lyric());

            // test lock state.
            foreach (var lockState in EnumUtils.GetValues<LockState>())
            {
                test(new Lyric
                {
                    Lock = lockState
                });
            }

            // reference lyric.
            test(new Lyric
            {
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            });

            test(new Lyric
            {
                ReferenceLyricConfig = new SyncLyricConfig(),
            });

            void test(Lyric lyric)
            {
                testEveryWritablePropertiesInObjectAtTheSameTime(lyric, (l, propertyName) => HitObjectWritableUtils.IsWriteLyricPropertyLocked(l, propertyName));
                testEveryWritablePropertiesInObject(lyric, (l, propertyName) => HitObjectWritableUtils.IsWriteLyricPropertyLocked(l, propertyName));

                testEveryWritablePropertiesInObjectAtTheSameTime(lyric, (l, propertyName) => HitObjectWritableUtils.GetLyricPropertyLockedBy(l, propertyName));
                testEveryWritablePropertiesInObject(lyric, (l, propertyName) => HitObjectWritableUtils.GetLyricPropertyLockedBy(l, propertyName));
            }
        }

        #endregion

        #region Create or remove notes.

        [Test]
        public void TestIsCreateOrRemoveNoteLocked()
        {
            // standard.
            test(new Lyric());

            // test lock state.
            foreach (var lockState in EnumUtils.GetValues<LockState>())
            {
                test(new Lyric
                {
                    Lock = lockState
                });
            }

            // reference lyric.
            test(new Lyric
            {
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            });

            test(new Lyric
            {
                ReferenceLyricConfig = new SyncLyricConfig(),
            });

            void test(Lyric lyric)
            {
                HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(lyric);
                HitObjectWritableUtils.GetCreateOrRemoveNoteLockedBy(lyric);
            }
        }

        #endregion

        #region Note property

        [Test]
        public void TestIsWriteNotePropertyLocked()
        {
            // standard.
            test(new Note());

            // test with reference lyric.
            test(new Note
            {
                ReferenceLyric = new Lyric(),
            });

            void test(Note note)
            {
                testEveryWritablePropertiesInObjectAtTheSameTime(note, (l, propertyName) => HitObjectWritableUtils.IsWriteNotePropertyLocked(l, propertyName));
                testEveryWritablePropertiesInObject(note, (l, propertyName) => HitObjectWritableUtils.IsWriteNotePropertyLocked(l, propertyName));

                testEveryWritablePropertiesInObjectAtTheSameTime(note, (l, propertyName) => HitObjectWritableUtils.GetNotePropertyLockedBy(l, propertyName));
                testEveryWritablePropertiesInObject(note, (l, propertyName) => HitObjectWritableUtils.GetNotePropertyLockedBy(l, propertyName));
            }
        }

        #endregion

        private static void testEveryWritablePropertiesInObjectAtTheSameTime<THitObject>(THitObject hitObject, Action<THitObject, string[]> action)
        {
            // the purpose of this test case if focus on checking every property in the hit-object should be able to know the writable or not.
            // return value is not in the test scope.
            string[] allWriteableProperties = typeof(THitObject).GetProperties()
                                                                .Where(x => x.CanRead && x.CanWrite)
                                                                .Where(x => x.CustomAttributes.All(customAttributeData => customAttributeData.AttributeType != typeof(JsonIgnoreAttribute)))
                                                                .Select(x => x.Name)
                                                                .ToArray();
            action(hitObject, allWriteableProperties);
        }

        private static void testEveryWritablePropertiesInObject<THitObject>(THitObject hitObject, Action<THitObject, string> action)
        {
            testEveryWritablePropertiesInObjectAtTheSameTime(hitObject, (l, propertyNames) =>
            {
                foreach (string propertyName in propertyNames)
                {
                    action(hitObject, propertyName);
                }
            });
        }
    }
}
