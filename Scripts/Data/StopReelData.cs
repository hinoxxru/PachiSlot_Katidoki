using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 停止リールの情報をBitで管理するクラス
/// オリジナルクラス　かつ　シングルトン
/// </summary>
public class StopReelData 
{
    // 回転情報をBit管理する変数（プロパティ）
    public uint _allReelStop_Bit { set; get; } // 1は回転中　0は停止状態

    public uint _firstStop_Bit { set; get; }   // 第１停止リールはどこか
    public uint _secoundStop_Bit { set; get; } // 第２停止リールはどこか
    public uint _thirdStop_Bit { set; get; }   // 第３停止リールはどこか

    public uint _current_AllJointBit { set; get; } // F_S_TのBitを連結する　押し順などに利用


    // 3bitだけの演算をしても、結果32bitになってしまう　1111_1111_1111_1111_1111_1111_1111_1100

    private static StopReelData stopReelData = new StopReelData();

    private StopReelData() // privateのコンストラクタ
    {

    }

    public static StopReelData GetInstance()
    {
        return stopReelData;
    }


    /// <summary>
    /// Bit初期化用　全リール回転中
    /// </summary>
    public void Reel_BitReset_Spin()
    {
        uint spin = 0b111; // 各リール回転 左1　中1　右1
        _allReelStop_Bit = spin;

        uint flagReset = 0b000; // フラグが立っていない状態
        _firstStop_Bit = flagReset;   // 初期化
        _secoundStop_Bit = flagReset; // 初期化
        _thirdStop_Bit = flagReset;   // 初期化
    }


    /// <summary>
    /// Bit初期化用　全リール停止状態
    /// </summary>
    public void Reel_BitReset_Stop()
    {
        uint stop = 0b000; // 各リール回転 左0　中0　右0
        _allReelStop_Bit = stop;
    }


    /// <summary>
    /// リールスクリプトから呼ぶためのメソッド　Bit情報をセット
    /// </summary>
    /// <param name="bit_args">左011 中101 右110</param>
    /// <returns></returns>
    public bool SetBitInformation(uint bit_args)
    {
        bool b;
        b = SetPushOrder(bit_args);
        b = SetAllReelInformation(bit_args);
        return b; // 非同期みたいに処理を飛ばされると困るので、なんとなく戻り値をつけてみる
    }

    /// <summary>
    /// 押し順のBit情報をセットする
    /// </summary>
    /// <param name="bit_args">左011 中101 右110 にする</param>
    bool SetPushOrder(uint bit_args)
    {
        if(_firstStop_Bit == 0b000) // 第１停止フラグが立っていない場合
        {
            _firstStop_Bit = ~(_firstStop_Bit | bit_args); // これは NOTOR　000|011 -> 100 になる（引数が０のリールにフラグを立てる）
            _firstStop_Bit = _firstStop_Bit & 0x0f; // 32bitになってしまうので 4bitに変換 頭が1になってしまうので注意 1000とか
        }
        else if(_secoundStop_Bit == 0b000) // 第２停止フラグが立っていない場合
        {
            _secoundStop_Bit =　~(_secoundStop_Bit | bit_args);
            _secoundStop_Bit = _secoundStop_Bit & 0x0f; // 32bitになってしまうので 4bitに変換 頭が1になってしまうので注意 1000とか
        }
        else if(_thirdStop_Bit == 0b000) // 第３停止フラグが立っていない場合
        {
            _thirdStop_Bit = ~(_thirdStop_Bit | bit_args);
            _thirdStop_Bit = _thirdStop_Bit & 0x0f; // 32bitになってしまうので 4bitに変換 頭が1になってしまうので注意 1000とか
        }
        else
        {
            Debug.LogError("第何停止が止まったという情報をセットできません");
        }

        return true;
    }


    /// <summary>
    /// 全リール把握用の変数に停止リールをセットする 0停止状態　1回転中
    /// </summary>
    /// <param name="bit_args"></param>
    /// <returns></returns>
    bool SetAllReelInformation(uint bit_args)
    {
        _allReelStop_Bit &= bit_args; // 111 & 011 -> 011 左リールフラグを折る
        return true;
    }


    /// <summary>
    /// 停止リールのBitを連結して返す
    /// </summary>
    /// <returns></returns>
    public uint GetCurrent_AllJointBit()
    {
        uint jointBit = _firstStop_Bit << 8;
        uint tempBit = _secoundStop_Bit << 4;
        jointBit |= tempBit;
        jointBit |= _thirdStop_Bit;
        // Debug.Log(Convert.ToString(jointBit, 2));
        return jointBit;
    }
}

