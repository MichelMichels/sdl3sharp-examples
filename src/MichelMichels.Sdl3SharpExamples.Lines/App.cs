using Sdl3Sharp;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Events;
using Math = Sdl3Sharp.Utilities.Math;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Coloring;

namespace MichelMichels.Sdl3SharpExamples.Lines;

public class App : AppBase
{
    private const int WINDOW_WIDTH = 640;
    private const int WINDOW_HEIGHT = 480;

    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("03 Lines", WINDOW_WIDTH, WINDOW_HEIGHT, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        /* Lines (line segments, really) are drawn in terms of points: a set of
           X and Y coordinates, one set for each end of the line.
           (0, 0) is the top left of the window, and larger numbers go down
           and to the right. This isn't how geometry works, but this is pretty
           standard in 2D graphics. */
        List<Point<float>> points =
        [
            new(100, 354),
            new(220, 230),
            new(140, 230),
            new(320, 100),
            new(500, 230),
            new(420, 230),
            new(540, 354),
            new(400, 354),
            new(100, 354)
        ];

        // as you can see from this, rendering draws over whatever was drawn before it. 
        mRenderer.DrawColor = new Color<byte>(100, 100, 100, Color.OpaqueAlphaByte);  // grey, full alpha
        mRenderer.TryClear(); // start with a blank canvas

        // You can draw lines, one at a time, like these brown ones...
        mRenderer.DrawColor = new Color<byte>(127, 49, 32, Color.OpaqueAlphaByte);

        mRenderer.TryRenderLine(240, 450, 400, 450);
        mRenderer.TryRenderLine(240, 356, 400, 356);
        mRenderer.TryRenderLine(240, 356, 240, 450);
        mRenderer.TryRenderLine(400, 356, 400, 450);

        // You can also draw a series of connected lines in a single batch... 
        mRenderer.DrawColor = new Color<byte>(0, 255, 0, Color.OpaqueAlphaByte);
        mRenderer.TryRenderLines(points.ToArray());

        // here's a bunch of lines drawn out from a center point in a circle.
        // we randomize the color of each line, so it functions as animation.
        for (int i = 0; i < 360; i++)
        {
            float size = 30.0f;
            float x = 320.0f;
            float y = 95.0f - (size / 2.0f);
            float radius = i * (Math.PiF / 180.0f);

            byte r = (byte)Sdl3Sharp.Utilities.Random.Next(256);
            byte g = (byte)Sdl3Sharp.Utilities.Random.Next(256);
            byte b = (byte)Sdl3Sharp.Utilities.Random.Next(256);
            mRenderer.DrawColor = new Color<byte>(r, g, b, Color.OpaqueAlphaByte);
            mRenderer.TryRenderLine(x, y, x + Math.Cos(radius) * size, y + Math.Sin(radius) * size);
        }

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