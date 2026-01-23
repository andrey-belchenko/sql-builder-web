-- Start of DDL Script for Table ASUSE.vr_array_storage
-- Generated 15.06.2016 15:35:47 from ASUSE@asUSE.WORLD
drop table vr_array_storage
/
-- Start of DDL Script for Table ASUSE.vr_array_storage
-- Generated 03-окт-2017 12:31:40 from ASUSE@asuse.WORLD

CREATE GLOBAL TEMPORARY TABLE vr_array_storage
    (array_id                       VARCHAR2(100 BYTE),
    nval                            NUMBER,
    sval                            VARCHAR2(4000))
ON COMMIT PRESERVE ROWS
  NOPARALLEL
/

-- Grants for Table
GRANT DELETE ON vr_array_storage TO public
/
GRANT INSERT ON vr_array_storage TO public
/
GRANT SELECT ON vr_array_storage TO public
/
GRANT UPDATE ON vr_array_storage TO public
/




-- Indexes for vr_array_storage

CREATE INDEX ivr_vr_array_storage_name ON vr_array_storage
  (
    array_id                        ASC
  )
--NOPARALLEL
--LOGGING
/



-- End of DDL Script for Table ASUSE.vr_array_storage



create public synonym vr_array_storage for vr_array_storage
/
-- End of DDL Script for Table ASUSE.vr_array_storage

