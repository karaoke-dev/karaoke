// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public abstract class PropertyDetector<TItem, TProperty, TConfig> : PropertyDetector<TItem, TProperty>
    where TConfig : IHasConfig<TConfig>, new()
{
    protected readonly TConfig Config;

    protected PropertyDetector(TConfig config)
    {
        Config = config;
    }
}

public abstract class PropertyDetector<TItem, TProperty>
{
    /// <summary>
    /// Determined if detect <typeparamref name="TProperty"/> from <typeparamref name="TItem"/> is supported.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool CanDetect(TItem item) => GetInvalidMessage(item) == null;

    /// <summary>
    /// Will get the invalid message if <typeparamref name="TProperty"/> from the <typeparamref name="TItem"/> is not able to be detected.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public LocalisableString? GetInvalidMessage(TItem item)
        => GetInvalidMessageFromItem(item);

    /// <summary>
    /// Detect the <typeparamref name="TProperty"/> from the <typeparamref name="TItem"/>.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public TProperty Detect(TItem item)
    {
        if (!CanDetect(item))
            throw new NotDetectatableException();

        return DetectFromItem(item);
    }

    protected abstract LocalisableString? GetInvalidMessageFromItem(TItem item);

    protected abstract TProperty DetectFromItem(TItem item);
}

