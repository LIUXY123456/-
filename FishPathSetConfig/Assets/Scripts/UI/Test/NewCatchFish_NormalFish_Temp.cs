using BPLaBaGame.FSM;
using Game_Buyu;
using UnityEngine;

namespace SubGame_NewCatchFish
{

    public class NewCatchFish_NormalFish_Temp : NewCatchFish_fishBase
    {
        /// <summary>
        /// �������¼�
        /// </summary>
        public void BeHitedEvent()
        {
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        public void FrozenEvent(float timer)
        {
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        public void DeadEvent()
        {
            Debug.LogError("�� ������");
            //�л�״̬
            fishfsm.ChangeState("StateDead");
        }
        /// <summary>
        /// ���������¼�
        /// </summary>
        public void EnterFishpondEvent()
        {
            OnHit += BeHitedEvent;
            OnFrozen += FrozenEvent;
            OnDead += DeadEvent;

        }
        /// <summary>
        /// �뿪�����¼�
        /// </summary>
        public void LeaveFishpondEvent()
        {
            OnHit -= BeHitedEvent;
            OnFrozen -= FrozenEvent;
            OnDead -= DeadEvent;
        }
        public override void InitFish(NewCatchFish_FishInfo fishInfo)
        {
            base.InitFish(fishInfo);
            fishInfo = fishInfo;

            //foreach (var item in NewCatChFish_Mgr.Instance.fishConfig)
            //{
            //    if (item.fishKind == fishInfo.FishKind)
            //    {
            //        foreach (var path in item.fishPath)
            //        {
            //            if (path.Key == fishInfo.PathId)
            //            {
            //                fishPath = path.Value;
            //            }
            //        }
            //    }
            //}
            if (fishPath == null)
            {
                Debug.LogError("�Ҳ������·��");
                return;
            }


            OnEnterFishpond += EnterFishpondEvent;
            OnLeavaFishpond += LeaveFishpondEvent;
            //transform.SetParent(NewCatChFish_Mgr.Instance.fishpond.transform);
            fishfsm = new FSM();
            fishfsm.AddState(new StateMove_Temp(), this);
            fishfsm.AddState(new StateFrozen_Temp(), this);
            fishfsm.AddState(new StateDead_Temp(), this);

            fishfsm.Init();
        }
    }


    public class StateMove_Temp : IState
    {
        public Object owner { get; set; }
        public IState previousState { get; set; }
        public object[] parameters { get; set; }
        NewCatchFish_fishBase fish;

        public YuChao_ConfigPanel yuChaoPanel;
        public void StateEnd()
        {

        }

        public void StateEnter()
        {
            Debug.LogError("����move״̬");
            fish = (owner as NewCatchFish_fishBase);
            fish.currentPathIndex = 0;
            fish.transform.position = new Vector3(fish.fishPath.fishPoint[0].x, fish.fishPath.fishPoint[0].y, 0);
        }

        public void StateUpdate()
        {
            //            Debug.LogError("move����");

            if (fish.currentPathIndex < fish.fishPath.fishPoint.Count - 1)
            {
                var targetPos = new Vector3(fish.fishPath.fishPoint[fish.currentPathIndex + 1].x,
                    fish.fishPath.fishPoint[fish.currentPathIndex + 1].y, 0);

                fish.transform.LookAt(targetPos);
                fish.transform.rotation *= Quaternion.Euler(new Vector3(0, -90, -90));
                //Quaternion.LookRotation
                // fish.transform.rotation =   Quaternion.Lerp(fish.transform.rotation, Quaternion.Euler(targetPos -fish.transform.position), .6f);
                fish.transform.position += fish.transform.up *
                 fish.fishPath.fishPoint[fish.currentPathIndex].speed * Time.deltaTime;
                if ((targetPos - fish.transform.position).magnitude <= 5)
                {
                    fish.currentPathIndex += 1;
                }
            }
            else
            {
                //ObjectPool.Instance.CollectAGameObject<NewCatchFish_NormalFish_Temp>(fish as NewCatchFish_NormalFish_Temp);//������
            }

            //������Ƿ�����Ϸ������
            //if (NewCatchFish_FIshpond.NewCatchFish_GameAreaBound.IsInArea(fish.transform.position))
            //{
            //    if (!NewCatChFish_Mgr.Instance.fishpond.fishesInFishpond.Contains(fish))
            //    {
            //        NewCatChFish_Mgr.Instance.fishpond.fishesInFishpond.Add(fish);
            //        NewCatChFish_Mgr.Instance.fishpond.fishesNotInFishpond.Remove(fish);
            //        fish.OnEnterFishpond();//���������¼�
            //    }
            //}
            //else
            //{
            //    if (NewCatChFish_Mgr.Instance.fishpond.fishesInFishpond.Contains(fish))
            //    {
            //        NewCatChFish_Mgr.Instance.fishpond.fishesInFishpond.Remove(fish);
            //        NewCatChFish_Mgr.Instance.fishpond.fishesNotInFishpond.Add(fish);
            //        fish.OnLeavaFishpond();//�뿪�����¼�
            //    }
            //}
        }

        public string Tostring()
        {
            return "StateMove";
        }
    }
    public class StateFrozen_Temp : IState
    {
        public Object owner { get; set; }
        public IState previousState { get; set; }
        public object[] parameters { get; set; }
        NewCatchFish_fishBase fish;
        public void StateEnd()
        {

        }

        public void StateEnter()
        {

        }

        public void StateUpdate()
        {

        }

        public string Tostring()
        {
            return "StateFrozen";
        }
    }

    public class StateDead_Temp : IState
    {
        public Object owner { get; set; }
        public IState previousState { get; set; }
        public object[] parameters { get; set; }
        NewCatchFish_fishBase fish;
        float timer;
        public void StateEnd()
        {

        }

        public void StateEnter()
        {
            fish = (owner as NewCatchFish_fishBase);
            var ani = fish.GetComponent<Animator>();
            if (ani != null)
            {
                ani.SetInteger("state", 1);
                //�л�����״̬
            }
            timer = 0f;
        }

        public void StateUpdate()
        {
            if (timer > 0.4f)
            {
                GameObject.Destroy(fish.gameObject);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        public string Tostring()
        {
            return "StateDead";
        }
    }
}