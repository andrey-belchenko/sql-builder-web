-- Fix duplicates caused by different case
-- This script updates all object names to lowercase and removes duplicates

-- Step 1: Update db_objects table - convert object_name to lowercase
UPDATE report_dev_sqlb.db_objects
SET object_name = LOWER(object_name);

-- Step 2: Remove duplicates from db_objects
-- Keep the first occurrence of each (object_name, object_type) combination
DELETE FROM report_dev_sqlb.db_objects d1
WHERE EXISTS (
    SELECT 1
    FROM report_dev_sqlb.db_objects d2
    WHERE d2.object_name = d1.object_name
      AND d2.object_type = d1.object_type
      AND d2.ctid < d1.ctid
);

-- Step 3: Update dependencies table - convert both object_name and used_object_name to lowercase
UPDATE report_dev_sqlb.dependencies
SET object_name = LOWER(object_name),
    used_object_name = LOWER(used_object_name);

-- Step 4: Remove duplicates from dependencies
-- Keep the first occurrence of each (object_name, used_object_name) combination
DELETE FROM report_dev_sqlb.dependencies d1
WHERE EXISTS (
    SELECT 1
    FROM report_dev_sqlb.dependencies d2
    WHERE d2.object_name = d1.object_name
      AND d2.used_object_name = d1.used_object_name
      AND d2.ctid < d1.ctid
);
