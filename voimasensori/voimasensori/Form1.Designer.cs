namespace voimasensori
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblLoad1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.warningIcon = new System.Windows.Forms.ToolStripStatusLabel();
            this.aGauge1 = new System.Windows.Forms.AGauge();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 105);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(563, 411);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // lblLoad1
            // 
            this.lblLoad1.AutoSize = true;
            this.lblLoad1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoad1.Location = new System.Drawing.Point(31, 42);
            this.lblLoad1.Name = "lblLoad1";
            this.lblLoad1.Size = new System.Drawing.Size(445, 37);
            this.lblLoad1.TabIndex = 1;
            this.lblLoad1.Text = "Rocket motor thurst (grams)";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.warningIcon});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(999, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // warningIcon
            // 
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(12, 17);
            this.warningIcon.Text = "-";
            // 
            // aGauge1
            // 
            this.aGauge1.BaseArcColor = System.Drawing.Color.DimGray;
            this.aGauge1.BaseArcRadius = 180;
            this.aGauge1.BaseArcStart = 135;
            this.aGauge1.BaseArcSweep = 270;
            this.aGauge1.BaseArcWidth = 2;
            this.aGauge1.Center = new System.Drawing.Point(200, 200);
            this.aGauge1.Location = new System.Drawing.Point(578, 105);
            this.aGauge1.MaxValue = 500F;
            this.aGauge1.MinValue = -100F;
            this.aGauge1.Name = "aGauge1";
            this.aGauge1.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Red;
            this.aGauge1.NeedleColor2 = System.Drawing.Color.DimGray;
            this.aGauge1.NeedleRadius = 160;
            this.aGauge1.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.aGauge1.NeedleWidth = 3;
            this.aGauge1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesInterInnerRadius = 173;
            this.aGauge1.ScaleLinesInterOuterRadius = 180;
            this.aGauge1.ScaleLinesInterWidth = 1;
            this.aGauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesMajorInnerRadius = 170;
            this.aGauge1.ScaleLinesMajorOuterRadius = 180;
            this.aGauge1.ScaleLinesMajorStepValue = 50F;
            this.aGauge1.ScaleLinesMajorWidth = 2;
            this.aGauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge1.ScaleLinesMinorInnerRadius = 175;
            this.aGauge1.ScaleLinesMinorOuterRadius = 180;
            this.aGauge1.ScaleLinesMinorTicks = 9;
            this.aGauge1.ScaleLinesMinorWidth = 1;
            this.aGauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleNumbersFormat = null;
            this.aGauge1.ScaleNumbersRadius = 195;
            this.aGauge1.ScaleNumbersRotation = 0;
            this.aGauge1.ScaleNumbersStartScaleLine = 0;
            this.aGauge1.ScaleNumbersStepScaleLines = 1;
            this.aGauge1.Size = new System.Drawing.Size(400, 400);
            this.aGauge1.TabIndex = 3;
            this.aGauge1.Text = "aGauge1";
            this.aGauge1.Value = 0F;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 562);
            this.Controls.Add(this.aGauge1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblLoad1);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "Rocket motor test bench";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label lblLoad1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel warningIcon;
        private System.Windows.Forms.AGauge aGauge1;
    }
}

