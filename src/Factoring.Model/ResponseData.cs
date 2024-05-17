using System;
using System.Collections.Generic;

namespace Factoring.Model
{
    public class ResponseData<T>
    {
        public ResponseData()
        {
        }

        public ResponseData(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ResponseData(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}