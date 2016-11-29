namespace timeMachine
{
    partial class frmFileLocations
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
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.txtExtRefFile = new System.Windows.Forms.TextBox();
            this.btnUpdateFileLocations = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.lblDisclaimer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectFile.Location = new System.Drawing.Point(9, 17);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(86, 18);
            this.lblSelectFile.TabIndex = 0;
            this.lblSelectFile.Text = "Select File:";
            // 
            // txtExtRefFile
            // 
            this.txtExtRefFile.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtRefFile.Location = new System.Drawing.Point(12, 38);
            this.txtExtRefFile.Name = "txtExtRefFile";
            this.txtExtRefFile.Size = new System.Drawing.Size(457, 26);
            this.txtExtRefFile.TabIndex = 1;
            // 
            // btnUpdateFileLocations
            // 
            this.btnUpdateFileLocations.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateFileLocations.Location = new System.Drawing.Point(216, 126);
            this.btnUpdateFileLocations.Name = "btnUpdateFileLocations";
            this.btnUpdateFileLocations.Size = new System.Drawing.Size(131, 44);
            this.btnUpdateFileLocations.TabIndex = 2;
            this.btnUpdateFileLocations.Text = "Import File";
            this.btnUpdateFileLocations.UseVisualStyleBackColor = true;
            this.btnUpdateFileLocations.Click += new System.EventHandler(this.btnUpdateFileLocations_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectFile.Location = new System.Drawing.Point(475, 38);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 26);
            this.btnSelectFile.TabIndex = 3;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // lblDisclaimer
            // 
            this.lblDisclaimer.AutoSize = true;
            this.lblDisclaimer.Location = new System.Drawing.Point(12, 78);
            this.lblDisclaimer.Name = "lblDisclaimer";
            this.lblDisclaimer.Size = new System.Drawing.Size(213, 13);
            this.lblDisclaimer.TabIndex = 4;
            this.lblDisclaimer.Text = "Caution: Your import file must be a \'.csv\' file.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(377, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Reference number must be column 1 and description/title must be in column 3.";
            // 
            // frmFileLocations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 206);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDisclaimer);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.btnUpdateFileLocations);
            this.Controls.Add(this.txtExtRefFile);
            this.Controls.Add(this.lblSelectFile);
            this.Name = "frmFileLocations";
            this.Text = "Import External Tasks";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectFile;
        private System.Windows.Forms.TextBox txtExtRefFile;
        private System.Windows.Forms.Button btnUpdateFileLocations;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label lblDisclaimer;
        private System.Windows.Forms.Label label2;
    }
}