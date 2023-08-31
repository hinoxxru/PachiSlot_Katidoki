using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.ValueTuple;

/// <summary>
/// リール配列の情報を保持するクラス
/// オリジナルクラス　かつ　シングルトン
///
/// 持っている機能
/// 図柄番号を返す機能、図柄名(String)を返す機能、いずれも下段だけ返すor上中下段を返す
/// </summary>
public class ReelHairetuData 
{

    // --------配列データはリソースから読み込めないのでベタ打ち（モノビヘイビア承継していない）
    private int[] _reelHairetu_Left = new int[21] {2,1,32,1,4,2,4,1,8,64,2,4,1,2,64,4,2,1,16,8,16};
    private int[] _reelHairetu_Center = new int[21] {2,8,32,8,1,2,8,2,1,4,16,8,2,1,4,8,1,2,64,64,1};
    private int[] _reelHairetu_Right = new int[21] {1,8,32,2,4,1,4,2,16,1,16,2,4,1,8,2,4,1,64,2,4};
    
    // --------配列データはリソースから読み込めないのでベタ打ち（モノビヘイビア承継していない）



    // --------図柄名データはリソースから読み込めないのでベタ打ち（モノビヘイビア承継していない）
    // 図柄番号から図柄名を検索する辞書　キーが図柄番号
    // 図柄名の取得方法は　zugaraNames[32] とか
    Dictionary<int, string> _zugaraNames = new Dictionary<int, string>()
        {
            { 1, "Replay" },
            { 2, "Bell" },
            { 4, "Suika" },
            { 8, "Cherry" },
            {16, "Reg" },
            {32, "Big" },
            {64, "Blank" }
        };
    // --------図柄名データはリソースから読み込めないのでベタ打ち（モノビヘイビア承継していない）



    // --------ここシングルトン
    private static ReelHairetuData reelHairetuData = new ReelHairetuData();

    private ReelHairetuData() // privateのコンストラクタ
    {
        // リール配列の並びを逆にする（リール配列を考える時は下から1が始まるから）
        Array.Reverse(_reelHairetu_Left);
        Array.Reverse(_reelHairetu_Center);
        Array.Reverse(_reelHairetu_Right);
    }

    public static ReelHairetuData GetInstance()
    {
        return reelHairetuData;
    }
    // --------ここシングルトン



    /// <summary>
    /// 外部から呼び出せる　コマ数を渡すと上中下段のintを返す
    /// タプル
    /// </summary>
    /// <param name="komaNum"></param>
    /// <param name="reelName"></param>
    /// <returns></returns>
    public (int jyoudann, int tyuudann, int gedann) Get_ZugaraNum_Tuple(int komaNum, string reelName)
    {
        
        // int ge   = Over21KomaConverter(komaNum - 1); // 要素番号とコマ数より 1 少ない
        int ge = Over21KomaConverter(komaNum); // 上限20コマへ正規化
        int tyuu = Over21KomaConverter(ge + 1);
        int jyou = Over21KomaConverter(ge + 2);

        // 戻り値
        return (ReelNameToArray(reelName)[jyou], // 上段
                ReelNameToArray(reelName)[tyuu], // 中段
                ReelNameToArray(reelName)[ge]);  // 下段
        
    }

    /// <summary>
    /// 外部から呼び出せる　図柄名を返す
    /// </summary>
    /// <param name="komaNum">下段の停止コマ数</param>
    /// <param name="reelName"></param>
    /// <returns></returns>
    public (string jyoudann, string tyuudann, string gedann) Get_ZugaraName_Tuple(int komaNum, string reelName)
    {
        // 上中下段の停止コマ数を再定義
        // int ge = Over21KomaConverter(komaNum - 1); // 要素番号とコマ数より 1 少ない
        int ge = Over21KomaConverter(komaNum); // 上限20コマへ正規化
        int tyuu = Over21KomaConverter(ge + 1);
        int jyou = Over21KomaConverter(ge + 2);

        // 配列から図柄番号を取得
        int zugaraNum_jyou = ReelNameToArray(reelName)[jyou];
        int zugaraNum_tyuu = ReelNameToArray(reelName)[tyuu];
        int zugaraNum_ge = ReelNameToArray(reelName)[ge];

        // 図柄名を返す(string)
        return (_zugaraNames[zugaraNum_jyou], // 上段
                _zugaraNames[zugaraNum_tyuu], // 中段
                _zugaraNames[zugaraNum_ge]);  // 下段

    }



    /// <summary>
    /// 外部から呼び出せる　コマ数を渡すと上中下段のintを返す
    /// タプル
    /// </summary>
    /// <param name="komaNum"></param>
    /// <param name="reelName"></param>
    /// <returns></returns>
    public (int jyoudann, int tyuudann, int gedann,
            int ue4Koma, int ue5Koma, int ue6Koma, int Ue7Koma)Get_ZugaraNum_7koma_Tuple(int komaNum, string reelName)
    {

        // int ge = Over21KomaConverter(komaNum - 1); // 要素番号とコマ数より 1 少ない
        int ge = Over21KomaConverter(komaNum); // 上限20コマへ正規化
        int tyuu = Over21KomaConverter(ge + 1);
        int jyou = Over21KomaConverter(ge + 2);
        int ue4koma = Over21KomaConverter(ge + 3);
        int ue5koma = Over21KomaConverter(ge + 4);
        int ue6koma = Over21KomaConverter(ge + 5);
        int ue7koma = Over21KomaConverter(ge + 6);

        // Debug.Log("引数でもらったリール名は " + reelName);
        // Debug.Log("リール配列を参照する配列は " + ReelNameToArray(reelName));

        // 戻り値
        return (ReelNameToArray(reelName)[jyou], // 上段  /////ここエラー
                ReelNameToArray(reelName)[tyuu], // 中段
                ReelNameToArray(reelName)[ge], // 下段
                ReelNameToArray(reelName)[ue4koma], // 枠上1コマ
                ReelNameToArray(reelName)[ue5koma], // 枠上2コマ
                ReelNameToArray(reelName)[ue6koma], // 枠上3コマ
                ReelNameToArray(reelName)[ue7koma]);   // 枠上4コマ

    }



    ///// <summary>
    ///// 外部から呼び出せる　コマ数を渡すとコントロール方式　引き込み判定用のintを返す
    ///// タプル
    ///// </summary>
    ///// <param name="komaNum"></param>
    ///// <param name="reelName"></param>
    ///// <returns></returns>
    //public int[]  Get_ZugaraNum_7Koma_Tuple(int komaNum, string reelName)
    //{
    //    int index = Over21KomaConverter(komaNum - 1); // 要素番号とコマ数より 1 少ない
    //    int[] reelHairetu_ints = new int[7];

    //    for (int i = 0; i < 7; i++)
    //    {
    //        int jittai = ReelNameToArray(reelName)[index];
    //        // reelHairetu_ints[i] = ReelNameToArray(reelName)[index];
    //        reelHairetu_ints[i] = jittai;
    //        index = Over21KomaConverter(index + 1);
    //    }

    //    // 戻り値
    //    return reelHairetu_ints; // 配列を返すが要素0が下段 1が中段 2が上段 3が枠上１コマ... で７まで

    //}


    // 変換系のメソッド
    int[] ReelNameToArray(string reelName)
    {
        if (reelName == "ReelLeftObj")
        {
            return _reelHairetu_Left;
        }
        else if (reelName == "ReelCenterObj")
        {
            return _reelHairetu_Center;
        }
        else if (reelName == "ReelRightObj")
        {
            return _reelHairetu_Right;
        }
        return null;
    }


    // ２１コマを超える場合は修正
    int Over21KomaConverter(int argsKoma)
    {
        if (argsKoma >= 21)
        {  // 21を超える場合は1からに変換
            argsKoma -= 21;
        }
        return argsKoma;
    }
}
