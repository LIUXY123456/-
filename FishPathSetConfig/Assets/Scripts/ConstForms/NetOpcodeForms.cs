using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EricFramework
{
    public class NetOpcodeForms
    {
        #region 公共
        public const string Common_KickOff = "kick_off";

        public const string Game_Notice = "barrage";

        #endregion

        #region 拉霸

        public const string Laba_Pond = "/laba/amount/pool";

        #endregion

        #region 捕鱼
        public const string Buyu_launch = "catchFish/launch";
        public const string Buyu_boom = "catchFish/boom";
        public const string Buyu_catchFish = "catchFish/catchFish";
        public const string Buyu_newFish = "catchFish/newFish";
        public const string Buyu_specialEvent = "catchFish/specialEvent";
        public const string Buyu_catchSpecial = "catchFish/catchSpecial";
        public const string Buyu_playerEnter = "catchFish/scenes";
        public const string Buyu_playerExit = "catchFish/offline";
        public const string Buyu_changeBattery = "catchFish/switchBattery";
        public const string Buyu_changeRelyOn = "catchFish/robotRelyOn";
        #endregion


        #region 斗地主
        public const string Landlords_magic = "fightTheLandlord_magic";//魔法
        public const string Landlords_quickMessage = "fightTheLandlord_quickMessage";//聊天
        public const string Landlords_dealCards = "fightTheLandlord_dealCards";//收到发牌
        public const string Landlords_rush = "fightTheLandlord_rush";//通知用户叫地主
        public const string Landlords_nrush = "fightTheLandlord_nrush";//通知用户其他玩家叫地主的信息
        public const string Landlords_lastsCards = "fightTheLandlord_lastsCards";//确认地主（并翻底牌）
        public const string Landlords_multiple = "fightTheLandlord/multiple";//其他玩家加倍信息
        public const string Landlords_cards = "fightTheLandlord/cards";//获取出牌信息（判断出牌）
        public const string Landlords_done = "fightTheLandlord/done";//游戏结算
        public const string Landlords_usersChange = "fightTheLandlord/usersChange";//用户变化
        #endregion

        #region 奔驰宝马
        public const string BCBM_NewGame = "benz_bmw/new_game";//新游戏开局
        public const string BCBM_Player_Bet = "benz_bmw/player_bet";//玩家下注
        public const string BCBM_GameOpen = "benz_bmw/game_open";//开奖消息
        public const string BCBM_BankerQueueChange = "benz_bmw/banker/queue/change";//上庄消息
        public const string BCBM_Offline = "benz_bmw/offline";//退出消息
        public const string BCBM_UnPlay = "benz_bmw/unplay_bet";//清除下注
        #endregion

        #region 飞禽走兽
        public const string FQZS_NewGame = "animal/new_game";//新游戏开局
        public const string FQZS_Player_Bet = "animal/player_bet";//其他玩家下注
        public const string FQZS_GameOpen = "animal/game_open";//开奖
        public const string FQZS_Offline = "animal/offline";//退出游戏
        public const string FQZS_bankerQueueStat = "animal/banker/queue/change";//在线玩家数量
        public const string FQZS_UnPlay = "animal/unplay_bet";//清除下注
        #endregion

        #region 大厅
        /// <summary>
        /// 邮件的红点
        /// </summary>
        public const string Hall_EmailRed = "/message/red_dot";
        #endregion

        #region 美人心计
        public const string SchemesOfABeautyPrizePool = "schemesOfABeauty/prizePool";
        #endregion

        #region 21点
        public const string BlackJack_EnterRoom = "blackjack/enterRoomAll";//进入房间
        public const string BlackJack_ExitRoom = "blackjack/exitRoomAll";//退出房间
        public const string BlackJack_Start = "blackjack/startAll";//开始游戏
        public const string BlackJack_Bet = "blackjack/betAll";//下注
        public const string BlackJack_DealCard = "blackjack/dealCardAll";//发牌
        public const string BlackJack_StartSafe = "blackjack/startSafeAll";//保险确认
        public const string BlackJack_Turn = "blackjack/turnAll";//轮换
        public const string BlackJack_AddCard = "blackjack/addCardAll";//要牌
        public const string BlackJack_Double = "blackjack/doubleAll";//双倍
        public const string BlackJack_Split = "blackjack/splitAll";//分牌
        public const string BlackJack_Safe = "blackjack/safeAll";//保险
        public const string BlackJack_Settle = "blackjack/settleAll";//结算
        public const string BlackJack_GameOver = "blackjack/gameOver";//游戏结束
        public const string BlackJack_CardCount = "blackjack/shuffleCard";//牌数
        public const string BlackJack_StopCardAll = "blackjack/stopCardAll";//停牌
        #endregion

        #region 新捕鱼
        public const string NewCatchFish_EnterRoomBroadcast = "fish/enterRoomAll";         //进房间群发
        public const string NewCatchFish_ExitRoomBroadcastBroadcast = "fish/exitRoomAll";  //退房间群发  
        public const string NewCatchFish_LaunchBroadcast = "fish/lanchAll";                //发炮弹群发
        public const string NewCatchFish_FishDieBroadcast = "fish/fishDieAll";             //鱼死亡群发
        public const string NewCatchFish_NewFishBroadcast = "fish/newFishAll";             //新生成鱼群发
        public const string NewCatchFish_SwitchBetLevelBroadcast = "fish/switchBetLevelAll";//切换炮弹等级群发
        public const string NewCatchFish_SwitchSceneBroadcast = "fish/switchSceneAll";      //切换场景群发
        public const string NewCatchFish_LineShellBroadcast = "fish/lineShellAll";          //切换激光炮群发
        public const string NewCatchFish_SceneIceBroadcast = "fish/sceneIceAll";            //场景冰冻群发
        public const string NewCatchFish_RobotRelayOnBroadcast = "fish/robotRelyOn";        //机器人依赖


        #endregion
    }
}