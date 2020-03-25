/*************************************************************************
 *  Copyright © 2019-2020 BaiPing. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  FSM.cs
 *  Author       :  BaiPing
 *  Date         :  12/12/2019
 *  Description  :  有限状态机控制器
 *************************************************************************/

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using EricFramework;

namespace BPLaBaGame.FSM
{
    public class FSM
    {

        /// <summary> 默认状态   </summary>
        public IState defaultState;

        /// <summary>  当前状态  </summary>
        public IState currentState;

        /// <summary>   状态集合 </summary>
        public List<IState> states = new List<IState>();

        /// <summary>  状态与转换条件的绑定集合  </summary>
        public Dictionary<IState, List<Transition>> transitions = new Dictionary<IState, List<Transition>>();

        private float pauseTime;
        /// <summary> 状态机初始化   </summary>
        public void Init()
        {
            if (states == null)
            {
                Debug.LogError("states is null!!");
                return;
            }
            if (transitions == null)
            {
                Debug.LogError("transition is null!!");
                return;
            }
            if (defaultState == null)
            {
                defaultState = states[0];
            }
            defaultState.StateEnter();
            currentState = defaultState;
            pauseTime = 0;
        }

        /// <summary> 状态机更新   </summary>
        public void FSMUpdate()
        {
            if (currentState == null)
            {
                Debug.LogError("currentstate is null!!!");
                return;
            }
            if (pauseTime>0)
            {
                pauseTime -= Time.deltaTime;
                Debug.Log("<color=purple>FSM Pause!! time left:"+pauseTime+"</color>");
                return;
            }
            else
            {
                pauseTime = 0;
            }
            //遍历一次当前状态的转换条件集合，当可以转换时，掉用当前状态的exit，调用下一个状态的enter
            foreach (var item in transitions[currentState])
            {
                if (item.transition())
                {
                    ChangeState(item.to);
                }
            }
            //当没有状态可以转换时，状态更新
            currentState.StateUpdate();
        }

        /// <summary>  转换状态，默认为自动调用，为了灵活，也可强制转换状态  </summary>
        public void ChangeState(IState state, params object[] parameters)
        {
            currentState.StateEnd();
            state.previousState = currentState;
            state.parameters = parameters;
            currentState = state;
            currentState.StateEnter();
        }

        /// <summary>  转换状态，默认为自动调用，为了灵活，也可强制转换状态  </summary>
        public void ChangeState(IState state)
        {
            currentState.StateEnd();
            state.previousState = currentState;

            currentState = state;
            currentState.StateEnter();
        }
        /// <summary>  转换状态，默认为自动调用，为了灵活，也可强制转换状态  </summary>
        public void ChangeState(string state,params object[] parameters)
        {
            foreach (var item in states)
            {
                if (item.Tostring()==state)
                {
                    ChangeState(item,parameters);
                    return; 
                }
            }
            Debug.LogError("没有找到此状态，请检查状态机初始化时是否添加该状态：" + state);
          
        }
        /// <summary>  转换状态，默认为自动调用，为了灵活，也可强制转换状态  </summary>
        //public void ChangeState(string state, float delay,params object[] parameters)
        //{
        //    foreach (var item in states)
        //    {
        //        if (item.Tostring() == state)
        //        {
        //            GlobalMain.Instance.StartCoroutine(DelayInvoke(item, delay, parameters));
        //            return;
        //        }
        //    }
        //    Debug.LogError("没有找到此状态，请检查状态机初始化时是否添加该状态：" + state);

        //}

        IEnumerator DelayInvoke(IState item, float delay, params object[] parameters)
        {
            yield return new WaitForSeconds(delay);
            ChangeState(item, parameters);
        }

        /// <summary>  添加转换条件  </summary>
        public void AddTransition(Transition transition)
        {
            if (!transitions.ContainsKey(transition.from))
            {
                transitions.Add(transition.from, new List<Transition>());
            }
            transitions[transition.from].Add(transition);
        }

        /// <summary>  添加状态  </summary>
        public void AddState(IState state, UnityEngine.Object obj=null)
        {
            states.Add(state);
            state.owner = obj;
            if (!transitions.ContainsKey(state))
            {
                transitions.Add(state, new List<Transition>());
            }
        }

        public void SetPause(float time)
        {
            pauseTime = time;
        }
    }
}