   M S B A M L   `   `   `  ����J  FMy_steam_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:My_steam_client.TemplatesMy_steam_client.Templates  5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089(    "My_steam_client.Templates.Showcase  �� 
TargetType   ShowCase_styles �� Property �� Value&    My_steam_client.Templates.Slider  Slider_Styles*  $My_steam_client.Templates.SideButton  SideButtonTemplate �� BasedOn �� Triggers �� Binding �� RelativeSource �� AncestorType �� Value	 N� EnterActions
 �� TargetProperty	 W� To g� Duration N� ExitActions'   !My_steam_client.Templates.SideBar  SideBar_styles T� X P� Property P� Value  Main_buttons3  -System.Windows.Media.Effects.DropShadowEffect  NoShadow   ShadowDepth   
BlurRadius   Opacity�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      1local'clr-namespace:My_steam_client.Templates  5      %�	  &	        &	     '  �    7  &	 @    &	 �    &	 	    �� 5      "    6   .�5   
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
   �� 5"       6   �� *IsMouseOver+ ��  local:SideBar True6u   	 5#      �� 5$      .��5%      �� .��5&      W� $'
 !(local:SideButton.IconTranslateX)�5'   !    -155(   !   $ 0:0:0.2D�5)   !   5*      5+       5.      �� 5/      .��50      �� .��51      W� $'
 !(local:SideButton.IconTranslateX)�52   !    13553   !   $ 0:0:0.2D�54   !   55      56      58      5:      �� 5<      "   6   .�5?   
   ��  � z� 5@      T� 5A      $
 -200Q�6%   5C      �� 5D   
    � ��$ 0,0,0,0q�6$    5F   
   P� 5G       �@J�IsMouseOver$
 True��6-   	 5I      �� 5J      .��5K      �� .��5L      W� $8
 2(UIElement.RenderTransform).(TranslateTransform.X)�5M       -505N      $ 0:0:0.2D�5O      5P      5Q      5R       5V      �� 5W      .��5X      �� .��5Y      W� $8
 2(UIElement.RenderTransform).(TranslateTransform.X)�5Z       -2005[      $ 0:0:0.2D�5\      5]      5^      5_      5a      5c      �� 5e      "  ��6!   .�5f   
   ��  � ��$ Center=�6.   �� 5g   
    � ��$ 	0,0,0,100q�6#   5h        5j      $ 0Q�6(   $ 0Q�68   $ 0Q�6G   5k      