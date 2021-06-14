using IFC_GUI.ViewModels;
using NodeNetwork.Utilities;
using ReactiveUI;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IFC_GUI.Views
{
    /// <summary>
    /// Interaction logic for BreadcrumBarView.xaml
    /// </summary>
    public partial class BreadcrumbBarView : UserControl, IViewFor<BreadCrumbBarViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(BreadCrumbBarViewModel), typeof(BreadcrumbBarView), new PropertyMetadata(null));

        public BreadCrumbBarViewModel ViewModel
        {
            get => (BreadCrumbBarViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (BreadCrumbBarViewModel)value;
        }
        #endregion
        public BreadcrumbBarView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindList(ViewModel, vm => vm.ActivePath, v => v.list.ItemsSource).DisposeWith(d);
                this.WhenAnyValue(v => v.list.SelectedItem)
                    .Where(i => i != null)
                    .Cast<BreadCrumbViewModel>()
                    .Do(_ => list.UnselectAll())
                    .InvokeCommand(this, v => v.ViewModel.SelectCrumb).DisposeWith(d);
            });
        }
    }
}
