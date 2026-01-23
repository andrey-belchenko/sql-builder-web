declare
    table_exists number;
begin
    select count(*) into table_exists from all_tables where table_name = 'VR_REPORTS_LOG';
    if(table_exists > 0) then
        execute immediate 'drop table vr_reports_log';
        execute immediate 'drop sequence sqvr_reports_log';
        execute immediate 'drop public synonym vr_reports_log';
    end if;
end;
/
create table vr_reports_log
(
   kod_log number,
   puser varchar2(30) not null,
   repname varchar2(300) not null,
   params nclob not null,
   date_start date,
   date_finish date,
   time_total interval day to second,
   error_text varchar2(1000),
   stack_text varchar2(1000),
   terminal varchar2(100),
   
   constraint xpkvr_reports_log primary key (kod_log)
)
/
create sequence sqvr_reports_log start with 1 increment by 1;
/
CREATE OR REPLACE TRIGGER t_vr_reports_log
 BEFORE
  INSERT OR UPDATE
 ON vr_reports_log
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
begin
    if inserting then
        select sqvr_reports_log.nextval, user, sysdate, SYS_CONTEXT('USERENV', 'TERMINAL')
        into :new.kod_log, :new.puser, :new.date_start, :new.terminal 
        from dual;
    elsif updating then
        select sysdate, numtodsinterval(sysdate - :old.date_start,'day')
        into :new.date_finish, :new.time_total
        from dual;
    end if;
end;
/
grant delete on vr_reports_log to public
/
grant insert on vr_reports_log to public
/
grant select on vr_reports_log to public
/
grant update on vr_reports_log to public
/
comment on table vr_reports_log is 'Логирование отчётов Sql.Builer'
/
comment on column vr_reports_log.puser is 'Пользователь'
/
comment on column vr_reports_log.repname is 'Отчёт'
/
comment on column vr_reports_log.params is 'Параметры отчёта (xml)'
/
comment on column vr_reports_log.date_start is 'Начало формирования'
/
comment on column vr_reports_log.date_finish is 'Окончание формирования'
/
comment on column vr_reports_log.time_total is 'Затраченое время'
/
comment on column vr_reports_log.error_text is 'Текст ошибки'
/
comment on column vr_reports_log.stack_text is 'Стек вызовов при ошибке'
/
create public synonym vr_reports_log for vr_reports_log
/ 
create index ind1vr_reports_log on vr_reports_log (repname,date_start desc)
/
CREATE INDEX VR_REPORTS_LOG_NAME ON VR_REPORTS_LOG
   (  REPNAME ASC  ) 
/
