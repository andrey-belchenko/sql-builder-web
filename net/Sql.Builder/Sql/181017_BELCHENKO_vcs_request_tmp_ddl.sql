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
	num_zayav VARCHAR2(300),
	type_of_request NUMBER
)
ON COMMIT PRESERVE ROWS
/
-- Синоним
CREATE PUBLIC SYNONYM vcs_request_tmp FOR vcs_request_tmp;
/
-- Комментарии
COMMENT ON TABLE vcs_request_tmp IS 'Заявка привязанная к профилю пользователя';
COMMENT ON COLUMN vcs_request_tmp.num_zayav IS '№ Заявки';

/

CREATE INDEX vcs_request_tmp_tmp_user_id ON vcs_request_tmp (tmp_user_id  ASC )
/
