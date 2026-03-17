using Sdl3Sharp;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Events;
using Timer = Sdl3Sharp.Timing.Timer;
using Random = Sdl3Sharp.Utilities.Random;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Coloring;

namespace MichelMichels.Sdl3SharpExamples.Points;

public class App : AppBase
{
    private const int WINDOW_WIDTH = 640;
    private const int WINDOW_HEIGHT = 480;
    private const int NUM_POINTS = 500;
    private const int MIN_PIXELS_PER_SECOND = 30; // move at least this many pixels per second.
    private const int MAX_PIXELS_PER_SECOND = 60; // move this many pixels per second at most.

    /* (track everything as parallel arrays instead of a array of structs,
       so we can pass the coordinates to the renderer in a single function call.) */

    /* Points are plotted as a set of X and Y coordinates.
       (0, 0) is the top left of the window, and larger numbers go down
       and to the right. This isn't how geometry works, but this is pretty
       standard in 2D graphics. */
    private Point<float>[] points = new Point<float>[NUM_POINTS];
    private float[] pointSpeeds = new float[NUM_POINTS];
    private ulong lastTime = 0;

    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("04 Points", WINDOW_WIDTH, WINDOW_HEIGHT, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        // set up the data for a bunch of points.
        for (int i = 0; i < points.Length; i++)
        {
            float x = Random.NextFloat() * WINDOW_WIDTH;
            float y = Random.NextFloat() * WINDOW_HEIGHT;
            points[i] = new Point<float>(x, y);
            pointSpeeds[i] = MIN_PIXELS_PER_SECOND + (Random.NextFloat() * (MAX_PIXELS_PER_SECOND - MIN_PIXELS_PER_SECOND));
        }

        lastTime = Timer.MillisecondTicks;

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        ulong now = Timer.MillisecondTicks;

        float elapsed = (now - lastTime) / 1000.0f; // seconds since last iteration

        // let's move all our points a little for a new frame.
        for (int i = 0; i < points.Length; i++)
        {
            float distance = elapsed * pointSpeeds[i];
            points[i] = new Point<float>(points[i].X + distance, points[i].Y + distance);
            if ((points[i].X >= WINDOW_WIDTH) || (points[i].Y >= WINDOW_HEIGHT))
            {
                // off the screen; restart it elsewhere!
                if (Random.Next(2) == 1)
                {
                    points[i] = new Point<float>(Random.NextFloat() * WINDOW_WIDTH, 0.0f);
                }
                else
                {
                    points[i] = new Point<float>(0.0f, Random.NextFloat() * WINDOW_HEIGHT);
                }

                pointSpeeds[i] = MIN_PIXELS_PER_SECOND + (Random.NextFloat() * (MAX_PIXELS_PER_SECOND - MIN_PIXELS_PER_SECOND));
            }
        }

        lastTime = now;

        // as you can see from this, rendering draws over whatever was drawn before it.
        mRenderer.DrawColor = new Color<byte>(0, 0, 0, Color.OpaqueAlphaByte); // black, full alpha
        mRenderer.TryClear();  // start with a blank canvas.
        mRenderer.DrawColor = new Color<byte>(255, 255, 255, Color.OpaqueAlphaByte);  // white, full alpha
        mRenderer.TryRenderPoints(points);  // draw all the points! 

        /* You can also draw single points with SDL_RenderPoint(), but it's
           cheaper (sometimes significantly so) to do them all at once. */

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