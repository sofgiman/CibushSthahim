using Cibush_Shtahim.Objects;
using Cibush_Shtahim.ViewModels;

using System;
using System.Collections.Generic;
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

namespace Cibush_Shtahim.Views
{
    /// <summary>
    /// 
    /// Interaction logic for game.xaml
    /// </summary>
    public partial class game : UserControl
    {

        //total points user earned
        public static int TotalPoints;


        //starting ball
        private Recetengle_Ball ball;
        //list of balls to organize them
        List<Recetengle_Ball> listOfBalls = new List<Recetengle_Ball>();

        // indicate horizntal(false) or vertical(true) line direction
        public bool IsVerticalLineDirection;
       
        // total area of the canvas
        private double CanvasTotalArea;
        //precentage that needed to reach in order to pass next level
        private int PrecentageToPassNextLevel;
        // the area user captured
        private double TotalCapturedArea ;
        //flag for indicate that user can pass next level
        private bool UserReachedMaxCaptureAreaPrecentage = false;

        // current points user gained
        private int TotalUserPoints;
        //number of tries the user pressed on canvas
        private int NumberOfUserClicks;
        //flag which indicate the game ended(no health points)
        private bool HealthPointsIsZero;
        //user clicks on free area on canvas
        private bool UserClicksOnFreeArea = true;
        //The line is being drawn
        private bool LineIsBeingDrawn = false;


        //constrator to init component
        //and calculate canvas game area
        public game()
        {
            InitializeComponent();
            CanvasTotalArea = gameCanvas.Width * gameCanvas.Height;
        }

        /// <summary>
        /// function being  called when game is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TotalCapturedArea = 0;
            NumberOfUserClicks = 0;
          //  PrectengeCaculateBlock.Text = "0";

            //set precenteage to pass 
            PrecentageToPassNextLevel = 90;
            PrecentegeTabToPass.Text = PrecentageToPassNextLevel.ToString();
            Precentage.Foreground = Brushes.Red;
            PrecentegeTabToPass.Foreground = Brushes.Red;
            //point tab
            this.TotalUserPoints = 0;
            PointTab.Text =  "0";
            //set cursor on canvas
            gameCanvas.Cursor = Cursors.UpArrow;
            IsVerticalLineDirection = true;
    
            StartMovaBall();

            //add event handler for canvas moveDown Event to trace clicks on canvas
            EventManager.RegisterClassHandler(typeof(Canvas), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnMouseDownInCanvas));
            //event that triggers when user press on the space
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            //set HP isn't zero
            HealthPointsIsZero = false;

        }

     
        /* 
         * Handles Event for User press the screen on the game canvas, 
         * as a result a new line is created (of Type UI element) depends on position click 
         * and start the line timer.
         */
        public void OnMouseDownInCanvas(object sender, MouseButtonEventArgs e)
        {
            //check that other line isn't being drawn 
            if (!LineIsBeingDrawn)
            {
                UserClicksOnFreeArea = true;
                LineIsBeingDrawn = true;
                // get the point that the user press on
                Point p = e.GetPosition((Canvas)sender);
                //check if the point on uncaptred area
                for (int i = 0; i < gameCanvas.Children.Count && UserClicksOnFreeArea; i++)
                {
                    if (gameCanvas.Children[i].GetType() == typeof(DrawRect))
                    {
                        DrawRect DK = (DrawRect)gameCanvas.Children[i];
                        double DK_left = DK.getX1();
                        double DK_Right = DK.getX2();
                        double DK_Top = DK.getY1();
                        double DK_Bottom = DK.getY2();                       
                        if (p.X >= DK_left && p.X <= DK_Right && p.Y >= DK_Top && p.Y <= DK_Bottom)
                        {
                            //point is on captured area
                            UserClicksOnFreeArea = false;
                            LineIsBeingDrawn = false;
                        }
                    }
                }
                if (UserClicksOnFreeArea)
                {
                    // create line and event handlers 
                    Line DrawLine = new Line(gameCanvas, p, IsVerticalLineDirection);

                    //add event handlers to line
                    DrawLine.DrawLineEnded += dl_DrawLineEnded;
                    DrawLine.CheckCollapse += DrawLine_CheckCollapse;

                    gameCanvas.Children.Add(DrawLine);
                    DrawLine.Timer.Start();

                }
            }
        }

        /* 
         * Handles event that check line state 
         * each line tick the hanlder check if line hit ball
         */
        private void DrawLine_CheckCollapse(object sender, EventArgs e)
        {
            Line line = (Line)sender;
            // check all balls
            for (int i = 0; i < listOfBalls.Count; i++)
            {
                bool WidthBall_CollapseLine;
                bool HeightBall_CollapseLine;
                //get cordinants of ball
                double LeftBall = Canvas.GetLeft(listOfBalls[i].getR1());
                double RightBall = Canvas.GetLeft(listOfBalls[i].getR1()) + listOfBalls[i].getR1().Width;
                double TopBall = Canvas.GetTop(listOfBalls[i].getR1());
                double BottomBall = Canvas.GetTop(listOfBalls[i].getR1()) + listOfBalls[i].getR1().Height;
                // get cordinants of line
                double Y_p1 = line.getP1().Y;
                double Y_p2 = line.getP2().Y;
                double X_p1 = line.getP1().X;
                double X_p2 = line.getP2().X;
              
                //  check ball is left to point
                {
                    if (RightBall < X_p1 && RightBall < X_p2)
                        WidthBall_CollapseLine = false;
                    //  check ball is right to point
                    else if (LeftBall > X_p2 && LeftBall > X_p1)
                        WidthBall_CollapseLine = false;
                    // ball is between the line points
                    else
                        WidthBall_CollapseLine = true;                 

                    // cheeck line is between the ball Y's points
                    if (TopBall < Y_p1 && BottomBall > Y_p2)
                        HeightBall_CollapseLine = true;
                    // check ball touch line Y's points
                    else if (TopBall < Y_p1 && BottomBall > Y_p1)
                        HeightBall_CollapseLine = true;
                    else if (TopBall < Y_p2 && BottomBall > Y_p2)
                        HeightBall_CollapseLine = true;
                    // check ball is between line Y's points
                    else if (TopBall > Y_p1 && BottomBall < Y_p2)
                        HeightBall_CollapseLine = true;
                    else
                        HeightBall_CollapseLine = false;
                }


                // check if line hit ball
                if (WidthBall_CollapseLine == true && HeightBall_CollapseLine == true)
                {
                     line.Timer.Stop();
                    //dcrease HP
                     decreaeHealthPoints();
                    //remove line event handlers
                    line.DrawLineEnded -= dl_DrawLineEnded;
                    line.CheckCollapse -= DrawLine_CheckCollapse;
                    NumberOfUserClicks++;
                    LineIsBeingDrawn = false;
                    //check user have no longer HP
                    if (HealthPointsIsZero && Window.GetWindow(this) != null)
                    {
                        //save user points to Total points for later use in another screen   
                        TotalPoints = TotalUserPoints;
                        //clear event handlers and resources
                        for (int index=0 ;i<gameCanvas.Children.Count;i++)
                        {
                            gameCanvas.Children.Remove(gameCanvas.Children[index]);
                        }
                        gameCanvas.Children.Clear();
                          
                        var window = Window.GetWindow(this);
                        window.KeyDown -= HandleKeyPress;
                        //switch user control to disqualification screen
                        Window.GetWindow(this).DataContext = MainWindow.disqualificationScreen;
                    }
                    // remove line from canvas
                    gameCanvas.Children.Remove(line);
                }
            }
        }
        /*
         *  decrease Health Points every time ball hit the line
         */
        public void decreaeHealthPoints()
        {
            // check what heart need to remove
            if (Heart3.Visibility == Visibility.Visible)
            {
                Heart3.Visibility = Visibility.Hidden;
            }
            else if (Heart2.Visibility == Visibility.Visible)
            {
                Heart2.Visibility = Visibility.Hidden;
            }
            else if (Heart1.Visibility == Visibility.Visible)
            {
                Heart1.Visibility = Visibility.Hidden;
                //user have no longer HP
                HealthPointsIsZero = true;
            }
        }
        
        
        //creating and start moving the 1st ball
        private void StartMovaBall()
        {
            //create ball
            ball = new Recetengle_Ball(gameCanvas, 4, 2);
            // add ball to list bals
            listOfBalls.Add(ball);
            for (int i = 0; i < listOfBalls.Count; i++)
            {
                //start ball timer
                listOfBalls[i].Timer.Start();
            }
         }

        //event that triggerd when user press key
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            //if key is space key
            if (e.Key == Key.Space)
            {
                //change to horizontial cursor
                if (IsVerticalLineDirection)
                {
                    gameCanvas.Cursor = Cursors.Cross;
                    IsVerticalLineDirection = false;
                }
                // change to vertical cursor
                else
                {
                    gameCanvas.Cursor = Cursors.UpArrow;
                    IsVerticalLineDirection = true;
                }
            }
        }


      

        /*
         * Handle event which occuer when line finished drawing 
         * the event check where to draw the captured area; there are 3 options:
         * 1. balls on 1st side the line divided - captured area on the 2nd side
         * 2. balls on 2nd side the line divided - captured area on the 1st side
         * 3. balls are on both sides the line divided - captured area between the sides
         */
        public void dl_DrawLineEnded(object sender, EventArgs e)
        {
            NumberOfUserClicks++;

            Line line = ((Line)sender);
            //cordinants of the line
            Double lineX2 = line.getP2().X;
            Double lineX1 = line.getP1().X;
            Double lineY2 = line.getP2().Y;
            Double lineY1 = line.getP1().Y;
            
            int ballsOnRightSideCounter = 0;
            int ballsOnLeftSideCounter = 0;
            int ballsOnTopSideCounter = 0;
            int ballsOnBottomSideCounter = 0;
            //check if line is vertical
            if (line.GetCursurType())
            {
                // check where the rectengle needed to be drawn
                for (int i = 0; i < listOfBalls.Count; i++)
                {
                    Double left = Canvas.GetLeft(listOfBalls[i].getR1());
                    Double right = Canvas.GetLeft(listOfBalls[i].getR1()) + listOfBalls[i].getR1().Width;
                    Double top = Canvas.GetTop(listOfBalls[i].getR1());
                    Double bottom = Canvas.GetTop(listOfBalls[i].getR1()) + listOfBalls[i].getR1().Height;
                    //ball between line Y's cordinants
                    if (line.get_y1Limit() <= top && bottom <= line.get_y2Limit())
                    {
                        //check ball on right side
                        if (left >= lineX2 && right <= line.get_x2Limit())
                        {
                            ballsOnRightSideCounter++;
                        }
                        //check ball on left side
                        if (right < lineX2 && left > line.get_x1Limit())
                        {
                            ballsOnLeftSideCounter++;
                        }
                    }
                }
                //Draw the recetangle
                if (ballsOnRightSideCounter > 0 && ballsOnLeftSideCounter > 0)
                {
                    DrawCaptureAreaAndUpdateUserData(lineX1 - 1, lineY1, lineX2 + 1, lineY2);
                }
                else if (ballsOnRightSideCounter > 0)
                {
                    DrawCaptureAreaAndUpdateUserData(line.get_x1Limit(), line.get_y1Limit(), lineX2, lineY2);
                }
                else if (ballsOnLeftSideCounter > 0)
                {
                    DrawCaptureAreaAndUpdateUserData(lineX1, lineY1, line.get_x2Limit(), line.get_y2Limit());
                }
            }
            //check if line is horizontial
            else
            {
                // check where the rectengle needed to be drawn
                for (int i = 0; i < listOfBalls.Count; i++)
                {
                    Double left = Canvas.GetLeft(listOfBalls[i].getR1());
                    Double right = Canvas.GetLeft(listOfBalls[i].getR1()) + listOfBalls[i].getR1().Width;
                    Double top = Canvas.GetTop(listOfBalls[i].getR1());
                    Double bottom = Canvas.GetTop(listOfBalls[i].getR1()) + listOfBalls[i].getR1().Height;
                    // check if ball X's is between the line X's
                    if (line.get_x1Limit() <= left && right <= line.get_x2Limit())
                    {
                        //check ball on bottom side
                        if (top >= lineY1 && bottom <= line.get_y2Limit())
                        {
                            ballsOnBottomSideCounter++;
                        }
                        //check ball on top side
                        if (bottom < lineY1 && top > line.get_y1Limit())
                        {
                            ballsOnTopSideCounter++;
                        }
                    }
                }
                //draw the rectengle needed to draw from horizontial click 
                if (ballsOnTopSideCounter > 0 && ballsOnBottomSideCounter > 0)
                {
                    DrawCaptureAreaAndUpdateUserData(lineX1, lineY1 - 1, lineX2, lineY2 + 1);
                }
                else if (ballsOnBottomSideCounter > 0)
                {
                    DrawCaptureAreaAndUpdateUserData(line.get_x1Limit(), line.get_y1Limit(), lineX2, lineY2);
                }
                else if (ballsOnTopSideCounter > 0)
                {
                    DrawCaptureAreaAndUpdateUserData(lineX1, lineY1, line.get_x2Limit(), line.get_y2Limit());
                }
            }
            //remove line from canvas and remove line event handlers 
            gameCanvas.Children.Remove(line);
            line.DrawLineEnded -= dl_DrawLineEnded;
            line.CheckCollapse -= DrawLine_CheckCollapse;

            LineIsBeingDrawn = false;
            
            //check if need to pass next level
            if (UserReachedMaxCaptureAreaPrecentage)
            {
                AddNewBallToScreen();
                UpdatePrecentageToPass();
                UserReachedMaxCaptureAreaPrecentage = false;
            }
        }
       /**
        * Draw capture area  based on a given point left top and right botoom point
        * and update relevant data like precentges and num of points and etc.
        */
        public void DrawCaptureAreaAndUpdateUserData(double x1, double y1, double x2, double y2)
        {
            DrawRect rect = new DrawRect(gameCanvas, new Point(x1, y1), new Point(x2, y2));
            gameCanvas.Children.Add(rect);
            //update cuptared area 
            TotalCapturedArea += rect.getArea();
            //caculate points
            TotalUserPoints += CaculateFactorPoints(rect.getArea() / 100);
            PointTab.Text = (TotalUserPoints.ToString());
            int PrectanageCompleted = ((int)(TotalCapturedArea / CanvasTotalArea * 100));
            // update precetnage completed 
            //PrectengeCaculateBlock.Text = PrectanageCompleted.ToString();
            PrecentegeTabToPass.Text = (PrecentageToPassNextLevel - PrectanageCompleted).ToString();
            if (PrecentageToPassNextLevel - PrectanageCompleted <= 50)
            {
                Precentage.Foreground = Brushes.Yellow;
                PrecentegeTabToPass.Foreground = Brushes.Yellow;
            }
            if (PrecentageToPassNextLevel - PrectanageCompleted <= 20)
            {
                Precentage.Foreground = Brushes.Green;
                PrecentegeTabToPass.Foreground = Brushes.Green;
            }
            //check user should pass next level
            if ((int)(TotalCapturedArea / CanvasTotalArea * 100) >= PrecentageToPassNextLevel)
            {

                UserReachedMaxCaptureAreaPrecentage = true;
                Precentage.Foreground = Brushes.Red;
               PrecentegeTabToPass.Foreground = Brushes.Red;
            }
        }
        //update precentage needed to pass to next level
        private void UpdatePrecentageToPass()
        {
            if (listOfBalls.Count == 4)
            {
                PrecentageToPassNextLevel = 85;
                PrecentegeTabToPass.Text = PrecentageToPassNextLevel.ToString();
            }
            else if (listOfBalls.Count == 8)
            {
                PrecentageToPassNextLevel = 80;
                PrecentegeTabToPass.Text = PrecentageToPassNextLevel.ToString();
            }
            else if (listOfBalls.Count == 11)
            {
                PrecentageToPassNextLevel = 75;
                PrecentegeTabToPass.Text = PrecentageToPassNextLevel.ToString();
            }
            else if (listOfBalls.Count == 14)
            {
                PrecentageToPassNextLevel = 70;
                PrecentegeTabToPass.Text = PrecentageToPassNextLevel.ToString();
            }
        }
    /* get the points user earned 
    * return the points or boosted points 
    */
    public int CaculateFactorPoints(double points)
        {
            if (points >= 0)
            {
                if (NumberOfUserClicks <= 1)
                    return (int)(points * 1.5);
                else if (NumberOfUserClicks <= 3 && NumberOfUserClicks > 1)
                    return (int)(points * 1.4);
                else if (NumberOfUserClicks <= 6 && NumberOfUserClicks > 3)
                    return (int)(points * 1.3);
                else if (NumberOfUserClicks <= 10 && NumberOfUserClicks > 6)
                    return (int)(points * 1.2);
                else
                    return (int)points;
            }
            else
                return 0;
        }

     
     
      
        //random that meant to make the x and y delta positive or nagative
        Random rndX = new Random();
        Random rndY = new Random();
        //randoms to value delta x and y
        Random rndNumsX =new Random();
        Random rndNumsY = new Random();

        //add new ball to the game canvas
        //and reset all the instances affected by the balls
        public void AddNewBallToScreen()
        {
            // the deltas of x and y
            double dx;
            double dy;
                       

            //clear canvas and removing canvas chlidren
            for (int i= 0; i < gameCanvas.Children.Count; i++)
            {
                gameCanvas.Children.Remove(gameCanvas.Children[i]);
            }
            gameCanvas.Children.Clear();
         
            int nX = rndX.Next(1, 3);  
            //check positive or nagative for dx 
            if (nX == 1)
                dx = rndNumsX.NextDouble() * 3 + 2;
            else
                dx = rndNumsX.NextDouble() * -3- 2;
               
           int nY = rndY.Next(1, 3);
            //check positive or nagative for dy
            if (nY == 1)
                dy = rndNumsX.NextDouble() * 3 + 2;
            else
                dy = rndNumsX.NextDouble() * -3 - 2;
            //add new ball
             listOfBalls.Add(new Recetengle_Ball(gameCanvas, dx, dy));

            for (int i = 0; i < listOfBalls.Count; i++)
            {
                //reset all balls that removed
                if (i<listOfBalls.Count-1)
                   listOfBalls[i].ResetParametres();
                //start the new ball timer
                else
                    listOfBalls[i].Timer.Start();

            }
            //reset instances      
            //PrectengeCaculateBlock.Text = "0" ;
            PrecentegeTabToPass.Text = PrecentageToPassNextLevel.ToString();
            NumberOfUserClicks = 0;
            gameCanvas.Cursor = Cursors.UpArrow;
            IsVerticalLineDirection = true;
            TotalCapturedArea = 0;
        }

        /**
         * handle user exit game
         */
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //switch user control to entry screen
            Window.GetWindow(this).DataContext = MainWindow.entryWindowViewModel;
        }
    }
}
