using DynamicData;
using IFC_GUI.DataAccess;
using IFC_GUI.Models;
using IFC_GUI.ViewModels.NodeViewModels;
using IFC_GUI.ViewModels.NodeViewModels.NodeTypes;
using IFC_GUI.ViewModels.Toolkit.Layout;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.ViewModels
{
    class NetworkBreadcrumb : BreadcrumbViewModel
    {
        #region Network
        private NetworkViewModel _network;

        // The Taskmodel which nests the nodes in this network.
        public TaskModel TaskModel { get; set; } = null;

        public NetworkViewModel Network
        {
            get => _network;
            set => this.RaiseAndSetIfChanged(ref _network, value);
        }
        #endregion
    }
    public class MainWindowViewModel : ReactiveObject
    {
        #region Network
        private readonly ObservableAsPropertyHelper<NetworkViewModel> _network;
        public NetworkViewModel Network => _network.Value;
        #endregion

        public BreadcrumbBarViewModel NetworkBreadcrumbBar { get; } = new BreadcrumbBarViewModel();
        public NodeListViewModel NodeList { get; } = new NodeListViewModel();
        public MenuBarViewModel MenuBar { get; } = new MenuBarViewModel();

        // List of all TaskModels that are currently shown via TaskNodes
        public List<TaskModel> globalAllTaskModels { get; set; } = new List<TaskModel>();
        // the path of the ifc file which is loaded
        public string globalFileName { get; set; }

        // TaskTime Window
        //public IfcTaskTimeViewModel TaskTimeWindow { get; set; }
        //

        // ShowSubnetwork command
        public ReactiveCommand<Unit, Unit> ShowSubNetwork { get; }
        public ReactiveCommand<Unit, Unit> AutoLayout { get; }




        public MainWindowViewModel()
        {
            this.WhenAnyValue(vm => vm.NetworkBreadcrumbBar.ActiveItem).Cast<NetworkBreadcrumb>()
                .Select(b => b?.Network)
                .ToProperty(this, vm => vm.Network, out _network);

            var mainNetwork = new NetworkBreadcrumb
            {
                Name = "Main",
                TaskModel = null,
                Network = new NetworkViewModel()
            };

            NetworkBreadcrumbBar.ActivePath.Add(mainNetwork);

            // update globalAllTaskModels list, when IfcTaskNodes were added or removed
            mainNetwork.Network.Nodes.Connect().ActOnEveryObject(
                addedNode => {
                    if (addedNode.GetType() == typeof(IfcTaskNodeViewModel) && !globalAllTaskModels.Contains(((IfcTaskNodeViewModel)addedNode).TaskModel))
                    {
                        globalAllTaskModels.Add(((IfcTaskNodeViewModel)addedNode).TaskModel);
                    }
                },
                removedNode => {
                    if (removedNode.GetType() == typeof(IfcTaskNodeViewModel) && globalAllTaskModels.Contains(((IfcTaskNodeViewModel)removedNode).TaskModel)) // check 'contains' is not needed but we keep it for safety
                    {
                        globalAllTaskModels.Remove(((IfcTaskNodeViewModel)removedNode).TaskModel);
                    }
            });

            // update IsSuccesorFrom and IsPredessecorTo attributes, when connections were added or removed
            mainNetwork.Network.Connections.Connect().ActOnEveryObject(
                addedCon => {
                    var tmInput = ((IfcTaskNodeViewModel)addedCon.Input.Parent).TaskModel;
                    var tmOutput = ((IfcTaskNodeViewModel)addedCon.Output.Parent).TaskModel;

                    if (!tmInput.IsSuccessorFrom.Contains(tmOutput.GlobalId))
                    {
                        tmInput.IsSuccessorFrom.Add(tmOutput.GlobalId);
                    }
                    if (!tmOutput.IsPredecessorTo.Contains(tmInput.GlobalId))
                    {
                        tmOutput.IsPredecessorTo.Add(tmInput.GlobalId);
                    }
                },
                removeCon => {
                    var tmInput = ((IfcTaskNodeViewModel)removeCon.Input.Parent).TaskModel;
                    var tmOutput = ((IfcTaskNodeViewModel)removeCon.Output.Parent).TaskModel;

                    tmInput.IsSuccessorFrom.Remove(tmOutput.GlobalId);
                    tmOutput.IsPredecessorTo.Remove(tmInput.GlobalId);
                }
                );

            // all available NodeTypes in the NodeList
            NodeList.AddNodeType(() => new IfcTaskNodeViewModel());
            NodeList.AddNodeType(() => new TestNodeViewModel());

            // AutoLayout command
            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            AutoLayout = ReactiveCommand.Create(() => layouter.Layout(new Configuration { Network = Network }, 10000));

            //TODO: selected Node is not a groupnode so it has no subnet, but every Node should have a subnet
            //TODO: always create subnet even if empty nests
            ShowSubNetwork = ReactiveCommand.Create(() =>
            {
                if (!Network.SelectedNodes.Items.Any())
                {
                    return;
                }

                // show subnetwork of last or first selectedNode in SelectedNodes list - must be an IfcTaskNode
                var selectedNode = Network.SelectedNodes.Items.First(); // .Last()
                IfcTaskNodeViewModel selectedTaskNode;

                // dont do anything if selectedNode is not an IfcTaskNode
                if (selectedNode.GetType() != typeof(IfcTaskNodeViewModel))
                {
                    return;
                }
                else
                {
                    selectedTaskNode = (IfcTaskNodeViewModel)selectedNode;

                    var subnetwork = new NetworkBreadcrumb
                    {
                        Name = selectedTaskNode.TaskModel.Name,
                        TaskModel = selectedTaskNode.TaskModel,
                        Network = new NetworkViewModel()
                    };

                    subnetwork.Network.Nodes.Connect().ActOnEveryObject(
                        addedNode => {
                            if (addedNode.GetType() == typeof(IfcTaskNodeViewModel))
                            {
                                var tm = ((IfcTaskNodeViewModel)addedNode).TaskModel;
                                if (!globalAllTaskModels.Contains(tm))
                                {
                                    globalAllTaskModels.Add(tm);
                                }
                                if (subnetwork.TaskModel != null) // actually not needed
                                {
                                    if(!tm.Nests.Contains(subnetwork.TaskModel.GlobalId))
                                    {
                                        tm.Nests.Add(subnetwork.TaskModel.GlobalId);
                                    }
                                    if(!subnetwork.TaskModel.IsNestedBy.Contains(tm.GlobalId))
                                    {
                                        subnetwork.TaskModel.IsNestedBy.Add(tm.GlobalId);
                                    }
                                }
                            }
                        },
                        removedNode => {
                            if (removedNode.GetType() == typeof(IfcTaskNodeViewModel))
                            {
                                var tm = ((IfcTaskNodeViewModel)removedNode).TaskModel;
                                globalAllTaskModels.Remove(tm);
                                if (subnetwork.TaskModel != null) // actually not needed
                                {
                                    tm.Nests.Remove(subnetwork.TaskModel.GlobalId);
                                    subnetwork.TaskModel.IsNestedBy.Remove(tm.GlobalId);
                                }
                            }
                        });

                    subnetwork.Network.Connections.Connect().ActOnEveryObject(
                        addedCon => {
                            var tmInput = ((IfcTaskNodeViewModel)addedCon.Input.Parent).TaskModel;
                            var tmOutput = ((IfcTaskNodeViewModel)addedCon.Output.Parent).TaskModel;

                            if (!tmInput.IsSuccessorFrom.Contains(tmOutput.GlobalId))
                            {
                                tmInput.IsSuccessorFrom.Add(tmOutput.GlobalId);
                            }
                            if (!tmOutput.IsPredecessorTo.Contains(tmInput.GlobalId))
                            {
                                tmOutput.IsPredecessorTo.Add(tmInput.GlobalId);
                            }
                        },
                        removeCon => {
                            var tmInput = ((IfcTaskNodeViewModel)removeCon.Input.Parent).TaskModel;
                            var tmOutput = ((IfcTaskNodeViewModel)removeCon.Output.Parent).TaskModel;

                            tmInput.IsSuccessorFrom.Remove(tmOutput.GlobalId);
                            tmOutput.IsPredecessorTo.Remove(tmInput.GlobalId);
                        }
                        );

                    NetworkBreadcrumbBar.ActivePath.Add(subnetwork);
                    IfcDataHandling.RecursiveNestingTaskModelToTaskNode(globalAllTaskModels, NetworkBreadcrumbBar, subnetwork, selectedTaskNode.TaskModel.GlobalId, false);
                }
                /*if (selectedNode.TaskModel.Nests != null )//&& selectedNode.TaskModel.Nests.Any())
                {
                    var subnetwork = new NetworkBreadcrumb
                    {
                        Name = selectedNode.TaskModel.Name,
                        Network = new NetworkViewModel()
                    };

                    NetworkBreadcrumbBar.ActivePath.Add(subnetwork);
                    IfcDataHandling.RecursiveNesting(globalAllTaskModels, NetworkBreadcrumbBar, subnetwork, selectedNode.TaskModel.GlobalId, false);

                    // List<TaskModel> nested = new List<TaskModel>();

                    // IfcDataHandling.RecursiveNesting(globalAllTaskModels, NetworkBreadcrumbBar, (NetworkBreadcrumb)NetworkBreadcrumbBar.ActiveItem, ((IfcTaskNodeViewModel)Network.SelectedNodes.Items.First()).TaskModel.GlobalId, false);
                }*/
            });
        }
    }
}
