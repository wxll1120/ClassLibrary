using System;
using System.Collections.Generic;
using System.Threading;

namespace ClassLibrary.WorkingProcess
{
    /// <summary>
    /// 工作进度基础类
    /// </summary>
    public class WorkProcessManager : IDisposable
    {
        #region 私有变量

        /// <summary>
        /// 开始工作时间
        /// </summary>
        private long startTime;

        /// <summary>
        /// 同步方式
        /// </summary>
        private WorkSyncType syncType;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkProcessManager()
        {
            Init();
        }

        static WorkProcessManager()
        {

        }

        #region 属性/公有变量

        /// <summary>
        /// 异步操作UI上下文
        /// </summary>
        public SynchronizationContext SyncContext { get; set; }

        /// <summary>
        /// 当前工作进度
        /// </summary>
        public int CurrentWork { get; set; }

        /// <summary>
        /// 总工作量
        /// </summary>
        public int TotalWork { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        /// 是否繁忙
        /// </summary>
        public bool IsBusy
        {
            get { return false; }
            set { IsBusy = value; }
        }

        /// <summary>
        /// 处理时间
        /// </summary>
        protected TimeSpan ProcessTime
        {
            get
            {
                return new TimeSpan(DateTime.Now.Ticks - startTime);
            }
        }


        protected object UserState
        {
            get { return Guid.NewGuid(); }
        }

        #endregion

        /// <summary>
        /// 初始化工作状态管理对象
        /// </summary>
        public void Init()
        {
            Init(WorkSyncType.Sync);
        }

        /// <summary>
        /// 初始化工作状态管理对象
        /// </summary>
        /// <param name="syncType">同步方式</param>
        public void Init(WorkSyncType syncType)
        {
            startTime = DateTime.Now.Ticks;
            CurrentWork = 0;
            TotalWork = 0;

            this.syncType = syncType;

            if (this.syncType.Equals(WorkSyncType.Async))
                SyncContext = SynchronizationContext.Current;
            else
                SyncContext = null;
        }

        /// <summary>
        /// 重置工作状态
        /// </summary>
        public void Reset()
        {
            startTime = DateTime.Now.Ticks;
            CurrentWork = 0;
            TotalWork = 0;
        }

        /// <summary>
        /// 取消工作
        /// </summary>
        public virtual void Cancel()
        {
            IsCancel = true;
        }

        /// <summary>
        /// 事件集合
        /// </summary>
        private EventHandlerList eventList = new EventHandlerList();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="d"></param>
        protected void RegisterEvent(object key,Delegate d)
        {
            eventList.AddHandler(key, d);
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="d"></param>
        protected void RemoveEvent(object key, Delegate d)
        {
            eventList.RemoveHandler(key, d);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <typeparam name="T">泛型事件参数</typeparam>
        /// <param name="key">事件标识</param>
        /// <param name="args"> 事件</param>
        public void InvokeEvent<T>(object key,T args)
        {
            if (SyncContext == null)
            {
                Delegate de = eventList[key];

            if (de != null)
                de.DynamicInvoke(this,args);
            }
            else
            {
                SyncState syncState = new SyncState();
                syncState.EventKey = key;
                syncState.Args = args;
                SyncContext.Send(new SendOrPostCallback(SendOrPostInvoke), syncState);
            }           
        }

        /// <summary>
        /// 异步调用事件回调方法
        /// </summary>
        /// <param name="state"></param>
        private void SendOrPostInvoke(object state)
        {
            SyncState syncState = state as SyncState;
            Delegate de = eventList[syncState.EventKey];
            de.DynamicInvoke(this,syncState.Args);
        }

        protected virtual void Dispose(bool disposing)
        {
            SyncContext = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }

    /// <summary>
    /// 异步调用事件回调方法参数
    /// </summary>
    public class SyncState
    {
        /// <summary>
        /// 事件标识
        /// </summary>
        public object EventKey { get; set; }

        /// <summary>
        /// 事件参数
        /// </summary>
        public object Args { get; set; }
    }
}