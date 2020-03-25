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
        #region �����ֶ�
        public FishPathController fishContro;
        public FishPathDataManager fishData;

        /// <summary>
        /// ���ߵ����
        /// </summary>
        private LineRenderer line;
        //���������ʱ���������ʹ��Ⱥ��ʱ��ʼ��ʱ��
        private static bool isCounting = false;
        //����
        private GameObject clock;
        private IEnumerator clockIE;
        #endregion

        #region �����㶨����ֶ�
        /// <summary>
        /// ���е��������ݵ��б�
        /// </summary>
        public List<Fish> oneFishDic;
        /// <summary>
        /// ��ǰ�����λ�õ���ʵ�����б�
        /// </summary>
        private List<Vector3> lineV3List;
        /// <summary>
        /// ���������������ñ�������б�
        /// </summary>
        private List<Vector3> realLineV3;
        /// <summary>
        /// ����ʱ�ĵ�������������ʱ����
        /// </summary>
        private int lineCount;
        #endregion

        #region ��Ⱥ���ֶ�
        /// <summary>
        /// ���ߵ��б�
        /// </summary>
        private List<LineRenderer> lineList;
        /// <summary>
        /// ������Ⱥ���ֵ�
        /// </summary>
        Dictionary<byte, List<List<Fish>>> fishBoomDic;
        /// <summary>
        /// �Ƿ�ʼ����
        /// </summary>
        bool isLine_FB;
        /// <summary>
        /// �������Ƿ�ʼ�༭
        /// </summary>
        bool isOtherStart;
        /// <summary>
        /// ��Ⱥ����ʱ�ĵ�������������ʱ����
        /// </summary>
        private int lineCount_FB;
        #endregion

        #region
        public GameObject oneFishPanel;//������༭����
        public GameObject oneFishPathWidget;//������·�߽���
        public GameObject oneFishEditWidget;//���������༭����
        public Text lineText;//��ʼ���߰�ť

        //FB��FishBoom
        /// <summary>
        /// ��Ⱥ�༭�ܽ���
        /// </summary>
        public GameObject FB_Panel;
        /// <summary>
        /// ��ǰѡ���ĳ�����������Ⱥ�ĵ���
        /// </summary>
        public GameObject FB_AllFishListWidget;
        /// <summary>
        /// ��Ⱥ�������ͱ༭����
        /// </summary>
        public GameObject FB_TypeEditWidget;
        /// <summary>
        /// ��ǰ��Ⱥ��һ����ı༭����
        /// </summary>
        public GameObject FB_FirstFishEditWidget;
        /// <summary>
        ///������ı༭����
        /// </summary>
        public GameObject FB_OtherFishEditWidget;
        /// <summary>
        /// չʾ������Ⱥ�Ľ���
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

            InitalPanel();//��ʼ��������Ϣ
        }

        #region �����ʼ������
        private void InitalPanel()
        {
            clock = GameObject.Find("Clock_C");//��ʱ��
            clockIE = CountingTimes();//��ʱЭ��
            line = GameObject.Find("BG_C").GetComponent<LineRenderer>();//����
            lineList = new List<LineRenderer>(); //lineList.Add(line);//����һ�����������ӵ��б�

            oneFishPanel = GameObject.Find("OneFishEdit_C");//��������ܱ༭����
            oneFishPathWidget = GameObject.Find("oneFishPathScr_C");
            oneFishEditWidget = GameObject.Find("oneEdit_C");

            FB_Panel = GameObject.Find("FishBoomEdit_C");//��Ⱥ�༭����
            FB_AllFishListWidget = GameObject.Find("FishBoomListScr_C");//���������µ�������Ⱥ
            FB_TypeEditWidget = GameObject.Find("FishBoomEdit_FishType_C");//��ǰ��Ⱥ�����������ý���
            FB_FirstFishEditWidget = GameObject.Find("FishBoomEdit_FirstFish_C");//��ǰ��Ⱥ��һ����༭����
            FB_OtherFishEditWidget = GameObject.Find("FishBoomEdit_OtherFish_C");//��ǰ��Ⱥ������ı༭����

            X_Tran = GameObject.Find("TranslationX_C").GetComponent<InputField>();
            Y_Tran = GameObject.Find("TranslationY_C").GetComponent<InputField>();
            rotate = GameObject.Find("Rotation_C").GetComponent<InputField>();
            X_Tran.onEndEdit.AddListener(FB_OtherFishEdit_TranslationX_InputField);
            Y_Tran.onEndEdit.AddListener(FB_OtherFishEdit_TranslationY_InputField);
            rotate.onEndEdit.AddListener(FB_OtherFishEdit_Rotate_InputField);

            lineText = GameObject.Find("LineTXT_C").GetComponent<Text>();
            InitialClickEvent();//��ʼ������¼�
            InitialConfigDictionary_OneFish();//��ʼ�������������ֵ�
            InitialConfigDictionary_FishBoom();

            oneFishPathWidget.SetActive(false); oneFishEditWidget.SetActive(false);
            FB_Panel.SetActive(false); FB_AllFishListWidget.SetActive(false); FB_TypeEditWidget.SetActive(false);
            FB_FirstFishEditWidget.SetActive(false); FB_OtherFishEditWidget.SetActive(false);
        }
        #endregion

        #region ��ʼ����������ȡ��ǰ�������ļ��ķ��������������ļ����ɶ�Ӧ�㰴ť�Ļ�����
        /// <summary>
        /// ��ʼ�������ֵ䣨�����㣩
        /// </summary>
        private void InitialConfigDictionary_OneFish()
        {
            if (File.Exists(fishData.oneFishConfigPath))
            {
                oneFishDic = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Fish>>(File.ReadAllText(fishData.oneFishConfigPath));
                Debug.Log("�����������ֵ��ʼ����ɣ�" + oneFishDic.Count);

                Debug.Log("���е�ĳ��ȣ�" + oneFishDic[0].fishPath[1].fishPoint.Count);
                InitialFishTypeScrollView();
                return;
            }
            Debug.Log("�����������ļ�������");
        }
        /// <summary>
        /// ��ʼ������������Ļ�����ť
        /// </summary>
        private void InitialFishTypeScrollView()
        {
            for (int i = 0; i < oneFishDic.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, GameObject.Find("FishContent_C").transform);
                // 1.��ȡ���ñ��е����ݣ����������ڰ�ť����ʾ��Ӧ������,���󶨷���
                object m = i;
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.FishTypeButton_OnClick(m); });
                o.transform.GetComponentInChildren<Text>().text = fishContro.FishName(oneFishDic[i].fishKind);
            }
        }
        /// <summary>
        /// ��ʼ�������ֵ䣨��Ⱥ��
        /// </summary>
        private void InitialConfigDictionary_FishBoom()
        {
            if (File.Exists(fishData.fishBoomConfigPath))
            {
                fishBoomDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<byte, List<List<Fish>>>>(File.ReadAllText(fishData.fishBoomConfigPath));
                Debug.Log("��Ⱥ�����ֵ��ʼ����ɣ�" + fishBoomDic.Count);
                InitialFishBoomScrollView();
                return;
            }
            Debug.Log("��Ⱥ�㳱�����ļ�������");
        }
        /// <summary>
        /// ��ʼ������������Ⱥ�Ļ�����ť
        /// </summary>
        private void InitialFishBoomScrollView()
        {
            foreach (var item in fishBoomDic)
            {
                //����Ԥ����
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, GameObject.Find("FishBoomContent_C").transform);
                //�󶨵������������
                object m = item;
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.SceneFishBoom_OnClick(m); });
                o.transform.GetComponentInChildren<Text>().text = "������:" + item.ToString();
            }
        }
        #endregion



        #region ��ť����¼�

        #region ��Ⱥ�͵�����ı༭ѡ��
        /// <summary>
        /// ѡ������༭���
        /// </summary>
        private void SelectOneFishEdit_OnClick()
        {
            if (!fishData.isFishBoom) return;
            if (clockIE != null) StopCoroutine(clockIE);//�رռ�ʱЭ��
            oneFishPanel.SetActive(true);
            FB_Panel.SetActive(false);
            //������
            line.positionCount = 0;
            fishData.isFishBoom = false;//�Ƿ�����ȺΪfalse
            PathDown_FishBoom();PathDown_OneFish();//�ر����е���
            //ɾ�������line���
            int count = lineList.Count;
            while (count > 1)
            {
                Destroy(lineList[count]);
                count--;
            }
        }
        /// <summary>
        /// ѡ�������༭���
        /// </summary>
        private void SelectFishBoomEdit_OnClick()
        {
            if (fishData.isFishBoom) return;
            if(fishData.nowFish != null) Destroy(fishData.nowFish.gameObject);//ɾ����ǰ����
            if (clockIE != null) StopCoroutine(clockIE);//�رռ�ʱЭ��
            oneFishPanel.SetActive(false);        
            FB_Panel.SetActive(true);
            //������
            line.positionCount = 0;
            fishData.isFishBoom = true;//�Ƿ�����ȺΪtrue
            PathDown_FishBoom(); PathDown_OneFish();//�ر����е���
        }
        #endregion

        #region ������ɾ����ķ���
        private void AddOneFishMessage_OnClick()
        {
            if (GameObject.Find("AddOneFishName_C").GetComponent<InputField>().text == "") { EricDebug.Debu.LogR("������ֲ���Ϊ��"); return; }
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
            GameObject o = Instantiate(go, GameObject.Find("FishContent_C").transform);

            // 1.��ȡ���ñ��е����ݣ����������ڰ�ť����ʾ��Ӧ������,���󶨷���
            //Debug.Log(oneFishDic[i].fishKind);
            object m = oneFishDic.Count;
            o.GetComponent<Button>().onClick.AddListener(delegate () { this.FishTypeButton_OnClick(m); });
            o.transform.GetComponentInChildren<Text>().text = GameObject.Find("AddOneFishName_C").GetComponent<InputField>().text;//�µ��������
            Fish newFish = new Fish(0, (byte)(oneFishDic.Count + 1));//����һ���µ������
            newFish.fishPath = new Dictionary<int, FishPath>();
            FishPath newFishPath = new FishPath();
            newFishPath.pathId = 1;
            newFishPath.pathName = "·��1";
            newFishPath.fishPoint = new List<FishVecter>();
            newFishPath.fishPoint.Add(new FishVecter(-80, 375, 100, (((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f)));
            newFishPath.fishPoint.Add(new FishVecter(1450, 375, 0, 0));
            newFish.fishPath.Add(1,newFishPath);
            oneFishDic.Add(newFish);

            GameObject.Find("AddOneFishName_C").GetComponent<InputField>().text = "";//��������ÿ�

            Debug.Log("�����һ���㣡����");
        }

        private void DestroyOneFishMessage_OnClick()
        {
            Destroy(GameObject.Find("FishContent_C").transform.GetChild(oneFishDic.Count - 1).gameObject);//ɾ�����һ����
            oneFishDic.RemoveAt(oneFishDic.Count - 1);//ɾ���ֵ������һ����
            PathDown_OneFish();//�ر�·�ߺͱ༭����

            Debug.Log("ɾ����һ���㣡����");
        }
        #endregion

        #region ��ʼ���߰�ť����������ɣ�
        /// <summary>
        /// �����ť����ʼ����
        /// </summary>
        private void StartLine_OnClick()
        {
            if (!fishData.isLine)
            {
                fishData.isLine = true;
                lineText.text = "ֹͣ����";
                //���������Ѵ��ڵĵ�����б�
                lineCount = line.positionCount;
                lineV3List = new List<Vector3>(); realLineV3 = new List<Vector3>();
                Debug.Log("��ǰ������" + lineCount);
                for (int i = 0; i < lineCount; i++)
                {
                    lineV3List.Add(line.GetPosition(i));
                    realLineV3.Add(new Vector3(line.GetPosition(i).x - 302, line.GetPosition(i).y - 375, 0));
                }
            }
            else
            {
                fishData.isLine = false;
                lineText.text = "��ʼ����";
            }

        }
        #endregion

        #region ���ÿ��·�߰�ť����¼�
        /// <summary>
        /// ������İ�ť����¼���������ɵ�������ʾ��Ӧ���·�߱༭���ڣ�
        /// </summary>
        public void FishTypeButton_OnClick(object _index)
        {
            Quit_OneFish();//�رձ༭����
            //�򿪵����������ݶ�Ӧ���ID�����ɶ�Ӧ��·�߱༭��ť
            if (!oneFishPathWidget.activeSelf) oneFishPathWidget.SetActive(true);//��·�ߵ���
            fishData.nowFishKindIndex = (int)_index;
            FishPathShow();//��ʾ��Ӧ���·�ߵ���
        }
        /// <summary>
        /// ���·�߰�ť���ɷ���
        /// </summary>
        private void FishPathShow()
        {
            //ɾ��֮ǰ������
            int count = GameObject.Find("PathContent_C").transform.childCount;
            while (count > 0)
            {
                Destroy(GameObject.Find("PathContent_C").transform.GetChild(count - 1).gameObject);
                count--;
            }
            //��ȡ���ñ��ж�Ӧ���·���б������б�����·�߰�ť���������õ���Ϣ����д��Ӧ��·������
            //��ȡ��ǰ��·�����������û��·�ߣ�Ĭ��Ϊ1��
            Debug.Log(oneFishDic[fishData.nowFishKindIndex].fishPath.Count);
            for (int i = 0; i < oneFishDic[fishData.nowFishKindIndex].fishPath.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishPath_C");
                GameObject o = Instantiate(go, GameObject.Find("PathContent_C").transform);
                // 1.��ȡ���ñ��е����ݣ����������ڰ�ť����ʾ��Ӧ������,���󶨷���
                o.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "·��" + oneFishDic[fishData.nowFishKindIndex].fishPath[i + 1].pathId;
                o.transform.GetChild(1).GetComponent<InputField>().text = oneFishDic[fishData.nowFishKindIndex].fishPath[i + 1].pathName;
                object k = i + 1;
                o.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { FishPathBtn_OnClick(oneFishDic[fishData.nowFishKindIndex], k); });
            }
        }
        /// <summary>
        /// ���·�߱༭��ť����¼�
        /// </summary>
        private void FishPathBtn_OnClick(Fish _fish, object _index)
        {
            if(clockIE != null) StopCoroutine(clockIE);//�رռ�ʱЭ��
            fishData.nowPathIdIndex = (int)_index;
            fishData.nowFishPath = oneFishDic[fishData.nowFishKindIndex].fishPath[fishData.nowPathIdIndex];
            fishData.nowFishPath.pathName = GameObject.Find("PathContent_C").transform.GetChild((int)_index - 1).GetChild(1).GetComponent<InputField>().text;
            oneFishEditWidget.SetActive(true);  //�򿪱༭���
            fishData.isLine = false; lineText.text = "��ʼ����";
            FishPathMessage_Show(_fish, _index);//����Ӧ·�ߵ������Ϣ����˵��У������ٶ�ʱ�䣩
            FishPathLine_Show(_fish, _index);//����·����Ϣ�ڳ��������ɶ�Ӧ���ߣ����ڳ�ʼλ�����ɶ�Ӧ����
        }
        #endregion

        #region ������һ��
        /// <summary>
        /// ���߷�����һ��
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

        #region �����ǰ�����������ߵķ���
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

            //��������е���
        }
        #endregion

        #region Ӧ��
        /// <summary>
        /// ���ֶ����õĵ���ֵ���������е���ʱ����
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
                    Debug.Log("�ٶȲ���Ϊ0������");
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
            EricDebug.Debu.LogB("��ʱ�䣺" + fishData.nowFishPath.costTime);
            Debug.Log("Ӧ�óɹ�������");
        }
        #endregion

        #region ���һ��·��
        private void AddOneLine_OnClick()
        {
            //if (!fishData.isLine) return;
            FishPath newFishPath = new FishPath();
            newFishPath.pathId = oneFishDic[fishData.nowFishKindIndex].fishPath.Count + 1;
            newFishPath.pathName = "·��" + (oneFishDic[fishData.nowFishKindIndex].fishPath.Count + 1);
            newFishPath.fishPoint = new List<FishVecter>();
            newFishPath.fishPoint.Add(new FishVecter(-80, 375, 100, (((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f)));
            newFishPath.fishPoint.Add(new FishVecter(1450, 375, 0, 0));
            oneFishDic[fishData.nowFishKindIndex].fishPath.Add(newFishPath.pathId, newFishPath);
            //����Ԥ���ﵽ��Ӧ��������
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishPath_C");
            GameObject o = Instantiate(go, GameObject.Find("PathContent_C").transform);
            // 1.��ȡ���ñ��е����ݣ����������ڰ�ť����ʾ��Ӧ������,���󶨷���
            o.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "·��" + newFishPath.pathId;
            o.transform.GetChild(1).GetComponent<InputField>().text = newFishPath.pathName;
            object k = newFishPath.pathId;
            o.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { FishPathBtn_OnClick(oneFishDic[fishData.nowFishKindIndex], k); });
        }
        #endregion

        #region ɾ��һ��·��
        private void DestroyOneLine_OnClick()
        {
            //if (!fishData.isLine) return;
            if (oneFishDic[fishData.nowFishKindIndex].fishPath.Count == 1)
            {
                Debug.Log("���ٱ���һ��·��");
                return;
            }
            Destroy(GameObject.Find("PathContent_C").transform.GetChild(oneFishDic[fishData.nowFishKindIndex].fishPath.Count - 1).gameObject);
            oneFishDic[fishData.nowFishKindIndex].fishPath.Remove(oneFishDic[fishData.nowFishKindIndex].fishPath.Count);
            //�رձ༭����
            Quit_OneFish();
        }
        #endregion

        #region ���������·���ֵ�
        public void AddThisLineToDic_OnClick()
        {
            if (!fishData.isLine) return;
            oneFishDic[fishData.nowFishKindIndex].fishPath[fishData.nowPathIdIndex] = fishData.nowFishPath;
            Debug.Log(fishData.nowFishPath.fishPoint[0].x);
            Debug.Log("�ɹ���ӵ��ֵ䣻��ǰ��������ǣ�" + fishData.nowFishKindIndex + "��" + "��ǰ·����key�ǣ�" + fishData.nowPathIdIndex);
        }
        #endregion

        #endregion

        #region 1.���ɶ�Ӧ���㣻2.���ܣ�3.������
        private NewCatchFish_NormalFish_Temp CreateFishPrefab()
        {
            Debug.Log("���������" + fishData.nowFishKindIndex);
            //������һ����
            if (fishData.nowFish != null) Destroy(fishData.nowFish.gameObject);
            //����һ����
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
        /// ������
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
        
        #region �߼�����

        #region ��ʱ��
        /// <summary>
        /// ��ʱ��
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

        #region �����Ļ���line�ߵķ���
        private void LineController_OnClick()
        {
            if (fishData.isLine)//�����ʼ����
            {
                if (Input.GetMouseButtonDown(0))//������������
                {
                    if (Input.mousePosition.x > 1800 || Input.mousePosition.y < 153) return;//����������귶Χ�ͷ���

                    lineCount++;//���������һ
                    Vector3 mouse1 = Input.mousePosition;
                    Vector3 mouse2 = new Vector3(mouse1.x - 302, mouse1.y - 375);
                    lineV3List.Add(mouse1);
                    realLineV3.Add(mouse2);
                    //�ڳ��������ɶ�Ӧ����
                    line.positionCount = lineCount;
                    for (int i = 0; i < lineCount; i++)
                    {
                        line.SetPosition(i, lineV3List[i]);
                    }
                    //�ڵ��������ɶ�Ӧ��Ԥ������ʾ
                    GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/FishV3_C");
                    GameObject o = Instantiate(go, GameObject.Find("FishPointList_C").transform);
                    //��������Ϣ����Ԥ�������ʾ����
                    o.transform.GetChild(4).GetComponent<Text>().text = (line.positionCount - 1).ToString();//��ʾ��ǰ�������
                    o.transform.GetChild(0).GetComponent<InputField>().text = mouse2.x.ToString();//��ʾx����
                    o.transform.GetChild(1).GetComponent<InputField>().text = mouse2.y.ToString();//��ʾy����
                    if (lineV3List.Count == 1)//����ǵ�һ���㣬������һ����Ӧ����
                    {
                        fishData.nowFish = CreateFishPrefab();//������
                    }
                    //FishRotation_Set();//���¶�λ��ĽǶ�
                    Debug.Log("��ǰ��ʵ����:" + mouse1);
                    Debug.Log("�������õ�����:" + mouse2);
                }
            }
        }
        #endregion

        #region 1.����Ӧ���Ӧ·�ߵ���Ϣ����InputFild�ķ���; 2.����Ӧ��Ķ�Ӧ·���ڳ�������ʾ�ķ���
        private void FishPathMessage_Show(Fish _fish, object index)//_index����ǰ·�ߵ�����
        {
            int _index = (int)index;
            GameObject.Find("NowFishName_C").GetComponent<Text>().text = fishContro.FishName(_fish.fishKind);//��ʾ��ǰѡ�������
            GameObject.Find("NowLineNum_C").GetComponent<Text>().text = _fish.fishPath[_index].pathName;//��ʾ��ǰѡ���·��ID
            List<FishVecter> tempFish = _fish.fishPath[_index].fishPoint;//���ݸ�·�ߵĵ����б�ĳ������ɶ�Ӧ������Ԥ����
                                                                         //ɾ��֮ǰ��Ԥ����
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
                //��������Ϣ����Ԥ�������ʾ����
                o.transform.GetChild(4).GetComponent<Text>().text = i.ToString();//��ʾ��ǰ�������
                o.transform.GetChild(0).GetComponent<InputField>().text = tempFish[i].x.ToString();
                o.transform.GetChild(1).GetComponent<InputField>().text = tempFish[i].y.ToString();
                o.transform.GetChild(2).GetComponent<InputField>().text = tempFish[i].speed.ToString();
                o.transform.GetChild(3).GetComponent<InputField>().text = tempFish[i].time.ToString();
            }
        }

        private void FishPathLine_Show(Fish _fish, object _index)
        {
            int index = (int)_index;
            //ɾ��֮ǰ��·��
            line.positionCount = 0;
            line.positionCount = _fish.fishPath[index].fishPoint.Count;

            for (int i = 0; i < _fish.fishPath[index].fishPoint.Count; i++)
            {
                Vector3 v3 = new Vector3(_fish.fishPath[index].fishPoint[i].x + 302, _fish.fishPath[index].fishPoint[i].y + 375, 0);
                line.SetPosition(i, v3);
            }
            fishData.nowFish = CreateFishPrefab();//���ɶ�Ӧ����
        }
        #endregion

        #region ���йرյ�������
        private void PathDown_OneFish()//�ر�·����
        {
            oneFishPathWidget.SetActive(false); Quit_OneFish();
        }
        public void Quit_OneFish()//�رղ˵���
        {
            oneFishEditWidget.SetActive(false);
            fishData.isLine = false;
            lineText.text = "��ʼ����";
        }
        #endregion

        #endregion


        //-------------------***�㳱����***-------------------//

        #region ��Ⱥ

        #region ��������Ⱥ�ĵ���¼�
        /// <summary>
        /// ��Ⱥ������ť����¼�
        /// </summary>0
        private void SceneFishBoom_OnClick(object _index)
        {
            Quit_FishBoom();//�رձ༭����
            if (!FB_AllFishListWidget.activeSelf) FB_AllFishListWidget.SetActive(true);//�򿪵�ǰѡ��ĳ�����������Ⱥ��ʾ����
            fishData.fishBoomSceneIndex = (byte)(int)_index;
            fishData.nowFishList_FB_Two = fishBoomDic[fishData.fishBoomSceneIndex];//����ǰѡ���ĳ�����������Ⱥ��ֵ��������
            BeforeFishBoomShow();//��ʾ��Ӧ��Ⱥ��·�ߵ���
        }
        /// <summary>
        /// ��ǰ������Ⱥ��������ʾ
        /// </summary>
        private void BeforeFishBoomShow()
        {
            Transform beforeFishBoomParent = GameObject.Find("BeforeFishBoomContent_C").transform;//��ȡ��ʾ��ǰѡ��ĳ�����������Ⱥ������
            //ɾ��֮ǰ������
            int count = beforeFishBoomParent.childCount;
            while (count > 0)
            {
                Destroy(beforeFishBoomParent.GetChild(count - 1).gameObject);
                count--;
            }
            //��ȡ���ñ��ж�Ӧ��Ⱥ��·���б������б�����·�߰�ť���������õ���Ϣ����д��Ӧ��·������
            //��ȡ��ǰ��·�����������û��·�ߣ�Ĭ��Ϊ1��
            for (int i = 0; i < fishBoomDic[fishData.fishBoomSceneIndex].Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, beforeFishBoomParent);
                // 1.��ȡ���ñ��е����ݣ����������ڰ�ť����ʾ��Ӧ������,���󶨷���
                string fishName = "";
                if (fishBoomDic[fishData.fishBoomSceneIndex][i][0].fishKind == 0) fishName = "δѡ��";
                else fishName = fishContro.FishName(fishBoomDic[fishData.fishBoomSceneIndex][i][0].fishKind);
                o.transform.GetComponentInChildren<Text>().text ="��" + (i + 1) + "����:" + fishName;
                object k = i;
                o.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate () { OneFishBoomEdit_OnClick(fishBoomDic[fishData.fishBoomSceneIndex][(int)k], k); });
            }
        }
        /// <summary>
        /// ������Ⱥ��ť��ÿ����Ⱥ�ĵ���¼�
        /// </summary>
        /// <param name="_fishList_Two"></param>
        /// <param name="_index"></param>
        private void OneFishBoomEdit_OnClick(List<Fish> _fishList_Two, object _index)
        {
            fishData.fishBoomListIndex = (int)_index;
            fishData.nowFishList_FB = _fishList_Two;//����ǰѡ�����Ⱥ��ֵ�����������ʱ�ֶ�
            //�ر�����������ֻ�򿪵�һ����༭���
            Quit_FishBoom(); FB_TypeEdit_BasicMessage_Show();//�򿪻�����Ϣ�༭��岢��ʼ��
        }
        #endregion

        #region ��Ⱥ�������ɾ���������¼�
        /// <summary>
        /// ���һ����Ⱥ����
        /// </summary>
        private void AddFishBoomScene_OnClick()
        {
            //����Ԥ����
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
            GameObject o = Instantiate(go, GameObject.Find("FishBoomContent_C").transform);
            //�󶨵������������
            object m = fishBoomDic.Count;
            o.GetComponent<Button>().onClick.AddListener(delegate () { this.SceneFishBoom_OnClick((int)m - 1); });
            o.transform.GetComponentInChildren<Text>().text = "������:" + m.ToString();
            List<List<Fish>> temp = new List<List<Fish>>();
            fishBoomDic.Add((byte)(fishBoomDic.Count), temp);//�����һ���µ���Ⱥ�������ֵ�
            PathDown_FishBoom();//�ر����е���
        }
        /// <summary>
        /// ɾ��һ����Ⱥ����
        /// </summary>
        private void DestroyFishScene_OnClick()
        {
            if (fishBoomDic.Count == 1) return;//������һ��
            //ɾ�����һ��Ԥ����
            Destroy(GameObject.Find("FishBoomContent_C").transform.GetChild(fishBoomDic.Count - 1).gameObject);
            fishBoomDic.Remove((byte)(fishBoomDic.Count - 1));
            PathDown_FishBoom();//�ر����е���
        }
        #endregion

        #region ��ǰ�����е�������Ⱥ������¼����˳��¼������һ����Ⱥ��ɾ��һ����Ⱥ��
        /// <summary>
        /// �ڵ�ǰ�������һ����Ⱥ
        /// </summary>
        private void AddFishListToScene_OnClick()
        {
            //����Ԥ����
            GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
            GameObject o = Instantiate(go, GameObject.Find("FB_FishBoomListContent_C").transform);

            int tempIndex = fishBoomDic[fishData.fishBoomSceneIndex].Count;//��ȡ��ǰ��ά�б�ĳ���
            o.transform.GetComponentInChildren<Text>().text = "��" + (tempIndex) + "����:δѡ��";
            fishBoomDic[fishData.fishBoomSceneIndex].Add(new List<Fish>());//�½�һ����Ⱥ�������ֵ�
            object k = tempIndex;
            o.GetComponent<Button>().onClick.AddListener(delegate() { OneFishBoomEdit_OnClick(fishBoomDic[fishData.fishBoomSceneIndex][(int)k], k); });

            Quit_FishBoom();//�ر����е��༭����
        }
        /// <summary>
        /// ɾ��һ����ǰ��������Ⱥ
        /// </summary>
        private void DestroyFishListScene_OnClick()
        {
            if (fishBoomDic[fishData.fishBoomSceneIndex].Count == 1) return;//������һ��
            //ɾ�������е����
            Destroy(GameObject.Find("FB_FishBoomListContent_C").transform.GetChild(fishBoomDic[fishData.fishBoomSceneIndex].Count - 1).gameObject);
            //ɾ���ֵ��ж�Ӧ������
            fishBoomDic[fishData.fishBoomSceneIndex].RemoveAt(fishBoomDic[fishData.fishBoomSceneIndex].Count - 1);
            Quit_FishBoom();//�ر����е��༭����
        }
        /// <summary>
        /// �ر���Ⱥ�б���
        /// </summary>
        private void Quit_FB_FishBoomList()
        {
            PathDown_FishBoom();//�ر����е���
            ClearAllLineAndFish();//���֮ǰ���������������
        }
        #endregion

        #region ��Ⱥ�����������ķ���
       /// <summary>
       /// ��Ⱥ�������������ʾ����
       /// </summary>
        private void FB_TypeEdit_BasicMessage_Show()
        {
            ClearAllLineAndFish();//���֮ǰ���������������
            FB_TypeEditWidget.SetActive(true);
            //��ȡ��ǰ��������
            InputField fishType_Inpt = GameObject.Find("FishType_Inpt_C").GetComponent<InputField>();
            InputField fishCount_Inpt = GameObject.Find("FishNum_Inpt_C").GetComponent<InputField>();
            fishType_Inpt.text = ""; fishCount_Inpt.text = "";//��ʼ�������
            string fishType = "";
            if (fishData.nowFishList_FB.Count == 0)
                fishType = "δ������Ⱥ����";
            else fishType = "��ǰ����:" + fishContro.FishName(fishData.nowFishList_FB[0].fishKind);
            GameObject.Find("NowFishListType_FB_C").GetComponent<Text>().text = fishType;//��ǰ������ำֵ
            GameObject.Find("NowFishListIndex_FB_C").GetComponent<Text>().text = fishData.fishBoomListIndex.ToString();//��ǰ��Ⱥ�ڶ�ά��Ⱥ�����е�����
            GameObject.Find("NowFishListCount_FB_C").GetComponent<Text>().text = fishData.nowFishList_FB.Count.ToString();//��ǰ��Ⱥ���������

            if (fishData.nowFishList_FB.Count != 0)//�������
            {
                FishBoomInitital();//��ʼ����Ⱥ��ʾ
            }
        }
        /// <summary>
        /// ��Ⱥ��ʼ��
        /// </summary>
        private void FishBoomInitital()
        {
            //�������������ݣ����ɶ�Ӧ�����Ĭ��·��
            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                NewCatchFish_NormalFish_Temp fishTemp = CreateFishPrefab_FB(fishData.nowFishList_FB[i].fishKind, i);//�������·��
                lineList[i].positionCount = 0;
                for (int j = 0; j < fishData.nowFishList_FB[i].fishPath[1].fishPoint.Count; j++)
                {
                    lineList[i].SetPosition(j, new Vector3(fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].x, fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].y, 0));
                }
                SetFishPosAndRot();//������ĳ�ʼλ�úͽǶ�
            }
        }
        /// <summary>
        /// ��Ⱥ��������������ܵķ�������ʾ��Ⱥ�ƶ���
        /// </summary>
        private void FB_TypeEdit_FishRun_OnClick()
        {
            //��ȫ�ж��Ƿ�����
            if (fishData.nowFishList_FB.Count == 0) { Debug.Log("��ǰ��Ⱥû���㣡����"); return; }

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
        /// ��Ⱥ�����������ر��¼�
        /// </summary>
        private void FB_TypeEdit_Quit_OnClick()
        {
            Quit_FishBoom();//�ر����б༭��������
        }
        /// <summary>
        /// ��Ⱥ�����������Ӧ�ð�ť�¼�
        /// </summary>
        private void FB_TypeEdit_Apply_OnClick()
        {
            //��ȡ��ǰ��������
            InputField fishType = GameObject.Find("FishType_Inpt_C").GetComponent<InputField>();
            InputField fishCount = GameObject.Find("FishNum_Inpt_C").GetComponent<InputField>();
            if (fishType.text == "" || fishCount.text == "") { Debug.Log("�������Ϊ�գ�����");return; }
            fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Clear();//���֮ǰ����Ⱥ����
            //ɾ����ǰ�����е����������������·��line
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

            //�������������ݣ����ɶ�Ӧ�����Ĭ��·��
            for (int i = 0; i < int.Parse(fishCount.text); i++)
            {
                NewCatchFish_NormalFish_Temp fishTemp = CreateFishPrefab_FB(int.Parse(fishType.text), i);//�������·��
                lineList[i].positionCount = 0;//����line��
                //�����ɵ����·����ӵ��ֵ�
                Fish fish = new Fish();//��Ķ���
                fish.fishId = 0;fish.fishKind = byte.Parse(fishType.text);

                FishPath fishPath = new FishPath(); fishPath.pathId = 1; fishPath.pathName = "·��1";//���·��

                fishPath.fishPoint = new List<FishVecter>();//��·�ߵĵ�
                fishPath.fishPoint.Add(new FishVecter(-80, 375, 100, (((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f)));
                fishPath.fishPoint.Add(new FishVecter(1450, 375, 0, 0));
                fishPath.costTime = ((new Vector2(1450, 375) - new Vector2(-80, 375)).magnitude) / 100f;
                fish.fishPath = new Dictionary<int, FishPath>();fish.fishPath.Add(1, fishPath);
                fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Add(fish);

                //���µĵ���ӵ�line����ʾ������
                lineList[i].SetPosition(0, new Vector3(-80 + 302, 375 + 375, 0));
                lineList[i].SetPosition(1, new Vector3(1450 + 302, 375 + 375, 0));
            }

            //��ʼ�����ý���
            GameObject.Find("NowFishListType_FB_C").GetComponent<Text>().text = ((ObjectType)(1350 + int.Parse(fishType.text))).ToString();//��ǰ������ำֵ
            GameObject.Find("NowFishListCount_FB_C").GetComponent<Text>().text = fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Count.ToString();
            fishType.text = ""; fishCount.text = "";//��ʼ�������
        }
        /// <summary>
        /// ��Ⱥ�����������༭��ť�¼�
        /// </summary>
        private void FB_TypeEdit_Edit_OnClick()
        {
            if (fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex].Count == 0) { Debug.Log("����Ⱥû���㣡����");return; }
            //��ת����һ����༭����
            FB_TypeEditWidget.SetActive(false);
            //����һ�����·�߸�ֵ������Ⱥ�е�������
            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                if (i == 0) continue;
                fishData.nowFishList_FB[i].fishPath.Add(1, fishData.nowFishList_FB[0].fishPath[1]);
            }
            FB_FirstFishEdit_EditShow();//���õ�һ����༭����ʼ������
        }
        #endregion

        #region ��Ⱥ��һ����༭���ķ���
        /// <summary>
        /// ��һ����༭����ʼ������
        /// </summary>
        private void FB_FirstFishEdit_EditShow()
        {
            ClearAllLineAndFish();//���֮ǰ���������������
            FB_FirstFishEditWidget.SetActive(true);//�򿪱����
            //��ʼ�������ʾ
            isLine_FB = false; GameObject.Find("FirstFish_StartLine_C").GetComponentInChildren<Text>().text = "��ʼ����";
            GameObject.Find("FishTypeTXT_FirstFish_C").GetComponent<Text>().text = fishContro.FishName(fishData.nowFishList_FB[0].fishKind);
            GameObject.Find("FishTypeTXT_FishListIndex_C").GetComponent<Text>().text = fishData.fishBoomListIndex.ToString();
            FB_FirstFishEdit_FishPathCreate();//���·�ߵ����ɷ����������ɶ�Ӧ�ĵ�Ԥ����
        }
        /// <summary>
        /// ��ʼ���߰�ť
        /// </summary>
        private void FB_FirstFishEdit_StartLine_OnClick()
        {
            Text lineText = GameObject.Find("FirstFish_StartLine_C").GetComponentInChildren<Text>();
            if (!isLine_FB)
            {
                isLine_FB = true;
                lineText.text = "ֹͣ����";
                lineCount_FB = lineList[0].positionCount;
                //���������Ѵ��ڵĵ�����б�
                lineV3List = new List<Vector3>(); realLineV3 = new List<Vector3>();
                Debug.Log("��ǰ������" + lineCount_FB);
                for (int i = 0; i < lineCount_FB; i++)
                {
                    lineV3List.Add(lineList[0].GetPosition(i));
                    realLineV3.Add(new Vector3(lineList[0].GetPosition(i).x - 302, lineList[0].GetPosition(i).y - 375, 0));
                }
            }
            else
            {
                isLine_FB = false;
                lineText.text = "��ʼ����";
            }
        }
        /// <summary>
        /// ��һ��
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
        /// ������е�
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
        /// Ӧ��
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
                    Debug.Log("�ٶȲ���Ϊ0������");
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
            EricDebug.Debu.LogB("��ʱ�䣺" + fishData.nowFishList_FB[0].fishPath[1].costTime);
            Debug.Log("Ӧ�óɹ�������");

            fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex][0].fishPath[1] = fishData.nowFishList_FB[0].fishPath[1];
            Debug.Log(fishData.nowFishList_FB[0].fishPath[1].fishPoint[0].x);
            Debug.Log("�ɹ���ӵ��ֵ䣻��ǰ��Ⱥ�������ǣ�" + fishData.fishBoomListIndex + "����ǰ·����key�ǣ�" + fishData.fishBoomSceneIndex);
        }        
        /// <summary>
        /// ����
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
        /// �뿪
        /// </summary>
        private void FB_FirstFishEdit_Quit_OnClick()
        {
            //�ر�һ����ı༭���
            FB_FirstFishEditWidget.SetActive(false);
            //������������༭���
            FB_TypeEdit_BasicMessage_Show();//������Ϣ����ʼ��
        }
        /// <summary>
        /// �༭�������·����Ϣ
        /// </summary>
        private void FB_FirstFishEdit_OtherFishEdit_OnClick()
        {
            //�رմ˽��棬����ת��������ı༭����
            Quit_FishBoom();//�ر����е���
            FB_OtherFishEdit_Initital();//������༭�����ʼ��
        }

        #region ���༭�����߼�����
        /// <summary>
        /// ���·�����ɷ���
        /// </summary>
        private void FB_FirstFishEdit_FishPathCreate()
        {
            fishData.nowFish_FB = CreateFishPrefab_FB(fishData.nowFishList_FB[0].fishKind, 0);//�����㣬�Ͷ�Ӧ��·�����
            //�����·�ߵ���ӵ���Ӧ��line�����
            lineList[0].positionCount = 0;//����line�ĵ�
            //����line��
            for (int i = 0; i < fishData.nowFishList_FB[0].fishPath[1].fishPoint.Count; i++)
            {
                lineList[0].SetPosition(i, new Vector3(fishContro.RealCoordToScenesCoord(fishData.nowFishList_FB[0].fishPath[1]).fishPoint[0].x, fishContro.RealCoordToScenesCoord(fishData.nowFishList_FB[0].fishPath[1]).fishPoint[0].y, 0));
            }

            List<FishVecter> tempFish = fishData.nowFishList_FB[0].fishPath[1].fishPoint;//���ݸ�·�ߵĵ����б�ĳ������ɶ�Ӧ������Ԥ����
            //ɾ��֮ǰ��Ԥ����
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
                //��������Ϣ����Ԥ�������ʾ����
                o.transform.GetChild(4).GetComponent<Text>().text = i.ToString();//��ʾ��ǰ�������
                o.transform.GetChild(0).GetComponent<InputField>().text = tempFish[i].x.ToString();
                o.transform.GetChild(1).GetComponent<InputField>().text = tempFish[i].y.ToString();
                o.transform.GetChild(2).GetComponent<InputField>().text = tempFish[i].speed.ToString();
                o.transform.GetChild(3).GetComponent<InputField>().text = tempFish[i].time.ToString();
            }
            SetFishPosAndRot();//������ĳ�ʼ����ͽǶ�
        }
        #endregion

        #endregion

        #region ��Ⱥ������ı༭���ķ���
        /// <summary>
        /// ������༭�����ʼ������
        /// </summary>
        private void FB_OtherFishEdit_Initital()
        {
            ClearAllLineAndFish();//���֮ǰ���������������
            FB_OtherFishEditWidget.SetActive(true);//�򿪱�����

            GameObject.Find("NowFishListType_FB_C").GetComponent<Text>().text = fishContro.FishName(fishData.nowFishList_FB[0].fishKind);//��ǰ������ำֵ
            GameObject.Find("NowFishListIndex_FB_C").GetComponent<Text>().text = "��ǰ��Ⱥ����:" + fishData.fishBoomListIndex;//��ǰ��Ⱥ�ڶ�ά��Ⱥ�����е�����

            FishBoomInitital();//��ʾ��ǰ��Ⱥ���е����line
            //����û����İ�ťԤ����
            for (int i = 0; i < fishData.nowFishList_FB.Count; i++)
            {
                GameObject go = Resources.Load<GameObject>("Prefabs/Panels/TestTools/HuangYu_C");
                GameObject o = Instantiate(go, GameObject.Find("OtherFishList_FB_C").transform);
                // 1.��ȡ���ñ��е����ݣ����������ڰ�ť����ʾ��Ӧ������,���󶨷���
                object m = i;
                o.GetComponent<Button>().onClick.AddListener(delegate () { this.FB_OtherFishEdit_FishBtn_OnClick(m); });
                o.transform.GetComponentInChildren<Text>().text = fishContro.FishName(fishData.nowFishList_FB[i].fishKind);
            }
            //ɾ��֮ǰ��·�ߣ��������µ�·��
            for (int i = 0; i < lineList.Count; i++)
            {
                lineList[i].positionCount = 0;
                for (int j = 0; j < fishData.nowFishList_FB[i].fishPath[1].fishPoint.Count; j++)
                {
                    Vector3 v3 = new Vector3(fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].x, fishData.nowFishList_FB[i].fishPath[1].fishPoint[j].y,0);
                    lineList[i].SetPosition(j, v3);
                }
            }
            SetFishPosAndRot();//������ĳ�ʼ����ͽǶ�
        }
        /// <summary>
        /// Ӧ��
        /// </summary>
        private void FB_OtherFishEdit_Apply_OnClick()
        {
            //�������������������ֵ���
            fishBoomDic[fishData.fishBoomSceneIndex][fishData.fishBoomListIndex][fishData.FB_fishIndex].fishPath[1].fishPoint = fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint;
        }
        /// <summary>
        /// ƽ��X�����������¼�
        /// </summary>
        /// <param name="str"></param>
        private void FB_OtherFishEdit_TranslationX_InputField(string str)
        {
            Text fishIndex = GameObject.Find("OtherFish_NowFishIndex_C").GetComponent<Text>();
            if (fishIndex.text == "" || fishIndex.text == "0") return;
            if (str == "") return;
            float result = 0;
            if (!float.TryParse(str, out result)) { Debug.Log("���������֣�����");return; };
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
            fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint = ScenesCoordToRealCoord(new List<Vector3>(temp));//���µ����긳ֵ����ʱ����
            SetFishPosAndRot();//������ĳ�ʼλ�úͽǶ�
        }
        /// <summary>
        /// ƽ��Y�����������¼�
        /// </summary>
        /// <param name="str"></param>
        private void FB_OtherFishEdit_TranslationY_InputField(string str)
        {
            Text fishIndex = GameObject.Find("OtherFish_NowFishIndex_C").GetComponent<Text>();
            if (fishIndex.text == "" || fishIndex.text == "0") return;
            if (str == "") return;
            float result = 0;
            if (!float.TryParse(str, out result)) { Debug.Log("���������֣�����"); return; };
            result = float.Parse(str);

            Vector3[] temp = new Vector3[lineList[fishData.FB_fishIndex].positionCount];
            lineList[fishData.FB_fishIndex].GetPositions(temp);
            lineList[fishData.FB_fishIndex].positionCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].y += result;
            }
            lineList[fishData.FB_fishIndex].SetPositions(temp);
            fishData.nowFishList_FB[fishData.FB_fishIndex].fishPath[1].fishPoint = ScenesCoordToRealCoord(new List<Vector3>(temp));//���µ����긳ֵ����ʱ����
            SetFishPosAndRot();//������ĳ�ʼλ�úͽǶ�
        }
        /// <summary>
        /// ��ת���������¼�
        /// </summary>
        /// <param name="str"></param>
        private void FB_OtherFishEdit_Rotate_InputField(string str)
        {
            Text fishIndex = GameObject.Find("OtherFish_NowFishIndex_C").GetComponent<Text>();
            if (fishIndex.text == "" || fishIndex.text == "0") return;
            if (str == "") return;
            float result = 0;
            if (!float.TryParse(str, out result)) { Debug.Log("���������֣�����"); return; };
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

            SetFishPosAndRot();//������ĳ�ʼλ�úͽǶ�
        }
        /// <summary>
        /// ������༭����رշ���
        /// </summary>
        private void FB_OtherFishEdit_Quit_OnClick()
        {
            Quit_FishBoom();//�ر����е���
            FB_FirstFishEdit_EditShow();//��һ��������ʼ������
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
            fishData.FB_fishIndex = (int)_index;//��ǰѡ����������
        }
        #endregion

        #region ������ķ���
        private NewCatchFish_NormalFish_Temp CreateFishPrefab_FB(int _fishType, int _nowFishIndex)
        {
            //����һ����
            GameObject go = Resources.Load<GameObject>("Prefabs/Items/NewCatchFish_Temp/" + (ObjectType)(1350 + _fishType - 1));//ObjectPool.Instance.GetAGameObject<NewCatchFish_NormalFish_Temp>((ObjectType)1350 + fishData.nowFishKindIndex, GameObject.Find("BG_C").transform);
            GameObject o = Instantiate(go, GameObject.Find("BG_C").transform);
            fishData.fishObjList.Add(o);

            LineRenderer lineTemp = GameObject.Find("BG_C").AddComponent<LineRenderer>();//���һ����Ӧ��line����
            lineList.Add(lineTemp);//��ӵ�line�б�

            ////�����ǰ�����ߣ���ʼ�����������
            //if (lineList[_nowFishIndex].positionCount != 0) o.transform.position = lineList[_nowFishIndex].GetPosition(0);

            NewCatchFish_NormalFish_Temp tempFish = o.GetComponent<NewCatchFish_NormalFish_Temp>();

            return tempFish;
        }
        #endregion

        /// <summary>
        /// ������е�line�����Ԥ����
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
        /// ������ĳ�ʼλ�úͽǶ�
        /// </summary>
        private void SetFishPosAndRot()
        {
            for (int i = 0; i < fishData.fishObjList.Count; i++)
            {
                if (lineList[i].positionCount >= 1)
                    fishData.fishObjList[i].transform.position = lineList[i].GetPosition(0);//������ĳ�ʼλ��
                //������ĳ�ʼ�Ƕ�
                if (lineList[i].positionCount >= 2)
                {
                    fishData.fishObjList[i].transform.localEulerAngles = new Vector3(0, 0, 0);//���ȽǶȹ���
                    fishData.fishObjList[i].transform.localEulerAngles = new Vector3(0, 0, fishContro.GetAngle(lineList[i].GetPosition(0), lineList[i].GetPosition(1)));//���ȽǶȹ���
                }
            }
        }
        #endregion



        #region ���йرյ�������
        private void PathDown_FishBoom()//��ǰ����������Ⱥ�����ķ���
        {
            FB_AllFishListWidget.SetActive(false); Quit_FishBoom();
        }
        public void Quit_FishBoom()//�رղ˵���
        {
            if (clockIE != null) StopCoroutine(clockIE);//�رռ�ʱЭ��
            FB_TypeEditWidget.SetActive(false); FB_FirstFishEditWidget.SetActive(false);
            FB_OtherFishEditWidget.SetActive(false); //FB_ShowFishEditWidget.SetActive(false);
            fishData.isLine = false;
            lineText.text = "��ʼ����";
        }
        #endregion






        #region ��ť����¼��󶨷���
        /// <summary>
        /// ��ť����¼��󶨷���
        /// </summary>
        private void InitialClickEvent()
        {
            #region ������
            GameObject.Find("AddOneFishBtn_C").GetComponent<Button>().onClick.AddListener(AddOneFishMessage_OnClick);//���һ����

            GameObject.Find("DestroyOneFishBtn_C").GetComponent<Button>().onClick.AddListener(DestroyOneFishMessage_OnClick);//ɾ��һ����

            GameObject.Find("OneFishBtn_C").GetComponent<Button>().onClick.AddListener(SelectOneFishEdit_OnClick);//������༭���

            GameObject.Find("FishBoomBtn_C").GetComponent<Button>().onClick.AddListener(SelectFishBoomEdit_OnClick);//��Ⱥ�༭���

            GameObject.Find("PathDown_C").GetComponent<Button>().onClick.AddListener(PathDown_OneFish);//�ر�·�ߵ���

            GameObject.Find("Quit_C").GetComponent<Button>().onClick.AddListener(Quit_OneFish);//�رձ༭����

            GameObject.Find("Line_C").GetComponent<Button>().onClick.AddListener(StartLine_OnClick);//��ʼ����

            GameObject.Find("Clear_C").GetComponent<Button>().onClick.AddListener(ClearLine);//��������е����е����

            GameObject.Find("CreateMap_C").GetComponent<Button>().onClick.AddListener(CreateConfigEvent);//�������ñ�

            GameObject.Find("LastStep_C").GetComponent<Button>().onClick.AddListener(LastStepLine_OnClick);//������һ��

            GameObject.Find("RunBtn_C").GetComponent<Button>().onClick.AddListener(FishMove);//��������е����е����

            GameObject.Find("ApplyLine_C").GetComponent<Button>().onClick.AddListener(ReadConfigToList);//Ӧ��

            GameObject.Find("ToRun_C").GetComponent<Button>().onClick.AddListener(ToRun_OnClick);//����

            GameObject.Find("AddInDic_C").GetComponent<Button>().onClick.AddListener(AddThisLineToDic_OnClick);//���·�ߵ��ֵ�

            GameObject.Find("AddOneLine_C").GetComponent<Button>().onClick.AddListener(AddOneLine_OnClick);//����µ�·��

            GameObject.Find("DestroyOneLine_C").GetComponent<Button>().onClick.AddListener(DestroyOneLine_OnClick);//��������е����е����

            #endregion

            #region
            //*********-----��Ⱥ���-----*********//
            GameObject.Find("FishBoomPathDown_C").GetComponent<Button>().onClick.AddListener(PathDown_FishBoom);//�ر���Ⱥ��·�ߵ���

            GameObject.Find("FishBoomEditQuit_C").GetComponent<Button>().onClick.AddListener(Quit_FishBoom);//�ر���Ⱥ�ı༭����

            GameObject.Find("AddOneSceneBtn_C").GetComponent<Button>().onClick.AddListener(AddFishBoomScene_OnClick);//���һ������

            GameObject.Find("DestroyOneSceneBtn_C").GetComponent<Button>().onClick.AddListener(DestroyFishScene_OnClick);//ɾ��һ������

            GameObject.Find("AddOneFishList_C").GetComponent<Button>().onClick.AddListener(AddFishListToScene_OnClick);//���һ����Ⱥ

            GameObject.Find("DestroyOneFishList_C").GetComponent<Button>().onClick.AddListener(DestroyFishListScene_OnClick);//ɾ��һ����Ⱥ

            GameObject.Find("FishBoomPathDown_C").GetComponent<Button>().onClick.AddListener(Quit_FB_FishBoomList);//�˳���Ⱥ����

            GameObject.Find("FishType_FishRunBtn_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_FishRun_OnClick);//��Ⱥ�����������

            GameObject.Find("FishType_SetBtn_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_Apply_OnClick);//��Ⱥ�������Ӧ��

            GameObject.Find("FishType_EditBtn_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_Edit_OnClick);//��Ⱥ�������༭

            GameObject.Find("FishBoomEditQuit_C").GetComponent<Button>().onClick.AddListener(FB_TypeEdit_Quit_OnClick);//��Ⱥ�������ر�

            GameObject.Find("FirstFish_StartLine_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_StartLine_OnClick);//��һ������忪ʼ����
            GameObject.Find("FirstFish_LastLine_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_LastStep_OnClick);//��һ���������һ��
            GameObject.Find("FirstFish_ClearAll_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_ClearAllLine_OnClick);//��һ����������
            GameObject.Find("FirstFish_Apply_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_Apply_OnClick);//��һ�������Ӧ��
            GameObject.Find("FirstFish_Run_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_FishRun_OnClick);//��һ�����������
            GameObject.Find("FirstFish_OtherFishEdit_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_OtherFishEdit_OnClick);//��һ�������༭
            GameObject.Find("FirstFishEditQuit_FB_C").GetComponent<Button>().onClick.AddListener(FB_FirstFishEdit_Quit_OnClick);//��һ�������ر�

            GameObject.Find("OtherFishEditQuit_FB_C").GetComponent<Button>().onClick.AddListener(FB_OtherFishEdit_Quit_OnClick);//���������ر�
            GameObject.Find("OtherFish_SetBtn_C").GetComponent<Button>().onClick.AddListener(FB_OtherFishEdit_Apply_OnClick);//���������Ӧ��
            #endregion
        }
        #endregion

        void Update()
        {
            LineController_OnClick();//�������
        }

        #region �������ñ������㣩
        /// <summary>
        /// �������ñ�
        /// </summary>
        private void CreateConfigEvent()
        {
            #region
            ////�㳱����
            //byte sceneIndex = byte.Parse(GetInputFieldText("SceneIndex_C"));
            ////·����ʾ++
            //GetInputField("Path_C").text = (path + 1).ToString();
            //Debug.Log(GetInputField("Path_C").text);
            //string[] tempPosition = GetInputFieldText("OriginPoint_C").Split(' ');
            //string[] tempDirection = GetInputFieldText("OriginDirection_C").Split(' ');
            ////��ȡλ�÷���
            //float[] position = new float[] { float.Parse(tempPosition[0]), float.Parse(tempPosition[1]) };
            //float[] direction = new float[] { float.Parse(tempDirection[0]), float.Parse(tempDirection[1]) };
            ////�ٶȽ��ٶ�
            //float speed = float.Parse(GetInputFieldText("Speed_C"));
            //float rotate = float.Parse(GetInputFieldText("RotateRate_C"));


            ////��ȡ����
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
            if (fishData.isFishBoom)//��Ⱥ
            {
                //д�����ñ�
                if (File.Exists(fishData.fishBoomConfigPath))
                {
                    File.Delete(fishData.fishBoomConfigPath);
                }
                //�������ñ�
                File.Create(fishData.fishBoomConfigPath).Dispose();
                File.WriteAllText(fishData.fishBoomConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(fishBoomDic));
                Debug.Log("���ñ����ɳɹ���");
            }
            else//������
            {
                //д�����ñ�
                if (File.Exists(fishData.oneFishConfigPath))
                {
                    File.Delete(fishData.oneFishConfigPath);
                }
                //�������ñ�
                File.Create(fishData.oneFishConfigPath).Dispose();
                File.WriteAllText(fishData.oneFishConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(oneFishDic));
                Debug.Log("���ñ����ɳɹ���");
            }
        }
        #endregion
    }

    /// <summary>
    /// ���෽��������
    /// </summary>
    public class FishPathController
    {
        public YuChao_ConfigPanel yuChaoPanel;
        public FishPathController(YuChao_ConfigPanel _yuChaoPanel)
        {
            yuChaoPanel = _yuChaoPanel;
        }



        #region �����ֵ丳��ʼֵ
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

        #region ��������ж�
        public string FishName(int _fishId)
        {
            string str = "";
            switch (_fishId)
            {
                case 1:
                    str = "����";
                    break;
                case 2:
                    str = "������";
                    break;
                case 3:
                    str = "������";
                    break;
                case 4:
                    str = "������";
                    break;
                case 5:
                    str = "С����";
                    break;
                case 6:
                    str = "����";
                    break;
                case 7:
                    str = "ʨ����";
                    break;
                case 8:
                    str = "��β��";
                    break;
                case 9:
                    str = "��";
                    break;
                case 10:
                    str = "����";
                    break;
                case 11:
                    str = "������";
                    break;
                case 12:
                    str = "����";
                    break;
                case 13:
                    str = "ħ����";
                    break;
                case 14:
                    str = "������";
                    break;
                case 15:
                    str = "��Ϻ";
                    break;
                case 16:
                    str = "����";
                    break;
                case 17:
                    str = "����";
                    break;
                case 18:
                    str = "�з";
                    break;
                case 19:
                    str = "����";
                    break;
                case 20:
                    str = "ˮĸ";
                    break;
                case 21:
                    str = "������";
                    break;
                case 22:
                    str = "���Ǿ�";
                    break;
                case 23:
                    str = "���";
                    break;
                case 24:
                    str = "ը����";
                    break;
                case 25:
                    str = "������";
                    break;
                case 26:
                    str = "�����";
                    break;
                case 27:
                    str = "˫���ӳ�";
                    break;
                case 28:
                    str = "����";
                    break;
                default:
                    break;
            }
            return str;
        }
        #endregion

        #region ��vector3����ת��ΪFishVector����
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

        #region 1.���洢������ת��Ϊ��Ļ��ʾ������ 2.����Ļ����ת��Ϊ�洢������
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

        #region ���һ�������б��е�time
        public FishPath Time_FishV3(FishPath _fishPath)
        {
            List<FishVecter> fishV3 = new List<FishVecter>();

            float times = 0;
            for (int i = 0; i < _fishPath.fishPoint.Count - 1; i++)
            {
                if(_fishPath.fishPoint[i].speed == 0)
                {
                    Debug.Log("��" + (i+1) + "�������ֵ�����");
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

        #region ��������֮��ǶȲ�ֵ
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
    /// ��·��������
    /// </summary>
    public class FishPathDataManager
    {
        public YuChao_ConfigPanel yuChaoPanel;
        public FishPathDataManager(YuChao_ConfigPanel _yuChaoPanel)
        { yuChaoPanel = _yuChaoPanel; }

        /// <summary>
        /// �Ƿ�����Ⱥ�༭���
        /// </summary>
        public bool isFishBoom;
        /// <summary>
        /// ��ǰ�����е���
        /// </summary>
        public NewCatchFish_NormalFish_Temp nowFish;
        /// <summary>
        /// ��ǰ�����е�·�ߣ��������ʱ��ֵ��
        /// </summary>
        public FishPath nowFishPath;

        /// <summary>
        /// һ����������
        /// </summary>
        public int fishNum;
        /// <summary>
        /// ��ǰ�������б��е�����
        /// </summary>
        public int nowFishKindIndex;
        /// <summary>
        /// ��ǰ·������
        /// </summary>
        public int nowPathIdIndex;
        /// <summary>
        /// �Ƿ�ʼ����
        /// </summary>
        public bool isLine;
        /// <summary>
        /// �Ƿ�ʼ�༭
        /// </summary>
        public bool isEdit;
        /// <summary>
        /// ��ʼ��ʱ
        /// </summary>
        public bool isTime;
        /// <summary>
        /// �����Ϣ���������ܣ�
        /// </summary>
        public NewCatchFish_FishInfo fishInfo;


        /// <summary>
        /// ��ǰѡ�����Ⱥ�б�ĳ�����
        /// </summary>
        public byte fishBoomSceneIndex;
        /// <summary>
        /// ��ǰѡ�����Ⱥ������
        /// </summary>
        public int fishBoomListIndex;
        /// <summary>
        /// ��ǰѡ����������
        /// </summary>
        public int FB_fishIndex;
        /// <summary>
        /// ��ǰ�����е���Ⱥ
        /// </summary>
        public List<Fish> nowFishList_FB;
        /// <summary>
        /// ��ǰ���������е���Ⱥ
        /// </summary>
        public List<List<Fish>> nowFishList_FB_Two;
        /// <summary>
        /// ��ǰѡ�����
        /// </summary>
        public NewCatchFish_NormalFish_Temp nowFish_FB;
        /// <summary>
        /// ��ǰ�����е���Ԥ����
        /// </summary>
        public List<GameObject> fishObjList;

        //���ñ�·��
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
        /// ���ID
        /// </summary>
        public int fishId;
        /// <summary>
        /// �������
        /// </summary>
        public byte fishKind;
        /// <summary>
        /// ���·������
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
        /// ·��ID
        /// </summary>
        public int pathId;
        /// <summary>
        /// ·�ߵ������ֵ䣺key�����꣬value���ٶȣ���һ���ٶ�Ϊ0��
        /// </summary>
        public List<FishVecter> fishPoint;
        /// <summary>
        /// ·����Ҫ��ʱ��
        /// </summary>
        public float costTime;

        /// <summary>
        /// ·�����ƣ���ʱ��
        /// </summary>
        public string pathName;

        //public FishPath()
        //{
        //    //pathId = 1;
        //    //pathName = "·��1";
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