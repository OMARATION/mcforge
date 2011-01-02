using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Gaming.CTF
{
    public class CTFTeam : Team
    {
        public ushort[] flagBase = { 0, 0, 0 };
        public ushort[] flagLocation = { 0, 0, 0 };

        public bool flagishome;
        public bool spawnset;
        public bool flagmoved;

        public Player holdingFlag = null;
        public CatchPos tempFlagblock;
        public CatchPos tfb;
        public int ftcount = 0;

        public Level mapOn;

        public void Drawflag()
        {

            ushort x = flagLocation[0];
            ushort y = flagLocation[1];
            ushort z = flagLocation[2];

            if (mapOn.GetTile(x, (ushort)(y - 1), z) == Block.air)
            {
                flagLocation[1] = (ushort)(flagLocation[1] - 1);
            }

            mapOn.Blockchange(tfb.x, tfb.y, tfb.z, tfb.type);
            mapOn.Blockchange(tfb.x, (ushort)(tfb.y + 1), tfb.z, Block.air);
            mapOn.Blockchange(tfb.x, (ushort)(tfb.y + 2), tfb.z, Block.air);

            if (holdingFlag == null)
            {
                //DRAW ON GROUND SHIT HERE

                tfb.type = mapOn.GetTile(x, y, z);

                if (mapOn.GetTile(x, y, z) != Block.flagbase) { mapOn.Blockchange(x, y, z, Block.flagbase); }
                if (mapOn.GetTile(x, (ushort)(y + 1), z) != Block.mushroom) { mapOn.Blockchange(x, (ushort)(y + 1), z, Block.mushroom); }
                if (mapOn.GetTile(x, (ushort)(y + 2), z) != GetColorBlock(Color)) { mapOn.Blockchange(x, (ushort)(y + 2), z, GetColorBlock(Color)); }

                tfb.x = x;
                tfb.y = y;
                tfb.z = z;

            }
            else
            {
                //DRAW ON PLAYER HEAD
                x = (ushort)(holdingFlag.pos[0] / 32);
                y = (ushort)(holdingFlag.pos[1] / 32 + 3);
                z = (ushort)(holdingFlag.pos[2] / 32);

                if (tempFlagblock.x == x && tempFlagblock.y == y && tempFlagblock.z == z) { return; }


                mapOn.Blockchange(tempFlagblock.x, tempFlagblock.y, tempFlagblock.z, tempFlagblock.type);

                tempFlagblock.type = mapOn.GetTile(x, y, z);

                mapOn.Blockchange(x, y, z, GetColorBlock(Color));

                tempFlagblock.x = x;
                tempFlagblock.y = y;
                tempFlagblock.z = z;
            }


        }

        public static byte GetColorBlock(char color)
        {
            if (color == '2')
                return Block.green;
            if (color == '5')
                return Block.purple;
            if (color == '8')
                return Block.darkgrey;
            if (color == '9')
                return Block.blue;
            if (color == 'c')
                return Block.red;
            if (color == 'e')
                return Block.yellow;
            if (color == 'f')
                return Block.white;
            else
                return Block.air;
        }
    }
}
