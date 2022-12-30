using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phidget22;
using Phidget22.Events;

namespace voimasensori
{
    public partial class Form1 : Form
    {
        Mutex DataToDiskMutex = new Mutex();
        
        VoltageRatioInput ratio = null; //declare our voltageratio object that will be used to connect to the bridge device
                                        //double m = 41809117.297, b = 2412.078; //our data points as well as the calculated slope and intercept

        private readonly double m = 5215460.310;
        private readonly double b = -346.892;

        List<DataPoint> DataToDisk = new List<DataPoint>();
        string logfilename = "RocketData_" + DateTime.Now.ToLongDateString() + "-" + DateTime.Now.ToLongTimeString() + ".txt";
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ratio.Close(); //clean up to prevent issues down the line
        }

        public Form1()
        {
            InitializeComponent();
        }
        static void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 23;

                // Prefer a using declaration to ensure the instance is Disposed later.
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Explicit close is not necessary since TcpClient.Dispose() will be
                // called automatically.
                // stream.Close();
                // client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }
        private void udptest()
        {

            UdpClient udpClient = new UdpClient();

        Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there");
            try
            {
                udpClient.Send(sendBytes, sendBytes.Length, "10.73.73.135", 2390);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }

        }

        static async Task CallLaunch()
        {
            var client = new HttpClient();

            var result = await client.GetAsync("http://192.168.4.1/LAUNCH");
            Console.WriteLine(result.StatusCode);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Text = logfilename;

            ratio = new VoltageRatioInput(); //create an instance of the object

            //register the pertinent event handlers
            ratio.Attach += ratio_attach;
            ratio.Detach += ratio_detach;
            ratio.VoltageRatioChange += ratio_change;

            try
            {
                ratio.Open(); //open the device
            }
            catch (PhidgetException ex)
            {//let user know if there's an error while opening
                lblStatus1.Text = "Error connecting to device: " + ex.Description;
                lblStatus1.Visible = true;
            }

        }

        void ratio_attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            VoltageRatioInput attached = (VoltageRatioInput)sender;
            attached.DataInterval = attached.MinDataInterval;
        }

        void ratio_detach(object sender, Phidget22.Events.DetachEventArgs e)
        {
            //reset form when device detaches
            lblStatus1.Text = "Detached";
            ratio.Channel = 0;
        }

        void ratio_change(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e)
        {

            double l = ((m * e.VoltageRatio) + b);
            lblLoad1.Text = l.ToString("f3"); //if we have input the necessary information we can output a load value as data comes in

            if (chkActive.Checked)
            { 
                addToGraph(l, chart1);
                DataToDiskMutex.WaitOne();
                DataToDisk.Add(new DataPoint(l));
                DataToDiskMutex.ReleaseMutex();
            }
            aGauge1.Value = (float)l;
        }

        void addToGraph(double value, System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            chart.Series.First().Points.Add(value);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DataToDisk.Count == 0)
                return;

            // copy data to be written to disk
            // and release it for other thread as soon as possible
            // so this thread can continue to write to disk
            DataToDiskMutex.WaitOne();
            List<DataPoint> clonedDataToDisk = new List<DataPoint>(DataToDisk);
            DataToDisk.Clear();
            DataToDiskMutex.ReleaseMutex();

            SaveToCsv(clonedDataToDisk, logfilename);

        }

        private void SaveToCsv<T>(List<T> reportData, string path)
        {
            timerWriteToFile.Enabled = false;
            lblStatus1.Text = "Start log " + DateTime.Now.ToString();

            var lines = new List<string>();
            //IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(T)).OfType<PropertyDescriptor>();
            //var header = string.Join(",", props.ToList().Select(x => x.Name));
            //lines.Add(header);

            //var valueLines = reportData.Select(row => string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
            var valueLines = reportData.Select(row => row.ToString());// string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
            lines.AddRange(valueLines);
            File.WriteAllLines(path, lines.ToArray());
            timerWriteToFile.Enabled = true;
            lblStatus1.Text = "Wrote log " + DateTime.Now.ToString();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DataToDiskMutex.WaitOne();
            double l = DataToDisk.Count();
            DataToDiskMutex.ReleaseMutex();
            addToGraph(l, chartImpulse);
        }

        public static async Task<string> Get(string url)
        {
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            var responseStream = responseObject.GetResponseStream();
            var sr = new StreamReader(responseStream);
            string received = await sr.ReadToEndAsync();

            return received;
        }

        public string GetSync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CallLaunch().GetAwaiter().GetResult();
            Get("http://192.168.4.1/LAUNCH");
        }

        private async Task queryArduinoStatus()
        {
            try
            {
                lblArduinoStatus.BackColor = Color.Orange;
                Application.DoEvents();

                string ret = "";
                timer1.Enabled = false;
                RequestManager rm = new RequestManager();
                ret = await rm.GET("http://192.168.4.1/STATUS");
                if (ret != "")
                {
                    ret = ret.Replace("\r", "");
                    ret = ret.Replace("\n", "");
                    lblArduinoStatus.Text = "Arduino OK (" + ret + ")";
                    lblArduinoStatus.BackColor = Color.LightGreen;
                }

            }
            catch (Exception ex)
            {

            }

            timer1.Enabled = true;

        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            queryArduinoStatus();

            //lblArduinoStatus.Text = Get("http://192.168.4.1/STATUS");

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string logfilenameb = "BackupData_" + DateTime.Now.ToLongDateString() + "-" + DateTime.Now.ToLongTimeString() + ".csv";

            string csvLine;
            string csvContent;
            csvContent = "";

            foreach (System.Windows.Forms.DataVisualization.Charting.Series series in this.chart1.Series)
            {
                string seriesName = series.Name;
                int pointCount = series.Points.Count;
                //string seriesType = series.Type.ToString();
                string comma = ";";

                for (int p = 0; p < pointCount; p++)
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point = series.Points[p];
                    string yValuesCSV = String.Empty;
                    int count = point.YValues.Length;
                    for (int i = 0; i < count; i++)
                    {
                        yValuesCSV += point.YValues[i];

                        if (i != count - 1)
                            yValuesCSV += comma;
                    }

                    csvLine = seriesName + "-" + comma + point.XValue + comma + yValuesCSV;
                    csvContent += csvLine + "\r\n";
                }
            }

            // Using stream writer class the chart points are exported. Create an instance of the stream writer class.
            System.IO.StreamWriter file = new System.IO.StreamWriter(logfilenameb);

            // Write the datapoints into the file.
            file.WriteLine(csvContent);

            file.Close();

        }
    }
}
