CREATE TABLE IF NOT EXISTS report_dev_sqlb.db_objects (
    object_name text,
    object_type text,
    processed boolean NOT NULL DEFAULT false
);


insert into  report_dev_sqlb.db_objects (object_name, object_type)
select distinct used_object_name ,used_object_type from report_dev_sqlb.dependencies d;