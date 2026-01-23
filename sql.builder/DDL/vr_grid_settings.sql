CREATE TABLE VR_GRID_SETTINGS
(
    kod_gs           NUMBER NOT NULL PRIMARY KEY,
    -- имя отчета    
    repname          VARCHAR2(300) NOT NULL,
    -- имя настройки 
    name             VARCHAR2(300) NOT NULL,
    -- xml-файл с настройками
    data             NCLOB,
	-- 1 - пользовательская, 0 - системная
	visible          NUMBER DEFAULT 1 NOT NULL,
    d_m              DATE NOT NULL,
    u_m              VARCHAR2(30) NOT NULL
)
/
-- сиквенс
CREATE SEQUENCE SQ_GRID_SETTINGS
START WITH 1
INCREMENT BY 1;
/    
-- тригер
CREATE OR REPLACE TRIGGER T_VR_GRID_SETTINGS
BEFORE INSERT OR UPDATE
ON VR_GRID_SETTINGS
REFERENCING NEW AS NEW OLD AS OLD
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        SELECT SQ_GRID_SETTINGS.NEXTVAL INTO :NEW.KOD_GS FROM DUAL;
    END IF;
    SELECT SYSDATE,USER INTO :NEW.D_M,:NEW.U_M FROM DUAL;
END;
/
-- public синоним
CREATE OR REPLACE PUBLIC SYNONYM VR_GRID_SETTINGS FOR VR_GRID_SETTINGS
/
-- Grants for Table
GRANT DELETE ON VR_GRID_SETTINGS TO public
/
GRANT INSERT ON VR_GRID_SETTINGS TO public
/
GRANT SELECT ON VR_GRID_SETTINGS TO public
/
GRANT UPDATE ON VR_GRID_SETTINGS TO public
/

-- Comments for VR_GRID_SETTINGS
COMMENT ON TABLE VR_GRID_SETTINGS IS 'Таблица для хранения пользовательских визуальных настроек отчетов проекта Sql.Builder в формате xml'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.KOD_GS IS 'ключ'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.REPNAME IS 'имя отчета'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.NAME IS 'имя настройки'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.DATA IS 'xml-файл с настройками гридов'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.VISIBLE IS '1 - пользовательская, 0 - системная'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.D_M IS 'дата последнего редактирования'
/
COMMENT ON COLUMN VR_GRID_SETTINGS.U_M IS 'последний редактирующий пользователь'
/
