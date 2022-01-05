using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotRepeatsRandom : IGameRandom
{
    public int GetValue(int start, int end)
    {
        return Random.Range(start, end);
    }
}
