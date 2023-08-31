using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlayData 
{
    // ------ ここシングルトンのためのコード --------------
    private static GamePlayData gamePlayData = new GamePlayData();

    private GamePlayData() // privateのコンストラクタ
    {

    }

    public static GamePlayData GetInstance()
    {
        return gamePlayData;
    }
    // ------ ここシングルトンのためのコード --------------




    public int _totalMedal { set; get; }          // 総払い出し枚数
    public int _previousCreditMedal { set; get; } // 払出前のクレジット（Bet時点の）
    public int _creditMedal { set; get; }         // クレジット枚数
    public int _currentInMedal { set; get; }      // in枚数（UI表示したりに使う？）
    public int _currentOutMedal { set; get; }     // out枚数（UI表示したりに使う？）
    public int _bonusPayOutCounter { set; get; }    // ボーナス消化カウンター

    public int _BBCount { set; get; } // BB回数
    public int _RBCount { set; get; } // RB回数


    public int _totalGameCount { set; get; }   // 総回転数
    public int _betweenGameCount { set; get; } // 現在のゲーム数（ボーナス当選でリセット）

    // ★★　小役の入賞回数をカウントするにはどうするか
    // public List<int> _nyuusyouCastCounts_int = new List<int>(); // 入賞役
    public List<ICastBase> _nyuusyouCast_ICastBase = new List<ICastBase>(); // 入賞役
    public IBonusBase _currentNyuusyouBonus;



    // Cast_BBなどをIBonusBaseにキャスト
    public void InterfaceCast(ICastBase cast)
    {
        _currentNyuusyouBonus = (IBonusBase)cast;
        Debug.Log("IBonusBaseにキャストしました");
    }


    public void ValueInitialize()
    {
        _totalMedal = 50;
        _previousCreditMedal = 0;
        _creditMedal = 30;
        _currentInMedal = 0;
        _currentOutMedal = 0;
        _BBCount = 0;
        _RBCount = 0;
        _totalGameCount = 0;
        _betweenGameCount = 0;
    }

    // クレジットを 50から０ の範囲に抑える
    public void Check_CreditRange()
    {
        // 50以上なら
        if(_creditMedal >= 51)
        {
            _creditMedal = 50;
        }

        if(_creditMedal < 0)
        {
            _creditMedal = 0;
        }
    }

}




