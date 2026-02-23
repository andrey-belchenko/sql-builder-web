with RECURSIVE dependency_tree as (
    -- Anchor: Start with all procedures and their direct dependencies
    select d.object_name as object_name,
        d.used_object_name as dependent_object,
        o2.object_type as dependent_type,
        1 as level,
        array [ d.used_object_name ] as path
    from report_dev_sqlb.dependencies d
        inner join report_dev_sqlb.db_objects o on d.object_name = o.object_name
        --and o.object_type = 'procedure'
        left join report_dev_sqlb.db_objects o2 on d.used_object_name = o2.object_name
    union
    all -- Recursive: Find what those dependencies depend on
    select dt.object_name,
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
    select object_name,
        level,
        dependent_object,
        coalesce(dependent_type, 'UNKNOWN') as dependent_type
    from dependency_tree
    order by object_name,
        level,
        dependent_object
)
select *
from pd where dependent_type='view' and object_name like 'ng_rep_other.%'

