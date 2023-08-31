using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// フラグ情報を保持するクラス
/// オリジナルクラス　かつ　シングルトン
/// </summary>
public class FlagData 
{
    public string _currentFlag { set; get; } // プロパティ

    // 試み
    public ICastBase _currentCast { set; get; } // フラグ情報を派生クラスで保持
    public ICastBase _currentBounusCast { set; get; } // 成立もしくは消化ボーナスの情報（払出上限とか）

    private static FlagData flagData = new FlagData();

    private FlagData() // privateのコンストラクタ
    {

    }

    public static FlagData GetInstance()
    {
        return flagData;
    }


}

