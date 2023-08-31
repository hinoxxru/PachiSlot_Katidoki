using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using UniRx;

public partial class StateController
{
    /// <summary>
    /// 第２停止ステート
    /// </summary>
    public class StateSecondStopButton : StateBase
    {
        bool isWait; // UniRx用

        public override void OnEnter(StateController owner, StateBase prevState)
        {
            // Debug.Log(this.GetType().Name + " に移行しました");

            isWait = true;
            UniTaskWait();
        }


        public override void OnClick(StateController owner)
        {
            if (!isWait) // X 秒まった後に押せるようになる
            {
                // 座標取得  // 座標は右下が(0, 0, 0)　左上が(1366, 768, 0)
                Vector3 mousePosition = Input.mousePosition;

                ReelStop_Script reelStop_Script = GameObject.Find("ScriptObj").GetComponent<ReelStop_Script>();

                string stopReelName = reelStop_Script.ReelStop(mousePosition.x, mousePosition.y);

                owner.ChangeState(stateThirdStopButton);
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

