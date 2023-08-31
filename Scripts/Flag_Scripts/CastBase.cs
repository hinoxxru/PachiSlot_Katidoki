using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 小役インターフェイス
public interface ICastBase
{
    public string GetCastName();
    public int    GetCastNumber();
    public uint   GetCastBit();
    public int    GetPayOutMedal();
    public bool   GetCastIsBonus();
}

// ボーナス用インターフェイス
public interface IBonusBase
{
    public string GetBonusType();
    public int GetMaxPayOut();
}


public class Cast_Hazure : ICastBase
{
    string _castName = "Hazure";
    int    _castNumber = 0;
    uint   _castBit = 0b_00000000_00000000_00000000;
    int    _payOutMedal = 0;
    bool   _isBonus = false;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }
}


public class Cast_Replay : ICastBase
{
    string _castName = "Replay";
    int    _castNumber = 1;
    uint   _castBit = 0b_00000001_00000001_00000001;
    int    _payOutMedal = 0;
    bool _isBonus = false;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }
}

public class Cast_Bell : ICastBase
{
    string _castName = "Bell";
    int _castNumber = 2;
    uint _castBit = 0b_00000010_00000010_00000010;
    int _payOutMedal = 10;
    bool _isBonus = false;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }
}

public class Cast_Suika : ICastBase
{
    string _castName = "Suika";
    int _castNumber = 4;
    uint _castBit = 0b_00000100_00000100_00000100;
    int _payOutMedal = 8;
    bool _isBonus = false;

    public string GetCastName(){ return _castName; }
    public int GetCastNumber(){ return _castNumber; }
    public uint GetCastBit(){ return _castBit; }
    public int GetPayOutMedal(){ return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }
}

public class Cast_Cherry : ICastBase
{
    string _castName = "Cherry";
    int _castNumber = 8;
    uint _castBit = 0b_00001000_00001000_00001000;
    int _payOutMedal = 2;
    bool _isBonus = false;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }
}

public class Cast_RB : ICastBase , IBonusBase
{
    string _castName = "Reg";
    int _castNumber = 16;
    uint _castBit = 0b_00010000_00010000_00010000;
    int _payOutMedal = 0;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 144; // 3枚掛け 12枚Pay  Get 108枚
    public string GetBonusType() { return "RB"; }
    public int GetMaxPayOut() { return _maxPayOut; }

}

public class Cast_BB : ICastBase , IBonusBase
{
    string _castName = "Big";
    int _castNumber = 32;
    uint _castBit = 0b_00100000_00100000_00100000;
    int _payOutMedal = 0;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 420; //BigBonus の最大払出枚数  3枚掛け12枚Pay Get310枚
    public string GetBonusType() { return "BB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}


// ------------- ここから試作（8/28はまだ宣言してない） -----------------


public class Cast_RBAndReplay : ICastBase, IBonusBase
{
    string _castName = "RBAndReplay";
    int _castNumber = 17;
    uint _castBit = 0b_00010001_00010001_00010001;
    int _payOutMedal = 0;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 144; // 3枚掛け 12枚Pay  Get 108枚
    public string GetBonusType() { return "RB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}


public class Cast_RBAndBell : ICastBase, IBonusBase
{
    string _castName = "Cast_RBAndBell";
    int _castNumber = 18;
    uint _castBit = 0b_00010010_00010010_00010010;
    int _payOutMedal = 10;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 144; // 3枚掛け 12枚Pay  Get 108枚
    public string GetBonusType() { return "RB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}


public class Cast_RBAndSuika : ICastBase, IBonusBase
{
    string _castName = "Cast_RBAndSuika";
    int _castNumber = 20;
    uint _castBit = 0b_00010100_00010100_00010100;
    int _payOutMedal = 8;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 144; // 3枚掛け 12枚Pay  Get 108枚
    public string GetBonusType() { return "RB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}
 
public class Cast_RBCherry : ICastBase, IBonusBase
{
    string _castName = "Cast_RBCherry";
    int _castNumber = 24;
    uint _castBit = 0b_00011000_00011000_00011000;
    int _payOutMedal = 2;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 144; // 3枚掛け 12枚Pay  Get 108枚
    public string GetBonusType() { return "RB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}

public class Cast_BBAndReplay : ICastBase, IBonusBase
{
    string _castName = "BBAndReplay";
    int _castNumber = 33;
    uint _castBit = 0b_00100001_00100001_00100001;
    int _payOutMedal = 0;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 420; //BigBonus の最大払出枚数  3枚掛け12枚Pay Get310枚
    public string GetBonusType() { return "BB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}


public class Cast_BBAndBell : ICastBase, IBonusBase
{
    string _castName = "BBAndBell";
    int _castNumber = 34;
    uint _castBit = 0b_00100010_00100010_00100010;
    int _payOutMedal = 10;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 420; //BigBonus の最大払出枚数  3枚掛け12枚Pay Get310枚
    public string GetBonusType() { return "BB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}

public class Cast_BBAndSuika : ICastBase, IBonusBase
{
    string _castName = "BBAndSuika";
    int _castNumber = 36;
    uint _castBit = 0b_00100100_00100100_00100100;
    int _payOutMedal = 8;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 420; //BigBonus の最大払出枚数  3枚掛け12枚Pay Get310枚
    public string GetBonusType() { return "BB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}


public class Cast_BBAndCherry : ICastBase, IBonusBase
{
    string _castName = "BBAndCherry";
    int _castNumber = 40;
    uint _castBit = 0b_00101000_00101000_00101000;
    int _payOutMedal = 2;
    bool _isBonus = true;

    public string GetCastName() { return _castName; }
    public int GetCastNumber() { return _castNumber; }
    public uint GetCastBit() { return _castBit; }
    public int GetPayOutMedal() { return _payOutMedal; }
    public bool GetCastIsBonus() { return _isBonus; }

    // Bonus要素
    int _maxPayOut = 420; //BigBonus の最大払出枚数  3枚掛け12枚Pay Get310枚
    public string GetBonusType() { return "BB"; }
    public int GetMaxPayOut() { return _maxPayOut; }
}

