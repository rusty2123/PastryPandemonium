using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eppy;



public class MoveLookup : MonoBehaviour {


     public List<Tuple<int, int>> Moves = new List<Tuple<int, int>>()
    {
        Tuple.Create(1, 2),
        Tuple.Create(1, 10),
        Tuple.Create(2, 1),
        Tuple.Create(2, 5),
        Tuple.Create(2, 3),
        Tuple.Create(3, 2),
        Tuple.Create(3, 15),
        Tuple.Create(4, 5),
        Tuple.Create(4, 11),
        Tuple.Create(5, 4),
        Tuple.Create(5, 8),
        Tuple.Create(5, 6),
        Tuple.Create(6, 5),
        Tuple.Create(6, 14),
        Tuple.Create(7, 8),
        Tuple.Create(7, 12),
        Tuple.Create(8, 7),
        Tuple.Create(8, 9),
        Tuple.Create(9, 8),
        Tuple.Create(9, 13),
        Tuple.Create(10, 1),
        Tuple.Create(10, 11),
        Tuple.Create(10, 22),
        Tuple.Create(11, 4),
        Tuple.Create(11, 10),
        Tuple.Create(11, 12),
        Tuple.Create(11, 19),
        Tuple.Create(12, 7),
        Tuple.Create(12, 11),
        Tuple.Create(12, 16),
        Tuple.Create(13, 9),
        Tuple.Create(13, 14),
        Tuple.Create(13, 18),
        Tuple.Create(14, 6),
        Tuple.Create(14, 13),
        Tuple.Create(14, 15),
        Tuple.Create(14, 21),
        Tuple.Create(15, 3),
        Tuple.Create(15, 14),
        Tuple.Create(15, 24),
        Tuple.Create(16, 12),
        Tuple.Create(16, 17),
        Tuple.Create(17, 16),
        Tuple.Create(17, 18),
        Tuple.Create(17, 20),
        Tuple.Create(18, 13),
        Tuple.Create(18, 17),
        Tuple.Create(19, 11),
        Tuple.Create(19, 20),
        Tuple.Create(20, 17),
        Tuple.Create(20, 19),
        Tuple.Create(20, 21),
        Tuple.Create(20, 23),
        Tuple.Create(21, 14),
        Tuple.Create(21, 20),
        Tuple.Create(22, 10),
        Tuple.Create(22, 23),
        Tuple.Create(23, 20),
        Tuple.Create(23, 22),
        Tuple.Create(23, 24),
        Tuple.Create(24, 15),
        Tuple.Create(24, 23)
    };

    public List<Tuple<int, int, int>> Mills = new List<Tuple<int, int, int>>()
    {
        Tuple.Create(1, 2, 3),
        Tuple.Create(1, 10, 22),
        Tuple.Create(2, 1, 3),
        Tuple.Create(2, 5, 8),
        Tuple.Create(3, 1, 2),
        Tuple.Create(3, 15, 24),
        Tuple.Create(4, 5, 6),
        Tuple.Create(4, 11, 19),
        Tuple.Create(5, 4, 6),
        Tuple.Create(5, 2, 8),
        Tuple.Create(6, 4, 5),
        Tuple.Create(6, 14, 21),
        Tuple.Create(7, 8, 9),
        Tuple.Create(7, 12, 16),
        Tuple.Create(8, 7, 9),
        Tuple.Create(8, 2, 5),
        Tuple.Create(9, 7, 8),
        Tuple.Create(9, 13, 18),
        Tuple.Create(10, 11, 12),
        Tuple.Create(10, 1, 22),
        Tuple.Create(11, 10, 12),
        Tuple.Create(11, 4, 19),
        Tuple.Create(12, 10, 11),
        Tuple.Create(12, 7, 16),
        Tuple.Create(13, 14, 15),
        Tuple.Create(13, 9, 18),
        Tuple.Create(14, 13, 15),
        Tuple.Create(14, 6, 21),
        Tuple.Create(15, 13, 14),
        Tuple.Create(15, 3, 24),
        Tuple.Create(16, 17, 18),
        Tuple.Create(16, 7, 12),
        Tuple.Create(17, 16, 18),
        Tuple.Create(17, 20, 23),
        Tuple.Create(18, 16, 17),
        Tuple.Create(18, 9, 13),
        Tuple.Create(19, 20, 21),
        Tuple.Create(19, 4, 11),
        Tuple.Create(20, 19, 21),
        Tuple.Create(20, 17, 23),
        Tuple.Create(21, 19, 20),
        Tuple.Create(21, 6, 14),
        Tuple.Create(22, 23, 24),
        Tuple.Create(22, 1, 10),
        Tuple.Create(23, 22, 24),
        Tuple.Create(23, 17, 20),
        Tuple.Create(23, 22, 23),
        Tuple.Create(23, 3, 15),
    };



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
