using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using EricFramework;
using SubGame_NewCatchFish;
using BPLaBaGame.FSM;
using System.Linq;

namespace Game_Buyu
{
    public class YuChao_ConfigPanel : MonoBehaviour
    {
        #region 公用字段
        public FishPathController fishContro;
        public FishPathDataManager fishData;

        /// <summary>
        /// 连线的组件
        /// </summary>
        private LineRenderer line;
        //点击开启计时器（当点击使鱼群跑时开始计时）
        private static bool isCounting = false;
        //读秒
        private GameObject clock;
        private IEnumerator clockIE;
        #endregion

        #region 单条鱼定义的字段
        /// <summary>
        /// 所有单条鱼数据的列表
        /// </summary>
        public List<Fish> oneFishDic;
        /// <summary>
        /// 当前鼠标点击位置的真实坐标列表
        /// </summary>
        private List<Vector3> lineV3List;
        /// <summary>
        /// 真正用于生成配置表的坐标列表
        /// </summary>
        private List<Vector3> realLineV3;
        /// <summary>
        /// 连线时的点击次数，当完成时归零
        /// </summary>
        private int lineCount;
        #endregion

        #region 鱼群的字段
        /// <summary>
        /// 连线的列表
        /// </summary>
        private List<LineRenderer> lineList;
        /// <summary>
        /// 所有鱼群的字典
        /// </summary>
        Dictionary<byte, List<List<Fish>>> fishBoomDic;
        /// <summary>
        /// 是否开始连线
        /// </summary>
        bool isLine_FB;
        /// <summary>
        /// 其他鱼是否开始编辑
        /// </summary>
        bool isOtherStart;
        /// <summary>
        /// 鱼群连线时的点击次数，当完成时归零
        /// </summary>
        private int lineCount_FB;
        #endregion

        #region
        public GameObject oneFishPanel;//单条鱼编辑界面
        public GameObject oneFishPathWidget;//单条鱼路线界面
        public GameObject oneFishEditWidget;//单条鱼具体编辑界面
        public Text lineText;//开始连线按钮

        //FB：FishBoom
        /// <summary>
        /// 鱼群编辑总界面
        /// </summary>
        public GameObject FB_Panel;
        /// <summary>
        /// 当前选定的场景的所有鱼群的弹窗
        /// </summary>
        public GameObject FB_AllFishListWidget;
        /// <summary>
        /// 鱼群基本类型编辑界面
        /// </summary>
        public GameObject FB_TypeEditWidget;
        /// <summary>
        /// 当前鱼群第一条鱼的编辑界面
        /// </summary>
        public GameObject FB_FirstFishEditWidget;
        /// <summary>
        ///其他鱼的编辑界面
        /// </summary>
        public GameObject FB_OtherFishEditWidget;
        /// <summary>
        /// 展示整个鱼群的界面
        /// </summary>
        //public GameObject FB_ShowFishEditWidget;


        InputField X_Tran;
        InputField Y_Tran;
        InputField rotate;
        #endregion

        void Start()
        {
            fishContro = new FishPathController(this);
            fishData = new FishPathDataManager(this);

            fishBoomDic = new Dictionary<byte, List<List<Fish>>>();
            fishData.isLine = false;
            isLine_FB = false;
            isOtherStart = false;
            lineCount = 0;
            lineCount_FB = 0;

            InitalPanel();//初始化界面信息
        }

        #region 界面初始化方法
        private void InitalPanel()
        {
            clock = GameObject.Find("Clock_C");//计时器
            clockIE = CountingTimes();//计时协程
            line = GameObject.Find("BG_C").GetComponent<LineRenderer>();//连线
            lineList = new List<LineRenderer>(); //lineList.Add(line);//将第一个连线组件添加到列表

            oneFishPanel = GameObject.Find("OneFishEdit_C");//单条鱼的总编辑界面
            oneFishPathWidget = GameObject.Find("oneFishPathScr_C");
            oneFishEditWidget = GameObject.Find("oneEdit_C");

            FB_Panel = GameObject.Find("FishBoomEdit_C");//鱼群编辑界面
            FB_AllFishListWidget = GameObject.Find("FishBoomListScr_C");//单独场景下的所有鱼群
            FB_TypeEditWidget = GameObject.Find("FishBoomEdit_FishType_C");//当前鱼群基本属性设置界面
            FB_FirstFishEditWidget = GameObject.Find("FishBoomEdit_FirstFish_C");//当前鱼群第一条鱼编辑界面
            FB_OtherFishEditWidget = GameObject.Find("FishBoomEdit_OtherFish_C");//当前鱼群其他鱼的编辑界面

            X_Tran = GameObject.Find("TranslationX_C").GetComponent<InputField>();
            Y_Tran = GameObject.Find("TranslationY_C").GetComponent<InputField>();
            rotate = GameObject.Find("Rotation_C").GetComponent<InputField>();
            X_Tran.onEndEdit.AddListener(FB_OtherFishEdit_TranslationX_InputField);
            Y_Tran.onEndEdit.AddListener(FB_OtherFishEdit_TranslationY_InputField);
            rotate.onEndEdit.AddListener(FB_OtherFishEdit_Rotate_InputField);

            lineText = GameObject.Find("LineTXT_C").GetComponent<Text>();
            InitialClickEvent();//初始化点击事件
            InitialConfigDictionary_OneFish();//初始化单条鱼配置字典
            InitialConfigDictionary_FishBoom();

            oneFishPathWidget.SetActive(false); oneFishEditWidget.SetActive(false);
            FB_Panel.SetActive(false); FB_AllFishListWidget.SetActive(false); FB_TypeEditWidget.SetActive(false);
            FB_FirstFishEditWidget.SetActive(false); FB_OtherFishEditWidget.SetActive(false);
        }
        #endregion

        #region 初始化方法：获取当前的配置文件的方法；根据配置文件生成对应鱼按钮的滑动条
        /// <summary>
        /// 初始化配置字典（单个鱼）
        /// </summary>
        private void InitialConfigDictionary_OneFish()
        {
            if (File.Exists(fishData.oneFishConfigPath))
            {
                oneFishDic = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Fish>>(File.ReadAllText(fishData.oneFishConfigPath));
                Debug.Log("单个鱼配置字典初始化完成？" + oneFishDic.Count);

                Debug.Log("所有点的长度：" + oneFishDic[0].fishPath[1].fishPoint.Count);
                InitialFishTypeScrollView();
                return;
            }
            Debug.Log("单个鱼配置文件不存在");
        }
        /// <summary>
        /// 初始化所有鱼种类的滑动按钮
        /// </summary>
        private void InitialFishTypeScrollView()
        {
            for (int i = 0; i < oneFishDic.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, GameObject.Find("FishContent_C").transform);
                // 1.读取配置表中的内容，根据内容在按钮中显示对应的名字,并绑定方法
                object m = i;
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.FishTypeButton_OnClick(m); });
                o.transform.GetComponentInChildren<Text>().text = fishContro.FishName(oneFishDic[i].fishKind);
            }
        }
        /// <summary>
        /// 初始化配置字典（鱼群）
        /// </summary>
        private void InitialConfigDictionary_FishBoom()
        {
            if (File.Exists(fishData.fishBoomConfigPath))
            {
                fishBoomDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<byte, List<List<Fish>>>>(File.ReadAllText(fishData.fishBoomConfigPath));
                Debug.Log("鱼群配置字典初始化完成？" + fishBoomDic.Count);
                InitialFishBoomScrollView();
                return;
            }
            Debug.Log("鱼群鱼潮配置文件不存在");
        }
        /// <summary>
        /// 初始化生成所有鱼群的滑动按钮
        /// </summary>
        private void InitialFishBoomScrollView()
        {
            foreach (var item in fishBoomDic)
            {
                //生成预设物
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, GameObject.Find("FishBoomContent_C").transform);
                //绑定点击方法和命名
                object m = item;
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.SceneFishBoom_OnClick(m); });
                o.transform.GetComponentInChildren<Text>().text = "场景号:" + item.ToString();
            }
        }
        #endregion



        #region 按钮点击事件

        #region 鱼群和单条鱼的编辑选择
        /// <summary>
        /// 选择单条鱼编辑面板
        /// </summary>
        private void SelectOneFishEdit_OnClick()
        {
            if (!fishData.isFishBoom) return;
            if (clockIE != null) StopCoroutine(clockIE);//关闭计时协程
            oneFishPanel.SetActive(true);
            FB_Panel.SetActive(false);
            //清空面板
            line.positionCount = 0;
            fishData.isFishBoom = false;//是否是鱼群为false
            PathDown_FishBoom();PathDown_OneFish();//关闭所有弹窗
            //删除多余的line组件
            int count = lineList.Count;
            while (count > 1)
            {
                Destroy(lineList[count]);
                count--;
            }
        }
        /// <summary>
        /// 选择多条鱼编辑面板
        /// </summary>
        private void SelectFishBoomEdit_OnClick()
        {
            if (fishData.isFishBoom) return;
            if(fishData.nowFish != null) Destroy(fishData.nowFish.gameObject);//删除当前的鱼
            if (clockIE != null) StopCoroutine(clockIE);//关闭计时协程
            oneFishPanel.SetActive(false);        
            FB_Panel.SetActive(true);
            //清空面板
            line.positionCount = 0;
            fishData.isFishBoom = true;//是否是鱼群为true
            PathDown_FishBoom(); PathDown_OneFish();//关闭所有弹窗
        }
        #endregion

        #region 添加鱼和删除鱼的方法
        private void AddOneFishMessage_OnClick()
        {
            if (GameObject.Find("AddOneFishName_C").GetComponent<InputField>().text == "") { EricDebug.Debu.LogR("鱼的名字不能为空"); return; }
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
            GameObject o = Instantiate(go, GameObject.Find("FishContent_C").transform);

            // 1.读取配置表中的内容，根据内容在按钮中显示对应的名字,并绑定方法
            //Debug.Log(oneFishDic[i].fishKind);
            object m = oneFishDic.Count;
            o.GetComponent<Button>().onClick.AddListener(delegate () { this.FishTypeButton_OnClick(m); });
            o.transform.GetComponentInChildren<Text>().text = GameObject.Find("AddOneFishName_C").GetComponent<InputField>().text;//新的鱼的名字
            Fish newFish = new Fish(0, (byte)(oneFishDic.Count + 1));//生成一个新的鱼对象
            newFish.fishPath = new Dictionary<int, FishPath>();
            FishPath newFishPath = new FishPath();
            newFishPath.pathId = 1;
            newFishPath.pathName = "路线1";
            newFishPath.fishPoint = new List<FishVecter>();
            newFishPath.fishPoint.Add(new FishVecter(-80, 375, 100, (((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f)));
            newFishPath.fishPoint.Add(new FishVecter(1450, 375, 0, 0));
            newFish.fishPath.Add(1,newFishPath);
            oneFishDic.Add(newFish);

            GameObject.Find("AddOneFishName_C").GetComponent<InputField>().text = "";//将输入框置空

            Debug.Log("添加了一条鱼！！！");
        }

        private void DestroyOneFishMessage_OnClick()
        {
            Destroy(GameObject.Find("FishContent_C").transform.GetChild(oneFishDic.Count - 1).gameObject);//删除最后一个鱼
            oneFishDic.RemoveAt(oneFishDic.Count - 1);//删除字典中最后一个鱼
            PathDown_OneFish();//关闭路线和编辑界面

            Debug.Log("删除了一条鱼！！！");
        }
        #endregion

        #region 开始连线按钮方法（已完成）
        /// <summary>
        /// 点击按钮，开始连线
        /// </summary>
        private void StartLine_OnClick()
        {
            if (!fishData.isLine)
            {
                fishData.isLine = true;
                lineText.text = "停止连线";
                //将场景中已存在的点存入列表
                lineCount = line.positionCount;
                lineV3List = new List<Vector3>(); realLineV3 = new List<Vector3>();
                Debug.Log("当前点数：" + lineCount);
                for (int i = 0; i < lineCount; i++)
                {
                    lineV3List.Add(line.GetPosition(i));
                    realLineV3.Add(new Vector3(line.GetPosition(i).x - 302, line.GetPosition(i).y - 375, 0));
                }
            }
            else
            {
                fishData.isLine = false;
                lineText.text = "开始连线";
            }

        }
        #endregion

        #region 鱼的每个路线按钮点击事件
        /// <summary>
        /// 所有鱼的按钮点击事件（点击生成弹窗，显示对应鱼的路线编辑窗口）
        /// </summary>
        public void FishTypeButton_OnClick(object _index)
        {
            Quit_OneFish();//关闭编辑界面
            //打开弹窗，并根据对应鱼的ID，生成对应的路线编辑按钮
            if (!oneFishPathWidget.activeSelf) oneFishPathWidget.SetActive(true);//打开路线弹窗
            fishData.nowFishKindIndex = (int)_index;
            FishPathShow();//显示对应鱼的路线弹窗
        }
        /// <summary>
        /// 鱼的路线按钮生成方法
        /// </summary>
        private void FishPathShow()
        {
            //删除之前的物体
            int count = GameObject.Find("PathContent_C").transform.childCount;
            while (count > 0)
            {
                Destroy(GameObject.Find("PathContent_C").transform.GetChild(count - 1).gameObject);
                count--;
            }
            //读取配置表中对应鱼的路线列表，根据列表生成路线按钮，根据配置的信息，填写对应的路线名称
            //获取当前鱼路线数量，如果没有路线，默认为1条
            Debug.Log(oneFishDic[fishData.nowFishKindIndex].fishPath.Count);
            for (int i = 0; i < oneFishDic[fishData.nowFishKindIndex].fishPath.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishPath_C");
                GameObject o = Instantiate(go, GameObject.Find("PathContent_C").transform);
                // 1.读取配置表中的内容，根据内容在按钮中显示对应的名字,并绑定方法
                o.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "路线" + oneFishDic[fishData.nowFishKindIndex].fishPath[i + 1].pathId;
                o.transform.GetChild(1).GetComponent<InputField>().text = oneFishDic[fishData.nowFishKindIndex].fishPath[i + 1].pathName;
                object k = i + 1;
                o.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { FishPathBtn_OnClick(oneFishDic[fishData.nowFishKindIndex], k); });
            }
        }
        /// <summary>
        /// 鱼的路线编辑按钮点击事件
        /// </summary>
        private void FishPathBtn_OnClick(Fish _fish, object _index)
        {
            if(clockIE != null) StopCoroutine(clockIE);//关闭计时协程
            fishData.nowPathIdIndex = (int)_index;
            fishData.nowFishPath = oneFishDic[fishData.nowFishKindIndex].fishPath[fishData.nowPathIdIndex];
            fishData.nowFishPath.pathName = GameObject.Find("PathContent_C").transform.GetChild((int)_index - 1).GetChild(1).GetComponent<InputField>().text;
            oneFishEditWidget.SetActive(true);  //打开编辑面板
            fishData.isLine = false; lineText.text = "开始连线";
            FishPathMessage_Show(_fish, _index);//将对应路线的相关信息填入菜单中（坐标速度时间）
            FishPathLine_Show(_fish, _index);//根据路线信息在场景中生成对应连线，并在初始位置生成对应的鱼
        }
        #endregion

        #region 返回上一步
        /// <summary>
        /// 连线返回上一步
        /// </summary>
        private void LastStepLine_OnClick()
        {
            if (!fishData.isLine) return;
            if (line.positionCount == 0) return;
            Destroy(GameObject.Find("FishPointList_C").transform.GetChild(line.positionCount - 1).gameObject);
            lineCount--;
            lineV3List.RemoveAt(lineCount);
            realLineV3.RemoveAt(lineCount);

            line.positionCount = 0;
            line.positionCount = lineV3List.Count;
            for (int i = 0; i < lineCount; i++)
            {
                line.SetPosition(i, lineV3List[i]);
            }

            if(line.positionCount == 0) Destroy(fishData.nowFish.gameObject);
        }
        #endregion

        #region 清除当前场景所有连线的方法
        public void ClearLine()
        {
            if (!fishData.isLine) return;
            while (lineCount > 0)
            {
                Destroy(GameObject.Find("FishPointList_C").transform.GetChild(lineCount - 1).gameObject);
                lineCount--;
            }
            lineV3List.Clear(); realLineV3.Clear();
            line.positionCount = 0;

            //清除场景中的鱼
        }
        #endregion

        #region 应用
        /// <summary>
        /// 将手动配置的点阵赋值给数据类中的临时对象
        /// </summary>
        public void ReadConfigToList()
        {
            if (!fishData.isLine) return;
            List<FishVecter> revalV3 = new List<FishVecter>();
            Transform fishPointParent = GameObject.Find("FishPointList_C").transform;
            for (int i = 0; i < fishPointParent.childCount; i++)
            {
                float x = float.Parse(fishPointParent.GetChild(i).GetChild(0).GetComponent<InputField>().text);
                float y = float.Parse(fishPointParent.GetChild(i).GetChild(1).GetComponent<InputField>().text);
                float speed = float.Parse(fishPointParent.GetChild(i).GetChild(2).GetComponent<InputField>().text == "" ? "0" : fishPointParent.GetChild(i).GetChild(2).GetComponent<InputField>().text);
                if (i != fishPointParent.childCount - 1 && speed == 0)
                {
                    revalV3.Clear();
                    Debug.Log("速度不能为0！！！");
                    return;
                }
                revalV3.Add(new FishVecter(x, y, speed, 0));
            }
            fishData.nowFishPath.fishPoint = revalV3;
            fishData.nowFishPath = fishContro.Time_FishV3(fishData.nowFishPath);

            for (int i = 0; i < GameObject.Find("FishPointList_C").transform.childCount; i++)
            {
                fishPointParent.GetChild(i).GetChild(3).GetComponent<InputField>().text = fishData.nowFishPath.fishPoint[i].time.ToString();
            }
            EricDebug.Debu.LogB("总时间：" + fishData.nowFishPath.costTime);
            Debug.Log("应用成功！！！");
        }
        #endregion

        #region 添加一条路线
        private void AddOneLine_OnClick()
        {
            //if (!fishData.isLine) return;
            FishPath newFishPath = new FishPath();
            newFishPath.pathId = oneFishDic[fishData.nowFishKindIndex].fishPath.Count + 1;
            newFishPath.pathName = "路线" + (oneFishDic[fishData.nowFishKindIndex].fishPath.Count + 1);
            newFishPath.fishPoint = new List<FishVecter>();
            newFishPath.fishPoint.Add(new FishVecter(-80, 375, 100, (((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f)));
            newFishPath.fishPoint.Add(new FishVecter(1450, 375, 0, 0));
            oneFishDic[fishData.nowFishKindIndex].fishPath.Add(newFishPath.pathId, newFishPath);
            //生成预设物到对应父物体下
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishPath_C");
            GameObject o = Instantiate(go, GameObject.Find("PathContent_C").transform);
            // 1.读取配置表中的内容，根据内容在按钮中显示对应的名字,并绑定方法
            o.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "路线" + newFishPath.pathId;
            o.transform.GetChild(1).GetComponent<InputField>().text = newFishPath.pathName;
            object k = newFishPath.pathId;
            o.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { FishPathBtn_OnClick(oneFishDic[fishData.nowFishKindIndex], k); });
        }
        #endregion

        #region 删除一条路线
        private void DestroyOneLine_OnClick()
        {
            //if (!fishData.isLine) return;
            if (oneFishDic[fishData.nowFishKindIndex].fishPath.Count == 1)
            {
                Debug.Log("最少保留一条路线");
                return;
            }
            Destroy(GameObject.Find("PathContent_C").transform.GetChild(oneFishDic[fishData.nowFishKindIndex].fishPath.Count - 1).gameObject);
            oneFishDic[fishData.nowFishKindIndex].fishPath.Remove(oneFishDic[fishData.nowFishKindIndex].fishPath.Count);
            //关闭编辑界面
            Quit_OneFish();
        }
        #endregion

        #region 添加这条线路到字典
        public void AddThisLineToDic_OnClick()
        {
            if (!fishData.isLine) return;
            oneFishDic[fishData.nowFishKindIndex].fishPath[fishData.nowPathIdIndex] = fishData.nowFishPath;
            Debug.Log(fishData.nowFishPath.fishPoint[0].x);
            Debug.Log("成功添加到字典；当前鱼的索引是：" + fishData.nowFishKindIndex + "；" + "当前路径的key是：" + fishData.nowPathIdIndex);
        }
        #endregion

        #endregion

        #region 1.生成对应的鱼；2.鱼跑；3.鱼再跑
        private NewCatchFish_NormalFish_Temp CreateFishPrefab()
        {
            Debug.Log("鱼的索引：" + fishData.nowFishKindIndex);
            //销毁上一条鱼
            if (fishData.nowFish != null) Destroy(fishData.nowFish.gameObject);
            //生成一条鱼
            GameObject go = Resources.Load<GameObject>("Prefabs/Items/NewCatchFish_Temp/" + (ObjectType)(1350 + fishData.nowFishKindIndex));//ObjectPool.Instance.GetAGameObject<NewCatchFish_NormalFish_Temp>((ObjectType)1350 + fishData.nowFishKindIndex, GameObject.Find("BG_C").transform);
            GameObject o = Instantiate(go, GameObject.Find("BG_C").transform);

            if (line.positionCount != 0) o.transform.position = line.GetPosition(0);
            Debug.Log(line.GetPosition(0));

            NewCatchFish_NormalFish_Temp tempFish = o.GetComponent<NewCatchFish_NormalFish_Temp>();
            return tempFish;
        }
        private void FishMove()
        {
            if (!fishData.isLine) return;
            NewCatchFish_FishInfo fishInfo = new NewCatchFish_FishInfo();
            fishInfo = new NewCatchFish_FishInfo();
            fishInfo.FishID = 0;
            fishInfo.FishKind = fishData.nowFishKindIndex + 1;
            fishInfo.PathId = fishData.nowPathIdIndex;
            fishInfo.CreateTime = 10;
            fishInfo.CostTime = 1500;

            string temp = Newtonsoft.Json.JsonConvert.SerializeObject(fishContro.RealCoordToScenesCoord(fishData.nowFishPath));

            fishData.nowFish.fishPath = Newtonsoft.Json.JsonConvert.DeserializeObject<SubGame_NewCatchFish.FishPath>(temp);

            fishData.nowFish.InitFish(fishInfo);
            fishData.isTime = true;
            if (clockIE != null) StopCoroutine(clockIE);
            clockIE = CountingTimes();
            StartCoroutine(clockIE);
        }
        /// <summary>
        /// 重新跑
        /// </summary>
        public void ToRun_OnClick()
        {
            if (!fishData.isLine) return;
            string temp = Newtonsoft.Json.JsonConvert.SerializeObject(fishContro.RealCoordToScenesCoord(fishData.nowFishPath));
            fishData.nowFish.fishPath = Newtonsoft.Json.JsonConvert.DeserializeObject<SubGame_NewCatchFish.FishPath>(temp);

            fishData.nowFish.currentPathIndex = 0;
            fishData.nowFish.transform.position = new Vector3(fishData.nowFish.fishPath.fishPoint[0].x, fishData.nowFish.fishPath.fishPoint[0].y, 0);
        }
        #endregion
        
        #region 逻辑方法

        #region 计时器
        /// <summary>
        /// 计时器
        /// </summary>
        /// <returns></returns>
        private IEnumerator CountingTimes()
        {

            float times = 0;
            Text timeText = GameObject.Find("Clock_C").GetComponent<Text>();
            timeText.text = "00:00";
            while (true)
            {
                if (fishData.nowFish.currentPathIndex == fishData.nowFishPath.fishPoint.Count - 1) fishData.isTime = false;
                if (fishData.isTime)
                {
                    if (!clock.activeSelf)
                    {
                        clock.SetActive(true);
                    }
                    timeText.text = ((int)times).ToString() + ":" + (((int)(times * 100)) % 100).ToString("00");
                    times += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    yield break;
                }
            }
        }
        #endregion

        #region 点击屏幕添加line线的方法
        private void LineController_OnClick()
        {
            if (fishData.isLine)//如果开始连线
            {
                if (Input.GetMouseButtonDown(0))//如果点击鼠标左键
                {
                    if (Input.mousePosition.x > 1800 || Input.mousePosition.y < 153) return;//如果超出坐标范围就返回

                    lineCount++;//点击次数加一
                    Vector3 mouse1 = Input.mousePosition;
                    Vector3 mouse2 = new Vector3(mouse1.x - 302, mouse1.y - 375);
                    lineV3List.Add(mouse1);
                    realLineV3.Add(mouse2);
                    //在场景中生成对应的线
                    line.positionCount = lineCount;
                    for (int i = 0; i < lineCount; i++)
                    {
                        line.SetPosition(i, lineV3List[i]);
                    }
                    //在弹窗中生成对应的预设物显示
                    GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishV3_C");
                    GameObject o = Instantiate(go, GameObject.Find("FishPointList_C").transform);
                    //将点数信息填入预设物的显示框中
                    o.transform.GetChild(4).GetComponent<Text>().text = (line.positionCount - 1).ToString();//显示当前点的索引
                    o.transform.GetChild(0).GetComponent<InputField>().text = mouse2.x.ToString();//显示x坐标
                    o.transform.GetChild(1).GetComponent<InputField>().text = mouse2.y.ToString();//显示y坐标
                    if (lineV3List.Count == 1)//如果是第一个点，就生成一条对应的鱼
                    {
                        fishData.nowFish = CreateFishPrefab();//生成鱼
                    }
                    //FishRotation_Set();//重新定位鱼的角度
                    Debug.Log("当前真实坐标:" + mouse1);
                    Debug.Log("用于配置的坐标:" + mouse2);
                }
            }
        }
        #endregion

        #region 1.将对应鱼对应路线的信息填入InputFild的方法; 2.将对应鱼的对应路线在场景中显示的方法
        private void FishPathMessage_Show(Fish _fish, object index)//_index代表当前路线的索引
        {
            int _index = (int)index;
            GameObject.Find("NowFishName_C").GetComponent<Text>().text = fishContro.FishName(_fish.fishKind);//显示当前选择的鱼类
            GameObject.Find("NowLineNum_C").GetComponent<Text>().text = _fish.fishPath[_index].pathName;//显示当前选择的路线ID
            List<FishVecter> tempFish = _fish.fishPath[_index].fishPoint;//根据该路线的点数列表的长度生成对应数量的预设物
                                                                         //删除之前的预设物
            int count = GameObject.Find("FishPointList_C").transform.childCount;
            while (count > 0)
            {
                Destroy(GameObject.Find("FishPointList_C").transform.GetChild(count - 1).gameObject);
                count--;
            }
            for (int i = 0; i < tempFish.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishV3_C");
                GameObject o = Instantiate(go, GameObject.Find("FishPointList_C").transform);
                //将点数信息填入预设物的显示框中
                o.transform.GetChild(4).GetComponent<Text>().text = i.ToString();//显示当前点的索引
                o.transform.GetChild(0).GetComponent<InputField>().text = tempFish[i].x.ToString();
                o.transform.GetChild(1).GetComponent<InputField>().text = tempFish[i].y.ToString();
                o.transform.GetChild(2).GetComponent<InputField>().text = tempFish[i].speed.ToString();
                o.transform.GetChild(3).GetComponent<InputField>().text = tempFish[i].time.ToString();
            }
        }

        private void FishPathLine_Show(Fish _fish, object _index)
        {
            int index = (int)_index;
            //删除之前的路线
            line.positionCount = 0;
            line.positionCount = _fish.fishPath[index].fishPoint.Count;

            for (int i = 0; i < _fish.fishPath[index].fishPoint.Count; i++)
            {
                Vector3 v3 = new Vector3(_fish.fishPath[index].fishPoint[i].x + 302, _fish.fishPath[index].fishPoint[i].y + 375, 0);
                line.SetPosition(i, v3);
            }
            fishData.nowFish = CreateFishPrefab();//生成对应的鱼
        }
        #endregion

        #region 所有关闭弹窗方法
        private void PathDown_OneFish()//关闭路线栏
        {
            oneFishPathWidget.SetActive(false); Quit_OneFish();
        }
        public void Quit_OneFish()//关闭菜单栏
        {
            oneFishEditWidget.SetActive(false);
            fishData.isLine = false;
            lineText.text = "开始连线";
        }
        #endregion

        #endregion


        //-------------------***鱼潮方法***-------------------//

        #region 鱼群

        #region 场景和鱼群的点击事件
        /// <summary>
        /// 鱼群场景按钮点击事件
        /// </summary>0
        private void SceneFishBoom_OnClick(object _index)
        {
            Quit_FishBoom();//关闭编辑界面
            if (!FB_AllFishListWidget.activeSelf) FB_AllFishListWidget.SetActive(true);//打开当前选择的场景的所有鱼群显示弹窗
            fishData.fishBoomSceneIndex = (byte)(int)_index;
            fishData.nowFishList_FB_Two = fishBoomDic[fishData.fishBoomSceneIndex];//将当前选定的场景中所有鱼群赋值给数据类
            BeforeFishBoomShow();//显示对应鱼群的路线弹窗
        }
        /// <summary>
        /// 当前场景鱼群弹窗的显示
        /// </summary>
        private void BeforeFishBoomShow()
        {
            Transform beforeFishBoomParent = GameObject.Find("BeforeFishBoomContent_C").transform;//获取显示当前选择的场景的所有鱼群父物体
            //删除之前的物体
            int count = beforeFishBoomParent.childCount;
            while (count > 0)
            {
                Destroy(beforeFishBoomParent.GetChild(count - 1).gameObject);
                count--;
            }
            //读取配置表中对应鱼群的路线列表，根据列表生成路线按钮，根据配置的信息，填写对应的路线名称
            //获取当前鱼路线数量，如果没有路线，默认为1条
            for (int i = 0; i < fishBoomDic[fishData.fishBoomSceneIndex].Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, beforeFishBoomParent);
                // 1.读取配置表中的内容，根据内容在按钮中显示对应的名字,并绑定方法
                string fishName = "";
                if (fishBoomDic[fishData.fishBoomSceneIndex][i][0].fishKind == 0) fishName = "未选择";
                else fishName = fishContro.FishName(fishBoomDic[fishData.fishBoomSceneIndex][i][0].fishKind);
                o.transform.GetComponentInChildren<Text>().text ="第" + (i + 1) + "组鱼:" + fishName;
                object k = i;
                o.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OneFishBoomEdit_OnClick(fishBoomDic[fishData.fishBoomSceneIndex][(int)k], k); });
            }
        }
        /// <summary>
        /// 所有鱼群按钮中每个鱼群的点击事件
        /// </summary>
        /// <param name="_fishList_Two"></param>
        /// <param name="_index"></param>
        private void OneFishBoomEdit_OnClick(List<Fish> _fishList_Two, object _index)
        {
            fishData.fishBoomListIndex = (int)_index;
            fishData.nowFishList_FB = _fishList_Two;//将当前选择的鱼群赋值给数据类的临时字段
            //关闭其他弹窗，只打开第一条鱼编辑面板
            Quit_FishBoom(); FB_TypeEdit_BasicMessage_Show();//打开基本信息编辑面板并初始化
        }
        #endregion

        #region 鱼群场景添加删除场景的事件
        /// <summary>
        /// 添加一个鱼群场景
        /// </summary>
        private void AddFishBoomScene_OnClick()
        {
            //生成预设物
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
            GameObject o = Instantiate(go, GameObject.Find("FishBoomContent_C").transform);
            //绑定点击方法和命名
            object m = fishBoomDic.Count;
            o.GetComponent<Button>().onClick.AddListener(delegate () { this.SceneFishBoom_OnClick((int)m - 1); });
            o.transform.GetComponentInChildren<Text>().text = "场景号:" + m.ToString();
            List<List<Fish>> temp = new List<List<Fish>>();
            fishBoomDic.Add((byte)(fishBoomDic.Count), temp);//新添加一个新的鱼群场景到字典
            PathDown_FishBoom();//关闭所有弹窗
        }
        /// <summary>
        /// 删除一个鱼群场景
        /// </summary>
        private void DestroyFishScene_OnClick()
        {
            if (fishBoomDic.Count == 1) return;//最少留一个
            //删除最后一个预设物
            Destroy(GameObject.Find("FishBoomContent_C").transform.GetChild(fishBoomDic.Count - 1).gameObject);
            fishBoomDic.Remove((byte)(fishBoomDic.Count - 1));
            PathDown_FishBoom();//关闭所有弹窗
        }
        #endregion

        #region 当前场景中的所有鱼群面板点击事件（退出事件、添加一个鱼群、删除一个鱼群）
        /// <summary>
        /// 在当前场景添加一个鱼群
        /// </summary>
        private void AddFishListToScene_OnClick()
        {
            //生成预设物
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
            GameObject o = Instantiate(go, GameObject.Find("FB_FishBoomListContent_C").transform);

            int tempIndex = fishBoomDic[fishData.fishBoomSceneIndex].Count;//获取当前二维列表的长度
            o.transform.GetComponentInChildren<Text>().text = "第" + (tempIndex) + "组鱼:未选择";
            fishBoomDic[fishData.fishBoomSceneIndex].Add(new List<Fish>());//新建一个鱼群并放入字典
            object k = tempIndex;
            o.GetComponent<Button>().onClick.AddListener(delegate() { OneFishBoomEdit_OnClick(fishBoomDic[fishData.fishBoomSceneIndex][(int)k], k); });

            Quit_FishBoom();//关闭所有弹编辑窗口
        }
        /// <summary>
        /// 删除一个当前场景的鱼群
        /// </summary>
        private void DestroyFishListScene_OnClick()
        {
            if (fishBoomDic[fishData.fishBoomSceneIndex].Count == 1) return;//至少留一个
            //删除场景中的组件
            Destroy(GameObject.Find("FB_FishBoomListContent_C").transform.GetChild(fishBoomDic[fishData.fishBoomSceneIndex].Count - 1).gameObject);
            //删除字典中对应的数据
            fishBoomDic[fishData.fishBoomSceneIndex].RemoveAt(fishBoomDic[fishData.fishBoomSceneIndex].Count - 1);
            Quit_FishBoom();//关闭所有弹编辑窗口
        }
        /// <summary>
        /// 关闭鱼群列表弹窗
        /// </summary>
        private void Quit_FB_FishBoomList()
        {
            PathDown_FishBoom();//关闭所有弹窗
            ClearAllLineAndFish();//清除之前场景的所有鱼和线
        }
        #endregion

        #region 鱼群基本配置面板的方法
       /// <summary>
       /// 鱼群基本配置面板显示方法
       /// </summary>
        private void FB_TypeEdit_BasicMessage_Show()
        {
            ClearAllLineAndFish();//清除之前场景的所有鱼和线
            FB_TypeEditWidget.SetActive(true);
            //获取当前输入框组件
            InputField fishType_Inpt = GameObject.Find("FishType_Inpt_C").GetComponent<InputField>();
            InputField fishCount_Inpt = GameObject.Find("FishNum_Inpt_C").GetComponent<InputField>();
            fishType_Inpt.text = ""; fishCount_Inpt.text = "";//初始化输入框
            string fishType = "";
            if (fishData.nowFishList_FB.Count == 0)
                fishType = "未设置鱼群种类";
            else fishType = "当前鱼类:" + fishContro.FishName(fishData.nowFishList_FB[0].fishKind);
            GameObject.Find("NowFishListType_FB_C").GetComponent<Text>().text = fishType;//当前鱼的种类赋值
            GameObject.Find("NowFishListIndex_FB_C").GetComponent<Text>().text = fishData.fishBoomListIndex.ToString();//当前鱼群在二维鱼群数组中的索引
            GameObject.Find("NowFishListCount_FB_C").GetComponent<Text>().text = fishData.nowFishList_FB.Count.ToString();//当前鱼群中鱼的数量

            if (fishData.nowFishList_FB.Count != 0)//如果有鱼
            {
                FishBoomInitital();//初始化鱼群显示
            }
        }
        /// <summary>
        /// 鱼群初始化
        /// </summary>
        private void FishBoomInitital()
        {
            //根据输入框的数据，生成对应的鱼和默认路线
            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                NewCatchFish_NormalFish_Temp fishTemp = CreateFishPrefab_FB(fishData.nowFishList_FB[i].fishKind, i);//生成鱼和路线
                lineList[i].positionCount = 0;
                for (int j = 0; j < fishData.nowFishList_FB[i].fishPath[1].fishPoint.Count; j++)
                {
                    lineList[i].SetPosition(j, new Vector3(fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].x, fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].y, 0));
                }
                SetFishPosAndRot();//设置鱼的初始位置和角度
            }
        }
        /// <summary>
        /// 鱼群基本配置面板鱼跑的方法（演示鱼群移动）
        /// </summary>
        private void FB_TypeEdit_FishRun_OnClick()
        {
            //安全判断是否有鱼
            if (fishData.nowFishList_FB.Count == 0) { Debug.Log("当前鱼群没有鱼！！！"); return; }

            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                NewCatchFish_FishInfo fishInfo = new NewCatchFish_FishInfo();
                fishInfo = new NewCatchFish_FishInfo();
                fishInfo.FishID = 0;
                fishInfo.FishKind = fishData.nowFishList_FB[i].fishKind;
                fishInfo.PathId = fishData.nowFishList_FB[i].fishPath[1].pathId;
                fishInfo.CreateTime = 10;
                fishInfo.CostTime = (long)(fishData.nowFishList_FB[i].fishPath[1].costTime);

                string temp = Newtonsoft.Json.JsonConvert.SerializeObject(fishContro.RealCoordToScenesCoord(fishData.nowFishList_FB[i].fishPath[1]));

                fishData.nowFish.fishPath = Newtonsoft.Json.JsonConvert.DeserializeObject<SubGame_NewCatchFish.FishPath>(temp);

                fishData.nowFish.InitFish(fishInfo);
            }
            fishData.isTime = true;
            if (clockIE != null) StopCoroutine(clockIE);
            clockIE = CountingTimes();
            StartCoroutine(clockIE);
        }
        /// <summary>
        /// 鱼群基本配置面板关闭事件
        /// </summary>
        private void FB_TypeEdit_Quit_OnClick()
        {
            Quit_FishBoom();//关闭所有编辑弹窗界面
        }
        /// <summary>
        /// 鱼群基本配置面板应用按钮事件
        /// </summary>
        private void FB_TypeEdit_Apply_OnClick()
        {
            //获取当前输入框组件
            InputField fishType = GameObject.Find("FishType_Inpt_C").GetComponent<InputField>();
            InputField fishCount = GameObject.Find("FishNum_Inpt_C").GetComponent<InputField>();
            if (fishType.text == "" || fishCount.text == "") { Debug.Log("输入框不能为空！！！");return; }
            fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Clear();//清空之前的鱼群数据
            //删除当前场景中的所有鱼和鱼所属的路线line
            int count = fishData.nowFishList_FB.Count;
            while (count > 0)
            {
                Destroy(line.transform.GetChild(count - 1).gameObject);
                if(lineList.Count != 1)
                {
                    Destroy(lineList[lineList.Count - 1]);
                    lineList.RemoveAt(lineList.Count - 1);
                }
                count--;
            }

            //根据输入框的数据，生成对应的鱼和默认路线
            for (int i = 0; i < int.Parse(fishCount.text); i++)
            {
                NewCatchFish_NormalFish_Temp fishTemp = CreateFishPrefab_FB(int.Parse(fishType.text), i);//生成鱼和路线
                lineList[i].positionCount = 0;//重置line点
                //将生成的鱼和路线添加到字典
                Fish fish = new Fish();//鱼的对象
                fish.fishId = 0;fish.fishKind = byte.Parse(fishType.text);

                FishPath fishPath = new FishPath(); fishPath.pathId = 1; fishPath.pathName = "路线1";//鱼的路线

                fishPath.fishPoint = new List<FishVecter>();//鱼路线的点
                fishPath.fishPoint.Add(new FishVecter(-80, 375, 100, (((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f)));
                fishPath.fishPoint.Add(new FishVecter(1450, 375, 0, 0));
                fishPath.costTime = ((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f;
                fish.fishPath = new Dictionary<int, FishPath>();fish.fishPath.Add(1, fishPath);
                fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Add(fish);

                //将新的点添加到line中显示到界面
                lineList[i].SetPosition(0, new Vector3(-80 + 302, 375 + 375, 0));
                lineList[i].SetPosition(1, new Vector3(1450 + 302, 375 + 375, 0));
            }

            //初始化配置界面
            GameObject.Find("NowFishListType_FB_C").GetComponent<Text>().text = ((ObjectType)(1350 + int.Parse(fishType.text))).ToString();//当前鱼的种类赋值
            GameObject.Find("NowFishListCount_FB_C").GetComponent<Text>().text = fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Count.ToString();
            fishType.text = ""; fishCount.text = "";//初始化输入框
        }
        /// <summary>
        /// 鱼群基本配置面板编辑按钮事件
        /// </summary>
        private void FB_TypeEdit_Edit_OnClick()
        {
            if (fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Count == 0) { Debug.Log("该鱼群没有鱼！！！");return; }
            //跳转到第一条鱼编辑界面
            FB_TypeEditWidget.SetActive(false);
            //将第一条鱼的路线赋值给该鱼群中的所有鱼
            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                if (i == 0) continue;
                fishData.nowFishList_FB[i].fishPath.Add(1, fishData.nowFishList_FB[0].fishPath[1]);
            }
            FB_FirstFishEdit_EditShow();//调用第一条鱼编辑面板初始化方法
        }
        #endregion

        #region 鱼群第一条鱼编辑面板的方法
        /// <summary>
        /// 第一条鱼编辑面板初始化方法
        /// </summary>
        private void FB_FirstFishEdit_EditShow()
        {
            ClearAllLineAndFish();//清除之前场景的所有鱼和线
            FB_FirstFishEditWidget.SetActive(true);//打开本面板
            //初始化面板显示
            isLine_FB = false; GameObject.Find("FirstFish_StartLine_C").GetComponentInChildren<Text>().text = "开始连线";
            GameObject.Find("FishTypeTXT_FirstFish_C").GetComponent<Text>().text = fishContro.FishName(fishData.nowFishList_FB[0].fishKind);
            GameObject.Find("FishTypeTXT_FishListIndex_C").GetComponent<Text>().text = fishData.fishBoomListIndex.ToString();
            FB_FirstFishEdit_FishPathCreate();//鱼和路线的生成方法，并生成对应的点预设物
        }
        /// <summary>
        /// 开始连线按钮
        /// </summary>
        private void FB_FirstFishEdit_StartLine_OnClick()
        {
            Text lineText = GameObject.Find("FirstFish_StartLine_C").GetComponentInChildren<Text>();
            if (!isLine_FB)
            {
                isLine_FB = true;
                lineText.text = "停止连线";
                lineCount_FB = lineList[0].positionCount;
                //将场景中已存在的点存入列表
                lineV3List = new List<Vector3>(); realLineV3 = new List<Vector3>();
                Debug.Log("当前点数：" + lineCount_FB);
                for (int i = 0; i < lineCount_FB; i++)
                {
                    lineV3List.Add(lineList[0].GetPosition(i));
                    realLineV3.Add(new Vector3(lineList[0].GetPosition(i).x - 302, lineList[0].GetPosition(i).y - 375, 0));
                }
            }
            else
            {
                isLine_FB = false;
                lineText.text = "开始连线";
            }
        }
        /// <summary>
        /// 上一步
        /// </summary>
        private void FB_FirstFishEdit_LastStep_OnClick()
        {
            if (!isLine_FB) return;
            if (lineList[0].positionCount == 0) return;
            Destroy(GameObject.Find("FistFishPointList_FB_C").transform.GetChild(lineList[0].positionCount - 1).gameObject);
            lineCount_FB--;
            lineV3List.RemoveAt(lineCount_FB);
            realLineV3.RemoveAt(lineCount_FB);

            lineList[0].positionCount = 0;
            line.positionCount = lineV3List.Count;
            for (int i = 0; i < lineCount_FB; i++)
            {
                lineList[0].SetPosition(i, lineV3List[i]);
            }

            if (lineList[0].positionCount == 0) Destroy(fishData.nowFish.gameObject);
        }
        /// <summary>
        /// 清空所有点
        /// </summary>
        private void FB_FirstFishEdit_ClearAllLine_OnClick()
        {
            if (!isLine_FB) return;
            while (lineCount_FB > 0)
            {
                Destroy(GameObject.Find("FistFishPointList_FB_C").transform.GetChild(lineCount_FB - 1).gameObject);
                lineCount_FB--;
            }
            lineV3List.Clear(); realLineV3.Clear();
            lineList[0].positionCount = 0;
        }
        /// <summary>
        /// 应用
        /// </summary>
        private void FB_FirstFishEdit_Apply_OnClick()
        {
            if (!isLine_FB) return;
            List<FishVecter> revalV3 = new List<FishVecter>();
            Transform fishPointParent = GameObject.Find("FistFishPointList_FB_C").transform;
            for (int i = 0; i < fishPointParent.childCount; i++)
            {
                float x = float.Parse(fishPointParent.GetChild(i).GetChild(0).GetComponent<InputField>().text);
                float y = float.Parse(fishPointParent.GetChild(i).GetChild(1).GetComponent<InputField>().text);
                float speed = float.Parse(fishPointParent.GetChild(i).GetChild(2).GetComponent<InputField>().text == "" ? "0" : fishPointParent.GetChild(i).GetChild(2).GetComponent<InputField>().text);
                if (i != fishPointParent.childCount - 1 && speed == 0)
                {
                    revalV3.Clear();
                    Debug.Log("速度不能为0！！！");
                    return;
                }
                revalV3.Add(new FishVecter(x, y, speed, 0));
            }
            fishData.nowFishList_FB[0].fishPath[1].fishPoint = revalV3;
            fishData.nowFishList_FB[0].fishPath[1] = fishContro.Time_FishV3(fishData.nowFishList_FB[0].fishPath[1]);

            for (int i = 0; i < GameObject.Find("FistFishPointList_FB_C").transform.childCount; i++)
            {
                fishPointParent.GetChild(i).GetChild(3).GetComponent<InputField>().text = fishData.nowFishList_FB[0].fishPath[1].fishPoint[i].time.ToString();
            }
            EricDebug.Debu.LogB("总时间：" + fishData.nowFishList_FB[0].fishPath[1].costTime);
            Debug.Log("应用成功！！！");

            fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex][0].fishPath[1] = fishData.nowFishList_FB[0].fishPath[1];
            Debug.Log(fishData.nowFishList_FB[0].fishPath[1].fishPoint[0].x);
            Debug.Log("成功添加到字典；当前鱼群的索引是：" + fishData.fishBoomListIndex + "；当前路径的key是：" + fishData.fishBoomSceneIndex);
        }        
        /// <summary>
        /// 鱼跑
        /// </summary>
        private void FB_FirstFishEdit_FishRun_OnClick()
        {
            if (!isLine_FB) return;
            NewCatchFish_FishInfo fishInfo = new NewCatchFish_FishInfo();
            fishInfo = new NewCatchFish_FishInfo();
            fishInfo.FishID = 0;
            fishInfo.FishKind = fishData.nowFishList_FB[0].fishKind;
            fishInfo.PathId = fishData.nowFishList_FB[0].fishPath[1].pathId;
            fishInfo.CreateTime = 10;
            fishInfo.CostTime = (long)(fishData.nowFishList_FB[0].fishPath[1].costTime);

            string temp = Newtonsoft.Json.JsonConvert.SerializeObject(fishContro.RealCoordToScenesCoord(fishData.nowFishList_FB[0].fishPath[1]));

            fishData.nowFish.fishPath = Newtonsoft.Json.JsonConvert.DeserializeObject<SubGame_NewCatchFish.FishPath>(temp);

            fishData.nowFish.InitFish(fishInfo);
            fishData.isTime = true;
            if (clockIE != null) StopCoroutine(clockIE);
            clockIE = CountingTimes();
            StartCoroutine(clockIE);
        }
        /// <summary>
        /// 离开
        /// </summary>
        private void FB_FirstFishEdit_Quit_OnClick()
        {
            //关闭一条鱼的编辑面板
            FB_FirstFishEditWidget.SetActive(false);
            //返回鱼类基本编辑面板
            FB_TypeEdit_BasicMessage_Show();//基本信息面板初始化
        }
        /// <summary>
        /// 编辑其他鱼的路线信息
        /// </summary>
        private void FB_FirstFishEdit_OtherFishEdit_OnClick()
        {
            //关闭此界面，并跳转到其他鱼的编辑界面
            Quit_FishBoom();//关闭所有弹窗
            FB_OtherFishEdit_Initital();//其他鱼编辑界面初始化
        }

        #region 本编辑面板的逻辑方法
        /// <summary>
        /// 鱼和路线生成方法
        /// </summary>
        private void FB_FirstFishEdit_FishPathCreate()
        {
            fishData.nowFish_FB = CreateFishPrefab_FB(fishData.nowFishList_FB[0].fishKind, 0);//生成鱼，和对应的路线组件
            //将鱼的路线点添加到对应的line组件中
            lineList[0].positionCount = 0;//重置line的点
            //设置line点
            for (int i = 0; i < fishData.nowFishList_FB[0].fishPath[1].fishPoint.Count; i++)
            {
                lineList[0].SetPosition(i, new Vector3(fishContro.RealCoordToScenesCoord(fishData.nowFishList_FB[0].fishPath[1]).fishPoint[0].x, fishContro.RealCoordToScenesCoord(fishData.nowFishList_FB[0].fishPath[1]).fishPoint[0].y, 0));
            }

            List<FishVecter> tempFish = fishData.nowFishList_FB[0].fishPath[1].fishPoint;//根据该路线的点数列表的长度生成对应数量的预设物
            //删除之前的预设物
            int count = GameObject.Find("FistFishPointList_FB_C").transform.childCount;
            while (count > 0)
            {
                Destroy(GameObject.Find("FistFishPointList_FB_C").transform.GetChild(count - 1).gameObject);
                count--;
            }
            for (int i = 0; i < tempFish.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishV3_C");
                GameObject o = Instantiate(go, GameObject.Find("FistFishPointList_FB_C").transform);
                //将点数信息填入预设物的显示框中
                o.transform.GetChild(4).GetComponent<Text>().text = i.ToString();//显示当前点的索引
                o.transform.GetChild(0).GetComponent<InputField>().text = tempFish[i].x.ToString();
                o.transform.GetChild(1).GetComponent<InputField>().text = tempFish[i].y.ToString();
                o.transform.GetChild(2).GetComponent<InputField>().text = tempFish[i].speed.ToString();
                o.transform.GetChild(3).GetComponent<InputField>().text = tempFish[i].time.ToString();
            }
            SetFishPosAndRot();//重设鱼的初始坐标和角度
        }
        #endregion

        #endregion

        #region 鱼群其他鱼的编辑面板的方法
        /// <summary>
        /// 其他鱼编辑界面初始化方法
        /// </summary>
        private void FB_OtherFishEdit_Initital()
        {
            ClearAllLineAndFish();//清除之前场景的所有鱼和线
            FB_OtherFishEditWidget.SetActive(true);//打开本界面

            GameObject.Find("NowFishListType_FB_C").GetComponent<Text>().text = fishContro.FishName(fishData.nowFishList_FB[0].fishKind);//当前鱼的种类赋值
            GameObject.Find("NowFishListIndex_FB_C").GetComponent<Text>().text = "当前鱼群索引:" + fishData.fishBoomListIndex;//当前鱼群在二维鱼群数组中的索引

            FishBoomInitital();//显示当前鱼群所有的鱼和line
            //生成没条鱼的按钮预设物
            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, GameObject.Find("OtherFishList_FB_C").transform);
                // 1.读取配置表中的内容，根据内容在按钮中显示对应的名字,并绑定方法
                object m = i;
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.FB_OtherFishEdit_FishBtn_OnClick(m); });
                o.transform.GetComponentInChildren<Text>().text = fishContro.FishName(fishData.nowFishList_FB[i].fishKind);
            }
            //删除之前的路线，并生成新的路线
            for (int i = 0; i < lineList.Count; i++)
            {
                lineList[i].positionCount = 0;
                for (int j = 0; j < fishData.nowFishList_FB[i].fishPath[1].fishPoint.Count; j++)
                {
                    Vector3 v3 = new Vector3(fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].x, fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].y,0);
                    lineList[i].SetPosition(j, v3);
                }
            }
            SetFishPosAndRot();//重设鱼的初始坐标和角度
        }
        /// <summary>
        /// 应用
        /// </summary>
        private void FB_OtherFishEdit_Apply_OnClick()
        {
            //将该条鱼的新坐标存入字典中
            fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex][fishData.FB_fishIndex].fishPath[1].fishPoint = fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint;
        }
        /// <summary>
        /// 平移X轴输入框监听事件
        /// </summary>
        /// <param name="str"></param>
        private void FB_OtherFishEdit_TranslationX_InputField(string str)
        {
            Text fishIndex = GameObject.Find("OtherFish_NowFishIndex_C").GetComponent<Text>();
            if (fishIndex.text == "" || fishIndex.text == "0") return;
            if (str == "") return;
            float result = 0;
            if (!float.TryParse(str, out result)) { Debug.Log("请输入数字！！！");return; };
            result = float.Parse(str);

            //List<Vector3> temp =
            Vector3[] temp = new Vector3[lineList[fishData.FB_fishIndex].positionCount];
            lineList[fishData.FB_fishIndex].GetPositions(temp);
            lineList[fishData.FB_fishIndex].positionCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].x += result;
            }
            lineList[fishData.FB_fishIndex].SetPositions(temp);
            fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint = ScenesCoordToRealCoord(new List<Vector3>(temp));//将新的坐标赋值给临时数据
            SetFishPosAndRot();//设置鱼的初始位置和角度
        }
        /// <summary>
        /// 平移Y轴输入框监听事件
        /// </summary>
        /// <param name="str"></param>
        private void FB_OtherFishEdit_TranslationY_InputField(string str)
        {
            Text fishIndex = GameObject.Find("OtherFish_NowFishIndex_C").GetComponent<Text>();
            if (fishIndex.text == "" || fishIndex.text == "0") return;
            if (str == "") return;
            float result = 0;
            if (!float.TryParse(str, out result)) { Debug.Log("请输入数字！！！"); return; };
            result = float.Parse(str);

            Vector3[] temp = new Vector3[lineList[fishData.FB_fishIndex].positionCount];
            lineList[fishData.FB_fishIndex].GetPositions(temp);
            lineList[fishData.FB_fishIndex].positionCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].y += result;
            }
            lineList[fishData.FB_fishIndex].SetPositions(temp);
            fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint = ScenesCoordToRealCoord(new List<Vector3>(temp));//将新的坐标赋值给临时数据
            SetFishPosAndRot();//设置鱼的初始位置和角度
        }
        /// <summary>
        /// 旋转输入框监听事件
        /// </summary>
        /// <param name="str"></param>
        private void FB_OtherFishEdit_Rotate_InputField(string str)
        {
            Text fishIndex = GameObject.Find("OtherFish_NowFishIndex_C").GetComponent<Text>();
            if (fishIndex.text == "" || fishIndex.text == "0") return;
            if (str == "") return;
            float result = 0;
            if (!float.TryParse(str, out result)) { Debug.Log("请输入数字！！！"); return; };
            result = float.Parse(str);

            Vector3[] temp = new Vector3[lineList[fishData.FB_fishIndex].positionCount];
            lineList[fishData.FB_fishIndex].GetPositions(temp);
            lineList[fishData.FB_fishIndex].positionCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                float x = (temp[i].x - 970.5f) * Mathf.Cos(result) - (temp[i].y - 750) * Mathf.Sin(result) + 970.5f;
                float y = (temp[i].x - 970.5f) * Mathf.Sin(result) + (temp[i].y - 750) * Mathf.Cos(result) + 750f;
                temp[i].x = x; temp[i].y = y;
            }
            lineList[fishData.FB_fishIndex].SetPositions(temp);

            SetFishPosAndRot();//设置鱼的初始位置和角度
        }
        /// <summary>
        /// 其他鱼编辑界面关闭方法
        /// </summary>
        private void FB_OtherFishEdit_Quit_OnClick()
        {
            Quit_FishBoom();//关闭所有弹窗
            FB_FirstFishEdit_EditShow();//第一条鱼界面初始化方法
        }

        public List<FishVecter> ScenesCoordToRealCoord(List<Vector3> _sceneV3)
        {
            List<FishVecter> scenesFishPoint = new List<FishVecter>();
            for (int i = 0; i < _sceneV3.Count; i++)
            {
                scenesFishPoint.Add(new FishVecter(_sceneV3[i].x - 302, _sceneV3[i].y - 375, fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint[i].speed, fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint[i].time));
            }
            return scenesFishPoint;
        }

        private void FB_OtherFishEdit_FishBtn_OnClick(object _index)
        {
            fishData.FB_fishIndex = (int)_index;//当前选择的鱼的索引
        }
        #endregion

        #region 生成鱼的方法
        private NewCatchFish_NormalFish_Temp CreateFishPrefab_FB(int _fishType, int _nowFishIndex)
        {
            //生成一条鱼
            GameObject go = Resources.Load<GameObject>("Prefabs/Items/NewCatchFish_Temp/" + (ObjectType)(1350 + _fishType - 1));//ObjectPool.Instance.GetAGameObject<NewCatchFish_NormalFish_Temp>((ObjectType)1350 + fishData.nowFishKindIndex, GameObject.Find("BG_C").transform);
            GameObject o = Instantiate(go, GameObject.Find("BG_C").transform);
            fishData.fishObjList.Add(o);

            LineRenderer lineTemp = GameObject.Find("BG_C").AddComponent<LineRenderer>();//添加一个对应的line连线
            lineList.Add(lineTemp);//添加到line列表

            ////如果当前鱼有线，初始化到他的起点
            //if (lineList[_nowFishIndex].positionCount != 0) o.transform.position = lineList[_nowFishIndex].GetPosition(0);

            NewCatchFish_NormalFish_Temp tempFish = o.GetComponent<NewCatchFish_NormalFish_Temp>();

            return tempFish;
        }
        #endregion

        /// <summary>
        /// 清除所有的line和鱼的预设物
        /// </summary>
        private void ClearAllLineAndFish()
        {
            for (int i = 0; i < lineList.Count; i++)
            {
                Destroy(lineList[i]);
            }
            lineList.Clear();
            for (int i = 0; i < fishData.fishObjList.Count; i++)
            {
                Destroy(fishData.fishObjList[i].gameObject);
            }
            fishData.fishObjList.Clear();
        }
        /// <summary>
        /// 设置鱼的初始位置和角度
        /// </summary>
        private void SetFishPosAndRot()
        {
            for (int i = 0; i < fishData.fishObjList.Count; i++)
            {
                if (lineList[i].positionCount >= 1)
                    fishData.fishObjList[i].transform.position = lineList[i].GetPosition(0);//设置鱼的初始位置
                //设置鱼的初始角度
                if (lineList[i].positionCount >= 2)
                {
                    fishData.fishObjList[i].transform.localEulerAngles = new Vector3(0, 0, 0);//首先角度归零
                    fishData.fishObjList[i].transform.localEulerAngles = new Vector3(0, 0, fishContro.GetAngle(lineList[i].GetPosition(0), lineList[i].GetPosition(1)));//首先角度归零
                }
            }
        }
        #endregion



        #region 所有关闭弹窗方法
        private void PathDown_FishBoom()//当前场景所有鱼群弹窗的方法
        {
            FB_AllFishListWidget.SetActive(false); Quit_FishBoom();
        }
        public void Quit_FishBoom()//关闭菜单栏
        {
            if (clockIE != null) StopCoroutine(clockIE);//关闭计时协程
            FB_TypeEditWidget.SetActive(false); FB_FirstFishEditWidget.SetActive(false);
            FB_OtherFishEditWidget.SetActive(false); //FB_ShowFishEditWidget.SetActive(false);
            fishData.isLine = false;
            lineText.text = "开始连线";
        }
        #endregion






        #region 按钮点击事件绑定方法
        /// <summary>
        /// 按钮点击事件绑定方法
        /// </summary>
        private void InitialClickEvent()
        {
            #region 单条鱼
            GameObject.Find("AddOneFishBtn_C").GetComponent<Button>().onClick.AddListener(AddOneFishMessage_OnClick);//添加一条鱼

            GameObject.Find("DestroyOneFishBtn_C").GetComponent<Button>().onClick.AddListener(DestroyOneFishMessage_OnClick);//删除一条鱼

            GameObject.Find("OneFishBtn_C").GetComponent<Button>().onClick.AddListener(SelectOneFishEdit_OnClick);//单条鱼编辑面板

            GameObject.Find("FishBoomBtn_C").GetComponent<Button>().onClick.AddListener(SelectFishBoomEdit_OnClick);//鱼群编辑面板

            GameObject.Find("PathDown_C").GetComponent<Button>().onClick.AddListener(PathDown_OneFish);//关闭路线弹窗

            GameObject.Find("Quit_C").GetComponent<Button>().onClick.AddListener(Quit_OneFish);//关闭编辑窗口

            GameObject.Find("Line_C").GetComponent<Button>().onClick.AddListener(StartLine_OnClick);//开始连线

            GameObject.Find("Clear_C").GetComponent<Button>().onClick.AddListener(ClearLine);//清除场景中的所有点和鱼

            GameObject.Find("CreateMap_C").GetComponent<Button>().onClick.AddListener(CreateConfigEvent);//生成配置表

            GameObject.Find("LastStep_C").GetComponent<Button>().onClick.AddListener(LastStepLine_OnClick);//返回上一步

            GameObject.Find("RunBtn_C").GetComponent<Button>().onClick.AddListener(FishMove);//清除场景中的所有点和鱼

            GameObject.Find("ApplyLine_C").GetComponent<Button>().onClick.AddListener(ReadConfigToList);//应用

            GameObject.Find("ToRun_C").GetComponent<Button>().onClick.AddListener(ToRun_OnClick);//鱼跑

            GameObject.Find("AddInDic_C").GetComponent<Button>().onClick.AddListener(AddThisLineToDic_OnClick);//添加路线到字典

            GameObject.Find("AddOneLine_C").GetComponent<Button>().onClick.AddListener(AddOneLine_OnClick);//添加新的路线

            GameObject.Find("DestroyOneLine_C").GetComponent<Button>().onClick.AddListener(DestroyOneLine_OnClick);//清除场景中的所有点和鱼

            #endregion

            #region
            //*********-----鱼群点击-----*********//
            GameObject.Find("FishBoomPathDown_C").GetComponent<Button>().onClick.AddListener(PathDown_FishBoom);//关闭鱼群的路线弹窗

            GameObject.Find("FishBoomEditQuit_C").GetComponent<Button>().onClick.AddListener(Quit_FishBoom);//关闭鱼群的编辑弹窗

            GameObject.Find("AddOneSceneBtn_C").GetComponent<Button>().onClick.AddListener(AddFishBoomScene_OnClick);//添加一个场景

            GameObject.Find("DestroyOneSceneBtn_C").GetComponent<Button>().onClick.AddListener(DestroyFishScene_OnClick);//删除一个场景

            GameObject.Find("AddOneFishList_C").GetComponent<Button>().onClick.AddListener(AddFishListToScene_OnClick);//添加一个鱼群

            GameObject.Find("DestroyOneFishList_C").GetComponent<Button>().onClick.AddListener(DestroyFishListScene_OnClick);//删除一个鱼群

            GameObject.Find("FishBoomPathDown_C").GetComponent<Button>().onClick.AddListener(Quit_FB_FishBoomList);//退出鱼群弹窗

            GameObject.Find("FishType_FishRunBtn_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_FishRun_OnClick);//鱼群基本面板鱼跑

            GameObject.Find("FishType_SetBtn_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_Apply_OnClick);//鱼群基本面板应用

            GameObject.Find("FishType_EditBtn_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_Edit_OnClick);//鱼群基本面板编辑

            GameObject.Find("FishBoomEditQuit_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_Quit_OnClick);//鱼群基本面板关闭

            GameObject.Find("FirstFish_StartLine_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_StartLine_OnClick);//第一条鱼面板开始连线
            GameObject.Find("FirstFish_LastLine_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_LastStep_OnClick);//第一条鱼面板上一步
            GameObject.Find("FirstFish_ClearAll_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_ClearAllLine_OnClick);//第一条鱼面板清空
            GameObject.Find("FirstFish_Apply_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_Apply_OnClick);//第一条鱼面板应用
            GameObject.Find("FirstFish_Run_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_FishRun_OnClick);//第一条鱼面板鱼跑
            GameObject.Find("FirstFish_OtherFishEdit_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_OtherFishEdit_OnClick);//第一条鱼面板编辑
            GameObject.Find("FirstFishEditQuit_FB_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_Quit_OnClick);//第一条鱼面板关闭

            GameObject.Find("OtherFishEditQuit_FB_C").GetComponent<Button>().onClick.AddListener(FB_OtherFishEdit_Quit_OnClick);//其他鱼面板关闭
            GameObject.Find("OtherFish_SetBtn_C").GetComponent<Button>().onClick.AddListener(FB_OtherFishEdit_Apply_OnClick);//其他鱼面板应用
            #endregion
        }
        #endregion

        void Update()
        {
            LineController_OnClick();//点击连线
        }

        #region 产生配置表（单个鱼）
        /// <summary>
        /// 产生配置表
        /// </summary>
        private void CreateConfigEvent()
        {
            #region
            ////鱼潮场景
            //byte sceneIndex = byte.Parse(GetInputFieldText("SceneIndex_C"));
            ////路线显示++
            //GetInputField("Path_C").text = (path + 1).ToString();
            //Debug.Log(GetInputField("Path_C").text);
            //string[] tempPosition = GetInputFieldText("OriginPoint_C").Split(' ');
            //string[] tempDirection = GetInputFieldText("OriginDirection_C").Split(' ');
            ////获取位置方向
            //float[] position = new float[] { float.Parse(tempPosition[0]), float.Parse(tempPosition[1]) };
            //float[] direction = new float[] { float.Parse(tempDirection[0]), float.Parse(tempDirection[1]) };
            ////速度角速度
            //float speed = float.Parse(GetInputFieldText("Speed_C"));
            //float rotate = float.Parse(GetInputFieldText("RotateRate_C"));


            ////获取对象
            //if (fishModelMap == null)
            //{
            //    fishModelMap = new Dictionary<ushort, Dictionary<byte, Buyu_FishManager.FishMapModel>>();
            //}
            //if (!fishModelMap.ContainsKey(choiceFishType))
            //{
            //    fishModelMap[choiceFishType] = new Dictionary<byte, Buyu_FishManager.FishMapModel>();
            //}
            //fishModelMap[choiceFishType][path] = new Buyu_FishManager.FishMapModel(position, direction, speed, rotate);
            #endregion
            //Temp_CreateConfig();
            //fishContro.GiveFishDic_Save();
            if (fishData.isFishBoom)//鱼群
            {
                //写入配置表
                if (File.Exists(fishData.fishBoomConfigPath))
                {
                    File.Delete(fishData.fishBoomConfigPath);
                }
                //生成配置表
                File.Create(fishData.fishBoomConfigPath).Dispose();
                File.WriteAllText(fishData.fishBoomConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(fishBoomDic));
                Debug.Log("配置表生成成功！");
            }
            else//单条鱼
            {
                //写入配置表
                if (File.Exists(fishData.oneFishConfigPath))
                {
                    File.Delete(fishData.oneFishConfigPath);
                }
                //生成配置表
                File.Create(fishData.oneFishConfigPath).Dispose();
                File.WriteAllText(fishData.oneFishConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(oneFishDic));
                Debug.Log("配置表生成成功！");
            }
        }
        #endregion
    }

    /// <summary>
    /// 鱼类方法管理器
    /// </summary>
    public class FishPathController
    {
        public YuChao_ConfigPanel yuChaoPanel;
        public FishPathController(YuChao_ConfigPanel _yuChaoPanel)
        {
            yuChaoPanel = _yuChaoPanel;
        }



        #region 给鱼字典赋初始值
        public void GiveFishDic_Save()
        {
            yuChaoPanel.oneFishDic = new List<Fish>();
            for (int i = 0; i < 28; i++)
            {
                Fish f = new Fish(0, (byte)(i + 1));
                yuChaoPanel.oneFishDic.Add(f);
                Debug.Log(yuChaoPanel.oneFishDic[i].fishPath[1].fishPoint.Count);
            }
        }
        #endregion

        #region 鱼的名字判断
        public string FishName(int _fishId)
        {
            string str = "";
            switch (_fishId)
            {
                case 1:
                    str = "黄鱼";
                    break;
                case 2:
                    str = "大眼鱼";
                    break;
                case 3:
                    str = "蓝棘鱼";
                    break;
                case 4:
                    str = "阿波罗";
                    break;
                case 5:
                    str = "小丑鱼";
                    break;
                case 6:
                    str = "河豚";
                    break;
                case 7:
                    str = "狮子鱼";
                    break;
                case 8:
                    str = "凤尾鱼";
                    break;
                case 9:
                    str = "龟";
                    break;
                case 10:
                    str = "鲶鱼";
                    break;
                case 11:
                    str = "灯笼鱼";
                    break;
                case 12:
                    str = "剑鱼";
                    break;
                case 13:
                    str = "魔鬼鱼";
                    break;
                case 14:
                    str = "金鲨鱼";
                    break;
                case 15:
                    str = "龙虾";
                    break;
                case 16:
                    str = "飞鱼";
                    break;
                case 17:
                    str = "鳄鱼";
                    break;
                case 18:
                    str = "螃蟹";
                    break;
                case 19:
                    str = "珍珠";
                    break;
                case 20:
                    str = "水母";
                    break;
                case 21:
                    str = "美人鱼";
                    break;
                case 22:
                    str = "独角鲸";
                    break;
                case 23:
                    str = "金蟾";
                    break;
                case 24:
                    str = "炸弹鱼";
                    break;
                case 25:
                    str = "冰冻鱼";
                    break;
                case 26:
                    str = "电击鱼";
                    break;
                case 27:
                    str = "双倍加成";
                    break;
                case 28:
                    str = "宝箱";
                    break;
                default:
                    break;
            }
            return str;
        }
        #endregion

        #region 将vector3坐标转换为FishVector坐标
        public List<FishVecter> V3ToFishV3(List<Vector3> _v3)
        {
            List<FishVecter> fishV3 = new List<FishVecter>();
            for (int i = 0; i < _v3.Count; i++)
            {
                FishVecter k = new FishVecter();
                k.x = _v3[i].x;
                k.y = _v3[i].y;
                //k.speed = _speed;
                //k.time = _time;
                fishV3.Add(k);
            }
            return fishV3;
        }
        #endregion

        #region 1.将存储的坐标转换为屏幕显示的坐标 2.将屏幕坐标转换为存储的坐标
        public FishPath RealCoordToScenesCoord(FishPath _fishPath)
        {
            FishPath realPath = new FishPath();
            realPath.costTime = _fishPath.costTime;
            realPath.pathId = _fishPath.pathId;
            realPath.pathName = _fishPath.pathName;
            realPath.fishPoint = new List<FishVecter>();

            for (int i = 0; i < _fishPath.fishPoint.Count; i++)
            {
                realPath.fishPoint.Add(new FishVecter(_fishPath.fishPoint[i].x + 302, _fishPath.fishPoint[i].y + 375, _fishPath.fishPoint[i].speed, _fishPath.fishPoint[i].time));
            }
            return realPath;
        }
        #endregion

        #region 算出一个点阵列表中的time
        public FishPath Time_FishV3(FishPath _fishPath)
        {
            List<FishVecter> fishV3 = new List<FishVecter>();

            float times = 0;
            for (int i = 0; i < _fishPath.fishPoint.Count - 1; i++)
            {
                if(_fishPath.fishPoint[i].speed == 0)
                {
                    Debug.Log("第" + (i+1) + "个点的数值不完成");
                    break;
                }
                Vector2 a = new Vector2(_fishPath.fishPoint[i].x, _fishPath.fishPoint[i].y);
                Vector2 b = new Vector2(_fishPath.fishPoint[i + 1].x, _fishPath.fishPoint[i + 1].y);
                _fishPath.fishPoint[i].time = (((b - a).magnitude) / _fishPath.fishPoint[i].speed) * 1000;
                times += _fishPath.fishPoint[i].time;
            }
            int timeTemp = (int)times;
            _fishPath.costTime = timeTemp;
            return _fishPath;
        }
        #endregion

        #region 计算两点之间角度差值
        public float GetAngle(Vector3 a, Vector3 b)
        {
            b.x -= a.x;
            b.z -= a.z;

            float deltaAngle = 0;
            if (b.x == 0 && b.z == 0)
            {
                return 0;
            }
            else if (b.x > 0 && b.z > 0)
            {
                deltaAngle = 0;
            }
            else if (b.x > 0 && b.z == 0)
            {
                return 90;
            }
            else if (b.x > 0 && b.z < 0)
            {
                deltaAngle = 180;
            }
            else if (b.x == 0 && b.z < 0)
            {
                return 180;
            }
            else if (b.x < 0 && b.z < 0)
            {
                deltaAngle = -180;
            }
            else if (b.x < 0 && b.z == 0)
            {
                return -90;
            }
            else if (b.x < 0 && b.z > 0)
            {
                deltaAngle = 0;
            }

            float angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
            return angle;
        }

        #endregion
    }

    /// <summary>
    /// 鱼路线数据类
    /// </summary>
    public class FishPathDataManager
    {
        public YuChao_ConfigPanel yuChaoPanel;
        public FishPathDataManager(YuChao_ConfigPanel _yuChaoPanel)
        { yuChaoPanel = _yuChaoPanel; }

        /// <summary>
        /// 是否是鱼群编辑面板
        /// </summary>
        public bool isFishBoom;
        /// <summary>
        /// 当前场景中的鱼
        /// </summary>
        public NewCatchFish_NormalFish_Temp nowFish;
        /// <summary>
        /// 当前场景中的路线（点击生成时赋值）
        /// </summary>
        public FishPath nowFishPath;

        /// <summary>
        /// 一共多少种鱼
        /// </summary>
        public int fishNum;
        /// <summary>
        /// 当前鱼在总列表中的索引
        /// </summary>
        public int nowFishKindIndex;
        /// <summary>
        /// 当前路线索引
        /// </summary>
        public int nowPathIdIndex;
        /// <summary>
        /// 是否开始连线
        /// </summary>
        public bool isLine;
        /// <summary>
        /// 是否开始编辑
        /// </summary>
        public bool isEdit;
        /// <summary>
        /// 开始计时
        /// </summary>
        public bool isTime;
        /// <summary>
        /// 鱼的信息（用于鱼跑）
        /// </summary>
        public NewCatchFish_FishInfo fishInfo;


        /// <summary>
        /// 当前选择的鱼群列表的场景号
        /// </summary>
        public byte fishBoomSceneIndex;
        /// <summary>
        /// 当前选择的鱼群的索引
        /// </summary>
        public int fishBoomListIndex;
        /// <summary>
        /// 当前选择的鱼的索引
        /// </summary>
        public int FB_fishIndex;
        /// <summary>
        /// 当前场景中的鱼群
        /// </summary>
        public List<Fish> nowFishList_FB;
        /// <summary>
        /// 当前场景中所有的鱼群
        /// </summary>
        public List<List<Fish>> nowFishList_FB_Two;
        /// <summary>
        /// 当前选择的鱼
        /// </summary>
        public NewCatchFish_NormalFish_Temp nowFish_FB;
        /// <summary>
        /// 当前场景中的鱼预设物
        /// </summary>
        public List<GameObject> fishObjList;

        //配置表路径
        public string oneFishConfigPath = Application.streamingAssetsPath + "/" + AppConst.Buyu_FIshKind_ConfigPath;
        public string fishBoomConfigPath = Application.streamingAssetsPath + "/" + AppConst.Buyu_FIshBoom_ConfigPath;

        public void DataInitialize()
        {
            fishNum = 28;
            nowFishPath = new FishPath();
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
        //public List<FishPath> fishPath;
        public Fish()
        { //fishPath = new Dictionary<int, FishPath>();
        }
        public Fish(int _fishId, byte _fishKind)
        {
            fishId = _fishId; fishKind = _fishKind; fishPath = new Dictionary<int, FishPath>(); fishPath.Add(1, new FishPath());
        }
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

        //public FishPath()
        //{
        //    //pathId = 1;
        //    //pathName = "路线1";
        //    //FishVecter v1 = new FishVecter(-100, 375, 100, (new Vector2(1470, 375) - new Vector2(-100, 375)).magnitude / 100);
        //    //FishVecter v2 = new FishVecter(1470, 375, 0, 0);
        //    //fishPoint = new List<FishVecter>();
        //    //fishPoint.Add(v1);
        //    //fishPoint.Add(v2);
        //}
    }
    public class FishVecter
    {
        public float x;
        public float y;
        public float speed;
        public float time;
        public FishVecter() { }
        public FishVecter(float _x, float _y, float _speed, float _time)
        {
            x = _x; y = _y; speed = _speed; time = _time;
        }
    }
}