prompt PL/SQL Developer Export Tables for user SCOTT@ORCL
prompt Created by Administrator on 2019年4月10日
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
values (108, 1000, '18357392456', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '百色靖西市', '451081000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (110, 500, null, to_date('02-04-2019', 'dd-mm-yyyy'), '公安厅', '亲警官', '公安厅', '450000000000', '桂林市公安局', '450300000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (101, 10000, '18395629876', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '桂林市公安局', '450300000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (102, 10000, '18495638765', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '南宁市公安局', '450100000000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (103, 1000, '18456789876', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '贺州市公安局城中派出所', '451102400000');
insert into B_CARD_GRANT (id, receive_number, receive_phone, receive_time, extend_name, receive_name, extend_id_v_d_fw_comp__mc, extend_id, receive_id_v_d_fw_comp__mc, receive_id)
values (104, 5000, '18456789876', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '桂林市公安局秀峰分局', '450302000000');
commit;
prompt 6 records loaded
prompt Loading B_CARD_RECOVERY...
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (66, 200, '18456742356', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '覃将官', '公安厅', '450000000000', '南宁市公安局', '450100000000', '02', '作废');
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (62, 300, '18496749876', to_date('02-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '桂林市公安局', '450300000000', '02', '作废');
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (64, 200, '19843274567', to_date('01-04-2019', 'dd-mm-yyyy'), '公安厅', '亲警官', '公安厅', '450000000000', '南宁市公安局', '450100000000', '02', '作废');
insert into B_CARD_RECOVERY (id, submit_number, submit_phone, recovery_time, recovery_name, submit_name, recovery_id_v_d_fw_comp__mc, recovery_id, submit_id_v_d_fw_comp__mc, submit_id, type_id, type_id_d_cardtype__mc)
values (61, 300, '1859675842', to_date('02-04-2019', 'dd-mm-yyyy'), '公安厅', '覃警官', '公安厅', '450000000000', '桂林市公安局', '450300000000', '01', '闲置');
commit;
prompt 4 records loaded
prompt Loading B_CARD_STOCK...
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (41, 100000, 75700, to_date('10-04-2019', 'dd-mm-yyyy'), '450000000000', '公安厅', '750');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (44, 5000, 4500, to_date('01-04-2019', 'dd-mm-yyyy'), '450302000000', '桂林市公安局秀峰分局', '500');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (42, 10200, 9900, to_date('01-04-2019', 'dd-mm-yyyy'), '450300000000', '桂林市公安局', '0');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (43, 9600, 9500, to_date('01-04-2019', 'dd-mm-yyyy'), '450100000000', '南宁市公安局', '100');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (45, 0, -50, to_date('01-04-2019', 'dd-mm-yyyy'), '450102350000', '南宁市公安局邕武派出所', '50');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (46, 0, 0, to_date('01-04-2019', 'dd-mm-yyyy'), '451102000000', '贺州市公安局八步分局', '0');
insert into B_CARD_STOCK (id, stock_whole, stock_overplus, input_time, company_id, company_id_v_d_fw_comp__mc, stock_scrap)
values (47, 0, 0, to_date('10-04-2019', 'dd-mm-yyyy'), '451102400000', '贺州市公安局城中派出所', '0');
commit;
prompt 7 records loaded
prompt Loading D_CARDTYPE...
insert into D_CARDTYPE (dm, mc)
values ('01', '闲置');
insert into D_CARDTYPE (dm, mc)
values ('02', '作废');
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
