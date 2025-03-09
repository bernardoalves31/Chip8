using System;
using SDL2;

class Program
{
    static private Cpu cpu = new();

    static void Main()
    {
        LoadFontSet();
        LoadGame("TETRIS");
        EmulateCycle();


    }

    static void EmulateCycle()
    {


        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.WriteLine("SDL failed");
            return;
        }

        IntPtr window = SDL.SDL_CreateWindow("Chip-8", 0, 0, 640, 320, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

        IntPtr renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

        SDL.SDL_Event sdl_event;
        SDL.SDL_SetRenderDrawColor(window, 0, 0, 0, 0);
        bool open = true;

        uint frameStart;
        int frameTime;
        int frame_delay = 1000 / 60;

        while (open)
        {
            frameStart = SDL.SDL_GetTicks();

            SDL.SDL_PollEvent(out sdl_event);

            if (sdl_event.type == SDL.SDL_EventType.SDL_QUIT)
            {
                open = false;
            }

            if (sdl_event.type == SDL.SDL_EventType.SDL_KEYDOWN)
            {
                SetKeyDown(SDL.SDL_GetKeyName(sdl_event.key.keysym.sym));
            }

            if (sdl_event.type == SDL.SDL_EventType.SDL_KEYUP)
            {
                SetKeyUp(SDL.SDL_GetKeyName(sdl_event.key.keysym.sym));
            }
            cpu.SetOpcode();
            Draw(renderer);

            frameTime = (int)(SDL.SDL_GetTicks() - frameStart);
            if (frame_delay > frameTime)
            {
                SDL.SDL_Delay((uint)(frame_delay - frameTime));
            }


        }

        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();

    }

    static void LoadGame(String file)
    {
        FileStream fileStream = new FileStream(file, FileMode.Open);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        int i = 0;
        try
        {
            while (fileStream.CanRead)
            {
                cpu.mem[512 + i] = binaryReader.ReadByte();
                //    Console.WriteLine(cpu.mem[512 + i]);
                i++;
            }

        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Game loaded");
        }
    }

    static void LoadFontSet()
    {
        for (int i = 0; i < 80; i++)
        {
            cpu.mem[i] = cpu.fontSet[i];
            //  Console.WriteLine(cpu.fontSet[i]);
        }
    }

    static void Draw(IntPtr renderer)
    {

        if (cpu.draw)
        {
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(renderer);

            SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

            SDL.SDL_Rect rect = new SDL.SDL_Rect();

            rect.w = 10;
            rect.h = 10;

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (cpu.screen[(y * 64) + x] == 1)
                    {
                        rect.x = x * 10;
                        rect.y = y * 10;

                        SDL.SDL_RenderFillRect(renderer, ref rect);
                    }
                }
            }

        }
        SDL.SDL_RenderPresent(renderer);
        cpu.draw = false;
    }

    static void SetKeyDown(string key)
    {
        switch (key)
        {
            case "1":
                cpu.keys[0x1] = 1;
                break;

            case "2":
                cpu.keys[0x2] = 1;
                break;

            case "3":
                cpu.keys[0x3] = 1;
                break;

            case "4":
                cpu.keys[0xC] = 1;
                break;

            case "Q":
                cpu.keys[0x4] = 1;
                break;

            case "W":
                cpu.keys[0x5] = 1;
                break;

            case "E":
                cpu.keys[0x6] = 1;
                break;

            case "R":
                cpu.keys[0xD] = 1;
                break;

            case "A":
                cpu.keys[0x7] = 1;
                break;

            case "S":
                cpu.keys[0x8] = 1;
                break;

            case "D":
                cpu.keys[0x9] = 1;
                break;

            case "F":
                cpu.keys[0xE] = 1;
                break;

            case "Z":
                cpu.keys[0xA] = 1;
                break;

            case "X":
                cpu.keys[0x0] = 1;
                break;

            case "C":
                cpu.keys[0xB] = 1;
                break;

            case "V":
                cpu.keys[0xF] = 1;
                break;

            default:
                break;
        }
    }

    static void SetKeyUp(string key)
    {
        
        switch (key)
        {
             case "1":
                cpu.keys[0x1] = 0;
                break;

            case "2":
                cpu.keys[0x2] = 0;
                break;

            case "3":
                cpu.keys[0x3] = 0;
                break;

            case "4":
                cpu.keys[0xC] = 0;
                break;

            case "Q":
                cpu.keys[0x4] = 0;
                break;

            case "W":
                cpu.keys[0x5] = 0;
                break;

            case "E":
                cpu.keys[0x6] = 0;
                break;

            case "R":
                cpu.keys[0xD] = 0;
                break;

            case "A":
                cpu.keys[0x7] = 0;
                break;

            case "S":
                cpu.keys[0x8] = 0;
                break;

            case "D":
                cpu.keys[0x9] = 0;
                break;

            case "F":
                cpu.keys[0xE] = 0;
                break;

            case "Z":
                cpu.keys[0xA] = 0;
                break;

            case "X":
                cpu.keys[0x0] = 0;
                break;

            case "C":
                cpu.keys[0xB] = 0;
                break;

            case "V":
                cpu.keys[0xF] = 0;
                break;

            default:
                break;
        }
    }



}