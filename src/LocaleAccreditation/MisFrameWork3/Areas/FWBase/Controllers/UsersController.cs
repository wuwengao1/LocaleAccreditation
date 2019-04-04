using System;
using System.Collections.Generic;
using System.Web.Mvc;

using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;
using MisFrameWork3.Classes.Authorize;


namespace MisFrameWork3.Areas.FWBase.Controllers
{
    [Logined(OperateId = "SYS.USER")]
    public class UsersController : FWBaseController
    {
        // GET: FWBase/Users
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

                List<UnCaseSenseHashTable> ssdw = DbUtilityManager.Instance.DefaultDbUtility.Query("D_XZQH", cdtId, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { ssdw = ssdw });
            }
            else
            {
                List<UnCaseSenseHashTable> ssdw = DbUtilityManager.Instance.DefaultDbUtility.Query("D_XZQH", null, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { ssdw = ssdw });
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

        public ActionResult ViewFormQueryCompany()
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
            ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("FW_S_USERS");
            data.LoadFromNameValueCollection(Request.Form, ti, true);
            //查询出所属单位
            Condition cdtCompany = new Condition();
            cdtCompany.AddSubCondition("AND", "DM", "=", data["COMPANY_ID"]);

            List<UnCaseSenseHashTable> recordCompaies = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", cdtCompany, "*", null);

            if (recordCompaies.Count != 1)
                return Json(new { success = false, message = "所属单位信息无效：" + data["COMPANY_ID"] }, JsonRequestBehavior.AllowGet);
            data["DISABLED"] = recordCompaies[0]["DISABLED"];
            //处理密码部份
            data["USER_PASSWD"] = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data["USER_PASSWD"].ToString(), "MD5");
            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {

                session.BeginTransaction();
                DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_USERS", data);
                if (!String.IsNullOrEmpty(Request["ROLES_ID"]))
                {
                    string[] roleIds = Request["ROLES_ID"].Split(',');
                    foreach (string roleId in roleIds)
                    {
                        UnCaseSenseHashTable record = new UnCaseSenseHashTable();
                        record["USER_ID"] = data["USER_ID"];
                        record["ROLE_ID"] = roleId;
                        DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_USERS_M_ROLES", record);
                    }
                }
                session.Commit();
                session.Close();
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

            ITableInfo ti = DbUtilityManager.Instance.DefaultDbUtility.CreateTableInfo("FW_S_USERS");
            data.LoadFromNameValueCollection(Request.Form, ti, true);
            data["USER_ID"] = Request["OBJECT_ID"];//这一字字段是加载不进来的。  
            //这里要判断一下用户的角色级别
            FWUserInfo userTarget = new FWUserInfo();
            if (!userTarget.LoadData(Request["OBJECT_ID"]))
                return Json(new { success = false, message = "该用户不存在" }, JsonRequestBehavior.AllowGet);
            if (userTarget.RoleLevel <= Membership.CurrentUser.RoleLevel)
                return Json(new { success = false, message = "当前登陆用户的角色级别过低，不允许操作" }, JsonRequestBehavior.AllowGet);
            if (!data.HasValue("USER_PASSWD"))//没有值就是不设置密码
            {
                data.Remove("USER_PASSWD");
            }
            else
                data["USER_PASSWD"] = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data["USER_PASSWD"].ToString(), "MD5");

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                //TODO 这里一定要改成参数形式的SQL语句防止SQL攻击
                DbUtilityManager.Instance.DefaultDbUtility.Execute(session, "DELETE FROM FW_S_USERS_M_ROLES WHERE USER_ID='" + Request["OBJECT_ID"] + "'", null);
                DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session, "FW_S_USERS", data, false);
                if (!String.IsNullOrEmpty(Request["ROLES_ID"]))
                {
                    string[] roleIds = Request["ROLES_ID"].Split(',');
                    foreach (string roleId in roleIds)
                    {
                        UnCaseSenseHashTable record = new UnCaseSenseHashTable();
                        record["USER_ID"] = Request["OBJECT_ID"];
                        record["ROLE_ID"] = roleId;
                        DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_USERS_M_ROLES", record);
                    }
                }
                session.Commit();
                session.Close();
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

        public ActionResult ActionChangeState()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            string id = "";
            int state = 0;
            try
            {
                id = Request["OBJECT_ID"].Replace("\'", "").Replace("\"", "");
                state = int.Parse(Request["state"]);
            }
            catch (Exception ee)
            {
                return Json(new { success = false, message = "主键值有误【" + id + "】" }, JsonRequestBehavior.AllowGet);
            }

            //这里要判断一下用户的角色级别
            FWUserInfo userTarget = new FWUserInfo();
            if (!userTarget.LoadData(Request["OBJECT_ID"]))
                return Json(new { success = false, message = "该用户不存在" }, JsonRequestBehavior.AllowGet);
            if (userTarget.RoleLevel <= Membership.CurrentUser.RoleLevel)
                return Json(new { success = false, message = "当前登陆用户的角色级别过低，不允许操作" }, JsonRequestBehavior.AllowGet);

            if (state == 0)//启用的话，要检查一下单位信息，看单位有没被停用
            {
                try
                {
                    UnCaseSenseHashTable recordUser = DbUtilityManager.Instance.DefaultDbUtility.GetOneRecord("FW_S_USERS", id, "USER_ID,COMPANY_ID,(SELECT c.DISABLED as C_DISABLED FROM FW_S_COMAPANIES as c where c.ID=COMPANY_ID) as C_DISABLED");
                    if (recordUser == null)
                        return Json(new { success = false, message = "用有此用户：" + id }, JsonRequestBehavior.AllowGet);
                    if (recordUser.GetDecimalValue("C_DISABLED") != 0)
                        return Json(new { success = false, message = "因为所属单位被停用，所以不能启用本用户" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, message = e.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  FW_S_USERS set DISABLED=" + state + " where USER_ID=\'" + id + "\'");
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
            //#region 初始化基本查询参数 id,limit,offset,search,sort,order

            Condition cdtMyCompany = new Condition();

            if (!Membership.CurrentUser.HaveAuthority("SYS__USER__SELECT_OTHOR_COMPANY"))
                cdtMyCompany.AddSubCondition("AND", "COMPANY_ID", "=", Membership.CurrentUser.CompanyId);

            string USER_ID = Request["USER_ID"];
            string USER_NAME = Request["USER_NAME"];
            string COMPANY_ID = Request["COMPANY_ID"];
            string CREATE_ON = Request["CREATE_ON"];
            string START_DATE = Request["START_DATE"];
            string END_DATE = Request["END_DATE"];

            if (!String.IsNullOrEmpty(USER_ID))
            {
                cdtMyCompany.AddSubCondition("AND", "USER_ID", "like", "%" + USER_ID + "%");
            }
            if (!String.IsNullOrEmpty(USER_NAME))
            {
                cdtMyCompany.AddSubCondition("AND", "USER_NAME", "like", "%" + USER_NAME + "%");
            }
            if (!String.IsNullOrEmpty(COMPANY_ID))
            {
                cdtMyCompany.AddSubCondition("AND", "COMPANY_ID", "=", COMPANY_ID);
            }
            if (!String.IsNullOrEmpty(CREATE_ON))
            {
                cdtMyCompany.AddSubCondition("AND", "CREATE_ON", "=", Convert.ToDateTime(CREATE_ON));
            }
            if (!String.IsNullOrEmpty(START_DATE))
            {
                cdtMyCompany.AddSubCondition("AND", "START_DATE", ">=", Convert.ToDateTime(START_DATE));
            }
            if (!String.IsNullOrEmpty(END_DATE))
            {
                cdtMyCompany.AddSubCondition("AND", "END_DATE", "<=", Convert.ToDateTime(END_DATE));
            }

            return QueryDataFromEasyUIDataGrid("FW_S_USERS", "CRATE_ON,UPDATE_ON", "USER_ID,USER_NAME", cdtMyCompany, "USER_ID,USER_NAME,ROLES_ID,ROLES_ID_V_D_FW_S_ROLES__MC,COMPANY_ID,COMPANY_ID_V_D_FW_COMP__MC,DISABLED,CREATE_BY,CREATE_ON,UPDATE_BY,UPDATE_ON,TEL1,TEL2,EMAIL,ADDR");
        }

        public ActionResult JsonRoles(string user_id)
        {
            Condition cdtMain = new Condition();
            //如果当前不是超级管理员，只查询自己有的角色。
            if (Membership.CurrentUser.RoleLevel > 0)
            {
                Condition cdtIsMe = new Condition();
                foreach (UnCaseSenseHashTable record in Membership.CurrentUser.GetRolesId(true))
                {
                    cdtIsMe.AddSubCondition("OR", "ID", "=", record["ID"]);
                }
                foreach (UnCaseSenseHashTable record in Membership.CurrentUser.GetRolesId(false))
                {
                    cdtIsMe.AddSubCondition("OR", "ID", "=", record["ID"]);
                }
                cdtMain.AddSubCondition(cdtIsMe);
            }
            List<UnCaseSenseHashTable> rows = DbUtilityManager.Instance.DefaultDbUtility.Query("FW_S_ROLES", cdtMain, "*", "SORT_CODE,ID", null, -1, -1);
            int total = DbUtilityManager.Instance.DefaultDbUtility.RecordCount("FW_S_ROLES", cdtMain);

            Condition cdt_uid_c_rid = new Condition();
            cdt_uid_c_rid.AddSubCondition("AND", "USER_ID", "=", user_id);
            Condition cdt_rid = new Condition("AND", "ROLE_ID", "=", "后面循环会改这个TAG,所以这里随便设");
            cdt_uid_c_rid.AddSubCondition(cdt_rid);
            foreach (UnCaseSenseHashTable r in rows)
            {
                if (String.IsNullOrEmpty(user_id))
                    r["CHECKED"] = false;
                else
                {
                    cdt_rid.Tag = r["ID"];
                    r["CHECKED"] = DbUtilityManager.Instance.DefaultDbUtility.RecordCount("FW_S_USERS_M_ROLES", cdt_uid_c_rid) > 0;
                }
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JsonCompanies()
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
                cdtIds.AddSubCondition("AND", "DM", "like", comId3);
            }
            return this.QueryDataFromEasyUIDataGrid("V_D_FW_COMP", null, "DM,MC", cdtIds, "DM,MC");
        }
    }
}