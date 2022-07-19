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

        public bool AddError(Exception ex, string fileName)
        {
            _errorRepository.Add(new Repository.Domain.TblError
            {
                Col = 0,
                FileName = fileName,
                InnerException = ex.InnerException != null ? ex.InnerException.Message ?? "" : "",
                Line = 0,
                Message = ex.Message ?? "",
                MethodName = "",
                StackTrace = ex.StackTrace ?? ""
            });
            return _errorRepository.SaveChange() > 0;
        }

        public bool AddError(string message, string fileName)
        {
            _errorRepository.Add(new Repository.Domain.TblError
            {
                Col=0,
                FileName=fileName,
                InnerException="",
                Line=0,
                Message=message,
                MethodName = "",
                StackTrace="",
            });
            return _errorRepository.SaveChange() > 0;
        }
    }
    public interface IErrorBusiness
    {
        bool AddError(Exception ex, string fileName);
        bool AddError(string message, string fileName);
    }
}
