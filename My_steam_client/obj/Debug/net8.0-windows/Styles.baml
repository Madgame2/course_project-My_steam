   M S B A M L   `   `   `  ����J  FMy_steam_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:My_steam_client.TemplatesMy_steam_client.Templates  5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089*   $My_steam_client.Templates.SideButton  �� 
TargetType   SideButtonTemplate �� BasedOn �� Property �� Value �� Triggers �� Binding �� RelativeSource �� AncestorType �� Value	 N� EnterActions
 �� TargetProperty	 W� To g� Duration N� ExitActions'   !My_steam_client.Templates.SideBar  SideBar_styles T� X P� Property P� Value  Main_buttons�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      1local'clr-namespace:My_steam_client.Templates  5      %;  '         7   &	 >    &	 �    �� 5      "    6   8   .�5   
   ��  � ��
 �����6'   �� 5   
    �@  IconTranslateX$	 135Q�6+    5	   
   �� 5
       6   �� *IsMouseOver+ ��  local:SideBar True6u   	 5      �� 5      .��5      �� .��5      W� $'
 !(local:SideButton.IconTranslateX)�5   !    -155   !   $ 0:0:0.2D�5   !   5      5       5      �� 5      .��5      �� .��5      W� $'
 !(local:SideButton.IconTranslateX)�5   !    1355   !   $ 0:0:0.2D�5   !   5      5      5       5"      �� 5$      "   6   .�5'   
   ��  � z� 5(      T� 5)      $
 -200Q�6%   5+      �� 5,   
    � ��$ 0,0,0,0q�6$    5.   
   P� 5/       �@J�IsMouseOver$
 True��6-   	 51      �� 52      .��53      �� .��54      W� $8
 2(UIElement.RenderTransform).(TranslateTransform.X)�55       -5056      $ 0:0:0.2D�57      58      59      5:       5>      �� 5?      .��5@      �� .��5A      W� $8
 2(UIElement.RenderTransform).(TranslateTransform.X)�5B       -2005C      $ 0:0:0.2D�5D      5E      5F      5G      5I      5K      �� 5M      "  ��6!   .�5N   
   ��  � ��$ Center=�6.   �� 5O   
    � ��$ 	0,0,0,100q�6#   5P      5R      