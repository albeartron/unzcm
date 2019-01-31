using System;
using System.IO;

namespace unzcm
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = null;

            // check if enough command line arguments
            if (args.Length != 1)
            {
                Console.WriteLine("Error: Invalid command line argument(s).");
                return;
            }

            // set file name from first command line argument
            fileName = (string)args.GetValue(0);
            fileName = System.IO.Path.GetFullPath(fileName);

            // check if file exists
            if (!System.IO.File.Exists(fileName))
            {
                Console.WriteLine("Error: File does not exist.");
                return;
            }

            // open the file
            BinaryReader inputFile = new BinaryReader(new FileStream(fileName, FileMode.Open));

            // structure:
            //  header:
            //      uint32  index
            //  texture:
            //      string  name
            //      uint32  type
            //      uint32  length
            //      sprite:
            //          bool    isSprite?
            //          string  spriteName
            //          uint32  locationX
            //          uint32  locationY
            //          uint32  sizeX
            //          uint32  sizeY
            //          float   originX
            //          float   originY
            //          depending on type:
            //              1: uint32 char_ref and uint32 flags
            //              2: uint32 flags
            //              4: uint32 flags
            //              default: nothing

            Int32 fileIndex;
            //byte stringLength;
            string textureName;
            Int32 textureType;
            Int32 textureLength;
            bool isSprite;
            string spriteName;
            Int32 spriteLocationX, spriteLocationY, spriteSizeX, spriteSizeY;
            float spriteOriginX, spriteOriginY;
            Int32 char_ref = 0;
            bool has_char_ref;
            Int32 flags = 0;
            bool has_flags;

            // column header
            Console.Write("totalTextures\t");
            Console.Write("fileIndex\t");
            Console.Write("textureName\t");
            Console.Write("textureType\t");
            Console.Write("textureLength\t");
            Console.Write("idx\t");
            Console.Write("isSprite\t");
            Console.Write("spriteName\t");
            Console.Write("spriteLocationX\t");
            Console.Write("spriteLocationY\t");
            Console.Write("spriteSizeX\t");
            Console.Write("spriteSizeY\t");
            Console.Write("spriteOriginX\t");
            Console.Write("spriteOriginY\t");
            Console.Write("char_ref\t");
            Console.Write("flags\n");


            // file header
            fileIndex = inputFile.ReadInt32();

            for (int i = 0; i < fileIndex; i++)
            {
                // texture header
                textureName = inputFile.ReadString();
                //Console.WriteLine(textureName);
                textureType = inputFile.ReadInt32();
                textureLength = inputFile.ReadInt32();

                for (int j = 0; j < textureLength; j++)
                {
                    // sprite entry, read values
                    isSprite = inputFile.ReadBoolean();
                    spriteName = inputFile.ReadString();
                    spriteLocationX = inputFile.ReadInt32();
                    spriteLocationY = inputFile.ReadInt32();
                    spriteSizeX = inputFile.ReadInt32();
                    spriteSizeY = inputFile.ReadInt32();
                    spriteOriginX = inputFile.ReadSingle();
                    spriteOriginY = inputFile.ReadSingle();

                    has_char_ref = false;
                    has_flags = false;
                    switch (textureType)
                    {
                        case 1:
                            char_ref = inputFile.ReadInt32();
                            has_char_ref = true;
                            goto case 2;
                        case 2:
                        case 4:
                            flags = inputFile.ReadInt32();
                            has_flags = true;
                            break;
                        default:
                            break;
                    }

                    // print values
                    Console.Write(fileIndex + "\t");
                    Console.Write(i + "\t");
                    Console.Write(textureName + "\t");
                    Console.Write(textureType + "\t");
                    Console.Write(textureLength + "\t");
                    Console.Write(j + "\t");
                    Console.Write(isSprite + "\t");
                    Console.Write(spriteName + "\t");
                    Console.Write(spriteLocationX + "\t");
                    Console.Write(spriteLocationY + "\t");
                    Console.Write(spriteSizeX + "\t");
                    Console.Write(spriteSizeY + "\t");
                    Console.Write(spriteOriginX + "\t");
                    Console.Write(spriteOriginY + "\t");
                    if (has_char_ref)
                    {
                        Console.Write(char_ref);
                    }
                    Console.Write("\t");
                    if (has_flags)
                    {
                        Console.Write(flags);
                    }
                    Console.Write("\n");
                }
            }

            // close the file
            inputFile.Close();
        }
    }
}
