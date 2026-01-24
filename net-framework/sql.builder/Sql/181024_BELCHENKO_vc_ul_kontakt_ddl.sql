ALTER TABLE vc_ul_kontakt drop CONSTRAINT xfkvc_ul_kontakt_kod_kontact
/
ALTER TABLE vc_ul_kontakt ADD CONSTRAINT xfkvc_ul_kontakt_kod_kontact FOREIGN KEY (kod_kontact) REFERENCES is_kontact (kod_kontact) ON DELETE cascade;
/
--Ñâÿçü vc_ul_kontakt ñ vc_user_login
ALTER TABLE vc_ul_kontakt drop CONSTRAINT xfkvc_ul_kontakt_user_id
/
ALTER TABLE vc_ul_kontakt ADD CONSTRAINT xfkvc_ul_kontakt_user_id FOREIGN KEY (user_id) REFERENCES vc_user_login (user_id) ON DELETE cascade;

/