using Microsoft.AspNetCore.Mvc.ModelBinding;
using MEDITRACK.COMMON.Entities.DTO;
using MEDITRACK.COMMON.Resource;
using MEDITRACK.COMMON;
using MySqlConnector;
using System.Diagnostics;

namespace MEDITRACK
{
    /// <summary>
    /// Class static gồm các hàm xử lý lỗi khi gọi API
    /// </summary>
    public static class HandleError
    {

        /// <summary>
        /// Sinh ra đối tượng lỗi trả về khi gặp exception
        /// </summary>
        /// <param name="exception">Đối tượng exception gặp phải</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        public static ErrorResult? GenerateExceptionResult(Exception ex)
        {
            return new ErrorResult(
                MISAErrorCode.Exception,
                ex.Message,
                ex.Data
                );
        }

        /// <summary>
        /// sinh ra đối tượng lỗi trả về khi lỗi server
        /// </summary>
        /// <returns>Đối tượng lỗi </returns>
        //public static ErrorResult? GenerateServerErrorResult()
        //{
        //    var errorResult = new ErrorResult(
        //        MISAErrorCode.ServerError,
        //        ErrorResource.ServerException,
        //        ErrorResource.ServerException);

        //    return errorResult;

        //}

        /// <summary>
        /// sinh ra đối tượng lỗi trả về khi không tìm thấy dữ liệu
        /// </summary>
        /// <returns>Đối tượng lỗi </returns>
        //public static ErrorResult? GenerateNotFoundErrorResult()
        //{
        //    var errorResult = new ErrorResult(
        //        MISAErrorCode.NotFound,
        //        ErrorResource.NotFound,
        //       );

        //    return errorResult;

        //}

        public static ErrorResult? GenerateValidateErrorResult()
        {
            ErrorService ex= new ErrorService();
            var errorResult = new ErrorResult(
                ex.ErrorCode,
                ex.ErrorMessage,
                ex.Errors
                );

            return errorResult;
        }

        public static ErrorResult? GenerateDuplicateErrorResult()
        {
            ErrorService ex = new ErrorService();
            var errorResult = new ErrorResult(
                ex.ErrorCode,
                ex.ErrorMessage,
                ex.Errors
                );

            return errorResult;
        }
    }
}
