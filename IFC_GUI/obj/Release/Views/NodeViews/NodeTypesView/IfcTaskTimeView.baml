   M S B A M L   `   `   `  ���� B  >IFC_GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:IFC_GUI.Views.NodeViews.NodeTypesView%IFC_GUI.Views.NodeViews.NodeTypesView  ;    5IFC_GUI.Views.NodeViews.NodeTypesView.IfcTaskTimeView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Elocal3clr-namespace:IFC_GUI.Views.NodeViews.NodeTypesView     $	��500��$	��300��  � ForceCursor  . .��� 
������� � ColumnDefinitions ��  � RowDefinitions �� .+�� $��Center=�$��Center=����� *
Background+ �� $
��Auto ��� $��10 ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��Name:�� -    ��Name$ nameTextBox��$��2��$��0��$	��150��$��0,0,0,5q��� $��0��$��1��DataOrigin:�� -    ��Name$ dataOriginComboBox��$��2��$��1��$	��150��$��0,0,0,5q�.@��� .��
Measured�� .��	Predicted�� .��	Simulated�� .��User defined�� .��Not defined�� $��0��$��2��UserDefinedDataOrigin:�� -   $" userDefinedDataOriginTextBox��$��2��$��2��$��0,0,0,5q�$	��150���� $��0��$��3��DurationType:�� -   $ durationTypeComboBox��$��2��$��3��$��0,0,0,5q�$	��150��.@��� .��Elapsed time�� .��
Worktime�� .��Not defined�� $��0��$��4��ScheduleDuration:�� -   $ scheduleDurationTextBox��$��2��$��4��$��0,0,0,5q�$	��150���� $��0��$��5��ScheduleStart:�� -   $ scheduleStartTextBox��$��2��$��5��$��0,0,0,5q�$	��150���� $��0��$��6��ScheduleFinish:�� -   $ scheduleFinishTextBox��$��2��$��6��$��0,0,0,5q�$	��150���� $��0��$��7��EarlyStart:�� -   $ earlyStartTextBox��$��2��$��7��$��0,0,0,5q�$	��150���� $��0��$��8��EarlyFinish:�� -	   $ earlyFinishTextBox��$��2��$��8��$��0,0,0,5q�$	��150���� $��0��$��9��
LateStart:�� -
   $ lateStartTextBox��$��2��$��9��$��0,0,0,5q�$	��150���� $��0��$��10��LateFinish:�� -   $ lateFinishTextBox��$��2��$��10��$��0,0,0,5q�$	��150���� $��0��$��11��
FreeFloat:�� -   $ freeFloatTextBox��$��2��$��11��$��0,0,0,5q�$	��150���� $��0��$��12��TotalFloat:�� -   $ totalFloatTextBox��$��2��$��12��$��0,0,0,5q�$	��150���� $��0��$��13��IsCritical:�� -    ��Name$ isCriticalCheckBox��$��2��$��13��$��Center=�$��0,0,0,5q��� $��0��$��14��StatusTime:�� -   $ statusTimeTextBox��$��2��$��14��$��0,0,0,5q�$	��150���� $��0��$��15��ActualDuration:�� -   $ actualDurationTextBox��$��2��$��15��$��0,0,0,5q�$	��150���� $��0��$��16��ActualStart:�� -   $ actualStartTextBox��$��2��$��16��$��0,0,0,5q�$	��150���� $��0��$��17��ActualFinish:�� -   $ actualFinishTextBox��$��2��$��17��$��0,0,0,5q�$	��150���� $��0��$��18��RemainingTime:�� -   $ remainingTimeTextBox��$��2��$��18��$��0,0,0,5q�$	��150���� $��0��$��19��Completion:�� -   $ completionTextBox��$��2��$��19��$��0,0,0,5q�$	��150��� $��21��$��3��$��Right=�.+��� -    ��Name$ okButton��$��60��$��0,0,10,0q�.��_Ok�� -   $ cancelButton��$��60��.��	_Cancel