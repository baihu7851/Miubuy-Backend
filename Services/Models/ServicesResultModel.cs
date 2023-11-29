using System;

namespace Services.Models
{
    /// <summary>
    /// 服務層回傳基本物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServicesResultModel<T> where T : class
    {
        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public int Code { get; private set; } = -1;

        /// <summary>
        /// 狀況訊息
        /// </summary>
        public string Message { get; private set; } = "尚未設定回傳狀態";

        /// <summary>
        /// 例外狀況
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 資料內容
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSusses { get; private set; }

        public void SetSuccess(T data, int code = 0)
        {
            IsSusses = true;
            Code = code;
            Data = data;
        }

        public void SetError(string message, int code)
        {
            IsSusses = false;
            Code = code;
            Message = message;
        }

        public void SetException(Exception exception, int code = -1)
        {
            IsSusses = false;
            Code = code;
            Message = exception.Message;
            Exception = exception;
        }
    }
}