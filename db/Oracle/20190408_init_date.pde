SPISNON`�                 �J  �J   ��     Header             	   B_MACHINE      ID   NUMBER	   MACHINENO   VARCHAR2   SSDW   VARCHAR2   SSDW_V_D_FW_COMP__MC   VARCHAR2   SBFZR   VARCHAR2   SBFZR_V_D_FW_S_USERS__MC   VARCHAR2   DISABLED   NUMBER   DELETED_MARK   NUMBER	   CREATE_BY   VARCHAR2	   CREATE_ON   DATE	   UPDATE_BY   VARCHAR2	   UPDATE_ON   DATE�  create table B_MACHINE
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
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
  add constraint PK_B_MACHINE primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   B_MACHINE_ZZY      ID   NUMBER	   MACHINENO   VARCHAR2   ZZY_ID   VARCHAR2�  create table B_MACHINE_ZZY
(
  id        NUMBER not null,
  machineno VARCHAR2(255),
  zzy_id    VARCHAR2(255)
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
comment on column B_MACHINE_ZZY.machineno
  is '设备编号';
comment on column B_MACHINE_ZZY.zzy_id
  is '制证员id';
alter table B_MACHINE_ZZY
  add constraint PK_B_MACHINE_ZZY primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   B_ZZY	      ID   NUMBER   ZZY_NAME   VARCHAR2   SSDW   VARCHAR2   SSDW_V_D_FW_COMP__MC   VARCHAR2	   CREATE_BY   VARCHAR2	   CREATE_ON   DATE	   UPDATE_BY   VARCHAR2	   UPDATE_ON   DATE   DISABLED   NUMBER�  create table B_ZZY
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
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
  add constraint PK_B_ZZY primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
	   C_JZZ_TMPI      SYSTEMID   VARCHAR2   SLBH   VARCHAR2   ZZSBID   VARCHAR2   XM	   NVARCHAR2   GMSFHM   VARCHAR2   XB   VARCHAR2   MZ   VARCHAR2   CSRQ   DATE   FWCS	   NVARCHAR2   ZZSY   VARCHAR2
   HJDZSSXQMC   VARCHAR2   XJZDZQZ	   NVARCHAR2
   JZZYXQQSRQ   DATE
   JZZYXQJZRQ   DATE   FFDWMC   VARCHAR2   RESERVATION01   VARCHAR2   RESERVATION02   VARCHAR2   RESERVATION03   VARCHAR2   RESERVATION04   VARCHAR2   RESERVATION05   VARCHAR2   RESERVATION06   VARCHAR2   RESERVATION07   VARCHAR2   RESERVATION08   VARCHAR2   RESERVATION09   VARCHAR2   RESERVATION10   VARCHAR2   RESERVATION11   VARCHAR2   RESERVATION12   VARCHAR2   RESERVATION13   VARCHAR2   RESERVATION14   VARCHAR2   RESERVATION15   VARCHAR2   RESERVATION16   VARCHAR2   RESERVATION17   VARCHAR2   RESERVATION18   VARCHAR2   RESERVATION19   VARCHAR2   RESERVATION20   VARCHAR2   RESERVATION21   VARCHAR2   RESERVATION22   VARCHAR2   RESERVATION23   VARCHAR2   RESERVATION24   VARCHAR2   RESERVATION25   VARCHAR2   RESERVATION26   VARCHAR2   RESERVATION27   VARCHAR2   RESERVATION28   VARCHAR2   RESERVATION29   VARCHAR2   RESERVATION30   VARCHAR2   RESERVATION31   VARCHAR2   RESERVATION32   VARCHAR2   RESERVATION33   VARCHAR2   RESERVATION34   VARCHAR2   RESERVATION35   VARCHAR2   RESERVATION36   DATE   RESERVATION37   DATE   RESERVATION38   DATE   RESERVATION39   DATE   RESERVATION40   DATE   HJDZSSXQ   VARCHAR2   MZMC   VARCHAR2   FFDW   VARCHAR2   ZZXXXRSJ   DATE   ZZXXSFDQ   VARCHAR2   ZZXXDQSJ   DATE
   ZZXXZZSFCG   VARCHAR2   ZZJGFKSJ   DATE   ZZXXCWLX   VARCHAR2
   ZZXXCWLXMC   VARCHAR2   ZZZXPH   VARCHAR2
   ZZJGHXSFCG   VARCHAR2   ZZJGHXSJ   DATE   ZZXXZZRQ   DATE   ZZXXZZDW   VARCHAR2
   ZZXXZZDWMC   VARCHAR2   HJDXZ   VARCHAR2   UNC	   NVARCHAR2   create table C_JZZ_TMP
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
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
   FW_S_ATTACHED      ID   VARCHAR2	   SERVER_ID   NUMBER   DIR   VARCHAR2   SRC_FILE_NAME   VARCHAR2   FILE_EXT   VARCHAR2	   FILE_SIZE   NUMBER   YMD   VARCHAR2	   CREATE_BY   VARCHAR2	   CREATE_ON   DATE	   UPDATE_BY   VARCHAR2	   UPDATE_ON   DATE  create table FW_S_ATTACHED
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table FW_S_ATTACHED
  add constraint PK_FW_S_ATTACHED primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   FW_S_COMAPANIES      ID   NUMBER   NAME   VARCHAR2   COMPANY_CODE   VARCHAR2   TEL1   VARCHAR2   TEL2   VARCHAR2   FAX   VARCHAR2   ADDR_WORKING   VARCHAR2   REMARK   VARCHAR2   LIAISONS   VARCHAR2   CORPORATE_REPRESENTATIVE   VARCHAR2   CR_CODE   VARCHAR2   DISABLED   NUMBER	   CREATE_BY   VARCHAR2	   CREATE_ON   DATE	   UPDATE_BY   VARCHAR2	   UPDATE_ON   DATE   DELETED_MARK   NUMBER   VER   NUMBER�  create table FW_S_COMAPANIES
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table FW_S_COMAPANIES
  add constraint PK_FW_S_COMAPANIES primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   FW_S_DB_VERSION   
   DB_VERSION   VARCHAR23  create table FW_S_DB_VERSION
(
  db_version VARCHAR2(50) not null
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255;
alter table FW_S_DB_VERSION
  add constraint PK_FW_S_DB_VERSION primary key (DB_VERSION)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255;

   FW_S_ROLES      ID   NUMBER	   SORT_CODE   NUMBER	   ROLE_NAME   VARCHAR2   ROLE_DESCRIPT   VARCHAR2   DISABLED   NUMBER	   CREATE_BY   VARCHAR2	   CREATE_ON   DATE	   UPDATE_BY   VARCHAR2	   UPDATE_ON   DATE   DELETED_MARK   NUMBER   VER   NUMBER  create table FW_S_ROLES
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table FW_S_ROLES
  add constraint PK_FW_S_ROLES primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   FW_S_ROLES_OPERATE      ID   NUMBER   ROLE_ID   NUMBER
   OPERATE_ID   VARCHAR2   DISABLED   NUMBERZ  create table FW_S_ROLES_OPERATE
(
  id         NUMBER not null,
  role_id    NUMBER not null,
  operate_id VARCHAR2(200) not null,
  disabled   NUMBER default 0 not null
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table FW_S_ROLES_OPERATE
  add constraint PK_FW_S_ROLES_OPERATE primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   FW_S_SYS_GENERATE_INFO      ID   NUMBER   TN   VARCHAR2   KI   VARCHAR2   SEQ   NUMBERn  create table FW_S_SYS_GENERATE_INFO
(
  id  NUMBER not null,
  tn  VARCHAR2(100),
  ki  VARCHAR2(100),
  seq NUMBER
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255;
alter table FW_S_SYS_GENERATE_INFO
  add constraint PK_FW_S_SYS_GENERATE_INFO primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255;
   FW_S_SYS_LOG      ID   NUMBER   CREATE_TIME   DATE   THREAD   VARCHAR2	   LOG_LEVEL   VARCHAR2   LOGGER   VARCHAR2   MESSAGE   VARCHAR2	   EXCEPTION   VARCHAR2�  create table FW_S_SYS_LOG
(
  id          NUMBER(38) not null,
  create_time DATE,
  thread      VARCHAR2(50),
  log_level   VARCHAR2(50),
  logger      VARCHAR2(200),
  message     VARCHAR2(2048),
  exception   VARCHAR2(2048)
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255;
alter table FW_S_SYS_LOG
  add constraint PK_FW_S_SYS_LOG primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255;

   FW_S_USERS      USER_ID   VARCHAR2	   USER_NAME   VARCHAR2   USER_PASSWD   VARCHAR2   LOGINED_FLAGS   NUMBER   ROLES_ID   VARCHAR2   ROLES_ID_V_D_FW_S_ROLES__MC   VARCHAR2   VER   NUMBER   DISABLED   NUMBER	   CREATE_BY   VARCHAR2	   CREATE_ON   DATE	   UPDATE_BY   VARCHAR2	   UPDATE_ON   DATE   DELETED_MARK   NUMBER
   COMPANY_ID   VARCHAR2   COMPANY_ID_V_D_FW_COMP__MC   VARCHAR2   TEL1   VARCHAR2   TEL2   VARCHAR2   EMAIL   VARCHAR2   ADDR   VARCHAR2  create table FW_S_USERS
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
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table FW_S_USERS
  add constraint PK_FW_S_USERS primary key (USER_ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
   FW_S_USERS_M_ROLES      ID   NUMBER   USER_ID   VARCHAR2   ROLE_ID   NUMBER   DISABLED   NUMBER0  create table FW_S_USERS_M_ROLES
(
  id       NUMBER(38) not null,
  user_id  VARCHAR2(50),
  role_id  NUMBER,
  disabled NUMBER
)
tablespace USERS
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table FW_S_USERS_M_ROLES
  add constraint PK_FW_S_USERS_M_ROLES primary key (ID)
  using index 
  tablespace USERS
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
            b�      0.0   3   dd����������������   0   1����   04-04-2019 00:00:00��������   1	   AC4543243   450303000000   桂林市公安局叠彩分局   ZZY   ZZY   0   0����   04-04-2019 00:00:00   admin   04-04-2019 19:25:34   4
   AC56435454   450303000000   桂林市公安局叠彩分局   ZZY2   ZZY2   0   0����   04-04-2019 00:00:00   admin   04-04-2019 19:25:47   2   AC763465   450302000000   桂林市公安局秀峰分局   ZZY2   ZZY2   0   0����   04-04-2019 00:00:00   admin   04-04-2019 19:26:27   5   AC543   450303000000   桂林市公安局叠彩分局��������   0   0   admin   04-04-2019 14:26:37   admin   04-04-2019 19:23:19   6   AC67575����������������   0   0   admin   04-04-2019 14:35:04   admin   04-04-2019 19:23:23       M   M    (
      1.0   3
   AC56435454   ZZY2   4	   AC4543243   ZZY   5   AC763465   ZZY2       �  �   c�      2.0   3   dd   451102530000!   贺州市公安局桂岭派出所   GZS   04-04-2019 11:24:54��������   0   4   gg   450303000000   桂林市公安局叠彩分局   GZS   04-04-2019 11:30:38��������   0   1   梦   451102530000!   贺州市公安局桂岭派出所   admin   03-04-2019 19:06:46��������   0   2   高   450300000000   桂林市公安局   admin   04-04-2019 09:48:03��������   0       'K  'K   M�$     3.0   1   450100112000000001011	   AC4543243	   林青珊   45212619810820092X   2   08   20-08-1981 00:00:00��������   广西陆川县3   南宁市青秀区长湖路埌东村十组86栋3号   28-05-2012 00:00:00   28-05-2013 00:00:00   南宁市公安局南湖分局/   广西陆川县大桥镇陆透村吕屋队21号3   南宁市青秀区长湖路埌东村十组86栋3号   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000E53C03A1����   15-08-2012 04:00:19��������   08-06-2012 17:31:30����   壮   450115000000   29-05-2012 08:31:05   1   08-06-2012 17:31:30   1   14-08-2012 11:19:34��������    107800C00299000000000000E53C03A1��������   08-06-2012 17:39:52   450000   广西区公安厅/   广西陆川县大桥镇陆透村吕屋队21号����   1   450100112000000001034	   AC4543243	   陈彩凤   452625198003101184   2   08   10-03-1980 00:00:00��������   广西德保县3   南宁市青秀区长湖路埌东村十组54栋2号   28-05-2012 00:00:00   28-05-2013 00:00:00   南宁市公安局南湖分局.   广西德保县那甲乡定录村吸布屯3号3   南宁市青秀区长湖路埌东村十组54栋2号   1����������������������������������������������������������������������������������������������������������������������������    107800C0029900000000000011851099����   15-08-2012 04:00:19��������   08-06-2012 17:31:31����   壮   450115000000   29-05-2012 08:31:05   1   08-06-2012 17:31:31   1   14-08-2012 11:19:34��������    107800C0029900000000000011851099��������   08-06-2012 17:41:08   450000   广西区公安厅.   广西德保县那甲乡定录村吸布屯3号����   1   450100112000000002178	   AC4543243	   陶苏平   45033019891007192X   2   01   07-10-1989 00:00:00��������   广西平乐县)   南宁市良庆区建业路42号202号房   25-05-2012 00:00:00   25-05-2013 00:00:00   南宁市公安局良庆分局2   广西平乐县阳安乡雷峰村委雷峰村66号)   南宁市良庆区建业路42号202号房   1����������������������������������������������������������������������������������������������������������������������������    107800C002990000000000005E9E83C6����   15-08-2012 04:00:28��������   01-06-2012 15:36:12����   汉   450108000000   29-05-2012 08:35:49   1   01-06-2012 15:36:12   1   14-08-2012 11:10:16��������    107800C002990000000000005E9E83C6��������   01-06-2012 18:29:28   450000   广西区公安厅2   广西平乐县阳安乡雷峰村委雷峰村66号����   1   450100112000000002184	   AC4543243	   吴佳俊   452502197903193411   1   01   19-03-1979 00:00:00��������   广西贵港市港北区)   南宁市良庆区建业路42号202号房   25-05-2012 00:00:00   25-05-2013 00:00:00   南宁市公安局良庆分局;   广西贵港市港北区根竹乡泗民村松柏垌屯29号)   南宁市良庆区建业路42号202号房   1����������������������������������������������������������������������������������������������������������������������������    107800C002990000000000000A189122����   15-08-2012 04:00:28��������   01-06-2012 15:36:12����   汉   450108000000   29-05-2012 08:35:49   1   01-06-2012 15:36:12   1   14-08-2012 11:10:16��������    107800C002990000000000000A189122��������   01-06-2012 18:29:40   450000   广西区公安厅;   广西贵港市港北区根竹乡泗民村松柏垌屯29号��������   450700112000000258208	   AC4543243	   吴伟军   35030219880630101X   1   01   30-06-1988 00:00:00��������   福建省莆田市城厢区*   广西钦州市钦南区西环南路188号   24-08-2012 00:00:00   24-08-2013 00:00:00   钦州市公安局钦南分局;   福建省莆田市龙桥街道泗华村孔里自然村36号*   广西钦州市钦南区西环南路188号����������������������������������������������������������������������������������������������������������������������������������������   08-10-2012 09:38:53��������   08-10-2012 09:25:27����   汉   450702000000   24-08-2012 18:05:28   1   08-10-2012 09:25:43����������������������������������������D   福建省莆田市城厢区龙桥街道泗华村孔里自然村36号����   1   450100112000000002719	   AC4543243	   莫洁珍   452230198404163529   2   08   16-04-1984 00:00:00��������   广西蒙山县>   南宁市青秀区长湖路50号三月花国际大酒店宿舍   28-05-2012 00:00:00   28-05-2013 00:00:00   南宁市公安局南湖分局:   广西蒙山县新圩镇四联村义雅十二组8号101室>   南宁市青秀区长湖路50号三月花国际大酒店宿舍   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000064700AE����   15-08-2012 04:00:29��������   08-06-2012 17:31:57����   壮   450115000000   29-05-2012 08:35:59   1   08-06-2012 17:31:57   1   14-08-2012 11:19:44��������    107800C00299000000000000064700AE��������   08-06-2012 19:35:40   450000   广西区公安厅:   广西蒙山县新圩镇四联村义雅十二组8号101室����   1   450100112000000002776	   AC4543243	   陈桂运   452402199209040910   1   01   04-09-1992 00:00:00��������   广西贺州市八步区>   南宁市青秀区长湖路50号三月花国际大酒店宿舍   28-05-2012 00:00:00   28-05-2013 00:00:00   南宁市公安局南湖分局D   广西贺州市八步区黄田镇长龙村寨面前十二组288-2号>   南宁市青秀区长湖路50号三月花国际大酒店宿舍   1����������������������������������������������������������������������������������������������������������������������������    107800C002990000000000005AAFFB31����   15-08-2012 04:00:29��������   08-06-2012 17:31:58����   汉   450115000000   29-05-2012 08:35:59   1   08-06-2012 17:31:58   1   14-08-2012 11:19:45��������    107800C002990000000000005AAFFB31��������   08-06-2012 19:39:59   450000   广西区公安厅D   广西贺州市八步区黄田镇长龙村寨面前十二组288-2号����   1   450100112000000002955	   AC4543243	   黄秋娟   452130197703010063   2   08   01-03-1977 00:00:00��������   广西钦州市钦北区,   南宁市良庆区银沙大道9号1栋501室   25-05-2012 00:00:00   25-05-2013 00:00:00   南宁市公安局良庆分局=   广西钦州市钦北区那蒙镇陂角村委东江村18-1号,   南宁市良庆区银沙大道9号1栋501室   1����������������������������������������������������������������������������������������������������������������������������    107800C0029900000000000093FCBC26����   15-08-2012 04:00:29��������   01-06-2012 15:36:35����   壮   450108000000   29-05-2012 08:35:59   1   01-06-2012 15:36:35   1   14-08-2012 11:10:21��������    107800C0029900000000000093FCBC26��������   01-06-2012 19:24:06   450000   广西区公安厅=   广西钦州市钦北区那蒙镇陂角村委东江村18-1号����   1   450100112000000002964   AC763465	   蓝子泉   452528197804116553   1   01   11-04-1978 00:00:00��������   广西博白县8   南宁市良庆区建业路68号昌弘制药有限公司   25-05-2012 00:00:00   25-05-2013 00:00:00   南宁市公安局良庆分局3   广西博白县新田镇那花村圳口田队020号8   南宁市良庆区建业路68号昌弘制药有限公司   1����������������������������������������������������������������������������������������������������������������������������    107800C002990000000000004B976310����   15-08-2012 04:00:29��������   01-06-2012 15:36:36����   汉   450108000000   29-05-2012 08:35:59   1   01-06-2012 15:36:36   1   14-08-2012 11:10:21��������    107800C002990000000000004B976310��������   01-06-2012 19:24:48   450000   广西区公安厅3   广西博白县新田镇那花村圳口田队020号����   1   450100112000000000259   AC763465	   黄章信   452528195712086730   1   01   08-12-1957 00:00:00��������   广西博白县,   南宁市西乡塘区友爱路西二巷35号   24-05-2012 00:00:00   24-05-2013 00:00:00!   南宁市公安局西乡塘分局0   广西博白县那卜镇名六村李冲队001号,   南宁市西乡塘区友爱路西二巷35号   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000803200D6����   15-08-2012 04:00:18��������   02-06-2012 11:28:49����   汉   450107000000   29-05-2012 08:29:50   1   02-06-2012 11:28:49   1   14-08-2012 11:13:52��������    107800C00299000000000000803200D6��������   03-06-2012 11:38:40   450000   广西区公安厅0   广西博白县那卜镇名六村李冲队001号����   1   450100112000000000264   AC763465	   甘连珍   452128198212103524   2   08   10-12-1982 00:00:00��������   广西扶绥县*   南宁市西乡塘区明秀路北一巷101   24-05-2012 00:00:00   24-05-2013 00:00:00!   南宁市公安局西乡塘分局0   广西扶绥县渠旧镇三合村渠思屯153号*   南宁市西乡塘区明秀路北一巷101   1����������������������������������������������������������������������������������������������������������������������������    107800C0029900000000000085C8A9CC����   15-08-2012 04:00:18��������   02-06-2012 11:28:50����   壮   450107000000   29-05-2012 08:29:50   1   02-06-2012 11:28:50   1   14-08-2012 11:13:52��������    107800C0029900000000000085C8A9CC��������   03-06-2012 11:39:47   450000   广西区公安厅0   广西扶绥县渠旧镇三合村渠思屯153号����   1   450100112000000000798   AC763465	   黄冠吉   450621197705111414   1   08   11-05-1977 00:00:00��������   广西上思县    南宁市良庆区五象路72号   24-05-2012 00:00:00   24-05-2013 00:00:00   南宁市公安局良庆分局/   广西上思县那琴乡桃岭村上南屯11号    南宁市良庆区五象路72号   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000E75376DD����   15-08-2012 04:00:18��������   01-06-2012 15:35:53����   壮   450108000000   29-05-2012 08:30:33   1   01-06-2012 15:35:53   1   14-08-2012 11:10:12��������    107800C00299000000000000E75376DD��������   01-06-2012 16:07:47   450000   广西区公安厅/   广西上思县那琴乡桃岭村上南屯11号����   1   450100112000000000947   AC763465	   黄原章   450703198008257212   1   01   25-08-1980 00:00:00��������   广西钦州市钦北区    南宁市良庆区银象路42号   24-05-2012 00:00:00   24-05-2013 00:00:00   南宁市公安局良庆分局A   广西钦州市钦北区平吉镇广平村委那宽村四队18号    南宁市良庆区银象路42号   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000A7CF73B7����   15-08-2012 04:00:18��������   01-06-2012 15:36:01����   汉   450108000000   29-05-2012 08:30:33   1   01-06-2012 15:36:01   1   14-08-2012 11:10:13��������    107800C00299000000000000A7CF73B7��������   01-06-2012 18:12:29   450000   广西区公安厅A   广西钦州市钦北区平吉镇广平村委那宽村四队18号����   1   450100112000000000952   AC763465	   韦秀凤   452124198012062427   2   08   06-12-1980 00:00:00��������   广西上林县2   南宁市青秀区滨湖路埌东村九组5栋4号   27-05-2012 00:00:00   27-05-2013 00:00:00   南宁市公安局南湖分局/   广西上林县塘红乡万福村良王庄18号2   南宁市青秀区滨湖路埌东村九组5栋4号   1����������������������������������������������������������������������������������������������������������������������������    107800C002990000000000009E9028C0����   15-08-2012 04:00:18��������   08-06-2012 17:31:29����   壮   450115000000   29-05-2012 08:30:33   1   08-06-2012 17:31:29   1   14-08-2012 11:19:48��������    107800C002990000000000009E9028C0��������   08-06-2012 21:12:11   450000   广西区公安厅/   广西上林县塘红乡万福村良王庄18号����   1   450100112000000002054   AC763465	   吴彩玉   452628198709262122   2   01   26-09-1987 00:00:00��������   广西凌云县/   南宁市青秀区祥宾路居民二区8栋1号   27-05-2012 00:00:00   27-05-2013 00:00:00   南宁市公安局南湖分局/   广西凌云县逻楼镇仰村村陇后屯21号/   南宁市青秀区祥宾路居民二区8栋1号   1����������������������������������������������������������������������������������������������������������������������������    107800C0029900000000000013B495FC����   15-08-2012 04:00:18��������   08-06-2012 17:31:45����   汉   450115000000   29-05-2012 08:30:33   1   08-06-2012 17:31:45   1   14-08-2012 11:19:39��������    107800C0029900000000000013B495FC��������   08-06-2012 18:14:52   450000   广西区公安厅/   广西凌云县逻楼镇仰村村陇后屯21号����   1   450100112000000000163   AC763465	   李俊良   450924198403153221   2   01   15-03-1984 00:00:00��������   广西兴业县=   南宁市良庆区沿海经济走廊开发区月湖一街6号   23-05-2012 00:00:00   23-05-2013 00:00:00   南宁市公安局良庆分局,   广西兴业县龙安镇长居村四队11号=   南宁市良庆区沿海经济走廊开发区月湖一街6号   1����������������������������������������������������������������������������������������������������������������������������    107800C0029900000000000038D7539A����   15-08-2012 04:00:18��������   01-06-2012 15:35:42����   汉   450108000000   29-05-2012 08:30:33   1   01-06-2012 15:35:42   1   14-08-2012 11:05:30��������    107800C0029900000000000038D7539A��������   01-06-2012 15:54:07   450000   广西区公安厅,   广西兴业县龙安镇长居村四队11号����   1   450100112000000000189   AC763465   梁旺   452501197409172918   1   01   17-09-1974 00:00:00��������   广西玉林市玉州区#   南宁市良庆区东湖二街58号   23-05-2012 00:00:00   23-05-2013 00:00:00   南宁市公安局良庆分局5   广西玉林市玉州区仁东镇木根村梧村78号#   南宁市良庆区东湖二街58号   1����������������������������������������������������������������������������������������������������������������������������    107800C0029900000000000009337704����   15-08-2012 04:00:18��������   01-06-2012 15:35:48����   汉   450108000000   29-05-2012 08:30:54   1   01-06-2012 15:35:48   1   14-08-2012 11:05:31��������    107800C0029900000000000009337704��������   01-06-2012 16:03:06   450000   广西区公安厅5   广西玉林市玉州区仁东镇木根村梧村78号����   1   450100112000000000802   AC763465	   李继东   452327197409222156   1   01   22-09-1974 00:00:00��������   广西灌阳县G   南宁市良庆区银海大道706-1号金地花园10栋1单元102号房   24-05-2012 00:00:00   24-05-2013 00:00:00   南宁市公安局良庆分局/   广西灌阳县灌阳镇徐源村李家屯13号G   南宁市良庆区银海大道706-1号金地花园10栋1单元102号房   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000D59D3F45����   15-08-2012 04:00:18��������   01-06-2012 15:35:53����   汉   450108000000   29-05-2012 08:30:54   1   01-06-2012 15:35:53   1   14-08-2012 11:10:13��������    107800C00299000000000000D59D3F45��������   01-06-2012 17:58:07   450000   广西区公安厅/   广西灌阳县灌阳镇徐源村李家屯13号����   1   450100112000000000849   AC763465	   张春生   452528198302016194   1   01   01-02-1983 00:00:00��������   广西博白县$   南宁市良庆区景华二街104号   24-05-2012 00:00:00   24-05-2013 00:00:00   南宁市公安局良庆分局3   广西博白县凤山镇立石村文盛塘队099号$   南宁市良庆区景华二街104号   1����������������������������������������������������������������������������������������������������������������������������    107800C002990000000000000D5FF029����   15-08-2012 04:00:18��������   01-06-2012 15:35:54����   汉   450108000000   29-05-2012 08:30:54   1   01-06-2012 15:35:54   1   14-08-2012 11:10:13��������    107800C002990000000000000D5FF029��������   01-06-2012 17:59:02   450000   广西区公安厅3   广西博白县凤山镇立石村文盛塘队099号����   1   450100112000000000852   AC763465	   黄代波   452229198403201014   1   08   20-03-1984 00:00:00��������   广西融水苗族自治县7   南宁市青秀区祥宾路埌东村八组47栋5-301室   27-05-2012 00:00:00   27-05-2013 00:00:00   南宁市公安局南湖分局:   广西融水苗族自治县永乐乡四莫村平地屯4号7   南宁市青秀区祥宾路埌东村八组47栋5-301室   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000D170265D����   15-08-2012 04:00:18��������   08-06-2012 17:31:28����   壮   450115000000   29-05-2012 08:30:54   1   08-06-2012 17:31:28   1   14-08-2012 11:19:48��������    107800C00299000000000000D170265D��������   08-06-2012 21:11:50   450000   广西区公安厅:   广西融水苗族自治县永乐乡四莫村平地屯4号����   1   450100112000000000921   AC763465	   梁文映   452528197307106979   1   01   10-07-1973 00:00:00��������   广西博白县4   南宁市良庆区大沙田玉洞村谢坡2队121号   24-05-2012 00:00:00   24-05-2013 00:00:00   南宁市公安局良庆分局0   广西博白县沙陂镇八壁村东福队072号4   南宁市良庆区大沙田玉洞村谢坡2队121号   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000DE83830B����   15-08-2012 04:00:18��������   01-06-2012 15:36:01����   汉   450108000000   29-05-2012 08:31:05   1   01-06-2012 15:36:01   1   14-08-2012 11:10:13��������    107800C00299000000000000DE83830B��������   01-06-2012 18:12:15   450000   广西区公安厅0   广西博白县沙陂镇八壁村东福队072号����   1   450100112000000000854   AC763465	   韦家鹏   452502197308072535   1   08   07-08-1973 00:00:00��������   广西贵港市覃塘区7   南宁市青秀区祥宾路埌东村五组35栋6-601号   28-05-2012 00:00:00   28-05-2013 00:00:00   南宁市公安局南湖分局9   广西贵港市覃塘区樟木乡那柳村韦柳屯107号7   南宁市青秀区祥宾路埌东村五组35栋6-601号   1����������������������������������������������������������������������������������������������������������������������������    107800C00299000000000000EC02D88B����   15-08-2012 04:00:18��������   08-06-2012 17:31:28����   壮   450115000000   29-05-2012 08:31:05   1   08-06-2012 17:31:28   1   14-08-2012 11:19:33��������    107800C00299000000000000EC02D88B��������   08-06-2012 17:36:50   450000   广西区公安厅9   广西贵港市覃塘区樟木乡那柳村韦柳屯107号����       �  �   ��     4.0@   /attached/file/20181119/8c044e47-e6ac-43b7-9ce9-a293de55dffe.zip   0   file   services.zip   .zip   9618   20181119   admin   19-11-2018 15:19:04��������@   /attached/file/20181122/7c2e16d7-d32d-430d-a68e-0ebd2b995d2c.zip   0   file   新建文件夹 (2).zip   .zip   1334   20181122   admin   22-11-2018 15:28:31��������@   /attached/file/20181126/5ea089f5-c081-4c43-83ae-c5fc0844a1fb.zip   0   file
   WeChat.zip   .zip   94728   20181126   admin   26-11-2018 18:14:02��������@   /attached/file/20181126/ece4b995-32a4-45dd-ad2d-7c70cf7ea405.zip   0   file   新建文件夹 (2).zip   .zip   1334   20181126   admin   26-11-2018 18:14:11��������@   /attached/file/20181127/0ead73fe-e6e5-41bf-833b-994d7b8238c1.zip   0   file   services.zip   .zip   9618   20181127   admin   27-11-2018 15:56:29��������@   /attached/file/20181127/8e280219-9eb5-469f-a3f8-6944797a88ad.zip   0   file
   WeChat.zip   .zip   94728   20181127   admin   27-11-2018 15:59:50��������A   /attached/image/20181211/27509a96-72b1-40b2-a138-31404a76e604.jpg   0   image&   u=3210700219,1849066843&fm=27&gp=0.jpg   .jpg   39603   20181211   admin   11-12-2018 14:15:06��������A   /attached/image/20181211/54947859-3c9c-4536-8224-7118ccadca5f.jpg   0   image&   u=3210700219,1849066843&fm=27&gp=0.jpg   .jpg   39603   20181211   admin   11-12-2018 14:18:04��������A   /attached/image/20181211/6c2b6bd7-2a7e-48c1-b5c9-53949a594a3a.jpg   0   image&   u=3210700219,1849066843&fm=27&gp=0.jpg   .jpg   39603   20181211   admin   11-12-2018 14:32:22��������A   /attached/image/20181211/8aec5945-659e-4f70-846a-47cd0b54f81d.jpg   0   image&   u=3210700219,1849066843&fm=27&gp=0.jpg   .jpg   39603   20181211   admin   11-12-2018 15:28:13��������       f  f   j�     5.0   1!   贺州市公安局桂岭派出所   451102530000��������������������������������   0��������   admin   27-12-2018 14:24:53   0����   2   贺州市公安局八步分局   451102000000��������������������������������   0����������������   0����   3   桂林市公安局叠彩分局   450303000000��������������������������������   0��������   admin   02-01-2019 10:10:51   0����   4   桂林市公安局秀峰分局   450302000000��������������������������������   0����������������   0����   5!   贺州市公安局城中派出所   451102400000��������������������������������   0����������������   0����   6$   南宁市公安局大沙田派出所   450108310000��������������������������������   0����������������   0����   7!   柳州市公安局城东派出所   450206620000��������������������������������   0����������������   0����   8   桂林市公安局   450300000000��������������������������������   0����������������   0����   9	   公安厅   450000000000��������������������������������   0����������������   0����   10!   南宁市公安局邕武派出所   450102350000��������������������������������   0����������������   0����   11!   南宁市公安局向阳派出所   450103380000��������������������������������   0����������������   0����   12*   桂林市临桂县公安局二塘派出所   450322550000��������������������������������   0����������������   0����   13!   钦州市钦北区红阳派出所   450703640000��������������������������������   0����������������   0����   14   百色右江向阳所   451002380000��������������������������������   0����������������   0����   15   百色靖西市   451081000000��������������������������������   0����������������   0����   16   南宁市公安局   450100000000��������������������������������   0����������������   0����       e  e   ^�      7.0   1   0   超级管理员   省厅d   0����   24-10-2018 21:14:32   admin   04-04-2019 15:08:20   0����   2   10   市级管理员   市级管理员d   0����   24-10-2018 16:14:42   admin   04-04-2019 19:33:08   0   22   3   11   区县管理员   区县管理员   0����   24-10-2018 16:31:14   admin   04-04-2019 19:33:15   0   12   4   12   派出所管理员   派出所管理员   0����   24-10-2018 16:33:57   admin   04-04-2019 19:33:23   0   7   1000   14	   制证员   制证人员   0   admin   04-04-2019 10:28:17   admin   04-04-2019 19:45:48   0   4       ^  ^   h�     8.0   1169   1001   MACHINE   0   1170   1001   MACHINE.MACHINEMGR   0   1171   1001   MACHINE.MACHINEZZTJ   0   1172   1001   ZZY   0   1173   1001
   ZZY.ZZYMGR   0   1303   4   SYS.ROLE   0   1304   4   SYS.USER   0   1305   4   SYS.USER.__OPTION__   0   1306   4   SYS.USER.SELECT_OTHOR_COMPANY   0   1307   4   SYS.USER.CHANGE_ROLE   0   1308   4   MACHINE   0   1309   4   MACHINE.MACHINEMGR   0   1310   4   ZZJL   0   1311   4   ZZJL.ZZJLCX   0   1271   2   SYS   0   1272   2   SYS.COMPANIES   0   1273   2   SYS.ROLE   0   1274   2   SYS.USER   0   1275   2   SYS.USER.__OPTION__   0   1276   2   SYS.USER.SELECT_OTHOR_COMPANY   0   1277   2   SYS.USER.CHANGE_ROLE   0   1278   2   MACHINE   0   1279   2   MACHINE.MACHINEMGR   0   1286   3   SYS   0   1287   3   SYS.COMPANIES   0   1288   3   SYS.ROLE   0   1328   1000   MACHINE   0   1226   1   SYS   0   1227   1   SYS.COMPANIES   0   1228   1   SYS.ROLE   0   1229   1   SYS.USER   0   1230   1   SYS.USER.__OPTION__   0   1231   1   SYS.USER.SELECT_OTHOR_COMPANY   0   1232   1   SYS.USER.CHANGE_ROLE   0   1233   1   GLOBAL   0   1234   1   GLOBAL.QUERY   0   1312   4   MACHINE.MACHINEZZTJ   0   1313   4   ZZJL.ZZYZZJL   0   1314   4   ZZY   0   1315   4
   ZZY.ZZYMGR   0   1316   4   GLOBAL   0   1280   2   ZZJL   0   1289   3   SYS.USER   0   1290   3   SYS.USER.__OPTION__   0   1291   3   SYS.USER.SELECT_OTHOR_COMPANY   0   1292   3   SYS.USER.CHANGE_ROLE   0   1293   3   MACHINE   0   1294   3   MACHINE.MACHINEMGR   0   1295   3   ZZJL   0   1296   3   ZZJL.ZZJLCX   0   1297   3   MACHINE.MACHINEZZTJ   0   1281   2   ZZJL.ZZJLCX   0   1282   2   MACHINE.MACHINEZZTJ   0   1283   2   ZZJL.ZZYZZJL   0   1284   2   ZZY   0   1285   2
   ZZY.ZZYMGR   0   1298   3   ZZJL.ZZYZZJL   0   1299   3   ZZY   0   1300   3
   ZZY.ZZYMGR   0   1301   3   GLOBAL   0   1302   3   GLOBAL.QUERY   0   1317   4   GLOBAL.QUERY   0   1329   1000   MACHINE.MACHINEMGR   0   1330   1000   ZZJL   0   1331   1000   ZZJL.ZZJLCX   0   1332   1000   MACHINE.MACHINEZZTJ   0   1333   1000   ZZJL.ZZYZZJL   0   29   9   SYS__COMPANY   0   30   9   SYS__ROLE__ADD   0   31   9   SYS__ROLE__QUERY_ALL   0   32   8   SYS__COMPANY   0   33   8	   SYS__ROLE   0   34   8   SYS__ROLE__ADD   0   35   8   SYS__ROLE__QUERY_ALL   0   36   8	   SYS__USER   0   37   10   SYS   0   38   10   SYS__COMPANIES   0   39   10	   SYS__ROLE   0   40   10   SYS__ROLE__OPTION__   0   41   10   SYS__ROLE__ADD   0   42   10   SYS__ROLE__EDIT   0   43   10   SYS__ROLE__QUERY_ALL   0   44   10	   SYS__USER   0   45   11	   SYS__ROLE   0   46   11   SYS__ROLE__OPTION__   0   47   11   SYS__ROLE__ADD   0   48   11   SYS__ROLE__EDIT   0   49   11   SYS__ROLE__QUERY_ALL   0   50   11   DEMO   0   51   11   DEMO__DEMO1   0   52   11   GLOBAL   0   53   11   GLOBAL__QUERY   0       �  �   _%     11.0   GZS   桂林市公安局叠彩分局    202CB962AC59075B964B07152D234B70
   1476616448   2   市级管理员   23   0   admin   04-04-2019 11:08:41   admin   08-04-2019 11:07:18   0   450303000000   桂林市公安局叠彩分局����������������   ZZY   ZZY    E34775E9CC0503B5CFD9E075741A4FE7   -2040463168   1000	   制证员   9   0   admin   04-04-2019 14:26:12   admin   04-04-2019 19:45:17   0   450303000000   桂林市公安局叠彩分局����������������   ZZY2   ZZY2    202CB962AC59075B964B07152D234B70	   133059968   1000	   制证员   10   1   GZS   04-04-2019 14:33:08   admin   08-04-2019 11:17:54   0   450302000000   桂林市公安局秀峰分局����������������   admin   系统管理员    E10ADC3949BA59ABBE56E057F20F883E	   969390976   1   超级管理员   999   0����   29-10-2018 16:26:51   admin   08-04-2019 11:09:42   0   2   通用企业   1   2   3@2.c   444444       �  �   GX      12.0   1080   GZS   2����   1086   ZZY2   1000����   1062   450206620000   3����   1063   450206000000   3����   1064   450200000000   2����   1003   HB0002   3����   1004   HB0002   4����   1017   HB0003   4����   1   admin   1   0   1027   HB0001   2����   1028   HB0001   3����   1029   HB0001   4����   1061   HB0004   2����   1084   ZZY   1000����