
DECLARE
    p_date      DATE;
    s_data_id   rr_temp.skod%TYPE;
BEGIN
    p_date := :p_date;
    /*p_date := to_date('01.01.2022','DD.MM.YYYY');*/
    s_data_id := '0AADA478-448B-48CF-8185-57AE48B92444';

    DELETE FROM rr_temp
          WHERE skod = s_data_id;

    ------------------------------------------------------------
    INSERT INTO rr_temp (skod,
                         n1                                         /* kodp */
                           ,
                         n2                                  /* kod_folders */
                           ,
                         s1                                     /* num_dela */
                           ,
                         s2                                      /* fio_upr */
                           ,
                         s3                            /* zayavitel_po_delu */
                           ,
                         d1                            /* dat_zayav_bankrot */
                           ,
                         d2                                   /* dat_create */
                           ,
                         d3                                /* dat_srok_proc */
                           ,
                         d4                                   /* dat_finish */
                           ,
                         n3                                    /* kod_stage */
                           ,
                         n4                                      /* kod_mat */
                           ,
                         n5                                  /* gosposhlina */
                           ,
                         n6                                          /* ogr */
                           ,
                         d5                                     /* dat_post */
                           ,
                         n7                                      /* ostatok */
                           )
        WITH stages_data
             AS (SELECT bk.kod_stage_bk,
                        bk.kod_stage,
                        bk.kod_folders,
                        bk.fio_upr,
                        bk.dat_create,
                        bk.dat_srok_proc,
                        bk.dat_finish
                   FROM ur_stage_bk bk),
            any_stages_data /*72895(1) п.1 */
            as (SELECT bk.kod_folders,
                        max(bk.zayavitel_po_delu) zayavitel_po_delu,
                        max(bk.dat_zayav_bankrot) dat_zayav_bankrot,
                        max(bk.num_dela) num_dela
                   FROM ur_stage_bk bk
                GROUP BY bk.kod_folders
            )
        SELECT s_data_id,
               f.kodp,
               f.kod_folders,
               asd.num_dela,
               stages.fio_upr,
               asd.zayavitel_po_delu,
               asd.dat_zayav_bankrot,
               stages.dat_create,
               stages.dat_srok_proc,
               stages.dat_finish,
               stages.kod_stage,
               hm.kod_mat,
               NVL (hm.gosposhlina, 0),
               NVL (hm.ogr, 0),
               hm.dat_post,
               mor_all.dolg + NVL (hm.ogr, 0) - NVL (hm.opl_ogr, 0)
          FROM (  SELECT f.kodp,
                         f.kod_folders,
                         MAX (st.dat_create) AS dat_create
                    FROM ur_folders f
                         JOIN ur_stage_bk st ON st.kod_folders = f.kod_folders
                   WHERE     f.kod_sdp = 9
                         AND f.kod_podr IN :p_dep
                         AND st.dat_create < p_date
                         AND f.dat_finish IS NULL
                GROUP BY f.kodp, f.kod_folders) f
               JOIN stages_data stages
                   ON     stages.kod_folders = f.kod_folders
                      AND stages.dat_create = f.dat_create
               LEFT JOIN any_stages_data asd
                   ON     asd.kod_folders = f.kod_folders
               JOIN ur_mat m ON m.kod_folders = f.kod_folders
               JOIN ur_hist_mat hm ON hm.kod_mat = m.kod_mat
               JOIN
               (  SELECT dp.kod AS kod_mat,
                         NVL (SUM (  (SELECT NVL (SUM (fr.nachisl), 0)
                                        FROM sr_facras fr
                                       WHERE fr.kod_sf = sf.kod_sf)
                                   - (SELECT NVL (SUM (op.opl), 0)
                                        FROM sr_opl op
                                       WHERE     op.kod_sf = sf.kod_sf
                                             AND op.kod_type_opl IN (0,
                                                                     2,
                                                                     3,
                                                                     4))),
                              0)
                             AS dolg
                    FROM ur_dogplat dp
                         JOIN vv_all_deb_sf sf ON sf.kod_deb_sf = dp.kod_deb_sf
                   WHERE dp.kod_sdp = 15
                GROUP BY dp.kod) mor_all
                   ON mor_all.kod_mat = m.kod_mat
         WHERE mor_all.dolg > 0;

    UPDATE rr_temp t
       SET (n8, n9) =
               (SELECT SUM (
                           CASE WHEN dp.vid_real = 2 THEN dp.sum_v ELSE 0 END),
                       SUM (
                           CASE WHEN dp.vid_real = 7 THEN dp.sum_v ELSE 0 END)
                  FROM ur_dogplat dp
                 WHERE dp.kod = t.n4 AND dp.kod_sdp = 15)
     WHERE skod = s_data_id;

    UPDATE rr_temp t
       SET n10 = NVL (n8, 0) + NVL (n9, 0) + NVL (n5, 0) + NVL (n6, 0)
     WHERE skod = s_data_id;

    UPDATE rr_temp t
       SET s4 =
               (SELECT DISTINCT
                       stragg (
                           TO_CHAR (m.dat_meeting, 'DD.MM.YYYY'))
                       OVER (
                           ORDER BY m.dat_meeting
                           ROWS BETWEEN UNBOUNDED PRECEDING
                                AND     UNBOUNDED FOLLOWING)
                           AS list
                  FROM ur_cred_meeting m
                 WHERE m.kod_folders = t.n2)
     WHERE skod = s_data_id;

    UPDATE rr_temp t
       SET s5 =
               (SELECT m.resheniya
                  FROM ur_cred_meeting m
                 WHERE     m.kod_folders = t.n2
                       AND m.dat_meeting = (SELECT MAX (dat_meeting)
                                              FROM ur_cred_meeting
                                             WHERE kod_folders = t.n2))
     WHERE skod = s_data_id;

    UPDATE rr_temp t
       SET (s6,
            s7,
            s8,
            s9) =
               (SELECT dc.comment_sud_spor_sdel,
                       dc.objal_au_actions,
                       dc.comment_sud_subs_resp,
                       dc.zayav_others
                  FROM ur_au_duty_control dc
                 WHERE dc.kod_au_duty_control IN (SELECT MAX (
                                                             kod_au_duty_control)
                                                    FROM ur_au_duty_control
                                                   WHERE kod_folders = t.n2))
     WHERE skod = s_data_id;
END;