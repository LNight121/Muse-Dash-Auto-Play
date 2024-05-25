# Muse Dash Auto Play（梦游少年计划）
傻瓜式操作，只要你第一个键打准了就能全连大部分歌，AP部分歌。如果某首歌一直搞不定自己去改代码。

因为我只是为了Stream成就，所以我没仔细研究铺面文件结构，有些铺子没法正常使用是正常的。

## 注意
由于更新了文件结构导致我没搞明白连打的逻辑，现在单个连打最长为0.5秒，如果出现长度大于0.5s的还请按几下键盘帮一下。

因为我懒得动relese，还请参照[编译本程序](#编译本程序)自行编译

首次使用前自行配置Setings.json。
默认键位为:
- 上 DFUI
- 下 JKER

默认路径为`"E:\\SteamLibrary\\steamapps\\common\\Muse Dash"`

[使用例子](https://www.bilibili.com/video/BV1EH4y167cL)
## 编译本程序
首次编译后缺少的AssetBundleOperationNative.dll请自行去我的Release中复制。
## 注意
此程序仅开源一部分，禁止使用AssetBundleOperationNative.dll做任何其他的事情,此dll中部分敏感函数已被注释，无法使用。