using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Kits.ClientKit.Handlers.LoopSystems
{
    public static class EventTriggerHandler
    {
        /// <summary>
        /// 为 EventTrigger 添加事件
        /// </summary>
        public static void AddListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            List<EventTrigger.Entry> triggers = eventTrigger.triggers;
            int index = triggers.FindIndex(entry => entry.eventID == type);
            if (index < 0)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = type;
                entry.callback.AddListener(callback);
                triggers.Add(entry);
            }
            else
            {
                triggers[index].callback.AddListener(callback);
            }
        }


        /// <summary>
        /// 为 EventTrigger 移除事件
        /// </summary>
        public static void RemoveListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            List<EventTrigger.Entry> triggers = eventTrigger.triggers;
            int index = triggers.FindIndex(entry => entry.eventID == type);
            if (index >= 0)
            {
                triggers[index].callback.RemoveListener(callback);
            }
        }
    }
}