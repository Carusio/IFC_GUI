   M S B A M L   `   `   `  ���� B  >IFC_GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:IFC_GUI.Views.NodeViews.NodeTypesView%IFC_GUI.Views.NodeViews.NodeTypesView  5    /IFC_GUI.Views.NodeViews.NodeTypesView.TaskTimeV   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Elocal3clr-namespace:IFC_GUI.Views.NodeViews.NodeTypesView       � Title$  TaskTime properties��
�������$	��500��$	��555�� � ForceCursor .  � WindowStartupLocation$ CenterOwner=�.��� 
������� � ColumnDefinitions ��  � RowDefinitions �� .+�� $��Center=�$��Center=����� *
Background+ �� $
��Auto ��� $	��150 ��� $
��Auto � �� $��25 ��� $
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
��Auto �.+��� $��0��$��0��$��15(�$��Center=�$	��Top=�$��Medium"�$��	Underline{�	Attribute�� $��1��$��0��$��15(�$��Center=�$	��Top=�$��Medium"�$��	Underline{�Format�� $��2��$��0��$��15(�$��Center=�$	��Top=�$��Medium"�$��	Underline{�Value�� $��0��$��1��Name:�� -    ��Name$ nameTextBox��$��2��$��1��$	��200��$��0,0,0,5q��� $��0��$��2��DataOrigin:�� -    ��Name$ dataOriginComboBox��$��2��$��2��$	��150��$��0,0,0,5q�.@��� .��
Measured�� .��	Predicted�� .��	Simulated�� .��User defined�� .��Not defined�� $��0��$��3��UserDefinedDataOrigin:�� -   $" userDefinedDataOriginTextBox��$��2��$��3��$��0,0,0,5q�$	��200���� $��0��$��4��DurationType:�� -   $ durationTypeComboBox��$��2��$��4��$��0,0,0,5q�$	��200��.@��� .��Elapsed time�� .��
Worktime�� .��Not defined�� $��0��$��5��ScheduleDuration:�� $��1��$��5��$��Center=�PnYnMnDTnHnMnS�� -   $ scheduleDurationTextBox��$��2��$��5��$��0,0,0,5q�$	��200���� $��0��$��6��ScheduleStart:�� $��1��$��6��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ scheduleStartTextBox��$��2��$��6��$��0,0,0,5q�$	��200���� $��0��$��7��ScheduleFinish:�� $��1��$��7��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ scheduleFinishTextBox��$��2��$��7��$��0,0,0,5q�$	��200���� $��0��$��8��EarlyStart:�� $��1��$��8��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ earlyStartTextBox��$��2��$��8��$��0,0,0,5q�$	��200���� $��0��$��9��EarlyFinish:�� $��1��$��9��$��Center=�YYYY-MM-DDThh:mm:ss�� -	   $ earlyFinishTextBox��$��2��$��9��$��0,0,0,5q�$	��200���� $��0��$��10��
LateStart:�� $��1��$��10��$��Center=�YYYY-MM-DDThh:mm:ss�� -
   $ lateStartTextBox��$��2��$��10��$��0,0,0,5q�$	��200���� $��0��$��11��LateFinish:�� $��1��$��11��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ lateFinishTextBox��$��2��$��11��$��0,0,0,5q�$	��200���� $��0��$��12��
FreeFloat:�� $��1��$��12��$��Center=�PnYnMnDTnHnMnS�� -   $ freeFloatTextBox��$��2��$��12��$��0,0,0,5q�$	��200���� $��0��$��13��TotalFloat:�� $��1��$��13��$��Center=�PnYnMnDTnHnMnS�� -   $ totalFloatTextBox��$��2��$��13��$��0,0,0,5q�$	��200���� $��0��$��14��IsCritical:�� -    ��Name$ isCriticalCheckBox��$��2��$��14��$��Center=�$��0,0,0,5q��� $��0��$��15��StatusTime:�� $��1��$��15��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ statusTimeTextBox��$��2��$��15��$��0,0,0,5q�$	��200���� $��0��$��16��ActualDuration:�� $��1��$��16��$��Center=�PnYnMnDTnHnMnS�� -   $ actualDurationTextBox��$��2��$��16��$��0,0,0,5q�$	��200���� $��0��$��17��ActualStart:�� $��1��$��17��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ actualStartTextBox��$��2��$��17��$��0,0,0,5q�$	��200���� $��0��$��18��ActualFinish:�� $��1��$��18��$��Center=�YYYY-MM-DDThh:mm:ss�� -   $ actualFinishTextBox��$��2��$��18��$��0,0,0,5q�$	��200���� $��0��$��19��RemainingTime:�� $��1��$��19��$��Center=�PnYnMnDTnHnMnS�� -   $ remainingTimeTextBox��$��2��$��19��$��0,0,0,5q�$	��200���� $��0��$��20��Completion:�� -   $ completionTextBox��$��2��$��20��$��0,0,0,5q�$	��200��� $��22��$��3��$��Right=�.+��� -    ��Name$ okButton��$��60��$��0,0,10,0q�.��_Ok�� -   $ cancelButton��$��60��.��	_Cancel