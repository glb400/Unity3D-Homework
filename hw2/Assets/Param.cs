using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Param : MonoBehaviour
{
    //chessboard
    public static int[,] board = new int[3, 3];
    //1 for Player 1 & 0 for Player 2 
    public static int turn = 1;
    //0 represents to be continued，1 represents Player1 wins，2 represents Player2 wins
    public static int result = 0;
    //rst represents whether to reset
    public static int rst = 0;
    //cnt == 9 means a draw
    public static int cnt = 0;
}