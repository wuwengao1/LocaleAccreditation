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
    public class RecoveryController : FWBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JsonConditionRecovery()
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

                cdtId.AddSubCondition("AND", "RECOVERY_ID", "like", comId3);
               
            }
            return QueryDataFromEasyUIDataGrid("B_CARD_RECOVERY", "RECOVERY_TIME,SUBMIT_NUMBER", "SUBMIT_ID,SUBMIT_NUMBER,SUBMIT_PHONE,RECOVERY_TIME,RECOVERY_NAME,SUBMIT_NAME,RECOVERY_ID_V_D_FW_COMP__MC,RECOVERY_ID,SUBMIT_ID_V_D_FW_COMP__MC,TYPE_ID_D_CARDTYPE__MC,TYPE_ID", cdtId, "*");
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
            string COMPANY_ID = Membership.CurrentUser.CompanyId.ToString();//回收单位ID
            string COMPANY_NAME = Membership.CurrentUser.CompanyName.ToString();//回收单位名称
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
                ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("B_CARD_RECOVERY");
                data.LoadFromNameValueCollection(Request.Unvalidated.Form, ti, true);//使用Request.Unvalidated.Form可以POST HTML标签数据。
                data["RECOVERY_ID"] = COMPANY_ID;
                data["RECOVERY_NAME"] = COMPANY_NAME;
                Int64 SUBMIT_ID = Convert.ToInt64(data["SUBMIT_ID"].ToString());
                Int64 RECOVERY_ID = Convert.ToInt64(COMPANY_ID);
                if (RECOVERY_ID< SUBMIT_ID)
                {
                    session.BeginTransaction();
                    int r = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "B_CARD_RECOVERY", data);

                    //更新库存
                    if (data["TYPE_ID"].ToString() == "01")//判断回收类型
                    {
                        //回收单位剩余库存增加，总库存不变
                        Condition cdtId_add = new Condition("AND", "COMPANY_ID", "=", COMPANY_ID);
                        List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_add, "*", null, null, -1, -1);
                        if (record.Count == 1)
                        {
                            int SUBMIT_NUMBER = Convert.ToInt32(data["SUBMIT_NUMBER"].ToString());   //提交数量
                            int STOCK_OVERPLUS = Convert.ToInt32(record[0]["STOCK_OVERPLUS"].ToString()); //剩余库存量
                            if (STOCK_OVERPLUS >= 0 && SUBMIT_NUMBER >= 0)
                            {
                                STOCK_OVERPLUS = STOCK_OVERPLUS + SUBMIT_NUMBER;
                                string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                                record[0]["STOCK_OVERPLUS"] = number_overplus;
                            }
                            UnCaseSenseHashTable data_add = new UnCaseSenseHashTable();
                            data_add["ID"] = record[0]["ID"];
                            data_add["STOCK_WHOLE"] = record[0]["STOCK_WHOLE"];
                            data_add["STOCK_OVERPLUS"] = record[0]["STOCK_OVERPLUS"];
                            data_add["INPUT_TIME"] = record[0]["INPUT_TIME"];
                            data_add["COMPANY_ID"] = record[0]["COMPANY_ID"];
                            data_add["STOCK_SCRAP"] = record[0]["STOCK_SCRAP"];
                            data_add["COMPANY_ID_V_D_FW_COMP__MC"] = record[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                            Session session_number = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                            session_number.BeginTransaction();
                            int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_number, "B_CARD_STOCK", data_add, false);
                            session_number.Commit();
                            session_number.Close();
                        }else {
                            return Json(new { success = false, message = "数据库中无该回收单位！" }, JsonRequestBehavior.AllowGet);
                        }
                        //提交单位剩余库存和总库存减少
                        Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", data["SUBMIT_ID"]);
                        List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                        if (record_minus.Count == 1)
                        {

                            int SUBMIT_NUMBER = Convert.ToInt32(data["SUBMIT_NUMBER"].ToString());
                            int STOCK_OVERPLUS = Convert.ToInt32(record_minus[0]["STOCK_OVERPLUS"].ToString());
                            int STOCK_WHOLE = Convert.ToInt32(record_minus[0]["STOCK_WHOLE"].ToString());
                            if (STOCK_OVERPLUS - SUBMIT_NUMBER >= 0)
                            {
                                if (STOCK_OVERPLUS >= 0 && SUBMIT_NUMBER >= 0 && STOCK_WHOLE >= 0 && STOCK_WHOLE - SUBMIT_NUMBER >= 0)
                                {
                                    STOCK_OVERPLUS = STOCK_OVERPLUS - SUBMIT_NUMBER;
                                    STOCK_WHOLE = STOCK_WHOLE - SUBMIT_NUMBER;
                                    string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                                    record_minus[0]["STOCK_OVERPLUS"] = number_overplus;
                                    string number_WHOLE = Convert.ToString(STOCK_WHOLE);
                                    record_minus[0]["STOCK_WHOLE"] = number_WHOLE;
                                }
                                UnCaseSenseHashTable data_minus = new UnCaseSenseHashTable();
                                data_minus["ID"] = record_minus[0]["ID"];
                                data_minus["STOCK_WHOLE"] = record_minus[0]["STOCK_WHOLE"];
                                data_minus["STOCK_OVERPLUS"] = record_minus[0]["STOCK_OVERPLUS"];
                                data_minus["INPUT_TIME"] = record_minus[0]["INPUT_TIME"];
                                data_minus["COMPANY_ID"] = record_minus[0]["COMPANY_ID"];
                                data_minus["STOCK_SCRAP"] = record_minus[0]["STOCK_SCRAP"];
                                data_minus["COMPANY_ID_V_D_FW_COMP__MC"] = record_minus[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                                Session session_minus = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                                session_minus.BeginTransaction();
                                int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_minus, "B_CARD_STOCK", data_minus, false);
                                session_minus.Commit();
                                session_minus.Close();
                            }
                            else {
                                return Json(new { success = false, message = "剩余库存数量不足！" }, JsonRequestBehavior.AllowGet);
                            }
                        }else{
                            return Json(new { success = false, message = "数据库中无该提交单位！" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //提交单位的总库存减少，报废库存减少
                        Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", data["SUBMIT_ID"]);
                        List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                        if (record_minus.Count == 1)
                        {

                            int SUBMIT_NUMBER = Convert.ToInt32(data["SUBMIT_NUMBER"].ToString());
                            int STOCK_SCRAP = Convert.ToInt32(record_minus[0]["STOCK_SCRAP"].ToString());
                            int STOCK_WHOLE = Convert.ToInt32(record_minus[0]["STOCK_WHOLE"].ToString());
                            if (STOCK_SCRAP - SUBMIT_NUMBER < 0)
                            {
                                return Json(new { success = false, message = "提交单位报废卡数量不足！" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                if (STOCK_SCRAP >= 0 && SUBMIT_NUMBER >= 0 && STOCK_WHOLE >= 0 && STOCK_WHOLE - SUBMIT_NUMBER >= 0)
                                {
                                    STOCK_SCRAP = STOCK_SCRAP - SUBMIT_NUMBER;
                                    STOCK_WHOLE = STOCK_WHOLE - SUBMIT_NUMBER;
                                    string number_overplus = Convert.ToString(STOCK_SCRAP);
                                    record_minus[0]["STOCK_SCRAP"] = number_overplus;
                                    string number_WHOLE = Convert.ToString(STOCK_WHOLE);
                                    record_minus[0]["STOCK_WHOLE"] = number_WHOLE;
                                }
                                UnCaseSenseHashTable data_minus = new UnCaseSenseHashTable();
                                data_minus["ID"] = record_minus[0]["ID"];
                                data_minus["STOCK_WHOLE"] = record_minus[0]["STOCK_WHOLE"];
                                data_minus["STOCK_OVERPLUS"] = record_minus[0]["STOCK_OVERPLUS"];
                                data_minus["INPUT_TIME"] = record_minus[0]["INPUT_TIME"];
                                data_minus["COMPANY_ID"] = record_minus[0]["COMPANY_ID"];
                                data_minus["STOCK_SCRAP"] = record_minus[0]["STOCK_SCRAP"];
                                data_minus["COMPANY_ID_V_D_FW_COMP__MC"] = record_minus[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                                Session session_minus = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                                session_minus.BeginTransaction();
                                int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_minus, "B_CARD_STOCK", data_minus, false);
                                session_minus.Commit();
                                session_minus.Close();
                            }
                        }
                        else {
                            return Json(new { success = false, message = "数据库中无该提交单位！" }, JsonRequestBehavior.AllowGet);
                        }
                        Condition cdtId_add = new Condition("AND", "COMPANY_ID", "=", COMPANY_ID);
                        List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_add, "*", null, null, -1, -1);
                        if (record.Count == 1)
                        {
                            //回收单位报废库存增加
                            int SUBMIT_NUMBER = Convert.ToInt32(data["SUBMIT_NUMBER"].ToString());   //提交数量
                            int STOCK_SCRAP = Convert.ToInt32(record[0]["STOCK_SCRAP"].ToString()); //剩余库存量
                            if (STOCK_SCRAP >= 0 && SUBMIT_NUMBER >= 0)
                            {
                                STOCK_SCRAP = STOCK_SCRAP + SUBMIT_NUMBER;
                                string number_overplus = Convert.ToString(STOCK_SCRAP);
                                record[0]["STOCK_SCRAP"] = number_overplus;
                            }
                            UnCaseSenseHashTable data_add = new UnCaseSenseHashTable();
                            data_add["ID"] = record[0]["ID"];
                            data_add["STOCK_WHOLE"] = record[0]["STOCK_WHOLE"];
                            data_add["STOCK_OVERPLUS"] = record[0]["STOCK_OVERPLUS"];
                            data_add["INPUT_TIME"] = record[0]["INPUT_TIME"];
                            data_add["COMPANY_ID"] = record[0]["COMPANY_ID"];
                            data_add["STOCK_SCRAP"] = record[0]["STOCK_SCRAP"];
                            data_add["COMPANY_ID_V_D_FW_COMP__MC"] = record[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                            Session session_number = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                            session_number.BeginTransaction();
                            int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_number, "B_CARD_STOCK", data_add, false);
                            session_number.Commit();
                            session_number.Close();
                        }
                       
                    }
                    session.Commit();
                    session.Close();
                    if (0 == r)
                    {
                        return Json(new { success = false, message = "保存信息时出错！" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else {
                    return Json(new { success = false, message ="无该操作权限！" }, JsonRequestBehavior.AllowGet);
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
                string type = Request["TYPE_ID"];
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
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_RECOVERY", cdtId, "*", null, null, -1, -1);
                session.BeginTransaction();
                //__TIPS__*:这里修改表名，参考ActionAdd
                DbUtilityManager.Instance.DefaultDbUtility.Execute("delete from B_CARD_RECOVERY  where ID=" + id.ToString());
                string SUBMIT_ID = record[0]["SUBMIT_ID"].ToString();//提交单位ID
                string RECOVERY_ID = record[0]["RECOVERY_ID"].ToString();//回收单位ID
                int submit_number = Convert.ToInt32(record[0]["SUBMIT_NUMBER"].ToString()); //提交数量
                if (record.Count == 1)
                {
                    if (record[0]["TYPE_ID"].ToString() == "01")
                    {
                        //提交单位卡数量恢复
                        Condition recovery_add = new Condition("AND", "COMPANY_ID", "=", SUBMIT_ID);
                        List<UnCaseSenseHashTable> record_recovery_add = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", recovery_add, "*", null, null, -1, -1);
                        if (record_recovery_add.Count == 1)
                        {
                            int STOCK_WHOLE = Convert.ToInt32(record_recovery_add[0]["STOCK_WHOLE"].ToString());
                            int STOCK_OVERPLUS = Convert.ToInt32(record_recovery_add[0]["STOCK_OVERPLUS"].ToString());
                            if (STOCK_OVERPLUS >= 0 && submit_number >= 0 && STOCK_WHOLE >= 0)
                            {
                                STOCK_OVERPLUS = STOCK_OVERPLUS + submit_number;
                                STOCK_WHOLE = STOCK_WHOLE + submit_number;
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
                            data_add["STOCK_SCRAP"] = record_recovery_add[0]["STOCK_SCRAP"];
                            data_add["COMPANY_ID_V_D_FW_COMP__MC"] = record_recovery_add[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                            Session session_number = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                            session_number.BeginTransaction();
                            int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_number, "B_CARD_STOCK", data_add, false);
                            session_number.Commit();
                            session_number.Close();
                        }

                        //回收单位库存量减少
                        Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", RECOVERY_ID);
                        List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                        if (record_minus.Count == 1)
                        {

                            int STOCK_OVERPLUS = Convert.ToInt32(record_minus[0]["STOCK_OVERPLUS"].ToString());
                            if (STOCK_OVERPLUS >= 0 && submit_number >= 0)
                            {
                                STOCK_OVERPLUS = STOCK_OVERPLUS - submit_number;
                                string number_overplus = Convert.ToString(STOCK_OVERPLUS);
                                record_minus[0]["STOCK_OVERPLUS"] = number_overplus;
                            }
                            UnCaseSenseHashTable data_minus = new UnCaseSenseHashTable();
                            data_minus["ID"] = record_minus[0]["ID"];
                            data_minus["STOCK_WHOLE"] = record_minus[0]["STOCK_WHOLE"];
                            data_minus["STOCK_OVERPLUS"] = record_minus[0]["STOCK_OVERPLUS"];
                            data_minus["INPUT_TIME"] = record_minus[0]["INPUT_TIME"];
                            data_minus["COMPANY_ID"] = record_minus[0]["COMPANY_ID"];
                            data_minus["STOCK_SCRAP"] = record_minus[0]["STOCK_SCRAP"];
                            data_minus["COMPANY_ID_V_D_FW_COMP__MC"] = record_minus[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                            Session session_minus = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                            session_minus.BeginTransaction();
                            int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_minus, "B_CARD_STOCK", data_minus, false);
                            session_minus.Commit();
                            session_minus.Close();
                        }
                    }
                    else
                    {
                        //提交单位总库存与报废库存恢复
                        Condition recovery_add = new Condition("AND", "COMPANY_ID", "=", SUBMIT_ID);
                        List<UnCaseSenseHashTable> record_recovery_add = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", recovery_add, "*", null, null, -1, -1);
                        if (record_recovery_add.Count == 1)
                        {
                            int STOCK_WHOLE = Convert.ToInt32(record_recovery_add[0]["STOCK_WHOLE"].ToString());
                            int STOCK_SCRAP = Convert.ToInt32(record_recovery_add[0]["STOCK_SCRAP"].ToString());
                            if (STOCK_SCRAP >= 0 && submit_number >= 0 && STOCK_WHOLE >= 0)
                            {
                                STOCK_SCRAP = STOCK_SCRAP + submit_number;
                                STOCK_WHOLE = STOCK_WHOLE + submit_number;
                                string number_overplus = Convert.ToString(STOCK_SCRAP);
                                string number_whole = Convert.ToString(STOCK_WHOLE);
                                record_recovery_add[0]["STOCK_SCRAP"] = number_overplus;
                                record_recovery_add[0]["STOCK_WHOLE"] = number_whole;
                            }
                            UnCaseSenseHashTable data_add = new UnCaseSenseHashTable();
                            data_add["ID"] = record_recovery_add[0]["ID"];
                            data_add["STOCK_WHOLE"] = record_recovery_add[0]["STOCK_WHOLE"];
                            data_add["STOCK_OVERPLUS"] = record_recovery_add[0]["STOCK_OVERPLUS"];
                            data_add["INPUT_TIME"] = record_recovery_add[0]["INPUT_TIME"];
                            data_add["COMPANY_ID"] = record_recovery_add[0]["COMPANY_ID"];
                            data_add["STOCK_SCRAP"] = record_recovery_add[0]["STOCK_SCRAP"];
                            data_add["COMPANY_ID_V_D_FW_COMP__MC"] = record_recovery_add[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                            Session session_number = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                            session_number.BeginTransaction();
                            int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_number, "B_CARD_STOCK", data_add, false);
                            session_number.Commit();
                            session_number.Close();
                        }

                        //回收单位报废库存量减少
                        Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", RECOVERY_ID);
                        List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                        if (record_minus.Count == 1)
                        {

                            int STOCK_SCRAP = Convert.ToInt32(record_minus[0]["STOCK_SCRAP"].ToString());
                            if (STOCK_SCRAP >= 0 && submit_number >= 0)
                            {
                                STOCK_SCRAP = STOCK_SCRAP - submit_number;
                                string number_overplus = Convert.ToString(STOCK_SCRAP);
                                record_minus[0]["STOCK_SCRAP"] = number_overplus;
                            }
                            UnCaseSenseHashTable data_minus = new UnCaseSenseHashTable();
                            data_minus["ID"] = record_minus[0]["ID"];
                            data_minus["STOCK_WHOLE"] = record_minus[0]["STOCK_WHOLE"];
                            data_minus["STOCK_OVERPLUS"] = record_minus[0]["STOCK_OVERPLUS"];
                            data_minus["INPUT_TIME"] = record_minus[0]["INPUT_TIME"];
                            data_minus["COMPANY_ID"] = record_minus[0]["COMPANY_ID"];
                            data_minus["STOCK_SCRAP"] = record_minus[0]["STOCK_SCRAP"];
                            data_minus["COMPANY_ID_V_D_FW_COMP__MC"] = record_minus[0]["COMPANY_ID_V_D_FW_COMP__MC"];
                            Session session_minus = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
                            session_minus.BeginTransaction();
                            int s = DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session_minus, "B_CARD_STOCK", data_minus, false);
                            session_minus.Commit();
                            session_minus.Close();
                        }

                    }

                }
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
                    return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC,DW", cdtId2, "*");
                }
                else
                {

                    return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC,DW", cdtId2, "*");
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
            if ("D_CARDTYPE".Equals(Request["dic"])) {
                return View("ViewCardType");
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
                cdtIds2.AddSubCondition("OR", "SUBMIT_NUMBER", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "SUBMIT_PHONE", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECOVERY_TIME", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECOVERY_NAME", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "SUBMIT_NAME", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECOVERY_ID_V_D_FW_COMP__MC", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "RECOVERY_ID", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "SUBMIT_ID_V_D_FW_COMP__MC", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "SUBMIT_ID", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "TYPE_ID", "like", "%" + search + "%");
                cdtIds2.AddSubCondition("OR", "TYPE_ID_D_CARDTYPE__MC", "like", "%" + search + "%");
            }
            if (!string.IsNullOrEmpty(date_range_type) && date_range_type != "0" && (!string.IsNullOrEmpty(start_date) || !string.IsNullOrEmpty(end_date)))
            {
                if (!string.IsNullOrEmpty(start_date))
                {
                    cdtIds.AddSubCondition("AND", "RECOVERY_TIME", "=", DateTime.Parse(start_date));
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
                cdtIds.AddSubCondition("AND", "RECOVERY_ID", "like", comId3);                
            }
            List<UnCaseSenseHashTable> records = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_RECOVERY", cdtIds, "*", null, null, -1, -1);

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
            table.SetWidths(new float[] { 2.5F, 8, 8, 8, 8, 8,8, 8,8 });
            table.WidthPercentage = 100;
            AddBodyContentCell(table, "序号", cn);
            AddBodyContentCell(table, "回收人", cn);
            AddBodyContentCell(table, "回收单位", cn);
            AddBodyContentCell(table, "提交人", cn);
            AddBodyContentCell(table, "提交单位", cn);
            AddBodyContentCell(table, "回收数量", cn);
            AddBodyContentCell(table, "回收类型", cn);
            AddBodyContentCell(table, "回收时间", cn);
            AddBodyContentCell(table, "回收人联系电话", cn);
            for (int i = 0; i < records.Count; i++)
            {
                UnCaseSenseHashTable record = records[i];
                AddBodyContentCell(table, Convert.ToString(i + 1), cn);
                if (!string.IsNullOrEmpty((string)record["RECOVERY_NAME"]))
                {
                    AddBodyContentCell(table, record["RECOVERY_NAME"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["RECOVERY_ID_V_D_FW_COMP__MC"]))
                {
                    AddBodyContentCell(table, record["RECOVERY_ID_V_D_FW_COMP__MC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty((string)record["SUBMIT_NAME"]))
                {
                    AddBodyContentCell(table, record["SUBMIT_NAME"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty((string)record["SUBMIT_ID_V_D_FW_COMP__MC"]))
                {
                    AddBodyContentCell(table, record["SUBMIT_ID_V_D_FW_COMP__MC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (!string.IsNullOrEmpty(record["SUBMIT_NUMBER"].ToString()))
                {
                    AddBodyContentCell(table, record["SUBMIT_NUMBER"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }               
                if (!string.IsNullOrEmpty(record["TYPE_ID"].ToString()))
                {
                    if (record["TYPE_ID"].ToString() == "01")
                        AddBodyContentCell(table, "闲置", cn);
                    else
                        AddBodyContentCell(table, "报废", cn);
                }
                else
                {
                    AddBodyContentCell(table, "未知", cn);
                }
                if (!string.IsNullOrEmpty(record["RECOVERY_TIME"].ToString()))
                {
                    string s = record["RECOVERY_TIME"].ToString();
                    string date = s.Substring(0, 8);
                    AddBodyContentCell(table, date, cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }


                if (!string.IsNullOrEmpty((string)record["SUBMIT_PHONE"]))
                {
                    AddBodyContentCell(table, record["SUBMIT_PHONE"].ToString(), cn);
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

    }
}