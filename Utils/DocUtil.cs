using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public partial class DocUtil : Form
    {
        public DocUtil()
        {
            InitializeComponent();
        }

        private void Util_Load(object sender, EventArgs e)
        {
            //txtSourcePath.Text = @"C:\Users\Leon\Desktop\档号\zpda_DrMbGZ2(1).xls";
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
                btnTransfer.Enabled = true;
                ShowMsg("表格已经上传,请进行数据处理");
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            var list = new List<DocNumberMapping>();
            var readData = new Action(() =>
            {
                if (!string.IsNullOrEmpty(txtSourcePath.Text))
                {
                    ShowMsg("数据处理中,请稍后");
                    HSSFWorkbook hssfwb;
                    using (FileStream file = new FileStream(txtSourcePath.Text, FileMode.Open, FileAccess.Read))
                    {
                        hssfwb = new HSSFWorkbook(file);
                    }
                    ISheet sheet = hssfwb.GetSheetAt(0);
                    sheet.GetRow(0).GetCell(15).SetCellValue("档号");
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        var currentRow = sheet.GetRow(row);
                        if (currentRow != null)
                        {
                            var doc = new DocNumberMapping();
                            var c1 = sheet.GetRow(row).GetCell(1);
                            if (c1 != null)
                                doc._张号 = c1.NumericCellValue;
                            var c4 = sheet.GetRow(row).GetCell(4);
                            if (c4 != null)
                                doc._全宗号 = c4.NumericCellValue;
                            var c5 = sheet.GetRow(row).GetCell(5);
                            if (c5 != null)
                                doc._年度 = c5.NumericCellValue;
                            var c7 = sheet.GetRow(row).GetCell(7);
                            if (c7 != null)
                                doc._业务类别 = c7.StringCellValue;
                            var c15 = currentRow.CreateCell(15);
                            c15.SetCellValue(doc._档号);
                            list.Add(doc);
                        }
                    }
                    sheet.AutoSizeColumn(15);

                    using (FileStream file = new FileStream(txtSourcePath.Text, FileMode.Open, FileAccess.Write))
                    {
                        hssfwb.Write(file);
                    }
                }
            });

            readData.BeginInvoke((obj) =>
            {
                GenerateFolder(list.Select(x => x._档号).Distinct().ToList());
            }, null);
        }

        private void GenerateFolder(List<string> folders)
        {

            InvokeMainForm((obj) =>
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    var dir = folderDialog.SelectedPath;
                    txtDocFolder.Text = dir;
                    folders.ForEach(doc =>
                    {
                        var p = Path.Combine(dir, doc);
                        if (!Directory.Exists(p))
                            Directory.CreateDirectory(p);
                        ShowMsg("档号生成完毕");
                    });


                }

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

        #region ShowMsg 消息提示
        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="strMsg"></param>
        private void ShowMsg(string strMsg)
        {
            if (this.InvokeRequired)
            {
                var act = new Action<string>(ShowMsg);
                this.Invoke(act, strMsg);
            }
            else
            {
                this.lbMsg.Text = strMsg;
            }
        }
        #endregion

    }

    public class DocNumberMapping
    {
        public double _张号 { get; set; }
        public double _全宗号 { get; set; }
        public double _年度 { get; set; }
        public string _业务类别 { get; set; }
        public string _档号
        {
            get
            {
                StringBuilder bd = new StringBuilder();
                if (_全宗号 > 0)
                {
                    bd.Append($"{_全宗号}-");
                }
                bd.Append("ZP-");
                bd.Append($"{_年度}-");
                switch (_业务类别)
                {
                    case "会务活动":
                        bd.Append("hw-");
                        break;
                    case "基建项目":
                        bd.Append("jj-");
                        break;
                    case "实物":
                        bd.Append("sw-");
                        break;
                }
                bd.Append(_张号.ToString().PadLeft(4, '0'));
                return bd.ToString();
            }
        }
    }
}
