using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池类型（要存放哪些对象？）
/// </summary>
public enum ObjectType
{
    /*炮弹：-1~99*/
    Buyu_bullet_ZuanTou = -1,
    Buyu_bullet_01 = 0,
    Buyu_bullet_02,
    Buyu_bullet_03,
    Buyu_bullet_04,
    Buyu_bullet_05,
    Buyu_bullet_06,
    Buyu_bullet_07,
    Buyu_bullet_08,
    Buyu_bullet_09,
    Buyu_bullet_10,

    /*渔网：100~199*/
    Buyu_网一 = 100,
    Buyu_网二,
    Buyu_网三,
    Buyu_网四,
    Buyu_网五,
    Buyu_网六,
    Buyu_网七,
    Buyu_网吧,
    Buyu_网九,
    Buyu_网十,
    /*鱼：200 ~399*/
    Buyu_黄鱼 = 200,
    Buyu_大眼鱼,
    Buyu_蓝棘鱼,
    Buyu_阿波罗,
    Buyu_小丑鱼,
    Buyu_河豚,
    Buyu_狮子鱼,
    Buyu_凤尾鱼,
    Buyu_龟,
    Buyu_鲶鱼,
    Buyu_灯笼鱼,
    Buyu_剑鱼,
    Buyu_魔鬼鱼,
    Buyu_金鲨鱼,
    Buyu_龙虾,
    Buyu_飞鱼,
    Buyu_鳄鱼,
    Buyu_螃蟹,
    Buyu_珍珠,
    Buyu_水母,
    Buyu_美人鱼,
    Buyu_独角鲸,
    Buyu_金蟾,
    Buyu_炸弹鱼,
    Buyu_冰冻鱼,
    Buyu_电击鱼,
    Buyu_翻倍卡,
    Buyu_宝箱,

    /*fm：401~499*/
    FM_Wild = 401,
    FM_Bar,
    FM_Bell,
    FM_Cherry,
    FM_Clevis,
    FM_Coin,
    FM_Grape,
    FM_Leaf,
    FM_Orange,
    FM_Seven,
    FM_Star,
    FM_Symbol,

    /*特效：500~599*/
    TestEffect = 500,
    ATT_PaiRoolEffect,
    Buyu_FlashEffect,
    Buyu_TestEffect,
    Buyu_BoomFishEffect,
    Buyu_FreezeFishEffect,
    Buyu_FlashFishEffect,
    Buyu_GoldEffect,
    Buyu_GoldBoomEffect0,
    Buyu_GoldBoomEffect1,
    Buyu_GoldBoomEffect1Follow,
    Buyu_GoldNumItem,
    Buyu_SpecialCatchEffect,
    Buyu_BigWinEffect,
    Buyu_ZuanTouAni,
    Buyu_DujiaojingDeadEffect,
    Buyu_DoubleCardEffect,

    /*斗地主*/
    DDZ_斗地主的牌_C = 600,

    /* 水果拉霸：650 */
    Laba_FruitItem = 650,

    /* 拉霸特效：670 */
    Laba_victory = 670,

    /* 777：700~750 */
    Seven_SymbolItem = 700,
    Seven_MiniPanel = 701,

    /* 777 特效：670 */
    Seven_victory = 720,

    /*ATT*/
    ATT的牌_C = 750,

    /*奔驰宝马：800~810*/
    BCBM_Player = 800,
    BCBM_Jetton = 810,

    /*飞禽走兽：811~820*/
    FQZS_Player = 815,
    FQZS_Jetton = 820,

    /*美人心计：821~830*/
    YPT_GameItem = 830,

    /* kingkong 900~999*/
    /// <summary>恐龙 </summary>
    KingKong_dinosaur = 900,
    /// <summary>女士 </summary>
    KingKong_girl,
    /// <summary>翼龙 </summary>
    KingKong_pterosaur,
    /// <summary>相机 </summary>
    KingKong_camera,
    /// <summary>图腾 </summary>
    KingKong_totem,
    /// <summary>轮船 </summary>
    KingKong_boat,
    /// <summary>A</summary>
    KingKong_A,
    /// <summary>K</summary>
    KingKong_K,
    /// <summary>Q</summary>
    KingKong_Q,
    /// <summary>J</summary>
    KingKong_J,
    /// <summary>10</summary>
    KingKong__10,
    /// <summary>KingKong</summary>
    KingKong_kingKong,
    /// <summary>Scatter</summary>
    KingKong_scatter,

    luo_shi = 930,

    /*-----点击特效-----*/
    CheckEffect = 1000,

    //-----------21D-----------//1100~1199
    BlackJack_Pai_C = 1100,
    BlackJack_Jetton_C = 1101,
    BlackJack_BlastCard_C = 1102,
    BlackJack_Deuce_C = 1103,
    BlackJack_BlackJack_C= 1104,
    BlackJack_Table_C= 1105,
    BlackJack_TableText_C= 1106,

    //--------------------YPT------------
    SchemesOfABeauty_Tile_A = 1200,
    SchemesOfABeauty_Tile_K,
    SchemesOfABeauty_Tile_Q,
    SchemesOfABeauty_Tile_Y,
    SchemesOfABeauty_Tile_Yellow,
    SchemesOfABeauty_Tile_Green,
    SchemesOfABeauty_Tile_Blue,
    SchemesOfABeauty_Tile_YPT,
    SchemesOfABeauty_LineRender = 1210,

    //--------NewCatchFish-------
    Battery0 =1300,
    Battery1,
    Battery2,
    Battery3,
    Battery4,
    Battery5,
    Battery6,
    Battery7,
    Battery8,
    Battery9,
    bullet_0 =1330,
    bullet_1,   
    bullet_2,
    bullet_3,
    bullet_4,
    bullet_5,
    bullet_6,
    bullet_7,
    bullet_8,
    bullet_9,
    bullet_10,
    NewCatchFish_黄鱼 = 1350,
    NewCatchFish_大眼鱼,
    NewCatchFish_蓝棘鱼,
    NewCatchFish_阿波罗,
    NewCatchFish_小丑鱼,
    NewCatchFish_河豚,
    NewCatchFish_狮子鱼,
    NewCatchFish_凤尾鱼,
    NewCatchFish_龟,
    NewCatchFish_鲶鱼,
    NewCatchFish_灯笼鱼,
    NewCatchFish_剑鱼,
    NewCatchFish_魔鬼鱼,
    NewCatchFish_金鲨鱼,
    NewCatchFish_龙虾,
    NewCatchFish_飞鱼,
    NewCatchFish_鳄鱼,
    NewCatchFish_螃蟹,
    NewCatchFish_珍珠,
    NewCatchFish_水母,
    NewCatchFish_美人鱼,
    NewCatchFish_独角鲸,
    NewCatchFish_金蟾,
    NewCatchFish_炸弹鱼,
    NewCatchFish_冰冻鱼,
    NewCatchFish_电击鱼,
    NewCatchFish_翻倍卡,
    NewCatchFish_宝箱,


}
