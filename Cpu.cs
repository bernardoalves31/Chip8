public class Cpu
{
    //Setting memory
    public byte[] mem = new byte[4096];

    //Settings registers
    // 8 bits
    public byte[] v = new byte[16];

    //16 bits
    public ushort I = 0, pc = 0x200, opcode = 0;

    //Setting screen
    public byte[] screen = new byte[64 * 32];

    //Stack and stack pointer
    public ushort[] stack = new ushort[16];
    public ushort sp;

    //Delay timer
    public byte delayTimer;

    //Keys
    public byte[] keys = new byte[16];

    public Random rnd = new Random();

    public void SetOpcode()
    {
        //Decode opcode moving 2 different bytes in 1 ushort 
        opcode = (ushort)(mem[pc] << 8 | mem[pc + 1]);

        switch (opcode & 0xF000)
        {
            case 0x0000:
                if ((opcode & 0x000F) == 0x0000) //00E0
                {
                    for (int i = 0; i < mem.Length; i++)
                    {
                        mem[i] = 0;
                    }
                    break;

                }

                //00EE
                --sp;
                pc = stack[sp];
                pc += 2;
                break;

            case 0x1000:
                //1NNN
                pc = (ushort)(opcode & 0x0FFF);
                break;

            case 0x2000:
                //2NNN
                stack[sp] = pc;
                ++sp;
                pc = (ushort)(opcode & 0x0FFF);
                break;

            case 0x3000:
                //3XNN
                if (v[(opcode & 0x0F00) >> 8] == (opcode & 0x00FF))
                {
                    pc += 2;
                }
                pc += 2;
                break;

            case 0x4000:
                //4XNN
                if (v[(opcode & 0x0F00) >> 8] != (opcode & 0x00FF))
                {
                    pc += 2;
                }
                pc += 2;
                break;

            case 0x5000:
                //5XY0
                if (v[(opcode & 0x0F00) >> 8] == v[(opcode & 0x00F0) >> 4])
                {
                    pc += 2;
                }
                pc += 2;
                break;

            case 0x6000:
                //6XNN
                v[(opcode & 0x0F00) >> 8] = (byte)(opcode & 0x00FF);
                pc += 2;
                break;

            case 0x7000:
                //7XNN
                v[(opcode & 0x0F00) >> 8] += (byte)(opcode & 0x00FF);
                break;

            case 0x8000:
                switch (opcode & 0x000F)
                {
                    case 0x0000:
                        //8XY0 
                        v[(opcode & 0x0F00) >> 8] = v[(opcode & 0x00F0) >> 4];
                        pc += 2;
                        break;

                    case 0x0001:
                        //8XY1
                        v[(opcode & 0x0F00) >> 8] |= v[(opcode & 0x00F0) >> 4];
                        break;

                    case 0x0002:
                        //8XY2
                        v[(opcode & 0x0F00) >> 8] &= v[(opcode & 0x00F0) >> 4];
                        break;

                    case 0x0003:
                        //8XY3
                        v[(opcode & 0x0F00) >> 8] ^= v[(opcode & 0x00F0) >> 4];
                        break;

                    case 0x0004:
                        //8XY4
                        if ((ushort)(v[(opcode & 0x0F00) >> 8] + v[(opcode & 0x00F0) >> 4]) > byte.MaxValue) //Has overflow
                        {
                            v[15] = 1;
                        }
                        else
                        {
                            v[15] = 0;
                        }
                        v[(opcode & 0x0F00) >> 8] += v[(opcode & 0x00F0) >> 4];
                        pc += 2;
                        break;


                    case 0x0005:
                        //8XY5
                        if ((ushort)(v[(opcode & 0x0F00) >> 8] - v[(opcode & 0x00F0) >> 4]) < byte.MinValue) //Has underflow
                        {
                            v[15] = 0;
                        }
                        else
                        {
                            v[15] = 1;
                        }
                        v[(opcode & 0x0F00) >> 8] -= v[(opcode & 0x00F0) >> 4];
                        pc += 2;
                        break;

                    case 0x0006:
                        //8XY6
                        v[15] = (byte)(v[(opcode & 0x0F00) >> 8] & 0x01);
                        v[(opcode & 0x0F00) >> 8] >>= 1;
                        break;

                    case 0x0007:
                        //8XY7
                        if ((ushort)(v[(opcode & 0X0F0) >> 4] - v[(opcode & 0X0F00) >> 8]) < byte.MinValue)
                        {
                            v[15] = 0;
                        }
                        else
                        {
                            v[15] = 1;
                        }
                        v[(opcode & 0X0F00) >> 8] = (byte)(v[(opcode & 0X0F0) >> 4] - v[(opcode & 0X0F00) >> 8]);
                        pc += 2;
                        break;

                    case 0x000E:
                        //8XYE
                        v[15] = (byte)((v[(opcode & 0x0F00) >> 8] & 0x80) >> 7);
                        v[(opcode & 0x0F00) >> 8] <<= 1;
                        pc += 2;
                        break;

                    default:
                        Console.WriteLine("Unknown upcode");
                        break;
                }
                break;

            case 0x9000:
                //9XY0
                if (v[(opcode & 0X0F00) >> 8] != v[(opcode & 0X00F0) >> 4])
                {
                    pc += 2;
                }
                pc += 2;
                break;

            case 0xA000:
                //ANNN
                I = (ushort)(opcode & 0x0FFF);
                pc += 2;
                break;

            case 0xB000:
                //BNNN
                pc = (ushort)(v[0] + (opcode & 0x0FFF));
                break;

            case 0xC000:
                //CXNN
                v[(opcode & 0x0F00) >> 8] = (byte)(rnd.Next(0, 255) & 0x00FF);
                pc += 2;
                break;

            case 0xD000:
                //DXYN
                ushort vx = v[(opcode & 0x0F00) >> 8];
                ushort vy = v[(opcode & 0x00F00) >> 4];
                ushort n = (ushort)(opcode & 0x000F);

                v[15] = 0;

                for (int i = 0; i < n; i++)
                {
                    byte pixel = mem[I + i];

                    for (int j = 0; j < 8; j++)
                    {
                        if ((pixel & 0x80) >> j != 0)
                        {
                            if (screen[vx + i + (vy + j) * 64] == 1)
                            {
                                v[15] = 1;
                            }
                            screen[vx + j + (vy + i) * 64] ^= pixel;
                        }
                    }

                }

                pc += 2;
                break;

            case 0xE000:
                //EX9E
                ushort x = v[(opcode & 0x0F00) >> 8];
                if (keys[x] != 0)
                {
                    pc += 4;
                    break;
                }

                //EXA1
                if (keys[x] == 0)
                {
                    pc += 4;
                    break;
                }

                pc += 2;
                break;

            case 0xF000:
                switch (opcode & 0x00FF)
                {
                    case 0x0007:
                        //FX07
                        v[(opcode & 0x0F00) >> 8] = delayTimer;
                        pc += 2;
                        break;

                    case 0x000A:
                        //FX0A
                        bool pressed = false;
                        for (int i = 0; i < 16; i++)
                        {
                            if(keys[i] == 1)
                            {
                                pressed = true;
                                v[(opcode & 0x0F00) >> 8] = (byte)i;
                            }
                        }

                        if(!pressed)
                        {
                            break;
                        }
                        
                        pc += 2;
                        break;

                    case 0x0015:
                        //FX15
                        delayTimer = v[(opcode & 0x0F00) >> 8];
                        pc += 2;
                        break;

                    case 0x0018:
                        //FX18
                        pc += 2;
                        break;

                    case 0x001E:
                        //FX1E
                        I += v[(opcode & 0x0F00) >> 8];
                        pc += 2;
                        break;

                    case 0x0029:
                        //FX29
                        I = (ushort)(v[(opcode & 0x0F00) >> 8] * 5);
                        I *= 5;
                        pc += 2;
                        break;

                    case 0x0033:
                        //FX33
                        byte wx = v[(opcode & 0x0F00) >> 8];
                        mem[I] = (byte)(wx / 100);
                        mem[I + 1] = (byte)((wx % 100) / 10);
                        mem[I + 2] = (byte)(wx % 10);
                        pc += 2;
                        break;
                    case 0x0055:
                        //fX55
                        for (int i = 0; i <= ((opcode & 0x0F00) >> 8); i++)
                        {
                            mem[I + i] = v[i];
                        }
                        pc += 2;
                        break;
                    case 0x0065:
                        //FX65
                        for (int i = 0; i <= ((opcode & 0x0F00) >> 8); i++)
                        {
                            v[i] = mem[I + i];
                        }
                        break;


                    default:
                        Console.WriteLine("Unknown upcode");
                        break;
                }
                break;

            default:
                Console.WriteLine("Unknown upcode");
                break;
        }

        if(delayTimer > 0)
            --delayTimer;

    }
}