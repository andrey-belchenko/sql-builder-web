CREATE OR REPLACE TRIGGER T_VC_USER_LOGIN
 BEFORE 
 INSERT OR DELETE OR UPDATE
 ON VC_USER_LOGIN
 REFERENCING OLD AS OLD NEW AS NEW
 FOR EACH ROW 
DECLARE
    v_changed   NUMBER := 0;
    v_fio_new   VARCHAR2 (300);
    v_fio_old   VARCHAR2 (300);
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


    IF (UPDATING)
    THEN
        v_fio_new :=
               :new.person_last_name
            || ' '
            || :new.person_first_name
            || ' '
            || :new.person_middle_name;
        v_fio_old :=
               :old.person_last_name
            || ' '
            || :old.person_first_name
            || ' '
            || :old.person_middle_name;

        IF (NVL ( :new.user_phone, '-') != NVL ( :old.user_phone, '-'))
        THEN
            v_changed := 1;

            INSERT INTO vc_ul_izmen (user_id,
                                     kod_kontact,
                                     field,
                                     old_val,
                                     new_val)
                SELECT user_id,
                       kod_kontact,
                       'Телефон',
                       :old.user_phone,
                       :new.user_phone
                  FROM vc_ul_kontakt
                 WHERE user_id = :new.user_id;
        END IF;
        IF (NVL ( :new.user_email, '-') != NVL ( :old.user_email, '-'))
        THEN
            INSERT INTO vc_ul_izmen (user_id,
                                     kod_kontact,
                                     field,
                                     old_val,
                                     new_val)
                SELECT user_id,
                       kod_kontact,
                       'E-mail',
                       :old.user_email,
                       :new.user_email
                  FROM vc_ul_kontakt
                 WHERE user_id = :new.user_id;

            v_changed := 1;
        END IF;
        IF (NVL (v_fio_new, '-') != NVL (v_fio_old, '-'))
        THEN
            INSERT INTO vc_ul_izmen (user_id,
                                     kod_kontact,
                                     field,
                                     old_val,
                                     new_val)
                SELECT user_id,
                       kod_kontact,
                       'ФИО',
                       v_fio_old,
                       v_fio_new
                  FROM vc_ul_kontakt
                 WHERE user_id = :new.user_id;

            v_changed := 1;
        END IF;

        IF (v_changed = 1)
        THEN
            UPDATE is_kontact
               SET oper = v_fio_new,
                   phone = :new.user_phone,
                   e_mail = :new.user_email
             WHERE kod_kontact IN (SELECT kod_kontact
                                     FROM vc_ul_kontakt
                                    WHERE user_id = :new.user_id);
        END IF;
    END IF;
END;
/
