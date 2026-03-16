using MichelMichels.Sdl3SharpExamples.Rectangles;
using Sdl3Sharp;

using Sdl sdl = new(static builder => builder
    .SetAppName("05 - Rectangles")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);