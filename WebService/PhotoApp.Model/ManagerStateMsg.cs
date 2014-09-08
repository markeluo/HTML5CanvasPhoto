using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoApp.Model
{
     public class ManagerStateMsg
    {
         public ManagerStateMsg(bool _bolstate, string _msg)
         {
             statevalue = _bolstate;
             msgInfo = _msg;
         }

        private bool statevalue;

        public bool Statevalue
        {
            get { return statevalue; }
            set { statevalue = value; }
        }
        private object returnValue;
         /// <summary>
         /// 返回值
         /// </summary>
        public object ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }

        private string msgInfo;

        public string MsgInfo
        {
            get { return msgInfo; }
            set { msgInfo = value; }
        }
    }
}
