using Sdl3Sharp;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Events;
using Math = Sdl3Sharp.Utilities.Math;
using Timer = Sdl3Sharp.Timing.Timer;
using Sdl3Sharp.Video.Drawing;

namespace MichelMichels.Sdl3SharpExamples.Primitives;

public class App : AppBase
{
    private const int WINDOW_WIDTH = 640;
    private const int WINDOW_HEIGHT = 480;

    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    private static readonly Point<float>[] points = new Point<float>[500];

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("02 Primitives", WINDOW_WIDTH, WINDOW_HEIGHT, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        // set up some random points
        for (int i = 0; i < points.Length; i++)
        {
            float x = (Sdl3Sharp.Utilities.Random.NextFloat() * 440.0f) + 100.0f;
            float y = (Sdl3Sharp.Utilities.Random.NextFloat() * 280.0f) + 100.0f;
            points[i] = new Point<float>(x, y);
        }

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        // as you can see from this, rendering draws over whatever was drawn before it.
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(33, 33, 33, 255); // dark gray, full alpha
        mRenderer.TryClear();

        // draw a filled rectangle in the middle of the canvas.
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(0, 0, 255, 255); // blue, full alpha

        int x = 100;
        int y = 100;
        int width = 440;
        int height = 280;
        Rect<float> rect = new(x, y, width, height);
        mRenderer.TryRenderFilledRect(rect);

        // draw some points across the canvas.
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(255, 0, 0, 255); // red, full alpha
        mRenderer.TryRenderPoints(points);

        // draw a unfilled rectangle in-set a little bit.
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(0, 255, 0, 255); // green full alpha

        Rect<float> inner = new(rect.Left + 30, rect.Top + 30, rect.Width - 60, rect.Height - 60);
        mRenderer.TryRenderRect(inner);

        // draw two lines in an X across the whole canvas.
        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(255, 255, 0, 255); // yellow, full alpha
        mRenderer.TryRenderLine(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
        mRenderer.TryRenderLine(0, WINDOW_HEIGHT, WINDOW_WIDTH, 0);

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