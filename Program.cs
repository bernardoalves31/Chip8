using System;
using SDL2;



class Program
{
    
        //Setting memory
        static byte[] mem = new byte[4096];

        //Settings registers
        // 8 bits
        static byte v0 = 0, v1 = 0, v2 = 0, v3 = 0, v4 = 0, v5 = 0, v6 = 0, v7 = 0, v8 = 0, v9 = 0, 
        va = 0, vb = 0, vc = 0, vd = 0, ve = 0, vf = 0;

        //16 bits
        static ushort I = 0, pc = 0x200, opcode = 0;

        //Setting screen
        static byte[] screen = new byte[64 * 32];

        //Stack and stack pointer
        static ushort[] stack = new ushort[16];
        static ushort sp;


    static void Main()
    {
        Console.WriteLine("Hello world");
        

        Console.WriteLine(pc);

    }





}