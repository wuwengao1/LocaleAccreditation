﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;

namespace MisFrameWork3.Areas.Machine.Controllers
{
    public class MachineController : FWBaseController
    {
        public ActionResult Index()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            ViewBag.RoleLevel = RoleLevel;
            if (RoleLevel != 0)
            {
                ViewBag.ShowChangeStateButton = false;
                ViewBag.ShowDeleteButton = false;
                ViewBag.DisableBTN_Add = true;
            }
            else
            {
                ViewBag.ShowChangeStateButton = true;
                ViewBag.ShowDeleteButton = true;
                ViewBag.DisableBTN_Add = false;
            }
            return View();
        }

        public ActionResult ViewFormAdd()
        {
            return View();
        }
        public ActionResult ViewFormEdit()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            ViewBag.RoleLevel = RoleLevel;
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
                Condition cdtId2 = new Condition();
                cdtId2.AddSubCondition("AND", "DELETED_MARK", "=", "0");
                List<UnCaseSenseHashTable> sb = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", null, "MACHINENO as DM", null, null, -1, -1);
                List<UnCaseSenseHashTable> dw = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", null, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { sb = sb, dw = dw });
            }
        }

        #region __TIPS__:框架通用函数( 增 删 改)
        public ActionResult JsonDataList()//业务主界面数据查询函数
        {
            //接收的基本查询参数有： id,limit,offset,search,sort,order            
            //__TIPS__*:根据表结构，修改以下函数的参数CompanyId
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
            return QueryDataFromEasyUIDataGrid("B_MACHINE", null, "*", cdtIds, "*");
        }
        
        public ActionResult ActionAdd()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                /*
                __TIPS__*: 区取表相关的字段信息的方式有两种：
                    1、加载窗体提交的数据可以使用LoadFromNameValueCollection 结合正则表达式过滤掉没有用的数据
                        比如：data.LoadFromNameValueCollection(Request.Form, "NAME|TYPE|COMPANY_CODE",true);
                        这样只加载NAME、TYPE、COMPANY_CODE 这几项，其它项不处理
                    2、获取表信息，然后加只加载与表字段同名的内容，这个方法最常用，比如这样：
                        ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("FW_S_COMAPANIES");
                        data.LoadFromNameValueCollection(Request.Form, ti, true);
                    通过以上方式，data 里可以保留业务所需的数据。
                    因止，下面的内容只需要修改表名即可完成数据库操作。
                */
                ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("B_MACHINE");
                data.LoadFromNameValueCollection(Request.Unvalidated.Form, ti, true);//使用Request.Unvalidated.Form可以POST HTML标签数据。
                session.BeginTransaction();
                int r = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "B_MACHINE", data);
                if (!string.IsNullOrEmpty(data["SBFZR"].ToString()))
                {
                    Condition cdtId3 = new Condition();
                    cdtId3.AddSubCondition("AND", "ZZY_ID", "=", data["SBFZR"].ToString());
                    cdtId3.AddSubCondition("AND", "MACHINENO", "=", data["MACHINENO"].ToString());
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(session, "B_MACHINE_ZZY", cdtId3, "*", null, null, -1, -1);
                    if (record.Count == 0)
                    {
                        UnCaseSenseHashTable data2 = new UnCaseSenseHashTable();
                        data2["MACHINENO"] = data["MACHINENO"].ToString();
                        data2["ZZY_ID"] = data["SBFZR"].ToString();
                        int re = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "B_MACHINE_ZZY", data);
                    }
                }
                session.Commit();
                session.Close();
                if (0 == r)
                {
                    return Json(new { success = false, message = "保存信息时出错！" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                session.Rollback();
                session.Close();
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            var result = new { success = true, message = "保存成功" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionEdit()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                //__TIPS__*:这里修改表名，参考ActionAdd 
                ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("B_MACHINE");
                data.LoadFromNameValueCollection(Request.Unvalidated.Form, ti, true);//使用Request.Unvalidated.Form可以POST HTML标签数据。
                data["ID"] = Request["OBJECT_ID"];//这ID字段是加载不进来的。  
                session.BeginTransaction();
                int r = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session, "B_MACHINE", data, false);
                if (!string.IsNullOrEmpty(data["SBFZR"].ToString()))
                {
                    Condition cdtId3 = new Condition();
                    cdtId3.AddSubCondition("AND", "ZZY_ID", "=", data["SBFZR"].ToString());
                    cdtId3.AddSubCondition("AND", "MACHINENO", "=", data["MACHINENO"].ToString());
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(session, "B_MACHINE_ZZY", cdtId3, "*", null, null, -1, -1);
                    if (record.Count == 0)
                    {
                        UnCaseSenseHashTable data2 = new UnCaseSenseHashTable();
                        data2["MACHINENO"] = data["MACHINENO"].ToString();
                        data2["ZZY_ID"] = data["SBFZR"].ToString();
                        int re = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "B_MACHINE_ZZY", data2);
                    }
                }
                session.Commit();
                session.Close();
                if (0 == r)
                {
                    return Json(new { success = false, message = "保存信息时出错！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                session.Rollback();
                session.Close();
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            var result = new { success = true, message = "保存成功" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActionDelete()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            int id = 0;
            try
            {
                id = int.Parse(Request["OBJECT_ID"]);
            }
            catch (Exception ee)
            {
                return Json(new { success = false, message = "主键值有误【" + id + "】" }, JsonRequestBehavior.AllowGet);
            }

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                //__TIPS__*:这里修改表名，参考ActionAdd
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  B_MACHINE set DELETED_MARK = 1 where ID=" + id.ToString());
                session.Commit();
                session.Close();
            }
            catch (Exception e)
            {
                session.Rollback();
                session.Close();
                var eResult = new { success = false, message = e.ToString() };
                return Json(eResult, JsonRequestBehavior.AllowGet); ;
            }

            var result = new { success = true, message = "删除成功" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActionChangeState()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            int id = 0;
            int state = 0;
            try
            {
                id = int.Parse(Request["OBJECT_ID"]);
                state = int.Parse(Request["state"]);
            }
            catch (Exception ee)
            {
                return Json(new { success = false, message = "主键值有误【" + id + "】" }, JsonRequestBehavior.AllowGet);
            }

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                //__TIPS__*:这里修改表名，参考ActionAdd
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  B_MACHINE set DISABLED=" + state + " where ID=" + id.ToString());
                session.Commit();
                session.Close();
            }
            catch (Exception e)
            {
                session.Rollback();
                session.Close();
                var eResult = new { success = false, message = e.ToString() };
                return Json(eResult, JsonRequestBehavior.AllowGet); ;
            }
            return Json(new { success = true, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region __TIPS__:框架通用函数 ( 字典控件相关 )
        public ActionResult JsonDicShort()
        {
            //__TIPS__:这里可以先过滤一下业务允许使用什么字典
            List<UnCaseSenseHashTable> records = GetDicData(Request["dic"], null);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JsonDicLarge()
        {
            //__TIPS__:这里可以先过滤一下业务允许使用什么字典
            if ("V_D_FW_S_USERS".Equals(Request["dic"]))
            {
                int RoleLevel = Membership.CurrentUser.RoleLevel;
                Condition cdtId2 = new Condition("AND", "ROLES_ID", "=", 1000);
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
                    cdtId2.AddSubCondition("AND", "COMPANY_ID", "like", comId3);
                    return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC", cdtId2, "*");
                }
                else
                {

                    return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC", cdtId2, "*");
                }
            }
            else
            {
                return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC", null, "*");
            }

        }

        public ActionResult ViewDicLargeUI()
        {
            /*
             * __TIPS__:有些特殊的字典可能需要显示更多的东西所以这里可以根据Request的值返回不同的视图
             *          以下演示根据字典内容，返回不同的视图。
             * */
            
            if ("V_D_FW_COMP".Equals(Request["dic"]))
                return View("ViewSSDW");
            else if ("V_D_FW_S_USERS".Equals(Request["dic"]))
                return View("ViewSBFZR");
            else
                return View("~/Views/Shared/ViewCommonDicUI.cshtml");
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
                    AddBodyContentCell(table, "",  cn);
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