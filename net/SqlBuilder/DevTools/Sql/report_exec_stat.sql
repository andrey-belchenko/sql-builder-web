CREATE OR REPLACE VIEW report_dev_sqlb.report_exec_stat AS
WITH max_finished_at AS (
    -- Calculate the overall maximum finished_at date across all executions
    SELECT MAX(finished_at) AS max_finished_at
    FROM report_dev_sqlb.reports_exec
    WHERE finished_at IS NOT NULL
),
reports_with_extracted_name AS (
    -- Extract report name (everything after the last dot) from reports table
    SELECT 
        name,
        title,
        path,
        nav_id,
        COALESCE(
            SUBSTRING(name FROM '\.([^\.]+)$'),
            name
        ) AS report_name_short
    FROM report_dev_sqlb.reports
),
exec_stats AS (
    -- Calculate execution statistics grouped by report name
    SELECT 
        re.reports_name,
        MAX(re.finished_at) AS last_exec,
        COUNT(CASE 
            WHEN re.finished_at IS NOT NULL 
                AND mfd.max_finished_at IS NOT NULL
                AND re.finished_at >= (mfd.max_finished_at - INTERVAL '30 days')
                AND re.finished_at <= mfd.max_finished_at
            THEN 1 
        END) AS exec_1m,
        COUNT(CASE 
            WHEN re.finished_at IS NOT NULL 
                AND mfd.max_finished_at IS NOT NULL
                AND re.finished_at >= (mfd.max_finished_at - INTERVAL '90 days')
                AND re.finished_at <= mfd.max_finished_at
            THEN 1 
        END) AS exec_3m,
        COUNT(CASE 
            WHEN re.finished_at IS NOT NULL 
                AND mfd.max_finished_at IS NOT NULL
                AND re.finished_at >= (mfd.max_finished_at - INTERVAL '365 days')
                AND re.finished_at <= mfd.max_finished_at
            THEN 1 
        END) AS exec_12m
    FROM report_dev_sqlb.reports_exec re
    CROSS JOIN max_finished_at mfd
    GROUP BY re.reports_name
)
SELECT 
    r.name,
    r.title,
    r.path,
    r.nav_id,
    es.last_exec,
    COALESCE(es.exec_1m, 0) AS exec_1m,
    COALESCE(es.exec_3m, 0) AS exec_3m,
    COALESCE(es.exec_12m, 0) AS exec_12m,
    CASE r.nav_id
        WHEN 'nav310' THEN 'Рязань'
        WHEN 'nav10' THEN 'Казань (Эл)'
        WHEN 'nav101' THEN 'Казань (Теп)'
    END AS customer
FROM reports_with_extracted_name r
LEFT JOIN exec_stats es ON r.report_name_short = es.reports_name;
