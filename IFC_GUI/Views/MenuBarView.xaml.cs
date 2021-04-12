using DynamicData;
using IFC_GUI.DataAccess;
using IFC_GUI.Models;
using IFC_GUI.ViewModels;
using IFC_GUI.ViewModels.NodeViewModels;
using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using Microsoft.Win32;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for MenuBarView.xaml
    /// </summary>
    public partial class MenuBarView : UserControl, IViewFor<MenuBarViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(MenuBarViewModel), typeof(MenuBarView), new PropertyMetadata(null));

        public MenuBarViewModel ViewModel
        {
            get => (MenuBarViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MenuBarViewModel)value;
        }
        #endregion
        public MenuBarView()
        {
            InitializeComponent();
        }

        private void BtnNewFile_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowView)Window.GetWindow(this)).ViewModel = new MainWindowViewModel();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? ok = openFileDialog.ShowDialog();

            if (ok != true || !IfcDataHandling.CheckFileExtension(openFileDialog.FileName))  // TODO: check if ifc file or others (ifcxml? ...)
            {
                return;
            }

            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            mw.ViewModel.globalFileName = openFileDialog.FileName;

            //no need for alltaskModels, input the globalAllTaskModels list in recursivenesting method
            var alltaskModels = IfcDataHandling.OpenIfcData(openFileDialog.FileName);

            mw.ViewModel.globalAllTaskModels = alltaskModels;

            IfcDataHandling.RecursiveNestingTaskModelToTaskNode(alltaskModels, mw.ViewModel.NetworkBreadcrumbBar, (NetworkBreadcrumb)mw.ViewModel.NetworkBreadcrumbBar.ActivePath.Items.First(), "", false);

            /*foreach (TaskModel tm in alltaskModels)
            {
                IfcTaskNodeViewModel tn = new IfcTaskNodeViewModel(tm);

                if (!tm.IsNestedBy.Any())
                {
                    // All nodes which are not part of a nest
                    mw.ViewModel.Network.Nodes.Add(tn);



                    var subnetworkbreadcrumb = new NetworkBreadcrumb
                    {
                        Name = tm.Name,
                        Network = new NetworkViewModel()
                    };
                    subnetworkbreadcrumb.Network.Nodes.Add(tn);

                    mw.ViewModel.NetworkBreadcrumbBar.ActivePath.Add(subnetworkbreadcrumb);
                }
                else {  }

                //mw.ViewModel.Network.Nodes.Add(tn);
                taskNodes.Add(tn);

                if (tm.IsSuccessorFrom.Any())
                {
                    foreach (var guid in tm.IsSuccessorFrom)
                    {
                        foreach (var node in taskNodes)
                        {
                            if (node.TaskModel.GlobalId == guid)
                            {
                                var connection = new IfcConnectionViewModel(mw.ViewModel.Network, tn.Input, node.Output);
                                mw.ViewModel.Network.Connections.Add(connection);
                            }
                        }
                    }
                }
            }*/
        }
        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            //List<TaskModel> allTaskModels = new List<TaskModel>();

            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            /*var allTaskNodes = mw.ViewModel.Network.Nodes.Items.OfType<IfcTaskNodeViewModel>();

            foreach (var tasknode in allTaskNodes)
            {
                allTaskModels.Add(tasknode.TaskModel);
            }*/

            IfcDataHandling.UpdateIfcData(mw.ViewModel.globalFileName, mw.ViewModel.globalAllTaskModels);

            /*
            // TODO: save should only save an existing file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                if (File.Exists(saveFileDialog.FileName))
                {
                    IfcDataHandling.UpdateIfcData(saveFileDialog.FileName, allTaskModels);
                    MessageBox.Show("File saved");
                }
                else
                {
                    IfcDataHandling.NewIfcData(saveFileDialog.FileName, allTaskModels);
                    MessageBox.Show("New File saved");
                }
            */
        }

        private void BtnSaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true) 
                if (File.Exists(saveFileDialog.FileName))
                {
                    IfcDataHandling.UpdateIfcData(saveFileDialog.FileName, mw.ViewModel.globalAllTaskModels);
                    MessageBox.Show("File saved");
                }
                else
                {
                    IfcDataHandling.NewIfcData(saveFileDialog.FileName, mw.ViewModel.globalAllTaskModels);
                    MessageBox.Show("New File saved");
                }

        }
        private void BtnExitFile_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
