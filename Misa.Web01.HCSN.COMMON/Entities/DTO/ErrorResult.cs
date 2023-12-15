
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON
{
    public class ErrorResult
    {

         /// <summary>
           /// định danh mã lỗi nội bộ
           /// </summary>
        public MISAErrorCode ErrorCode { get; set; } = MISAErrorCode.Exception;

        /// <summary>
        /// thông báo lỗi 
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// thông tin chi tiết hơn về lỗi
        /// </summary>
        public IDictionary? Data { get; set; }

        /// <summary>
        /// mã tra cứu lỗi 
        /// </summary>
        //public string? TraceId { get; set; }
        #region Constructor

        //public ErrorResult(MISAErrorCode notFound, string serverException)
        //{

        //}
        public ErrorResult()
        {
        }

        public ErrorResult(MISAErrorCode errorCode, string? mesage, IDictionary? data)
        {
            ErrorCode = errorCode;
            Message = mesage;
            Data = data;
        }

        #endregion
    }

}
