using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Func : MonoBehaviour
{
    public static void Reset()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Param.board[i, j] = 0;
        Param.cnt = 0;
        Param.rst = 1;
    }

    public static bool IsFinish()
    {
        if (Param.result == 1 | Param.result == 2 | Param.cnt == 9)
            return true;

        Param.turn = 3 - Param.turn;

        //represent the whole final state space
        for (int i = 0; i < 3; i++)
        {
            if ((Param.board[i, 0] == Param.board[i, 1]) &&
                (Param.board[i, 0] == Param.board[i, 2]) &&
                (Param.board[i, 0] != 0))
            {
                Param.result = Param.board[i, 0];
                return true;
            }
            if ((Param.board[0, i] == Param.board[1, i]) &&
                (Param.board[0, i] == Param.board[2, i]) &&
                (Param.board[0, i] != 0))
            {
                Param.result = Param.board[0, i];
                return true;
            }
        }

        if ( ((Param.board[0, 0] == Param.board[1, 1]) &&
            (Param.board[0, 0] == Param.board[2, 2]) &&
            (Param.board[1, 1] != 0)) |
            ((Param.board[2, 0] == Param.board[1, 1]) &&
            (Param.board[2, 0] == Param.board[0, 2]) &&
            (Param.board[1, 1] != 0)) )
        {  
            Param.result = Param.board[1, 1];
            return true;
        }

        return false;
    }
}
