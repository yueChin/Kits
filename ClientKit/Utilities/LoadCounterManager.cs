using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kits.ClientKit.Utilities
{
    /// <summary>
    /// 事件计数器
    /// </summary>
    public class LoadCounterManager
    {
        static LoadCounterManager instance = null;
        Dictionary<int,LoadCounter> loadCounterDic = new Dictionary<int, LoadCounter> ();
        public static LoadCounterManager GetInstance()
        {
            if (instance == null)
                instance = new LoadCounterManager();

            return instance;
        }

        public int StartLoadCounter( int maxTimes, UnityAction completeCall, UnityAction<int, float> singleCall = null)
        {
            if (maxTimes <= 0)
                return -1;

            int uid = 0;

            loadCounterDic[uid] = new LoadCounter(uid, maxTimes, completeCall, singleCall);
            return uid;
        }

        /// <summary>
        /// 计数++
        /// </summary>
        /// <param name="uid"></param>
        public void OnLoadOver(int uid)
        {
            if (!loadCounterDic.ContainsKey(uid))
            {
                Debug.LogError("计数器不存在 UID:" + uid );
                return;
            }
            loadCounterDic[uid].OnLoadOver();
        }

        public void Remove(int uid,bool isRemoveForComplete = false)
        {
            if (isRemoveForComplete)
            {
                if(loadCounterDic.ContainsKey(uid))
                    while (loadCounterDic.ContainsKey(uid))
                    {
                        loadCounterDic[uid].OnLoadOver();
                    }
            }
            else
            {
                loadCounterDic.Remove(uid);
            }
        }

        public bool IsUidContain(int uid)
        {
            return loadCounterDic.ContainsKey(uid);
        }
    }

    public class LoadCounter
    {
        public int UID;
        public int MaxTimes = 0;
        public int CurTimes = 0;
        UnityAction CompleteCall;
        UnityAction<int, float> SingleCall;

        public LoadCounter(int uid, int maxTimes, UnityAction completeCall, UnityAction<int, float> singleCall = null)
        {
            UID = uid;
            MaxTimes = maxTimes;
            CurTimes = 0;
            CompleteCall = completeCall;
            SingleCall = singleCall;
        }

        public void OnLoadOver()
        {
            CurTimes++;
            if (SingleCall != null)
            {
                SingleCall(CurTimes, (float)CurTimes / MaxTimes);
            }

            if (CurTimes == MaxTimes)
            {
                CompleteCall();
                LoadCounterManager.GetInstance().Remove(UID);
            }
            else if (CurTimes > MaxTimes)
            {
                Debug.LogError("计数器溢出 uid:" + UID + "溢出表现：" + CurTimes + "/" + MaxTimes );
            }
        }

    }
}