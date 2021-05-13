# WPFToolkit
WPF开发用到的各种通用工具类

## Controls
通用WPF自定义控件

重写了很多WPF的原生控件，增强了原生控件的功能，自带主题，可动态切换主题，比WPF原生控件更好看，可以直接用于产品开发。
同时也使控件更易于修改Style，在不重写模板的情况下，让用户可以最大化修改控件样式的各个细节。
为了和原生的控件进行区分，WPFToolKit里的所有的控件以'K'开头，比如'KButton','KComboBox'。

KControls定义了多套主题和皮肤，开发者可以很方便的在多套主题之间进行切换，详情请参见Demo

目前KControls支持的皮肤有：
* MAC OSX
* Windows10
* element-ui
* layui

## Business.Controls
开发这套控件的目的是为了可以让项目快速开发成型，这套控件封装了一些项目中常用的控件，比如菜单导航，树形列表等等。
使用这套控件，开发人员不用再关注XAML的编写，只需要提供相应的数据和简单的设置一些控件属性就可以了

## MVVM
一套简易的MVVM框架，该框架完美的实现了ViewModel和View之间的分离
Interactive模块有部分代码参考了DevExpress的源码

# DragDrop

一整通用的WPF拖拽框架，支持对TreeView和ListBox等ItemsControl的拖拽功能。


