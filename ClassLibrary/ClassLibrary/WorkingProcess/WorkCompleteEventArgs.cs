using System;
using System.Collections.Generic;

namespace ClassLibrary.WorkingProcess
{
    /// <summary>
    /// 任务完成事件所需的参数
    /// </summary>
    public class WorkCompleteEventArgs : System.ComponentModel.RunWorkerCompletedEventArgs
    {
        /// <summary>
        /// 任务完成事件参数构造函数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <param name="cancelled"></param>
        /// <param name="completedTime"></param>
        public WorkCompleteEventArgs(object result, Exception error = null, bool cancelled = false,
            TimeSpan completedTime = new TimeSpan())
            : base(result, error, cancelled)
        {
            this.processTime = completedTime;
        }

        /// <summary>
        /// 完成任务耗费时间
        /// </summary>
        private TimeSpan processTime;

        /// <summary>
        /// 完成任务耗费时间
        /// </summary>
        public TimeSpan ProcessTime
        {
            get { return processTime; }
            set { processTime = value; }
        }

        private object data;

        /// <summary>
        /// 事件状态返回值
        /// </summary>
        public object Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
    }
}
