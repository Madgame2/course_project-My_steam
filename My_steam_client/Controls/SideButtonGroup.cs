using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace My_steam_client.Controls
{
    public static class SideButtonGroup
    {
        private static readonly Dictionary<string, List<ToggleButton>> _groups = new();

        public static readonly DependencyProperty GroupNameProperty=
            DependencyProperty.RegisterAttached("GroupName",typeof(string),typeof(SideButtonGroup),new PropertyMetadata(null, OnGroupNameChanged));

        public static string GetGroupName(DependencyObject obj) => (string)obj.GetValue(GroupNameProperty);
        public static void SetGroupName(DependencyObject obj,string value)=>obj.SetValue(GroupNameProperty,value);


        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ToggleButton button) return;

            string newGroup = e.NewValue as string;
            if(string.IsNullOrEmpty(newGroup)) return;


            if(!_groups.TryGetValue(newGroup, out var list))
            {
                list = new List<ToggleButton>();
                _groups[newGroup] = list;
            }

            list .Add(button);

            button.Checked += (s, _) =>
            {
                foreach (var other in list)
                {
                    if (other != button)
                    {
                        other.IsChecked = false;
                    }
                }
            };
        }
    }
}
