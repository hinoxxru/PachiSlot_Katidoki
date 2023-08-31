using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class FlagLottery : MonoBehaviour
{
    int _lottreyNumber;  // 乱数
    int _settingNumber = 6; // 設定


    ///// ここまだ実験段階
    // 各種フラグ派生クラス
    ICastBase cast_Hazure = new Cast_Hazure();
    ICastBase cast_Replay = new Cast_Replay();
    ICastBase cast_Bell = new Cast_Bell();
    ICastBase cast_Suika = new Cast_Suika();
    ICastBase cast_Cherry = new Cast_Cherry();
    ICastBase cast_RB = new Cast_RB();
    ICastBase cast_BB = new Cast_BB();
    ///// ここまだ実験段階

    public ICastBase[] _castList = new ICastBase[7];

    List<string[]> _lotteryDatas_NormalTime = new List<string[]>(); // CSVの中身を入れるリスト;
    List<string[]> _lotteryDatas_BonusRound = new List<string[]>(); // CSVの中身を入れるリスト;

    private void Start()
    {
        // 配列にいれ無いと抽選が大変
        _castList[0] = cast_Hazure;
        _castList[1] = cast_Replay;
        _castList[2] = cast_Bell;
        _castList[3] = cast_Suika;
        _castList[4] = cast_Cherry;
        _castList[5] = cast_RB;
        _castList[6] = cast_BB;

        LoadCSV_ToRecourses("LotteryProbabilityCSV_NomalTIme", _lotteryDatas_NormalTime); // ボーナス非成立時の抽選
        LoadCSV_ToRecourses("LotteryProbabilityCSV_BonusRound", _lotteryDatas_BonusRound); // ボーナス非成立時の抽選
        // LotteryProbabilityCSV_BonusRound

        //TextAsset _csvFile; // CSVファイル
        //string fileName = "WinningProbabilityCSV";
        //_csvFile = Resources.Load(fileName) as TextAsset; // Resouces下のCSV読み込み
        //StringReader reader = new StringReader(_csvFile.text);



        //// ★★★ここは制御表用の読み込みだから直す

        //// , で分割しつつ一行ずつ読み込み
        //// リストに追加していく
        //while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        //{
        //    string line = reader.ReadLine(); // 一行ずつ読み込み
        //    _lotteryDatas.Add(line.Split(',')); // , 区切りでリストに追加
        //}

        //// csvDatas[フラグ][設定]を指定して値を自由に取り出せる
        //// Debug.Log(_lotteryDatas[0][_settingNumber]);

    }

    public string CallLottrey()
    {

        int castIndex = 99; // これが改良後処理異常の場合は99のまま

        FlagData flagData = FlagData.GetInstance();
        ICastBase currentCast;

        _lottreyNumber = Random.Range(0, 65535); // フラグ抽選（乱数生成）
        // Debug.Log("レバーオンで引いた数値は " + _lottreyNumber);


        // ボーナス消化中か確認
        BonusData bonusData = BonusData.GetInstance();
        Debug.Log("フラグ抽選 Bonus状態確認 " + bonusData._currentBonusState);

        List<string[]> lotteryDatas;
        if (!bonusData.Get_IsBonusDigestion()) // ボーナス消化中じゃなかったら
        {
            lotteryDatas = _lotteryDatas_NormalTime;
        }
        else if(bonusData.Get_IsBonusDigestion()) // ボーナス消化中だったら
        {
            lotteryDatas = _lotteryDatas_BonusRound;
        }
        else
        {
            lotteryDatas = null;
            Debug.LogError("ボーナスの状態が不明です");
        }

        // 以下、小役抽選だけどメソッドにするのもあり
        for (int i = 0; i < lotteryDatas.Count(); i++)
        {

            int subtraction = int.Parse(lotteryDatas[i][_settingNumber]); // int型にキャスト  [小役index(行)][設定(列)]
            _lottreyNumber -= subtraction; // 減算方式抽選

            if (_lottreyNumber <= 0)
            {
                castIndex = int.Parse(lotteryDatas[i][0]); // 要素０はフラグ名
                currentCast = _castList[castIndex];

                flagData._currentCast = currentCast;
                Debug.Log("セットしたフラグは " + flagData._currentCast.GetCastName());

                BonusState_Changejudgment(currentCast);

                return currentCast.GetCastName();
            }
            // Debug.Log("For文は今 " + i + "周目");
        }

        currentCast = _castList[0]; // ハズレという事
        flagData._currentCast = currentCast;
        Debug.Log("セットしたフラグは " + flagData._currentCast.GetCastName());

        return currentCast.GetCastName();
    }



    /// <summary>
    /// 抽選確率のCSVを読み込んでListに入れる
    /// </summary>
    /// <param name="path"></param>
    /// <param name="list"></param>
    void LoadCSV_ToRecourses(string path, List<string[]> list)
    {
        TextAsset _csvFile; // CSVファイル
        _csvFile = Resources.Load(path) as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(_csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            list.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // csvDatas[フラグ][設定]を指定して値を自由に取り出せる
        // Debug.Log(_lotteryDatas[0][_settingNumber]);
    }

    // 成立小役がボーナス系かチェックする
    void BonusState_Changejudgment(ICastBase cast)
    {
        var castIsBonusBase = cast is IBonusBase;
        if (castIsBonusBase)
        {
            BonusData bonusData = BonusData.GetInstance();
            // if(bonusData._currentBonusState == )
            Debug.Log("ボーナス系フラグを引きました");

            bonusData._currentBonusState.ChangeState(bonusData); // ボーナス入賞待ち状態へ変更
            Debug.Log("遷移した先のボーナス状態は " + bonusData._currentBonusState);
        }
    }
}

