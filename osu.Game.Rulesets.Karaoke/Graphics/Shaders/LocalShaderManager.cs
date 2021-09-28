// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Shaders;
using osu.Framework.IO.Stores;

namespace osu.Game.Rulesets.Karaoke.Graphics.Shaders
{
    public class LocalShaderManager : ShaderManager
    {
        private static ResourceStore<byte[]> localStore;

        private static ResourceStore<byte[]> createStore(IResourceStore<byte[]> store)
        {
            var resourceStore = new ResourceStore<byte[]>(store);

            // add resource from osu.framework.font
            resourceStore.AddStore(new NamespacedResourceStore<byte[]>(new FontResourceStore(), "Resources"));

            // all shader is in shader namespace.
            var resources = new NamespacedResourceStore<byte[]>(resourceStore, @"Shaders");
            return resources;
        }

        public LocalShaderManager(IResourceStore<byte[]> store)
            : base(localStore = createStore(store))
        {
        }

        public T LocalCustomizedShader<T>() where T : class, ICustomizedShader
        {
            var type = typeof(T);

            return type switch
            {
                Type _ when type == typeof(OutlineShader) => new OutlineShader(Load(VertexShaderDescriptor.TEXTURE_2, OutlineShader.SHADER_NAME)) as T,
                Type _ when type == typeof(RainbowShader) => new RainbowShader(Load(VertexShaderDescriptor.TEXTURE_2, RainbowShader.SHADER_NAME)) as T,
                Type _ when type == typeof(ShadowShader) => new ShadowShader(Load(VertexShaderDescriptor.TEXTURE_2, ShadowShader.SHADER_NAME)) as T,
                _ => throw new NotImplementedException()
            };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            localStore.Dispose();
        }
    }
}
