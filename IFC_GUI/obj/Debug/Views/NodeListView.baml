   M S B A M L   `   `   `  ����B  >IFC_GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:IFC_GUI.ViewsIFC_GUI.Views  5      F BNodeNetwork, Version=5.2.0.0, Culture=neutral, PublicKeyToken=null\=clr-namespace:NodeNetwork.Views.Controls;assembly=NodeNetworkNodeNetwork.Views.Controls 5           IFC_GUI.Views.NodeListView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:IFC_GUI.Views     5      Rcontrols=clr-namespace:NodeNetwork.Views.Controls;assembly=NodeNetwork     5        � 	Resources  5         tilesTemplate>  8NodeNetwork.Views.Controls.ViewModelViewHostNoAnimations-  'System.Windows.Input.KeyboardNavigation   TabNavigationJ FReactiveUI.Wpf, Version=13.2.0.0, Culture=neutral, PublicKeyToken=null"  ReactiveUI.ViewModelViewHost   	ViewModel � LayoutTransform �� ScaleX �� ScaleY � Cursor  tilesItemsPanelTemplate  tilesItemsControlTemplate  listTemplate �� 
TargetType �� Property	 �� Value
 �� Triggers P� Property P� Value (� Stretch �� Left
 �� Top  listItemsPanelTemplate  listItemsControlTemplate��%�  &	        &	     &	 0    &	 k    &	     &	 @    �� 5   
   .R�5      � .+�5        $	��Top=�69   $��
0, 5, 0, 5q�6Q   $
 None=�6{    6e   ��  5      �� 5      $	 0.5Q�6)   $	 0.5Q�66   5      � 5      -   5        ���	#01000000$
 Hand��5   J   5      5      �� 5   
   .R�5      � 5      �� 5   
   .R�5      �� $��Stretch=�6   5      �� 5!   
   .R�5#      � -   5        $��Stretch=�5#      $��
0, 0, 0, 0q�61   ��5$      �� 5%      " �6   .�5&      ��  � ��
	 �����67   
 5'      P� 5(       �@J�IsMouseOver . . �5)   "   ��  � ��	 �#f7f7f76?   5*      5,      .+�5/      �� $��10,5,5,5q�6   �� 6/   �63   �� *Name+6L   (� 51      $ Uniform=�6   $	��100��6,   $��20��68   $��Right=�6D   $��5q�6`   $ SizeAll��6k   .��52      �� $
��12.5��6   $��30��6*   ���	#01ffffff.+�53      �� $ 0��6$   $ 0��64   $��4��6C   $��4��6M   ���	#FF999999���	#FF9999996i   �� 54      $	 7.5��6$   $ 0��66   $��4��6E   $��4��6O   ���	#FF999999���	#FF9999996k   �� 55      $ 0��6$   $	 7.5��64   $��4��6E   $��4��6O   ���	#FF999999���	#FF9999996k   �� 56      $	 7.5��6$   $	 7.5��66   $��4��6G   $��4��6Q   ���	#FF999999���	#FF9999996m   �� 57      $ 0��6$   $ 15��64   $��4��6D   $��4��6N   ���	#FF999999���	#FF9999996j   �� 58      $	 7.5��6$   $ 15��66   $��4��6F   $��4��6P   ���	#FF999999���	#FF9999996l   �� 59      $ 0��6$   $
 22.5��64   $��4��6F   $��4��6P   ���	#FF999999���	#FF9999996l   �� 5:      $	 7.5��6$   $
 22.5��66   $��4��6H   $��4��6R   ���	#FF999999���	#FF9999996n   5;      5<      5=      5>      �� 5@   
   .R�5A      �� $��Stretch=�6   5B      �� 5C   
   .R�5D      �� $��Stretch=�6   5E      5F      .��5H      �  � RowDefinitions 5I   
   �� 5J      $
��Auto �6   6,   �� 5K      $��* �6   6)   .+�5M   
   � $��0��6    � ColumnDefinitions 5N      �� 5O      $
��Auto �6#   �� 5P      $��* �6#   �� 5Q      $
��Auto �6#   .+�5U      � $��0��6   .+�5V      �� -   5         ��Name$ 
titleLabel��5V      $��
5, 0, 0, 0q�60   $��Center=�6D   $
��Left=�6_   $��18(�6z   $��Segoe UI Semilight)�6�   $��Available nodes��6�   5W      � 5X      -   5         �Name$ searchBoxGrid��5X   #   $��1��6   $	��500��6:   .+�5Y      �� -   5         ��Name$ 	searchBox��5Y       �� TextWrapping$
 Wrap=�6-   $��0,2,0,0q�6A   �� 5Z      -   5        $ emptySearchBoxMessage��5Z      $��4,4,4,4q�6;   $��	Search...��6L    J� IsHitTestVisible .  
�������6v   5[      � 5\      $��2��6   .+�5]      �� -   5         ��Name$ viewComboBox��5]      $��Center=�61   $��Right=�6L   $��80��6h   $��20��6s   5^      5_      � 5b   
   $��1��6   $��Stretch=�6   $��Stretch=�63   .+�5c      �� $
��Auto=�6   $��Visible=�6>   .��5d      �� -   5         ��Name$ elementsList��5d      ��.   �� 
GroupStyle 5e      (  "System.Windows.Controls.GroupStyle  5f         ContainerStyle 5g      �� 5h   "   " ��6(   .�5i   &   ��  � ��	 5j   *   �� 5k   .   .R�5l   2   9�  9� 
IsExpanded . ��6;   �� *Name+.��5m   6   �� 5n   3   5o   /   5q   '   5r   #   5t      5v      5w      �� 5x      -	   5        $ emptyMessage��5x      $��No matching nodes found��6.   $��Center=�6M   $��Center=�6j   5y      5z      5{      