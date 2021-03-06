﻿using System;

namespace MHWDecorationsModifier.MyException
{
    public class InvalidKeyWordException : ApplicationException
    {
        private readonly string _error;


        //无参数构造函数
        public InvalidKeyWordException()
        {
        }

        //带一个字符串参数的构造函数，作用：当程序员用Exception类获取异常信息而非 MyException时把自定义异常信息传递过去
        public InvalidKeyWordException(string msg) : base(msg)
        {
            this._error = msg;
        }


        public string GetError()
        {
            return _error;
        }
    }
}