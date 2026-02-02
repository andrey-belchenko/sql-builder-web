using System;
using System.Data;
using System.Linq;
// WCF is Windows-specific and not needed for cross-platform
//using System.ServiceModel;
//using System.Windows.Forms;
using System.Xml.Linq;
using infoenergo.core.Data;
using infoenergo.sys;
using sql.builder.DataApi;
using sql.builder.WinForms;
//using infoenergo.framework.Extensions.Oracle;

namespace sql.builder.XmlHelpers
{
    // WCF functionality commented out - not needed for cross-platform
    /*
    public static class WCFHelper
    {
        public static bool IsServer { get; private set; }
        public static bool IsClient { get; private set; }

        static int _port = 8080;
        static ServiceHost _host;
        public static IServerData ServerData { get; private set; }
        public static IWCFServer Server { get; private set; }

        public static string ServiceName { get; private set; }

        public static void StartServer(IWCFServer server)
        {
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
            _host.AddServiceEndpoint(typeof(IServerData), binding, "");
            _host.Open();

            IsServer = true;
        }

        public static void ShutdownServer()
        {
            if(_host != null) _host.Close();
            _host = null;

            IsServer = false;
        }

        public static void StartClient(string service_name)
        {
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
        }
    }

    public interface IWCFServer
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

    [ServiceContract]
    public interface IServerData
    {
        [OperationContract]
        string GetEncryptedConnectionString();

        [OperationContract]
        void SetData(VDataSet data);

        [OperationContract]
        void SetReportTime(string time);

        [OperationContract]
        void SendMessage(string text);

        [OperationContract]
        string GetReportName();

        [OperationContract]
        string GetWorkFolder();

        [OperationContract]
        XElement GetReportParams();

        [OperationContract]
        void SetClientState(ClientState state);

        [OperationContract]
        ServerCommand GetCommand();
    }

    public class ServerData : IServerData
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

    public enum ServerCommand
    {
        Wait,
        ExecuteReport,
        PrintExcel,
        CloseApplication
    }

    public enum ClientState
    {
        Unknown,
        Ready,
        ExecutingReport,
        PrintingExcel,
        Fault
    }
    */
    
    // Placeholder to prevent compilation errors where WCFHelper is referenced
    public static class WCFHelper
    {
        public static bool IsServer { get; private set; }
        public static bool IsClient { get; private set; }
        
        public static object Server { get; private set; }
        
        public static void StartServer(object server) { }
        public static void ShutdownServer() { }
        public static void StartClient(string service_name) { }
    }
    
    // Placeholder interfaces and classes for WCF
    public interface IWCFServer { }
    public class ServerData { }
    public enum ServerCommand { Wait, ExecuteReport, PrintExcel, CloseApplication }
    public enum ClientState { Unknown, Ready, ExecutingReport, PrintingExcel, Fault }
}