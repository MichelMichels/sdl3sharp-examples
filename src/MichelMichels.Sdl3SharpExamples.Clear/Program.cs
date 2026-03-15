using MichelMichels.Sdl3SharpExamples.Clear;
using Sdl3Sharp;

using Sdl sdl = new(static builder => builder
    .SetAppName("01 - Clear")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);