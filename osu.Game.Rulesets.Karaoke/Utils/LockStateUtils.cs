// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq.Expressions;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LockStateUtils
    {
        public static bool IsPropertyLock<T>(LockState currentState, Expression<Func<T>> action)
        {
            var state = GetLyricLockState(action);

            return state < currentState;
        }

        public static LockState GetLyricLockState<T>(Expression<Func<T>> action)
        {
            if (!(action.Body is MemberExpression memberExpression))
                throw new Exception();

            return GetLyricLockStateByPropertyName(memberExpression.Member.Name);
        }

        public static LockState GetLyricLockStateByPropertyName(string name)
        {
            switch (name)
            {
                case "Text":
                case "TimeTags":
                    return LockState.Partial;

                case "LyricStartTime":
                case "LyricEndTime":
                    return LockState.Full;

                case "RubyTags":
                case "RomajiTags":
                    return LockState.Partial;

                case "StartTime":
                case "Duration":
                    return LockState.Full;

                case "Singers":
                case "LayoutIndex":
                case "Translates":
                case "Language":
                    return LockState.Full;

                // todo : can edit it at any time.
                case "Order":
                case "Lock":
                    return LockState.Full;

                default:
                    throw new NotSupportedException(nameof(name));
            }
        }
    }
}
