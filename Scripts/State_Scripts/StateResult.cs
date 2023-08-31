using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using UniRx;

public partial class StateController
{
    public class StateResult : StateBase
    {
        bool isWait;

        public override void OnEnter(StateController owner, StateBase prevState)
        {
            // Debug.Log(this.GetType().Name + " に移行しました");

            // 入賞チェック
            NyuusyouCheck nyuusyouCheck = new NyuusyouCheck();
            nyuusyouCheck.Call_NyuusyouCheck();



            isWait = true;
            UniTaskWait();

        }


        // 引数のowner呼べないからUpdateでChangeState
        public override void OnUpdate(StateController owner)
        {
            // Debug.Log(isWait);
            if (!isWait)
            {
                owner.ChangeState(statePayOut);
            }
        }



        /// <summary>
        /// X 秒待つ UniRx  引数のowner呼べないからUpdateでChangeState
        /// </summary>
        void UniTaskWait()
        {
            var observable = Observable
                .Start(() => "OnNext.")
                .DoOnSubscribe(() => isWait = false)
                .DelaySubscription(System.TimeSpan.FromSeconds(0.1f))
                .Subscribe();
        }


    }
}

