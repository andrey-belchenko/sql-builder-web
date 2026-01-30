update report_dev_sqlb.db_objects set object_name =  REPLACE(object_name, 'asuse".', '') where object_name like 'asuse".%';
update report_dev_sqlb.dependencies set object_name =  REPLACE(object_name, 'asuse".', '') where object_name like 'asuse".%';
update report_dev_sqlb.dependencies set used_object_name =  REPLACE(used_object_name, 'asuse".', '') where used_object_name like 'asuse".%';
