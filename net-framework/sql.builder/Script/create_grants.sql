declare
name_user VARCHAR2(30);
name_shema VARCHAR2(30);
name_table VARCHAR2(30);
cursor name_t is
select table_name from all_tables where owner=upper(name_shema);

begin

name_user := upper('public');
name_shema := upper('asuse');

open name_t;
        loop
          fetch name_t into name_table;
          exit when name_t%notfound;
          execute immediate ('grant select on '||name_shema|| '.' ||name_table||' to ' || name_user );
        end loop;
close name_t;

commit;
end;



