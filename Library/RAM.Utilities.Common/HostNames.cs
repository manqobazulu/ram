using System;
using System.Net;
using System.Collections;

namespace RAM.Utilities.Common{
    /// <summary>
    /// Summary description for HostNames.
    /// </summary>
    public class HostNames
    {
        private static SortedList _CachedHostNames = new SortedList();
        public static SortedList CachedHostNames { get { return _CachedHostNames; } }

        public static string HostnameFromIPAddress(string ipAddress)
        {
            try
            {
                string hostName = string.Empty;
                //hostName = Dns.GetHostByAddress(ipAddress).HostName;
                hostName = Dns.GetHostEntry(ipAddress).HostName;
                if (hostName.Length == 0) hostName = ipAddress;
                return hostName;
            }
            catch (Exception)
            {
                return ipAddress;
            }
        }

        public static string CachedHostnameFromIPAddress(string ipAddress)
        {
            try
            {
                if (_CachedHostNames == null) _CachedHostNames = new SortedList();
                string hostName = string.Empty;
                bool found = false;
                for (int i = 0; i < _CachedHostNames.Count; i++)
                {
                    if (_CachedHostNames.GetKey(i).ToString() == ipAddress)
                    {
                        hostName = ((CachedHostName)_CachedHostNames.GetByIndex(i)).HostName;
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    hostName = HostnameFromIPAddress(ipAddress);
                    CachedHostName cachedHostName = new CachedHostName(ipAddress, hostName);
                    _CachedHostNames.Add(ipAddress, cachedHostName);
                }
                return hostName;
            }
            catch (Exception)
            {
                try
                {
                    //clear the cache and try again
                    _CachedHostNames = new SortedList();
                    string hostName = string.Empty;
                    hostName = HostnameFromIPAddress(ipAddress);
                    CachedHostName cachedHostName = new CachedHostName(ipAddress, hostName);
                    _CachedHostNames.Add(ipAddress, cachedHostName);
                    return hostName;
                }
                catch (Exception)
                {
                    //this should then never happen - review logs to make sure
                    return ipAddress;
                }
            }
        }
    }

    public class CachedHostName
    {
        private string _IPAddress = string.Empty;
        private string _HostName = string.Empty;
        private int _TTLSeconds = (3600 * 6);
        private DateTime _ExpiryDate = System.DateTime.Now;
        public string IPAddress { get { return _IPAddress; } set { _IPAddress = value; } }
        public string HostName
        {
            get
            {
                if (this.Expired == true) this.RefreshHostName();
                return _HostName;
            }
            set
            {
                _HostName = value;
            }
        }
        public int TTLSeconds { get { return _TTLSeconds; } set { _TTLSeconds = value; } }
        public DateTime ExpiryDate { get { return _ExpiryDate; } set { _ExpiryDate = value; } }
        public CachedHostName(string ipAddress, string hostName)
        {
            _IPAddress = ipAddress;
            _HostName = hostName;
            _ExpiryDate = System.DateTime.Now.AddSeconds(_TTLSeconds);
        }
        public CachedHostName(string ipAddress, string hostName, int tTLSeconds)
        {
            _IPAddress = ipAddress;
            _HostName = hostName;
            _TTLSeconds = tTLSeconds;
            _ExpiryDate = System.DateTime.Now.AddSeconds(_TTLSeconds);
        }
        public bool Expired
        {
            get
            {
                return (_ExpiryDate < System.DateTime.Now);
            }
        }
        public void RefreshHostName()
        {
            _HostName = HostNames.HostnameFromIPAddress(_IPAddress);
            _ExpiryDate = System.DateTime.Now.AddSeconds(_TTLSeconds);
        }

    }
}
