using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static bool isSinglePlayer = false;
    public static bool playerGoFirst = true;
    public static bool firstPlayer = true;

    public static string[] characterSelection = { "berryMuffin", "chipMuffin", "lemonMuffin", "chocolateCupcake", "redCupcake", "whiteCupcake" }; //change for names of characters later
    public static string characterLocalPlayer = ""; 
    public static string characterOpponentPlayer = "";
    public GameObject[] Pieces;
    private int[] aiLocations = new int[24];
    private static int y;

    public int from;
    public int to;
    public int remove;
    public bool aiMadeMove;

    //AI levelset on single player menu
    public static string difficultyLevel = "easy"; 
    public static int movesSinceLastMillFormed = 0;


    void Awake()
    {
        movesSinceLastMillFormed = 0;
        aiMadeMove = false;
    }


    public int[] getAIMove(BitArray computer, BitArray player)
    {
        int[] move = new int[3];
        move = AI.move(player, computer, App.phase, difficultyLevel);
        return move;
    }
    
    public void getThreadedAIMove(BitArray computer, BitArray player)
    {
        Debug.Log("#2 begin");
        int[] move = new int[3];
        move = AI.move(player, computer, App.phase, difficultyLevel);
        from = move[0];
        to = move[1];
        remove = move[2];
        aiMadeMove = true;
        Debug.Log("#2 end");
    }

}

