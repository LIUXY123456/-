using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EricFramework
{

    /// <summary>
    /// 游戏常量类（存储固定常量）
    /// </summary>
    public class AppConst
    {
        //------Debug-------
        public const bool isDebug = true;//false 从服务器下载，true 从本地加载
        public const bool unityLogger = true;  //是否显示debug信息
        public const bool hotFixTrigger = false;//是否开启代码热更新
        public static bool canScreenSleep = false;//是否可熄屏
        //---------------网络数据-----------------
        //主服务器地址
        public static string MainServerAddress = "http://47.108.156.38:18080";
        //public const string MainServerAddress = "http://119.3.192.197";
        //public const string MainServerAddress = "http://www.qq.com";

        //版本更新地址   /XMZ.apk
        public const string VersionUpdateAddress = "https://game-luckydreamfactory.oss-cn-chengdu.aliyuncs.com/VersionUpdate";
        public const string versionCodeUri = VersionUpdateAddress + "/version.txt";
        public const string newApkUpdateUri = VersionUpdateAddress + "/LuckyDream.apk";

        public const string iosUrlLink = "https://www.pangusign.vip/4FC";
        public static string versionCodeLocalPath = Application.persistentDataPath + "/version/version.txt";
        public static string versionCodeTempLocalPath = Application.persistentDataPath + "/version/temp/version.txt";
        public static string newApkLocalPath = Application.persistentDataPath + "/version/LuckyDream.apk";

        //由服务器获取
        public static string WebsocketServerAddress = "";
        //心跳包发送ip
        internal static string HeartBeatsPath = "http://47.108.156.38:18080";
        //热更新地址
        public static string hotfixUrl = "https://game-luckydreamfactory.oss-cn-chengdu.aliyuncs.com/VersionUpdate/InjectFix/Assembly-CSharp.patch.bytes";
        //热更新地址
        public static string hotfixUrl_iOS = "https://game-luckydreamfactory.oss-cn-chengdu.aliyuncs.com/VersionUpdate/InjectFix_iOS/Assembly-CSharp.patch.bytes";

        //错误、异常处理地址
        public static string errorAcceptIp = "192.168.0.142";
        public static string errorAcceptPort = "52013";
        //----------------游戏数据------------------------
        public const string debugNickName = "夏屋小农";
        public const string defultNickName = "夏屋小农";

        //------------资源加载数据-----------------
        /*-----Debug-----*/

        //Debug资源模式
        public const bool isDebugAssetMode = true;//debug资源模式  false：执行资源AB包加载，true：resources加载

        /*-----Normal-----*/

        //AB包的资源路径
        //服务器资源根目录
        public const string serverAssetRootUrl = "https://game-luckydreamfactory.oss-cn-chengdu.aliyuncs.com";
        //子游戏根目录
        public static string subGameRootPath = Application.persistentDataPath + "/game";
        //资源列表对比临时文件目录

#if UNITY_EDITOR
        public static string manifestTempPath = Application.streamingAssetsPath + "/manifestTemp/Manifest.txt";
#elif UNITY_ANDROID
    public static string manifestTempPath = Application.persistentDataPath+ "/manifestTemp/Manifest.txt";
#else
    public static string manifestTempPath = Application.persistentDataPath + "/manifestTemp/Manifest.txt";
#endif


        //公共包资源
        public const string commonAsPath = "/common.unity3d";
        //登录注册流程
        public const string loginAsPath = "/login.unity3d";

        //游戏大厅流程
#if UNITY_ANDROID
        public const string gameHallAsPath = "/game/common/common_gamehall.unity3d";
#elif UNITY_IOS
    public const string gameHallAsPath = "/game/common_iOS/common_gamehall.unity3d";
#endif

        public const string gameHallPanelAsPath = "/panels/gamehall.unity3d";//大厅
        public const string gameHallTextureAsPath = "/atlas/gamehall.unity3d";//大厅图集
        public const string gameHallBGMAsPath = "/audios/gamehall.unity3d";//大厅音效
        public const string gameHallItemAsPath = "/items/gamehall.unity3d";//大厅Items

        //选择场次流程
        public const string roomPanelAsPath = "/panels/room.unity3d";//场次选择资源包
        public const string roomTextureAsPath = "/atlas/room.unity3d";//选择场次

        #region -------- common资源列表文件目录 --------

        public const string CommonRootPath = "/game/common";
#if UNITY_IOS
     public const string CommonManifestAsPath = "/game/common_iOS/common_iOS-Manifest.txt";
#else
        public const string CommonManifestAsPath = "/game/common/common-Manifest.txt";
#endif
        public static string CommonManifestUrl = serverAssetRootUrl + CommonManifestAsPath;

        #endregion


        #region -------- 捕鱼游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string buyuRootPath = "/game/buyu";

#if UNITY_IOS
    public const string buyuManifestAsPath = "/game/buyu_iOS/buyu_iOS-Manifest.txt";
#else
        public const string buyuManifestAsPath = "/game/buyu/buyu-Manifest.txt";
#endif
        public static string buyuManifestUrl = serverAssetRootUrl + buyuManifestAsPath;
        //资源目录
        public const string buyuUiAsPath = "/game/buyu/ui.unity3d";//捕鱼panel资源包
        public static string buyuUiAsUrl = serverAssetRootUrl + buyuUiAsPath;
        public const string buyuAudioAsPath = "/game/buyu/audio.unity3d";
        public static string buyuAudioAsUrl = serverAssetRootUrl + buyuAudioAsPath;
        public const string buyuFishAsPath = "/game/buyu/fish.unity3d";
        public const string buyuFishAsUrl = serverAssetRootUrl + buyuFishAsPath;
        public const string buyuNetAndBulletAsPath = "/game/buyu/netandbullet.unity3d";
        public const string buyuNetAndBulletAsUrl = serverAssetRootUrl + buyuNetAndBulletAsPath;
        public const string buyuOtherAsPath = "/game/buyu/other.unity3d";
        public const string buyuOtherAsUrl = serverAssetRootUrl + buyuOtherAsPath;

        #endregion

        #region -------- 拉霸游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string labaRootPath = "/game/laba";

#if UNITY_IOS
    public const string labaManifestAsPath = "/game/laba_iOS/laba_iOS-Manifest.txt";
#else
        public const string labaManifestAsPath = "/game/laba/laba-Manifest.txt";
#endif
        public const string labaManifestUrl = serverAssetRootUrl + labaManifestAsPath;
        //资源目录
        public const string labaUiAsPath = "/game/laba/ui.unity3d";//Sprite、Panel、Item
        public const string labaUiAsUrl = serverAssetRootUrl + labaUiAsPath;
        public const string labaAudioAsPath = "/game/laba/audio.unity3d";//Audio
        public const string labaAudioAsUrl = serverAssetRootUrl + labaAudioAsPath;
        public const string labaOtherAsPath = "/game/laba/other.unity3d";//Effect、
        public const string labaOtherAsUrl = serverAssetRootUrl + labaOtherAsPath;

        #endregion

        #region -------- 斗地主游戏流程 - 资源列表文件目录 --------

        public const string landlordsPanelAsPath = "/panels/ddz.unity3d";
        public const string landlordsItemAsPath = "/items/ddz.unity3d";
        public const string landlordsTextureAsPath = "/atlas/ddz.unity3d";
        public const string landlordsEffectAsPath = "/animation.unity3d";

        #endregion


        #region -------- ATT游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string ATTRootPath = "/game/att";
#if UNITY_IOS
    public const string ATTManifestAsPath = "/game/att_iOS/att_iOS-Manifest.txt";
#else
        public const string ATTManifestAsPath = "/game/att/att-Manifest.txt";
#endif
        public static string ATTManifestUrl = serverAssetRootUrl + ATTManifestAsPath;
        //资源目录
        public const string ATTPanelAsPath = "/panels/att.unity3d";
        public const string ATTItemAsPath = "/items/att.unity3d";
        public const string ATTTextureAsPath = "/atlas/att.unity3d";
        public const string ATTBGMAsPath = "/audios/att.unity3d";
        public const string ATTEffectAsPath = "/effects/att.unity3d";

        #endregion


        #region -------- 777游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string SevenRootPath = "/game/777";
#if UNITY_IOS
    public const string SevenManifestAsPath = "/game/777_iOS/777_iOS-Manifest.txt";
#else
        public const string SevenManifestAsPath = "/game/777/777-Manifest.txt";
#endif
        public const string SevenManifestUrl = serverAssetRootUrl + SevenManifestAsPath;
        //资源目录
        public const string SevenUiAsPath = "/game/777/ui.unity3d";//Sprite、Panel、Item
        public const string SevenUiAsUrl = serverAssetRootUrl + SevenUiAsPath;
        public const string SevenAudioAsPath = "/game/777/audio.unity3d";//Audio
        public const string SevenAudioAsUrl = serverAssetRootUrl + SevenAudioAsPath;
        public const string SevenOtherAsPath = "/game/777/other.unity3d";//Effect、
        public const string SevenOtherAsUrl = serverAssetRootUrl + SevenOtherAsPath;

        #endregion


        #region -------- KingKong流程 - 资源列表文件目录 --------

        public const string KingKongRootPath = "/game/kingkong";
#if UNITY_IOS
     public const string KingKongManifestAsPath = "/game/kingkong_iOS/kingkong_iOS-Manifest.txt";
#else
        public const string KingKongManifestAsPath = "/game/kingkong/kingkong-Manifest.txt";
#endif
        public static string KingKongManifestUrl = serverAssetRootUrl + KingKongManifestAsPath;


        public const string KingKongPanelAsPath = "/panels/KingKong.unity3d";
        public const string KingKongItemAsPath = "/items/KingKong.unity3d";
        public const string KingKongTextureAsPath = "/atlas/KingKong.unity3d";
        public const string KingKongEffectAsPath = "/effects/KingKong.unity3d";

        #endregion


        #region -------- 飞禽走兽游戏流程 - 资源列表文件目录 --------

        public const string FQZSRootPath = "/game/fqzs";

#if UNITY_IOS
     public const string FQZSManifestAsPath = "/game/fqzs_iOS/fqzs_iOS-Manifest.txt";
#else
        public const string FQZSManifestAsPath = "/game/fqzs/fqzs-Manifest.txt";
#endif
        public static string FQZSManifestUrl = serverAssetRootUrl + FQZSManifestAsPath;
        public const string fqzsUiAsPath = "/game/fqzs/ui.unity3d";//Sprite、Panel、Item
        public const string fqzsUiAsUrl = serverAssetRootUrl + fqzsUiAsPath;
        public const string fqzsAudioAsPath = "/game/fqzs/audio.unity3d";//Audio
        public const string fqzsAudioAsUrl = serverAssetRootUrl + fqzsAudioAsPath;
        public const string fqzsOtherAsPath = "/game/fqzs/other.unity3d";//Effect、
        public const string fqzsOtherAsUrl = serverAssetRootUrl + fqzsOtherAsPath;

        #endregion


        #region -------- 钻石大亨游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string ZSDHRootPath = "/game/zsdh";

#if UNITY_IOS
     public const string ZSDHManifestAsPath = "/game/zsdh_iOS/zsdh_iOS-Manifest.txt";
#else
        public const string ZSDHManifestAsPath = "/game/zsdh/zsdh-Manifest.txt";
#endif
        public static string ZSDHManifestUrl = serverAssetRootUrl + ZSDHManifestAsPath;
        //资源目录
        public const string ZSDHPanelASPath = "/panels/zsdh.unity3d";
        public const string ZSDHItemASPath = "/items/zsdh.unity3d";
        public const string ZSDHTextureASPath = "/atlas/zsdh.unity3d";
        public const string ZSDHBGMASPath = "/audios/zsdh.unity3d";
        public const string ZSDHEffectASPath = "/effects/zsdh.unity3d";

        #endregion


        #region -------- 奔驰宝马游戏流程 - 资源列表文件目录 --------

        public const string BCBMRootPath = "/game/bcbm";

#if UNITY_IOS
    public const string BCBMManifestAsPath = "/game/bcbm_iOS/bcbm_iOS-Manifest.txt";
#else
        public const string BCBMManifestAsPath = "/game/bcbm/bcbm-Manifest.txt";
#endif
        public static string BCBMManifestUrl = serverAssetRootUrl + BCBMManifestAsPath;

        public const string bcbmUiAsPath = "/game/bcbm/ui.unity3d";//Sprite、Panel、Item
        public const string bcbmUiAsUrl = serverAssetRootUrl + bcbmUiAsPath;
        public const string bcbmAudioAsPath = "/game/bcbm/audio.unity3d";//Audio
        public const string bcbmAudioAsUrl = serverAssetRootUrl + bcbmAudioAsPath;
        public const string bcbmOtherAsPath = "/game/bcbm/other.unity3d";//Effect、
        public const string bcbmOtherAsUrl = serverAssetRootUrl + bcbmOtherAsPath;

        #endregion


        #region -------- 美人心计游戏流程 - 资源列表文件目录 --------

        public const string SchemesOfABeautyRootPath = "/game/schemesofabeauty";
#if UNITY_IOS
     public const string SchemesOfABeautyManifestAsPath = "/game/schemesofabeauty_iOS/schemesofabeauty_iOS-Manifest.txt";
#else
        public const string SchemesOfABeautyManifestAsPath = "/game/schemesofabeauty/schemesofabeauty-Manifest.txt";
#endif
        public static string SchemesOfABeautyManifestUrl = serverAssetRootUrl + SchemesOfABeautyManifestAsPath;



        public const string SchemesOfABeautyPanelAsPath = "/panels/schemesofabeauty.unity3d";
        public const string SchemesOfABeautyItemAsPath = "/items/schemesofabeauty.unity3d";
        public const string SchemesOfABeautyTextureAsPath = "/atlas/schemesofabeauty.unity3d";
        public const string SchemesOfABeautyEffectAsPath = "/effects/schemesofabeauty.unity3d";

        #endregion

        #region -------- 新捕鱼游戏流程 - 资源列表文件目录 --------

        public const string NewCatchFishRootPath = "/game/newcatchfish";
#if UNITY_IOS
        public const string NewCatchFishManifestAsPath = "/game/newcatchfish_iOS/newcatchfish_iOS-Manifest.txt";
#else
        public const string NewCatchFishManifestAsPath = "/game/newcatchfish/newcatchfish-Manifest.txt";
#endif
        public static string NewCatchFishManifestUrl = serverAssetRootUrl + SchemesOfABeautyManifestAsPath;



        public const string NewCatchFishPanelAsPath = "/panels/newcatchfish.unity3d";
        public const string NewCatchFishItemAsPath = "/items/newcatchfish.unity3d";
        public const string NewCatchFishTextureAsPath = "/atlas/newcatchfish.unity3d";
        public const string NewCatchFishEffectAsPath = "/effects/newcatchfish.unity3d";

        #endregion



        #region -------- 3D_FM游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string FMRootPath = "/game/fm";

#if UNITY_IOS
     public const string FMManifestAsPath = "/game/fm_iOS/fm_iOS-Manifest.txt";
#else
        public const string FMManifestAsPath = "/game/fm/fm-Manifest.txt";
#endif
        public static string FMManifestUrl = serverAssetRootUrl + FMManifestAsPath;
        //资源目录
        public const string FMUiAsPath = "/game/fm/ui.unity3d";//Sprite、Panel、Item
        public const string FMUiAsUrl = serverAssetRootUrl + FMUiAsPath;
        public const string FMAudioAsPath = "/game/fm/audio.unity3d";//Audio
        public const string FMAudioAsUrl = serverAssetRootUrl + FMAudioAsPath;
        public const string FMOtherAsPath = "/game/fm/other.unity3d";//Effect、
        public const string FMOtherAsUrl = serverAssetRootUrl + FMOtherAsPath;


        #endregion


        #region -------- 21D游戏流程 - 资源列表文件目录 --------

        //总目录
        public const string BlackJackRootPath = "/game/blackjack";

#if UNITY_IOS
     public const string BlackJackManifestAsPath = "/game/blackjack_iOS/blackjack_iOS-Manifest.txt";
#else
        public const string BlackJackManifestAsPath = "/game/blackjack/blackjack-Manifest.txt";
#endif
        public static string BlackJackManifestUrl = serverAssetRootUrl + BlackJackManifestAsPath;


        //资源目录
        public const string BlackJackUiAsPath = "/game/blackjack/ui.unity3d";//Sprite、Panel、Item
        public const string BlackJackUiAsUrl = serverAssetRootUrl + BlackJackUiAsPath;
        public const string BlackJackAudioAsPath = "/game/blackjack/audio.unity3d";//Audio
        public const string BlackJackAudioAsUrl = serverAssetRootUrl + BlackJackAudioAsPath;
        public const string BlackJackOtherAsPath = "/game/blackjack/items.unity3d";//Effect、
        public const string BlackJackOtherAsUrl = serverAssetRootUrl + BlackJackOtherAsPath;


        #endregion

        #region -------- Debug资源路径 --------

        //Panel
        public const string Test_DebugPanelASPath = "Prefabs/Panels/TestTools/";
        public const string Login_DebugPanelASPath = "Prefabs/Panels/Login/";
        public const string GameHall_DebugPanelASPath = "Prefabs/Panels/GameHall/";
        public const string Room_DebugPanelASPath = "Prefabs/Panels/Room/";
        public const string Buyu_DebugPanelASPath = "Prefabs/Panels/Buyu/";
        public const string Landlords_DebugPanelASPath = "Prefabs/Panels/Landlords/";
        public const string Laba_DebugPanelASPath = "Prefabs/Panels/Laba/";
        public const string BCBM_DebugPanelASPath = "Prefabs/Panels/BCBM/";//奔驰宝马
        public const string ATT_DebugPanelASPath = "Prefabs/Panels/ATT/";
        public const string Seven_DebugPanelASPath = "Prefabs/Panels/777/";
        public const string KingKong_DebugPanelASPath = "Prefabs/Panels/KingKong/";
        public const string FQZS_DebugPanelASPath = "Prefabs/Panels/FQZS/";//飞禽走兽
        public const string ZSDH_DebugPanelAsPath = "Prefabs/Panels/ZSDH/";
        public const string YPT_DebugPanelAsPath = "Prefabs/Panels/SchemesOfABeauty/";//美人心计
        public const string FM_DebugPanelASPath = "Prefabs/Panels/F-Machine/";
        public const string DDZ_GamePlayer_DebugPanelASPath = "Prefabs/Panels/Landlords/DDZ_GamePlay/";
        public const string BlackJack_DebugPanelASPath = "Prefabs/Panels/21D/";//21D
        public const string NewCatchFish_DebugPanelASPath = "Prefabs/Panels/NewCatchFish/";//21D
        //Item
        public const string Login_DebugItemASPath = "Prefabs/Items/Login/";
        public const string GameHall_DebugItemASPath = "Prefabs/Items/GameHall/";
        public const string Room_DebugItemASPath = "Prefabs/Items/Room/";
        public const string Buyu_DebugItemASPath = "Prefabs/Items/Buyu/";
        public const string Laba_DebugItemASPath = "Prefabs/Items/Laba/";
        public const string Landlords_DebugItemASPath = "Prefabs/Items/Landlord/";
        public const string BCBM_DebugItemASPath = "Prefabs/Items/BCBM/";
        public const string ATT_DebugItemASPath = "Prefabs/Items/ATT/";
        public const string Seven_DebugItemASPath = "Prefabs/Items/777/";
        public const string KingKong_DebugItemASPath = "Prefabs/Items/KingKong/";
        public const string FQZS_DebugItemASPath = "Prefabs/Items/FQZS/";//飞禽走兽
        public const string ZSDH_DebugItemASPath = "Prefabs/Items/ZSDH/";
        public const string YPT_DebugItemASPath = "Prefabs/Items/SchemesOfABeauty/";//美人心计
        public const string FM_DebugItemASPath = "Prefabs/Items/F-Machine/";//FM
        public const string BlackJack_DebugItemASPath = "Prefabs/Items/21D/";//21D
        public const string NewCatchFish_DebugItemASPath = "Prefabs/Items/NewCatchFish/";//21D
        public const string NewCatchFish_DebugItemASPath_Temp = "Prefabs/Items/NewCatchFish_Temp/";//临时加载新捕鱼的鱼

        //Audio
        public const string Login_DebugAudioASPath = "Audios/Login/";
        public const string GameHall_DebugAudioASPath = "Audios/GameHall/";
        public const string Room_DebugAudioASPath = "Audios/Room/";
        public const string Buyu_DebugAudioASPath = "Audios/Buyu/";
        public const string KingKong_DebugAudioASPath = "Audios/KingKong/";
        public const string Laba_DebugAudioASPath = "Audios/Laba/";
        public const string Landlords_DebugAudioASPath = "Audios/Landlords/";//斗地主音效
        public const string BCBM_DebugAudioASPath = "Audios/BCBM/";
        public const string ATT_DebugAudioASPath = "Audios/ATT/";
        public const string Seven_DebugAudioASPath = "Audios/777/";
        public const string FQZS_DebugAudioASPath = "Audios/FQZS/";//飞禽走兽
        public const string ZSDH_DebugAudioASPath = "Audios/ZSDH/";
        public const string YPT_DebugAudioASPath = "Audios/SchemesOfABeauty/";//美人心计
        public const string FM_DebugAudioASPath = "Audios/F-Machine/";
        public const string BlackJack_DebugAudioASPath = "Audios/21D/";//21D
        public const string NewCatchFish_DebugAudioASPath = "Audios/NewCatchFish/";//21D
        //Effect
        public const string Laba_DebugEffectASPath = "Prefabs/Effects/Laba/";
        public const string Seven_DebugEffectASPath = "Prefabs/Effects/777/";
        public const string FM_DebugEffectASPath = "Prefabs/Effects/F-Machine/";

        #endregion


        #region -------- 配置表地址 --------

        //资源配置表地址
        public const string assetConfigPath = "Config/AssetsConfig.json";
        public const string Buyu_fishModel_ConfigPath = "Config/FishModelConfig.json";
        public const string Buyu_FIshTide_ConfigPath = "Config/Buyu_FishTideConfig.json";
        public const string NewCatchFIsh_ConfigPath = "Config/Buyu_FIshKind_ConfigPath.json";

        public const string Buyu_FIshKind_ConfigPath = "Config/Buyu_FIshKind_ConfigPath.json";
        public const string Buyu_FIshBoom_ConfigPath = "Config/Buyu_FIshBoom_ConfigPath.json";
        #endregion


        //------------场景编号常量-----------------
        //public const byte LogInSceneIndex = 0;
        //public const byte GameHallSceneIndex = 1;
    }


    /// <summary>
    /// 场次类型
    /// </summary>
    public enum SpieltagType
    {
        体验场,
        青铜场,
        白银场,
        黄金场,
        铂金场,
        钻石场,
    }

    /// <summary>
    /// 地址映射
    /// </summary>
    public enum AddrType
    {
        A1 = 11,
        B1,
        C1,
        D1,
        E1,
        A2 = 21,
        B2,
        C2,
        D2,
        E2,
        A3 = 31,
        B3,
        C3,
        D3,
        E3,
    }
}
