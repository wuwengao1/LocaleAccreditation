using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Authorize;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MisFrameWork3.Areas.FWBase.Controllers
{
    [Logined(OperateId = "SYS.ROLE")]
    public class RolesController : FWBaseController
    {
        // GET: FWBase/Roles
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
            data["ROLE_NAME"] = Request["ROLE_NAME"];
            data["ROLE_DESCRIPT"] = Request["ROLE_DESCRIPT"];
            data["SORT_CODE"] = Request["SORT_CODE"];
            
            string[] allOperate = new string[0];
            if (!string.IsNullOrEmpty(Request["OPERATE"]))
            {
                allOperate = Request["OPERATE"].Split(',');
            }

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                
                session.BeginTransaction();
                DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_ROLES", data);
                
                foreach(string op in allOperate)
                {
                    UnCaseSenseHashTable operate = new UnCaseSenseHashTable();
                    operate["ROLE_ID"] = data["ID"];
                    operate["OPERATE_ID"] = op;
                    DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_ROLES_OPERATE", operate);                    
                }
                
                session.Commit();
                session.Close();
            }
            catch(Exception e)
            {
                session.Rollback();
                session.Close();
                var eResult = new { success = false, message = e.ToString() };
                return Json(eResult, JsonRequestBehavior.AllowGet); ;
            }
            return Json(new { success = true, message = "保存成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActionEdit()
        {
            UnCaseSenseHashTable data = new UnCaseSenseHashTable();
            data["ID"] = Request["OBJECT_ID"];
            data["ROLE_NAME"] = Request["ROLE_NAME"];
            data["ROLE_DESCRIPT"] = Request["ROLE_DESCRIPT"];
            data["SORT_CODE"] = Request["SORT_CODE"];

            string[] allOperate = new string[0];
            if (!string.IsNullOrEmpty(Request["OPERATE"]))
            {
                allOperate = Request["OPERATE"].Split(',');
            }

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                DbUtilityManager.Instance.DefaultDbUtility.UpdateRecord(session, "FW_S_ROLES", data, false);
                DbUtilityManager.Instance.DefaultDbUtility.Execute("delete from FW_S_ROLES_OPERATE where ROLE_ID=" + int.Parse(data["ID"].ToString()));
                foreach (string op in allOperate)
                {
                    UnCaseSenseHashTable operate = new UnCaseSenseHashTable();
                    operate["ROLE_ID"] = data["ID"];
                    operate["OPERATE_ID"] = op;
                    DbUtilityManager.Instance.DefaultDbUtility.InsertRecord(session, "FW_S_ROLES_OPERATE", operate);
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
            catch(Exception ee)
            {
                return Json(new { success = false, message = "主键值有误【"+id+"】" }, JsonRequestBehavior.AllowGet);
            }

            Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            try
            {
                session.BeginTransaction();
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  FW_S_ROLES_OPERATE set DISABLED="+state+" where ROLE_ID="+id.ToString());
                DbUtilityManager.Instance.DefaultDbUtility.Execute("update  FW_S_ROLES set DISABLED=" + state + " where ID=" + id.ToString());
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

        public ActionResult ActionDelete()
        {
            return View();
        }

        public ActionResult JsonDataList()
        {
            //#region 初始化基本查询参数 id,limit,offset,search,sort,order
            return QueryDataFromEasyUIDataGrid("FW_S_ROLES", "CRATE_ON,UPDATE_ON", "ROLE_NAME,ROLE_DESCRIPT",null, "*");
        }

        public ActionResult JsonMenuData()
        {
            string object_id = Request["OBJECT_ID"];
            List<UnCaseSenseHashTable> authority = new List<UnCaseSenseHashTable>();
            if (object_id!=null)
            {
                Condition cdt = new Condition();
                cdt.AddSubCondition("AND", "ROLE_ID", "=", object_id);
                authority = DbUtilityManager.Instance.DefaultDbUtility.Query("FW_S_ROLES_OPERATE", cdt, "*", null);
            }
            string filename = System.Web.HttpRuntime.AppDomainAppPath + "Configs\\menu.config";
            String strJson = "";
            StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                strJson += line.ToString();
            }
            sr.Close();

            JArray jsonObj = JArray.Parse(strJson);
            JArray jsonResult = new JArray();
            CalNodes("",jsonObj, ref jsonResult, authority);
            return this.Content(jsonResult.ToString(), "application/json");
        }

        /// <summary>
        /// 判断是否具备oprate权限
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="oprate"></param>
        /// <returns></returns>
        private bool HaveAuthority(List<UnCaseSenseHashTable> authority,string oprate)
        {
            foreach(UnCaseSenseHashTable record in authority)
            {
                if (record.HasValue("OPERATE_ID") && record["OPERATE_ID"].Equals(oprate))
                    return true;
            }
            return false;
        }

        public void CalNodes(string parentId,JArray config, ref JArray r, List<UnCaseSenseHashTable> authority)
        {
            for (int i = 0;i<config.Count;i++)
            {
                JObject node =(JObject)config[i];
                JObject obj = new JObject();
                obj["id"] = node["id"];
                obj["text"] = node["text"];
                if (node["id"].ToString().StartsWith("GLOBAL")){
                    obj["iconCls"] = "fa-treenode-icon fa fa-cogs";
                    if(node["children"]==null)
                        obj["iconCls"] = "fa-treenode-icon fa fa-cog";
                }
                    

                if (node["children"]==null && HaveAuthority(authority, obj["id"].ToString()))
                    obj["checked"] = true;

                if (node["iconCls"]!=null)
                    obj["iconCls"] = node["iconCls"];
                if (node["url"] != null)
                {
                    obj["attributes"] = new JObject();
                    obj["attributes"]["url"] = node["url"];
                }
                if (node["children"] != null)
                {
                    JArray children = (JArray)node["children"];
                    JArray chilerenObj = new JArray();
                    obj["children"] = chilerenObj;
                    CalNodes(obj["id"].ToString(),children, ref chilerenObj, authority);
                }
                r.Add(obj);
                if (node["option"] != null)
                {
                    //增加一个节点
                    JObject objOption = new JObject();
                    objOption["id"] = obj["id"].ToString()+".__OPTION__";
                    objOption["iconCls"] = "fa-treenode-icon fa fa-cogs";
                    objOption["text"] = obj["text"]+" 【可选项】";
                    JArray chilerenObjOption = new JArray();
                    objOption["children"] = chilerenObjOption;

                    string[] option = node["option"].ToString().Split(',');
                    foreach (string op in option)
                    {
                        string[] infor = op.Split(':');
                        JObject subOption = new JObject();
                        subOption["id"] = obj["id"].ToString()+"."+infor[0];
                        subOption["iconCls"] = "fa-treenode-icon fa fa-cog";
                        //subOption["text"] = "<span class=\"fa fa-cog\">" + infor[1];
                        subOption["text"] = infor[1];
                        if (HaveAuthority(authority, subOption["id"].ToString()))
                            subOption["checked"] = true;
                        chilerenObjOption.Add(subOption);
                    }
                    r.Add(objOption);
                }
                
            }
        }
    }
}