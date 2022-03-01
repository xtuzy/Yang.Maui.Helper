using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Yang.Maui.Helper.Tools
{
    delegate void AddMessage(string sNewMessage);
    /// <summary>
    /// 端口固定为399,不会自动重连,连接失败时请新创建对象
    /// </summary>
    public class MessageClientApp : IDisposable
    {
        // My Attributes
        private Socket m_sock;                      // Server connection
        private byte[] m_byBuff = new byte[256];    // Recieved data buffer
        private event AddMessage m_AddMessageEventHandler;              // Add Message Event handler for Form

        public string ServerIPAddressText;

        List<string> ClientRecievedData = new List<string>();
        
        public MessageClientApp(string ip)
        {
            ServerIPAddressText = ip;
            //m_AddMessageEventHandler += OnAddMessage;

            ConnectServer();
        }

        /// <summary>
        /// Connect button pressed. Attempt a connection to the server and 
        /// setup Recieved data callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectServer()
        {
            try
            {
                // Close the socket if it is still open
                if (m_sock != null && m_sock.Connected)
                {
                    m_sock.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    m_sock.Close();
                }

                // Create the socket object
                m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Define the Server address and port
                IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(ServerIPAddressText), 399);

                // Connect to the server blocking method and setup callback for recieved data
                // m_sock.Connect( epServer );
                // SetupRecieveCallback( m_sock );

                // Connect to server non-Blocking method
                m_sock.Blocking = false;
                AsyncCallback onconnect = new AsyncCallback(OnConnect);
                m_sock.BeginConnect(epServer, onconnect, m_sock);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ",Server Connect failed!");
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            // Check if we were sucessfull
            try
            {
                //sock.EndConnect( ar );
                if (sock.Connected)
                    SetupRecieveCallback(sock);
                else
                    Console.WriteLine("Unable to connect to remote machine," + "Connect Failed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ",Unusual error during Connect!");
            }
        }

        /// <summary>
        /// Get the new data and send it out to all other connections. 
        /// Note: If not data was recieved the connection has probably 
        /// died.
        /// </summary>
        /// <param name="ar"></param>
        private void OnRecievedData(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            // Check if we got any data
            try
            {
                int nBytesRec = sock.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    // Wrote the data to the List
                    //string sRecieved = Encoding.ASCII.GetString(m_byBuff, 0, nBytesRec);//改成支持中文
                    string sRecieved = Encoding.UTF8.GetString(m_byBuff, 0, nBytesRec);

                    // WARNING : The following line is NOT thread safe. Invoke is
                    // m_lbRecievedData.Items.Add( sRecieved );
                    if(m_AddMessageEventHandler!=null)
                        m_AddMessageEventHandler.Invoke(sRecieved);

                    // If the connection is still usable restablish the callback
                    SetupRecieveCallback(sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    Console.WriteLine("Client {0}, disconnected", sock.RemoteEndPoint);
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ",Unusual error druing Recieve!");
            }
        }

        /// <summary>
        /// 接收Server消息
        /// </summary>
        /// <param name="sMessage"></param>
        private void OnAddMessage(string sMessage)
        {
            //ClientRecievedData.Add(sMessage);
            //Console.WriteLine("Accept:" + sMessage);
        }

        /// <summary>
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        private void SetupRecieveCallback(Socket sock)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnRecievedData);
                sock.BeginReceive(m_byBuff, 0, m_byBuff.Length, SocketFlags.None, recieveData, sock);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ",Setup Recieve Callback failed!");
            }
        }

        /// <summary>
        /// Close the Socket connection bofore going home
        /// </summary>
        private void Connect_Closing()
        {
            if (m_sock != null && m_sock.Connected)
            {
                m_sock.Shutdown(SocketShutdown.Both);
                m_sock.Close();
                Console.WriteLine("连接关闭");
            }
        }

        /// <summary>
        /// Send the Message in the Message area. Only do this if we are connected
        /// </summary>
        public void SendMessage(string messageText)
        {
            // Check we are connected
            if (m_sock == null || !m_sock.Connected)
            {
                Console.WriteLine("Must be connected to Send a message");
                return;
            }

            // Read the message from the text box and send it
            try
            {
                // Convert to byte array and send.
                //Byte[] byteDateLine = Encoding.ASCII.GetBytes(NeedSendMessageText.ToCharArray());//改成支持中文
                Byte[] byteDateLine = Encoding.UTF8.GetBytes(messageText.ToCharArray());
                m_sock.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Send Message Failed!");
            }
        }

        public void Dispose()
        {
            Connect_Closing();
            ClientRecievedData.Clear();
            ClientRecievedData = null;
        }
    }
}
