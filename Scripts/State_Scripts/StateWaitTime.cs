using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
// using System.Threading;
// using System.Threading.Tasks;
using UniRx;


public partial class StateController
{
    /// <summary>
    /// ウェイトタイムを表現するためのステート
    /// </summary>
    public class StateWaitTime : StateBase
    {

        bool isWait; // UniRx用

        public override void OnEnter(StateController owner, StateBase prevState)
        {
            // Debug.Log(this.GetType().Name + " に移行しました");

            isWait = true;
            UniTaskWait();

        }


        // 引数のowner呼べないからUpdateでChangeState
        public override void OnUpdate(StateController owner)
        {
            if (!isWait)
            {
                owner.ChangeState(stateFirstStopButton);
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
                .DelaySubscription(System.TimeSpan.FromSeconds(1.0f))
                .Subscribe();
        }



    }
}

