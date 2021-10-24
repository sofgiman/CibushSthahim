using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cibush_Shtahim.Objects
{
    /*
     * an uiElement that draw rectengle on game canvas
     * the rect being drawn by two points
     * 1. left top pont
     * 2. right bottom point
     * */

    public class DrawRect: UIElement
    {
        //game canvas
        private Canvas gameCanvas;
        // the points
        private Point p1, p2;
        private Rect RectToFill;
        private double area =-1;

        //get points that are the Vertices of the rect 
        //get canvas and draw rect on it
        public DrawRect(Canvas gameCanvas,Point p1,Point p2)
        {
            this.gameCanvas = gameCanvas;
            this.p1 = p1;
            this.p2 = p2;
            // create rect
            RectToFill = new Rect(this.p1, this.p2);
            // draw rect
            InvalidateVisual();
        }
        public double getX1()
        {
            return p1.X;
        }
        public double getX2()
        {
            return p2.X;
        }
        public double getY1()
        {
            return p1.Y;
        }
        public double getY2()
        {
            return p2.Y;
        }
        // draw rect
        protected override void OnRender(DrawingContext dc)
        {
        Pen penLine = new Pen();
        penLine.Brush = Brushes.MediumBlue;
        dc.DrawRectangle(Brushes.MediumBlue, penLine, RectToFill);

        }
        public double getArea()
        {
            if (area == -1)
                area = (p2.X - p1.X) * (p2.Y - p1.Y);
            
            return area;
        }
    }   


}

