using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 配当データを保持するクラス
/// </summary>
public class CastHaitouData 
{
    // public string _currentCastHaitou { set; get; } // プロパティ
    Dictionary<int, int> haitouList = new Dictionary<int, int>()
    {
        { 0, 0  }, // ハズレ
        { 1, 0  }, // リプレイ 払い出し０→MaxBetState飛ばしたい
        { 2, 10 }, // ベル
        { 4, 6  }, // スイカ
        { 8, 2  }, // チェリー
        {16, 0  }, // RB入賞
        {32, 0  }  // BB入賞
    }; 


    private static CastHaitouData castHaitouData = new CastHaitouData();

    private CastHaitouData() // privateのコンストラクタ
    {

    }

    public static CastHaitouData GetInstance()
    {
        return castHaitouData;
    }


}

