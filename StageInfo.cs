using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muse_Dash
{
    public class StageInfo
    {
        public List<MusicData> musicDatas;
        public Decimal delay;
        public float bpm;
        public void UnSerialze(Stream stream)
        {
            var s = new BinaryReader(stream);
            s.ReadBytes(2);
            int length = s.ReadInt32();
            s.ReadBytes(length * 2 + 4);
            s.ReadBytes(2);
            length = s.ReadInt32();
            s.ReadBytes(length * 2 + 5);
            var l=s.ReadInt64();
            s.ReadBytes(7);
            length = s.ReadInt32();
            s.ReadBytes(length * 2);
            musicDatas = new List<MusicData>();
            for (int i = 0; i < l; i++)
            {
                var d = new MusicData();
                d.UnSerialze(s.BaseStream);
                s.ReadBytes(6);
                musicDatas.Add(d);
            }
        }
    }
}
