using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using UniRx;

public partial class StateController
{
    /// <summary>
    /// レバーオン待ちのステート
    /// </summary>
    public class StateLeverOn : StateBase
    {
        string[] _reelNames = new string[] { "ReelLeftObj", "ReelCenterObj", "ReelRightObj" };


        bool isWait; // UniRx用

        public override void OnEnter(StateController owner, StateBase prevState)
        {
            Debug.Log(this.GetType().Name + " に移行しました");

            isWait = true;
            UniTaskWait();
        }



        public override void OnClick(StateController owner)
        {

            if (!isWait) // X 秒まった後に押せるようになる
            {
                // フラグ抽選
                FlagLottery flagLottery = GameObject.Find("ScriptObj").GetComponent<FlagLottery>();
                flagLottery.CallLottrey();

                StopReelData stopReelData = StopReelData.GetInstance();
                stopReelData.Reel_BitReset_Spin();

                // リール始動
                Reel_Script reel_Script;
                for (int i = 0; i < 3; i++)
                {
                    reel_Script = GameObject.Find(_reelNames[i]).GetComponent<Reel_Script>();
                    reel_Script.CallReelSpin();
                }

                // ゲーム数加算（PlayData）
                GamePlayData gamePlayData = GamePlayData.GetInstance();
                gamePlayData._betweenGameCount++; // ボーナス間 1G加算
                gamePlayData._totalGameCount++;   // 総ゲーム数 1G加算


                // UIの数値更新
                ValueUpdate_Script valueUpdate_Script = GameObject.Find("ScriptObj").GetComponent<ValueUpdate_Script>();
                valueUpdate_Script.Update_BetweenBonusCount();
                valueUpdate_Script.Update_TotalGameCount();
                                

                owner.ChangeState(stateWaitTime);
            }
        }


        /// <summary>
        /// 1秒待つ UniRx  リール停止バグ解消のために実装　引数のowner呼べないからUpdateでChangeState
        /// </summary>
        void UniTaskWait()
        {
            var observable = Observable
                .Start(() => "OnNext.")
                .DoOnSubscribe(() => isWait = false)  // サブスクライブしたら呼ばれる
                .DelaySubscription(System.TimeSpan.FromSeconds(0.1f))
                .Subscribe();
        }
    }
}

