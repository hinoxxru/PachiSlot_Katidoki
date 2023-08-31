using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHousiki_Third : MonoBehaviour
{
    

    uint _firstStop_ReelBit;   // 第１停止はどのリールか
    uint _secoundStop_ReelBit; // 第２停止はどのリールか
    uint _thirdStop_ReelBit;   // 第３停止はどのリールか

    // 第１停止の情報を入れる用
    int[] _firstStop_Deme_ints = new int[3];   // 2 上段
                                               // 1 中段
                                               // 0 下段
    // 第２停止の情報を入れる用
    int[] _secoundStop_Deme_ints = new int[3]; // 2 上段
                                               // 1 中段
                                               // 0 下段

    // 第３停止の情報を入れる用
    int[] _thirdStop_ReelHairetu_ints = new int[7];   // 2 上段     6 ４コマスベリの範囲
                                                      // 1 中段     ↑
                                                      // 0 下段     3

    int[] tenpaiCheck_Order_first = new int[5];   // ここはこのままで良いような？
    int[] tenpaiCheck_Order_secound = new int[5]; // ここはこのままで良いような？

    uint   _currentFlag_LeftBit;   // フラグのBitを分割　左8bit
    uint   _currentFlag_CenterBit; // フラグのBitを分割　中8bit
    uint   _currentFlag_RightBit;  // フラグのBitを分割　右8bit

    int _suberiKoma; // テンパイ判定のスコープに入れようと思ったが、bool値を返したかったのでフィールド変数にした
    int _tenpaiLine; // テンパイチェックしてテンパイしているライン
    bool _isTenpai;  // テンパイチェックでテンパイしたら true   テンパイ無しはfalse
    int _tenmp_ThirdCheckKomaInt; // 第３リールの蹴飛ばし制御時、比較用のint



    // 蹴飛ばし制御で停止してはいけないコマ数をあらかじめ決めておく
    List<int> _dontStopKoma_Left = new List<int>   { 0, 1, 10, 11, 12, 14, 17, 20 }; // チェリー含む
    List<int> _dontStopKoma_Center = new List<int> { 0, 1, 13, 17 };
    List<int> _dontStopKoma_Right = new List<int>  { 10, 14, 18 };




    /// <summary>
    /// 外部から呼ぶ用　第２停止用　停止位置とフラグを元にスベリコマ数を返す
    /// </summary>
    /// <param name="stopKoma"></param>
    /// <returns></returns>
    public int Control_ThirdStop_Return_SuberiKoma(int stopKoma)
    {
        // ▼▼▼------------- 引数　現在の成立フラグ -> 入賞判定用のBitを分解 ----------▼▼▼
        // Castインターフェイス方式に切替え後
        FlagData flagData = FlagData.GetInstance();
        FlagBit_8bitSplit(flagData._currentCast.GetCastBit()); // CastインターフェイスのGetCastBitメソッドでBitを取得
        // ▲▲▲------------- 引数　現在の成立フラグ -> 入賞判定用のBitを分解 ----------▲▲▲


        // ▼▼▼------------ 押し順情報を取得 ------------▼▼▼
        StopReelData stopReelData = StopReelData.GetInstance();
        this._firstStop_ReelBit = stopReelData._firstStop_Bit; // 第１停止はどのリールか
        // Debug.Log("第１停止リールの停止ビットは" + Convert.ToString(_firstStop_ReelBit, 2));
        this._secoundStop_ReelBit = stopReelData._secoundStop_Bit; // 第２停止はどのリールか
        // Debug.Log("第２停止リールの停止ビットは" + Convert.ToString(_secoundStop_ReelBit, 2));
        this._thirdStop_ReelBit = stopReelData._thirdStop_Bit; // 第３停止はどのリールか
        // Debug.Log("第３停止リールの停止ビットは" + Convert.ToString(_thirdStop_ReelBit, 2));
        // ▲▲▲------------ 押し順情報を取得 ------------▲▲▲


        // ▼▼▼------------ 第１停止の出目を取得 ------------▼▼▼
        string firstStop_ReelName = stopReelBit_ToReelName(_firstStop_ReelBit); // 対応リール名に変換
        DemeData demeData = DemeData.GetInstance();
        var firstDemeTuple = demeData.GetDemeDeta_int_tuple(firstStop_ReelName); // 出目情報をタプルで取得
        _firstStop_Deme_ints[2] = firstDemeTuple.jyoudann;
        Debug.Log("C方式Thiより F上段は " + _firstStop_Deme_ints[2]);
        _firstStop_Deme_ints[1] = firstDemeTuple.tyuudann;
        Debug.Log("C方式Thiより F中段は " + _firstStop_Deme_ints[1]);
        _firstStop_Deme_ints[0] = firstDemeTuple.gedann;
        Debug.Log("C方式Thiより F下段は " + _firstStop_Deme_ints[0]);
        // ▲▲▲------------ 第１停止の出目を取得 ------------▲▲▲


        // ▼▼▼------------ 第２停止の出目を取得 ------------▼▼▼
        string secoundStop_ReelName = stopReelBit_ToReelName(_secoundStop_ReelBit); // 対応リール名に変換
        var secoundDemeTuple = demeData.GetDemeDeta_int_tuple(secoundStop_ReelName); // 出目情報をタプルで取得
        _secoundStop_Deme_ints[2] = secoundDemeTuple.jyoudann;
        Debug.Log("C方式Thiより S上段は " + _secoundStop_Deme_ints[2]);
        _secoundStop_Deme_ints[1] = secoundDemeTuple.tyuudann;
        Debug.Log("C方式Thiより S中段は " + _secoundStop_Deme_ints[1]);
        _secoundStop_Deme_ints[0] = secoundDemeTuple.gedann;
        Debug.Log("C方式Thiより S下段は " + _secoundStop_Deme_ints[0]);
        // ▲▲▲------------ 第２停止の出目を取得 ------------▲▲▲


        // ▼▼▼----------- 第３停止のリール配列を取得 ---------▼▼▼
        string thirdStop_ReelName = stopReelBit_ToReelName(_thirdStop_ReelBit);
        ReelHairetuData reelHairetuData = ReelHairetuData.GetInstance();
        var tuple_7koma = reelHairetuData.Get_ZugaraNum_7koma_Tuple(stopKoma, thirdStop_ReelName);
        _thirdStop_ReelHairetu_ints[0] = tuple_7koma.gedann;
        _thirdStop_ReelHairetu_ints[1] = tuple_7koma.tyuudann;
        _thirdStop_ReelHairetu_ints[2] = tuple_7koma.jyoudann;
        _thirdStop_ReelHairetu_ints[3] = tuple_7koma.ue4Koma;
        _thirdStop_ReelHairetu_ints[4] = tuple_7koma.ue5Koma;
        _thirdStop_ReelHairetu_ints[5] = tuple_7koma.ue6Koma;
        _thirdStop_ReelHairetu_ints[6] = tuple_7koma.Ue7Koma;
        // ▲▲▲-----------第３停止のリール配列を取得-------- -▲▲▲

        _suberiKoma = 0; // 初期化

        // ▼▼▼----------- 引き込み制御 ------------▼▼▼
        var tenpai_Tuple = Order_TenpaiCheck(_firstStop_ReelBit, _secoundStop_ReelBit);
        tenpaiCheck_Order_first = tenpai_Tuple.first;
        tenpaiCheck_Order_secound = tenpai_Tuple.secound;

        if (Hikikomi_BitTenpaiCheck()) // 小役テンパイでtrue
        {
            Debug.Log("C方式第３　テンパイラインは " +(_tenpaiLine + 1) + " ライン");

            // テンパイしているので、ここでキャン引き込みチェック
            if (CanHikikomiCheck(stopKoma))
            {
                return _suberiKoma; // ここで抜けるので次の処理に行かない
            }

            // 小役取りこぼしの時に強い出目が出るようにする
        }
        // ▲▲▲----------- 引き込み制御 ------------▲▲▲


        // ▼▼▼----------- 蹴飛ばし制御 ------------▼▼▼
        if (Ketobashi_TenpaiCheck())
        {
            Debug.Log("テンパイしてる！ 数値系セットしたハズ");
        }

        for (int i = 0; i < 5; i++) // 最大４コマ(i<5で5回チェック)
        {
            Debug.Log("蹴飛ばし制御（非停止コマ） " + i + "回目のループ");
            // 止まってはいけないコマに止まってないかチェック
            if (KetobashiSeigyo_DontNyuusyou(flagData._currentCast.GetCastName())) // フラグ名の取得方法を改良
            {
                Debug.Log("ハズレなのに小役入賞しそうだから蹴飛ばし");
                continue; // if分がtrueだったら1コマ滑らせるから、次のループに戻り最初からチェックする
            }

            if (KetobasiSeigyo_DontStopKoma_Check(stopKoma, thirdStop_ReelName))
            {
                Debug.Log("ドントストップで蹴飛ばし");
                continue; // if分がtrueだったら1コマ滑らせるから、次のループに戻り最初からチェックする
            }

            return _suberiKoma;
        }
        // ▲▲▲----------- 蹴飛ばし制御 ------------▲▲▲



        // Debug.Log("C方式第３　テンパイ無しのためスベリ０コマ");
        return 0; // スベリコマ数

    }

    /// <summary>
    /// 蹴飛ばし制御（ハズレフラグだけどテンパイしているから揃わないように蹴飛ばす）
    /// </summary>
    /// <param name="flagName"></param>
    /// <returns></returns>
    bool KetobashiSeigyo_DontNyuusyou(string flagName)
    {
        if (flagName != "Hazure")
        {
            Debug.Log("成立役はリプ以上　抜けます");
            return false; // ハズレ以外は蹴飛ばし判定から抜けてメソッド終了
        }

        Debug.Log("テンパイチェック始めます フラグは" + _isTenpai );
        Debug.Log("テンパイチェック始めます テンパイラインは" + _tenpaiLine + " index目線");
        if (_isTenpai) // テンパイ有りの状態のみ小役入賞させない
        {
            int checkKomaindex = Order_ThirdKomaCheck(_firstStop_ReelBit, _secoundStop_ReelBit, _tenpaiLine); // チェックするコマを指定
            uint thirdStop_FlagBit = stopReelBit_TocurrentFlagBit(_thirdStop_ReelBit);
            if (_thirdStop_ReelHairetu_ints[checkKomaindex + _suberiKoma] == _tenmp_ThirdCheckKomaInt) // テンパイ
            {
                _suberiKoma++; // スベらせる
                return true;
            }
        }

        Debug.Log("たぶん小役入賞していない　第３R蹴飛ばし判定で最後まできた");
        return false; // 抜けてメソッド終了
    }



    /// <summary>
    /// 止まってはいけないコマに止まってないかチェック
    /// </summary>
    /// <param name="stopKoma"></param>
    /// <param name="secoundStopReel"></param>
    /// <returns></returns>
    bool KetobasiSeigyo_DontStopKoma_Check(int stopKoma, string secoundStopReel)
    {
        int checkKoma = stopKoma + _suberiKoma;
        if(checkKoma >= 21)
        {
            checkKoma = checkKoma - 21; // 21コマは存在しないので0コマ目に戻す
        }
        // Debug.Log("DontStopでチェックするコマは " + checkKoma + " コマ目");
        if (secoundStopReel == "ReelLeftObj")
        {
            if (_dontStopKoma_Left.Contains(checkKoma)) // 停止してはいけないコマ数だった場合 スベる
            {
                _suberiKoma++;
                return true;
            }
        }

        if (secoundStopReel == "ReelCenterObj")
        {
            if (_dontStopKoma_Center.Contains(checkKoma)) // 停止してはいけないコマ数だった場合 スベる
            {
                _suberiKoma++;
                return true;
            }
        }

        if (secoundStopReel == "ReelRightObj")
        {
            if (_dontStopKoma_Right.Contains(checkKoma)) // 停止してはいけないコマ数だった場合 スベる
            {
                _suberiKoma++;
                return true;
            }
        }

        return false; // ここまで来たらスベリ無し
    }

    /// <summary>
    /// 蹴飛ばし制御用のテンパイチェック
    /// </summary>
    /// <returns></returns>
    bool Ketobashi_TenpaiCheck()
    {
        // uint firstStop_FlagBit = stopReelBit_TocurrentFlagBit(_firstStop_ReelBit);
        // uint secoundStop_FlagBit = stopReelBit_TocurrentFlagBit(_secoundStop_ReelBit);

        _tenpaiLine = 99; // テンパイライン初期化　エラーわかるように99
        _isTenpai = false; // 初期化　テンパイ無し
        _tenmp_ThirdCheckKomaInt = 99; // 初期化　エラーわかるように99

        for (int indexCount = 0; indexCount < 5; indexCount++) // indexは0スタートなので5になったら0に戻す
        {
            // テンパイチェック　第１出目と第２出目が等しいか？のチェック（チェリーは例外）
            if (_firstStop_Deme_ints[tenpaiCheck_Order_first[indexCount]] == _secoundStop_Deme_ints[tenpaiCheck_Order_secound[indexCount]])
            {
                Debug.Log("C方式第３より " + (indexCount + 1) +  "ラインでテンパイしてます!!（入賞ライン目線）");
                _tenpaiLine = indexCount; // これがテンパイしているライン
                _isTenpai = true; // テンパイ有りフラグ　「テンパイ有り」の場合のみ_tenpaiLineが信頼できる数値になる
                _tenmp_ThirdCheckKomaInt = _secoundStop_Deme_ints[tenpaiCheck_Order_secound[indexCount]]; // テンパイしてるラインの小役を代入（蹴飛ばし制御用）
                return true; // テンパイあり
            }
            Debug.Log("テンパイチェック " + indexCount + " ライン目");
        }

        Debug.Log("C方式第３より 小役テンパイ無し!!");
        return false; // テンパイ無しという事で falseを返す
    }



    /// <summary>
    /// 引き込み制御用のテンパイチェック（成立フラグとチェックする）
    /// </summary>
    /// <returns></returns>
    bool Hikikomi_BitTenpaiCheck()
    {
        uint firstStop_FlagBit = stopReelBit_TocurrentFlagBit(_firstStop_ReelBit);
        uint secoundStop_FlagBit = stopReelBit_TocurrentFlagBit(_secoundStop_ReelBit);

        _tenpaiLine = 99; // テンパイライン初期化　エラーわかるように99
        _isTenpai = false; // 初期化　テンパイ無し
        _tenmp_ThirdCheckKomaInt = 99; // 初期化　エラーわかるように99

        for (int indexCount = 0; indexCount < 5; indexCount++) // indexは0スタートなので5になったら0に戻す
        {

            // 第１出目と成立フラグのチェック（出目情報の配列は3つの要素しか無いが、チェックする順番は5つの要素がある
            if (_firstStop_Deme_ints[ tenpaiCheck_Order_first [indexCount] ] == firstStop_FlagBit)
            {
                // 第２停止の出目と成立フラグをチェック（第１出目と成立フラグが正しい場合）
                if (_secoundStop_Deme_ints[tenpaiCheck_Order_secound[ indexCount ] ] == secoundStop_FlagBit)
                {   
                    // Debug.Log("C方式第３より " + (indexCount + 1) +  "ラインでテンパイしてます!!（入賞ライン目線）");
                    _tenpaiLine = indexCount; // これがテンパイしているライン
                    _isTenpai = true; // テンパイ有りフラグ　「テンパイ有り」の場合のみ_tenpaiLineが信頼できる数値になる
                    _tenmp_ThirdCheckKomaInt = _secoundStop_Deme_ints[tenpaiCheck_Order_secound[indexCount]]; // テンパイしてるラインの小役を代入（蹴飛ばし制御用）
                    return true; // テンパイあり
                }
            }

            // Debug.Log("テンパイチェック " + indexCount + " ライン目");
        }

        // Debug.Log("C方式第３より 小役テンパイ無し!!");
        return false; // テンパイ無しという事で falseを返す
    }


    /// <summary>
    /// テンパイラインまで小役を引き込めるか（第３停止）チェックする
    /// </summary>
    /// <returns></returns>
    bool CanHikikomiCheck(int stopKoma)
    {


        //// この下はチェック用
        //for (int i = 0; i < 7; i++)
        //{
        //    Debug.Log(_thirdStop_ReelHairetu_ints[i] + "   戻り値で返ってきた第３停止のリール配列");
        //}

        int index = Order_ThirdKomaCheck(_firstStop_ReelBit, _secoundStop_ReelBit, _tenpaiLine);
        // Debug.Log("検証する位置は " + index);
        ///////////// ↑のここで算出したインデックスを判定に使う
        uint thirdStop_FlagBit = stopReelBit_TocurrentFlagBit(_thirdStop_ReelBit);

        // 引き込めるかチェック（この１コマだけのチェックで十分）
        for (int suberiCount = 0; suberiCount < 7 - index; suberiCount++) // 開始から上段とかだと要素数オーバーするからループ上限にindex足す
        {
            // Debug.Log("要素数は " + (index + suberiCount));
            if(_thirdStop_ReelHairetu_ints[index + suberiCount] == thirdStop_FlagBit)
            {
                //Debug.Log("引き込み!");
                _suberiKoma = suberiCount;
                return true;
            }
            // Debug.Log("第３停止 チェック" + suberiCount + "回目");
        }

        // Debug.Log("引き込み範囲内に小役無し");
        return false;
    }


    /// <summary>
    /// 押し順を元に、テンパイチェックのindexを返す
    /// </summary>
    /// <param name="F_bit"></param>
    /// <param name="S_Bit"></param>
    /// <returns></returns>
    public (int[] first, int[] secound)Order_TenpaiCheck(uint F_bit, uint S_Bit)
    {
        int[] first = new int[5]   { 1, 2, 0, 0, 0}; // 上中段は120で固定 クロスは下で書き換え
        int[] secound = new int[5] { 1, 2, 0, 0, 0}; // 上中段は120で固定 クロスは下で書き換え

        if (F_bit == 0b1010) // 中押しなら（中右も中左も同じ）
        {
            if(S_Bit == 0b1001) // 中右なら
            {
                first[3] = 1;
                first[4] = 1;
                secound[3] = 0;
                secound[4] = 2;
                // Debug.Log("C方式より　中右");
                return (first, secound);
            }
            if (S_Bit == 0b1100) // 中左なら
            {
                first[3] = 1;
                first[4] = 1;
                secound[3] = 2;
                secound[4] = 0;
                // Debug.Log("C方式より　中押し");
                return (first, secound);
            }
        }

        if(F_bit == 0b1100) // 左ファーストなら
        {
            if(S_Bit == 0b1010) // 順押しなら
            {
                first[3] = 2;
                first[4] = 0;
                secound[3] = 1;
                secound[4] = 1;
                // Debug.Log("C方式より　順押し");
                return (first, secound);
            }
            if(S_Bit == 0b1001) // 順ハサミなら
            {
                first[3] = 2;
                first[4] = 0;
                secound[3] = 0;
                secound[4] = 2;
                // Debug.Log("C方式より　順ハサミ");
                return (first, secound);
            }
        }

        if(F_bit == 0b1001) // 右ファーストなら
        {
            if (S_Bit == 0b1010) // 逆押しなら
            {
                first[3] = 0;
                first[4] = 2;
                secound[3] = 1;
                secound[4] = 1;
                // Debug.Log("C方式より　逆押し");
                return (first, secound);
            }
            if (S_Bit == 0b1100) // 逆ハサミなら
            {
                first[3] = 0;
                first[4] = 2;
                secound[3] = 2;
                secound[4] = 0;
                // Debug.Log("C方式より　逆ハサミ");
                return (first, secound);
            }

        }

        Debug.LogError("おそらく押し順判定でエラーです");
        return (first, secound);
    }


    /// <summary>
    /// 押し順を元に、テンパイチェックのindexを返す
    /// </summary>
    /// <param name="F_bit"></param>
    /// <param name="S_Bit"></param>
    /// <returns></returns>
    public int Order_ThirdKomaCheck(uint F_bit, uint S_Bit, int tenpaiLine)
    {
        int[] tempIndex = new int[5] { 1, 2, 0, 0, 0 }; // 初期状態

        if (F_bit == 0b1010) // 中押しなら（中右も中左も同じ）
        {
            if (S_Bit == 0b1100) // 中左なら
            {
                tempIndex[3] = 0;
                tempIndex[4] = 2;
            }
            if (S_Bit == 0b1001) // 中右なら
            {
                tempIndex[3] = 2;
                tempIndex[4] = 0;
            }
        }

        if (F_bit == 0b1100) // 左ファーストなら
        {
            if (S_Bit == 0b1010) // 順押しなら
            {
                tempIndex[3] = 0;
                tempIndex[4] = 2;
            }
            if (S_Bit == 0b1001) // 順ハサミなら
            {
                tempIndex[3] = 1;
                tempIndex[4] = 1;
            }
        }

        if (F_bit == 0b1001) // 右ファーストなら
        {
            if (S_Bit == 0b1010) // 逆押しなら
            {
                tempIndex[3] = 2; // ここ直す必要あるかも20230708
                tempIndex[4] = 0;
            }
            if (S_Bit == 0b1100) // 逆ハサミなら
            {
                tempIndex[3] = 1;
                tempIndex[4] = 1;
            }

        }

        return tempIndex[tenpaiLine];
    }



    /// <summary>
    /// フラグのBitを分解　　右シフトして左、中、右を抜き取る
    /// </summary>
    /// <param name="flagBit"></param>
    void FlagBit_8bitSplit(uint flagBit)
    {
        _currentFlag_LeftBit = flagBit >> 16; // 左の8bitだけ残る
        // Debug.Log("左の8bitは " + _currentFlag_LeftBit);

        _currentFlag_CenterBit = flagBit >> 8;
        _currentFlag_CenterBit = _currentFlag_CenterBit & 0b_0000_0000_1111_1111; // 1と1意外は0になる
        // Debug.Log("中の8bitは " + _currentFlag_CenterBit);

        _currentFlag_RightBit = flagBit & 0b_0000_0000_0000_0000_1111_1111; // 右の8bitだけ残る　1と1意外は0になる
        // Debug.Log("右の8bitは " + _currentFlag_RightBit);
    }


    /// <summary>
    /// 変換系　Bitを元に対応するリール名を返す
    /// </summary>
    /// <param name="argsBit"></param>
    /// <returns></returns>
    string stopReelBit_ToReelName(uint argsBit)
    {
        if(argsBit == 0b1100) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return "ReelLeftObj";
        }
        else if (argsBit == 0b1010) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return "ReelCenterObj";
        }
        else if(argsBit == 0b1001) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return "ReelRightObj";
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 変換系　Bitを元に対応するcurrentBitを返す
    /// </summary>
    /// <param name="argsBit"></param>
    /// <returns></returns>
    uint stopReelBit_TocurrentFlagBit(uint argsBit)
    {
        if (argsBit == 0b1100) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return _currentFlag_LeftBit;
        }
        else if (argsBit == 0b1010) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return _currentFlag_CenterBit;
        }
        else if (argsBit == 0b1001) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return _currentFlag_RightBit;
        }
        else
        {
            Debug.LogError("C方式 第３より 該当しません");
            return 0b0000;
        }
    }
}
