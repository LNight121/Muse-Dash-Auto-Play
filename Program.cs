// See https://aka.ms/new-console-template for more information
using Muse_Dash;
var songs = AutoPlay.Create("E:\\SteamLibrary\\steamapps\\common\\Muse Dash\\MuseDash_Data\\StreamingAssets\\aa\\StandaloneWindows64\\");
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
    foreach (var file in songs[sele])
    {
        Console.WriteLine($"{index}:{file.Name}");
        temp.Add(index++, file.FullName);
    }
    Console.WriteLine("选择难度");
    index = int.Parse(Console.ReadLine());
    var data = AutoPlay.Init(temp[index]);
    AutoPlay.Paly(data, new Byte[] { 68, 70 }, new Byte[] { 74, 75 });
    Console.WriteLine("输入n继续");
} while (Console.ReadLine() == "n");