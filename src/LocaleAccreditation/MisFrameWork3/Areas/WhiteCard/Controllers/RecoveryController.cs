using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.IO;
using System.Globalization;
using System.IO.Compression;

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

        public ActionResult JsonConditionCombinationInfo()
        {
            return View();
        }
        public ActionResult GetSelect()
        {
            List<UnCaseSenseHashTable> planType = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", null, "DM,MC", null, null, -1, -1);
            return JsonDateObject(new {planType = planType });
        }
        #region __TIPS__:框架通用函数( 增 删 改)
        public ActionResult JsonDataList()//业务主界面数据查询函数
        {
            //接收的基本查询参数有： id,limit,offset,search,sort,order            
            //__TIPS__*:根据表结构，修改以下函数的参数
            string RECOVERY = Request["RECOVERY"];
            string RECOVERY_ID = Request["RECOVERY_ID"];
            string SUBMIT_NUMBER = Request["SUBMIT_NUMBER"];
            string SUBMIT = Request["SUBMIT"];
            string SUBMIT_ID = Request["SUBMIT_ID"];

            Condition cdtIds = new Condition();
            if (!String.IsNullOrEmpty(RECOVERY))
            {
                cdtIds.AddSubCondition("AND", "RECOVERY", "=",RECOVERY);
            }
            if (!String.IsNullOrEmpty(RECOVERY_ID))
            {
                cdtIds.AddSubCondition("AND", "RECOVERY_ID", "=", RECOVERY_ID);
            }
            if (!String.IsNullOrEmpty(SUBMIT_NUMBER))
            {
                cdtIds.AddSubCondition("AND", "SUBMIT_NUMBER", "=",SUBMIT_NUMBER);
            }
            if (!String.IsNullOrEmpty(SUBMIT))
            {
                cdtIds.AddSubCondition("AND", "SUBMIT", "=",SUBMIT);
            }
            if (!String.IsNullOrEmpty(SUBMIT_ID))
            {
                cdtIds.AddSubCondition("AND", "SUBMIT_ID", "=", SUBMIT_ID);
            }
            return QueryDataFromEasyUIDataGrid("B_CARD_RECOVERY", "RECOVERY_TIME,SUBMIT_NUMBER", "SUBMIT_ID", cdtIds, "*");
        }


        public ActionResult JsonDataIndex()//业务主界面数据查询函数
        {
            //接收的基本查询参数有： id,limit,offset,search,sort,order            
            //__TIPS__*:根据表结构，修改以下函数的参数
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

                cdtId = new Condition("AND", "RECOVERY_ID", "like", comId3);
                return QueryDataFromEasyUIDataGrid("B_CARD_RECOVERY", "RECOVERY_TIME,SUBMIT_NUMBER", "SUBMIT_ID", cdtId, "*");
            }
            else
            {
                return QueryDataFromEasyUIDataGrid("B_CARD_RECOVERY", "RECOVERY_TIME,SUBMIT_NUMBER", "SUBMIT_ID", null, "*");
            }
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
                session.BeginTransaction();
                int r = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "B_CARD_RECOVERY", data);
                session.Commit();
                session.Close();

                //回收单位剩余库存和总库存增加
                Condition cdtId_add = new Condition("AND", "COMPANY_ID", "=", data["RECOVERY_ID"]);
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_add, "*", null, null, -1, -1);
                if (record.Count == 1)
                {
                    int STOCK_WHOLE = Convert.ToInt32(record[0]["STOCK_WHOLE"].ToString());
                    int SUBMIT_NUMBER = Convert.ToInt32(data["SUBMIT_NUMBER"].ToString());
                    int STOCK_OVERPLUS = Convert.ToInt32(record[0]["STOCK_OVERPLUS"].ToString());
                    if (STOCK_OVERPLUS >= 0 && SUBMIT_NUMBER >= 0 && STOCK_WHOLE >= 0)
                    {
                        STOCK_OVERPLUS = STOCK_OVERPLUS + SUBMIT_NUMBER;
                        STOCK_WHOLE = STOCK_WHOLE + SUBMIT_NUMBER;
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
                //提交单位剩余库存和总库存减少
                Condition cdtId_minus = new Condition("AND", "COMPANY_ID", "=", data["SUBMIT_ID"]);
                List<UnCaseSenseHashTable> record_minus = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", cdtId_minus, "*", null, null, -1, -1);
                if (record_minus.Count == 1)
                {

                    int SUBMIT_ID = Convert.ToInt32(data["SUBMIT_ID"].ToString());
                    int STOCK_OVERPLUS = Convert.ToInt32(record_minus[0]["STOCK_OVERPLUS"].ToString());
                    int STOCK_WHOLE = Convert.ToInt32(record_minus[0]["STOCK_WHOLE"].ToString());
                    if (STOCK_OVERPLUS >= 0 && SUBMIT_ID >= 0&& STOCK_WHOLE>=0)
                    {
                        STOCK_OVERPLUS = STOCK_OVERPLUS - SUBMIT_ID;
                        STOCK_WHOLE = STOCK_WHOLE - SUBMIT_ID;
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
                List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_RECOVERY", cdtId, "*", null, null, -1, -1);
                session.BeginTransaction();
                //__TIPS__*:这里修改表名，参考ActionAdd
                DbUtilityManager.Instance.DefaultDbUtility.Execute("delete from B_CARD_RECOVERY  where ID=" + id.ToString());
                session.Commit();
                session.Close();
                if (record.Count == 1)
                {
                    string SUBMIT_ID = record[0]["SUBMIT_ID"].ToString();//提交单位ID
                    string RECOVERY_ID = record[0]["RECOVERY_ID"].ToString();//回收单位ID
                    int submit_number = Convert.ToInt32(record[0]["SUBMIT_NUMBER"].ToString()); //提交数量

                    //提交单位卡数量恢复
                    Condition recovery_add = new Condition("AND", "COMPANY_ID", "=", SUBMIT_ID);
                    List<UnCaseSenseHashTable> record_recovery_add = DbUtilityManager.Instance.DefaultDbUtility.Query("B_CARD_STOCK", recovery_add, "*", null, null, -1, -1);
                    if (record_recovery_add.Count == 1)
                    {
                        int STOCK_WHOLE = Convert.ToInt32(record_recovery_add[0]["STOCK_WHOLE"].ToString());
                        int STOCK_OVERPLUS = Convert.ToInt32(record[0]["STOCK_OVERPLUS"].ToString());
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
                        int STOCK_WHOLE = Convert.ToInt32(record_minus[0]["STOCK_WHOLE"].ToString());
                        if (STOCK_OVERPLUS >= 0 && STOCK_WHOLE >= 0 && submit_number >= 0)
                        {
                            STOCK_OVERPLUS = STOCK_OVERPLUS - submit_number;
                            STOCK_WHOLE = STOCK_WHOLE - submit_number;
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
            return QueryDataFromEasyUIDataGrid(Request["dic"], null, "DM,MC", null, "*");
        }

        public ActionResult ViewDicLargeUI()
        {
            /*
             * __TIPS__:有些特殊的字典可能需要显示更多的东西所以这里可以根据Request的值返回不同的视图
             *          以下演示根据字典内容，返回不同的视图。
             * */
            if ("V_D_FW_COMP".Equals(Request["dic"])) {
                return View("ViewRecovery");
            }
            else
            {
                return View("~/Views/Shared/ViewCommonDicUI.cshtml");
            }
        }
        #endregion
    }
}