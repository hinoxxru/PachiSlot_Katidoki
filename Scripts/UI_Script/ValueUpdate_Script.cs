using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class ValueUpdate_Script  :  MonoBehaviour
{
    // こっちはインフォメーション
    [SerializeField] TextMeshProUGUI _totalMedal_Text;
    [SerializeField] TextMeshProUGUI _totalGameCount_Text;
    [SerializeField] TextMeshProUGUI _betweenBonusCount_Text;
    [SerializeField] TextMeshProUGUI _bBCount_Text;
    [SerializeField] TextMeshProUGUI _rBCount_Text;
    // こっちは盤面
    [SerializeField] TextMeshProUGUI _pay_Text;
    [SerializeField] TextMeshProUGUI _credit_Text;
    [SerializeField] TextMeshProUGUI _counter_Text;

    // ここはテスト
    [SerializeField] TextMeshProUGUI _BonusGameDigestion_Text;


    public void UI_Initialize()
    {
        _BonusGameDigestion_Text.enabled = false; // 非表示
    }

    public void Update_AllText()
    {
        Update_TotalMedal();
        Update_TotalGameCount();
        Update_BetweenBonusCount();
        Update_BBCount();
        Update_RBCount();
        Update_Pay();
        Update_Credit();
        Update_Counter();
    }

    public void Update_TotalMedal()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int totalMedal = gamePlayData._totalMedal;
        _totalMedal_Text.text = totalMedal.ToString();
    }

    public void Update_TotalGameCount()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int totalGameCount = gamePlayData._totalGameCount;
        _totalGameCount_Text.text = totalGameCount.ToString();
    }

    public void Update_BetweenBonusCount()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int betweenBonusCount = gamePlayData._betweenGameCount;
        _betweenBonusCount_Text.text = betweenBonusCount.ToString();
    }

    public void Update_BBCount()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int bBCount = gamePlayData._BBCount;
        _bBCount_Text.text = bBCount.ToString();
    }

    public void Update_RBCount()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int rBCount = gamePlayData._RBCount;
        _rBCount_Text.text = rBCount.ToString();
    }

    public void Update_Pay()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int pay = gamePlayData._currentOutMedal;
        _pay_Text.text = pay.ToString();
    }

    public void Update_Credit()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int credit = gamePlayData._creditMedal;
        _credit_Text.text = credit.ToString();
    }

    // MaxBet時の減り方
    public void Update_CreditCountDown()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int startCredit = gamePlayData._creditMedal + gamePlayData._currentInMedal; // MAXBET時点のクレジット枚数
        int endCredit = gamePlayData._creditMedal;// カウントアップの終点
        // float duration = (endCredit - startCredit) / 6; // 払出に要する時間（払出枚数をそのまま入れる）
        float duration = gamePlayData._currentInMedal / 8.1f; // 払出に要する時間（払出枚数をそのまま入れる）

        // DOTweenでカウントアップを実現
        DOVirtual.Float(startCredit, endCredit, duration, countUpValue =>
        {
            int value = (int)Math.Floor(countUpValue);
            _credit_Text.text = value.ToString();
        })
            .SetEase(Ease.Linear); // イージングをLinearにしないと等間隔でカウントアップしてくれない
    }



    public void Update_CreditCountUp()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int startCredit = gamePlayData._previousCreditMedal; // MAXBET時点のクレジット枚数
        int endCredit = gamePlayData._creditMedal; // カウントアップの終点
        float duration = (endCredit - startCredit) / 6; // 払出に要する時間（払出枚数をそのまま入れる）

        // DOTweenでカウントアップを実現
        DOVirtual.Float(startCredit, endCredit, duration, countUpValue =>
        {
            int value = (int)Math.Floor(countUpValue);
            _credit_Text.text = value.ToString();
        })
            .SetEase(Ease.Linear); // イージングをLinearにしないと等間隔でカウントアップしてくれない

    }

    public void Update_Counter()
    {
        GamePlayData gamePlayData = GamePlayData.GetInstance();
        int counter = gamePlayData._bonusPayOutCounter;
        _counter_Text.text = counter.ToString();
    }


    // テスト
    public void Show_BonusGame()
    {
        BonusData bonusData = BonusData.GetInstance();
        //if (bonusData.Get_CanBonusNyuusyou())
        if(bonusData.Get_IsBonusDigestion())
        {
            _BonusGameDigestion_Text.enabled = true; // ボーナス揃えたので"BonusGame中"表示
        }
        else
        {
            _BonusGameDigestion_Text.enabled = false; // まだボーナス揃ってないので非表示
        }
    }
}
