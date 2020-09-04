using System;
using System.Threading;
using Opc.UaFx;
using Opc.UaFx.Client;

namespace OPCUA_Client {
    class Program {

        private static void ConnectedEvent( object sender , EventArgs e ) {
            Console.WriteLine( "OPCUA Connected" );
        }

        private static void DisconnectedEvent( object sender , EventArgs e ) {
            Console.WriteLine( "OPCUA Disconnected" );
        }

        private static void Browse( OpcNodeInfo node , int level = 0 ) {
            Console.WriteLine( "{0}{1}({2})" ,
                    new string( '.' , level * 4 ) ,
                    node.Attribute( OpcAttribute.DisplayName ).Value ,
                    node.NodeId );

            level++;

            foreach( var childNode in node.Children() )
                Browse( childNode , level );
        }

        static void Main( string[] args ) {
            var client = new OpcClient( "opc.tcp://opcuaserver.com:48010" );
            client.Connected += new EventHandler(ConnectedEvent);
            client.Disconnected += new EventHandler(DisconnectedEvent);

            client.Connect();

            var node = client.BrowseNode( OpcObjectTypes.ObjectsFolder );
            Browse( node );

            Thread.Sleep( 6000 );
            client.Disconnect();
        }
    }
}
