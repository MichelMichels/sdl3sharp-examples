using MichelMichels.Sdl3Sharp.Examples.StreamingTextures;
using Sdl3Sharp;

using Sdl sdl = new(static builder => builder
    .SetAppName("07 - Streaming Textures")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);