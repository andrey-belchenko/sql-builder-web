CREATE OR REPLACE VIEW vv_sr_facvip_rec_info (
   kod_sf,
   kod_sf_old_neg )
AS
(-- Код исходного СФ kod_sf_old_neg для СФ - перерасчетов в минус
-- без учета повторных перерасчетов, если понадобится - учесть
SELECT "KOD_SF","KOD_SF_OLD_NEG"
  FROM

  (WITH rec1
             AS (  SELECT /*+materialize*/
                         kod_recalc, kod_sf
                     FROM fin_rec.sr_recalc_sf
                 GROUP BY kod_recalc, kod_sf)
        SELECT rec1.kod_sf,
               CASE WHEN a.pr_negative_recalc = 1 THEN v.kod_sf_old END
                   kod_sf_old_neg
          FROM rec1
               INNER JOIN fin.sr_facvip v ON (rec1.kod_sf = v.kod_sf)
               INNER JOIN
               (  SELECT kod_recalc,
                         CASE WHEN SUM (b.nachisl) <= 0 THEN 1 END
                             pr_negative_recalc
                    FROM rec1 a INNER JOIN fin.sr_facras b ON (a.kod_sf = b.kod_sf)
                GROUP BY kod_recalc) a
                   ON (rec1.kod_recalc = a.kod_recalc)
                   )

 WHERE kod_sf_old_neg IS NOT NULL

 GROUP BY KOD_SF,KOD_SF_OLD_NEG
 )