using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muse_Dash
{
    public class AtriReader : BinaryReader
    {
        public AtriReader(Stream input) : base(input)
        {
        }
        public override decimal ReadDecimal()
        {
            var temp = new int[4];
            for (int i = 0; i < 4; i++)
            {
                temp[3 - i] = ReadInt32();
            }
            return new Decimal(temp);
        }
    }
}
