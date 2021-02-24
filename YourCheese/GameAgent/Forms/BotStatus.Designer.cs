
namespace YourCheese.GameAgent.Forms
{
    partial class BotStatus
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.isImposterLabel = new System.Windows.Forms.Label();
            this.modeLabel = new System.Windows.Forms.Label();
            this.inEmergencyMeetingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "isImposter:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "MODE:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // isImposterLabel
            // 
            this.isImposterLabel.AutoSize = true;
            this.isImposterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isImposterLabel.Location = new System.Drawing.Point(101, 10);
            this.isImposterLabel.Name = "isImposterLabel";
            this.isImposterLabel.Size = new System.Drawing.Size(0, 20);
            this.isImposterLabel.TabIndex = 2;
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeLabel.Location = new System.Drawing.Point(101, 40);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(0, 20);
            this.modeLabel.TabIndex = 3;
            // 
            // inEmergencyMeetingLabel
            // 
            this.inEmergencyMeetingLabel.AutoSize = true;
            this.inEmergencyMeetingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inEmergencyMeetingLabel.Location = new System.Drawing.Point(34, 70);
            this.inEmergencyMeetingLabel.Name = "inEmergencyMeetingLabel";
            this.inEmergencyMeetingLabel.Size = new System.Drawing.Size(0, 20);
            this.inEmergencyMeetingLabel.TabIndex = 4;
            // 
            // BotStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 139);
            this.Controls.Add(this.inEmergencyMeetingLabel);
            this.Controls.Add(this.modeLabel);
            this.Controls.Add(this.isImposterLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BotStatus";
            this.Text = "BotStatus";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label isImposterLabel;
        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.Label inEmergencyMeetingLabel;
    }
}