using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
//using UniRx;
//using UniRx.Triggers;

public partial class StateController : MonoBehaviour
{
    // --------↓↓ ここからシングルトンのコード ↓↓-------------
    private static StateController instance;
    public static StateController Instance
    {

        get
        {

            if (instance == null)
            {
                StateController[] instances = null;
                instances = FindObjectsOfType<StateController>();
                if (instances.Length == 0)
                {
                    Debug.LogError("StateControllerのインスタンスが存在しません");
                    return null;
                }
                else if (instances.Length > 1)
                {
                    Debug.LogError("StateControllerのインスタンスが複数存在します");
                    return null;
                }
                else
                {
                    instance = instances[0];
                }
            }
            return instance;
        }
    }

    // -----------↑↑ ここまではシングルトンのコード ↑↑------------



    // ★★これ以下はステート管理に関係しないコード↓↓↓---------------------------

    private Rigidbody _rigidBody;
    private Vector3 defaultScale;
    [SerializeField] private Vector3 deathAddForce = Vector3.zero;
    [SerializeField] private Vector3 deathAddTorque = Vector3.zero;

    public GameObject _reelLeft;
    public GameObject _reelCenter;
    public GameObject _reelRight;

    // ★★これ以上はステート管理に関係しないコード↑↑↑---------------------------



    // ステートのインスタンス  readonlyにすると値の変更がされない変数にできる（コンストラクタ内でのみ値を変更できる）
    private static readonly StateInitialize stateInitialize = new StateInitialize();
    private static readonly StateMaxBet stateMaxBet = new StateMaxBet();
    private static readonly StateLeverOn stateLeverOn = new StateLeverOn();
    private static readonly StateWaitTime stateWaitTime = new StateWaitTime();
    private static readonly StateFirstStopButton stateFirstStopButton = new StateFirstStopButton();
    private static readonly StateSecondStopButton stateSecondStopButton = new StateSecondStopButton();
    private static readonly StateThirdStopButton stateThirdStopButton = new StateThirdStopButton();
    private static readonly StateResult stateResult = new StateResult();
    private static readonly StatePayOut statePayOut = new StatePayOut();



    /// <summary>
    /// 現在のステート
    /// </summary>
    private StateBase currentState = stateInitialize;






    // ｱﾏｶﾞﾐﾅ見本だと ここはpartialでクラスを２分割し OnStart()という別メソッドにして呼び出している
    private void Start()
    {
        currentState.OnEnter(this, null);
        _rigidBody = GetComponent<Rigidbody>();
        defaultScale = transform.localScale;
    }


    // ｱﾏｶﾞﾐﾅ見本だと ここはpartialでクラスを２分割し OnUpdate()という別メソッドにして呼び出している
    private void Update()
    {
        currentState.OnUpdate(this);

        if (Input.GetMouseButtonDown(0)) // リールストップが遅延するバグを修正するためDownにした(UniRx使って0.1秒待たないと全リール同時に止まるから注意）
        {
            OnClick();
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    OnClick();
        //}

    }


    public void OnClick()
    {
        // Debug.Log("Clickされました");
        // Debug.Log(currentState);
        currentState.OnClick(this);

    }



    // ステート変更
    public void ChangeState(StateBase nextState)
    {
        // Debug.Log("1.現在S " + currentState);
        // Debug.Log("　2.Next " + nextState);

        // currentState.OnExit(this, currentState);
        // nextState.OnEnter(this, nextState);
        currentState.OnExit(this, nextState);
        nextState.OnEnter(this, currentState);
        currentState = nextState;

        // Debug.Log("　　3.現在S " + currentState);
        // Debug.Log("　　　4.Next " + nextState);
    }

}

