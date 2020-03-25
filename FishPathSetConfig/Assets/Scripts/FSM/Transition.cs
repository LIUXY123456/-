/*************************************************************************
 *  Copyright © 2019-2020 BaiPing. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Transition.cs
 *  Author       :  BaiPing
 *  Date         :  12/12/2019
 *  Description  :  有限状态机切换状态转换条件
 *************************************************************************/

using System;

namespace BPLaBaGame.FSM
{
    public class Transition
    {
        /// <summary> 正在执行的状态 </summary>
        public IState from { get; set; }

        /// <summary> 将要切换的状态 </summary>
        public IState to { get; set; }

        /// <summary> 状态切换条件 </summary>
        public Func<bool> transition;

        /// <summary> 构造函数 </summary>
        public Transition(IState from, IState to)
        {
            this.from = from;
            this.to = to;
        }
    }
}