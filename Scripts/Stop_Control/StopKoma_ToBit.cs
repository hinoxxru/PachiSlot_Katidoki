using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopKoma_ToBit : MonoBehaviour
{
    int _leftKoma = 1;
    int _centerKoma = 4;
    int _rightKoma = 32;

    uint bit = 0b000000010000010000100000;

    // Start is called before the first frame update
    void Start()
    {
        int totalBit;
        totalBit = _leftKoma;
        totalBit = totalBit << 8;
        totalBit += _centerKoma;
        totalBit = totalBit << 8;
        totalBit += _rightKoma;
        Debug.Log("int型で表示すると " + totalBit);
        Debug.Log("2進数で表示すると " + Convert.ToString(totalBit, 2));

        if (bit == totalBit)
        {
            Debug.Log("等しい！");
        }
        else
        {
            Debug.Log("等しくないよ");
        }
    }
}
