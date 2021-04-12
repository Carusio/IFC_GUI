using IFC_GUI.ViewModels.NodeViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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

namespace IFC_GUI.Views.NodeViews
{
    /// <summary>
    /// Interaction logic for IfcOutputView.xaml
    /// </summary>
    public partial class IfcOutputView : IViewFor<IfcOutputViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IfcOutputViewModel), typeof(IfcOutputView), new PropertyMetadata(null));

        public IfcOutputViewModel ViewModel
        {
            get => (IfcOutputViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IfcOutputViewModel)value;
        }
        #endregion
        public IfcOutputView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                NodeOutputView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => NodeOutputView.ViewModel = null));
            });
        }
    }
}
