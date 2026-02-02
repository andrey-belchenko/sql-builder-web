
--Ñâÿçü vc_ul_zayav ñ c_zayav
ALTER TABLE vc_ul_zayav drop CONSTRAINT xfkvc_ul_zayav_kod_zayav
/
ALTER TABLE vc_ul_zayav ADD CONSTRAINT xfkvc_ul_zayav_kod_zayav FOREIGN KEY (kod_zayav) REFERENCES c_zayav (kod_zayav) ON DELETE cascade;

/
--Ñâÿçü vc_ul_zayav ñ vc_user_login
ALTER TABLE vc_ul_zayav drop CONSTRAINT xfkvc_ul_zayav_user_id
/
ALTER TABLE vc_ul_zayav ADD CONSTRAINT xfkvc_ul_zayav_user_id FOREIGN KEY (user_id) REFERENCES vc_user_login (user_id) ON DELETE cascade;

/
