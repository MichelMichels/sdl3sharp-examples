using System.Runtime.InteropServices;
using Sdl3Sharp;
using Sdl3Sharp.Events;
using Sdl3Sharp.Video;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Timing;

namespace MichelMichels.Sdl3SharpExamples.StreamingTextures;

public class App : AppBase
{
    private const int TEXTURE_SIZE = 150;
    private const int WINDOW_WIDTH = 800;
    private const int WINDOW_HEIGHT = 600;

    private Window mWindow = default!;
    private Renderer mRenderer = default!;
    private Texture texture = default!;

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("07 Streaming Textures", WINDOW_WIDTH, WINDOW_HEIGHT, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        if (!mRenderer.TryCreateTexture(PixelFormat.Rgba8888, TextureAccess.Streaming, TEXTURE_SIZE, TEXTURE_SIZE, out texture!))
        {
            return Failure;
        }

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        ulong now = Sdl3Sharp.Timing.Timer.MillisecondTicks;

        // we'll have some color move around over a few seconds.
        float direction = ((now % 2000) >= 1000) ? 1.0f : -1.0f;
        float scale = (((int)(now % 1000)) - 500) / 500.0f * direction;

        /* To update a streaming texture, you need to lock it first. This gets you access to the pixels.
           Note that this is considered a _write-only_ operation: the buffer you get from locking
           might not actually have the existing contents of the texture, and you have to write to every
           locked pixel! */

        /* You can use SDL_LockTexture() to get an array of raw pixels, but we're going to use
           SDL_LockTextureToSurface() here, because it wraps that array in a temporary SDL_Surface,
           letting us use the surface drawing functions instead of lighting up individual pixels. */

        if (texture.TryUnsafeLockToSurface(out Surface? surface))
        {
            surface.Format.TryGetPixelFormatDetails(out PixelFormatDetails pixelFormatDetails);
            surface.TryFill(pixelFormatDetails.MapColor(null, 0, 0, 0)); // Makes the whole surface black

            int width = TEXTURE_SIZE;
            int height = TEXTURE_SIZE / 10;
            int x = 0;
            int y = (int)(((float)(TEXTURE_SIZE - height)) * ((scale + 1.0f) / 2.0f));
            Rect<int> r = new(x, y, width, height);

            surface.TryFill(r, pixelFormatDetails.MapColor(null, 0, 255, 0));
            texture.UnsafeUnlock();
        }

        // as you can see from this, rendering draws over whatever was drawn before it. 
        mRenderer.DrawColor = new Color<byte>(66, 66, 66, Color.OpaqueAlphaByte); // grey, full alpha
        mRenderer.TryClear(); // start with a blank canvas.

        /* Just draw the static texture a few times. You can think of it like a
           stamp, there isn't a limit to the number of times you can draw with it. */

        // Center this one. It'll draw the latest version of the texture we drew while it was locked.
        float destX = ((float)(WINDOW_WIDTH - TEXTURE_SIZE)) / 2.0f;
        float destY = ((float)(WINDOW_HEIGHT - TEXTURE_SIZE)) / 2.0f;
        float destWidth = (float)TEXTURE_SIZE;
        float destHeight = (float)TEXTURE_SIZE;
        Rect<float> destinationRect = new(destX, destY, destWidth, destHeight);

        mRenderer.TryRenderTexture(destinationRect, texture);

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
        texture.Dispose();

        mRenderer?.Dispose();
        mRenderer = default!;

        mWindow?.Dispose();
        mWindow = default!;
    }
}