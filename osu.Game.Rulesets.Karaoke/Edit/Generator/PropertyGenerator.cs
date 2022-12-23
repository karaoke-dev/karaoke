// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public abstract class PropertyGenerator<TItem, TProperty, TConfig> : PropertyGenerator<TItem, TProperty>
    where TConfig : IHasConfig<TConfig>, new()
{
    protected readonly TConfig Config;

    protected PropertyGenerator(TConfig config)
    {
        Config = config;
    }
}

public abstract class PropertyGenerator<TItem, TProperty>
{
    /// <summary>
    /// Determined if generate <typeparamref name="TProperty"/> from <typeparamref name="TItem"/> is supported.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool CanGenerate(TItem item) => GetInvalidMessage(item) == null;

    /// <summary>
    /// Will get the invalid message if <typeparamref name="TProperty"/> from the <typeparamref name="TItem"/> is not able to be generated.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public LocalisableString? GetInvalidMessage(TItem item)
        => GetInvalidMessageFromItem(item);

    /// <summary>
    /// Generate the <typeparamref name="TProperty"/> from the <typeparamref name="TItem"/>.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public TProperty Generate(TItem item)
    {
        if (!CanGenerate(item))
            throw new NotGeneratableException();

        return GenerateFromItem(item);
    }

    protected abstract LocalisableString? GetInvalidMessageFromItem(TItem item);

    protected abstract TProperty GenerateFromItem(TItem item);
}
