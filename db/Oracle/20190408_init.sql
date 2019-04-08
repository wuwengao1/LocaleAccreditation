--------------------------------------------
-- Export file for user LOCALE@WWGORCL    --
-- Created by KK on 2019/4/8, ���� 11:45:13 --
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
  is '�豸���';
comment on column B_MACHINE.ssdw
  is '�豸������λ���';
comment on column B_MACHINE.ssdw_v_d_fw_comp__mc
  is '�豸������λ����';
comment on column B_MACHINE.sbfzr
  is '�豸�����˱��';
comment on column B_MACHINE.sbfzr_v_d_fw_s_users__mc
  is '�豸����������';
comment on column B_MACHINE.disabled
  is '�Ƿ�����(0-����,1-����)';
comment on column B_MACHINE.deleted_mark
  is '�Ƿ�ɾ��(0-��,1-��)';
comment on column B_MACHINE.create_by
  is '�豸¼����';
comment on column B_MACHINE.create_on
  is '�豸¼��ʱ��';
comment on column B_MACHINE.update_by
  is '�豸�޸���';
comment on column B_MACHINE.update_on
  is '�豸�޸�ʱ��';
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
  is '�豸���';
comment on column B_MACHINE_ZZY.zzy_id
  is '��֤Աid';
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
  is '��֤Ա����';
comment on column B_ZZY.ssdw
  is '������λ���';
comment on column B_ZZY.ssdw_v_d_fw_comp__mc
  is '������λ����';
comment on column B_ZZY.create_by
  is '������';
comment on column B_ZZY.create_on
  is '����ʱ��';
comment on column B_ZZY.update_by
  is '�޸���';
comment on column B_ZZY.update_on
  is '�޸�ʱ��';
comment on column B_ZZY.disabled
  is '�Ƿ�����(0-����,1-����)';
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
  is 'ϵͳ���';
comment on column C_JZZ_TMP.slbh
  is '������';
comment on column C_JZZ_TMP.xm
  is '����';
comment on column C_JZZ_TMP.gmsfhm
  is '������ݺ���';
comment on column C_JZZ_TMP.xb
  is '�Ա� �ֵ䣺01';
comment on column C_JZZ_TMP.mz
  is '���� �ֵ�: 01';
comment on column C_JZZ_TMP.csrq
  is '��������';
comment on column C_JZZ_TMP.fwcs
  is '��������������λ��';
comment on column C_JZZ_TMP.zzsy
  is '��\��ס����';
comment on column C_JZZ_TMP.hjdzssxqmc
  is '�������ڵ�';
comment on column C_JZZ_TMP.xjzdzqz
  is '�־�ס��ַ';
comment on column C_JZZ_TMP.jzzyxqqsrq
  is '��ס֤��Ч����ʼ����';
comment on column C_JZZ_TMP.jzzyxqjzrq
  is '��ס֤��Ч�ڽ�ֹ����';
comment on column C_JZZ_TMP.ffdwmc
  is '��֤��λ';
comment on column C_JZZ_TMP.reservation01
  is '��ӡ�õĻ�����ַ';
comment on column C_JZZ_TMP.reservation02
  is '��ӡ�õľ�ס��ַ';
comment on column C_JZZ_TMP.reservation03
  is '��֤���ά����־0-δά����1-��ά��';
comment on column C_JZZ_TMP.reservation36
  is 'ǩ������';
comment on column C_JZZ_TMP.reservation37
  is 'ά��ʱ�䣨��֤��ɣ�';
comment on column C_JZZ_TMP.reservation40
  is '���ݵ���ʱ��';
comment on column C_JZZ_TMP.zzxxxrsj
  is '��Ϣд��ʱ��';
comment on column C_JZZ_TMP.zzxxsfdq
  is '��Ϣ�Ƿ��ȡ;1��֤����';
comment on column C_JZZ_TMP.zzxxdqsj
  is '��Ϣ��ȡʱ��';
comment on column C_JZZ_TMP.zzxxzzsfcg
  is '��֤�Ƿ�ɹ�: 0=��;1=��;';
comment on column C_JZZ_TMP.zzjgfksj
  is '��Ϣ����ʱ��';
comment on column C_JZZ_TMP.zzxxcwlx
  is '��֤��Ϣ���������ʹ���';
comment on column C_JZZ_TMP.zzxxcwlxmc
  is '��֤��Ϣ��������������';
comment on column C_JZZ_TMP.zzzxph
  is '��֤оƬ��';
comment on column C_JZZ_TMP.zzxxzzrq
  is '��֤����';
comment on column C_JZZ_TMP.zzxxzzdw
  is '��֤��λ����';
comment on column C_JZZ_TMP.zzxxzzdwmc
  is '��֤��λ����';
comment on column C_JZZ_TMP.hjdxz
  is '��������ַ';

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
