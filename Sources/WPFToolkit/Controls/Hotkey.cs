using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 快捷键控件
    /// 快捷键设置规则：快捷键要使用控制按键开头，或者是一个单独的英文字母。英文字母后面不能出现控制按键
    /// 快捷键设置完毕判断策略：当输入英文字母的时候，就算设置完毕
    /// 
    /// 注意，这个控件继承自TextBox，因为之前发现过如果把该控件用于ListBox里，那么不会触发KeyDown事件。然而TextBox就可以触发KeyDown事件，所以就改成继承自TextBox
    /// 猜测是因为ListBoxItem给KeyDown事件做了特殊处理，设置了Handled=True
    /// </summary>
    public class Hotkey : TextBox
    {
        /// <summary>
        /// 按键类型
        /// </summary>
        private enum KeyTypes
        {
            /// <summary>
            /// 什么按键都没按
            /// </summary>
            None,

            /// <summary>
            /// 控制按键
            /// </summary>
            ControlKey,

            /// <summary>
            /// 26个英文字母
            /// </summary>
            CharacterKey
        }

        /// <summary>
        /// 最多有两个组合键
        /// </summary>
        public const int MAXIMUM_COMBINATION_KEYS = 3;

        #region 实例变量

        /// <summary>
        /// 上一次按下的按键
        /// </summary>
        private Key previouseKey = Key.None;

        /// <summary>
        /// 上一次按键的状态
        /// </summary>
        private KeyStates previouseKeyState = KeyStates.None;

        /// <summary>
        /// 最后一次记录的按键类型
        /// </summary>
        private KeyTypes previouseKeyType = KeyTypes.None;

        /// <summary>
        /// 标识下一次按键的时候是否需要重置
        /// </summary>
        private bool reset;

        #endregion

        #region 依赖属性

        /// <summary>
        /// 快捷键的组合键个数
        /// </summary>
        public int MaximumCombinations
        {
            get { return (int)GetValue(MaximumCombinationsProperty); }
            set { SetValue(MaximumCombinationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumHotkeys.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumCombinationsProperty =
            DependencyProperty.Register("MaximumCombinations", typeof(int), typeof(Hotkey), new PropertyMetadata(MAXIMUM_COMBINATION_KEYS));


        public string HotkeyText
        {
            get { return (string)GetValue(HotkeyTextProperty); }
            set { SetValue(HotkeyTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HotkeyText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HotkeyTextProperty =
            DependencyProperty.Register("HotkeyText", typeof(string), typeof(Hotkey), new PropertyMetadata(string.Empty));

        public List<Key> Hotkeys
        {
            get { return (List<Key>)this.GetValue(HotkeysProperty); }
            set { this.SetValue(HotkeysProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hotkeys.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HotkeysProperty =
            DependencyProperty.Register("Hotkeys", typeof(IEnumerable<Key>), typeof(Hotkey), new PropertyMetadata(null, OnHotkeysPropertyChangedCallback));

        #endregion

        #region 构造方法

        public Hotkey()
        {
        }

        #endregion

        #region 重写事件

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            Key pressedKey = this.GetPressedKey(e);

            // 首先判断按下的按键是否可以作为快捷键使用
            if (!this.IsKeySupported(pressedKey))
            {
                return;
            }

            if(this.reset)
            {
                this.Reset();
                this.reset = false;
            }

            // 解决会多次触发KeyDown事件的问题
            if (pressedKey == this.previouseKey && e.KeyStates == this.previouseKeyState)
            {
                return;
            }

            this.previouseKey = pressedKey;
            this.previouseKeyState = e.KeyStates;

            // 按下的是Control，Alt，Shift其中的一个按键
            if (this.IsControlKey(pressedKey))
            {
                // 超过了组合键个数
                if (this.Hotkeys.Count == this.MaximumCombinations)
                {
                    return;
                }

                // 上次按的是字符按键，重新设置
                if (this.previouseKeyType == KeyTypes.CharacterKey)
                {
                    this.Reset();
                }

                this.previouseKeyType = KeyTypes.ControlKey;
            }

            // 按下的是26个英文字母其中的一个
            if (this.IsCharacterKey(pressedKey))
            {
                if (this.previouseKeyType == KeyTypes.CharacterKey)
                {
                    // 如果上一个按键是英文字母，那么把英文字母替换掉
                    this.Hotkeys.RemoveAt(this.Hotkeys.Count - 1);
                }

                this.previouseKeyType = KeyTypes.CharacterKey;
            }

            //Console.WriteLine("添加Key = {0}", e.SystemKey);

            this.Hotkeys.Add(pressedKey);

            // 更新快捷键文本
            this.UpdateHotkeyText();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (this.IsControlKey(this.GetPressedKey(e)))
            {
                // 设置完了
                this.reset = true;
            }
        }

        #endregion

        #region 依赖属性回调

        private void Hotkeys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        break;
                    }

                case NotifyCollectionChangedAction.Remove:
                    {
                        break;
                    }

                case NotifyCollectionChangedAction.Reset:
                    {
                        break;
                    }

                default:
                    break;
            }
        }

        private void OnHotkeysPropertyChanged(IEnumerable<Key> newValue, IEnumerable<Key> oldValue)
        {
            if (newValue is INotifyCollectionChanged)
            {
                (newValue as INotifyCollectionChanged).CollectionChanged += this.Hotkeys_CollectionChanged;
            }

            if (oldValue is INotifyCollectionChanged)
            {
                (oldValue as INotifyCollectionChanged).CollectionChanged -= this.Hotkeys_CollectionChanged;
            }
        }

        /// <summary>
        /// 依赖属性回调
        /// </summary>
        /// <param name="d">触发回调的对象</param>
        /// <param name="e">回调参数</param>        
        private static void OnHotkeysPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Hotkey).OnHotkeysPropertyChanged(e.NewValue as IEnumerable<Key>, e.OldValue as IEnumerable<Key>);
        }

        #endregion

        private void Reset()
        {
            this.previouseKey = Key.None;
            this.previouseKeyState = KeyStates.None;
            this.previouseKeyType = KeyTypes.None;
            this.Hotkeys.Clear();
        }

        /// <summary>
        /// 更新快捷键文本
        /// </summary>
        /// <param name="hotKeys"></param>
        private void UpdateHotkeyText()
        {
            string text = string.Empty;
            int numKeys = this.Hotkeys.Count;

            for (int i = 0; i < numKeys; i++)
            {
                if (i != numKeys - 1)
                {
                    text += string.Format("{0} + ", this.Hotkeys[i]);
                }
                else
                {
                    text += string.Format("{0}", this.Hotkeys[i]);
                }
            }

            this.HotkeyText = text;
        }

        /// <summary>
        /// 判断按键是否为控制键（ctrl, shift, alt）
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool IsControlKey(Key k)
        {
            if (k >= Key.LeftShift && k <= Key.RightAlt)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断是否是字符
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool IsCharacterKey(Key k)
        {
            return k >= Key.A && k <= Key.Z;
        }

        /// <summary>
        /// 判断按键是否可以作为快捷键使用
        /// </summary>
        /// <returns></returns>
        private bool IsKeySupported(Key k)
        {
            if (this.IsControlKey(k))
            {
                return true;
            }

            if (this.IsCharacterKey(k))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前按下的按键
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Key GetPressedKey(KeyEventArgs e)
        {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}