-- Уничтожение
DROP TABLE vcs_request_tmp;
DROP PUBLIC SYNONYM vcs_request_tmp;
/
-- Таблица
CREATE  GLOBAL TEMPORARY  TABLE vcs_request_tmp
(
	tmp_request_id NUMBER,
	tmp_user_id NUMBER,
	user_id NUMBER,
	request_id NUMBER,
	request_num VARCHAR2(300),
	status NUMBER,
	request_date DATE,
	address VARCHAR2(300)
)
ON COMMIT PRESERVE ROWS
/
-- Синоним
CREATE PUBLIC SYNONYM vcs_request_tmp FOR vcs_request_tmp;
/
-- Комментарии
COMMENT ON TABLE vcs_request_tmp IS 'Заявка привязанная к профилю пользователя';
COMMENT ON COLUMN vcs_request_tmp.request_date IS 'Дата регистрации';
/
-- Гранты
GRANT SELECT ON vcs_request_tmp TO I_USER2;
GRANT INSERT ON vcs_request_tmp TO I_USER2;
GRANT UPDATE ON vcs_request_tmp TO I_USER2;
GRANT DELETE ON vcs_request_tmp TO I_USER2;
/
CREATE INDEX vcs_request_tmp_tmp_user_id ON vcs_request_tmp (tmp_user_id  ASC )
/
