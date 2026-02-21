with RECURSIVE dependency_tree as (
    -- Anchor: Start with all procedures and their direct dependencies
    select d.object_name as procedure_name,
        d.used_object_name as dependent_object,
        o2.object_type as dependent_type,
        1 as level,
        array [ d.used_object_name ] as path
    from report_dev_sqlb.dependencies d
        inner join report_dev_sqlb.db_objects o on d.object_name = o.object_name
        and o.object_type = 'procedure'
        left join report_dev_sqlb.db_objects o2 on d.used_object_name = o2.object_name
    union
    all -- Recursive: Find what those dependencies depend on
    select dt.procedure_name,
        d.used_object_name,
        o2.object_type,
        dt.level + 1,
        dt.path || d.used_object_name
    from report_dev_sqlb.dependencies d
        inner join dependency_tree dt on d.object_name = dt.dependent_object
        left join report_dev_sqlb.db_objects o2 on d.used_object_name = o2.object_name
    where not d.used_object_name = any(dt.path) -- Prevent cycles
),
pd as (
    select procedure_name,
        level,
        dependent_object,
        coalesce(dependent_type, 'UNKNOWN') as dependent_type
    from dependency_tree
    order by procedure_name,
        level,
        dependent_object
),
stat as (
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
    from pd rd
        left join stat s on s.table_name = rd.dependent_object
    where dependent_type = 'table'
)
select *
from rs
where not exists (
        select *
        from rs rs1
        where rs1.procedure_name = rs.procedure_name
            --and coalesce(cnt, 0) = 0 
            and cnt is  null
    );

