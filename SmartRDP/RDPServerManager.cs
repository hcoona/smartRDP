﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace SmartRDP
{
    class RDPServerManager
    {
        private List<RDPServer> servers = new List<RDPServer>();

        public List<RDPServer> Servers
        { 
            get
            {
                return servers;
            }
        }

        private RDPServerManager()
        {
            try
            {
                if(File.Exists(@"servers.json"))
                {
                    servers = JsonConvert.DeserializeObject<List<RDPServer>>(File.ReadAllText(@"servers.json"));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        ~RDPServerManager()
        {
            try
            {
                // 暂时放在这里
                // 关闭应用程序时，开始写入servers.json文件
                File.WriteAllText(@"servers.json", JsonConvert.SerializeObject(servers));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static readonly RDPServerManager Instance = new RDPServerManager();

        public string[] ServerNameArray
        {
            get
            {
                List<string> serverNameList = new List<string>();
                foreach(RDPServer server in servers)
                {
                    serverNameList.Add(server.Name);
                }

                return serverNameList.ToArray();
            }
        }

        public RDPServer getServerByName(string name, bool isVNC = false)
        {
            foreach (RDPServer server in servers)
            {
                if (server.Name == name && server.IsVNC == isVNC)
                    return server;
            }

            return null;
        }

        public void saveOrUpdateServer(RDPServer server)
        {
            RDPServer s = getServerByName(server.Name, server.IsVNC);
            if (s != null)
            {
                if (server.Password != s.Password)
                    s.Password = server.Password;
            }
            else
            {
                servers.Add(server);
            }
        }
    }
}
