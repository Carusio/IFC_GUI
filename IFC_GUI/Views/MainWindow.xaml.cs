using IFC_GUI.ViewModels;
using ReactiveUI;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace IFC_GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window, IViewFor<MainWindowViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(MainWindowViewModel), typeof(MainWindowView), new PropertyMetadata(null));

        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainWindowViewModel)value;
        }
        #endregion
        private readonly MenuItem showSubNetworkButton;

        public MainWindowView()
        {
            InitializeComponent();

            var nodeMenu = ((ContextMenu)Resources["nodeMenu"]).Items.OfType<MenuItem>();
            showSubNetworkButton = nodeMenu.First(c => c.Name == nameof(showSubNetworkButton));

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Network, v => v.networkView.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NodeList, v => v.nodeListView.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NetworkBreadCrumbBar, v => v.breadcrumbBarView.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MenuBar, v => v.menuBarView.ViewModel).DisposeWith(d);

                // right-click menu to show the subnetwork of a node
                this.BindCommand(ViewModel, vm => vm.ShowSubNetwork, v => v.showSubNetworkButton).DisposeWith(d);
                // Button for automatic layouting
                this.BindCommand(ViewModel, vm => vm.AutoLayout, v => v.autoLayoutButton);

                // IFC Task Time
                // this.BindCommand(ViewModel, vm => vm.TaskTimeWindow, v => v.taskTimeView.ViewModel);
            });
            this.ViewModel = new MainWindowViewModel();
        }
    }
}
