-- Start of DDL Script for Public Synonym VC_USER_LOGIN
-- Generated 17-окт-2018 20:48:02 from PUBLIC@DEVALPHA.WORLD

CREATE PUBLIC SYNONYM vc_user_login
  FOR VC_USER_LOGIN
/


-- End of DDL Script for Public Synonym VC_USER_LOGIN

-- Start of DDL Script for Table PLAN.VC_USER_LOGIN
-- Generated 17-окт-2018 20:48:02 from PLAN@DEVALPHA.WORLD

CREATE TABLE vc_user_login
    (user_id                        NUMBER ,
    user_password                  VARCHAR2(300 BYTE),
    user_email                     VARCHAR2(300 BYTE),
    user_phone                     VARCHAR2(20 BYTE),
    esia_guid                      VARCHAR2(50 BYTE),
    user_registration              DATE,
    user_last_login                DATE,
    user_snils                     VARCHAR2(30 BYTE),
    person_last_name               VARCHAR2(300 BYTE),
    person_first_name              VARCHAR2(300 BYTE),
    person_middle_name             VARCHAR2(300 BYTE),
    user_type                      NUMBER,
    pref_communication             NUMBER,
    blocking_date                  DATE,
    status                         NUMBER,
    u_m                            VARCHAR2(30 BYTE),
    d_m                            DATE,
    user_info                      CLOB)
  SEGMENT CREATION IMMEDIATE
  TABLESPACE  plan_tbl
  LOB ("USER_INFO") STORE AS SYS_LOB0000137599C00018$$
  (
  TABLESPACE  plan_tbl
   NOCACHE LOGGING
   CHUNK 8192
  )
  NOPARALLEL
  LOGGING
  MONITORING
/

-- Constraints for VC_USER_LOGIN

ALTER TABLE vc_user_login
ADD CONSTRAINT xpkvc_user_login PRIMARY KEY (user_id)
USING INDEX
  TABLESPACE  plan_tbl
/


-- Triggers for VC_USER_LOGIN

CREATE OR REPLACE TRIGGER t_vc_user_login
 BEFORE
  INSERT OR DELETE OR UPDATE
 ON vc_user_login
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
DECLARE
BEGIN
    IF (USER = kg_common.repadmin)
    THEN
        RETURN;
    END IF;

    IF (INSERTING AND :new.user_id IS NULL)
    THEN
        SELECT sqvc_user_login.NEXTVAL INTO :new.user_id FROM DUAL;
    END IF;

    IF (INSERTING OR UPDATING)
    THEN
        SELECT USER INTO :new.u_m FROM DUAL;

        SELECT SYSDATE INTO :new.d_m FROM DUAL;
    END IF;
END;
/


-- Comments for VC_USER_LOGIN

COMMENT ON COLUMN vc_user_login.blocking_date IS 'Дата блокировки. Если задана- доступ пользователю запрещен'
/
COMMENT ON COLUMN vc_user_login.esia_guid IS 'Идентификатор ЕСИА '
/
COMMENT ON COLUMN vc_user_login.person_first_name IS 'Имя'
/
COMMENT ON COLUMN vc_user_login.person_last_name IS 'Фамилия'
/
COMMENT ON COLUMN vc_user_login.person_middle_name IS 'Отчество'
/
COMMENT ON COLUMN vc_user_login.pref_communication IS 'Предпочитаемый способ связи 0=E-mail, 1=телефон'
/
COMMENT ON COLUMN vc_user_login.status IS 'Статус: 0 – активный; 1- удален; 2- новый (еще не создан в ЛКК)'
/
COMMENT ON COLUMN vc_user_login.user_email IS 'Адрес электронной почты'
/
COMMENT ON COLUMN vc_user_login.user_id IS 'Уникальный идентификатор'
/
COMMENT ON COLUMN vc_user_login.user_last_login IS 'Дата/время последнего входа в систему'
/
COMMENT ON COLUMN vc_user_login.user_password IS 'Пароль пользователя (в зашифрованном виде)'
/
COMMENT ON COLUMN vc_user_login.user_phone IS 'Мобильный телефон'
/
COMMENT ON COLUMN vc_user_login.user_registration IS 'Дата/время регистрации в системе'
/
COMMENT ON COLUMN vc_user_login.user_snils IS 'СНИЛС'
/
COMMENT ON COLUMN vc_user_login.user_type IS 'Тип заявителя по умолчанию (ФЛ=0,ЮЛ=1,ИП=2)'
/

-- End of DDL Script for Table PLAN.VC_USER_LOGIN

