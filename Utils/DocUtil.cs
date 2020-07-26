using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EpplusHelper;
using OfficeOpenXml;

namespace Utils
{
    public partial class DocUtil : Form
    {
        protected string tmpXlsxPath;
        public DocUtil()
        {
            InitializeComponent();
        }

        private void Util_Load(object sender, EventArgs e)
        {
            txtSourcePath.Text = @"C:\Users\Leon\Desktop\工作簿1.xls";
            tmpXlsxPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
            XLSToXLSXConverter.Convert(txtSourcePath.Text, tmpXlsxPath);
        }

        private void btnSourceUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = "Excel 工作簿|*.xls";//设置文件类型
            OpenFileDialog1.Title = "表格信息";//设置标题
            OpenFileDialog1.Multiselect = false;
            OpenFileDialog1.AutoUpgradeEnabled = true;//是否随系统升级而升级外观
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)//如果点的是确定就得到文件路径
            {
                txtSourcePath.Text = OpenFileDialog1.FileName;
                tmpXlsxPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
                XLSToXLSXConverter.Convert(txtSourcePath.Text, tmpXlsxPath);
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            var source = new List<DocNumberMapping>();
            var readData = new Action(() =>
            {
                if (!string.IsNullOrEmpty(tmpXlsxPath))
                {
                    using (var package = new ExcelPackage(new FileInfo(tmpXlsxPath)))
                    {
                        var worksheet = package.Workbook.Worksheets[1];
                        source = SheetReader<DocNumberMapping>.From(worksheet);
                    }
                }
            });

            readData.BeginInvoke((obj) =>
            {

            }, null);
        }

        #region InvokeMainForm 调用主线程
        protected void InvokeMainForm(Action act)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }
        protected void InvokeMainForm(Action<object> act, object obj)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(act, obj);
            }
            else
            {
                act.Invoke(obj);
            }
        }
        #endregion

    }

    public class DocNumberMapping
    {
        [ExcelColumn("保管期限")]
        public string _保管期限 { get; set; }

    }
}
