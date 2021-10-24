
using Cibush_Shtahim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Cibush_Shtahim
{
    /**
     * A uiElement which draws line horrizntly/verticaly 
     * Each mouse down event triggers line drwaing.
     * The drawing starts from the point theat the user clcik on.
     * 
     * The drawing stopps on two conditions
     * 1. reach limit of canvas or an area that is not captured
     * 2. hit a ball         
     * */
    public class Line:UIElement
    {
        //handles the end drawing 
        public event EventHandler DrawLineEnded;
        //check hit on a ball
        public event EventHandler CheckCollapse;
        // the game canvas line being drawn
        private Canvas gameCanvas;
        //points of the line
        private Point p1;
        private Point p2;
        //cordinats of the limits that line can be drawn
        private double x1Limit;
        private double y1Limit;
        private double x2Limit;
        private double y2Limit;
        //check how to draw line
        private bool CursorType_gameCanvas;
        //timer
        public DispatcherTimer Timer { get; set; }
        /*
         * get canvas to draw, the starting point and type of drawing
         * start the drawing
         * */
        public Line(Canvas gameCanvas,Point startLine,bool CursorType_gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            p1 = new Point(startLine.X, startLine.Y);
            p2 = new Point(startLine.X, startLine.Y);
            this.CursorType_gameCanvas = CursorType_gameCanvas;

            this.x1Limit = 0;
            this.y1Limit = 0;
            this.x2Limit = gameCanvas.RenderSize.Width  ;
            this.y2Limit = gameCanvas.RenderSize.Height ;
            //setting line limits 
            UpdateLimits();
            
            Timer = new DispatcherTimer();
            //setting timer interval
            Timer.Interval = TimeSpan.FromMilliseconds(Convert.ToDouble(0.3));
            //start the drawing 
            if (CursorType_gameCanvas)
                Timer.Tick += new EventHandler(AnimateLine_Height);
            else
            {
           
                Timer.Tick += new EventHandler(AnimateLine_Horizontal);
            }
        }
        public Point getP2()
        {
            return this.p2;
        }
        public Point getP1()
        {
            return this.p1;
        }
        public double get_x1Limit()
        {
            return x1Limit;   
        }
        public double get_y1Limit()
        {
            return y1Limit;
        }
        public double get_x2Limit()
        {
            return x2Limit;
        }
        public double get_y2Limit()
        {
            return y2Limit;
        }
        public  void SetCursurType(bool CursorType_gameCanvas)
        {
             this.CursorType_gameCanvas = CursorType_gameCanvas;           
        }
        public bool GetCursurType()
        {
            return CursorType_gameCanvas; 
        }
        //drawing the line each timer tick
        private void AnimateLine_Height(object sender, EventArgs e)
        {
            // check  line hit the ball
            CheckCollapse(this,EventArgs.Empty);
            //draw line
            this.InvalidateVisual();
            p1.Y-=4;
            p2.Y += 4;
            // check point 1 reached limit
            if (p1.Y <= y1Limit)
            {
                p1.Y = y1Limit;
            }
            // check point 2 reached limit
            if (p2.Y >= y2Limit)
            {
                p2.Y = y2Limit;
            }
            //stop drawing line cause both points reached limits 
            if (p1.Y <= y1Limit && p2.Y>=y2Limit)
            {
                p1.Y = y1Limit;
                p2.Y  = y2Limit;
                Timer.Stop();
                // handle the finished line drawed
            EventHandler handler = DrawLineEnded;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                } 
            }
        }
        protected override void OnRender(DrawingContext dc)
        {
            // draw line
            Pen penLine = new Pen();
            penLine.Brush = Brushes.GreenYellow;
            dc.DrawLine(penLine, p1, p2);
        }
        private void AnimateLine_Horizontal(object sender, EventArgs e)
        {
            // check  line hit the ball
            CheckCollapse(this, EventArgs.Empty);
         
                p1.X -= 4;          
                p2.X += 4;
            //draw line
            this.InvalidateVisual();
            // check point 1 reached limit
            if (p1.X<= x1Limit)
            {
                p1.X = x1Limit;
            }
            // check point 2 reached limit
            if (p2.X >= x2Limit)
            {
                p2.X = x2Limit;
            }
            //stop drawing line cause both points reached limits 
            if (p1.X <= x1Limit && p2.X >= x2Limit)
            {
                
                p1.X = x1Limit;
                p2.X = x2Limit;
                Timer.Stop();
                // handle the finiished line drawed
                EventHandler handler = DrawLineEnded;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// check where are the closest cordinates to the starting point and building limits for it
        /// </summary>
        public void UpdateLimits()
        {
       
            //check all children of the canvas
            for (int i = 0; i < gameCanvas.Children.Count; i++)
            {
                //check children of the canvas is a DrawRect type
                if ((gameCanvas.Children[i]).GetType().Equals(typeof(DrawRect)))
                {
                        
                    DrawRect RectengleCollide = (DrawRect)(gameCanvas.Children[i]);
                    double recX1 = RectengleCollide.getX1();
                    double recX2 = RectengleCollide.getX2();

                    //check if the starting point is between the rect Y cordinantes
                    if (RectengleCollide.getY2() >= p1.Y && RectengleCollide.getY1() <= p1.Y)
                    {
                        //check if rect right closer to line then current left limit
                        if (recX2 > x1Limit && recX2 <= p1.X)
                        {
                            x1Limit = recX2;
                        }
                        //check if rect left closer to line then current right limit
                        if (recX1 < x2Limit && recX1 >= p2.X)
                            x2Limit = recX1;
                  
                    }
                }
            }
            //check all children of the canvas
            {
                for (int i = 0; i < gameCanvas.Children.Count; i++)
                {
                    //check children of the canvas is a DrawRect type
                    if ((gameCanvas.Children[i]).GetType().Equals(typeof(DrawRect)))
                    {
                        DrawRect RectengleCollide = (DrawRect)(gameCanvas.Children[i]);
                        double recX1 = RectengleCollide.getX1();
                        double recX2 = RectengleCollide.getX2();
                        double recY1 = RectengleCollide.getY1();
                        double recY2 = RectengleCollide.getY2();
                        //check if the starting point is between the rect X cordinantes
                        if ( recX1<= p1.X && recX2>=p1.X)
                        {
                            //check if rect bottom closer to line then current top  limit
                            if (recY2 > y1Limit  && recY2 <= p1.Y)
                            {
                                y1Limit = recY2;
                            }
                            //check if rect top closer to line then current bottom limit
                            if (recY1 < y2Limit && recY1 >= p2.Y)
                                y2Limit = recY1;
                        }
                    }
                }
            } 

        }      
    }
}
