using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AddressableAssets; 
using System.IO;
using System;
using System.Linq;

public class TableHousiki_Script : MonoBehaviour
{
    TextAsset _csvFile; // CSVファイル

    string[] _flagNames = new string[] { "Hazure", "Replay", "Bell", "Suika", "Cherry", "Reg", "Big" };
    string[] _CSVNames = new string[] { "CSV_Left", "CSV_Center", "CSV_Right"};

    List<string[]> _splitCSVDatas = new List<string[]>(); // CSVの中身を入れるリスト;


    [SerializeField] private TextAsset[] _csvTextAssetArray_ForInspector; // 第１停止のみテーブル制御にする時用

    // CSVを読み込むためのIndexを引き出す ->
    Dictionary<uint, int> _allJointBit_ToCSVLoadIndex = new Dictionary<uint, int> 
    {
        { 0b1100_0000_0000 , 0 }, // FL左
        { 0b1010_0000_0000 , 1 }, // FC中
        { 0b1001_0000_0000 , 2 }, // FR右
    };
    //// ------------ここまで第１停止のみテーブル制御に関するコード



    // [SerializeField] private TextAsset[] _csvTextAssetArray_ForInspector; // *** 複数にする時はこう書く 要素数はインスペクターで指定 ***
    //// ↓こっちは全リール テーブル制御にする場合に使用する　まだ使わないけど準備した
    //Dictionary<uint, int> csv_Index_Array = new Dictionary<uint, int>
    //{
    //    { 0b1100_0000_0000 , 0 }, // FL左
    //    { 0b1100_1010_0000 , 1 }, // SC順押し
    //    { 0b1100_1010_1001 , 2 }, // TR順押し
    //                              // FLは１パターンしか無いためここは省略
    //    { 0b0100_0001_0000 , 3 }, // SR順ハサミ
    //    { 0b0100_0001_0010 , 4 }, // SR順ハサミ


    //    { 0b0010_0000_0000 , 5 }, // FC中
    //    { 0b0010_0100_0000 , 6 }, // SL中左
    //    { 0b0010_0100_0001 , 7 }, // TR中左
    //                              // FCは１パターンしか無いためここは省略
    //    { 0b0010_0001_0000 , 8 }, // SR中右
    //    { 0b0010_0001_0100 , 9 }, // TL中右


    //    { 0b0001_0000_0000 , 10}, // FR右
    //    { 0b0001_0010_0000 , 11}, // SC逆押し
    //    { 0b0001_0010_0100 , 12}, // TL逆押し
    //                              // FRは１パターンしか無いためここは省略
    //    { 0b0001_0100_0000 , 13}, // SL逆ハサミ
    //    { 0b0001_0100_0010 , 14}, // TC逆ハサミ
    //};


    void Start()
    {

    }



    /// <summary>
    /// 引数によりスベリコマ数を返すメソッド
    /// </summary>
    /// <param name="flagName">成立フラグ</param>
    /// <param name="stopKoma">停止位置コマ数</param>
    /// <returns></returns>
    public int ReturnSuberiKoma(int stopKoma)
    {
        // フラグ名を取得
        FlagData flagData = FlagData.GetInstance();
        string flagName = flagData._currentCast.GetCastName();

        StopReelData stopReelData = StopReelData.GetInstance();
        uint currentJointBit = stopReelData.GetCurrent_AllJointBit(); // F_S_Tを全て連結したBitを取得（今第何停止にするかに使う）

        int csvLoadIndex = _allJointBit_ToCSVLoadIndex[currentJointBit]; // どのCSVを読み込むか判定するためのIndexを引き出す
        // Debug.Log("参照するTextAssetのindex " + csvLoadIndex);
        
        TextAsset tempCSV = _csvTextAssetArray_ForInspector[csvLoadIndex]; // 対応CSVをセット WaitFor〜を使うと同期処理になる
        _splitCSVDatas.Clear(); // 初期化
        CSVRoad_ToArray(tempCSV, _splitCSVDatas); // 対応CSVを切り分けて

        int flagIndex = FlagToIndex(flagName); // 成立フラグ名から検索用indexに変換
        int suberiKoma = int.Parse(_splitCSVDatas[flagIndex][stopKoma]); // 検索用indexとリール停止のコマ数によりスベリコマ数を検索

        // -----↓ここから下はデバッグ用　要素確認--------
        //for (int i = 0; i < _splitCSVDatas[flagIndex].Length; i++)
        //{
        //    Debug.Log(_splitCSVDatas[flagIndex][i]);
        //}
        // -----↑ここから上はデバッグ用　要素確認--------


        return suberiKoma; // スベリコマ数を返す
    }



    // フラグの名前からCSV検索用のindexを取得
    int FlagToIndex(string flagName)
    {
        int indexNum = Array.IndexOf(_flagNames, flagName);
        return indexNum;
    }


    void CSVRoad_ToArray(TextAsset textAsset, List<string[]> csvArray)
    {
        StringReader reader = new StringReader(textAsset.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み

            // -----↓↓ここから下は、各フラグごとの制御表の順番を0->21から21->0に入れ替える↓↓---------
            List<string> reverseLine = new List<string>();
            reverseLine = line.Split(',').ToList();
            // string temp_flagName = reverseLine[0]; // 全て0-20に正規化したため要素0のフラグ名は不要に
            reverseLine.RemoveAt(0); // 要素0のフラグ名は削除
            // reverseLine.Add(temp_flagName); // 全て0-20に正規化したため要素0のフラグ名は不要に
            reverseLine.Reverse();
            string joinLine = string.Join(",", reverseLine);
            // -----↑↑ここから上は、各フラグごとの制御表の順番を0->21から21->0に入れ替える↑↑---------

            csvArray.Add(joinLine.Split(',')); // , 区切りでリストに追加
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        // Debug.Log(_csvDatas[0][1]);
        // Debug.Log(FlagToIndex("Replay"));

        //// -----↓ここから下はデバッグ用　要素確認--------
        //// index 1だからリプしか出ない
        //for (int i = 0; i < _splitCSVDatas[1].Length; i++)
        //{
        //    Debug.Log(_splitCSVDatas[1][i]);
        //}
        //// -----↑ここから上はデバッグ用　要素確認--------

    }

}
