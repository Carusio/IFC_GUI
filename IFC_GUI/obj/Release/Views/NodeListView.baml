   M S B A M L   `   `   `  ���� B  >IFC_GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:IFC_GUI.ViewsIFC_GUI.Views  F BNodeNetwork, Version=5.2.0.0, Culture=neutral, PublicKeyToken=null\=clr-namespace:NodeNetwork.Views.Controls;assembly=NodeNetworkNodeNetwork.Views.Controls      IFC_GUI.Views.NodeListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:IFC_GUI.Views     Rcontrols=clr-namespace:NodeNetwork.Views.Controls;assembly=NodeNetwork       � 	Resources     tilesTemplate>  8NodeNetwork.Views.Controls.ViewModelViewHostNoAnimations-  'System.Windows.Input.KeyboardNavigation   TabNavigationJ FReactiveUI.Wpf, Version=13.2.0.0, Culture=neutral, PublicKeyToken=null"  ReactiveUI.ViewModelViewHost   	ViewModel � LayoutTransform �� ScaleX �� ScaleY � Cursor  tilesItemsPanelTemplate  tilesItemsControlTemplate  listTemplate �� 
TargetType �� Property	 �� Value
 �� Triggers P� Property P� Value (� Stretch �� Left
 �� Top  listItemsPanelTemplate  listItemsControlTemplate��%�  &	        &	 �     &	 �     &	 �     &	 >    &	 Y    �� .R�� .+�  $	��Top=�$��
0, 5, 0, 5q�$
 None=� ��  �� $	 0.5Q�$	 0.5Q�� -   ���	#01000000$
 Hand���� .R�� �� .R��� $��Stretch=��� .R�� -   $��Stretch=�$��
0, 0, 0, 0q����� " �.���  � ��
	 �����
 P�  �@J�IsMouseOver . . ���  � ��	 �#f7f7f7.+��� $��10,5,5,5q��� ��� *Name+(� $ Uniform=�$	��100��$��20��$��Right=�$��5q�$ SizeAll��.���� $
��12.5��$��30�����	#01ffffff.+��� $ 0��$ 0��$��4��$��4�����	#FF999999���	#FF999999�� $	 7.5��$ 0��$��4��$��4�����	#FF999999���	#FF999999�� $ 0��$	 7.5��$��4��$��4�����	#FF999999���	#FF999999�� $	 7.5��$	 7.5��$��4��$��4�����	#FF999999���	#FF999999�� $ 0��$ 15��$��4��$��4�����	#FF999999���	#FF999999�� $	 7.5��$ 15��$��4��$��4�����	#FF999999���	#FF999999�� $ 0��$
 22.5��$��4��$��4�����	#FF999999���	#FF999999�� $	 7.5��$
 22.5��$��4��$��4�����	#FF999999���	#FF999999�� .R��� $��Stretch=��� .R��� $��Stretch=�.���  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��0�� � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�� $��0��.+��� -    ��Name$ 
titleLabel��$��
5, 0, 0, 0q�$��Center=�$
��Left=�$��18(�$��Segoe UI Semilight)�$��Available nodes��� -    �Name$ searchBoxGrid��$��1��$	��500��.+��� -    ��Name$ 	searchBox�� �� TextWrapping$
 Wrap=�$��0,2,0,0q��� -   $ emptySearchBoxMessage��$��4,4,4,4q�$��	Search...�� J� IsHitTestVisible .  
�������� $��2��.+��� -    ��Name$ viewComboBox��$��Center=�$��Right=�$��80��$��20��� $��1��$��Stretch=�$��Stretch=�.+��� $
��Auto=�$��Visible=�.���� -    ��Name$ elementsList����.   �� 
GroupStyle (  "System.Windows.Controls.GroupStyle     ContainerStyle �� " ��.���  � ��	 �� .R�9�  9� 
IsExpanded . ���� *Name+.���� �� -	   $ emptyMessage��$��No matching nodes found��$��Center=�$��Center=�