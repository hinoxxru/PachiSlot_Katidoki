using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using UniRx;

public partial class StateController
{
    /// <summary>
    /// MaxBet待ちのステート
    /// </summary>
    public class StateMaxBet : StateBase
    {

        bool isWait; // UniRx用

        public override void OnEnter(StateController owner, StateBase prevState)
        {

            Debug.Log(this.GetType().Name + " に移行しました");

            isWait = true;
            UniTaskWait();
        }


        public override void OnClick(StateController owner)
        {
            // Debug.Log("チェンジステート" + "次はstateLeverOn");

            if (!isWait) // X 秒まった後に押せるようになる
            {
                // in枚数をクレジットから引く（PlayData）
                GamePlayData gamePlayData = GamePlayData.GetInstance();
                gamePlayData._currentInMedal = 3; // 3枚掛け
                gamePlayData._creditMedal = gamePlayData._creditMedal - gamePlayData._currentInMedal; // クレジット枚数を保存
                gamePlayData._previousCreditMedal = gamePlayData._creditMedal; // 払出前のクレジットを保存しておく(払出カウントアップのスタート値）
                gamePlayData._totalMedal = gamePlayData._totalMedal - gamePlayData._currentInMedal;

                // クレジットの数値更新
                ValueUpdate_Script valueUpdate_Script = GameObject.Find("ScriptObj").GetComponent<ValueUpdate_Script>();
                // valueUpdate_Script.Update_Credit();
                valueUpdate_Script.Update_CreditCountDown();
                valueUpdate_Script.Update_TotalMedal();

                // ペイアウトの表示初期化
                gamePlayData._currentOutMedal = 0;
                valueUpdate_Script.Update_Pay();


                owner.ChangeState(stateLeverOn);
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

