using AssetBundleOperation.CSharpUseNative;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Muse_Dash
{
    public enum KeyType
    {
        None = 0,
        On=0x1,
        Off=0x2,
        Tap=0x4,
        Keep=0x8,
        Soft=0x10,
    }
    public static class AutoPlay
    {
        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
        public static void Paly(StageInfo datas,byte[] up,byte[] down)
        {
            long frequency;
            QueryPerformanceFrequency(out frequency);
            List<(decimal, byte, KeyType)> values = new List<(decimal, byte, KeyType)>();
            decimal last = 0;
            bool atri001 = false;
            datas.musicDatas.Sort();
            datas.musicDatas.ForEach(e =>
            {
                if (e.dt == 0)
                {
                    if(!atri001)
                    {
                        last = e.tick;
                        atri001 = true;
                    }
                    else
                    {
                        last += e.tick - e.showTick;
                    }
                }
                else
                {
                    e.tick -= last;
                }
            });
            last = 0;
            decimal atri = 4294960832;
            int a=0;
            int b=0;
            var iiii=datas.musicDatas.Where(e => e.configData.length != 0).ToArray();
            Dictionary<int,decimal> keyValuePairs = new Dictionary<int,decimal>();
            foreach (var data in datas.musicDatas)
            {
                if (data.dt != 0)
                {
                    if (!data.isLongPressing && !data.isLongPressEnd & data.configData.length != 0)
                    {
                        if(data.configData.note_uid.Substring(2, 2) == "02" || data.endIndex != 0)//长按
                        {
                            values.Add((data.tick, data.configData.pathway == 1 ? up[(up.Length + 1 - a++) % up.Length] : down[(down.Length + 1 - b++) % down.Length], KeyType.On));
                            //last = data.tick;
                        }
                        else
                        {
                            values.Add((data.tick, data.configData.pathway == 1 ? up[1] : down[1], KeyType.On | KeyType.Keep));//连打
                            if (data.configData.length > 640)
                            {
                                values.Add((data.configData.length + data.tick, data.configData.pathway == 1 ? up[1] : down[1], KeyType.Off | KeyType.Keep));
                            }
                            else
                            {
                                values.Add((data.configData.length * atri + data.tick, data.configData.pathway == 1 ? up[1] : down[1], KeyType.Off | KeyType.Keep));
                            }
                            //last = data.tick + data.configData.length * atri;
                        }
                    }
                    else if (data.isLongPressing)//历史遗留问题
                    {
                        if (!keyValuePairs.TryAdd(data.endIndex, data.tick - last))
                        {
                            keyValuePairs[data.endIndex] = data.tick - last;
                        }
                    }
                    else if (data.isLongPressEnd)//长按的结尾，似乎我这写错了，应该是模仿连打的写法而不是使用data.tick
                    {
                        values.Add((data.tick, data.configData.pathway == 1 ? up[(up.Length + 1 - --a) % up.Length] : down[(down.Length + 1 - --b) % down.Length], KeyType.Off));
                        //last = data.tick;
                    }
                    else if (data.isDouble)//双压，其实可以不列出来
                    {
                        values.Add((data.tick, data.configData.pathway == 1 ? up[0] : down[0], KeyType.Tap));
                        //last = data.tick;
                    }
                    else
                    {
                        if (!data.configData.note_uid.StartsWith("00") && (data.configData.note_uid.Substring(2,2) == "03" || data.configData.note_uid.Substring(2, 2) == "09"))//齿轮
                        {
                            values.Add((data.tick, data.configData.pathway == 1 ? down[0] : up[0], KeyType.Soft | KeyType.Tap));
                            //last = data.tick;
                        }
                        else
                        {
                            if (data.configData.note_uid.StartsWith("00"))//蓝色音符
                            {
                                values.Add((data.tick, data.configData.pathway == 1 ? up[0] : down[0], KeyType.Tap | KeyType.Soft));
                            }
                            else//其他的任何note
                            {
                                values.Add((data.tick, data.configData.pathway == 1 ? up[0] : down[0], KeyType.Tap));
                            }
                            //last = data.tick;
                            if (data.configData.time < 1000)
                            {
                                atri = data.tick / data.configData.time;
                            }
                        }
                    }
                }
            }
            last = frequency / atri;
            values.Sort(delegate ((Decimal, byte, KeyType) a, (Decimal, byte, KeyType) b)
            {
                return a.Item1.CompareTo(b.Item1);
            });
            for (int j = 0; j < values.Count; j++)//这个for用于移除齿轮导致的多余的点击
            {
                if ((values[j].Item3 & KeyType.Soft) != 0)
                {
                    decimal sum = atri / 10 + values[j].Item1;
                    bool flag = false;
                    for (int k = j + 1; k < values.Count; k++)
                    {
                        if (sum > values[k].Item1)
                        {
                            if ((values[k].Item3 & KeyType.Soft) == 0)
                            {
                                if (up.Contains(values[k].Item2) ^ up.Contains(values[j].Item2))
                                {
                                    flag = false;
                                }
                                else
                                {
                                    flag = true;
                                }
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(flag)
                    {
                        var atri00 = values[j];
                        values.RemoveAt(j);
                        values.Insert(j, (atri00.Item1, atri00.Item2, KeyType.None));
                    }
                    else
                    {
                        var atri00 = values[j];
                        values.RemoveAt(j);
                        values.Insert(j, (atri00.Item1 + atri / 50, atri00.Item2, (KeyType)(atri00.Item3 - KeyType.Soft)));//防止齿轮接蓝色音符，然后点太快撞到齿轮
                    }
                }
            }
            int p = 0;
            var key = values.ToArray();
            Console.Write("请按任意键继续...");
            Console.ReadKey();
            if(WindowHelper.SetWindowTop("MuseDash"))
            {
                long start;
                QueryPerformanceCounter(out start);
                long now;
                decimal offset = key[0].Item1;
                QueryPerformanceCounter(out now);
                int flag = 0;
                Do(key[p++], ref flag);
                int count = 0;
                while (p < key.Length)
                {
                    while (p < key.Length && (key[p].Item1 - offset) * last - now + start < ((long)(0.01 * frequency)))
                    {
                        Do(key[p++], ref flag);
                        QueryPerformanceCounter(out now);
                    }
                    Thread.Sleep(1);
                    count++;
                    if (flag > 0)
                    {
                        KeyboardSimulator.KeyPress(key[p - 1].Item2);
                        Thread.Sleep(5);
                        KeyboardSimulator.KeyRelese(key[p - 1].Item2);
                    }
                    QueryPerformanceCounter(out now);
                }
            }
        }
        public static void Do((Decimal,byte,KeyType) keyType,ref int flag)
        {
            if ((keyType.Item3 & KeyType.Keep) != 0)
            {
                if ((keyType.Item3 & KeyType.On) != 0)
                {
                    flag++;
                    KeyboardSimulator.KeyPress(keyType.Item2);
                    Thread.Sleep(5);
                    KeyboardSimulator.KeyRelese(keyType.Item2);
                }
                else if ((keyType.Item3 & KeyType.Off) != 0)
                {
                    flag--;
                }
                Console.WriteLine(flag);//其实这个flag可以换bool
                return;
            }
            if ((keyType.Item3 & KeyType.Tap) != 0)
            {
                KeyboardSimulator.KeyPress(keyType.Item2);
                Thread.Sleep(5);
                KeyboardSimulator.KeyRelese(keyType.Item2);
            }
            else if ((keyType.Item3 & KeyType.On) != 0)
            {
                KeyboardSimulator.KeyPress(keyType.Item2);
            }
            else if ((keyType.Item3 & KeyType.Off) != 0)
            {
                KeyboardSimulator.KeyRelese(keyType.Item2);
            }
        }
        public static StageInfo Init(string path)
        {
            long fileid = 0;
            IntPtr bundle = Native.LoadAssetBundleByPath(path,false,ref fileid);
            fileid = long.MinValue;
            fileid = Native.ObjectInfoYield(bundle, fileid);
            while (fileid != long.MinValue)
            {
                IntPtr map=Native.GetUnSerializeValue(bundle, fileid, "bpm");
                if(map != IntPtr.Zero)
                {
                    var result=new StageInfo();
                    result.bpm = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(map));
                    map = Native.GetUnSerializeValue(bundle, fileid, "serializationData.SerializedBytes.Array");
                    Byte[] res= new Byte[Marshal.ReadInt32(map)];
                    Marshal.Copy(map + 4, res, 0, res.Length);
                    Native.UnLoadAssetBundle(bundle);
                    result.UnSerialze(new MemoryStream(res));
                    return result;
                }
                fileid = Native.ObjectInfoYield(bundle, fileid);
            }
            return null;
        }
        public static Dictionary<string,List<FileInfo>> Create(string path)
        {
            var dict = new Dictionary<string, List<FileInfo>>();
            var d = new DirectoryInfo(path);
            var dd = d.GetFiles("*.bundle").Where(e => e.Name.StartsWith("noteasset_assets_")).ToArray();
            Dictionary<string,string> temp = new Dictionary<string, string>();
            foreach(var f in dd)
            {
                var s = f.Name.Substring("noteasset_assets_".Length);
                s = s.Substring(0, s.LastIndexOf("map") - 1);
                if (temp.ContainsKey(s))
                {
                    dict[temp[s]].Add(f);
                }
                else
                {
                    string ss = "";
                    long fileid = 0;
                    IntPtr bundle = Native.LoadAssetBundleByPath(f.FullName, false, ref fileid);
                    fileid = long.MinValue;
                    fileid = Native.ObjectInfoYield(bundle, fileid);
                    while (fileid != long.MinValue)
                    {
                        IntPtr map = Native.GetUnSerializeValue(bundle, fileid, "music.Array");
                        if (map != IntPtr.Zero)
                        {
                            ss = Marshal.PtrToStringAnsi(map + 4, Marshal.ReadInt32(map));
                            Native.UnLoadAssetBundle(bundle);
                            break;
                        }
                        fileid = Native.ObjectInfoYield(bundle, fileid);
                    }
                    temp.Add(s, ss);
                    if(!dict.TryAdd(ss, new List<FileInfo>() { f }))
                    {
                        dict[ss].Add(f);
                    }
                }
            }
            return dict;
        }
    }
}
