update report_dev_sqlb.db_objects set object_name =  REPLACE(object_name, 'asuse".', '') where object_name like 'asuse".%';
update report_dev_sqlb.dependencies set object_name =  REPLACE(object_name, 'asuse".', '') where object_name like 'asuse".%';
update report_dev_sqlb.dependencies set used_object_name =  REPLACE(used_object_name, 'asuse".', '') where used_object_name like 'asuse".%';


insert into report_dev_sqlb.db_objects (object_name,object_type)
values ('sg_kaz_nakopit_teplo.dbf_proc_sbros_teplo_dbf', 'procedure');

insert into report_dev_sqlb.dependencies  (object_name,used_object_name)
values 
('asuse2.20498','sg_kaz_nakopit_teplo.dbf_proc_sbros_teplo_dbf'),
('asuse2.20498_itog','sg_kaz_nakopit_teplo.dbf_proc_sbros_teplo_dbf');
