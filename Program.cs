// See https://aka.ms/new-console-template for more information
using Muse_Dash;
using Newtonsoft.Json;
using System.Text;

Console.WriteLine("Mues Dash Auto Play \nV1.0.0 Original");
Console.WriteLine("Wait for three seconds...");
#if !DEBUG
Thread.Sleep(3000);
#endif
string json = File.ReadAllText("Setings.json");
var set = JsonConvert.DeserializeObject<Setings>(json);
var songs = AutoPlay.Create($"{set.Path}\\MuseDash_Data\\StreamingAssets\\aa\\StandaloneWindows64\\");
do
{
    var temp = new Dictionary<int, string>();
    int index = 0;
    foreach (var song in songs.Keys)
    {
        Console.WriteLine($"{index}:{song}");
        temp.Add(index++, song);
    }
    Console.WriteLine("选择歌曲");
    index = int.Parse(Console.ReadLine());
    string sele = temp[index];
    temp.Clear();
    index = 0;
    if (songs[sele].Count > 1)
    {
        Console.WriteLine("出现同名歌曲，请选择");
        foreach (var file in songs[sele])
        {
            Console.WriteLine($"{index}:{file.Name}");
            temp.Add(index++, file.FullName);
        }
        index = int.Parse(Console.ReadLine());
        sele = temp[index];
    }
    else
    {
        sele = songs[sele][0].FullName;
    }
    var data = AutoPlay.Init(sele);
    AutoPlay.Paly(data, Encoding.ASCII.GetBytes(set.Up), Encoding.ASCII.GetBytes(set.Down));
    Console.WriteLine("输入n继续");
} while (Console.ReadLine() == "n");