using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;


namespace IFC_GUI.Views.NodeViews.NodeTypesView
{
    /// <summary>
    /// Interaction logic for IfcTaskNodeView.xaml
    /// </summary>
    public partial class IfcTaskNodeView : IViewFor<IfcTaskNodeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcTaskNodeViewModel), typeof(IfcTaskNodeView), new PropertyMetadata(null));

        public IfcTaskNodeViewModel ViewModel
        {
            get => (IfcTaskNodeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcTaskNodeViewModel)value;
        }
        #endregion
        
        public IfcTaskNodeView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.IfcTaskView.ViewModel).DisposeWith(d);

                IfcTaskView.ViewModel = this.ViewModel;
                Disposable.Create(() => IfcTaskView.ViewModel = null).DisposeWith(d);
            });
        }

        private void TasktimeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            var tasktimewindow = new TaskTimeView
            {
                Owner = mw,
                Title = $"TaskTime of {this.ViewModel.TaskModel.Name}"
            };

            tasktimewindow.ViewModel.TaskTimeModel = this.ViewModel.TaskModel.TaskTime;

            tasktimewindow.Show();
        }

        private void PriorityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
