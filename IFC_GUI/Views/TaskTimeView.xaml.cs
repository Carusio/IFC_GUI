using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using ReactiveUI;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace IFC_GUI.Views.NodeViews.NodeTypesView
{
    /// <summary>
    /// Interaction logic for TaskTimeV.xaml
    /// </summary>
    public partial class TaskTimeView : Window, IViewFor<TaskTimeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(TaskTimeViewModel), typeof(TaskTimeView), new PropertyMetadata(null));

        public TaskTimeViewModel ViewModel
        {
            get => (TaskTimeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TaskTimeViewModel)value;
        }
        #endregion
        public TaskTimeView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.Name, v => v.nameTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.DataOrigin, v => v.dataOriginComboBox.SelectedIndex);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.UserDefinedDataOrigin, v => v.userDefinedDataOriginTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.DurationType, v => v.durationTypeComboBox.SelectedIndex);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.ScheduleDuration, v => v.scheduleDurationTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.ScheduleStart, v => v.scheduleStartTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.ScheduleFinish, v => v.scheduleFinishTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.EarlyStart, v => v.earlyStartTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.EarlyFinish, v => v.earlyFinishTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.LateStart, v => v.lateStartTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.LateFinish, v => v.lateFinishTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.FreeFloat, v => v.freeFloatTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.TotalFloat, v => v.totalFloatTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.IsCritical, v => v.isCriticalCheckBox.IsChecked);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.StatusTime, v => v.statusTimeTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.ActualDuration, v => v.actualDurationTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.ActualStart, v => v.actualStartTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.ActualFinish, v => v.actualFinishTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.RemainingTime, v => v.remainingTimeTextBox.Text);
                this.OneWayBind(ViewModel, vm => vm.TaskTimeModel.Completion, v => v.completionTextBox.Text);
            });
            this.ViewModel = new TaskTimeViewModel();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // mw = (MainWindowView)Window.GetWindow(this).Owner;
            //MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            this.ViewModel.TaskTimeModel.Name = this.nameTextBox.Text;
            this.ViewModel.TaskTimeModel.DataOrigin = this.dataOriginComboBox.SelectedIndex;
            this.ViewModel.TaskTimeModel.UserDefinedDataOrigin = this.userDefinedDataOriginTextBox.Text;
            this.ViewModel.TaskTimeModel.DurationType = this.durationTypeComboBox.SelectedIndex;
            this.ViewModel.TaskTimeModel.ScheduleDuration = this.scheduleDurationTextBox.Text;
            this.ViewModel.TaskTimeModel.ScheduleStart = this.scheduleStartTextBox.Text;
            this.ViewModel.TaskTimeModel.ScheduleFinish = this.scheduleFinishTextBox.Text;
            this.ViewModel.TaskTimeModel.EarlyStart = this.earlyStartTextBox.Text;
            this.ViewModel.TaskTimeModel.EarlyFinish = this.earlyFinishTextBox.Text;
            this.ViewModel.TaskTimeModel.LateStart = this.lateStartTextBox.Text;
            this.ViewModel.TaskTimeModel.LateFinish = this.lateFinishTextBox.Text;
            this.ViewModel.TaskTimeModel.FreeFloat = this.freeFloatTextBox.Text;
            this.ViewModel.TaskTimeModel.TotalFloat = this.totalFloatTextBox.Text;
            this.ViewModel.TaskTimeModel.IsCritical = (bool)this.isCriticalCheckBox.IsChecked;
            this.ViewModel.TaskTimeModel.StatusTime = this.statusTimeTextBox.Text;
            this.ViewModel.TaskTimeModel.ActualDuration = this.actualDurationTextBox.Text;
            this.ViewModel.TaskTimeModel.ActualStart = this.actualStartTextBox.Text;
            this.ViewModel.TaskTimeModel.ActualFinish = this.actualFinishTextBox.Text;
            this.ViewModel.TaskTimeModel.RemainingTime = this.remainingTimeTextBox.Text;
            this.ViewModel.TaskTimeModel.Completion = this.completionTextBox.Text;

            //mw.ContentControlPopup.Content = null;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void completionTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
