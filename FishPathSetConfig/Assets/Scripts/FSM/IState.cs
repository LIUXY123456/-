/*************************************************************************
 *  Copyright © 2019-2020 BaiPing. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IState.cs
 *  Author       :  BaiPing
 *  Date         :  12/12/2019
 *  Description  :  有限状态机状态接口
 *************************************************************************/



using UnityEngine;


namespace BPLaBaGame.FSM
{
    public interface IState
    {
        /// <summary> 拥有者 </summary>
        Object owner { get; set; }

        /// <summary> 前一个状态 </summary>
        IState previousState { get; set; }

        /// <summary> 切换状态时传过来的参数 </summary>
        object[] parameters { get; set; }

        /// <summary> 进入状态时调用，只调用一次 </summary>
        void StateEnter();

        /// <summary> 状态更新，用户可自己控制调用间隔 </summary>
        void StateUpdate();

        /// <summary> 切换状态时调用，只调用一次 </summary>
        void StateEnd();

        string Tostring();
    }
}