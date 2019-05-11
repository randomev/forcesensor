using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phidget22;
using Phidget22.Events;

namespace voimasensori
{
    public partial class Form1 : Form
    {
        VoltageRatioInput ratio = null; //declare our voltageratio object that will be used to connect to the bridge device
        double m = 41809117.297, b = 2412.078; //our data points as well as the calculated slope and intercept

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
                warningIcon.Text = "Error connecting to device: " + ex.Description;
                warningIcon.Visible = true;
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
            warningIcon.Text = "Detached";
            ratio.Channel = 0;
        }

        void ratio_change(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            double l = ((m * e.VoltageRatio) + b);
            lblLoad1.Text = l.ToString("f3"); //if we have input the necessary information we can output a load value as data comes in

            addToGraph(l, chart1);
            aGauge1.Value = (float)l;
        }

        void addToGraph(double value, System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            chart.Series.First().Points.Add(value);
            
        }

    }
}
