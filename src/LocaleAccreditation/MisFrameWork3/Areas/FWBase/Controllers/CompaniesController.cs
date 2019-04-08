using System;
using System.Web.Mvc;

using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Authorize;
using MisFrameWork3.Classes.Membership;

namespace MisFrameWork3.Areas.FWBase.Controllers
{
    [Logined(OperateId = "SYS.COMPANIES")]
    public class CompaniesController : FWBaseController
    {
        // GET: FWBase/Companies
        public ActionResult Index()
        {
            return View();
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
            /*
            __TIPS__: 
                1、加载窗体提交的数据可以使用LoadFromNameValueCollection 结合正则表达式过滤掉没有用的数据
                    比如：data.LoadFromNameValueCollection(Request.Form, "NAME|TYPE|COMPANY_CODE",true);
                    这样只加载NAME、TYPE、COMPANY_CODE 这几项，其它项不处理
                2、获取表信息，然后加只加载与表字段同名的内容，这个方法最常用，比如这样：
                    ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("FW_S_COMAPANIES");
                    data.LoadFromNameValueCollection(Request.Form, ti, true);
            */
            //data.LoadFromNameValueCollection(Request.Form, "NAME|TYPE|COMPANY_CODE",true);
            ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("FW_S_COMAPANIES");
            data.LoadFromNameValueCollection(Request.Form, ti, true);

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                int r = DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_COMAPANIES", data);                
                session.Commit();
                session.Close();
                if (r!=1)
                    return Json(new { success = false, message = "保存记录失败！" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                session.Rollback();
                session.Close();
                return Json(new { success = false, message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
            var result = new { success = true, message = "保存成功" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActionEdit()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            
            ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("FW_S_COMAPANIES");
            data["ID"] = Request["OBJECT_ID"];//这一字字段是加载不进来的。
            data.LoadFromNameValueCollection(Request.Form, ti, true);

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();

                DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session, "FW_S_COMAPANIES", data, false);
                
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
            var result = new { success = true, message = "保存成功" };
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
                if (state==1)//如果是禁用的，要同时把用户也禁用了，如果是启用的，就要手动去逐个启用用户
                    DbUtilityManager.Instance.DefaultDbUtility.Execute("update  FW_S_USERS set DISABLED=" + state + " where COMPANY_ID=" + id.ToString());
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  FW_S_COMAPANIES set DISABLED=" + state + " where ID=" + id.ToString());
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
            var result = new { success = true, message = "操作成功" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JsonDataList()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            Condition cdtIds = new Condition();
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
                cdtIds.AddSubCondition("AND", "COMPANY_CODE", "like", comId3);
            }
            //#region 初始化基本查询参数 id,limit,offset,search,sort,order
            return QueryDataFromEasyUIDataGrid("FW_S_COMAPANIES", "CRATE_ON,UPDATE_ON", "NAME,COMPANY_CODE", cdtIds, "*");
        }
        
    }
}