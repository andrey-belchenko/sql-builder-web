using System;
using System.Data;
using System.Linq;
#if NETFRAMEWORK
using System.ServiceModel;
#endif
//using System.Windows.Forms;
using System.Xml.Linq;
using infoenergo.core.Data;
using infoenergo.sys;
using sql.builder.DataApi;
using sql.builder.WinForms;
//using infoenergo.framework.Extensions.Oracle;

namespace sql.builder.XmlHelpers
{
    internal static class WCFHelper
    {
        public static bool IsServer { get; private set; }
        public static bool IsClient { get; private set; }

        static int _port = 8080;
#if NETFRAMEWORK
        static ServiceHost _host;
#else
        static object _host; // ServiceHost not available in .NET 8 WCF
#endif
        internal static IServerData ServerData { get; private set; }
        internal static IWCFServer Server { get; private set; }

        internal static string ServiceName { get; private set; }

        internal static void StartServer(IWCFServer server)
        {
#if NETFRAMEWORK
            if (_host != null) return;

            Server = server;

            ServiceName = Guid.NewGuid().ToString("N");

            var uri = new Uri(string.Format("http://localhost:{0}/{1}", _port, ServiceName));
            var binding = new BasicHttpBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue
            };

            _host = new ServiceHost(typeof(ServerData), uri);
            ((ServiceHost)_host).AddServiceEndpoint(typeof(IServerData), binding, "");
            ((ServiceHost)_host).Open();

            IsServer = true;
#else
            // WCF server-side hosting not supported in .NET 8
            throw new PlatformNotSupportedException("WCF server-side hosting is not supported in .NET 8");
#endif
        }

        public static void ShutdownServer()
        {
#if NETFRAMEWORK
            if(_host != null) ((ServiceHost)_host).Close();
            _host = null;
#endif
            IsServer = false;
        }

        public static void StartClient(string service_name)
        {
#if NETFRAMEWORK
            ServiceName = service_name;

            var uri = new Uri(string.Format("http://localhost:{0}/{1}", _port, ServiceName));
            var address = new EndpointAddress(uri);
            var binding = new BasicHttpBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue
            };

            var factory = new ChannelFactory<IServerData>(binding, address);
            ServerData = factory.CreateChannel();

            IsClient = true;
#else
            // WCF client functionality may need alternative implementation for .NET 8
            throw new PlatformNotSupportedException("WCF client functionality may need alternative implementation for .NET 8");
#endif
        }
    }

    internal interface IWCFServer
    {
        void SendMessage(string text);
        XElement GetReportParams();
        string GetReportName();
        string GetWorkFolder();
        void SetClientState(ClientState state);
        void SetClientData(VDataSet data);
        void SetReportTime(string time);
        ServerCommand GetCommand();
    }

#if NETFRAMEWORK
    [ServiceContract]
#endif
    internal interface IServerData
    {
#if NETFRAMEWORK
        [OperationContract]
#endif
        string GetEncryptedConnectionString();

#if NETFRAMEWORK
        [OperationContract]
#endif
        void SetData(VDataSet data);

#if NETFRAMEWORK
        [OperationContract]
#endif
        void SetReportTime(string time);

#if NETFRAMEWORK
        [OperationContract]
#endif
        void SendMessage(string text);

#if NETFRAMEWORK
        [OperationContract]
#endif
        string GetReportName();

#if NETFRAMEWORK
        [OperationContract]
#endif
        string GetWorkFolder();

#if NETFRAMEWORK
        [OperationContract]
#endif
        XElement GetReportParams();

#if NETFRAMEWORK
        [OperationContract]
#endif
        void SetClientState(ClientState state);

#if NETFRAMEWORK
        [OperationContract]
#endif
        ServerCommand GetCommand();
    }

    internal class ServerData : IServerData
    {
        public string GetEncryptedConnectionString()
        {
            throw new NotImplementedException();
            //return HelperCrypt.Encrypt(DataHelper.GetConnectionString(db.Connection));
        }

        public void SetData(VDataSet data)
        {
            WCFHelper.Server.SetClientData(Cmn.ToVDataSet(data));
        }

        public void SetReportTime(string time)
        {
            WCFHelper.Server.SetReportTime(time);
        }

        public void SendMessage(string text)
        {
            WCFHelper.Server.SendMessage(text);
        }

        public string GetWorkFolder()
        {
            return WCFHelper.Server.GetWorkFolder();
        }

        public XElement GetReportParams()
        {
            return WCFHelper.Server.GetReportParams();
        }

        public void SetClientState(ClientState state)
        {
            WCFHelper.Server.SetClientState(state);
        }

        public ServerCommand GetCommand()
        {
            return WCFHelper.Server.GetCommand();
        }

        public string GetReportName()
        {
            return WCFHelper.Server.GetReportName();
        }
    }

    internal enum ServerCommand
    {
        Wait,
        ExecuteReport,
        PrintExcel,
        CloseApplication
    }

    internal enum ClientState
    {
        Unknown,
        Ready,
        ExecutingReport,
        PrintingExcel,
        Fault
    }
}