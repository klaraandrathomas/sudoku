using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
//using System.Windows.Input;


namespace sudoku
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Application.SetHighDpiMode(HighDpiMode.SystemAware);
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1());
            Application.Run(new MyWindow());
        }
    }
    class Board{

        public int[,] squares = new int[9,9];
        public int[,] rows = new int[9,9];

    }
    class MyWindow : Form {
        Board board = new Board(); //each small square is 50 pixels- total size = 450 pixels
        Levels levels = new Levels();
        Point currClicked = new Point(0,0);

        public MyWindow(){
            Text = "sudoku";
            ClientSize = new Size(450,450);
            StartPosition = FormStartPosition.CenterScreen;
        }
        protected override void OnMouseDown(MouseEventArgs args){
            currClicked = args.Location;
        }
        protected override void OnKeyDown(KeyEventArgs e){
            if ((e.KeyCode > Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode > Keys.NumPad0 && e.KeyCode < Keys.NumPad9)){
                board.rows[currClicked.X / 50, currClicked.Y /50] = (System.Convert.ToInt32(e.KeyCode.ToString().ToCharArray()[1]) - 48);
                //board.squares[(currClicked.X/3)+((currClicked.Y/3)*3), (currClicked.X%3)+((currClicked.Y%3)*3)] = (System.Convert.ToInt32(e.KeyCode.ToString().ToCharArray()[1]) - 48);
            }
            if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete){
                board.rows[currClicked.X / 50, currClicked.Y /50] = 0;
                //board.squares[(currClicked.X/3)+((currClicked.Y/3)*3), (currClicked.X%3)+((currClicked.Y%3)*3)] = 0;
            }
            Invalidate();
        }
        public bool valid(int input, int x, int y){
            for(int i = 0; i < 9; i++){
                if(y != i && (board.rows[x, i] == input || levels.levelList[levels.currLevel][x,i] == input))
                    return false;
                if(x != i && (board.rows[i, y] == input || levels.levelList[levels.currLevel][i,y] == input))
                    return false;
                if(/*(idk figure this out later) &&*/(board.squares[(x/3)+((y/3)*3), i] == input || levels.levelList[levels.currLevel][x-(x%3)+(i%3), (y-(y%3)+(i/3))] == input))
                    return false;
            }
            return true;
        }
        public bool gameFinished(){
            //need to include if numbers are red
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    if(board.rows[i,j] == 0 && levels.levelList[levels.currLevel][i,j] == 0)
                        return false;
                }
            }
            return true;
        }
        protected override void OnPaint(PaintEventArgs args) {
            Graphics g = args.Graphics;
            Pen pen;
            for(int i = 0; i <= 9; i++){ //adding lines on board
                if(i % 3 == 0)
                    pen = new Pen(Color.Black, 5);
                else
                    pen = new Pen(Color.Black, 1);
                g.DrawLine(pen, new Point(50*i, 0), new Point(50*i, 450));
                g.DrawLine(pen, new Point(0, 50*i), new Point(450, 50*i));
            }
            //highlight currclicked box
            if(gameFinished()){
                levels.currLevel++;
                board = new Board();
            }
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    if(levels.levelList[levels.currLevel][i,j] != 0){
                        g.DrawString(levels.levelList[levels.currLevel][i,j].ToString(), new Font("Times New Roman", 35, FontStyle.Bold), Brushes.Black, new PointF(i*50 + 5, j*50 -1));

                    }
                    if(board.rows[i,j] != 0 && valid(board.rows[i,j], i, j)){//valid makes sure not mistake
                        g.DrawString(board.rows[i,j].ToString(), new Font("Times New Roman", 35), Brushes.Black, new PointF(i*50 + 5, j*50 -1));
                    }
                    if(!valid(board.rows[i,j], i, j) && board.rows[i,j] != 0 ){
                        g.DrawString(board.rows[i,j].ToString(), new Font("Times New Roman", 35), Brushes.Red, new PointF(i*50 + 5, j*50 -1));
                    }
                }
            }
            
        }

    }
}
