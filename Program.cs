using System;
using SDL2;

class Program
{
    static private Cpu cpu = new();

    static void Main()
    {
        LoadGame("br8kout.ch8");
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

                if (sdl_event.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    SetKeyDown(SDL.SDL_GetKeyName(sdl_event.key.keysym.sym));
                }

                if (sdl_event.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    SetKeyUp(SDL.SDL_GetKeyName(sdl_event.key.keysym.sym));
                }

            }
        }


        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();

    }

    static void LoadGame(String file)
    {
        FileStream fileStream = new FileStream(file, FileMode.Open); 
        BinaryReader binaryReader = new BinaryReader(fileStream);
        int i = 0;
        try{
            while(fileStream.CanRead)
            {
                cpu.mem[512 + i] = binaryReader.ReadByte();
                Console.WriteLine(cpu.mem[512 + i]);
                i++;
            }

        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Game loaded");
        }
    }


    static void SetKeyDown(string key)
    {
        switch (key)
        {
            case "1":
                cpu.keys[0x01] = 1;
                break;

            case "2":
                cpu.keys[0x02] = 1;
                break;

            case "3":
                cpu.keys[0x03] = 1;
                break;

            case "4":
                cpu.keys[0x0C] = 1;
                break;

            case "Q":
                cpu.keys[0x04] = 1;
                break;

            case "W":
                cpu.keys[0x05] = 1;
                break;

            case "E":
                cpu.keys[0x06] = 1;
                break;

            case "R":
                cpu.keys[0x0D] = 1;
                break;

            case "A":
                cpu.keys[0x07] = 1;
                break;

            case "S":
                cpu.keys[0x08] = 1;
                break;

            case "D":
                cpu.keys[0x09] = 1;
                break;

            case "F":
                cpu.keys[0x0E] = 1;
                break;

            case "Z":
                cpu.keys[0x0A] = 1;
                break;

            case "X":
                cpu.keys[0x00] = 1;
                break;

            case "C":
                cpu.keys[0x0B] = 1;
                break;

            case "V":
                cpu.keys[0x0F] = 1;
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
                cpu.keys[0x01] = 0;
                break;

            case "2":
                cpu.keys[0x02] = 0;
                break;

            case "3":
                cpu.keys[0x03] = 0;
                break;

            case "4":
                cpu.keys[0x0C] = 0;
                break;

            case "Q":
                cpu.keys[0x04] = 0;
                break;

            case "W":
                cpu.keys[0x05] = 0;
                break;

            case "E":
                cpu.keys[0x06] = 0;
                break;

            case "R":
                cpu.keys[0x0D] = 0;
                break;

            case "A":
                cpu.keys[0x07] = 0;
                break;

            case "S":
                cpu.keys[0x08] = 0;
                break;

            case "D":
                cpu.keys[0x09] = 0;
                break;

            case "F":
                cpu.keys[0x0E] = 0;
                break;

            case "Z":
                cpu.keys[0x0A] = 0;
                break;

            case "X":
                cpu.keys[0x00] = 0;
                break;

            case "C":
                cpu.keys[0x0B] = 0;
                break;

            case "V":
                cpu.keys[0x0F] = 0;
                break;

            default:
                break;
        }
    }



}