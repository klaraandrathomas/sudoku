using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;
class Levels{
    public int currLevel;
    public List<int[,]> levelList;
    public Levels(){
        currLevel = 0;
        levelList = new List<int[,]>();
        StreamReader reader = new StreamReader("p096_sudoku.txt");
        for(int i = 0; i < 50; i++){
            reader.ReadLine();
            int[,] temp = new int[9,9];
            for(int j = 0; j < 9; j++){
                char[] s = reader.ReadLine().ToCharArray();
                for(int k = 0; k < 9; k++){
                    temp[j,k] = int.Parse(s[k].ToString());
                }
            }
            levelList.Add(temp);
        }
    }
}