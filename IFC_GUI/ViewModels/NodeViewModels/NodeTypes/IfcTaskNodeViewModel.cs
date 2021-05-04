using DynamicData;
using IFC_GUI.Models;
using IFC_GUI.Views;
using IFC_GUI.Views.NodeViews;
using IFC_GUI.Views.NodeViews.NodeTypesView;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IFC_GUI.ViewModels.NodeViewModels.NodeTypes
{
    public class IfcTaskNodeViewModel : IfcNodeViewModel
    {
        //TODO: if Ifctasknodeview doesnt work back to ifcnodeview
        static IfcTaskNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcTaskNodeView(), typeof(IViewFor<IfcTaskNodeViewModel>));
        }

        public IfcInputViewModel Input { get; }
        public IfcOutputViewModel Output { get; }

        private TaskModel _taskModel;
        public TaskModel TaskModel 
        { get => _taskModel;
          set => this.RaiseAndSetIfChanged(ref _taskModel, value); 
        }

        public IfcTaskNodeViewModel() : base(NodeType.TaskNode)
        {
            this.Name = "Task_Node";
            this.Resizable = ResizeOrientation.HorizontalAndVertical;
            this.TaskModel = new TaskModel();

            Input = new IfcInputViewModel(PortType.None)
            {
                Name = "IsSuccessorFrom"
            };
            this.Inputs.Add(Input);

            Output = new IfcOutputViewModel(PortType.None)
            {
                Name = "IsPredessecorTo"
            };
            this.Outputs.Add(Output);

            // MainWindowView mw = (MainWindowView)Window.GetWindow(this);
            //mw.ViewModel.globalAllTaskModels.Add(this.TaskModel);*/
        }

        public IfcTaskNodeViewModel(TaskModel tm) : base(NodeType.TaskNode)
        {
            this.Name = "Task_Node";
            this.Resizable = ResizeOrientation.HorizontalAndVertical;
            this.TaskModel = tm;

            Input = new IfcInputViewModel(PortType.None)
            {
                Name = "IsSuccessorFrom"
            };
            this.Inputs.Add(Input);


            Output = new IfcOutputViewModel(PortType.None)
            {
                Name = "IsPredessecorTo"
            };
            this.Outputs.Add(Output);
        }
    }
}
