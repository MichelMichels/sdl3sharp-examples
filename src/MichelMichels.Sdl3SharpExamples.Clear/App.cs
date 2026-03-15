using Sdl3Sharp;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Events;
using Math = Sdl3Sharp.Utilities.Math;
using Timer = Sdl3Sharp.Timing.Timer;

namespace MichelMichels.Sdl3SharpExamples.Clear;

public class App : AppBase
{
    private const int WINDOW_WIDTH = 800;
    private const int WINDOW_HEIGHT = 600;

    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("01 Clear", WINDOW_WIDTH, WINDOW_HEIGHT, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        double now = ((double)Timer.MillisecondTicks) / 1000.0;  // convert from milliseconds to seconds.

        // choose the color for the frame we will draw. The sine wave trick makes it fade between colors smoothly.
        float red = (float)(0.5 + 0.5 * Math.Sin(now));
        float green = (float)(0.5 + 0.5 * Math.Sin(now + Math.Pi * 2 / 3));
        float blue = (float)(0.5 + 0.5 * Math.Sin(now + Math.Pi * 4 / 3));

        mRenderer.DrawColorFloat = new Sdl3Sharp.Video.Coloring.Color<float>(red, green, blue, 1); // new color, full alpha

        // clear the window to the draw color.
        mRenderer.TryClear();

        // put the newly-cleared rendering on the screen.
        mRenderer.TryRenderPresent();

        return Continue;
    }

    protected override AppResult OnEvent(Sdl sdl, ref Event @event)
    {
        if (@event.Type is EventType.WindowCloseRequested)
        {
            return Success;
        }

        return Continue;
    }

    protected override void OnQuit(Sdl sdl, AppResult result)
    {
        mRenderer?.Dispose();
        mRenderer = default!;

        mWindow?.Dispose();
        mWindow = default!;
    }
}