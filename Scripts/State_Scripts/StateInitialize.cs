using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using UniRx;

public partial class StateController
{
    /// <summary>
    /// 起動時Initialize用のステート
    /// </summary>
    public class StateInitialize : StateBase
    {

        bool isWait; // UniRx用

        public override void OnEnter(StateController owner, StateBase prevState)
        {
            // 数値全て0に初期化
            GamePlayData gamePlayData = GamePlayData.GetInstance();
            gamePlayData.ValueInitialize();

            // UI全て更新（初期化）
            ValueUpdate_Script valueUpdate_Script = GameObject.Find("ScriptObj").GetComponent<ValueUpdate_Script>();
            valueUpdate_Script.UI_Initialize(); // UI初期化
            valueUpdate_Script.Update_AllText(); // UI全て更新処理

            BonusData bonusData = BonusData.GetInstance();
            IBonusState normalTime = new NormalTime();
            bonusData._currentBonusState = normalTime; // ボーナス非成立状態へ初期化

            Debug.Log(this.GetType().Name + " に移行しました");

            isWait = true;
            UniTaskWait();
        }


        // 引数のowner呼べないからUpdateでChangeState
        public override void OnUpdate(StateController owner)
        {
            if (!isWait)
            {
                owner.ChangeState(stateMaxBet);
            }
        }


        /// <summary>
        /// X秒待つ UniRx  引数のowner呼べないからこうする
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

