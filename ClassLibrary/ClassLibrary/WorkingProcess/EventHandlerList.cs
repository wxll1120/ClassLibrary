using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace ClassLibrary.WorkingProcess
{
    /// <summary>
    /// �¼��б�, ���̰߳�ȫ
    /// </summary>
    public class EventHandlerList : IDisposable
    {
        /// <summary>
        /// ���ڱ���"�¼���/ί��ֵ"�Ե�˽��ɢ�б� 
        /// </summary>
        private Dictionary<object, Delegate> eventList =
            new Dictionary<object, Delegate>();

        /// <summary>
        /// һ�����������û�ȡ�������봫����¼������ɢ�м��������ί��
        /// </summary>
        /// <param name="eventKey"></param>
        /// <returns></returns> 
        public virtual Delegate this[object eventKey]
        {
            //��������ڼ����У��򷵻�null 
            get
            {
                if (eventList.ContainsKey(eventKey))
                {
                    return eventList[eventKey];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                eventList[eventKey] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventKey"></param>
        public bool Contains(object eventKey)
        {
            return eventList.ContainsKey(eventKey);
        }

        /// <summary>
        /// ָ�����¼������ɢ��ۻ��Ӧ��ί�����������/��ȫһ��ί��ʵ��
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="handler"></param> 
        public virtual void AddHandler(object eventKey, Delegate handler)
        {
            if (eventList.ContainsKey(eventKey))
            {
                eventList[eventKey] = Delegate.Combine(eventList[eventKey], handler);
            }
            else
            {
                eventList[eventKey] = handler;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="handler"></param>
        public virtual void RemoveHandler(object eventKey, Delegate handler)
        {
            if (this.eventList.ContainsKey(eventKey))
            {
                eventList[eventKey] = Delegate.Remove(eventList[eventKey], handler);
            }
        }

        /// <summary>
        /// ��ָ�����¼�����ɢ�м��̶�Ӧ��ί�������ϴ����¼� 
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void DynamicInvoke(object eventKey, object sender, EventArgs e)
        {
            try
            {

                if (eventList.ContainsKey(eventKey))
                {
                    eventList[eventKey].DynamicInvoke(new object[] { sender, e });
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// ����һ���Դ����EventHandlerList������̰߳�ȫ�ķ�װ 
        /// </summary>
        /// <param name="eventList"></param>
        /// <returns></returns>
        public static EventHandlerList Synchronized(EventHandlerList eventList)
        {
            if (eventList == null)
            {
                throw new ArgumentNullException("eventList");
            }
            return new SyncEventHandlerList(eventList);
        }

        #region IDisposable ��Ա

        /// <summary>
        /// �ͷŶ�����ʹɢ�б�ռ�õ��ڴ���Դ����һ�������ռ��б����գ�
        /// �Ӷ���ֹ�����ռ������������� 
        /// </summary>
        public void Dispose()
        {
            eventList = null;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private class SyncEventHandlerList : EventHandlerList
        {
            /// <summary>
            /// 
            /// </summary>
            private EventHandlerList syncEventHandlerList = null;

            #region ���캯��

            private SyncEventHandlerList()
            {

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="eventList"></param>
            public SyncEventHandlerList(EventHandlerList eventList)
            {
                syncEventHandlerList = eventList;

                //�ͷŻ����е�ɢ�б����
                base.Dispose();
            }

            #endregion

            /// <summary>
            /// �̰߳�ȫ��������
            /// </summary>
            /// <param name="eventKey"></param>
            /// <returns></returns>
            public override Delegate this[object eventKey]
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return syncEventHandlerList[eventKey];
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    syncEventHandlerList[eventKey] = value;
                }
            }

            /// <summary>
            /// �̰߳�ȫ��AddHander����
            /// </summary>
            /// <param name="eventKey"></param>
            /// <param name="handler"></param> 
            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void AddHandler(object eventKey, Delegate handler)
            {
                syncEventHandlerList.AddHandler(eventKey, handler);
            }

            /// <summary>
            /// �̰߳�ȫ��RemoveHandler����
            /// </summary>
            /// <param name="eventKey"></param>
            /// <param name="handler"></param>
            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void RemoveHandler(object eventKey, Delegate handler)
            {
                syncEventHandlerList.RemoveHandler(eventKey, handler);
            }

            /// <summary>
            /// �̰߳�ȫ��Fire���� 
            /// �÷������ȴ�events ɢ�б����ҵ�һ��ί������
            /// Ȼ����ø�ί�������Ϸ�װ�����еĻص����� 
            /// </summary>
            /// <remarks>��Ϊɢ�б��п��ܰ����и��ָ����Ĳ�ͬ��ί�����͡��������ǲ������ڱ���������Ͱ�ȫ 
            /// * ��ί�е��á���ˣ�����ʹ����System.Delegate ��DynamicInvoke�����������ص������Ĳ��� 
            /// * ��ȫΪһ���������鴫�ݸ����� 
            /// * DynamicInvoke�������ڲ�����ص����������Ĳ��������Ǵ��ݵĲ��� 
            /// * �Ƿ�ƥ�䡣���ƥ�䣬�ص������������� 
            /// * 
            /// * ����DynamicInvoke�������׳�һ���쳣. </remarks>
            /// <param name="eventKey"></param>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public override void DynamicInvoke(object eventKey, object sender, EventArgs e)
            {
                syncEventHandlerList.DynamicInvoke(eventKey, sender, e);
            }
        }
    }
}
