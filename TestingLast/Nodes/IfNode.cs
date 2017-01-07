﻿using Crainiate.Diagramming.Flowcharting;
using DrawShapes.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingLast.Nodes
{
    class IfNode : DecisionNode
    {
        protected HolderNode falseNode;
        protected HolderNode backfalseNode;
        protected HolderNode middleNode;
        protected ConnectorNode falseConnector;
        public override void onShapeClicked()
        {
            if (Shape.Selected)
            {
                //AssignmentDialog db = new AssignmentDialog();
                IfBox ifBox = new IfBox();
                if (!String.IsNullOrEmpty(Statement)) {
                    ifBox.setExpression(Statement.ToString()); 
                    
                }
                DialogResult dr = ifBox.ShowDialog();
                if (dr == DialogResult.OK) {
                    Statement = ifBox.getExpression();
                    setText(Statement);       
                    //Shape.Label = new Crainiate.Diagramming.Label(Statement);
                }
                MessageBox.Show(surrondExpression(Statement));
                Shape.Selected = false;
            }
        }

        private String surrondExpression(String str)
        {
            return "if ( " + str + ") { }";
           
            
        }

        protected override void makeConnections()
        {
            middleNode = new HolderNode(this);
            middleNode.Shape.Label = new Crainiate.Diagramming.Label("Done");
            ///////////////////truepart
            TrueNode = new HolderNode(this);
            TrueNode.Shape.Label = new Crainiate.Diagramming.Label("Start IF");
            trueConnector = new ConnectorNode(this);
            trueConnector.Connector.Opacity = 50;
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            BackNode = new HolderNode(this);
            BackNode.Shape.Label = new Crainiate.Diagramming.Label("End IF");
            BackNode.OutConnector.EndNode = this;
            BackNode.OutConnector.Connector.End.Shape = middleNode.Shape;
            BackNode.OutConnector.Connector.Opacity = 50;        
            TrueNode.OutConnector.EndNode = BackNode;
            trueConnector.Selectable = false;
            BackNode.OutConnector.Selectable = false;
            BackNode.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
            /////////////////////////////false part
            falseNode = new HolderNode(this);
            falseNode.Shape.Label = new Crainiate.Diagramming.Label("Start Else");
            falseConnector = new ConnectorNode(this);
            falseConnector.Connector.Opacity = 50;
            falseConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            backfalseNode = new HolderNode(this);
            backfalseNode.Shape.Label = new Crainiate.Diagramming.Label("End Else");
            backfalseNode.OutConnector.EndNode = this;
            backfalseNode.OutConnector.Connector.End.Shape = middleNode.Shape;
            backfalseNode.OutConnector.Connector.Opacity = 50;
            falseNode.OutConnector.EndNode = backfalseNode;
            falseConnector.Selectable = false;
            backfalseNode.OutConnector.Selectable = false;
            backfalseNode.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }

        protected override void moveConnections()
        {
            //move middle Node
            middleNode.NodeLocation = new PointF(Shape.Center.X - middleNode.Shape.Width / 2, Shape.Center.Y - middleNode.Shape.Size.Height / 2);
            middleNode.shiftDown(moreShift);
            shiftMainTrack();
            /////////////// move true part
            PointF point = new PointF(Shape.Width+Shape.Location.X + 100, Shape.Center.Y - TrueNode.Shape.Size.Height / 2);
            TrueNode.NodeLocation = point;
           
            if (trueConnector.EndNode == null)
            {
                trueConnector.EndNode = TrueNode;
                //this.OutConnector.EndNode.shiftDown();
                TrueNode.attachNode(BackNode);
               
                //      holderNode.attachNode(this, backConnector);
            }
           else if (TrueNode.OutConnector.EndNode is HolderNode)
            {
                BackNode.NodeLocation = new PointF(point.X, point.Y + 100);
            }
            else
                TrueNode.OutConnector.EndNode.shiftDown(moreShift);
            ///////////////////////////////False Part
            PointF point2 = new PointF(Shape.Location.X - 100, Shape.Center.Y - TrueNode.Shape.Size.Height / 2);
            falseNode.NodeLocation = point2;
            // backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            if (falseConnector.EndNode == null)
            {
                falseConnector.EndNode = falseNode;
                //this.OutConnector.EndNode.shiftDown();
                falseNode.attachNode(backfalseNode);
               
                //      holderNode.attachNode(this, backConnector);
            }
           else if (falseNode.OutConnector.EndNode is HolderNode)
            {
                backfalseNode.NodeLocation = new PointF(point2.X, point2.Y + 100);
            }
            else
                falseNode.OutConnector.EndNode.shiftDown(moreShift);

            
        }

        public override void shiftMainTrack()
        {
                OutConnector.EndNode.shiftDown(moreShift);
           
        }
        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
            clickedConnector.StartNode.attachNode(newNode);
            if (OutConnector.EndNode.NodeLocation.Y < BackNode.NodeLocation.Y||
                OutConnector.EndNode.NodeLocation.Y < backfalseNode.NodeLocation.Y)
            {
                
                shiftMainTrack();
                
            }

            balanceMiddleNode();
        }

        private void balanceMiddleNode()
        {
            if (middleNode.NodeLocation.Y < BackNode.NodeLocation.Y)
            {
                middleNode.NodeLocation = new PointF(middleNode.NodeLocation.X, BackNode.NodeLocation.Y);

            }
            else if (middleNode.NodeLocation.Y < backfalseNode.NodeLocation.Y)
            {
                middleNode.NodeLocation = new PointF(middleNode.NodeLocation.X, backfalseNode.NodeLocation.Y);
            }
        }

        public IfNode()
        {
            
            Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            Shape.GradientColor = Color.Black;
            setText("IF");
            Shape.Invalidate();
        }
        public override void addToModel()
        {
            base.addToModel();
            falseNode.addToModel();
            backfalseNode.addToModel();
            middleNode.addToModel();
            Model.Lines.Add(falseConnector.Connector);
        }

    }
}