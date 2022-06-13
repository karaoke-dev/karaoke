// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Database;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components
{
    public class SingerAvatar : CompositeDrawable, ICanAcceptFiles, IHasPopover
    {
        private readonly string[] handledExtensions = { ".jpg", ".jpeg", ".png" };

        public IEnumerable<string> HandledExtensions => handledExtensions;

        private readonly Bindable<FileInfo> currentFile = new();

        [Resolved]
        private OsuGameBase game { get; set; }

        [Resolved]
        private ISingersChangeHandler singersChangeHandler { get; set; }

        private readonly Singer singer;

        public SingerAvatar(Singer singer)
        {
            this.singer = singer;

            InternalChildren = new[]
            {
                new DrawableSingerAvatar
                {
                    RelativeSizeAxes = Axes.Both,
                    Singer = singer
                }
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.ShowPopover();
            return true;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            game.RegisterImportHandler(this);
            currentFile.BindValueChanged(onFileSelected);
        }

        private void onFileSelected(ValueChangedEvent<FileInfo> file)
        {
            if (file.NewValue == null)
                return;

            this.HidePopover();

            singersChangeHandler.ChangeSingerAvatar(singer, file.NewValue);
        }

        Task ICanAcceptFiles.Import(params string[] paths)
        {
            Schedule(() => currentFile.Value = new FileInfo(paths.First()));
            return Task.CompletedTask;
        }

        Task ICanAcceptFiles.Import(params ImportTask[] tasks) => throw new NotImplementedException();

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (game.IsNotNull())
                game.UnregisterImportHandler(this);
        }

        public Popover GetPopover() => new FileChooserPopover(handledExtensions, currentFile);

        private class FileChooserPopover : OsuPopover
        {
            public FileChooserPopover(string[] handledExtensions, Bindable<FileInfo> currentFile)
            {
                Child = new Container
                {
                    Size = new Vector2(600, 400),
                    Child = new OsuFileSelector(currentFile.Value?.DirectoryName, handledExtensions)
                    {
                        RelativeSizeAxes = Axes.Both,
                        CurrentFile = { BindTarget = currentFile }
                    },
                };
            }
        }
    }
}
