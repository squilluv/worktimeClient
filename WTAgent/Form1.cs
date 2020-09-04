using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing.Imaging;
using System.Timers;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Windows.Automation;
using System.Security.Principal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WTAgent
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer ScreenTimer;

        //Объявляем таймер для браузера
        private static System.Timers.Timer URLTimer;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public Form1()
        {
            InitializeComponent();
        }

        class Machine
        {
            public string name { get; set; }
            public string machine { get; set; }
            public string id { get; set; }
        }

        private static string machineId;
        private static string path = "http://localhost";
        private static string name = "workers";
        private static string pass = "QxTyNpUdTr174";

        private void Form1_Load(object sender, EventArgs e) 
        {
            this.Visible = false;
            this.ShowInTaskbar = false;

            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "Для закрытия нажвмите дважды на иконку";
            notifyIcon1.BalloonTipText = "Инструкция";
            notifyIcon1.DoubleClick += notifyIcon1_Click;

            string res = GetMachines();
            Machine[] ResJson = JsonConvert.DeserializeObject<Machine[]>(res);
            Machine machine = Array.Find(ResJson, MyMachineName);
            machineId = machine.id;

            SetTimerURL();
            SetTimerScreenshot();
        }



        private static void SetTimerScreenshot()
        {
            // Create a timer with a two second interval.
            ScreenTimer = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer. 
            ScreenTimer.Elapsed += screenshot;
            ScreenTimer.AutoReset = true;
            ScreenTimer.Enabled = true;

        }

        private static void SetTimerURL()
        {
            // Create a timer with a two second interval.
            URLTimer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 
            URLTimer.Elapsed += GetURL;
            URLTimer.AutoReset = true;
            URLTimer.Enabled = true;
        }

        private static string GetMachines()
        {
            using (var hc = new HttpClient())   
            {
                var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(authToken));
                var response = hc.GetAsync(path + "/api/machines?_format=json").Result.Content.ReadAsStringAsync().Result;

                return response;
            }
        }

        private static bool MyMachineName(Machine m)
        {
            return m.machine == Environment.MachineName;
        }

        private static void GetURL(Object source, ElapsedEventArgs e)
        {
            Process currentProcess = GetActiveProcess();
            string ProccesName = currentProcess.ProcessName;

            if (ProccesName == "chrome")
            {

                foreach (Process process in Process.GetProcessesByName("chrome"))
                {
                    string url = GetChromeUrl(process);
                    if (url == null)
                        continue;

                    CommandPAS(1, null, url, currentProcess);

                }
            }
            else
            {
                CommandPAS(2, null, null, currentProcess);
            }


        }

        private static void CommandPAS(int props, Image bmp, string url, Process currentProcess)
        {
            string ProccesName = currentProcess.ProcessName;
            string WindowTitle = currentProcess.MainWindowTitle;
            string MachineName = Environment.MachineName;
            string UserName = Environment.UserName;
            string idFile;
            string selfFile;


            switch (props)
            {
                case 1:
                    using (var hc = new HttpClient())
                    {
                        ProcessClass pas = new ProcessClass();
                        Type tl = new Type();
                        tl.href = "http://localhost/rest/type/node/process_and_screen";
                        Links l = new Links();
                        l.type = tl;
                        pas._links = l;
                        List<Type2> t2 = new List<Type2>();
                        t2.Add(new Type2() { target_id = "process_and_screen" });
                        pas.type = t2;
                        List<Title> title = new List<Title>();
                        title.Add(new Title() { value = currentProcess.ProcessName });
                        pas.title = title;
                        List<FieldType> type = new List<FieldType>();
                        type.Add(new FieldType() { value = "url" });
                        pas.field_type = type;
                        List<FieldActiveWindow> activeWindows = new List<FieldActiveWindow>();
                        activeWindows.Add(new FieldActiveWindow() { value = currentProcess.MainWindowTitle });
                        pas.field_active_window = activeWindows;
                        List<FieldMachine> fieldMachines = new List<FieldMachine>();
                        fieldMachines.Add(new FieldMachine() { value = machineId });
                        pas.field_machine = fieldMachines;
                        List<FieldUrl> fieldUrl = new List<FieldUrl>();
                        fieldUrl.Add(new FieldUrl() { value = url });
                        pas.field_url = fieldUrl;
                        var pasJson = JsonConvert.SerializeObject(pas);
                        var data = new StringContent(pasJson, Encoding.UTF8, "application/hal+json");
                        var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(authToken));
                        var response = hc.PostAsync(path + "/node?_format=json", data).Result.Content.ReadAsStringAsync().Result;
                    }
                    break;
                case 2:
                    using (var hc = new HttpClient())
                    {
                        ProcessClass pas = new ProcessClass();
                        Type tl = new Type();
                        tl.href = "http://localhost/rest/type/node/process_and_screen";
                        Links l = new Links();
                        l.type = tl;
                        pas._links = l;
                        List<Type2> t2 = new List<Type2>();
                        t2.Add(new Type2() { target_id = "process_and_screen" });
                        pas.type = t2;
                        List<Title> title = new List<Title>();
                        title.Add(new Title() { value = currentProcess.ProcessName });
                        pas.title = title;
                        List<FieldType> type = new List<FieldType>();
                        type.Add(new FieldType() { value = "process" });
                        pas.field_type = type;
                        List<FieldActiveWindow> activeWindows = new List<FieldActiveWindow>();
                        activeWindows.Add(new FieldActiveWindow() { value = currentProcess.MainWindowTitle });
                        pas.field_active_window = activeWindows;
                        List<FieldMachine> fieldMachines = new List<FieldMachine>();
                        fieldMachines.Add(new FieldMachine() { value = machineId });
                        pas.field_machine = fieldMachines;
                        var pasJson = JsonConvert.SerializeObject(pas);
                        var data = new StringContent(pasJson, Encoding.UTF8, "application/hal+json");
                        var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(authToken));
                        var response = hc.PostAsync(path + "/node?_format=hal_json", data).Result.Content.ReadAsStringAsync().Result;
                    }
                    break;
                case 3:
                    using (var hc = new HttpClient())
                    {
                        FileClass file = new FileClass();
                        Type tl = new Type();
                        tl.href = "http://localhost/rest/type/file/image";
                        Links l = new Links();
                        l.type = tl;
                        file._links = l;
                        List<Type2> t2 = new List<Type2>();
                        t2.Add(new Type2() { target_id = "file" });
                        file.type = t2;
                        List<Filename> filename = new List<Filename>();
                        filename.Add(new Filename() { value = currentProcess.ProcessName + "-" + machineId + ".png" });
                        file.filename = filename;
                        List<Filemime> filemime = new List<Filemime>();
                        filemime.Add(new Filemime() { value = @"image/png" });
                        file.filemime = filemime;
                        List<Data> data = new List<Data>();
                        data.Add(new Data() { value = Convert.ToBase64String(ImageToByte(bmp)) });
                        file.data = data;
                        List<Uri> uri = new List<Uri>();
                        uri.Add(new Uri() { value = "public://image.png" });
                        file.uri = uri;
                        var pasJson = JsonConvert.SerializeObject(file);
                        var datam = new StringContent(pasJson, Encoding.UTF8, "application/hal+json");
                        var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(authToken));
                        var response = hc.PostAsync(path + "/entity/file?_format=hal_json", datam).Result.Content.ReadAsStringAsync().Result;
                        ResFileClass ResJson = JsonConvert.DeserializeObject<ResFileClass>(response);
                        selfFile = ResJson._links.self.href;
                        idFile = ResJson.uuid[0].value;
                    }
                    using (var hc = new HttpClient())
                    {
                        ScreenClass pas = new ScreenClass();
                        Type tl = new Type();
                        tl.href = "http://localhost/rest/type/node/process_and_screen";
                        Links l = new Links();
                        l.type = tl;
                        List<FieldImageUrl> urlFile = new List<FieldImageUrl>();
                        urlFile.Add(new FieldImageUrl() { href = selfFile });
                        l.url = urlFile;
                        pas._links = l;
                        List<Type2> t2 = new List<Type2>();
                        t2.Add(new Type2() { target_id = "process_and_screen" });
                        pas.type = t2;
                        List<Title> title = new List<Title>();
                        title.Add(new Title() { value = currentProcess.ProcessName });
                        pas.title = title;
                        List<FieldType> type = new List<FieldType>();
                        type.Add(new FieldType() { value = "screenshot" });
                        pas.field_type = type;
                        List<FieldActiveWindow> activeWindows = new List<FieldActiveWindow>();
                        activeWindows.Add(new FieldActiveWindow() { value = currentProcess.MainWindowTitle });
                        pas.field_active_window = activeWindows;
                        List<FieldMachine> fieldMachines = new List<FieldMachine>();
                        fieldMachines.Add(new FieldMachine() { value = machineId });
                        pas.field_machine = fieldMachines;
                        List<FieldUrl> fieldUrl = new List<FieldUrl>();
                        fieldUrl.Add(new FieldUrl() { value = url });
                        pas.field_url = fieldUrl;
                        Self3 s3 = new Self3();
                        s3.href = selfFile;
                        Type3 t3 = new Type3();
                        t3.href = "http://localhost/rest/type/file/image";
                        Links3 l3 = new Links3();
                        l3.self = s3;
                        l3.type = t3;
                        List<Uuid3> uuid = new List<Uuid3>();
                        uuid.Add(new Uuid3() { value = idFile });
                        List<NodeFileImage> nfi = new List<NodeFileImage>();
                        nfi.Add(new NodeFileImage() { _links = l3, uuid = uuid, display = "1", description = "", alt = "", title = "" });
                        Embedded e = new Embedded();
                        e.url = nfi;
                        pas._embedded = e;
                        var pasJson = JsonConvert.SerializeObject(pas);
                        var data = new StringContent(pasJson, Encoding.UTF8, "application/hal+json");
                        var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(authToken));
                        var response = hc.PostAsync(path + "/node?_format=hal_json", data).Result.Content.ReadAsStringAsync().Result;
                    }
                    break;
                case 4:
                    using (var hc = new HttpClient())
                    {
                        FileClass file = new FileClass();
                        Type tl = new Type();
                        tl.href = "http://localhost/rest/type/file/image";
                        Links l = new Links();
                        l.type = tl;
                        file._links = l;
                        List<Type2> t2 = new List<Type2>();
                        t2.Add(new Type2() { target_id = "file" });
                        file.type = t2;
                        List<Filename> filename = new List<Filename>();
                        filename.Add(new Filename() { value = currentProcess.ProcessName + "-" + machineId + ".png" });
                        file.filename = filename;
                        List<Filemime> filemime = new List<Filemime>();
                        filemime.Add(new Filemime() { value = @"image/png" });
                        file.filemime = filemime;
                        List<Data> data = new List<Data>();
                        data.Add(new Data() { value = Convert.ToBase64String(ImageToByte(bmp)) });
                        file.data = data;
                        List<Uri> uri = new List<Uri>();
                        uri.Add(new Uri() { value = "public://image.png" });
                        file.uri = uri;
                        var pasJson = JsonConvert.SerializeObject(file);
                        var datam = new StringContent(pasJson, Encoding.UTF8, "application/hal+json");
                        var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(authToken));
                        var response = hc.PostAsync(path + "/entity/file?_format=hal_json", datam).Result.Content.ReadAsStringAsync().Result;
                        ResFileClass ResJson = JsonConvert.DeserializeObject<ResFileClass>(response);
                        selfFile = ResJson._links.self.href;
                        idFile = ResJson.uuid[0].value;
                    }
                    using (var hc = new HttpClient())
                    {
                        ScreenClass pas = new ScreenClass();
                        Type tl = new Type();
                        tl.href = "http://localhost/rest/type/node/process_and_screen";
                        Links l = new Links();
                        l.type = tl;
                        List<FieldImageUrl> urlFile = new List<FieldImageUrl>();
                        urlFile.Add(new FieldImageUrl() { href = selfFile });
                        l.url = urlFile;
                        pas._links = l;
                        List<Type2> t2 = new List<Type2>();
                        t2.Add(new Type2() { target_id = "process_and_screen" });
                        pas.type = t2;
                        List<Title> title = new List<Title>();
                        title.Add(new Title() { value = currentProcess.ProcessName });
                        pas.title = title;
                        List<FieldType> type = new List<FieldType>();
                        type.Add(new FieldType() { value = "screenshot" });
                        pas.field_type = type;
                        List<FieldActiveWindow> activeWindows = new List<FieldActiveWindow>();
                        activeWindows.Add(new FieldActiveWindow() { value = currentProcess.MainWindowTitle });
                        pas.field_active_window = activeWindows;
                        List<FieldMachine> fieldMachines = new List<FieldMachine>();
                        fieldMachines.Add(new FieldMachine() { value = machineId });
                        pas.field_machine = fieldMachines;
                        Self3 s3 = new Self3();
                        s3.href = selfFile;
                        Type3 t3 = new Type3();
                        t3.href = "http://localhost/rest/type/file/image";
                        Links3 l3 = new Links3();
                        l3.self = s3;
                        l3.type = t3;
                        List<Uuid3> uuid = new List<Uuid3>();
                        uuid.Add(new Uuid3() { value = idFile });
                        List<NodeFileImage> nfi = new List<NodeFileImage>();
                        nfi.Add(new NodeFileImage() { _links = l3, uuid = uuid, display = "1", description = "", alt = "", title = "" });
                        Embedded e = new Embedded();
                        e.url = nfi;
                        pas._embedded = e;
                        var pasJson = JsonConvert.SerializeObject(pas);
                        var data = new StringContent(pasJson, Encoding.UTF8, "application/hal+json");
                        var authToken = Encoding.ASCII.GetBytes($"{name}:{pass}");
                        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(authToken));
                        var response = hc.PostAsync(path + "/node?_format=hal_json", data).Result.Content.ReadAsStringAsync().Result;
                    }
                    break;

            }
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private static string GetChromeUrl(Process process)
        {
            try
            {
                if (process == null)
                    throw new ArgumentNullException("process");

                if (process.MainWindowHandle == IntPtr.Zero)
                    return null;

                AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
                if (element == null)
                    return null;

                AutomationElementCollection edits5 = element.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                AutomationElement edit = edits5[0];
                string url = ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                return url;
            }
            catch
            {
                return null;
            }

        }

        private static Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();
            return hwnd != null ? GetProcessByHandle(hwnd) : null;

        }

        private static Process GetProcessByHandle(IntPtr hwnd)
        {
            try
            {
                uint processID;
                GetWindowThreadProcessId(hwnd, out processID);
                return Process.GetProcessById((int)processID);

            }
            catch { return null; }
        }

        private static void screenshot(Object source, ElapsedEventArgs e)
        {
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            Process currentProcess = GetActiveProcess();
            string ProccesName = currentProcess.ProcessName;
            string UserName = Environment.UserName;

            // Create a bitmap of the appropriate size to receive the screenshot.
            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                }

                if (ProccesName == "chrome")
                {

                    foreach (Process process in Process.GetProcessesByName("chrome"))
                    {
                        string url = GetChromeUrl(process);
                        if (url == null)
                            continue;

                        CommandPAS(3, bmp, url, currentProcess);

                    }
                }
                else
                {
                    CommandPAS(4, bmp, null, currentProcess);
                }

            }
        }

        void notifyIcon1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
