using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace State
{

	public abstract class StateBase
	{
		// 「virtual キーワード」は派生クラスでオーバーライド可能という意味


		/// <summary>
		/// ステート開始時に呼ばれる処理
		/// </summary>
		/// <param name="owner"> 誰が呼んだか </param>
		/// <param name="prevState"> 一つ前のステート </param>
		public virtual void OnEnter(StateController owner, StateBase prevState)
		{
			
		}


		/// <summary>
		/// 毎フレームのアップデートで呼ばれる処理  UpDateと同じ
		/// </summary>
		/// <param name="owner"> 誰が呼んだか </param>
		public virtual void OnUpdate(StateController owner) { }


		/// <summary>
		/// ボタンクリック時に呼ばれる
		/// </summary>
		/// <param name="owner"> 誰が呼んだか </param>
		/// <param name="nextState"> 次に遷移するステート </param>
		public virtual void OnClick(StateController owner) { }


		/// <summary>
		/// ステート終了時に呼ばれる
		/// </summary>
		/// <param name="owner"> 誰が呼んだか </param>
		/// <param name="nextState"> 次に遷移するステート </param>
		public virtual void OnExit(StateController owner, StateBase nextState) { }

	}
}
