using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muse_Dash
{
    public class MusicData:IComparable
    {
        public Int16 objId;
        public Decimal tick;
        public MusicConfigData configData;
        public MusicConfigData noteData;
        public bool isLongPressing;
        public Int32 doubleIdx;
        public bool isDouble;
        public bool isLongPressEnd;
        public Decimal longPressPTick;
        public Int32 endIndex;
        public Decimal dt;
        public Int32 longPressNum;
        public Decimal showTick;

        public int CompareTo(object? obj)
        {
            var o = (MusicData)obj;
            return tick.CompareTo(o.tick);
        }

        public void UnSerialze(Stream stream)
        {
            var s = new AtriReader(stream);
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            objId=s.ReadInt16();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            tick = s.ReadDecimal();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            if (s.ReadByte() == 0x2f)
            {
                s.ReadBytes(5);
                s.ReadBytes(s.ReadInt32() * 2);
            }
            else
            {
                s.ReadBytes(4);
            }
            configData = new MusicConfigData();
            configData.UnSerialze(s.BaseStream);
            if (s.ReadByte() == 3)
            {
                s.ReadBytes(5);
                noteData=new MusicConfigData();
                noteData.UnSerialze(s.BaseStream);
            }
            else
            {
                s.ReadByte();
            }
            s.ReadBytes(s.ReadInt32() * 2);
            isLongPressing = s.ReadBoolean();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            doubleIdx=s.ReadInt32();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            isDouble = s.ReadBoolean();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            isLongPressEnd =s.ReadBoolean();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            longPressPTick = s.ReadDecimal();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            endIndex = s.ReadInt32();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            dt=s.ReadDecimal();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            longPressNum=s.ReadInt32();
            s.ReadBytes(2);
            s.ReadBytes(s.ReadInt32() * 2);
            showTick = s.ReadDecimal();
            s.ReadByte();
        }
    }
}
