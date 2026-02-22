with stat as 

(SELECT 
    relname AS table_name, 
    max(reltuples::bigint) AS estimate_count
FROM 
    pg_class c
JOIN 
    pg_namespace n ON n.oid = c.relnamespace
WHERE 
    c.relkind = 'r' 
    AND n.nspname NOT IN ('pg_catalog', 'information_schema')
    
group by relname
),
    
    rs as (

select rd.* , s.estimate_count cnt

from report_dev_sqlb.report_dependencies rd 
left join  stat s on s.table_name =  rd.used_object_name

where used_object_type='table')


select * from  rs where 

not exists (select * from rs rs1 where rs1.rep_name=rs.rep_name 
and coalesce(cnt,0) =0
--and cnt is  null
)

and exists (select * from  report_dev_sqlb.report_dependencies rd  where rd.rep_name=rs.rep_name 
and  rd.used_object_type = 'procedure' 
--and cnt is  null
)


and exists (

select * from report_dev_sqlb.report_exec_stat s where s.name= rs.rep_name  and s.exec_3m>1
)
