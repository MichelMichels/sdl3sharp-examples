using Sdl3Sharp;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Events;
using Math = Sdl3Sharp.Utilities.Math;
using Timer = Sdl3Sharp.Timing.Timer;
using Sdl3Sharp.Video.Drawing;

namespace MichelMichels.Sdl3SharpExamples.Rectangles;

public class App : AppBase
{
    private const int WINDOW_WIDTH = 640;
    private const int WINDOW_HEIGHT = 480;

    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("05 Rectangles", WINDOW_WIDTH, WINDOW_HEIGHT, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        ulong now = Timer.MillisecondTicks;
        List<Rect<float>> rectangles = [];

        // we'll have the rectangles grow and shrink over a few seconds.
        float direction = ((now % 2000) >= 1000) ? 1.0f : -1.0f;
        float scale = (((int)(now % 1000)) - 500) / 500.0f * direction;

        // as you can see from this, rendering draws over whatever was drawn before it.
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(0, 0, 0, 255);  // black, full alpha        
        mRenderer.TryClear();  // start with a blank canvas.

        /* Rectangles are comprised of set of X and Y coordinates, plus width and
           height. (0, 0) is the top left of the window, and larger numbers go
           down and to the right. This isn't how geometry works, but this is
           pretty standard in 2D graphics. */

        // Let's draw a single rectangle (square, really).
        rectangles.Add(new Rect<float>(100, 100, 100 + (100 * scale), 100 + (100 * scale)));
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(255, 0, 0, 255);  // red, full alpha 
        mRenderer.TryRenderRect(rectangles[0]);

        // Now let's draw several rectangles with one function call.
        for (int i = 0; i < 3; i++)
        {
            float size = (i + 1) * 50.0f;

            float scaledSize = size + (size * scale);
            float centerX = (WINDOW_WIDTH - scaledSize) / 2;  /* center it. */
            float centerY = (WINDOW_HEIGHT - scaledSize) / 2;  /* center it. */
            rectangles.Add(new Rect<float>(centerX, centerY, scaledSize, scaledSize));
        }

        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(0, 255, 0, 255);  // green, full alpha 
        mRenderer.TryRenderRects(rectangles.Skip(1).ToArray()); // draw three rectangles at once 

        // those were rectangle _outlines_, really. You can also draw _filled_ rectangles!
        rectangles.Add(new Rect<float>(400, 50, 100 + (100 * scale), 50 + (50 * scale)));
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(0, 0, 255, 255);  /* blue, full alpha */
        mRenderer.TryRenderFilledRect(rectangles.Last());

        /* ...and also fill a bunch of rectangles at once... */
        List<Rect<float>> bottomRectangles = [];
        for (int i = 0; i < 16; i++)
        {
            float w = (float)(WINDOW_WIDTH / 16);
            float h = i * 8.0f;
            float x = i * w;
            float y = WINDOW_HEIGHT - h;
            bottomRectangles.Add(new Rect<float>(x, y, w, h));
        }
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(255, 255, 255, 255);  /* white, full alpha */
        mRenderer.TryRenderFilledRects(bottomRectangles.ToArray());

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