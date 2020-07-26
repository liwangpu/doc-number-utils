namespace Utils
{
    partial class DocUtil
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnSourceUpload = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据源:";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(65, 16);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(184, 21);
            this.txtSourcePath.TabIndex = 1;
            // 
            // btnSourceUpload
            // 
            this.btnSourceUpload.Location = new System.Drawing.Point(255, 14);
            this.btnSourceUpload.Name = "btnSourceUpload";
            this.btnSourceUpload.Size = new System.Drawing.Size(75, 23);
            this.btnSourceUpload.TabIndex = 2;
            this.btnSourceUpload.Text = "浏览";
            this.btnSourceUpload.UseVisualStyleBackColor = true;
            this.btnSourceUpload.Click += new System.EventHandler(this.btnSourceUpload_Click);
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(255, 43);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(75, 23);
            this.btnTransfer.TabIndex = 2;
            this.btnTransfer.Text = "数据处理";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // Util
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 106);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.btnSourceUpload);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.label1);
            this.Name = "Util";
            this.ShowIcon = false;
            this.Text = "档号小工具";
            this.Load += new System.EventHandler(this.Util_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Button btnSourceUpload;
        private System.Windows.Forms.Button btnTransfer;
    }
}

