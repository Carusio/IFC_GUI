   M S B A M L   `   `   `  ����B  >IFC_GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:IFC_GUIIFC_GUI  5   	   F BNodeNetwork, Version=5.2.0.0, Culture=neutral, PublicKeyToken=nullJ4clr-namespace:NodeNetwork.Views;assembly=NodeNetworkNodeNetwork.Views 5   	   -clr-namespace:IFC_GUI.ViewsIFC_GUI.Views  5	   	   ]3clr-namespace:IFC_GUI.Views.NodeViews.NodeTypesView%IFC_GUI.Views.NodeViews.NodeTypesView  5
   	   "    IFC_GUI.Views.MainWindowView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:IFC_GUI     5   	   Lnodenetwork4clr-namespace:NodeNetwork.Views;assembly=NodeNetwork     5   	   -viewsclr-namespace:IFC_GUI.Views     5	   	   MviewsTaskTime3clr-namespace:IFC_GUI.Views.NodeViews.NodeTypesView     5
   	     � Title$  Ifc Node Editor��5   	   $	��600��6!   $	��900��6.    � 	Resources 5         nodeMenu m�Name��%m   &	      �� 5   
   .@�5      m� $ showSubNetworkButton��6   $��Show subnetwork��65   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   $	��100��6*    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��100 �6   .+�5    
      IFC_GUI.Views.MenuBarView  -   5          Name$ menuBarView��5       $��0��61   $��2��6>   %   IFC_GUI.Views.BreadcrumbBarView  5"   
   -   5          Name$ breadcrumbBarView��5"   "   $��1��6=   $��0��6J   $��30��6Z   �� 5$   
   -   5        $��autoLayoutButton��5$   i   $��Stretch=�6   $��Stretch=�6/   $��1��6K   $��1��6X   .��6�   Auto-Layout6�   #  NodeNetwork.Views.NetworkView  5&   
   -   5          Name$ networkView��5&   "   $��2��67   $��2��6D    5'         NodeNetwork.Views.NodeView �� 
TargetType	 �� Property
 �� Value��%^   '        7   �� 5(      "  6   .�5)      �� 	 �@�ContextMenu8
   64   5*      5+      5,      �� 50   
   -   5         ��Name$ ContentControlPopup��50      $��2��66   6E       IFC_GUI.Views.NodeListView  52   
   -   5          Name$ nodeListView��52      $��3��63   $��2��6@   55      58      