-- Start of DDL Script for Public Synonym VC_UL_CREDITORS
-- Generated 17-окт-2018 20:48:48 from PUBLIC@DEVALPHA.WORLD

CREATE PUBLIC SYNONYM vc_ul_creditors
  FOR VC_UL_CREDITORS
/


-- End of DDL Script for Public Synonym VC_UL_CREDITORS

-- Start of DDL Script for Table PLAN.VC_UL_CREDITORS
-- Generated 17-окт-2018 20:48:48 from PLAN@DEVALPHA.WORLD

CREATE TABLE vc_ul_creditors
    (kod_ul_creditors               NUMBER ,
    e_code                         NUMBER,
    user_id                        NUMBER,
    u_m                            VARCHAR2(30 BYTE),
    d_m                            DATE)
  SEGMENT CREATION IMMEDIATE
  TABLESPACE  plan_tbl
  NOPARALLEL
  LOGGING
  MONITORING
/





-- Indexes for VC_UL_CREDITORS

CREATE INDEX xfkvc_ul_creditors_e_code ON vc_ul_creditors
  (
    e_code                          ASC
  )
  TABLESPACE  plan_tbl
NOPARALLEL
LOGGING
/

CREATE INDEX xfkvc_ul_creditors_user_id ON vc_ul_creditors
  (
    user_id                         ASC
  )
  TABLESPACE  plan_tbl
NOPARALLEL
LOGGING
/



-- Constraints for VC_UL_CREDITORS

ALTER TABLE vc_ul_creditors
ADD CONSTRAINT xpkvc_ul_creditors PRIMARY KEY (kod_ul_creditors)
USING INDEX
  TABLESPACE  plan_tbl
/




-- Triggers for VC_UL_CREDITORS

CREATE OR REPLACE TRIGGER t_vc_ul_creditors
 BEFORE
  INSERT OR DELETE OR UPDATE
 ON vc_ul_creditors
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
DECLARE
BEGIN


	IF(INSERTING and :NEW.kod_ul_creditors IS NULL) THEN
		SELECT sqvc_ul_creditors.nextval INTO :NEW.kod_ul_creditors FROM DUAL;
	END IF;

	IF  NOT DELETING THEN
		SELECT USER INTO :NEW.u_m FROM DUAL;
		SELECT SYSDATE INTO :NEW.d_m FROM DUAL;
	END IF;

END;
/


-- Comments for VC_UL_CREDITORS

COMMENT ON TABLE vc_ul_creditors IS 'Связь пользователей ЛК с контрагентами'
/
COMMENT ON COLUMN vc_ul_creditors.kod_ul_creditors IS 'Уникальный идентификатор'
/

-- End of DDL Script for Table PLAN.VC_UL_CREDITORS

-- Start of DDL Script for Public Synonym VC_UL_KONTAKT
-- Generated 17-окт-2018 20:48:49 from PUBLIC@DEVALPHA.WORLD

CREATE PUBLIC SYNONYM vc_ul_kontakt
  FOR VC_UL_KONTAKT
/


-- End of DDL Script for Public Synonym VC_UL_KONTAKT

-- Start of DDL Script for Table PLAN.VC_UL_KONTAKT
-- Generated 17-окт-2018 20:48:49 from PLAN@DEVALPHA.WORLD

CREATE TABLE vc_ul_kontakt
    (kod_ul_kontakt                 NUMBER ,
    kod_kontact                    NUMBER,
    user_id                        NUMBER,
    u_m                            VARCHAR2(30 BYTE),
    d_m                            DATE)
  SEGMENT CREATION IMMEDIATE
  TABLESPACE  plan_tbl
  NOPARALLEL
  LOGGING
  MONITORING
/





-- Indexes for VC_UL_KONTAKT

CREATE INDEX xfkvc_ul_kontakt_kod_kontact ON vc_ul_kontakt
  (
    kod_kontact                     ASC
  )
  TABLESPACE  plan_tbl
NOPARALLEL
LOGGING
/

CREATE INDEX xfkvc_ul_kontakt_user_id ON vc_ul_kontakt
  (
    user_id                         ASC
  )
  TABLESPACE  plan_tbl
NOPARALLEL
LOGGING
/



-- Constraints for VC_UL_KONTAKT

ALTER TABLE vc_ul_kontakt
ADD CONSTRAINT xpkvc_ul_kontakt PRIMARY KEY (kod_ul_kontakt)
USING INDEX
  TABLESPACE  plan_tbl
/




-- Triggers for VC_UL_KONTAKT

CREATE OR REPLACE TRIGGER t_vc_ul_kontakt
 BEFORE
  INSERT OR DELETE OR UPDATE
 ON vc_ul_kontakt
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
DECLARE
BEGIN


	IF(INSERTING and :NEW.kod_ul_kontakt IS NULL) THEN
		SELECT sqvc_ul_kontakt.nextval INTO :NEW.kod_ul_kontakt FROM DUAL;
	END IF;

	IF  NOT DELETING THEN
		SELECT USER INTO :NEW.u_m FROM DUAL;
		SELECT SYSDATE INTO :NEW.d_m FROM DUAL;
	END IF;

END;
/


-- Comments for VC_UL_KONTAKT

COMMENT ON TABLE vc_ul_kontakt IS 'Связь пользователей ЛК с контактами КИДО'
/
COMMENT ON COLUMN vc_ul_kontakt.kod_ul_kontakt IS 'Уникальный идентификатор'
/

-- End of DDL Script for Table PLAN.VC_UL_KONTAKT

-- Start of DDL Script for Public Synonym VC_UL_ZAYAV
-- Generated 17-окт-2018 20:48:50 from PUBLIC@DEVALPHA.WORLD

CREATE PUBLIC SYNONYM vc_ul_zayav
  FOR VC_UL_ZAYAV
/


-- End of DDL Script for Public Synonym VC_UL_ZAYAV

-- Start of DDL Script for Table PLAN.VC_UL_ZAYAV
-- Generated 17-окт-2018 20:48:50 from PLAN@DEVALPHA.WORLD

CREATE TABLE vc_ul_zayav
    (kod_ul_zayav                   NUMBER ,
    kod_zayav                      NUMBER,
    user_id                        NUMBER,
    u_m                            VARCHAR2(30 BYTE),
    d_m                            DATE)
  SEGMENT CREATION IMMEDIATE
  TABLESPACE  plan_tbl
  NOPARALLEL
  LOGGING
  MONITORING
/

-- Grants for Table
GRANT DELETE ON vc_ul_zayav TO v_rw_all
/
GRANT INSERT ON vc_ul_zayav TO v_rw_all
/
GRANT SELECT ON vc_ul_zayav TO v_rw_all
/
GRANT UPDATE ON vc_ul_zayav TO v_rw_all
/




-- Indexes for VC_UL_ZAYAV

CREATE INDEX xfkvc_ul_zayav_kod_zayav ON vc_ul_zayav
  (
    kod_zayav                       ASC
  )
  TABLESPACE  plan_tbl
NOPARALLEL
LOGGING
/

CREATE INDEX xfkvc_ul_zayav_user_id ON vc_ul_zayav
  (
    user_id                         ASC
  )
  TABLESPACE  plan_tbl
NOPARALLEL
LOGGING
/



-- Constraints for VC_UL_ZAYAV

ALTER TABLE vc_ul_zayav
ADD CONSTRAINT xpkvc_ul_zayav PRIMARY KEY (kod_ul_zayav)
USING INDEX
  TABLESPACE  plan_tbl
/




-- Triggers for VC_UL_ZAYAV

CREATE OR REPLACE TRIGGER t_vc_ul_zayav
 BEFORE
  INSERT OR DELETE OR UPDATE
 ON vc_ul_zayav
REFERENCING NEW AS NEW OLD AS OLD
 FOR EACH ROW
DECLARE
BEGIN


	IF(INSERTING and :NEW.kod_ul_zayav IS NULL) THEN
		SELECT sqvc_ul_zayav.nextval INTO :NEW.kod_ul_zayav FROM DUAL;
	END IF;

	IF  NOT DELETING THEN
		SELECT USER INTO :NEW.u_m FROM DUAL;
		SELECT SYSDATE INTO :NEW.d_m FROM DUAL;
	END IF;

END;
/


-- Comments for VC_UL_ZAYAV

COMMENT ON TABLE vc_ul_zayav IS 'Связь пользователей ЛК с заявками КИДО'
/
COMMENT ON COLUMN vc_ul_zayav.kod_ul_zayav IS 'Уникальный идентификатор'
/

-- End of DDL Script for Table PLAN.VC_UL_ZAYAV

-- Foreign Key
ALTER TABLE vc_ul_creditors
ADD CONSTRAINT xfkvc_ul_creditors_e_code FOREIGN KEY (e_code)
REFERENCES is_creditors (e_code)
/
ALTER TABLE vc_ul_creditors
ADD CONSTRAINT xfkvc_ul_creditors_user_id FOREIGN KEY (user_id)
REFERENCES vc_user_login (user_id)
/
-- Foreign Key
ALTER TABLE vc_ul_kontakt
ADD CONSTRAINT xfkvc_ul_kontakt_kod_kontact FOREIGN KEY (kod_kontact)
REFERENCES is_kontact (kod_kontact)
/
ALTER TABLE vc_ul_kontakt
ADD CONSTRAINT xfkvc_ul_kontakt_user_id FOREIGN KEY (user_id)
REFERENCES vc_user_login (user_id)
/
-- Foreign Key
ALTER TABLE vc_ul_zayav
ADD CONSTRAINT xfkvc_ul_zayav_kod_zayav FOREIGN KEY (kod_zayav)
REFERENCES c_zayav (kod_zayav)
/
ALTER TABLE vc_ul_zayav
ADD CONSTRAINT xfkvc_ul_zayav_user_id FOREIGN KEY (user_id)
REFERENCES vc_user_login (user_id)
/
-- End of DDL script for Foreign Key(s)
