CREATE OR REPLACE VIEW report_dev_sqlb.report_dependencies_view AS
WITH RECURSIVE dependency_chain AS (
    -- Base case: direct dependencies of reports
    SELECT 
        r.name AS rep_name,
        r.nav_id,
        d.used_object_name,
        1 AS level
    FROM report_dev_sqlb.reports r
    INNER JOIN report_dev_sqlb.dependencies d ON r.name = d.object_name
    
    UNION ALL
    
    -- Recursive case: dependencies of dependencies
    SELECT 
        dc.rep_name,
        dc.nav_id,
        d.used_object_name,
        dc.level + 1
    FROM dependency_chain dc
    INNER JOIN report_dev_sqlb.dependencies d ON dc.used_object_name = d.object_name
)
SELECT DISTINCT
    dc.rep_name,
    dc.nav_id,
    dc.used_object_name,
    do.object_type AS used_object_type
FROM dependency_chain dc
LEFT JOIN report_dev_sqlb.db_objects do ON dc.used_object_name = do.object_name
ORDER BY dc.rep_name, dc.nav_id, dc.used_object_name;
