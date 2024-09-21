// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

[Cached(typeof(IInteractableLyricState))]
public sealed partial class InteractableLyric : CompositeDrawable, IHasTooltip, IInteractableLyricState
{
    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
    private readonly IBindable<int> bindableLyricPropertyWritableVersion;

    private readonly Lyric lyric;
    private LocalisableString? lockReason;

    public Action<InteractableLyric, Vector2>? TextSizeChanged;

    public InteractableLyric(Lyric lyric)
    {
        this.lyric = lyric;

        bindableLyricPropertyWritableVersion = lyric.LyricPropertyWritableVersion.GetBoundCopy();

        bindableMode.BindValueChanged(x =>
        {
            triggerWritableVersionChanged();
        });

        bindableLyricPropertyWritableVersion.BindValueChanged(_ =>
        {
            triggerWritableVersionChanged();
        });
    }

    public IEnumerable<LayerLoader> Loaders
    {
        init
        {
            foreach (var loader in value)
            {
                var layer = loader.CreateLayer(lyric);
                AddInternal(layer);
            }

            var lyricLayers = layers.OfType<LyricLayer>().Single();
            lyricLayers.SizeChanged = size =>
            {
                TextSizeChanged?.Invoke(this, size);
            };
        }
    }

    private IEnumerable<Layer> layers => InternalChildren.OfType<Layer>();

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        var lyricLayer = layers.OfType<LyricLayer>().Single();
        baseDependencies.CacheAs<IPreviewLyricPositionProvider>(lyricLayer);
        return baseDependencies;
    }

    public void TriggerDisallowEditEffect()
    {
        layers.ForEach(x => x.TriggerDisallowEditEffect(bindableMode.Value));
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state)
    {
        bindableMode.BindTo(state.BindableMode);
    }

    private void triggerWritableVersionChanged()
    {
        var loadReason = GetLyricPropertyLockedReason(lyric, bindableMode.Value);
        lockReason = loadReason;

        // adjust the style.
        bool editable = lockReason == null;
        layers.ForEach(x => x.UpdateDisableEditState(editable));
    }

    public LocalisableString TooltipText => lockReason ?? string.Empty;

    public static LocalisableString? GetLyricPropertyLockedReason(Lyric lyric, LyricEditorMode mode)
    {
        var reasons = getLyricPropertyLockedReasons(lyric, mode);

        return reasons switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot modify this property due to this lyric is property is sync from another lyric.",
            LockLyricPropertyBy.LockState => "This property is locked and not editable",
            null => default(LocalisableString?),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private static LockLyricPropertyBy? getLyricPropertyLockedReasons(Lyric lyric, LyricEditorMode mode)
    {
        return mode switch
        {
            LyricEditorMode.View => null,
            LyricEditorMode.EditText => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.Text), nameof(Lyric.RubyTags), nameof(Lyric.TimeTags)),
            LyricEditorMode.EditReferenceLyric => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.ReferenceLyric), nameof(Lyric.ReferenceLyricConfig)),
            LyricEditorMode.EditLanguage => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.Language)),
            LyricEditorMode.EditRuby => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RubyTags)),
            LyricEditorMode.EditTimeTag => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.TimeTags)),
            LyricEditorMode.EditRomanisation => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.TimeTags)),
            LyricEditorMode.EditNote => HitObjectWritableUtils.GetCreateOrRemoveNoteLockedBy(lyric),
            LyricEditorMode.EditSinger => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.SingerIds)),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }
}
