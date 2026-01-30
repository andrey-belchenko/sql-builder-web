CREATE OR REPLACE VIEW report_dev_sqlb.report_dependencies AS
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
),

x as (

SELECT DISTINCT
    dc.rep_name,
    dc.nav_id,
    dc.used_object_name,
    dbo.object_type AS used_object_type
FROM dependency_chain dc
LEFT JOIN report_dev_sqlb.db_objects dbo ON dc.used_object_name = dbo.object_name
ORDER BY dc.rep_name, dc.nav_id, dc.used_object_name
)

select 

x.*,
case x.nav_id
    when 'nav310' then 'Рязань'
    when 'nav10' then 'Казань (Эл)'
    when 'nav101' then 'Казань (Теп)'
end as customer


from x where used_object_name not in


(
'vr_37989_calc_tbl',
'vr_37989_dog_tbl',
'vr_grid_settings',
'vv_all_deb_sf',
'raise_application_error','a_pmax', 'edo','sumdog', 'sumobj','sumdog0','sumobj0','prop','prop0','prop1','t_row','a','dual','all_indexes','dbms_mview.refresh','o'
)

;
