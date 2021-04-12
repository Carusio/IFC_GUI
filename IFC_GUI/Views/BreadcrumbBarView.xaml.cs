using IFC_GUI.ViewModels;
using NodeNetwork.Utilities;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IFC_GUI.Views
{
    /// <summary>
    /// Interaction logic for BreadcrumBarView.xaml
    /// </summary>
    public partial class BreadcrumbBarView : UserControl, IViewFor<BreadcrumbBarViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(BreadcrumbBarViewModel), typeof(BreadcrumbBarView), new PropertyMetadata(null));

        public BreadcrumbBarViewModel ViewModel
        {
            get => (BreadcrumbBarViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (BreadcrumbBarViewModel)value;
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
                    .Cast<BreadcrumbViewModel>()
                    .Do(_ => list.UnselectAll())
                    .InvokeCommand(this, v => v.ViewModel.SelectCrumb).DisposeWith(d);
            });
        }
    }
}
