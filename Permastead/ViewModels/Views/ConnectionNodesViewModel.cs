using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using NodifyM.Avalonia.ViewModelBase;

namespace Permastead.ViewModels.Views;

public class ConnectionNodesViewModel : NodifyEditorViewModelBase
{
    public ConnectionNodesViewModel()
    {
        var knot1 = new KnotNodeViewModel()
        {
            Location = new Point(300,100)
        };
        var input1 = new ConnectorViewModelBase()
        {
            Title = "GAIA",
            Flow = ConnectorViewModelBase.ConnectorFlow.Input
        };
        var output1 = new ConnectorViewModelBase()
        {
            Title = "B 1",
            Flow = ConnectorViewModelBase.ConnectorFlow.Output
        };
        
        Connections.Add(new ConnectionViewModelBase(this,output1,knot1.Connector,"Test"));
        Connections.Add(new ConnectionViewModelBase(this,knot1.Connector,input1));
        
        Nodes  = new(){
                
                new NodeViewModelBase()
                {
                    Location = new Point(400, 200),
                    Title = "Node 1",
                    Input = new ObservableCollection<object>
                    {
                        input1,
                       new ComboBox()
                       {
                            ItemsSource = new ObservableCollection<object>
                            {
                                 "Item 1",
                                 "Item 2",
                                 "Item 3"
                                 }
                       }
                    },
                    Output = new ObservableCollection<object>
                    {
                       
                        new ConnectorViewModelBase()
                        {
                            Title = "Output 2",
                            Flow = ConnectorViewModelBase.ConnectorFlow.Output
                        }
                    }
                },
                new NodeViewModelBase()
                {
                    Title = "Node 2",
                    Location = new Point(-100,-100),
                    Input = new ObservableCollection<object>
                    {
                        new ConnectorViewModelBase()
                        {
                            Title = "Input 1",
                            Flow = ConnectorViewModelBase.ConnectorFlow.Input
                        },
                        new ConnectorViewModelBase()
                        {
                            Flow = ConnectorViewModelBase.ConnectorFlow.Input,
                            Title = "Input 2"
                        }
                    },
                    Output = new ObservableCollection<object>
                    {
                        output1,
                        new ConnectorViewModelBase()
                        {
                            Flow = ConnectorViewModelBase.ConnectorFlow.Output,
                            Title = "Output 1"
                        },
                        new ConnectorViewModelBase()
                        {
                            Flow = ConnectorViewModelBase.ConnectorFlow.Output,
                            Title = "Output 2"
                        }
                    }
                }
            };
        Nodes.Add(knot1);
        
        knot1.Connector.IsConnected = true;
        output1.IsConnected = true;
        input1.IsConnected = true;

        var locations = Services.PlantingsService.GetGardenBeds(AppSession.ServiceMode);

        var x = 600;
        var y = 100;
        var ctr = 1;
        foreach (var location in locations)
        {
            var tmpNode = new NodeViewModelBase();
            tmpNode.Title = location.Description;
            tmpNode.Footer = location.Code;
            y = y + 70;
            tmpNode.Location = new Point(x, y);
            
            //get plantings for this bed
            //var plantings = Services.PlantingsService.GetPlantings(AppSession.ServiceMode);
            var tmpInput = new ConnectorViewModelBase()
            {
                Title = "Input" + ctr++,
                Flow = ConnectorViewModelBase.ConnectorFlow.Input
            };
            
            tmpNode.Input = new ObservableCollection<object>();
            tmpNode.Input.Add(tmpInput);
            
            Nodes.Add(tmpNode);
        }
        
        
    }
    
    public override void Connect(ConnectorViewModelBase source, ConnectorViewModelBase target)
    {
        base.Connect(source, target);
    }
    
    public override void DisconnectConnector(ConnectorViewModelBase connector)
    {
        base.DisconnectConnector(connector);
    }
}