using PMB.Repository.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Business
{
    public class ErrorBusiness : IErrorBusiness
    {
        private readonly IErrorRepository _errorRepository;
        public ErrorBusiness(IErrorRepository errorRepository)
        {
            _errorRepository = errorRepository;
        }

        public bool AddError(Exception ex, string fileName, string methodName)
        {
            _errorRepository.Add(new Repository.Domain.TblError
            {
                Col = 0,
                FileName = fileName,
                InnerException = ex.InnerException != null ? ex.InnerException.Message ?? "" : "",
                Line = 0,
                Message = ex.Message ?? "",
                MethodName = methodName,
                StackTrace = ex.StackTrace ?? ""
            });
            return _errorRepository.SaveChange() > 0;
        }

        public bool AddError(string message, string fileName, string methodName)
        {
            _errorRepository.Add(new Repository.Domain.TblError
            {
                Col=0,
                FileName=fileName,
                InnerException="",
                Line=0,
                Message=message,
                MethodName = methodName,
                StackTrace="",
            });
            return _errorRepository.SaveChange() > 0;
        }
    }
    public interface IErrorBusiness
    {
        bool AddError(Exception ex, string fileName, string methodName);
        bool AddError(string message, string fileName, string methodName);
    }
}
