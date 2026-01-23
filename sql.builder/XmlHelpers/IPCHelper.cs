using System;
using Devart.Data.Oracle;

namespace sql.builder.XmlHelpers
{
    // использовать WCFHelper
    internal static class IPCHelper
    {
        //public static IPCData Data { get; set; }

        //static string _channelName = "ServerChannel";
        //public static void InitializeServer()
        //{
        //    var channel = new IpcChannel(_channelName);
        //    ChannelServices.RegisterChannel(channel, false);
        //    RemotingConfiguration.RegisterWellKnownServiceType(typeof(IPCData), "IPCData", WellKnownObjectMode.Singleton);
        //    Data = new IPCData() {Connection = db.Connection};
        //}

        //public static void InitializeClient()
        //{
        //    var channel = new IpcChannel();
        //    var remoteType = new WellKnownClientTypeEntry(typeof(IPCData), "ipc://" + _channelName + "/IPCData");
        //    RemotingConfiguration.RegisterWellKnownClientType(remoteType);
        //    Data = new IPCData();
        //    ShowMessage.ShowInformation(Data.Connection.ToString());
        //}
    }

    //public class IPCData : MarshalByRefObject
    //{
    //    public OracleConnection Connection { get; set; }
    //}
}