with stat as (
    select relname as table_name,
        max(reltuples::bigint) as estimate_count
    from pg_class c
        join pg_namespace n on n.oid = c .relnamespace
    where c .relkind = 'r'
        and n.nspname not in ('pg_catalog', 'information_schema')
    group by relname
),
rs as (
    select rd. *,
        s.estimate_count cnt
    from report_dev_sqlb.report_dependencies rd
        left join stat s on s.table_name = rd.used_object_name
    where used_object_type = 'table'
)
select *
from rs
where not exists (
        select *
        from rs rs1
        where rs1.rep_name = rs.rep_name
            and coalesce(cnt, 0) = 0 --and cnt is  null
    );