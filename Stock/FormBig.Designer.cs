﻿namespace Stock
{
	partial class FormBig
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
			this.button1 = new System.Windows.Forms.Button();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.button2 = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(124, 453);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(105, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "強勢股買進";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// webBrowser1
			// 
			this.webBrowser1.Location = new System.Drawing.Point(123, 27);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(1025, 407);
			this.webBrowser1.TabIndex = 1;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(124, 482);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(104, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "強勢股賣出";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// listView1
			// 
			this.listView1.Location = new System.Drawing.Point(10, 27);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(105, 474);
			this.listView1.TabIndex = 3;
			this.listView1.UseCompatibleStateImageBehavior = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(124, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "label1";
			// 
			// FormBig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1167, 513);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.webBrowser1);
			this.Controls.Add(this.button1);
			this.Name = "FormBig";
			this.Text = "FormBig";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Label label1;
	}
}