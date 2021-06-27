using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


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
            Application.Run(new MyWindow());
        }
    }
    class Board{
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
            Invalidate();
        }
        protected override void OnKeyDown(KeyEventArgs e){
            if ((e.KeyCode > Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode > Keys.NumPad0 && e.KeyCode < Keys.NumPad9))
                board.rows[currClicked.X / 50, currClicked.Y /50] = ((int)(e.KeyCode.ToString()[1]) - '0');
            if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                board.rows[currClicked.X / 50, currClicked.Y /50] = 0;
            Invalidate();
        }
        //valid makes sure not mistake
        public bool valid(int input, int x, int y){
            for(int i = 0; i < 9; i++){
                //checks if double number in column
                if(y != i && (board.rows[x, i] == input || levels.levelList[levels.currLevel][x,i] == input))
                    return false;
                //checks if double number in row
                if(x != i && (board.rows[i, y] == input || levels.levelList[levels.currLevel][i,y] == input))
                    return false;
                //checks if same number in same square(3x3 block) of board-generated numbers
                if((levels.levelList[levels.currLevel][x-(x%3)+(i%3), (y-(y%3)+(i/3))] == input))
                    return false;
            }
            //checks if same number in same square(3x3 block) of inputed numbers
            for(int i = 0; i < 3; i++){
                for(int j = 0; j<3; j++){
                    if(i == x%3 && j == y%3)
                        continue;
                    if(board.rows[(x/3)*3 + i, (y/3)*3 + j] == input)
                        return false;
                }
            }
            return true;
        }
        public bool gameFinished(){
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    if(board.rows[i,j] == 0 && levels.levelList[levels.currLevel][i,j] == 0)
                        return false;
                    if(board.rows[i,j] != 0 && !valid(board.rows[i,j], i, j))
                        return false;
                    if(levels.levelList[levels.currLevel][i,j] != 0 && !valid(levels.levelList[levels.currLevel][i,j], i, j))
                        return false;
                }
            }
            return true;
        }
        protected override void OnPaint(PaintEventArgs args) {
            Graphics g = args.Graphics;
            Pen pen;
            //adding lines on board
            for(int i = 0; i <= 9; i++){
                if(i % 3 == 0)
                    pen = new Pen(Color.Black, 5);
                else
                    pen = new Pen(Color.Black, 1);
                g.DrawLine(pen, new Point(50*i, 0), new Point(50*i, 450));
                g.DrawLine(pen, new Point(0, 50*i), new Point(450, 50*i));
            }
            //highlight currclicked box
            g.FillRectangle(new SolidBrush(Color.FromArgb(60, 0, 0, 255)), new Rectangle((currClicked.X / 50)*50 ,(currClicked.Y / 50)*50, 50, 50));
            if(gameFinished()){
                levels.currLevel++;
                board = new Board();
            }
            //adds numbers
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    if(levels.levelList[levels.currLevel][i,j] != 0){
                        g.DrawString(levels.levelList[levels.currLevel][i,j].ToString(), new Font("Times New Roman", 35, FontStyle.Bold), Brushes.Black, new PointF(i*50 + 6, j*50 -1));

                    }
                    if(board.rows[i,j] != 0){
                        Brush b;
                        if(valid(board.rows[i,j], i, j))
                            b = Brushes.Black;
                        else
                            b = Brushes.Red;
                        g.DrawString(board.rows[i,j].ToString(), new Font("Times New Roman", 35), b, new PointF(i*50 + 6, j*50 -1));
                    }
                }
            }
            
        }

    }
}
