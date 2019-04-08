--------------------------------------------
-- Export file for user LOCALE@WWGORCL    --
-- Created by KK on 2019/4/8, 上午 11:45:13 --
--------------------------------------------

set define off
spool 20190408_init.log

prompt
prompt Creating table B_MACHINE
prompt ========================
prompt
create table B_MACHINE
(
  id                       NUMBER not null,
  machineno                VARCHAR2(255),
  ssdw                     VARCHAR2(50),
  ssdw_v_d_fw_comp__mc     VARCHAR2(255),
  sbfzr                    VARCHAR2(50),
  sbfzr_v_d_fw_s_users__mc VARCHAR2(255),
  disabled                 NUMBER default 0,
  deleted_mark             NUMBER default 0,
  create_by                VARCHAR2(50),
  create_on                DATE,
  update_by                VARCHAR2(50),
  update_on                DATE
)
;
comment on column B_MACHINE.machineno
  is '设备编号';
comment on column B_MACHINE.ssdw
  is '设备所属单位编号';
comment on column B_MACHINE.ssdw_v_d_fw_comp__mc
  is '设备所属单位名称';
comment on column B_MACHINE.sbfzr
  is '设备负责人编号';
comment on column B_MACHINE.sbfzr_v_d_fw_s_users__mc
  is '设备负责人名称';
comment on column B_MACHINE.disabled
  is '是否启用(0-启用,1-禁用)';
comment on column B_MACHINE.deleted_mark
  is '是否删除(0-否,1-是)';
comment on column B_MACHINE.create_by
  is '设备录入人';
comment on column B_MACHINE.create_on
  is '设备录入时间';
comment on column B_MACHINE.update_by
  is '设备修改人';
comment on column B_MACHINE.update_on
  is '设备修改时间';
alter table B_MACHINE
  add constraint PK_B_MACHINE primary key (ID);

prompt
prompt Creating table B_MACHINE_ZZY
prompt ============================
prompt
create table B_MACHINE_ZZY
(
  id        NUMBER not null,
  machineno VARCHAR2(255),
  zzy_id    VARCHAR2(255)
)
;
comment on column B_MACHINE_ZZY.machineno
  is '设备编号';
comment on column B_MACHINE_ZZY.zzy_id
  is '制证员id';
alter table B_MACHINE_ZZY
  add constraint PK_B_MACHINE_ZZY primary key (ID);

prompt
prompt Creating table B_ZZY
prompt ====================
prompt
create table B_ZZY
(
  id                   NUMBER not null,
  zzy_name             VARCHAR2(50),
  ssdw                 VARCHAR2(50),
  ssdw_v_d_fw_comp__mc VARCHAR2(255),
  create_by            VARCHAR2(50),
  create_on            DATE,
  update_by            VARCHAR2(50),
  update_on            DATE,
  disabled             NUMBER default 0
)
;
comment on column B_ZZY.zzy_name
  is '制证员名称';
comment on column B_ZZY.ssdw
  is '所属单位编号';
comment on column B_ZZY.ssdw_v_d_fw_comp__mc
  is '所属单位名称';
comment on column B_ZZY.create_by
  is '创建人';
comment on column B_ZZY.create_on
  is '创建时间';
comment on column B_ZZY.update_by
  is '修改人';
comment on column B_ZZY.update_on
  is '修改时间';
comment on column B_ZZY.disabled
  is '是否启用(0-请用,1-禁用)';
alter table B_ZZY
  add constraint PK_B_ZZY primary key (ID);

prompt
prompt Creating table C_JZZ_TMP
prompt ========================
prompt
create table C_JZZ_TMP
(
  systemid      VARCHAR2(100),
  slbh          VARCHAR2(48) not null,
  zzsbid        VARCHAR2(48),
  xm            NVARCHAR2(60),
  gmsfhm        VARCHAR2(36),
  xb            VARCHAR2(2),
  mz            VARCHAR2(4),
  csrq          DATE,
  fwcs          NVARCHAR2(240),
  zzsy          VARCHAR2(4),
  hjdzssxqmc    VARCHAR2(240),
  xjzdzqz       NVARCHAR2(240),
  jzzyxqqsrq    DATE,
  jzzyxqjzrq    DATE,
  ffdwmc        VARCHAR2(240),
  reservation01 VARCHAR2(400),
  reservation02 VARCHAR2(400),
  reservation03 VARCHAR2(400),
  reservation04 VARCHAR2(400),
  reservation05 VARCHAR2(400),
  reservation06 VARCHAR2(400),
  reservation07 VARCHAR2(400),
  reservation08 VARCHAR2(400),
  reservation09 VARCHAR2(400),
  reservation10 VARCHAR2(400),
  reservation11 VARCHAR2(400),
  reservation12 VARCHAR2(400),
  reservation13 VARCHAR2(400),
  reservation14 VARCHAR2(400),
  reservation15 VARCHAR2(400),
  reservation16 VARCHAR2(400),
  reservation17 VARCHAR2(400),
  reservation18 VARCHAR2(400),
  reservation19 VARCHAR2(400),
  reservation20 VARCHAR2(400),
  reservation21 VARCHAR2(400),
  reservation22 VARCHAR2(400),
  reservation23 VARCHAR2(400),
  reservation24 VARCHAR2(400),
  reservation25 VARCHAR2(400),
  reservation26 VARCHAR2(400),
  reservation27 VARCHAR2(400),
  reservation28 VARCHAR2(400),
  reservation29 VARCHAR2(400),
  reservation30 VARCHAR2(400),
  reservation31 VARCHAR2(400),
  reservation32 VARCHAR2(400),
  reservation33 VARCHAR2(400),
  reservation34 VARCHAR2(400),
  reservation35 VARCHAR2(400),
  reservation36 DATE,
  reservation37 DATE,
  reservation38 DATE,
  reservation39 DATE,
  reservation40 DATE default sysdate,
  hjdzssxq      VARCHAR2(12),
  mzmc          VARCHAR2(60),
  ffdw          VARCHAR2(24),
  zzxxxrsj      DATE,
  zzxxsfdq      VARCHAR2(2),
  zzxxdqsj      DATE,
  zzxxzzsfcg    VARCHAR2(2),
  zzjgfksj      DATE,
  zzxxcwlx      VARCHAR2(6),
  zzxxcwlxmc    VARCHAR2(240),
  zzzxph        VARCHAR2(100),
  zzjghxsfcg    VARCHAR2(2),
  zzjghxsj      DATE,
  zzxxzzrq      DATE,
  zzxxzzdw      VARCHAR2(40),
  zzxxzzdwmc    VARCHAR2(200),
  hjdxz         VARCHAR2(600),
  unc           NVARCHAR2(50)
)
;
comment on column C_JZZ_TMP.systemid
  is '系统编号';
comment on column C_JZZ_TMP.slbh
  is '受理编号';
comment on column C_JZZ_TMP.xm
  is '姓名';
comment on column C_JZZ_TMP.gmsfhm
  is '公民身份号码';
comment on column C_JZZ_TMP.xb
  is '性别 字典：01';
comment on column C_JZZ_TMP.mz
  is '民族 字典: 01';
comment on column C_JZZ_TMP.csrq
  is '出生日期';
comment on column C_JZZ_TMP.fwcs
  is '服务处所（工作单位）';
comment on column C_JZZ_TMP.zzsy
  is '暂\居住事由';
comment on column C_JZZ_TMP.hjdzssxqmc
  is '户籍所在地';
comment on column C_JZZ_TMP.xjzdzqz
  is '现居住地址';
comment on column C_JZZ_TMP.jzzyxqqsrq
  is '居住证有效期起始日期';
comment on column C_JZZ_TMP.jzzyxqjzrq
  is '居住证有效期截止日期';
comment on column C_JZZ_TMP.ffdwmc
  is '发证单位';
comment on column C_JZZ_TMP.reservation01
  is '打印用的户籍地址';
comment on column C_JZZ_TMP.reservation02
  is '打印用的居住地址';
comment on column C_JZZ_TMP.reservation03
  is '制证完成维护标志0-未维护，1-已维护';
comment on column C_JZZ_TMP.reservation36
  is '签发日期';
comment on column C_JZZ_TMP.reservation37
  is '维护时间（制证完成）';
comment on column C_JZZ_TMP.reservation40
  is '数据调度时间';
comment on column C_JZZ_TMP.zzxxxrsj
  is '信息写入时间';
comment on column C_JZZ_TMP.zzxxsfdq
  is '信息是否读取;1制证锁定';
comment on column C_JZZ_TMP.zzxxdqsj
  is '信息读取时间';
comment on column C_JZZ_TMP.zzxxzzsfcg
  is '制证是否成功: 0=否;1=是;';
comment on column C_JZZ_TMP.zzjgfksj
  is '信息反馈时间';
comment on column C_JZZ_TMP.zzxxcwlx
  is '制证信息：错误类型代码';
comment on column C_JZZ_TMP.zzxxcwlxmc
  is '制证信息：错误类型名称';
comment on column C_JZZ_TMP.zzzxph
  is '制证芯片号';
comment on column C_JZZ_TMP.zzxxzzrq
  is '制证日期';
comment on column C_JZZ_TMP.zzxxzzdw
  is '制证单位代码';
comment on column C_JZZ_TMP.zzxxzzdwmc
  is '制证单位名称';
comment on column C_JZZ_TMP.hjdxz
  is '户籍地详址';

prompt
prompt Creating table FW_S_ATTACHED
prompt ============================
prompt
create table FW_S_ATTACHED
(
  id            VARCHAR2(250) not null,
  server_id     NUMBER,
  dir           VARCHAR2(15),
  src_file_name VARCHAR2(250),
  file_ext      VARCHAR2(10),
  file_size     NUMBER,
  ymd           VARCHAR2(8),
  create_by     VARCHAR2(50),
  create_on     DATE,
  update_by     VARCHAR2(50),
  update_on     DATE
)
;
alter table FW_S_ATTACHED
  add constraint PK_FW_S_ATTACHED primary key (ID);

prompt
prompt Creating table FW_S_COMAPANIES
prompt ==============================
prompt
create table FW_S_COMAPANIES
(
  id                       NUMBER not null,
  name                     VARCHAR2(50) not null,
  company_code             VARCHAR2(40),
  tel1                     VARCHAR2(36),
  tel2                     VARCHAR2(36),
  fax                      VARCHAR2(36),
  addr_working             VARCHAR2(256),
  remark                   VARCHAR2(256),
  liaisons                 VARCHAR2(50),
  corporate_representative VARCHAR2(50),
  cr_code                  VARCHAR2(50),
  disabled                 NUMBER default 0 not null,
  create_by                VARCHAR2(100),
  create_on                DATE,
  update_by                VARCHAR2(100),
  update_on                DATE,
  deleted_mark             NUMBER not null,
  ver                      NUMBER
)
;
alter table FW_S_COMAPANIES
  add constraint PK_FW_S_COMAPANIES primary key (ID);

prompt
prompt Creating table FW_S_DB_VERSION
prompt ==============================
prompt
create table FW_S_DB_VERSION
(
  db_version VARCHAR2(50) not null
)
;
alter table FW_S_DB_VERSION
  add constraint PK_FW_S_DB_VERSION primary key (DB_VERSION);

prompt
prompt Creating table FW_S_ROLES
prompt =========================
prompt
create table FW_S_ROLES
(
  id            NUMBER not null,
  sort_code     NUMBER,
  role_name     VARCHAR2(50) not null,
  role_descript VARCHAR2(256),
  disabled      NUMBER default 0 not null,
  create_by     VARCHAR2(100),
  create_on     DATE,
  update_by     VARCHAR2(100),
  update_on     DATE,
  deleted_mark  NUMBER not null,
  ver           NUMBER
)
;
alter table FW_S_ROLES
  add constraint PK_FW_S_ROLES primary key (ID);

prompt
prompt Creating table FW_S_ROLES_OPERATE
prompt =================================
prompt
create table FW_S_ROLES_OPERATE
(
  id         NUMBER not null,
  role_id    NUMBER not null,
  operate_id VARCHAR2(200) not null,
  disabled   NUMBER default 0 not null
)
;
alter table FW_S_ROLES_OPERATE
  add constraint PK_FW_S_ROLES_OPERATE primary key (ID);

prompt
prompt Creating table FW_S_SYS_GENERATE_INFO
prompt =====================================
prompt
create table FW_S_SYS_GENERATE_INFO
(
  id  NUMBER not null,
  tn  VARCHAR2(100),
  ki  VARCHAR2(100),
  seq NUMBER
)
;
alter table FW_S_SYS_GENERATE_INFO
  add constraint PK_FW_S_SYS_GENERATE_INFO primary key (ID);

prompt
prompt Creating table FW_S_SYS_LOG
prompt ===========================
prompt
create table FW_S_SYS_LOG
(
  id          NUMBER(38) not null,
  create_time DATE,
  thread      VARCHAR2(50),
  log_level   VARCHAR2(50),
  logger      VARCHAR2(200),
  message     VARCHAR2(2048),
  exception   VARCHAR2(2048)
)
;
alter table FW_S_SYS_LOG
  add constraint PK_FW_S_SYS_LOG primary key (ID);

prompt
prompt Creating table FW_S_USERS
prompt =========================
prompt
create table FW_S_USERS
(
  user_id                     VARCHAR2(50) not null,
  user_name                   VARCHAR2(100) not null,
  user_passwd                 VARCHAR2(32) not null,
  logined_flags               NUMBER,
  roles_id                    VARCHAR2(512),
  roles_id_v_d_fw_s_roles__mc VARCHAR2(512),
  ver                         NUMBER,
  disabled                    NUMBER,
  create_by                   VARCHAR2(50),
  create_on                   DATE,
  update_by                   VARCHAR2(50),
  update_on                   DATE,
  deleted_mark                NUMBER not null,
  company_id                  VARCHAR2(50),
  company_id_v_d_fw_comp__mc  VARCHAR2(512),
  tel1                        VARCHAR2(23),
  tel2                        VARCHAR2(23),
  email                       VARCHAR2(100),
  addr                        VARCHAR2(100)
)
;
alter table FW_S_USERS
  add constraint PK_FW_S_USERS primary key (USER_ID);

prompt
prompt Creating table FW_S_USERS_M_ROLES
prompt =================================
prompt
create table FW_S_USERS_M_ROLES
(
  id       NUMBER(38) not null,
  user_id  VARCHAR2(50),
  role_id  NUMBER,
  disabled NUMBER
)
;
alter table FW_S_USERS_M_ROLES
  add constraint PK_FW_S_USERS_M_ROLES primary key (ID);

prompt
prompt Creating sequence SEQ_B_MACHINE
prompt ===============================
prompt
create sequence SEQ_B_MACHINE
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_B_MACHINE_ZZY
prompt ===================================
prompt
create sequence SEQ_B_MACHINE_ZZY
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_B_ZZY
prompt ===========================
prompt
create sequence SEQ_B_ZZY
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_ATTACHED
prompt ===================================
prompt
create sequence SEQ_FW_S_ATTACHED
minvalue 1
maxvalue 9999999999999999999999999999
start with 1000
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_COMAPANIES
prompt =====================================
prompt
create sequence SEQ_FW_S_COMAPANIES
minvalue 1
maxvalue 9999999999999999999999999999
start with 1020
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_ROLES
prompt ================================
prompt
create sequence SEQ_FW_S_ROLES
minvalue 1
maxvalue 9999999999999999999999999999
start with 1020
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_ROLES_OPERATE
prompt ========================================
prompt
create sequence SEQ_FW_S_ROLES_OPERATE
minvalue 1
maxvalue 9999999999999999999999999999
start with 1340
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_SYS_LOG
prompt ==================================
prompt
create sequence SEQ_FW_S_SYS_LOG
minvalue 1
maxvalue 9999999999999999999999999999
start with 1000
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_USERS
prompt ================================
prompt
create sequence SEQ_FW_S_USERS
minvalue 1
maxvalue 9999999999999999999999999999
start with 1060
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_FW_S_USERS_M_ROLES
prompt ========================================
prompt
create sequence SEQ_FW_S_USERS_M_ROLES
minvalue 1
maxvalue 9999999999999999999999999999
start with 1100
increment by 1
cache 20;

prompt
prompt Creating view V_D_B_ZZY
prompt =======================
prompt
create or replace force view v_d_b_zzy as
select z.ID as DM,z.ZZY_NAME AS MC,z."ID",z."ZZY_NAME",z."SSDW",z."SSDW_V_D_FW_COMP__MC",z."CREATE_BY",z."CREATE_ON",z."UPDATE_BY",z."UPDATE_ON"
    from B_ZZY z;

prompt
prompt Creating view V_D_FW_COMP
prompt =========================
prompt
CREATE OR REPLACE FORCE VIEW V_D_FW_COMP AS
SELECT
FW_S_COMAPANIES.COMPANY_CODE AS DM,
FW_S_COMAPANIES.NAME AS MC,
FW_S_COMAPANIES.ID,
FW_S_COMAPANIES.TEL1,
FW_S_COMAPANIES.TEL2,
FW_S_COMAPANIES.FAX,
FW_S_COMAPANIES.ADDR_WORKING,
FW_S_COMAPANIES.REMARK,
FW_S_COMAPANIES.LIAISONS,
FW_S_COMAPANIES.CORPORATE_REPRESENTATIVE,
FW_S_COMAPANIES.CR_CODE,
FW_S_COMAPANIES.DISABLED,
FW_S_COMAPANIES.CREATE_BY,
FW_S_COMAPANIES.CREATE_ON,
FW_S_COMAPANIES.UPDATE_BY,
FW_S_COMAPANIES.UPDATE_ON,
FW_S_COMAPANIES.DELETED_MARK,
FW_S_COMAPANIES.VER
FROM
FW_S_COMAPANIES;

prompt
prompt Creating view V_D_FW_S_ROLES
prompt ============================
prompt
CREATE OR REPLACE FORCE VIEW V_D_FW_S_ROLES AS
SELECT
FW_S_ROLES.ID AS DM,
FW_S_ROLES.ROLE_NAME AS MC,
'' AS WB,
'' AS PY
FROM
FW_S_ROLES;

prompt
prompt Creating view V_D_FW_S_USERS
prompt ============================
prompt
create or replace force view v_d_fw_s_users as
select u.user_id as DM,u.user_name as MC,u."USER_ID",u."USER_NAME",u."USER_PASSWD",u."LOGINED_FLAGS",u."ROLES_ID",u."ROLES_ID_V_D_FW_S_ROLES__MC",u."VER",u."DISABLED",u."CREATE_BY",u."CREATE_ON",u."UPDATE_BY",u."UPDATE_ON",u."DELETED_MARK",u."COMPANY_ID",u."COMPANY_ID_V_D_FW_COMP__MC",u."TEL1",u."TEL2",u."EMAIL",u."ADDR"
    from FW_S_USERS u;


spool off
