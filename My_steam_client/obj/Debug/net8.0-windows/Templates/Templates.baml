   M S B A M L   `   `   `  ����J  FMy_steam_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:My_steam_client.TemplatesMy_steam_client.Templates  5      G(clr-namespace:My_steam_client.ConvertersMy_steam_client.Converters  5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0895    /My_steam_client.Converters.ImageSourceConverter   UniversalImageConverter  SideButtonTemplate  �� 
TargetType*  $My_steam_client.Templates.SideButton �� Property �� Value �� 
TargetType � ColumnDefinitions K� ZIndex ��Name   	IconWidth   
IconHeight	 `� 	Converter
 �� Path �� RelativeSource �� AncestorType T� X T� Y�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      1local'clr-namespace:My_steam_client.Templates  5      1conv(clr-namespace:My_steam_client.Converters  5      %�  &	        &	      7      5      6    �� 5      "   6'   .�5	   
   ��  � �� 5
      �� 5      "  5      .R�5      �� #��z��6   ._�5      � $
��Left=�5   !   $��Center=�5   !   $��60,0,0,0q�5       5      �� 5   "   $
��Auto �63   �� 5   "   $
��Auto �63   .+�5      �� #��z��5   !   #��z��5   !   $ 1��5   !   ._�5   "   �� $ 	IconImage��6(   #��z 5   %   #��z 5   %   $��Center=�5   %   $��0��5   %   ��5   &   `� 5    *   8	   .1�5!   .   �� $

 Icon�66    6B   ��  local:SideButton�� 5"   .   $
 SvgStrokeColor�66    6L   ��  local:SideButton5#   +   5%   #   z�5'   "   T� 5(   &    69   �� *IconTranslateX+ �� *TemplatedParent+ 5)   9   �� *IconTranslateY+ �� *TemplatedParent+5+      �� 5,      #��z��6/   $��Center=�5-   +   $��Center=�5.   +   $��1��5/   +   50      51      52      54      55      57      