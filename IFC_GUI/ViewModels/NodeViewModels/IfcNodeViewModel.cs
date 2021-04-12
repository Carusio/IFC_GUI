using IFC_GUI.Views.NodeViews;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_GUI.ViewModels.NodeViewModels
{
    //All available NodeTypes in this application
    public enum NodeType
    {
        TaskNode, TestNode
    }
    public class IfcNodeViewModel : NodeViewModel
    {
        static IfcNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IfcNodeView(), typeof(IViewFor<IfcNodeViewModel>));
        }
        public NodeType NodeType { get; }

        public IfcNodeViewModel(NodeType type)
        {
            NodeType = type;
        }
    }
}
