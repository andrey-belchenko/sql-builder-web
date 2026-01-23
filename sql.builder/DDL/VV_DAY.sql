-- Start of DDL Script for Materialized View VLAD.VV_DAY
-- Generated 14-окт-2014 21:24:44 from VLAD@realvla

declare
    table_exists number;
begin
    select count(*) into table_exists from all_tables where table_name = 'VV_DAY';
    if(table_exists > 0) then
        execute immediate 'drop materialized view vv_day';
        execute immediate 'drop public synonym vv_day';
    end if;
end;

CREATE MATERIALIZED VIEW vv_day

REFRESH FORCE START WITH sysdate NEXT SYSDATE + 1 
AS
select  (ADD_MONTHS(trunc(SYSDATE,'YEAR'), -12*15) + level - 1) as dat_day from dual
      connect by level <= ADD_MONTHS(trunc(SYSDATE,'YEAR'), 12*10) - ADD_MONTHS(trunc(SYSDATE,'YEAR'), -12*15)
          
/
CREATE INDEX vv_day_dat_day
  ON vv_day(dat_day);

/

grant select on vv_day to public;

/
create public synonym vv_day for vv_day;
/


-- End of DDL Script for Materialized View VLAD.VV_DAY

