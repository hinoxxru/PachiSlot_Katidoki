using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Listの最後を取得するため

public class NyuusyouCheck 
{
    // string _currentFlag;

    int[] _currentDemeInt = new int[9]; //  左　　中　　右
                                        //  0    3    6
                                        //  1    4    7
                                        //  2    5    8

    


    List<int> _currentAllDemeBits = new List<int>();


    /// <summary>
    /// DemeDataから出目intを取得
    /// </summary>
    void GetDemeData()
    {
        DemeData demeData = DemeData.GetInstance();
        // 左リール
        var tuple = demeData.GetDemeDeta_int_tuple("ReelLeftObj");
        _currentDemeInt[0] = tuple.jyoudann;
        _currentDemeInt[1] = tuple.tyuudann;
        _currentDemeInt[2] = tuple.gedann;

        // 中リール
        tuple = demeData.GetDemeDeta_int_tuple("ReelCenterObj");
        _currentDemeInt[3] = tuple.jyoudann;
        _currentDemeInt[4] = tuple.tyuudann;
        _currentDemeInt[5] = tuple.gedann;

        // 右リール
        tuple = demeData.GetDemeDeta_int_tuple("ReelRightObj");
        _currentDemeInt[6] = tuple.jyoudann;
        _currentDemeInt[7] = tuple.tyuudann;
        _currentDemeInt[8] = tuple.gedann;
    }

    /// <summary>
    /// 成立出目を全５ライン分、リストに入れる
    /// </summary>
    void AddList_CurrentAllDemeBits()
    {
        _currentAllDemeBits.Add(JointDemeBit(1, 4, 7)); // 1ライン　中段
        _currentAllDemeBits.Add(JointDemeBit(0, 3, 6)); // 2ライン　上段
        _currentAllDemeBits.Add(JointDemeBit(2, 5, 8)); // 3ライン　下段
        _currentAllDemeBits.Add(JointDemeBit(0, 4, 8)); // 4ライン　右下がり
        _currentAllDemeBits.Add(JointDemeBit(2, 4, 6)); // 5ライン　右上がり
    }

    int JointDemeBit(int leftNum, int CenterNum, int rightNum)
    {
        int jointBit;
        jointBit = _currentDemeInt[leftNum];
        // Debug.Log("左リールのBitは " + _currentDemeInt[leftNum].ToString());
        jointBit = jointBit << 8;
        jointBit += _currentDemeInt[CenterNum];
        jointBit = jointBit << 8;
        jointBit += _currentDemeInt[rightNum];
        return jointBit;
    }

    // 入賞チェック実行
    public bool Call_NyuusyouCheck()
    {
        GetDemeData();
        AddList_CurrentAllDemeBits(); //　出目情報のbitセット
        bool isNyuusyou = false; // 初期化

        GamePlayData gamePlayData = GamePlayData.GetInstance();
        // gamePlayData._currentNyuusyouCast.Clear(); // 入賞役管理用のリスト初期化

        // フラグ情報はFlagLotteryでICastBaseを保持しているのでそこに参照する
        FlagLottery flagLottery = GameObject.Find("ScriptObj").GetComponent<FlagLottery>();


        foreach(int demeBit in _currentAllDemeBits)
        {
            // FlagLotteryにあるICastBase型を参照する
            foreach(ICastBase cast in flagLottery._castList)
            {
                if(cast.GetCastBit() == demeBit)
                {
                    Debug.Log("入賞したぞ ");
                    // gamePlayData._nyuusyouCastCounts_int.Add(cast.GetCastNumber()); // 入賞役をリストに追加（全ライン検証するので複合役もAddするハズ）
                    gamePlayData._nyuusyouCast_ICastBase.Add(cast);
                    gamePlayData._currentOutMedal = cast.GetPayOutMedal(); // 払出枚数をセット
                    isNyuusyou = true;
                }
            }
        }


        // 小役入賞ありだったら → ボーナス入賞チェックを行う
        if (isNyuusyou)
        {
            BonusNyuusyouCheck();
        }


        // 小役入賞なしだったら
        if (!isNyuusyou)
        {
            gamePlayData._currentOutMedal = 0; // 入賞してないから 0 枚でセット
            ICastBase cast = flagLottery._castList[0]; // ハズレCast
            gamePlayData._nyuusyouCast_ICastBase.Add(cast);
        }

        return isNyuusyou;
    }


    /// <summary>
    /// ボーナスが入賞しているかどうか
    /// </summary>
    /// <returns></returns>
    void BonusNyuusyouCheck()
    {
        Debug.Log("Bonus入賞チェック行います");

        GamePlayData gamePlayData = GamePlayData.GetInstance();
        // 型を比較。 Bonusのインターフェイスがあるか

        if (gamePlayData._nyuusyouCast_ICastBase.Last().GetCastIsBonus()) // Castが持っている情報がBonusなら
        {
            gamePlayData.InterfaceCast(gamePlayData._nyuusyouCast_ICastBase.Last()); // 入賞役をIBonusBaseへ変換し保持

            BonusData bonusData = BonusData.GetInstance();
            bonusData._currentBonusState.ChangeState(bonusData);
            bonusData.BonusGame_Start();
            // bonusData.Set_CanBonusNyuusyou(false); // 入賞したので「入賞可能状態」をfalseへ
            Debug.Log("Bonusが入賞しました");
        };
    }


}
