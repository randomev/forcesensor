using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ratio.Close(); //clean up to prevent issues down the line
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

            SaveToCsv(clonedDataToDisk, "RocketSaveData.txt");

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
    }
}
