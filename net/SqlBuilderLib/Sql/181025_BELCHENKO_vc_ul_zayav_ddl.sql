-- Новые колонки
ALTER TABLE vc_ul_zayav ADD num_zayav VARCHAR2(30);
/
-- Комментарии
COMMENT ON TABLE vc_ul_zayav IS 'Связь пользователей ЛК с заявками КИДО';
/
CREATE INDEX vc_ul_zayav_num_zayav ON vc_ul_zayav (num_zayav  ASC )
/
