using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace KaijiBot.Proxy
{
    class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException(string name) : base(ModifyMessage(name)) {  }

        private static string ModifyMessage(string name)
        {
            return string.Format("Cant find process by name {0}", name);
        }
    }
    class ProcessClosedException : Exception
    {
        public ProcessClosedException(string name) : base(ModifyMessage(name)) { }

        private static string ModifyMessage(string name)
        {
            return string.Format("Process {0} terminated", name);
        }
    }

    class ToManyProcessesException : Exception
    {
        Process[] processes;
        public ToManyProcessesException(string name, Process[] processes) 
            : base(ModifyMessage(name)) {
            this.processes = processes;
        }

        private static string ModifyMessage(string name)
        {
           return string.Format("Found too many processes with name {0}", name);
        }
    }

    class ProcessConnector
    {

        private Process process { get; set; }
        private string name { get; set; }

        public ProcessConnector()
        {

        }
        public int Connect(string name)
        {
            Logger.LoggerContoller.ProcessLogger
                .Debug(string.Format("Findnding process \"{0}\"", name));
            this.process = FindProcess(name);
            this.name = name;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            return process.Id;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            throw new ProcessClosedException(name);
        }

        private Process FindProcess(string name)
        {
            var processes = Process.GetProcessesByName(name);
            Logger.LoggerContoller.ProcessLogger
                .Verbose(string.Format("Found: {0} by name: {1}", processes.Length, name));
            processes = processes.Where(p => p.MainWindowTitle.IndexOf("ChromeApps") != -1).ToArray();
            Logger.LoggerContoller.ProcessLogger
                .Verbose(string.Format("Left after filter: {0}", processes.Length));
            if (processes.Length == 0)
            {
                throw new ProcessNotFoundException(name);
            } else if (processes.Length > 1)
            {
                throw new ToManyProcessesException(name, processes);
            } else
            {
                return processes[0];
            }
        }
    }
}
