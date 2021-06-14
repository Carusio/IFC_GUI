using IFC_GUI.DataAccess;
using IFC_GUI.ViewModels;
using Microsoft.Win32;
using ReactiveUI;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*|Ifc file (*.ifc)|*.ifc|Zip file (*.ifczip)|*.ifczip|Xml file (*.ifcxml)|*.ifcxml|Xbim file (*.xbim)|*.xbim"
            };

            bool? ok = openFileDialog.ShowDialog();

            // check if the type of the selected file is supported
            if (ok != true || !IfcDataHandling.CheckFileExtension(openFileDialog.FileName))
            {
                MessageBox.Show("file type not supported");
                return;
            }

            // clear the MainWindow before loading and visualizing new data
            ((MainWindowView)Window.GetWindow(this)).ViewModel = new MainWindowViewModel();

            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            mw.ViewModel.GlobalFilename = openFileDialog.FileName;

            try
            {
                mw.ViewModel.GlobalAllTaskModels = IfcDataHandling.OpenIfcData(openFileDialog.FileName);
                mw.ViewModel.GenerateTaskNodeForEachTaskModelOnCurrentLevel(mw.ViewModel.GlobalAllTaskModels, (NetworkBreadCrumb)mw.ViewModel.NetworkBreadCrumbBar.ActivePath.Items.First(), "");
            }
            catch (FileLoadException fle)
            {
                MessageBox.Show("File content corrupted.");
            }
        }

        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {            

            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            // force focus on network, so the markup bindings trigger and the changes to the attributes in the GUI are registered
            mw.networkView.Focus();

            if (!(mw.ViewModel.GlobalFilename == null))
            {
                IfcDataHandling.UpdateIfcData(mw.ViewModel.GlobalFilename, mw.ViewModel.GlobalAllTaskModels, System.IO.Path.GetExtension(mw.ViewModel.GlobalFilename));
                MessageBox.Show("File saved.");
            } else
            {
                BtnSaveAsFile_Click(sender, e);
            }
        }

        private void BtnSaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView mw = (MainWindowView)Window.GetWindow(this);

            // force focus on network, so the markup binding triggers and the last change of an attribute is registered
            mw.networkView.Focus();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "All files (*.*)|*.*|Ifc file (*.ifc)|*.ifc|Zip file (*.ifczip)|*.ifczip|Xml file (*.ifcxml)|*.ifcxml|Xbim file (*.xbim)|*.xbim",
                DefaultExt = ".ifc",
                OverwritePrompt = true,
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string wantedFileExtension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                string filePathWithoutExtension = System.IO.Path.ChangeExtension(saveFileDialog.FileName, null);
                string globalFileExtension = System.IO.Path.GetExtension(mw.ViewModel.GlobalFilename);
                string globalFilePathWithoutExtension = System.IO.Path.ChangeExtension(mw.ViewModel.GlobalFilename, null);

                // update existing ifc file
                if (File.Exists(saveFileDialog.FileName) && saveFileDialog.FileName == mw.ViewModel.GlobalFilename)
                {
                    IfcDataHandling.UpdateIfcData(mw.ViewModel.GlobalFilename, mw.ViewModel.GlobalAllTaskModels, System.IO.Path.GetExtension(mw.ViewModel.GlobalFilename));
                    MessageBox.Show("File updated.");
                }
                // convert existing ifc file to other format
                else if (filePathWithoutExtension == globalFilePathWithoutExtension && wantedFileExtension != globalFileExtension)
                {
                    IfcDataHandling.UpdateIfcData(mw.ViewModel.GlobalFilename, mw.ViewModel.GlobalAllTaskModels, wantedFileExtension);
                    mw.ViewModel.GlobalFilename = saveFileDialog.FileName;
                    MessageBox.Show($"File saved in {wantedFileExtension} format.");
                }
                // create new ifc file with new ifc project
                else
                {
                    mw.ViewModel.GlobalFilename = saveFileDialog.FileName;
                    IfcDataHandling.NewIfcData(mw.ViewModel.GlobalFilename, mw.ViewModel.GlobalAllTaskModels);
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
