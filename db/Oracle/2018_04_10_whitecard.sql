prompt PL/SQL Developer Export Tables for user SCOTT@ORCL
prompt Created by Administrator on 2019��4��10��
set feedback off
set define off

prompt Disabling triggers for B_CARD_GRANT...
alter table B_CARD_GRANT disable all triggers;
prompt Disabling triggers for B_CARD_RECOVERY...
alter table B_CARD_RECOVERY disable all triggers;
prompt Disabling triggers for B_CARD_STOCK...
alter table B_CARD_STOCK disable all triggers;
prompt Disabling triggers for D_CARDTYPE...
alter table D_CARDTYPE disable all triggers;
prompt Loading B_CARD_GRANT...
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (108, 1000, '18357392456', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '��ɫ������', '451081000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (110, 500, null, to_date('02-04-2019', 'dd-mm-yyyy'), '������', '�׾���', '������', '450000000000', '�����й�����', '450300000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (101, 10000, '18395629876', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й�����', '450300000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (102, 10000, '18495638765', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й�����', '450100000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (103, 1000, '18456789876', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й����ֳ����ɳ���', '451102400000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (104, 5000, '18456789876', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й��������־�', '450302000000');
commit;
prompt 6 records loaded
prompt Loading B_CARD_RECOVERY...
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (66, 200, '18456742356', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й�����', '450100000000', '02', '����');
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (62, 300, '18496749876', to_date('02-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й�����', '450300000000', '02', '����');
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (64, 200, '19843274567', to_date('01-04-2019', 'dd-mm-yyyy'), '������', '�׾���', '������', '450000000000', '�����й�����', '450100000000', '02', '����');
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (61, 300, '1859675842', to_date('02-04-2019', 'dd-mm-yyyy'), '������', '������', '������', '450000000000', '�����й�����', '450300000000', '01', '����');
commit;
prompt 4 records loaded
prompt Loading B_CARD_STOCK...
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (41, 100000, 75700, to_date('10-04-2019', 'dd-mm-yyyy'), '450000000000', '������', '750');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (44, 5000, 4500, to_date('01-04-2019', 'dd-mm-yyyy'), '450302000000', '�����й��������־�', '500');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (42, 10200, 9900, to_date('01-04-2019', 'dd-mm-yyyy'), '450300000000', '�����й�����', '0');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (43, 9600, 9500, to_date('01-04-2019', 'dd-mm-yyyy'), '450100000000', '�����й�����', '100');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (45, 0, -50, to_date('01-04-2019', 'dd-mm-yyyy'), '450102350000', '�����й����������ɳ���', '50');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (46, 0, 0, to_date('01-04-2019', 'dd-mm-yyyy'), '451102000000', '�����й����ְ˲��־�', '0');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (47, 0, 0, to_date('10-04-2019', 'dd-mm-yyyy'), '451102400000', '�����й����ֳ����ɳ���', '0');
commit;
prompt 7 records loaded
prompt Loading D_CARDTYPE...
insert into D_CARDTYPE (dm, mc)
values ('01', '����');
insert into D_CARDTYPE (dm, mc)
values ('02', '����');
commit;
prompt 2 records loaded
prompt Enabling triggers for B_CARD_GRANT...
alter table B_CARD_GRANT enable all triggers;
prompt Enabling triggers for B_CARD_RECOVERY...
alter table B_CARD_RECOVERY enable all triggers;
prompt Enabling triggers for B_CARD_STOCK...
alter table B_CARD_STOCK enable all triggers;
prompt Enabling triggers for D_CARDTYPE...
alter table D_CARDTYPE enable all triggers;

set feedback on
set define on
prompt Done
