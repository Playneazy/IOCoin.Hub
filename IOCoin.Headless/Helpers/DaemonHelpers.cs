using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Headless.Helpers
{
    public class DaemonHelpers
    {
        private string _DataDir { get; set; }
        public DaemonHelpers(string dataDir) => _DataDir = !dataDir.EndsWith("\\") ? dataDir + "\\" : dataDir;
        public bool isDataDirValid() => !Directory.Exists(_DataDir) ? false : true;
        public bool isDatabaseDirValid() => !Directory.Exists(_DataDir + "database") ? false : true;
        public bool isTXDatabaseDirValid() => !Directory.Exists(_DataDir + "txleveldb") ? false : true;
        public bool isWalletFileValid() => !File.Exists(_DataDir + "wallet.dat") ? false : true;
        public bool isPeersFileValid() => !File.Exists(_DataDir + "peers.dat") ? false : true;
        public bool isBlkFileValid() => !File.Exists(_DataDir + "blk0001.dat") ? false : true;
        public bool isAliasCacheFileValid() => !File.Exists(_DataDir + "aliascache.dat") ? false : true;
        public bool isDefaultDirStructureValid()
        {
            if (!isDataDirValid()) return false;
            if (!isDatabaseDirValid()) return false;
            if (!isTXDatabaseDirValid()) return false;

            return true;
        }
        public bool isDefaultFileStructureValid()
        {
            if (!isWalletFileValid()) return false;
            if (!isPeersFileValid()) return false;
            if (!isAliasCacheFileValid()) return false;
            if (!isBlkFileValid()) return false;

            return true;
        }

        public string FormatDirPath(bool EndSlash = true)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            { 
                if (_DataDir.EndsWith(@"\") && !EndSlash)
                {
                    return _DataDir.Substring(0, _DataDir.Length - 1);
                } else if (!_DataDir.EndsWith(@"\") & EndSlash)
                {
                    return _DataDir + @"\";
                }
            }

            return _DataDir;
        }

    }
}
