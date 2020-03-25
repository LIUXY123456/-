/*************************************************************************
 *  Copyright © 2019-2020 BaiPing. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  New.cs
 *  Author       :  BaiPing
 *  Date         :  3/6/2020
 *  Description  :  
 *************************************************************************/



using UnityEngine;
using System;
using BPLaBaGame.FSM;
using System.Collections.Generic;

namespace SubGame_NewCatchFish
{

	public class NewCatchFish_fishBase : MonoBehaviour
	{
        /// <summary>
        /// 击中事件
        /// </summary>
        public Action OnHit;
        /// <summary>
        /// 冰冻事件
        /// </summary>
        public Action<float> OnFrozen;
        /// <summary>
        /// 死亡事件
        /// </summary>
        public Action OnDead;
        /// <summary>
        /// 进入鱼塘
        /// </summary>
        public Action OnEnterFishpond;
        /// <summary>
        /// 离开鱼塘
        /// </summary>
        public Action OnLeavaFishpond;

        public FSM fishfsm;
        public FishPath fishPath;
        public  NewCatchFish_FishInfo fishInfo;

        public int currentPathIndex;

       // int ICollectable_Type_Eric.Type { get ; set ; }

        public virtual void InitFish(NewCatchFish_FishInfo fishInfo)
        {
           
        }

        public virtual void FixedUpdate()
        {
            if (fishfsm!=null &&
                fishfsm.currentState!=null)
            {
                fishfsm.FSMUpdate();
            }
        }

	}



    public class Fish
    {
        /// <summary>
        /// 鱼的ID
        /// </summary>
        public int fishId;
        /// <summary>
        /// 鱼的类型
        /// </summary>
        public byte fishKind;
        /// <summary>
        /// 鱼的路线数组
        /// </summary>
        public Dictionary<int, FishPath> fishPath;
        
    }
    [System.Serializable]
    public class FishPath
    {
        /// <summary>
        /// 路线ID
        /// </summary>
        public int pathId;
        /// <summary>
        /// 路线点坐标字典：key是坐标，value是速度（第一个速度为0）
        /// </summary>
        public List<FishVecter> fishPoint;
        /// <summary>
        /// 路线需要的时间
        /// </summary>
        public float costTime;

        /// <summary>
        /// 路线名称（临时）
        /// </summary>
        public string pathName;

       
    }
    public class FishVecter
    {
        public float x;
        public float y;
        public float speed;
        public float time;
        public FishVecter() { }
       
       
    }

    #region 数据模型


    /// <summary>
    /// 捕鱼房间信息
    /// </summary>
    public class NewCatchFish_Dto
    {
        public string RoomInfos;//所有房间配置信息

        public NewCatchFishRoom[] RealRoomInfos;//所有房间配置信息
        public int[] LeftTimes;//剩余体验次数
        public int[] PlayerNums;//房间内人数
        public int[] RealTableNums;//真实的桌子数
    }
    /// <summary>
    /// 新捕鱼房间信息
    /// </summary>
    public class NewCatchFishRoom
    {
        public int RoomId;
        public string RoomName;
        public int MaxPlayTime;
        public long GoldLimit;
        public bool IsOpen;
        public long MinBet;
        public long MaxBet;
        public long MaxBetLevel;
    }
    public class SendMSg_RoomID
    {
        public int roomid;
    }
    public class SendMSg_RoomEnter
    {
        public int roomid;
        public int tableId;
    }

    /// <summary>
    /// 房间内桌子信息
    /// </summary>
    public class FishTableInfo
    {
        public long UserId;     //用户id
        public string UserName; //用户名
        public string Avatar;   //头像
        public int Spot;        //座位号
    }
    public class NewCatchFish_RoomTables_Dto
    {
        public Dictionary<int, string> TablePlayers;
        public Dictionary<int, FishTableInfo[]> RealTablePlayers;
    }
    /// <summary>
    /// 捕鱼用户数据信息
    /// </summary>
    public class NewCatchFish_PlayerInfo
    {
        public long UserId;     //用户id
        public string UserName; //用户昵称
        public string Avatar;   //用户头像
        public int Spot;        //座位号
        public long Balance;    //用户余额
        public bool IsRobot;    //是否是机器人
        public int BetLevel;    //炮台等级
    }
    /// <summary>
    /// 捕鱼的鱼信息
    /// </summary>
    public class NewCatchFish_FishInfo
    {
        public int FishID;      //鱼的唯一id
        public int FishKind;    //鱼的类型
        public int PathId;      //鱼的路线id
        public long CreateTime; //鱼的生成时间
        public long CostTime;   //鱼的生命时长
    }

    /// <summary>
    /// 捕鱼炮弹信息
    /// </summary>
    public class NewCatchFish_BulletInfo
    {
        public long userId;        //发射者用户id
        public int Spot;           //发射者座位号
        public int Angle;          //炮弹角度
        public int ShellId;        //炮弹id
        public int ShellKind;     //炮弹类型（0,普通1，激光，2爆炸，3 闪电）
        public int LockFishId;     //锁定的鱼id(-1为没有锁定)
        public long CreateTime;    //生成时间(客户端根据差值算出来的当前时间，如果发送过来跟服务器时间偏差太大，会被丢弃)
    }

    /// <summary>
    /// 当前鱼塘信息
    /// </summary>
    public class CurrentFishpondInfo
    {
        public int RoomId;                            //房间id
        public int TableID;                           //桌子id
        public string Players;                        //房间内玩家信息
        public Dictionary<int, NewCatchFish_FishInfo> FishDatas;     //所有鱼的数据
        public Dictionary<int, NewCatchFish_BulletInfo> ShellDatas;    //所有炮弹数据
        public int SceneId;                           //当前场景（0为普通，其他为鱼潮）
        public long SceneCreateTime;                  //当前场景创建的时间（开始刷鱼的时间）
        public long NowTime;                          //当前服务器时间

        public NewCatchFish_PlayerInfo[] RealPalyers;
        public Dictionary<int, NewCatchFish_FishInfo> RealFishDatas;
        public Dictionary<int, NewCatchFish_BulletInfo> RealBullets;
    }

    /// <summary>
    /// 请求击中鱼群
    /// </summary>
    public class NewCatchFish_HitFishGroup_Msg
    {
        public int shellID;
        public int[] fishIds;
    }
    /// <summary>
    /// 请求击中一只鱼
    /// </summary>
    public class NewCatchFish_HitFish_Msg
    {
        public int shellID;
        public int fishId;
    }
    /// <summary>
    /// 请求切换炮台等级
    /// </summary>
    public class NewCatchFish_SwitchBetLevel
    {
        public long switchUserId;
        public int betLevel;
    }


    #endregion

    #region 消息广播
    public class NewCatchFishEnterRoom_Dto
    {
        public NewCatchFish_PlayerInfo FishPlayerInfo;
    }
    public class NewCatchFishExitRoom_Dto
    {
        public long userId;
        public int spot;
    }
    public class NewCatchFishLaunch_Dto
    {
        public int shellId;
        public long balance;
    }
    public class NewCatchFishDie_Dto
    {
        public long userId;
        public int spot;
        public int fishId;
        public long Balance;
        public long bonus;//奖金
    }
    public class NewCatchFishNewFish_Dto
    {
        public NewCatchFish_FishInfo FishFish;
    }
    public class NewCatchFishSwitchBetLevel_Dto
    {
        public long userId;
        public int spot;
        public int betLevel;
    }
    public class NewCatchFishSwitchScene_Dto
    {
        public int sceneId;
        public long sceneCreateTime;
    }
    public class NewCatchFishLineShell_Dto
    {
        public long userId;
        public int spot;
        public long beginTime;
        public long endTime;
    }
    public class NewCatchFishIceScene_Dto
    {
        public long beginTime;
        public long endTime;
    }
    public class NewCatchFishRobotRelayOn_Dto
    {
        public int robotPos;
        public int relyOnPos;
    }
    #endregion



























}
