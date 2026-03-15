using MichelMichels.Sdl3SharpExamples.Lines;
using Sdl3Sharp;

using Sdl sdl = new(static builder => builder
    .SetAppName("03 - Lines")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);