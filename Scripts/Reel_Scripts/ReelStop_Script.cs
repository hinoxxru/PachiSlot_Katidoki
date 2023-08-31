using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelStop_Script : MonoBehaviour
{
    string[] _reelNames = new string[] { "ReelLeftObj", "ReelCenterObj", "ReelRightObj" };

    void Start()
    {

    }

    /// <summary>
    /// 各ステートから呼ぶ　リールを停止させる
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public string ReelStop(float width, float height)
    {
        string stopReelName = ClickPosition_ToStopReel(width, height); // 停止させるリール名 算出

        Reel_Script reel_Script = GameObject.Find(stopReelName).GetComponent<Reel_Script>();
        reel_Script.CallReelStop(); // 対応するリール停止

        return stopReelName;
    }


    /// <summary>
    /// Input座標からどのリールを止めるか算出
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    string ClickPosition_ToStopReel(float width, float height)
    {

        // まずは上下の枠外判定
        float maxHeight = 680f; // 縦の最大値　これ以上で枠上　順押しへ
        float minHeight = 55;   // 縦の最小値　これ以下で枠下　順押しへ
        if (height > maxHeight || height < minHeight) // 最大値以上もしくは最小値以下は順押し
        {
            return Jyunoshi();
            // return "枠外";
        }

        // 左右の枠外判定
        float maxWidth = 1070f; // 横の最大値　これ以上で右枠外　順押しへ
        float minWidth =  300f; // 横の最小値　これ以下で左枠外　順押しへ
        if (width < minWidth || width >= maxWidth)
        {
            return Jyunoshi();
            // return "枠外";
        }

        // 左リール付近の判定
        float leftMaxW = 560f; // 左リールの範囲　右最大
        float leftMinW = 300f; // 左リールの範囲　左最大
        if (width >= leftMinW && width < leftMaxW)
        {
            if (IsReelSpinning(0b100))
            {
                // Debug.Log("左リールの範囲内");
                return _reelNames[0]; // 左リール
            }

            // 左停止の場合　順押し
            return Jyunoshi();
        }


        // 中リール付近の判定
        float centerMaxW = 790f; // 左リールの範囲　右最大
        float centerMinW = 560f; // 左リールの範囲　左最大
        if (width >= centerMinW && width < centerMaxW)
        {
            if (IsReelSpinning(0b010))
            {
                // Debug.Log("中リールの範囲内");
                return _reelNames[1]; // 中リール
            }

            // 中停止の場合　順押し
            return Jyunoshi();
        }


        // 右リール付近の判定
        float rightMaxW = 1070f; // 左リールの範囲　右最大
        float rightMinW = 790f; // 左リールの範囲　左最大
        if (width >= rightMinW && width < rightMaxW)
        {
            if (IsReelSpinning(0b001))
            {
                // Debug.Log("右リールの範囲内");
                return _reelNames[2]; // 右リール
            }

            // 右停止の場合　順押し
            return Jyunoshi();
        }

        return "失敗だよーん";
    }



    /// <summary>
    /// 引数で渡したリールが回っているか？
    /// </summary>
    /// <param name="bit">000の3桁</param>
    /// <returns></returns>
    bool IsReelSpinning(uint bit) 
    {
        StopReelData stopReelData = StopReelData.GetInstance();
        uint currentBit = stopReelData._allReelStop_Bit;
        uint resultBit = currentBit & bit; // どちらも1のbitが1になる 回転中であれば引数と同じ数値になる
        if (resultBit == bit)
        {
            return true; // 回転中
        }
        else
        {
            return false; // 停止している
        }
    }



    /// <summary>
    /// 順押し時の回転判定　左から順にチェックしていく
    /// </summary>
    /// <returns></returns>
    string Jyunoshi()
    {
        if (IsReelSpinning(0b100)) // 左リールは回っているか？ bit 100
        {
            return _reelNames[0]; // 左リール名
        }

        if (IsReelSpinning(0b010)) // 中リールは回っているか？ bit 010
        {
            return _reelNames[1]; // 中リール名
        }

        if (IsReelSpinning(0b001)) // 右リールは回っているか？ bit 001
        {
            return _reelNames[2]; // 右リール名
        }

        Debug.LogError("全リール停止しています");
        return null;
    }

}
