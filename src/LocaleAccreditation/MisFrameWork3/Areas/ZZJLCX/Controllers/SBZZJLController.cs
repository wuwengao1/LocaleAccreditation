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
        public ActionResult JsonConditionCombinationInfo()
        {
            return View();
        }

        #region __TIPS__:设备制证数量统计
        public ActionResult AcceptStat()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            string sql = "";
            if (Request["cdt_combination"] != null)
            {
                if (!Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_ALL_ZZJL"))
                {
                    sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in  " + GetMachineNo();
                }
                else
                {
                    sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where 1=1 ";
                }
                string jsoncdtCombination = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(Request["cdt_combination"]));
                Condition cdtCombination = Condition.LoadFromJson(jsoncdtCombination);
                cdtCombination.Relate = "AND";
                ReplaceCdtCombinationOpreate(cdtCombination);
                int count = cdtCombination.SubConditions.Count;
                if (count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Condition c = cdtCombination.SubConditions[i];
                        if (c.Src == "ZZXXZZRQ")
                        {
                            sql += " " + c.Relate + " " + c.Src + " " + c.Op + "  TO_DATE('" + c.Tag + "', 'YYYY-MM-DD HH24:MI:SS') ";
                        }
                        else
                        {
                            sql += " " + c.Relate + " " + c.Src + " " + c.Op + " '" + c.Tag + "'";
                        }
                    }
                }
                if (Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_OWN_ZZJL") && RoleLevel != 0)
                {
                    string userId = Membership.CurrentUser.UserId;
                    sql += " AND ZZSBID in (select MACHINENO from B_MACHINE where SBFZR = '" + userId + "')";
                }

                sql += " group by ZZSBID ";
            }
            if (!string.IsNullOrEmpty(sql))
            {
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(sql, -1, -1);
                return JsonDateObject(record);
            }
            else
            {
                string query_sql;
                string search = Request["search"];
                if (string.IsNullOrEmpty(search))
                {
                    if (!Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_ALL_ZZJL"))
                    {
                        query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in  " + GetMachineNo();
                    }
                    else
                    {
                        query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where 1=1 ";
                    }
                }
                else
                {
                    if (!Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_ALL_ZZJL"))
                    {
                        query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in  " + GetMachineNo() + " and ( ZZSBID like  '%" + search + "%' OR ZZXXZZDW like  '%" + search + "%' OR ZZXXZZDWMC like  '%" + search + "%') ";

                    }
                    else
                    {
                        query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where 1=1 and ( ZZSBID like  '%" + search + "%' OR ZZXXZZDW like  '%" + search + "%' OR ZZXXZZDWMC like  '%" + search + "%') ";
                    }
                }
                if (Request["date_range_type"] != null && (Request["start_date"] != null || Request["end_date"] != null))
                {
                    string fieldName = null;
                    int dataRangeTypeIndex = 0;
                    try
                    {
                        dataRangeTypeIndex = int.Parse(Request["date_range_type"]);
                    }
                    catch (Exception e)
                    {
                        dataRangeTypeIndex = 0;
                    }
                    string[] arrDataRangeFields = new string[] { "制证时间" };
                    if (dataRangeTypeIndex != 0)
                    {
                        fieldName = arrDataRangeFields[dataRangeTypeIndex - 1];
                        if (!String.IsNullOrEmpty(Request["start_date"]))
                        {
                            query_sql += " AND ZZXXZZRQ >=  TO_DATE('" + Request["start_date"].ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                        }
                        if (!String.IsNullOrEmpty(Request["end_date"]))
                        {
                            DateTime dtEndDate = DateTime.Parse(Request["end_date"]);
                            dtEndDate = dtEndDate.AddDays(1);//加多一天

                            query_sql += " AND ZZXXZZRQ <=  TO_DATE('" + dtEndDate.ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                        }
                    }
                }

                if (Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_OWN_ZZJL") && RoleLevel != 0)
                {
                    string userId = Membership.CurrentUser.UserId;
                    query_sql += " AND ZZSBID in (select MACHINENO from B_MACHINE where SBFZR = '" + userId + "')";
                }
                query_sql += " group by ZZSBID ";
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);
                return JsonDateObject(record);
            }
        }
        public ActionResult JsonDicShort()
        {
            //__TIPS__:这里可以先过滤一下业务允许使用什么字典
            List<UnCaseSenseHashTable> records = GetDicData(Request["dic"], null);
            return Json(records, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region 打印数据
        public FileResult ActionPrint(string name, string oject_id)
        {
            //获取数据
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            string QuerySql = "";
            string sql = "";
            if (!string.IsNullOrEmpty(Request["cdt_combination"]))
            {
                if (!Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_ALL_ZZJL"))
                {
                    sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in  " + GetMachineNo();
                }
                else
                {
                    sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where 1=1 ";
                }
                string a = Request["cdt_combination"];
                string jsoncdtCombination = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(Request["cdt_combination"]));
                Condition cdtCombination = Condition.LoadFromJson(jsoncdtCombination);
                cdtCombination.Relate = "AND";
                ReplaceCdtCombinationOpreate(cdtCombination);
                int count = cdtCombination.SubConditions.Count;
                if (count != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Condition c = cdtCombination.SubConditions[i];
                        if (c.Src == "ZZXXZZRQ")
                        {
                            sql += " " + c.Relate + " " + c.Src + " " + c.Op + "  TO_DATE('" + c.Tag + "', 'YYYY-MM-DD HH24:MI:SS') ";
                        }
                        else
                        {
                            sql += " " + c.Relate + " " + c.Src + " " + c.Op + " '" + c.Tag + "'";
                        }
                    }
                }
                if (Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_OWN_ZZJL") && RoleLevel != 0)
                {
                    string userId = Membership.CurrentUser.UserId;
                    sql += " AND ZZSBID in (select MACHINENO from B_MACHINE where SBFZR = '" + userId + "')";
                }
                sql += " group by ZZSBID ";
            }
            if (!string.IsNullOrEmpty(sql))
            {
                QuerySql = sql;
            }
            else
            {
                string search = Request["Search"];
                string date_range_type = Request["date_range_type"];
                string start_date = Request["start_date"];
                string end_date = Request["end_date"];
                string query_sql;
                if (!string.IsNullOrEmpty(search))
                {
                    query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where 1=1 and ( ZZSBID like  '%" + search + "%' OR ZZXXZZDW like  '%" + search + "%' OR ZZXXZZDWMC like  '%" + search + "%') ";
                }
                else
                {
                    query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where 1=1 ";
                }

                if (!string.IsNullOrEmpty(date_range_type) && date_range_type != "0" && (!string.IsNullOrEmpty(start_date) || !string.IsNullOrEmpty(end_date)))
                {
                    if (!String.IsNullOrEmpty(start_date))
                    {
                        query_sql += " AND ZZXXZZRQ >=  TO_DATE('" + start_date + "', 'YYYY-MM-DD HH24:MI:SS') ";
                    }
                    if (!String.IsNullOrEmpty(end_date))
                    {
                        DateTime dtEndDate = DateTime.Parse(end_date);
                        dtEndDate = dtEndDate.AddDays(1);//加多一天

                        query_sql += " AND ZZXXZZRQ <=  TO_DATE('" + dtEndDate.ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                    }
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
                    query_sql += " AND ZZXXZZDW like '" + comId3 + "'";
                }
                if (Membership.CurrentUser.HaveAuthority("ZZJL.MACHINEZZTJ.QUERY_OWN_ZZJL") && RoleLevel != 0)
                {
                    string userId = Membership.CurrentUser.UserId;
                    query_sql += " AND ZZSBID in (select MACHINENO from B_MACHINE where SBFZR = '" + userId + "')";
                }
                query_sql += " group by ZZSBID";
                QuerySql = query_sql;
            }


            List<UnCaseSenseHashTable> records = DbUtilityManager.Instance.DefaultDbUtility.Query(QuerySql, -1, -1);

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
            PdfPTable table = new PdfPTable(3);
            table.SetWidths(new float[] { 2.5F, 8, 8});
            table.WidthPercentage = 100;
            AddBodyContentCell(table, "序号", cn);
            AddBodyContentCell(table, "设备编号", cn);
            AddBodyContentCell(table, "数量", cn);

            for (int i = 0; i < records.Count; i++)
            {
                UnCaseSenseHashTable record = records[i];
                AddBodyContentCell(table, Convert.ToString(i + 1), cn);
                if (!string.IsNullOrEmpty((string)record["ZZSBID"]))
                {
                    AddBodyContentCell(table, record["ZZSBID"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                
                if (record["COUNT"] != null)
                {
                    if (!string.IsNullOrEmpty(record["COUNT"].ToString()))
                    {
                        AddBodyContentCell(table, record["COUNT"].ToString(), cn);
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