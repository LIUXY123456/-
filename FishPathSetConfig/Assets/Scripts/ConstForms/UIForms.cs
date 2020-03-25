using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI结构（用来给UI显示关闭等操作调用）
/// </summary>
public  struct UIForms
{
    /* ------ Test Scene ------ */
    public const string Test_TestPanel = "TestPanel";

    /*-------Jump Scene-------*/
    public const string jumpScenePanel = "Prefabs/Panels/JumpScene/JumpScenePanel";
    public const string loadingPanel = "LoadingPanel";
    public const string childGameLoadingPanel = "ChildGameLoadingPanel";

    //public const string healthNoticePanel = "Prefabs/Panels/Login/HealthNoticePanel";//健康公告界面

    /* ------ LogIn Scene ------ */
    public const string selectPanel = "SelectLoginPanel";                   //选择登录界面
    public const string logInPanel = "AccountLoginPanel";                   //账号登录界面
    public const string registerPanel = "RegisterPanel";                    //账号注册界面
    public const string findPasswordPanel = "FindPasswordPanel";            //找回密码界面
    public const string perfectInfoPanel = "PerfectInfoPanel";              //完善资料界面
    public const string changePicturePanel = "ChangePicturePanel";          //更换头像界面


    /* ------ Hall Scene ------ */
    public const string hallPanel = "HallPanel";                            //游戏大厅界面
    public const string userinfoPanel = "UserinfoPanel";                    //用户信息界面
    public const string storePanel = "StorePanel";                          //商城界面
    public const string menusPanel = "MenusPanel";                          //菜单弹窗界面
    public const string settingsPanel = "SettingsPanel";                    //设置界面
    public const string giftPanel = "GiftPanel";                            //礼物界面
    public const string safePanel = "SafePanel";                            //保险箱界面
    public const string emailPanel = "EmailPanel";                          //邮箱界面
    public const string rankPanel = "RankPanel";                            //排行榜界面
    public const string giftCardPanel = "GiftCardPanel";                    //礼品卡界面
    public const string bindingPanel = "BindingPanel";                      //绑定界面
    public const string bindingPanelTip = "BindingPanelTip";                 //绑定界面
    public const string sharePanel = "SharePanel";                          //分享界面
    public const string inviteCodePanel = "InviteCodePanel";                //邀请码界面
    public const string changeIconPanel = "ChangeIconPanel";                //更换头像界面
    public const string VerifyNicknamePanel = "VerifyNicknamePanel";        //检测昵称界面
    public const string AlterSafeCodePanel = "AlterSafeCodePanel";          //修改保险箱密码界面
    public const string changePasswordPanel = "ChangePasswordPanel";        //修改密码界面
    public const string openEmallPanel = "OpenEmallPanel";                  //邮件详情界面
    public const string presentRecordPanel = "PresentRecordPanel";          //礼物记录界面
    public const string initSafeCodePanel = "InitSafeCodePanel";
    public const string changeBindPanel = "ChangeBindPanel";                //更改手机号面板
    public const string verifyPasswordPanel = "VerifyPasswordPanel";        //确认登陆密码面板
    public const string taskPanel = "TaskPanel";                            //任务面板
    public const string unBingdingPanel = "UnBingdingPanel";                //设备解绑
    public const string unBingdingHintPanel = "UnBingdingHintPanel";        //绑定提示

    /* ------ Buyu Scene ------ */
    public const string buyuPanel = "BuyuPanel";                            //捕鱼游戏主界面
    public const string Buyu_RoomPanel = "Buyu_RoomPanel";                  //捕鱼游戏选择场次界面
    public const string Buyu_FishAsPanel = "Buyu_FishShowPanel";            //捕鱼游戏鱼鉴界面
    public const string Buyu_SettingPanel = "Buyu_SettingPanel";            //捕鱼游戏设置界面
    public const string Buyu_KickOffPanel = "Buyu_TipsPanel";

    /* ------ Laba Scene ------ */
    public const string Laba_RoomPanel = "Laba_RoomPanel";                  //拉霸场次界面
    public const string Laba_GamePanel = "Laba_GamePanel";                  //拉霸游戏界面
    public const string Laba_MaryPanel = "Laba_MaryPanel";                  //拉霸小玛丽界面
    public const string Laba_VictoryPanel = "Laba_VictoryPanel";            //胜利弹窗


    /* ------ Landlords Scene ------ */
    public const string Landlords_RoomPanel = "DDZ_RoomPanel";              //斗地主场次界面
    public const string DDZ_GameHallPanel = "DDZ_GameHallPanel";            //斗地主大厅界面
    public const string DDZ_GamePlayPanel = "DDZ_GamePlayPanel";            //斗地主游戏界面
    public const string DDZ_SettingsPanel = "DDZ_SettingsPanel";            //斗地主设置界面
    public const string DDZ_GameDonePanel = "DDZ_GameDonePanel";            //斗地主结算界面


    /* ------ BCBM Scene ------ */
    public const string BCBM_GamePlayPanel = "BCBM_GamePlayPanel";          //奔驰宝马游戏界面
    public const string BCBM_ResultPanel = "BCBM_ResultPanel";              //奔驰宝马玩家列表界面
    public const string BCBM_HintPanel = "BCBM_HintPanel";                  //奔驰宝马提示界面
    public const string BCBM_HelpPanel = "BCBM_HelpPanel";                  //奔驰宝马帮助界面


    /* ------ ATT Scene ------ */
    public const string ATT_Panel = "ATT_Panel";                            //ATT游戏界面
    public const string ATT_RoomPanel = "ATT_RoomPanel";                    //ATT选择场次面板
    public const string ATT_GuessPanel = "ATT_GuessPanel";                  //ATT猜大小面板
    public const string ATT_HintPanel = "ATT_HintPanel";                    //ATT面板

    /* ------ 777 Scene ------ */
    public const string Seven_RoomPanel = "Seven_RoomPanel";                //777场次界面
    public const string Seven_GamePanel = "Seven_GamePanel";                // 777游戏界面


    /* ------ KingKong Scene ------ */
    public const string KingKong_RoomPanel = "KingKong_RoomPanel";                //KingKong选择场次面板
    public const string KingKongGamePlay_Panel = "KingKong_Panel";                //KingKong游戏主界面
    public const string KingKongHelp_Panel = "KingKongHelp_Panel";                 //KingKong帮助界面
    public const string KingKongMusicSetting_Panel = "MusicSetting_Panel";                 //KingKong音乐设置界面
    public const string KingKongSoundEffectSetting_Panel = "SoundEffectSetting_Panel";     //KingKong音效设置界面

    /* ------ FQZS Scene ------ */
    public const string FQZS_GamePlayPanel = "FQZS_GamePlayPanel";                //飞禽走兽游戏主界面
    public const string FQZS_PlayerListPanel = "FQZS_PlayerListPanel";            //飞禽走兽玩家列表界面
    public const string FQZS_HelpPanel = "FQZS_HelpPanel";                        //飞禽走兽帮助界面
    public const string FQZS_HintPanel = "FQZS_HintPanel";                        //飞禽走兽提示界面
    public const string FQZS_ResultPanel = "FQZS_ResultPanel";                        //飞禽走兽结算界面

    /* ------ 钻石大亨 Scene ------ */
    public const string ZSDH_RoomPanel = "ZSDH_RoomPanel";                  //钻石大亨场次界面
    public const string ZSDH_Panel = "ZSDH_Panel";
    public const string ZSDH_HelpPanel = "ZSDH_HelpPanel";
    public const string ZSDH_AutoPanel = "ZSDH_AutoPanel";
    public const string ZSDH_QuitPanel = "ZSDH_QuitPanel";

    /* ------ 美人心计 Scene ------ */
    public const string SchemesOfABeauty_GamePlayPanel = "SchemesOfABeauty_GamePlayPanel";                         //美人心计游戏主界面
    public const string SchemesOfABeauty_RoomPanel = "SchemesOfABeauty_RoomPanel";                                 //美人心计场次界面

    /* ------ 3D水果机 Scene ------ */
    public const string FM_RoomPanel = "FM_RoomPanel";
    public const string FM_GamePanel = "FM_GamePanel";
    public const string FM_MaryPanel = "FM_MaryPanel";

    /* ------ 21点 Scene ------ */
    public const string BlackJack_RoomPanel = "BlackJack_RoomPanel";
    public const string BlackJack_HelpPanel = "BlackJack_HelpPanel";                    //帮助界面
    public const string BlackJackGamePlayPanel = "BlackJackGamePlayPanel";              //游戏界面
    public const string BlackJack_SelectTablePanel = "BlackJack_SelectTablePanel";      //桌子界面

    /* ------ 新捕鱼 Scene ------ */
    public const string NewCatchFish_RoomPanel = "NewCatchFish_RoomPanel";
    public const string NewCatchFish_FishBookPanel = "NewCatchFish_IllustratedHandbookPanel";
    public const string NewCatchFish_SettingPanel = "NewCatchFish_SettingPanel";
    public const string NewCatchFish_GamePlayPanel = "NewCatchFish_GamePlayPanel";              
    public const string NewCatchFish_SelectTablePanel = "NewCatchFish_SelectPanelPanel";      
}
