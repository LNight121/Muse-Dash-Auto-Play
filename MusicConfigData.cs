using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muse_Dash
{
    public class MusicConfigData
    {
        public Int32 id;
        public Decimal time;
        public String note_uid;
        public Decimal length;
        public bool blood;
        public Int32 pathway;
        public void UnSerialze(Stream stream)
        {
            var s = new AtriReader(stream);
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            id=s.ReadInt32();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            time = s.ReadDecimal();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            note_uid = "";
            if (s.ReadByte() == 1)
            {
                int length = s.ReadInt32();
                for (int i = 0; i < length; i++)
                {
                    note_uid += (char)s.ReadInt16();
                }
                s.ReadBytes(2);
            }
            else
            {
                s.ReadBytes(1);
            }
            s.ReadBytes(s.ReadInt32() * 2);
            length = s.ReadDecimal();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            blood = s.ReadBoolean();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            pathway=s.ReadInt32();
            s.ReadByte();
        }
        public override string ToString()
        {
            return note_uid;
        }
    }
}
