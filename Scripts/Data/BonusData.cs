using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Listの最後を取得するため

public class BonusData
{
    // uint bonusFlagBit = 0b0001; // 右から非成立,RB,BB,予備

    bool _canBonusNyuusyou { set; get; }   // ボーナス入賞可能か？フラグ（成立しているか）
    bool _isBonusDigestion { set; get; } // ボーナス消化中フラグ

    public IBonusState _currentBonusState;

    // int _bonusPayOut_TotalCount { set; get; }  // ボーナス払い出し枚数管理

    IBonusBase _currentDigesingBonus { set; get; } // 現在消化中のボーナスクラスを格納するための変数
                                                   // これにsetする時はBB_Cast型かRB_Cast型で代入する たぶんICastBaseでは入らない


    // ------ ここシングルトンのためのコード --------------
    private static BonusData bonusData = new BonusData();

    private BonusData() // privateのコンストラクタ
    {

    }

    public static BonusData GetInstance()
    {
        return bonusData;
    }
    // ------ ここシングルトンのためのコード --------------





    // 外部からボーナス入賞状態boolをSetするメソッド
    public void Set_CanBonusNyuusyou(bool canBonusNyuusyou)
    {
        _canBonusNyuusyou = canBonusNyuusyou;
    }

    // 外部からボーナス入賞状態boolをGetするメソッド
    public bool Get_CanBonusNyuusyou()
    {
        return _canBonusNyuusyou;
    }



    // 外部からボーナス入賞状態boolをSetするメソッド
    public void Set_IsBonusDigestion(bool isBonusDigestion)
    {
        _isBonusDigestion = isBonusDigestion;
    }

    // 外部からボーナス入賞状態boolをGetするメソッド
    public bool Get_IsBonusDigestion()
    {
        return _isBonusDigestion;
    }



    public void BonusGame_Start()
    {
        Debug.Log("BonusDataからボーナス入賞しました");
        _canBonusNyuusyou = false; // 入賞待ち終了
        _isBonusDigestion = true;  // 消化中フラグ開始
        // _bonusPayOut_TotalCount = 0; // 初期化
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        _currentDigesingBonus = gamePlayData._currentNyuusyouBonus; // 入賞したボーナスを格納
        gamePlayData._betweenGameCount = 0; // ボーナス間G数を初期化

        // ボーナス回数追加
        if(_currentDigesingBonus.GetBonusType() == "BB")
        {
            gamePlayData._BBCount++;
        }
        else if(_currentDigesingBonus.GetBonusType() == "RB")
        {
            gamePlayData._RBCount++;
        }
        else
        {
            Debug.LogError("ボーナスType が該当しません");
        }
    }

    public void BonusGame_End()
    {
        Debug.Log("BonusDataからボーナスゲーム終了します");
        _isBonusDigestion = false;  // 消化中フラグ開始
        // _bonusPayOut_TotalCount = 0; // 初期化
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        _currentDigesingBonus = null; // 初期化
    }
}

// 通常時
class NormalTime : IBonusState
{
    public bool CanBonusNyuusyou() { return false; }
    public bool IsBonusDigestion() { return false; }
    public void ChangeState(BonusData owner)
    {
        IBonusState bonusAlignWait = new BonusAlignWait();
        owner._currentBonusState = bonusAlignWait;
    }
}

// 入賞待ち
class BonusAlignWait : IBonusState
{
    public bool CanBonusNyuusyou() { return true; }
    public bool IsBonusDigestion() { return false; }
    public void ChangeState(BonusData owner)
    {
        IBonusState bonusRound = new BonusRound();
        owner._currentBonusState = bonusRound;
    }
}

// ボーナス消化中
class BonusRound : IBonusState
{
    public bool CanBonusNyuusyou() { return false; }
    public bool IsBonusDigestion() { return true; }
    public void ChangeState(BonusData owner)
    {
        IBonusState normalTime = new NormalTime();
        owner._currentBonusState = normalTime;
    }
}

// インターフェイス
public interface IBonusState
{
    public bool CanBonusNyuusyou();
    public bool IsBonusDigestion();
    public void ChangeState(BonusData owner);
}