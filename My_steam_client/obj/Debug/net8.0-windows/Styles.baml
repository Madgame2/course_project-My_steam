   M S B A M L   `   `   `  ����J  FMy_steam_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:My_steam_client.TemplatesMy_steam_client.Templates  5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089(    "My_steam_client.Templates.Showcase  �� 
TargetType   ShowCase_styles �� Property �� Value&    My_steam_client.Templates.Slider  Slider_Styles0   *My_steam_client.Templates.SimpleSideButton  SimapleSideButtonTemplate �� BasedOn �� Triggers �� Binding �� RelativeSource �� AncestorType �� Value	 N� EnterActions
 �� TargetProperty	 W� To g� Duration N� ExitActions*  $My_steam_client.Templates.SideButton  SideButtonTemplate'   !My_steam_client.Templates.SideBar  SideBar_styles T� X P� Property P� Value  Main_buttons3  -System.Windows.Media.Effects.DropShadowEffect  NoShadow   ShadowDepth   
BlurRadius   Opacity�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      1local'clr-namespace:My_steam_client.Templates  5      %F  &	        &	     '  �    7  '  J    7  &	 �
    &	 L    &	 �    �� 5      "    6   .�5   
   ��  �@  ElementHeight$	 150Q�6*   �� 5   
    �@  ElementMargin$ 0,5,0,5q�6*   �� 5	   
    �@  BorderRadius$ 10��6)   �� 5
   
   ! �@  ShowCaseElementBackGround$ 	#FF111111��66   5      �� 5      "   6   .�5   
   ��  �@ IndicatorsActiveBrush
 ����62   �� 5   
    �@ IndicatorsInactiveBrush
 �����64   5      �� 5      "   6   8   .�5   
   ��  �@ SvgStrokeColor �#A0A0A06+   �� 5   
    � �� �	#FF1414146'   �� 5   
    �@ IconTranslateX$	 135Q�6+   �� 5   
    �@ HoverTextBrush �	#FF426FFF6+   �� 5   
    �@ HoverImageCollor �	#FF426FFF6-   �� 5   
    �@ ActiveTextColor �	#FF426FFF6,   �� 5   
    �@ ActiveImageColor �	#FF426FFF6-   �� 5   
    �@ ActiveBacground �	#FF3F3F3F6,   �� 5   
    �@ TitleFontSize$ 18Q�6*   �� 5   
    �@ TitleFontWeight$
 Bold"�6,   �� 5   
    �@ TitleFontFamily$6 0pack://application:,,,/resources/Fonts/#Rajdhani)�6,   �� 5   
    � �� �#A0A0A06'    5!   
   �� 5"       6   �� *IsMouseOver+ ��  UserControl True6s   	 5#      �� 5$      .��5%      �� .��5&      W� $-
 '(local:SimpleSideButton.IconTranslateX)�5'   !    -155(   !   $ 0:0:0.2D�5)   !   5*      5+       5.      �� 5/      .��50      �� .��51      W� $-
 '(local:SimpleSideButton.IconTranslateX)�52   !    13553   !   $ 0:0:0.2D�54   !   55      56      58      5:      �� 5>      "   6   8   .�5?   
   ��  �@ SvgStrokeColor �#A0A0A06+   �� 5@   
    � �� �	#FF1414146'   �� 5A   
    �@ IconTranslateX$	 135Q�6+   �� 5B   
    �@ HoverTextBrush �	#FF426FFF6+   �� 5C   
    �@ HoverImageCollor �	#FF426FFF6-   �� 5D   
    �@ ActiveTextColor �	#FF426FFF6,   �� 5E   
    �@ ActiveImageColor �	#FF426FFF6-   �� 5F   
    �@ ActiveBacground �	#FF3F3F3F6,   �� 5G   
    �@ TitleFontSize$ 18Q�6*   �� 5H   
    �@ TitleFontWeight$
 Bold"�6,   �� 5I   
    �@ TitleFontFamily$6 0pack://application:,,,/resources/Fonts/#Rajdhani)�6,   �� 5J   
    � �� �#A0A0A06'    5M   
   �� 5N       6   �� *IsMouseOver+ ��  local:SideBar True6u   	 5O      �� 5P      .��5Q      �� .��5R      W� $'
 !(local:SideButton.IconTranslateX)�5S   !    -155T   !   $ 0:0:0.2D�5U   !   5V      5W       5Z      �� 5[      .��5\      �� .��5]      W� $'
 !(local:SideButton.IconTranslateX)�5^   !    1355_   !   $ 0:0:0.2D�5`   !   5a      5b      5d      5f      �� 5h      "   6   .�5k   
   ��  � z� 5l      T� 5m      $
 -200Q�6%   5o      �� 5p   
    � ��$ 0,0,0,0q�6$    5r   
   P� 5s       �@J�IsMouseOver$
 True��6-   	 5u      �� 5v      .��5w      �� .��5x      W� $8
 2(UIElement.RenderTransform).(TranslateTransform.X)�5y       -505z      $ 0:0:0.2D�5{      5|      5}      5~       5�      �� 5�      .��5�      �� .��5�      W� $8
 2(UIElement.RenderTransform).(TranslateTransform.X)�5�       -2005�      $ 0:0:0.2D�5�      5�      5�      5�      5�      5�      �� 5�      "  ��6!   .�5�   
   ��  � ��$ Center=�6.   �� 5�   
    � ��$ 	0,0,0,100q�6#   5�        5�      $ 0Q�6(   $ 0Q�68   $ 0Q�6G   5�      