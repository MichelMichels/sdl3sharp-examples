using MichelMichels.Sdl3SharpExamples.Primitives;
using Sdl3Sharp;

using Sdl sdl = new(static builder => builder
    .SetAppName("02 - Primitives")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);