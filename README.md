# 说明
`重要：此程序基于.net开发，想要运行必须先安装.NET Framework 4.5 或以上版本`

    这是一个使用C♯开发的程序
    此程序用于修改怪物猎人：世界中装饰品（珠子）的数量，可以无中生有
    此程序只在pc最新版（167898）上通过测试，其它版本不保证能用
    目前只成功开发了功能，并无UI，以后可能会制作UI
    使用了外部json（Decorations.json）来确定一些参数，方便修改
    暂时只能读取第一个存档（其实读取其它存档的功能已经做好了，懒着写进去）
    暂不支持冰原（废话）
    冰原pc发布之后此程序大概率会作废，等我肝爽了再来更新这个程序

## 以下是对Decorations.json中参数的一些说明
`警告：如果你看不懂这个文件，千万不要做出任何改动，任何改动都会导致不可预期的后果`

    ProcessName：程序在任务管理器中的名称，用于获取程序pid
    Signature：程序在内存中加载存档，会产生一段独一无二的特征码，因为猛汉王可以拥有三个存档，此特征码会出现三个
    Archive：存档在内存中的一些信息，以第一个存档为例子
        firstScanAddress：从此地址开始扫描
        lastScanAddress：终止与此地址
        interval：扫描间隔
        subtraction：珠子在存档内存中站的长度
        特别说明：每个存档储存珠子的地址最后四位必定相同，例如存档1的地址最后四位必定为F648

    Code：珠子的代码，目前一共有98颗珠子
    Names：珠子代码对应名称

# 简单教程：
`警告：使用前请先解压全部内容，否则无法运行`

[视频教程](https://www.bilibili.com/video/av70022048/ "转到哔哩哔哩")

    文字教程：

    先打开游戏，耐心等待游戏加载完成，可以不进入任何存档
    然后打开修改器，修改器会自动对内存进行扫描，并且显示出你所拥有的珠子，每个珠子前都有序号
    此时程序会暂停，问你输入你想要修改的序号，输入然后按下回车即可
    然后输入珠子代码，代码可以从Decorations.json的Names条目中查看，前面引号内的内容就是珠子代码，后面是珠子名称，不包括引号
    回车后输入你需要修改的数字，别输入负数！
    回车，修改完成！

    注意：显示珠子名称为空代表这个位置没有珠子，你可以随意更改，但是如果已经显示有珠子，修改需谨慎

    注意2：如果你在空的位置修改一个已经在其他位置有的珠子，珠子不会合并，但是这无伤大雅

    
# 更新日志
    2019年10月21日更新：由于游戏更新，特征码有所改动，修复json文件中的特征码以确保修改器仍然可以使用
