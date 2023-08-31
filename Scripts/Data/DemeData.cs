using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// 出目情報を保持するクラス
/// オリジナルクラス　かつ　シングルトン
/// </summary>
public class DemeData 
{
    // public string _currentFlag { set; get; } // プロパティ

    public List<int> _currentReelLeftDemes_int { set; get; } = new List<int>();   // 左リール  [0]上段 [1]中段 [2]下段
    public List<int> _currentReelCenterDemes_int { set; get; } = new List<int>(); // 中リール　[0]上段 [1]中段 [2]下段
    public List<int> _currentReelRightDemes_int { set; get; } = new List<int>();  // 右リール　[0]上段 [1]中段 [2]下段

    public List<string> _currentReelLeftDemes_String { set; get; } = new List<string>();   // 左リール　[0]上段 [1]中段 [2]下段
    public List<string> _currentReelCenterDemes_String { set; get; } = new List<string>(); // 中リール　[0]上段 [1]中段 [2]下段
    public List<string> _currentReelRightDemes_String { set; get; } = new List<string>();  // 右リール　[0]上段 [1]中段 [2]下段


    private static DemeData demeData = new DemeData();

    private DemeData() // privateのコンストラクタ
    {

    }

    public static DemeData GetInstance()
    {
        return demeData;
    }


    /// <summary>
    /// 外部から呼び出し用　int型とstring型の出目情報をセットする
    /// [0]上段  [1]中段  [2]下段
    /// </summary>
    /// <param name="stopKomaNum">下段の停止コマ数を渡す</param>
    /// <param name="reelName"></param>
    public void SetDemeData(int stopKomaNum, string reelName)
    {
        ReelHairetuData reelHairetuData = ReelHairetuData.GetInstance();
        // Debug.Log("リールから受け取ったコマ数は " + stopKomaNum);

        // リール配列を参照し図柄番号を取得
        var tuple_zugaraNum = reelHairetuData.Get_ZugaraNum_Tuple(stopKomaNum, reelName);

        ReelNameToListName_int(reelName).Clear();
        ReelNameToListName_int(reelName).Add(tuple_zugaraNum.jyoudann); // [0]上段
        ReelNameToListName_int(reelName).Add(tuple_zugaraNum.tyuudann); // [1]中段
        ReelNameToListName_int(reelName).Add(tuple_zugaraNum.gedann);   // [2]下段

        // リール配列を参照し図柄の名前を取得
        var tuple_zugaraName = reelHairetuData.Get_ZugaraName_Tuple(stopKomaNum, reelName);

        ReelNameToListName_string(reelName).Clear();
        ReelNameToListName_string(reelName).Add(tuple_zugaraName.jyoudann); // [0]上段
        ReelNameToListName_string(reelName).Add(tuple_zugaraName.tyuudann); // [1]中段
        ReelNameToListName_string(reelName).Add(tuple_zugaraName.gedann);   // [2]下段
    }


    /// <summary>
    /// 外部から出目情報を取得するためのメソッド
    /// int型で3つの戻り値を返す
    /// </summary>
    /// <param name="reelName"></param>
    /// <returns></returns>
    public (int jyoudann, int tyuudann, int gedann) GetDemeDeta_int_tuple(string reelName)// 名前付きのタプルの宣言
    {
        if (reelName == "ReelLeftObj")
        {
            return (_currentReelLeftDemes_int[0],   // 上段
                    _currentReelLeftDemes_int[1],   // 中段
                    _currentReelLeftDemes_int[2]);  // 下段
        }
        else if (reelName == "ReelCenterObj")
        {
            return (_currentReelCenterDemes_int[0],  // 上段
                    _currentReelCenterDemes_int[1],  // 中段
                    _currentReelCenterDemes_int[2]); // 下段
        }
        else if (reelName == "ReelRightObj")
        {
            return (_currentReelRightDemes_int[0],  // 上段
                    _currentReelRightDemes_int[1],  // 中段
                    _currentReelRightDemes_int[2]); // 下段
        }
            return (99, 99, 99); // ここまで来ると異常状態
    }


    /// <summary>
    /// 外部から出目情報を取得するためのメソッド
    /// string型で3つの戻り値を返す
    /// </summary>
    /// <param name="reelName"></param>
    /// <returns></returns>
    public (string jyoudann, string tyuudann, string gedann) GetDemeDeta_string_tuple(string reelName)// 名前付きのタプルの宣言
    {
        if (reelName == "ReelLeftObj")
        {
            return (_currentReelLeftDemes_String[0],   // 上段
                    _currentReelLeftDemes_String[1],   // 中段
                    _currentReelLeftDemes_String[2]);  // 下段
        }
        else if (reelName == "ReelCenterObj")
        {
            return (_currentReelCenterDemes_String[0],  // 上段
                    _currentReelCenterDemes_String[1],  // 中段
                    _currentReelCenterDemes_String[2]); // 下段
        }
        else if (reelName == "ReelRightObj")
        {
            return (_currentReelRightDemes_String[0],  // 上段
                    _currentReelRightDemes_String[1],  // 中段
                    _currentReelRightDemes_String[2]); // 下段
        }
        return (null, null, null); // ここまで来ると異常状態
    }


    // 変換系のメソッド
    List<int> ReelNameToListName_int(string reelName)
    {
        if (reelName == "ReelLeftObj")
        {
            return _currentReelLeftDemes_int;
        }
        else if (reelName == "ReelCenterObj")
        {
            return _currentReelCenterDemes_int;
        }
        else if (reelName == "ReelRightObj")
        {
            return _currentReelRightDemes_int;
        }
        return null;
    }


    // 変換系のメソッド
    List<string> ReelNameToListName_string(string reelName)
    {
        if (reelName == "ReelLeftObj")
        {
            return _currentReelLeftDemes_String;
        }
        else if (reelName == "ReelCenterObj")
        {
            return _currentReelCenterDemes_String;
        }
        else if (reelName == "ReelRightObj")
        {
            return _currentReelRightDemes_String;
        }
        return null;
    }
}

