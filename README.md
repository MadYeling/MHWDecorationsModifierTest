# 说明
`重要：此程序基于.net开发，想要运行必须先安装.NET Framework 4.5 或以上版本`

    这是一个使用C♯开发的程序
    此程序用于修改《怪物猎人：世界》中装饰品（珠子）的数量，可以无中生有
    此程序只在pc最新版上通过测试，其它版本不保证能用
    使用了外部json（Decorations.json）来确定一些参数，方便修改

## 以下是对Decorations.json中参数的一些说明
`警告：如果你看不懂这个文件，千万不要做出任何改动，任何改动都会导致不可预期的后果`

    ProcessName：程序在任务管理器中的名称，用于获取程序pid
    Signature：程序在内存中加载存档，会产生一段独一无二的特征码
    interval：扫描间隔
    subtraction：储存珠子的部分在存档内存中占的长度
    name：玩家名称地址与珠子首地址差
    Archive：存档在内存中的一些信息，以第一个存档为例子
        firstScanAddress：从此地址开始扫描
        lastScanAddress：终止与此地址
    Decorations：目前已知的珠子代码对应的名称

# 简单教程：
`警告：使用前请先解压全部内容，否则无法运行`
    
    首先需要安装 Performance Booster and Plugin Extender
    先打开游戏，耐心等待游戏加载完成，可以不进入任何存档
    然后打开修改器，修改器会自动对内存进行扫描，并且显示出你所拥有的珠子，每个珠子前都有序号
    此时程序会暂停，问你输入你想要修改的序号，输入然后按下回车即可
    然后输入珠子名称
    回车后输入你需要修改的数字，别输入负数！
    回车，修改完成！

    注意：显示珠子名称为空代表这个位置没有珠子，你可以随意更改，但是如果已经显示有珠子，修改需谨慎
    如果你在空的位置修改一个已经在其他位置有的珠子，珠子不会合并，但是这无伤大雅

    
# 更新日志
    2019年10月21日更新：由于游戏更新，特征码有所改动，修复json文件中的特征码以确保修改器仍然可以使用
    2019年10月22日更新：UI部分读取珠子信息已经基本完成，修改部分仍需努力
    2019年11月06日更新：游戏更新会导致特征码变动，修改特征码逻辑，去除会变动的部分，使修改器理论上能在其他版本的mhw上正常运行
    2019年11月07日更新：添加按名称搜索珠子逻辑，可以不输入珠子代码啦！
    2020年12月03日更新：支持冰原！但是目前只能修改第一个存档的珠子，并且游戏更新后不能继续使用。
    2023年07月21日更新：支持目前的最新版本