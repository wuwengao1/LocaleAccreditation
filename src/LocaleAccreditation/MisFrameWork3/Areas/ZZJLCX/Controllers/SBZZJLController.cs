using iTextSharp.text;
using iTextSharp.text.pdf;
using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MisFrameWork3.Areas.ZZJLCX.Controllers
{
    public class SBZZJLController : FWBaseController
    {
        // GET: ZZJLCX/SBZZJL
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetSelect()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            Condition cdtId;
            if (RoleLevel != 0)
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
                cdtId = new Condition("AND", "DM", "like", comId3);

                Condition cdtId2 = new Condition();
                cdtId2.AddSubCondition("AND", "SSDW", "like", comId3);
                cdtId2.AddSubCondition("AND", "DELETED_MARK", "=", "0");
                List<UnCaseSenseHashTable> sb = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId2, "MACHINENO as DM", null, null, -1, -1);
                List<UnCaseSenseHashTable> dw = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", cdtId, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { sb = sb, dw = dw });
            }
            else
            {
                List<UnCaseSenseHashTable> sb = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", null, "MACHINENO as DM", null, null, -1, -1);
                List<UnCaseSenseHashTable> dw = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", null, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { sb = sb, dw = dw });
            }
        }

        #region __TIPS__:设备制证数量统计
        public ActionResult AcceptStat()
        {

            string ZZSBID = Request["MACHINENO"];
            string SSDW = Request["SSDW"];
            string ACCEPT_DATE = Request["ACCEPT_DATE"];
            string ACCEPT_DATE2 = Request["ACCEPT_DATE2"];

            string zzys = "";
            //根据单位先获取单位下的制证员，然后根据制证员id查询与制证员关联的设备编号，最后根据设备编号查询制证记录
            if (!string.IsNullOrEmpty(SSDW))
            {
                char[] c = SSDW.ToCharArray();
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

                Condition cdtId3 = new Condition();
                cdtId3.AddSubCondition("AND", "SSDW", "like", comId2);
                List<UnCaseSenseHashTable> re = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId3, "MACHINENO", null, null, -1, -1);
                if (re.Count != 0)
                {
                    zzys = "(";
                    for (int i = 0; i < re.Count; i++)
                    {
                        zzys += "'" + re[i]["MACHINENO"] + "',";
                    }
                    zzys += "'0')";
                }
                else
                {
                    zzys = "('0')";
                }
            }

            string query_sql = "";
            if (string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(SSDW) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(SSDW) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " group by ZZSBID ";
            }

            else if (!string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(SSDW))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " and ZZSBID = '" + ZZSBID + "' group by ZZSBID ";
            }
            else if (!string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + ZZSBID + "' group by ZZSBID ";
            }

            else if (string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (!string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + "  and ZZSBID = '" + ZZSBID + "'and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (!string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + "  and ZZSBID = '" + ZZSBID + "'and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (!string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " and ZZSBID = '" + ZZSBID + "' and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (!string.IsNullOrEmpty(ZZSBID) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + ZZSBID + "' and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (!string.IsNullOrEmpty(ZZSBID) && !string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(SSDW))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + ZZSBID + "' and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else 
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + zzys + " and ZZSBID = '" + ZZSBID + "' and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            try
            {
                using (OracleConnection conn = new OracleConnection("data source=192.168.0.3/orcl;User Id=jzz_gx;Password=gx_dba;"))
                //using (OracleConnection conn = new OracleConnection("data source=127.0.0.1/WWGORCL;User Id=locale;Password=123;"))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = query_sql;
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            reader.GetString(0);
                        }
                        OracleDataAdapter ada = new OracleDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        ada.Fill(ds);
                        var s = ds.Tables[0];
                    }
                }
            }
            catch(Exception ee)
            {
                return Json(new { success = false, message = ee.Message }, JsonRequestBehavior.AllowGet);
            }
            List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);
            return JsonDateObject(record);
        }
        #endregion

        #region 打印数据
        public FileResult ActionPrint(string name, string oject_id)
        {
            //获取数据
            string MACHINENO = Request["MACHINENO"];
            string SSDW = Request["SSDW"];
            Condition cdtIds = new Condition();
            if (!String.IsNullOrEmpty(MACHINENO))
            {
                cdtIds.AddSubCondition("AND", "MACHINENO", "like", "%" + MACHINENO + "%");
            }
            if (!String.IsNullOrEmpty(SSDW))
            {
                cdtIds.AddSubCondition("AND", "SSDW", "=", SSDW);
            }
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            if (RoleLevel != 0)
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
                cdtIds.AddSubCondition("AND", "SSDW", "like", comId3);
                cdtIds.AddSubCondition("AND", "DELETED_MARK", "=", "0");
            }
            List<UnCaseSenseHashTable> records = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtIds, "*", null, null, -1, -1);

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
            PdfPTable table = new PdfPTable(6);
            table.SetWidths(new float[] { 2.5F, 8, 8, 12, 7, 6 });
            table.WidthPercentage = 100;
            AddBodyContentCell(table, "序号", cn);
            AddBodyContentCell(table, "设备编号", cn);
            AddBodyContentCell(table, "所属单位编号", cn);
            AddBodyContentCell(table, "所属单位名称", cn);
            AddBodyContentCell(table, "负责人名称", cn);
            AddBodyContentCell(table, "是否启用", cn);

            for (int i = 0; i < records.Count; i++)
            {
                UnCaseSenseHashTable record = records[i];
                AddBodyContentCell(table, Convert.ToString(i + 1), cn);
                if (!string.IsNullOrEmpty((string)record["MACHINENO"]))
                {
                    AddBodyContentCell(table, record["MACHINENO"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["SSDW"]))
                {
                    AddBodyContentCell(table, record["SSDW"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty((string)record["SSDW_V_D_FW_COMP__MC"]))
                {
                    AddBodyContentCell(table, record["SSDW_V_D_FW_COMP__MC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["SBFZR_V_D_FW_S_USERS__MC"]))
                {
                    AddBodyContentCell(table, record["SBFZR_V_D_FW_S_USERS__MC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty(record["DISABLED"].ToString()))
                {
                    if (record["DISABLED"].ToString() == "0")
                        AddBodyContentCell(table, "启用", cn);
                    else
                        AddBodyContentCell(table, "禁用", cn);
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