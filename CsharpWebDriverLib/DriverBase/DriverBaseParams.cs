﻿
using System;

namespace CsharpWebDriverLib
{
    public class DriverBaseParams
    {
        // значение времени (в сек) общих неявных ожиданий
        public int drvImplWaitTime { get; private set; }
        // значение времени (в сек) для явных ожиданий
        public int drvExplWaitTime { get; private set; }
        // константа времени (в сек) для максимального времени неявного ожидания
        public int drvMaxWaitTime { get; private set; }

        // Remote WinServer2019 with Docker "192.168.0.91"
        private const string remoteIpStr_WinServer2019 = "192.168.0.91";
        // Remote Ubuntu 20.4   with Docker "192.168.203.128"
        private const string remoteIpStr_Ubuntu_20_4 = "192.168.203.128";
        // Local Windows Ip  
        private const string localIpStr_Win = "192.168.0.101";

        public string localHostStr { get => "localhost"; } // LocalHost  
        public string localIpStr { get; private set; }     // Local Host Ip 
        public string remoteIpStr { get; private set; }   // Remote Host Ip 


        internal DriverBaseParams(int drvImplWaitTime = WebDriverExtensions.drvImplWaitTime,
                                int drvExplWaitTime = WebDriverExtensions.drvExplWaitTime,
                                int drvMaxWaitTime  = WebDriverExtensions.drvMaxWaitTime,
                                string localIpStr   = localIpStr_Win,
                                string remoteIpStr  = remoteIpStr_Ubuntu_20_4)
        {
            // значение времени (в сек) общих неявных ожиданий
            this.drvImplWaitTime = drvImplWaitTime;
            // значение времени (в сек) для явных ожиданий
            this.drvExplWaitTime = drvExplWaitTime;
            // значение времени (в сек) для максимального времени неявного ожидания
            this.drvMaxWaitTime = drvMaxWaitTime;
            // current Local Ip  
            this.localIpStr = localIpStr;
            // current Remote Ip
            this.remoteIpStr = remoteIpStr;
        }

        public static DriverBaseParams CreateDriverBaseParams()
            => new DriverBaseParams();

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime)
            => new DriverBaseParams(drvImplWaitTime);

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime, int drvExplWaitTime)
            => new DriverBaseParams(drvImplWaitTime, drvExplWaitTime);

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime, int drvExplWaitTime, int drvMaxWaitTime)
            => new DriverBaseParams(drvImplWaitTime, drvExplWaitTime, drvMaxWaitTime);

    }
}
