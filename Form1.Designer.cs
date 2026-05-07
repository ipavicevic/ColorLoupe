namespace DesktopColorSampler
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
            this.hexValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rgbValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pressSpaceMessageBox = new System.Windows.Forms.Label();
            this.colorSample = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.editColorButton = new System.Windows.Forms.Button();
            this.zoom = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.zoomOn = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // hexValue
            // 
            this.hexValue.BackColor = System.Drawing.Color.White;
            this.hexValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexValue.Location = new System.Drawing.Point(12, 95);
            this.hexValue.Name = "hexValue";
            this.hexValue.ReadOnly = true;
            this.hexValue.Size = new System.Drawing.Size(150, 26);
            this.hexValue.TabIndex = 0;
            this.hexValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "HEX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "RGB";
            // 
            // rgbValue
            // 
            this.rgbValue.BackColor = System.Drawing.Color.White;
            this.rgbValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgbValue.Location = new System.Drawing.Point(12, 25);
            this.rgbValue.Name = "rgbValue";
            this.rgbValue.ReadOnly = true;
            this.rgbValue.Size = new System.Drawing.Size(150, 26);
            this.rgbValue.TabIndex = 3;
            this.rgbValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "COLOR SAMPLE";
            // 
            // pressSpaceMessageBox
            // 
            this.pressSpaceMessageBox.AutoSize = true;
            this.pressSpaceMessageBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressSpaceMessageBox.Location = new System.Drawing.Point(12, 178);
            this.pressSpaceMessageBox.Name = "pressSpaceMessageBox";
            this.pressSpaceMessageBox.Size = new System.Drawing.Size(264, 26);
            this.pressSpaceMessageBox.TabIndex = 8;
            this.pressSpaceMessageBox.Text = "Move the mouse around...";
            this.pressSpaceMessageBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // colorSample
            // 
            this.colorSample.BackColor = System.Drawing.Color.White;
            this.colorSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorSample.Location = new System.Drawing.Point(168, 25);
            this.colorSample.Name = "colorSample";
            this.colorSample.Size = new System.Drawing.Size(100, 100);
            this.colorSample.TabIndex = 10;
            this.colorSample.Paint += new System.Windows.Forms.PaintEventHandler(this.colorSample_Paint);
            // 
            // editColorButton
            // 
            this.editColorButton.Enabled = false;
            this.editColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editColorButton.Location = new System.Drawing.Point(168, 128);
            this.editColorButton.Name = "editColorButton";
            this.editColorButton.Size = new System.Drawing.Size(100, 23);
            this.editColorButton.TabIndex = 11;
            this.editColorButton.Text = "EDIT COLOR";
            this.editColorButton.UseVisualStyleBackColor = true;
            this.editColorButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // zoom
            // 
            this.zoom.BackColor = System.Drawing.Color.White;
            this.zoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zoom.Location = new System.Drawing.Point(274, 25);
            this.zoom.Name = "zoom";
            this.zoom.Size = new System.Drawing.Size(100, 100);
            this.zoom.TabIndex = 12;
            this.zoom.Paint += new System.Windows.Forms.PaintEventHandler(this.zoom_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(281, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "X10 ZOOM";
            // 
            // zoomOn
            // 
            this.zoomOn.AutoSize = true;
            this.zoomOn.Checked = true;
            this.zoomOn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoomOn.Location = new System.Drawing.Point(274, 132);
            this.zoomOn.Name = "zoomOn";
            this.zoomOn.Size = new System.Drawing.Size(77, 17);
            this.zoomOn.TabIndex = 14;
            this.zoomOn.Text = "ZOOM ON";
            this.zoomOn.UseVisualStyleBackColor = true;
            this.zoomOn.CheckedChanged += new System.EventHandler(this.zoomOn_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 227);
            this.Controls.Add(this.zoomOn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.zoom);
            this.Controls.Add(this.editColorButton);
            this.Controls.Add(this.colorSample);
            this.Controls.Add(this.pressSpaceMessageBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rgbValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hexValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DESKTOP COLOR SAMPLER";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox hexValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox rgbValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label pressSpaceMessageBox;
        private System.Windows.Forms.Label colorSample;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button editColorButton;
        private System.Windows.Forms.Label zoom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox zoomOn;
    }
}

