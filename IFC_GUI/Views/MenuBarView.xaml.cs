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

            //var a = ((MainWindowView)Window.GetWindow(this)).ViewModel;

            /*this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.AutoLayout, v => v.BtnAutoLayout);
            });*/
        }

        private void BtnNewFile_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowView)Window.GetWindow(this)).ViewModel = new MainWindowViewModel();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*|Ifc file (*.ifc)|*.ifc|Zip file (*.ifczip)|*.ifczip|Xml file (*.ifcxml)|*.ifcxml|Xbim file (*.xbim)|*.xbim";
            bool? ok = openFileDialog.ShowDialog();

            // check if the type of the selected file is supported
            if (ok != true || !IfcDataHandling.CheckFileExtension(openFileDialog.FileName))  // TODO: check if ifc file or others (ifcxml? ...)
            {
                MessageBox.Show("file type not supported");
                return;
            }

            // clear the MainWindow before loading and visualizing new data
            ((MainWindowView)Window.GetWindow(this)).ViewModel = new MainWindowViewModel();

            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            mw.ViewModel.globalFileName = openFileDialog.FileName;

            //no need for alltaskModels, input the globalAllTaskModels list in recursivenesting method
            var alltaskModels = IfcDataHandling.OpenIfcData(openFileDialog.FileName);

            mw.ViewModel.globalAllTaskModels = alltaskModels;

            IfcDataHandling.RecursiveNestingTaskModelToTaskNode(alltaskModels, mw.ViewModel.NetworkBreadcrumbBar, (NetworkBreadcrumb)mw.ViewModel.NetworkBreadcrumbBar.ActivePath.Items.First(), "", false);
        }

        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {            

            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            // force focus on network, so the markup binding triggers and the last change of an attribute is registered
            mw.networkView.Focus();

            if (!(mw.ViewModel.globalFileName == null))
            {
                IfcDataHandling.UpdateIfcData(mw.ViewModel.globalFileName, mw.ViewModel.globalAllTaskModels, System.IO.Path.GetExtension(mw.ViewModel.globalFileName));
                MessageBox.Show("File saved.");
            }

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

            // force focus on network, so the markup binding triggers and the last change of an attribute is registered
            mw.networkView.Focus();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.*)|*.*|Ifc file (*.ifc)|*.ifc|Zip file (*.ifczip)|*.ifczip|Xml file (*.ifcxml)|*.ifcxml|Xbim file (*.xbim)|*.xbim";
            saveFileDialog.DefaultExt = ".ifc";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                string wantedFileExtension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                string filePathWithoutExtension = System.IO.Path.ChangeExtension(saveFileDialog.FileName, null);
                string globalFileExtension = System.IO.Path.GetExtension(mw.ViewModel.globalFileName);
                string globalFilePathWithoutExtension = System.IO.Path.ChangeExtension(mw.ViewModel.globalFileName, null);

                if (File.Exists(saveFileDialog.FileName) && saveFileDialog.FileName == mw.ViewModel.globalFileName)
                {
                    IfcDataHandling.UpdateIfcData(mw.ViewModel.globalFileName, mw.ViewModel.globalAllTaskModels, System.IO.Path.GetExtension(mw.ViewModel.globalFileName));
                    MessageBox.Show("File overwritten.");
                    /*MessageBoxResult result = MessageBox.Show("The file already exists. Do you want to override it?", "SaveAs", MessageBoxButton.YesNoCancel);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            IfcDataHandling.UpdateIfcData(mw.ViewModel.globalFileName, mw.ViewModel.globalAllTaskModels);
                            MessageBox.Show("File saved.");
                            break;
                        case MessageBoxResult.No:
                            MessageBox.Show("No file was saved.");
                            break;
                        case MessageBoxResult.Cancel:
                            MessageBox.Show("No file was saved.");
                            break;
                    }*/
                }
                // convert existing ifcproject to other format
                else if (/*!File.Exists(saveFileDialog.FileName) &&*/ filePathWithoutExtension == globalFilePathWithoutExtension && wantedFileExtension != globalFileExtension)
                {
                    IfcDataHandling.UpdateIfcData(mw.ViewModel.globalFileName, mw.ViewModel.globalAllTaskModels, wantedFileExtension);
                    mw.ViewModel.globalFileName = saveFileDialog.FileName;
                    MessageBox.Show("File saved in other format.");
                }
                else
                {
                    mw.ViewModel.globalFileName = saveFileDialog.FileName;
                    IfcDataHandling.NewIfcData(mw.ViewModel.globalFileName, mw.ViewModel.globalAllTaskModels);
                    MessageBox.Show("New File saved.");
                }
            }
        }
        private void BtnExitFile_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
