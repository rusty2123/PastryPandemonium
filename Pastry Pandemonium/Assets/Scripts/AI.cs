using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class AI : MonoBehaviour
{
#if UNITY_EDITOR
    internal const string _dllVersion = "AILibrary_x64";
#endif

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    internal const string _dllVersion = "AILibrary_x86";
#endif

    [DllImport(_dllVersion, CharSet = CharSet.Unicode)]
    static extern void getAIMove(int[] setPlayer, int[] setComputer, int phase, int difficulty, ref int from, ref int to, ref int remove);

    [DllImport(_dllVersion)]
    public static extern bool getDraw();

    public static int[] move(BitArray playerBoard, BitArray computerBoard, int stage, string difficulty)
    {
        int[] response = new int[4];
        int[] playerBoardInt = new int [24];
        int[] computerBoardInt = new int[24];
        int to = 0, from = 0, remove = 0;
        int difficultyInt = difficulty == "easy" ? 0 : 2;

        for (int i = 0; i < 24; i++)
        {
            playerBoardInt[i] = (playerBoard[i] ? 1 : 0);
            computerBoardInt[i] = (computerBoard[i] ? 1 : 0);
        }

        getAIMove(playerBoardInt, computerBoardInt, stage, difficultyInt, ref from, ref to, ref remove);

        // move is a 0-23 scale, not a 1-24
        response[0] = from - 1;
        response[1] = to - 1;
        response[2] = remove - 1;

        return response;
    }
}
