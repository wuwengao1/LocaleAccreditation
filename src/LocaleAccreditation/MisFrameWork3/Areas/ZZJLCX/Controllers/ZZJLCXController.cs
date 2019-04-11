using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MisFrameWork3.Areas.ZZJLCX.Controllers
{
    public class ZZJLCXController : FWBaseController
    {
        // GET: ZZJLCX/ZZJLCX
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult JsonConditionCombinationInfo()
        {
            return View();
        }

        public ActionResult JsonDataList()//业务主界面数据查询函数
        {
            //接收的基本查询参数有： id,limit,offset,search,sort,order            
            //__TIPS__*:根据表结构，修改以下函数的参数

            Condition cdtIds = new Condition();
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            if (!Membership.CurrentUser.HaveAuthority("ZZJL.ZZJLCX.QUERY_ALL_ZZJL"))
            {
                string COMPANY_ID = Membership.CurrentUser.CompanyId.ToString();
                char[] c = COMPANY_ID.ToCharArray();
                string comId = "";
                bool temp = false;
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    string cc = c[i].ToString();
                    if (cc != "0" && !temp)
                    {
                        temp = true;
                    }
                    if (temp)
                    {
                        comId += c[i];
                    }
                }
                char[] charArray = comId.ToCharArray();
                Array.Reverse(charArray);
                string comId2 = new String(charArray);
                comId2 += "%";
                cdtIds.AddSubCondition("AND", "ZZXXZZDW", "like", comId2);
            }
            if (Membership.CurrentUser.HaveAuthority("ZZJL.ZZJLCX.QUERY_OWN_ZZJL") && RoleLevel != 0)
            {
                string userId = Membership.CurrentUser.UserId;
                
                Condition cdtId = new Condition();
                cdtId.AddSubCondition("AND", "SBFZR", "=", userId);
                List<UnCaseSenseHashTable>  record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId, "MACHINENO", null, null, -1, -1);
                
                string zzjbh = "(";
                for (int i = 0; i < record.Count; i++)
                {
                    zzjbh += "'" + record[i]["MACHINENO"] + "',";
                }
                zzjbh += "'0')";
                cdtIds.AddSubCondition("AND", "ZZSBID", "in", "EXPR:" + zzjbh);
            }
            return QueryDataFromEasyUIDataGrid("C_JZZ_TMP", "ZZXXZZRQ", "SLBH,ZZZXPH,GMSFHM,ZZSBID,ZZXXZZDW,ZZXXZZDWMC", cdtIds, "*");
        }


        #region 打印数据
        public FileResult ActionPrint(string name, string oject_id)
        {
            //获取数据
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            Condition cdtIds = new Condition();
            string search = Request["Search"];
            string date_range_type = Request["date_range_type"];
            string start_date = Request["start_date"];
            string end_date = Request["end_date"];
            Condition cdtIds2 = new Condition();
            if (!string.IsNullOrEmpty(search))
            {
                cdtIds2.AddSubCondition("OR", "SLBH", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "ZZZXPH", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "GMSFHM", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "ZZSBID", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "ZZXXZZDW", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "ZZXXZZDWMC", "like", "%" + search + "%");
            }

            cdtIds2.Relate = "AND";
            cdtIds.AddSubCondition(cdtIds2);
            if (!string.IsNullOrEmpty(date_range_type) && date_range_type != "0" && (!string.IsNullOrEmpty(start_date) || !string.IsNullOrEmpty(end_date)))
            {
                if (!string.IsNullOrEmpty(start_date))
                {
                    cdtIds.AddSubCondition("AND", "ZZXXZZRQ", ">=", DateTime.Parse(start_date));
                }
                if (!string.IsNullOrEmpty(end_date))
                {
                    DateTime dtEndDate = DateTime.Parse(end_date);
                    dtEndDate = dtEndDate.AddDays(1);//加多一天
                    cdtIds.AddSubCondition("AND", "ZZXXZZRQ", "<=", dtEndDate);
                }
            }

            if (Request["cdt_combination"] != null)
            {
                string cdt = Request["cdt_combination"].ToString();
                string jsoncdtCombination = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(cdt));
                Condition cdtCombination = Condition.LoadFromJson(jsoncdtCombination);
                cdtCombination.Relate = "AND";
                ReplaceCdtCombinationOpreate(cdtCombination);
                cdtIds.AddSubCondition(cdtCombination);
            }

            if (!Membership.CurrentUser.HaveAuthority("SYS.USER.QUERY_ALL_USER"))
            {
                string COMPANY_ID = Membership.CurrentUser.CompanyId.ToString();
                char[] c = COMPANY_ID.ToCharArray();
                string comId = "";
                bool temp = false;
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    string cc = c[i].ToString();
                    if (cc != "0" && !temp)
                    {
                        temp = true;
                    }
                    if (temp)
                    {
                        comId += c[i];
                    }
                }
                char[] charArray = comId.ToCharArray();
                Array.Reverse(charArray);
                string comId3 = new String(charArray);
                comId3 += "%";
                cdtIds.AddSubCondition("AND", "ZZXXZZDW", "like", comId3);
            }
            if (Membership.CurrentUser.HaveAuthority("ZZJL.ZZJLCX.QUERY_OWN_ZZJL") && RoleLevel != 0)
            {
                string userId = Membership.CurrentUser.UserId;

                Condition cdtId = new Condition();
                cdtId.AddSubCondition("AND", "SBFZR", "=", userId);
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId, "MACHINENO", null, null, -1, -1);

                string zzjbh = "(";
                for (int i = 0; i < record.Count; i++)
                {
                    zzjbh += "'" + record[i]["MACHINENO"] + "',";
                }
                zzjbh += "'0')";
                cdtIds.AddSubCondition("AND", "ZZSBID", "in", "EXPR:" + zzjbh);
            }
            List<UnCaseSenseHashTable> records = DbUtilityManager.Instance.DefaultDbUtility.Query("C_JZZ_TMP", cdtIds, "*", null, null, -1, -1);

            //设置打印图纸大小
            Document document = new Document(PageSize.A4);
            //设置页边距
            document.SetMargins(36, 36, 36, 60);
            //中文字体
            string chinese = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "SIMSUN.TTC,1");
            BaseFont baseFont = BaseFont.CreateFont(chinese, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //文字大小12，文字样式
            Font cn = new Font(baseFont, 14, Font.NORMAL);

            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"D:\temp.pdf", FileMode.Create));

            //这样写：是生成文件到内存中去
            var memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);//生成到内存中
            //writer.PageEvent = new PdfPageHelper();//页脚
            document.Open();//打开文件


            //Paragraph title = new Paragraph("国家工作人员登记备案表", new Font(baseFont, 23, Font.BOLD, BaseColor.BLACK));
            Paragraph title = new Paragraph("", new Font(baseFont, 23, Font.BOLD, BaseColor.BLACK));
            title.Alignment = Element.ALIGN_CENTER; //居中
            title.SpacingAfter = 20;
            document.Add(title);

            //数据表格
            PdfPTable table = new PdfPTable(9);
            table.SetWidths(new float[] { 2.5F, 8, 8, 12, 12, 7, 6, 6, 6 });
            table.WidthPercentage = 100;
            AddBodyContentCell(table, "序号", cn);
            AddBodyContentCell(table, "受理编号", cn);
            AddBodyContentCell(table, "制证芯片号", cn);
            AddBodyContentCell(table, "身份号码", cn);
            AddBodyContentCell(table, "制证设备编号", cn);
            AddBodyContentCell(table, "制证单位代码", cn);
            AddBodyContentCell(table, "制证单位名称", cn);
            AddBodyContentCell(table, "制证时间", cn);
            AddBodyContentCell(table, "状态", cn);

            for (int i = 0; i < records.Count; i++)
            {
                UnCaseSenseHashTable record = records[i];
                AddBodyContentCell(table, Convert.ToString(i + 1), cn);
                if (!string.IsNullOrEmpty((string)record["SLBH"]))
                {
                    AddBodyContentCell(table, record["SLBH"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["ZZZXPH"]))
                {
                    AddBodyContentCell(table, record["ZZZXPH"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty((string)record["GMSFHM"]))
                {
                    AddBodyContentCell(table, record["GMSFHM"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["ZZSBID"]))
                {
                    AddBodyContentCell(table, record["ZZSBID"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["ZZXXZZDW"]))
                {
                    AddBodyContentCell(table, record["ZZXXZZDW"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }


                if (!string.IsNullOrEmpty((string)record["ZZXXZZDWMC"]))
                {
                    AddBodyContentCell(table, record["ZZXXZZDWMC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }


                if (record["ZZXXZZRQ"] != null)
                {
                    if (!string.IsNullOrEmpty(record["ZZXXZZRQ"].ToString()))
                    {
                        string s = record["ZZXXZZRQ"].ToString();
                        string date = s.Substring(0, 8);
                        AddBodyContentCell(table, date, cn);
                    }
                    else
                    {
                        AddBodyContentCell(table, "", cn);
                    }

                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                

                if (!string.IsNullOrEmpty((string)record["DISABLED"]))
                {
                    if (record["DISABLED"].ToString() == "0")
                        AddBodyContentCell(table, "成功", cn);
                    else
                        AddBodyContentCell(table, "失败", cn);
                }
                else
                {
                    AddBodyContentCell(table, "未知", cn);
                }

            }
            document.Add(table);

            document.Close();

            var bytes = memoryStream.ToArray();
            //result = Convert.ToBase64String(bytes);

            return File(bytes, "application/pdf");
        }

        private void AddBodyContentCell(PdfPTable bodyTable, String text, iTextSharp.text.Font font, int rowspan = 2, bool needRightBorder = false)
        {
            PdfPCell cell = new PdfPCell();
            //float defaultBorder = 0.5f;
            //cell.BorderWidthLeft = defaultBorder;
            //cell.BorderWidthTop = 0;
            //cell.BorderWidthRight = needRightBorder ? defaultBorder : 0;
            //cell.BorderWidthBottom = defaultBorder;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BASELINE;
            //cell.Rowspan = rowspan;
            cell.PaddingBottom = 3;
            cell.Phrase = new Phrase(text, font);
            bodyTable.AddCell(cell);
        }
        #endregion
    }
}