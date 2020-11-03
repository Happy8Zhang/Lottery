# Lottery
3D抽奖程序 

![image](https://github.com/Happy8Zhang/Lottery/blob/master/showing.gif)

【开发者】

1 C#

2 基于.net core3.1开发的WPF程序；

【非开发者】
 
1 安装Desktop Runtime, 下载地址：https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.5-windows-x64-installer

2 运行：bin/netcoreapp3.1/Demo.Lottery.exe。

【软件功能】

1 运行软件前，请先进行相应的【配置】；

2 软件运行后，按[Enter]键开始抽奖，再按[Enter]键停止抽奖，同时显示中奖人相关的信息；

3 开始抽奖前，可以点击界面右上角的设置按钮，进行奖项（特等奖、一等奖、二等奖...）设置（此设置可以不设置）；

4 所有抽奖结束后，可以点击左侧导出按钮，将所有获奖者保存于桌面上《获奖名单.xlsx》的文件中。

_____________________________________________________________________________________________________________
【配置】

Images文件夹

1 Background文件夹存放的是背景图片；

2 Employee文件夹

2.1 员工信息表（员工.xlsx）：其内容是参与抽奖人员的基本信息（名字，部门，工号，公司）；                      【此表一定要有，不可以缺少】

2.2 每个参与抽奖人员的头像，其文件命名规则：名字-部门-工号，切记所写名字、部门、工号一定要是员工信息表中某个。 【头像不一定要有，原因见3】

3 default.png，此图片代替没有提供头像图片的参与抽奖人员，即：某个参与抽奖人员存在于员工信息表中，但是没有提供头像图片，软件会自动使用default.png代替。
