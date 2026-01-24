-- Уничтожение
DROP TABLE vc_ul_izmen;
DROP PUBLIC SYNONYM vc_ul_izmen;
DROP SEQUENCE sqvc_ul_izmen;
/
-- Таблица
CREATE  TABLE vc_ul_izmen
(
	kod_ul_izmen NUMBER,
	user_id NUMBER,
	kod_kontact NUMBER,
	field VARCHAR2(300),
	old_val VARCHAR2(300),
	new_val VARCHAR2(300),
	u_m VARCHAR2(30),
	d_m DATE
)
/
-- Уникальный ключ
ALTER TABLE vc_ul_izmen ADD CONSTRAINT xpkvc_ul_izmen PRIMARY KEY (kod_ul_izmen) USING INDEX;
/
--Связь vc_ul_izmen с vc_user_login
ALTER TABLE vc_ul_izmen ADD CONSTRAINT xfkvc_ul_izmen_user_id FOREIGN KEY (user_id) REFERENCES vc_user_login (user_id) ON DELETE cascade;
CREATE INDEX xfkvc_ul_izmen_user_id ON vc_ul_izmen (user_id);
/
--Связь vc_ul_izmen с is_kontact
ALTER TABLE vc_ul_izmen ADD CONSTRAINT xfkvc_ul_izmen_kod_kontact FOREIGN KEY (kod_kontact) REFERENCES is_kontact (kod_kontact) ON DELETE cascade;
CREATE INDEX xfkvc_ul_izmen_kod_kontact ON vc_ul_izmen (kod_kontact);
/
-- Сиквенс
CREATE SEQUENCE sqvc_ul_izmen INCREMENT BY 1 START WITH 1;
/
-- Триггер
CREATE OR REPLACE TRIGGER t_vc_ul_izmen
BEFORE INSERT OR DELETE OR UPDATE ON vc_ul_izmen
REFERENCING NEW AS NEW OLD AS OLD FOR EACH ROW
DECLARE
BEGIN
	IF(USER = kg_common.repadmin) THEN
		RETURN;
	END IF;

	IF(INSERTING and :NEW.kod_ul_izmen IS NULL) THEN
		SELECT sqvc_ul_izmen.nextval INTO :NEW.kod_ul_izmen FROM DUAL;
	END IF;

	IF rg_kor_util.enabled AND NOT DELETING THEN
		SELECT USER INTO :NEW.u_m FROM DUAL;
		SELECT SYSDATE INTO :NEW.d_m FROM DUAL;
	END IF;

END;
/
-- Синоним
CREATE PUBLIC SYNONYM vc_ul_izmen FOR vc_ul_izmen;
/
-- Комментарии
/
-- Grants for Table
GRANT DELETE ON vc_ul_izmen TO is_spr_edit
/
GRANT INSERT ON vc_ul_izmen TO is_spr_edit
/
GRANT SELECT ON vc_ul_izmen TO is_spr_edit
/
GRANT UPDATE ON vc_ul_izmen TO is_spr_edit
/
GRANT DELETE ON vc_ul_izmen TO i_user2
/
GRANT INSERT ON vc_ul_izmen TO i_user2
/
GRANT SELECT ON vc_ul_izmen TO i_user2
/
GRANT UPDATE ON vc_ul_izmen TO i_user2
/
GRANT DELETE ON vc_ul_izmen TO v_rw_all
/
GRANT INSERT ON vc_ul_izmen TO v_rw_all
/
GRANT SELECT ON vc_ul_izmen TO v_rw_all
/
GRANT UPDATE ON vc_ul_izmen TO v_rw_all
/