using System.Collections;
using UnityEngine;


/* 代理结构脚本 */

public delegate void EricAction();

public delegate void EricAction<T>(T dto);

public delegate void EricAction<T0, T1>(T0 i, T1 j);

public delegate void EricAction<T0, T1, T2>(T0 i, T1 j, T2 k);

public delegate IEnumerator EricAction_IEnumerator();

public delegate int EricAction_int();

public delegate TResult AlonFunc<in T, out TResult>(T arg);

public delegate TResult AlonFunc<in T0, in T1, out TResult>(T0 arg0, T1 arg1);

