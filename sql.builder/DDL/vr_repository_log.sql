create table vr_repository_log
(
    puser varchar2(30) not null,
    rep_table varchar2(30) not null,
    date_log date not null,
    action varchar2(30) not null,
    text varchar2(500)
)
/
CREATE INDEX idxvr_repository_log ON vr_repository_log(puser);
/
CREATE PUBLIC SYNONYM vr_repository_log FOR vr_repository_log
/
CREATE OR REPLACE TRIGGER t_vr_repository_log
 BEFORE
  INSERT OR UPDATE
 ON vr_repository_log
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
declare
BEGIN
  if INSERTING then
    :NEW.date_log := sysdate;
    :NEW.puser := user;
  end if;
END;
/
COMMENT ON TABLE vr_repository_log IS 'Логирование работы с хранилищами Sql.Builder'
/
COMMENT ON COLUMN vr_repository_log.rep_table IS 'Таблица хранилища'
/
COMMENT ON COLUMN vr_repository_log.puser IS 'Пользователь'
/
COMMENT ON COLUMN vr_repository_log.date_log IS 'Время записи'
/
COMMENT ON COLUMN vr_repository_log.action IS 'Действие над хранилищем'
/
COMMENT ON COLUMN vr_repository_log.text IS 'Информация'
/
-- Grants for Table
GRANT DELETE ON vr_repository_log TO public
/
GRANT INSERT ON vr_repository_log TO public
/
GRANT SELECT ON vr_repository_log TO public
/
GRANT UPDATE ON vr_repository_log TO public
/
