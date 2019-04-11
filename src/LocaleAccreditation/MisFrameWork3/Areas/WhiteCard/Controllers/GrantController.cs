﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.IO;
using System.Globalization;
using System.IO.Compression;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;
using MisFrameWork3.Classes.Authorize;
using System.Web.Script.Serialization;
//using AutoUpdateWeb.Controllers;
using System.Web.Configuration;
using AutoUpdateWeb.Class;

namespace MisFrameWork3.Areas.WhiteCard.Controllers
{
    public class GrantController : FWBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JsonConditionGrant()
        {
            return View();
        }
        #region __TIPS__:框架通用函数( 增 删 改)
        public ActionResult JsonDataList()//业务主界面数据查询函数
        {
            Condition cdtId = new Condition();
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
                cdtId.AddSubCondition("AND", "EXTEND_ID", "like", comId3);
            }
            return QueryDataFromEasyUIDataGrid("B_CARD_GRANT", "RECEIVE_TIME,EXTEND", "EXTEND_ID,RECEIVE_NUMBER,RECEIVE_PHONE,RECEIVE_TIME,EXTEND_NAME,RECEIVE_NAME,EXTEND_ID_V_D_FW_COMP__MC,RECEIVE_ID_V_D_FW_COMP__MC,RECEIVE_ID", cdtId, "*");
        }
        public ActionResult ViewFormAdd()
        {
            return View();
        }

        public ActionResult ViewFormEdit()
        {
            return View();
        }

        public ActionResult ActionAdd()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            string COMPANY_ID = Membership.CurrentUser.CompanyId.ToString();
            string COMPANY_NAME = Membership.CurrentUser.CompanyName.ToString();
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
                ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("B_CARD_GRANT");
                data.LoadFromNameValueCollection(Request.Unvalidated.Form, ti, true);//使用Request.Unvalidated.Form可以POST HTML标签数据。
                data["EXTEND_ID"] = COMPANY_ID;
                data["EXTEND_NAME"] = COMPANY_NAME;
                Int64 RECEIVE_ID = Convert.ToInt64(data["RECEIVE_ID"].ToString());
                Int64 EXTEND_ID = Convert.ToInt64(COMPANY_ID);
                if (EXTEND_ID < RECEIVE_ID)
                {
                    session.BeginTransaction();
                    int r = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "B_CARD_GRANT", data);
                    session.Commit();
                    session.Close();
                    //领卡人剩余库存和总库存增加
                    Condition cdtId_add = new Condition("AND", "COMPANY_ID", "=", data["RECEIVE_ID"]);
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_add, "*", null, null, -1, -1);
                    if (record.Count == 1)
                    {
                        int STOCK_WHOLE = Convert.ToInt32(record[0]["STOCK_WHOLE"].ToString());
                        int RECEIVE_NUMBER = Convert.ToInt32(data["RECEIVE_NUMBER"].ToString());
                        int STOCK_OVERPLUS = Convert.ToInt32(record[0]["STOCK_OVERPLUS"].ToString());
                        if (STOCK_OVERPLUS >= 0 && RECEIVE_NUMBER >= 0 && STOCK_WHOLE >= 0)
                        {
                            STOCK_OVERPLUS = STOCK_OVERPLUS + RECEIVE_NUMBER;
                            STOCK_WHOLE = STOCK_WHOLE + RECEIVE_NUMBER;
                            string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                            string number_whole = Convert.ToString(STOCK_WHOLE);
                            record[0]["STOCK_OVERPLUS"] = number_overplus;
                            record[0]["STOCK_WHOLE"] = number_whole;
                        }
                        UnCaseSenseHashTable data_add = new UnCaseSenseHashTable();
                        data_add["ID"] = record[0]["ID"];
                        data_add["STOCK_WHOLE"] = record[0]["STOCK_WHOLE"];
                        data_add["STOCK_OVERPLUS"] = record[0]["STOCK_OVERPLUS"];
                        data_add["INPUT_TIME"] = record[0]["INPUT_TIME"];
                        data_add["COMPANY_ID"] = record[0]["COMPANY_ID"];
                        data_add["COMPANY_ID_V_D_FW_COMP__MC"] = record[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                        Session session_number = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                        session_number.BeginTransaction();
                        int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_number, "B_CARD_STOCK", data_add, false);
                        session_number.Commit();
                        session_number.Close();
                    }
                    else
                    {
                        return Json(new { success = false, message = "数据库中无该单位！" }, JsonRequestBehavior.AllowGet);
                    }
                    //发卡人剩余库存减少
                    Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", data["EXTEND_ID"]);
                    List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                    if (record_minus.Count == 1)
                    {

                        int RECEIVE_NUMBER = Convert.ToInt32(data["RECEIVE_NUMBER"].ToString());
                        int STOCK_OVERPLUS = Convert.ToInt32(record_minus[0]["STOCK_OVERPLUS"].ToString());
                        if (STOCK_OVERPLUS >= 0 && RECEIVE_NUMBER >= 0)
                        {
                            STOCK_OVERPLUS = STOCK_OVERPLUS - RECEIVE_NUMBER;
                            if (STOCK_OVERPLUS < 0)
                            {
                                return Json(new { success = false, message = "剩余库存量不足！" }, JsonRequestBehavior.AllowGet);
                            }
                            string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                            record_minus[0]["STOCK_OVERPLUS"] = number_overplus;

                        }
                        UnCaseSenseHashTable data_minus = new UnCaseSenseHashTable();
                        data_minus["ID"] = record_minus[0]["ID"];
                        data_minus["STOCK_WHOLE"] = record_minus[0]["STOCK_WHOLE"];
                        data_minus["STOCK_OVERPLUS"] = record_minus[0]["STOCK_OVERPLUS"];
                        data_minus["INPUT_TIME"] = record_minus[0]["INPUT_TIME"];
                        data_minus["COMPANY_ID"] = record_minus[0]["COMPANY_ID"];
                        data_minus["COMPANY_ID_V_D_FW_COMP__MC"] = record_minus[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                        Session session_minus = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                        session_minus.BeginTransaction();
                        int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_minus, "B_CARD_STOCK", data_minus, false);
                        session_minus.Commit();
                        session_minus.Close();
                    }
                    if (0 == r)
                    {
                        return Json(new { success = false, message = "保存信息时出错！" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "无该操作权限！" }, JsonRequestBehavior.AllowGet);
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


        //public ActionResult ActionEdit()
        //{
        //    UnCaseSenseHashTable data = new UnCaseSenseHashTable();
        //    Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
        //    try
        //    {
        //        //__TIPS__*:这里修改表名，参考ActionAdd 
        //        ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("B_PLAN");
        //        data.LoadFromNameValueCollection(Request.Unvalidated.Form, ti, true);//使用Request.Unvalidated.Form可以POST HTML标签数据。
        //        data["ID"] = Request["OBJECT_ID"];//这ID字段是加载不进来的。  
        //        string sqlType = WebConfigurationManager.ConnectionStrings["server_type"].ConnectionString;
        //        if (sqlType == "sqlite")
        //        {
        //            string id = Request["OBJECT_ID"].ToString();
        //            String dirPath = Server.MapPath("/plans/file") + "/" + id;
        //            //删除旧的文件
        //            DirectoryInfo dir = new DirectoryInfo(dirPath);
        //            if (dir.Exists)
        //            {
        //                DirectoryInfo[] childs = dir.GetDirectories();
        //                foreach (DirectoryInfo child in childs)
        //                {
        //                    child.Delete(true);
        //                }
        //                dir.Delete(true);
        //            }
        //            string fileName = Request["PLAN_FILE"];
        //            string path = System.Web.HttpContext.Current.Server.MapPath("~/" + fileName);
        //            ZipFile.ExtractToDirectory(path, dirPath);
        //        }
        //        session.BeginTransaction();
        //        int r = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session, "B_PLAN", data, false);
        //        session.Commit();
        //        session.Close();
        //        if (0 == r)
        //        {
        //            return Json(new { success = false, message = "保存信息时出错！" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        session.Rollback();
        //        session.Close();
        //        return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //    var result = new { success = true, message = "保存成功" };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

         public ActionResult ActionDelete()
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
                //删除之前，查找发放单位和接受单位的编号
                Condition cdtId = new Condition("AND", "ID", "=", id);
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_GRANT", cdtId, "*", null, null, -1, -1);
                session.BeginTransaction();
                //__TIPS__*:这里修改表名，参考ActionAdd
                DbUtilityManager.Instance.DefaultDbUtility.Execute("delete from B_CARD_GRANT  where ID=" + id.ToString());
                session.Commit();
                session.Close();
                if (record.Count == 1 ) {
                    string EXTEND_ID = record[0]["EXTEND_ID"].ToString();//发卡单位ID
                    string RECEIVE_ID = record[0]["RECEIVE_ID"].ToString();//领卡单位ID
                    int delete_number = Convert.ToInt32(record[0]["RECEIVE_NUMBER"].ToString());

                //发卡单位卡数量恢复
                    Condition recovery_add = new Condition("AND", "COMPANY_ID", "=", EXTEND_ID);
                    List<UnCaseSenseHashTable> record_recovery_add = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", recovery_add, "*", null, null, -1, -1);
                    if (record_recovery_add.Count == 1)
                    {
                        int STOCK_WHOLE = Convert.ToInt32(record_recovery_add[0]["STOCK_WHOLE"].ToString());
                        int STOCK_OVERPLUS = Convert.ToInt32(record_recovery_add[0]["STOCK_OVERPLUS"].ToString());
                        if (STOCK_OVERPLUS >= 0 && delete_number >= 0 && STOCK_WHOLE >= 0)
                        {
                            STOCK_OVERPLUS = STOCK_OVERPLUS + delete_number;
                            if (EXTEND_ID!= "450000000000") {
                                STOCK_WHOLE = STOCK_WHOLE + delete_number;
                            }                            
                            string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                            string number_whole = Convert.ToString(STOCK_WHOLE);
                            record_recovery_add[0]["STOCK_OVERPLUS"] = number_overplus;
                            record_recovery_add[0]["STOCK_WHOLE"] = number_whole;
                        }
                        UnCaseSenseHashTable data_add = new UnCaseSenseHashTable();
                        data_add["ID"] = record_recovery_add[0]["ID"];
                        data_add["STOCK_WHOLE"] = record_recovery_add[0]["STOCK_WHOLE"];
                        data_add["STOCK_OVERPLUS"] = record_recovery_add[0]["STOCK_OVERPLUS"];
                        data_add["INPUT_TIME"] = record_recovery_add[0]["INPUT_TIME"];
                        data_add["COMPANY_ID"] = record_recovery_add[0]["COMPANY_ID"];
                        data_add["COMPANY_ID_V_D_FW_COMP__MC"] = record_recovery_add[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                        Session session_number = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                        session_number.BeginTransaction();
                        int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_number, "B_CARD_STOCK", data_add, false);
                        session_number.Commit();
                        session_number.Close();
                    }

                    //领卡单位库存量减少
                    Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", RECEIVE_ID);
                    List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                    if (record_minus.Count == 1)
                    {

                        int STOCK_OVERPLUS = Convert.ToInt32(record_minus[0]["STOCK_OVERPLUS"].ToString());
                        int STOCK_WHOLE = Convert.ToInt32(record_minus[0]["STOCK_WHOLE"].ToString());
                        if (STOCK_OVERPLUS >= 0 && STOCK_WHOLE >= 0 && delete_number >= 0)
                        {
                            STOCK_OVERPLUS = STOCK_OVERPLUS - delete_number;
                            STOCK_WHOLE = STOCK_WHOLE - delete_number;
                            string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                            record_minus[0]["STOCK_OVERPLUS"] = number_overplus;
                            string number_whole = Convert.ToString(STOCK_WHOLE);
                            record_minus[0]["STOCK_WHOLE"] = number_whole;

                        }
                        UnCaseSenseHashTable data_minus = new UnCaseSenseHashTable();
                        data_minus["ID"] = record_minus[0]["ID"];
                        data_minus["STOCK_WHOLE"] = record_minus[0]["STOCK_WHOLE"];
                        data_minus["STOCK_OVERPLUS"] = record_minus[0]["STOCK_OVERPLUS"];
                        data_minus["INPUT_TIME"] = record_minus[0]["INPUT_TIME"];
                        data_minus["COMPANY_ID"] = record_minus[0]["COMPANY_ID"];
                        data_minus["COMPANY_ID_V_D_FW_COMP__MC"] = record_minus[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                        Session session_minus = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                        session_minus.BeginTransaction();
                        int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_minus, "B_CARD_STOCK", data_minus, false);
                        session_minus.Commit();
                        session_minus.Close();
                    }
                }
                
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
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  B_PLAN set DISABLED=" + state + " where ID=" + id.ToString());
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
                if (!Membership.CurrentUser.HaveAuthority("SYS.USER.SELECT_OTHOR_COMPANY"))
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
                    cdtId2.AddSubCondition("AND", "DM", "like", comId3);
                    return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC", cdtId2, "*");
                }
                else
                {

                    return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC", cdtId2, "*");
                }
            }
            else if ("V_D_FW_COMP".Equals(Request["dic"]))
            {
                Condition cdtId2 = new Condition();
                int RoleLevel = Membership.CurrentUser.RoleLevel;
                if (!Membership.CurrentUser.HaveAuthority("SYS.USER.SELECT_OTHOR_COMPANY"))
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
                    cdtId2.AddSubCondition("AND", "DM", "like", comId3);
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
            if ("V_D_FW_COMP".Equals(Request["dic"])) {
                return View("ViewCompanyType");
            }
            else
            {
                return View("~/Views/Shared/ViewCommonDicUI.cshtml");
            }
        }
        #endregion
        public FileResult ActionPrint()
        {
            //获取数据
            Condition cdtIds = new Condition();
            string search = Request["Search"];
            string date_range_type = Request["date_range_type"];
            string start_date = Request["start_date"];
            string end_date = Request["end_date"];
            Condition cdtIds2 = new Condition();
            if (!string.IsNullOrEmpty(search))
            {
                cdtIds2.AddSubCondition("OR", "RECEIVE_NUMBER", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECEIVE_PHONE", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECEIVE_TIME", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "EXTEND_NAME", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECEIVE_NAME", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "EXTEND_ID_V_D_FW_COMP__MC", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "EXTEND_ID", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECEIVE_ID_V_D_FW_COMP__MC", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECEIVE_ID", "like", "%" + search + "%");
            }
            if (!string.IsNullOrEmpty(date_range_type) && date_range_type != "0" && (!string.IsNullOrEmpty(start_date) || !string.IsNullOrEmpty(end_date)))
            {
                if (!string.IsNullOrEmpty(start_date))
                {
                    cdtIds.AddSubCondition("AND", "RECEIVE_TIME", "=", DateTime.Parse(start_date));
                }  
            }
            if (Request["cdt_combination"] != null)
            {
                string jsoncdtCombination = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(Request["cdt_combination"]));
                Condition cdtCombination = Condition.LoadFromJson(jsoncdtCombination);
                cdtCombination.Relate = "AND";
                ReplaceCdtCombinationOpreate(cdtCombination);
                cdtIds.AddSubCondition(cdtCombination);
            }
            cdtIds2.Relate = "AND";
            cdtIds.AddSubCondition(cdtIds2);
            if (!Membership.CurrentUser.HaveAuthority("MACHINE.MACHINEMGR.CHANGE_MACHINE"))
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
                cdtIds.AddSubCondition("AND", "EXTEND_ID", "like", comId3);      
            }
            List<UnCaseSenseHashTable> records = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_GRANT", cdtIds, "*", null, null, -1, -1);

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
            PdfPTable table = new PdfPTable(8);
            table.SetWidths(new float[] { 2.5F, 8, 8, 8, 8,8,8,8 });
            table.WidthPercentage = 100;
            AddBodyContentCell(table, "序号", cn);
            AddBodyContentCell(table, "发卡人", cn);
            AddBodyContentCell(table, "发卡单位", cn);
            AddBodyContentCell(table, "领卡人", cn);
            AddBodyContentCell(table, "领卡单位", cn);
            AddBodyContentCell(table, "领卡数量", cn);
            AddBodyContentCell(table, "领卡时间", cn);
            AddBodyContentCell(table, "领卡人联系电话", cn);   

            for (int i = 0; i < records.Count; i++)
            {
                UnCaseSenseHashTable record = records[i];
                AddBodyContentCell(table, Convert.ToString(i + 1), cn);
                if (!string.IsNullOrEmpty((string)record["EXTEND_NAME"]))
                {
                    AddBodyContentCell(table, record["EXTEND_NAME"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["EXTEND_ID_V_D_FW_COMP__MC"]))
                {
                    AddBodyContentCell(table, record["EXTEND_ID_V_D_FW_COMP__MC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty((string)record["RECEIVE_NAME"]))
                {
                    AddBodyContentCell(table, record["RECEIVE_NAME"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty((string)record["RECEIVE_ID_V_D_FW_COMP__MC"]))
                {
                    AddBodyContentCell(table, record["RECEIVE_ID_V_D_FW_COMP__MC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                //if (!string.IsNullOrEmpty(record["RECEIVE_ID_V_D_FW_COMP__MC"].ToString()))
                //{
                //    string s = record["RECEIVE_ID_V_D_FW_COMP__MC"].ToString();
                //    string date = s.Substring(0, 8);
                //    AddBodyContentCell(table, date, cn);
                //}
                //else
                //{
                //    AddBodyContentCell(table, "", cn);
                //}

                //if (!string.IsNullOrEmpty(record["DISABLED"].ToString()))
                //{
                //    if (record["DISABLED"].ToString() == "0")
                //        AddBodyContentCell(table, "启用", cn);
                //    else
                //        AddBodyContentCell(table, "禁用", cn);
                //}
                //else
                //{
                //    AddBodyContentCell(table, "未知", cn);
                //}

                if (!string.IsNullOrEmpty(record["RECEIVE_NUMBER"].ToString ()))
                {
                    AddBodyContentCell(table, record["RECEIVE_NUMBER"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty(record["RECEIVE_TIME"].ToString()))
                {
                    string s = record["RECEIVE_TIME"].ToString();
                       string date = s.Substring(0, 8);
                       AddBodyContentCell(table, date, cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["RECEIVE_PHONE"]))
                {
                    AddBodyContentCell(table, record["RECEIVE_PHONE"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                //if (!string.IsNullOrEmpty((string)record["ADDRESS"]))
                //{
                //    AddBodyContentCell(table, record["ADDRESS"].ToString(), cn);
                //}
                //else
                //{
                //    AddBodyContentCell(table, "", cn);
                //}

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

    }
}