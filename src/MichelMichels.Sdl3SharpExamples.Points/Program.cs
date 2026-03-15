using MichelMichels.Sdl3SharpExamples.Points;
using Sdl3Sharp;

using Sdl sdl = new(static builder => builder
    .SetAppName("04 - Points")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);