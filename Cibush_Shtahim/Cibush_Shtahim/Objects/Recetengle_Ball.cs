using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Cibush_Shtahim.Objects
{
    /*
     * 
     * an uielement which represnt the ball in shape of recteangle.
     * it moves on canvas using fixed delta X and Y axis.
     * 
     * */
    public class Recetengle_Ball : UIElement
    {
        //game canvas
        private Canvas gameCanvas;
        //the recetangle that is being drawn on the canvas
        private Rectangle r1;
        //cordinants of the area limit which the rectengle can move in
        private double x1Limit;
        private double y1Limit;
        private double x2Limit;
        private double y2Limit;
        //X and Y fixed delta to move the ball
        private double dX;
        private double dY;

        //timer
        public DispatcherTimer Timer { get; set; }
        /** 
        * @param {Canvas} gameCanvas add rectengle to canvas and start moving it by deltas
        * @param {double} dX delta to move on X axis 
        * @param {double} dy delta to move on Y axis 
        */
        public Recetengle_Ball(Canvas gameCanvas, double dX, double dY)
        {
            this.gameCanvas = gameCanvas;
            this.r1 = new Rectangle();
            // recetngle design
            r1.Width = 15;
            r1.Height = 15;
            r1.Fill = Brushes.Silver;
            r1.Stroke = Brushes.Beige;
            r1.StrokeThickness = 4;
            //add rect to canvas
            Canvas.SetLeft(r1, gameCanvas.Width/2);
            Canvas.SetTop(r1, gameCanvas.Height/2);
            gameCanvas.Children.Add(r1);
           
            this.x1Limit = 0;
            this.y1Limit = 0;
            this.x2Limit = gameCanvas.RenderSize.Width ;
            this.y2Limit = gameCanvas.RenderSize.Height ;
           
            this.dX = dX;
             this.dY = dY;

            Timer = new DispatcherTimer();
            // timer interval
            Timer.Interval = TimeSpan.FromMilliseconds(Convert.ToDouble(1));
            // hanle tick
            Timer.Tick += new EventHandler(this.moveBall);
        }
        public Rectangle getR1()
        {
            return r1;
        }
        /* event that happen each timer tick
         * the event move the ball if the limits allows it          
         */ 
        private void moveBall(object sender, EventArgs e)
        {
            
            double r1Left = Canvas.GetLeft(r1);
            //set rect left to check its not reached limits
            Canvas.SetLeft(r1,r1Left  - dX);
            double ball_Left = Canvas.GetLeft(r1);
            double ball_Right = Canvas.GetLeft(r1) + r1.Width;
            //check all children of canvas
            for (int i = 0; i < gameCanvas.Children.Count; i++)
            {
                //check children type is DrawRect
                if ((gameCanvas.Children[i]).GetType().Equals(typeof(DrawRect)))
                {
                    DrawRect dk = (DrawRect)(gameCanvas.Children[i]);
                    // check if ball need to update its left X limit
                    if (ball_Left <= dk.getX2() && ball_Right >= dk.getX2() && dX > 0
                        && Canvas.GetTop(r1) >= dk.getY1() && Canvas.GetTop(r1) + r1.Height <= dk.getY2()
                        && x1Limit < dk.getX2())
                    {
                        x1Limit = dk.getX2();
                    }
                    // check if ball need to update its right X limit
                    if (ball_Left <= dk.getX1() && ball_Right >= dk.getX1() && dX < 0
                       && Canvas.GetTop(r1) >= dk.getY1() && Canvas.GetTop(r1) + r1.Height <= dk.getY2()
                       && x2Limit > dk.getX1())
                    {
                        x2Limit = dk.getX1();
                    }
                }
            }
            //change limits if needed
            if (Canvas.GetLeft(r1) <= x1Limit || Canvas.GetLeft(r1) + r1.Width >= x2Limit)
            {
                dX = -dX;
                Canvas.SetLeft(r1, Canvas.GetLeft(r1) - dX);
            }

            //set rect top to check its not reached limits
            Canvas.SetTop(r1, Canvas.GetTop(r1) - dY);
            double ball_Top = Canvas.GetTop(r1) ;
            double ball_Bottom = Canvas.GetTop(r1)  + r1.Height;
            //check all children of canvas
            for (int i = 0; i < gameCanvas.Children.Count; i++)
            {
                //check children type is DrawRect
                if ((gameCanvas.Children[i]).GetType().Equals(typeof(DrawRect)))
                {
                    DrawRect dk = (DrawRect)(gameCanvas.Children[i]);
                    // check if ball need to update its top Y limit
                    if (ball_Top <= dk.getY2() && ball_Bottom >= dk.getY2() && dY > 0
                             && Canvas.GetLeft(r1) >= dk.getX1() && Canvas.GetLeft(r1) + r1.Width <= dk.getX2()
                             && y1Limit < dk.getY2())
                    {
                        y1Limit = dk.getY2();
                    }
                    // check if ball need to update its bottom Y limit
                    if (ball_Top <= dk.getY1() && ball_Bottom >= dk.getY1() && dY < 0
                            && Canvas.GetLeft(r1) >= dk.getX1() && Canvas.GetLeft(r1) + r1.Width <= dk.getX2()
                          &&  y2Limit > dk.getY1())
                    {
                        y2Limit = dk.getY1();
                    }

                }
            }

            //change limits if needed
            if (Canvas.GetTop(r1) <= y1Limit || Canvas.GetTop(r1) + r1.Height >= y2Limit)
            {
                dY = -dY;
                Canvas.SetTop(r1, Canvas.GetTop(r1) - dY);
            }

        }
        // reset all parametrs
        public void ResetParametres()
        {            
            Canvas.SetLeft(r1, gameCanvas.Width/2);
            Canvas.SetTop(r1, gameCanvas.Height/2);
            gameCanvas.Children.Add(r1);
            

            this.x1Limit = 0;
            this.y1Limit = 0;
            this.x2Limit = gameCanvas.RenderSize.Width;
            this.y2Limit = gameCanvas.RenderSize.Height;
        }
    }
}
