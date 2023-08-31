using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using UniRx;
using System.Linq; // Listの最後を取得するため

public partial class StateController
{
    public class StatePayOut : StateBase
    {
        bool isWait; // UniRx用

        public override void OnEnter(StateController owner, StateBase prevState)
        {
            // Debug.Log(this.GetType().Name + " に移行しました");

            MedalPayOut();

            UI_Update();



            isWait = true;
            UniTaskWait();

        }


        // 引数のowner呼べないからUpdateでChangeState
        public override void OnUpdate(StateController owner)
        {
            // Debug.Log(isWait);
            if (!isWait)
            {
                // ここでリプレイフラグ拾いたい
                GamePlayData gamePlayData = GamePlayData.GetInstance();
                ICastBase currentNyuusyouCast = gamePlayData._nyuusyouCast_ICastBase.Last();
                Debug.Log("PayOutから リプ入賞判定したい " + currentNyuusyouCast.GetCastName());

                // リプレイ入賞で MAXBET飛ばしてレバーオンへ
                if(currentNyuusyouCast.GetCastName() == "Replay")
                {
                    owner.ChangeState(stateLeverOn);
                }
                else
                {
                    owner.ChangeState(stateMaxBet);
                }
                
            }
        }



        /// <summary>
        /// 1秒待つ UniRx  引数のowner呼べないからUpdateでChangeState
        /// </summary>
        void UniTaskWait()
        {
            var observable = Observable
                .Start(() => "OnNext.") // "OnNext."消したいけど消し方わかんない
                .DoOnSubscribe(() => isWait = false)
                .DelaySubscription(System.TimeSpan.FromSeconds(0.6f))
                .Subscribe(); // 最初は .Subscribe(Debug.Log);だった 
        }


        /// <summary>
        /// 入賞結果をMedal枚数に反映させる（PlayData）
        /// </summary>
        void MedalPayOut()
        {
            BonusData bonusData = BonusData.GetInstance();
            GamePlayData gamePlayData = GamePlayData.GetInstance();

            // ボーナス消化中だったらカウンターに枚数追加
            if (bonusData.Get_IsBonusDigestion())
            {
                gamePlayData._bonusPayOutCounter += gamePlayData._currentOutMedal; // ボーナス払出カウンターに枚数追加
            }

            gamePlayData._creditMedal += gamePlayData._currentOutMedal;
            gamePlayData.Check_CreditRange(); // 50 - 0
            gamePlayData._totalMedal += gamePlayData._currentOutMedal;
        }


        /// <summary>
        /// UI全て更新（初期化）
        /// </summary>
        void UI_Update()
        {
            ValueUpdate_Script valueUpdate_Script = GameObject.Find("ScriptObj").GetComponent<ValueUpdate_Script>();
            valueUpdate_Script.Update_Pay();
            // valueUpdate_Script.Update_Credit();
            valueUpdate_Script.Update_CreditCountUp();

            // ここテスト用　BonusGame表示するだけ　あとで改良する
            valueUpdate_Script.Show_BonusGame();
        }

        void BonusDigesingValue_Update()
        {
            BonusData bonusData = BonusData.GetInstance();
            // if()
        }
    }
}

