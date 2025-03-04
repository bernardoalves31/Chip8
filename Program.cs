using System;
using SDL2;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello world");
        
        Cpu cpu = new Cpu();

        Console.WriteLine(cpu.delayTimer);


        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.WriteLine("SDL failed");
            return;
        }

        IntPtr window = SDL.SDL_CreateWindow("Chip-8", 0, 0, 640, 320, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

        SDL.SDL_Event sdl_event;
        SDL.SDL_SetRenderDrawColor(window, 0, 0, 0, 0);
        bool open = true;

        while (open)
        {
            while (SDL.SDL_PollEvent(out sdl_event) != 0)
            {
                if (sdl_event.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    open = false;
                }
            }
        }





    }

}