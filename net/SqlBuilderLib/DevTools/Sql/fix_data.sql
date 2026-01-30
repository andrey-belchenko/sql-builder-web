update report_dev_sqlb.db_objects set object_name =  REPLACE(object_name, 'ASUSE".', '') where object_name like 'ASUSE".%';
update report_dev_sqlb.dependencies set object_name =  REPLACE(object_name, 'ASUSE".', '') where object_name like 'ASUSE".%';
update report_dev_sqlb.dependencies set used_object_name =  REPLACE(used_object_name, 'ASUSE".', '') where used_object_name like 'ASUSE".%';
