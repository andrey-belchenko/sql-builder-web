-- Start of DDL Script for Public Synonym VCS_USER_LOGIN_TMP
-- Generated 17-окт-2018 20:52:25 from PUBLIC@DEVALPHA.WORLD

CREATE PUBLIC SYNONYM vcs_user_login_tmp
  FOR VCS_USER_LOGIN_TMP
/


-- End of DDL Script for Public Synonym VCS_USER_LOGIN_TMP

-- Start of DDL Script for Table PLAN.VCS_USER_LOGIN_TMP
-- Generated 17-окт-2018 20:52:25 from PLAN@DEVALPHA.WORLD

CREATE GLOBAL TEMPORARY TABLE vcs_user_login_tmp
    (tmp_user_id                    NUMBER,
    user_id                        NUMBER,
    user_password                  VARCHAR2(300 BYTE),
    user_email                     VARCHAR2(300 BYTE),
    user_phone                     VARCHAR2(300 BYTE),
    esia_guid                      VARCHAR2(300 BYTE),
    user_registration              DATE,
    user_last_login                DATE,
    user_snils                     VARCHAR2(300 BYTE),
    person_last_name               VARCHAR2(300 BYTE),
    person_first_name              VARCHAR2(300 BYTE),
    person_middle_name             VARCHAR2(300 BYTE),
    user_type                      NUMBER,
    pref_communication             NUMBER,
    blocking_date                  DATE,
    status                         NUMBER,
    user_info                      CLOB)
ON COMMIT PRESERVE ROWS

/





-- Comments for VCS_USER_LOGIN_TMP

COMMENT ON TABLE vcs_user_login_tmp IS 'Пользователь личного кабинета'
/
COMMENT ON COLUMN vcs_user_login_tmp.blocking_date IS 'Дата блокировки'
/
COMMENT ON COLUMN vcs_user_login_tmp.esia_guid IS 'Идентификатор ЕСИА '
/
COMMENT ON COLUMN vcs_user_login_tmp.person_first_name IS 'Имя'
/
COMMENT ON COLUMN vcs_user_login_tmp.person_last_name IS 'Фамилия'
/
COMMENT ON COLUMN vcs_user_login_tmp.person_middle_name IS 'Отчество'
/
COMMENT ON COLUMN vcs_user_login_tmp.pref_communication IS 'Предпочитаемый способ связи'
/
COMMENT ON COLUMN vcs_user_login_tmp.status IS 'Статус'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_email IS 'Адрес электронной почты'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_id IS 'Уникальный идентификатор'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_info IS 'Информация'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_last_login IS 'Дата/время последнего входа в систему'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_password IS 'Пароль пользователя (в зашифрованном виде)'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_phone IS 'Мобильный телефон'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_registration IS 'Дата/время регистрации в системе'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_snils IS 'СНИЛС'
/
COMMENT ON COLUMN vcs_user_login_tmp.user_type IS 'Тип заявителя по умолчанию'
/

-- End of DDL Script for Table PLAN.VCS_USER_LOGIN_TMP

-- Start of DDL Script for Public Synonym VCS_REQUEST_TMP
-- Generated 17-окт-2018 20:52:25 from PUBLIC@DEVALPHA.WORLD

CREATE PUBLIC SYNONYM vcs_request_tmp
  FOR VCS_REQUEST_TMP
/


-- End of DDL Script for Public Synonym VCS_REQUEST_TMP

-- Start of DDL Script for Table PLAN.VCS_REQUEST_TMP
-- Generated 17-окт-2018 20:52:25 from PLAN@DEVALPHA.WORLD

CREATE GLOBAL TEMPORARY TABLE vcs_request_tmp
    (tmp_request_id                 NUMBER,
    tmp_user_id                    NUMBER,
    user_id                        NUMBER,
    num_zayav                      VARCHAR2(300 BYTE),
    kod_spr_query                  NUMBER)
ON COMMIT PRESERVE ROWS

/





-- Indexes for VCS_REQUEST_TMP

CREATE INDEX vcs_request_tmp_tmp_user_id ON vcs_request_tmp
  (
    tmp_user_id                     ASC
  )
NOPARALLEL
LOGGING
/



-- Comments for VCS_REQUEST_TMP

COMMENT ON TABLE vcs_request_tmp IS 'Связь пользователей ЛК с заявками КИДО'
/
COMMENT ON COLUMN vcs_request_tmp.kod_spr_query IS 'Код типа заявки'
/
COMMENT ON COLUMN vcs_request_tmp.num_zayav IS '№ Заявки'
/

-- End of DDL Script for Table PLAN.VCS_REQUEST_TMP

