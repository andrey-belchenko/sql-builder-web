CREATE TABLE vr_repository_info
    (rep_table                      VARCHAR2(30 BYTE) NOT NULL,
    date_start                     DATE NOT NULL,
    date_end                       DATE NOT NULL
  ,
  CONSTRAINT PKVR_REPOSITORY_INFO PRIMARY KEY (rep_table) USING INDEX)
/
-- public синоним
CREATE OR REPLACE PUBLIC SYNONYM vr_repository_info FOR vr_repository_info
/
-- Comments for vr_repository_info
COMMENT ON TABLE vr_repository_info IS 'Информация о формировании хранилищ sql.builder'
/
COMMENT ON COLUMN vr_repository_info.rep_table IS 'Таблица хранилища'
/
COMMENT ON COLUMN vr_repository_info.date_start IS 'Время начала последнего успешного формирования'
/
COMMENT ON COLUMN vr_repository_info.date_end IS 'Время окончания последнего успешного формирования'
/
-- Grants for Table
GRANT DELETE ON vr_repository_info TO public
/
GRANT INSERT ON vr_repository_info TO public
/
GRANT SELECT ON vr_repository_info TO public
/
GRANT UPDATE ON vr_repository_info TO public
/
