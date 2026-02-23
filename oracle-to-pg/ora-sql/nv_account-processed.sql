CREATE OR REPLACE VIEW ASUSE.NV_ACCOUNT
 (
 KOD_ACCOUNT, KOD_F46, KOD_NUMOBJ, KOD_DOG, KOD_BU, KOD_NUMOBJ_SA, YM, DEN, 
NUM_PRIEM, DAT_PRIEM, RYM, DAT_PROMEZHUT, TARIF, KOD_F23, VOLTAGE, VID_T, 
NUM_T, CUST, PRICE, NACHISL, INFO, VIST_DAY, KOD_VID_ACT, KOD_SF, PR_SF, 
U_M, D_M, PR_HAND, RASCHET, KOD_BU_AKT, NAL, NDS, NACH, MONEY, VID_REAL, 
KOD_IST, KODP, DEP, PODR, KOD_DOG_FIN, NDOG, PRN_ELVED, PRIZN_SELO, VID_AVANS, 
KOD_TAR_FREE, KOD_DHH, KODINTERVAL, EDIZM, TEP_EL, GR_POINT, VID_T_NAME, 
KOD_OSN_PERERASCH, KOD_TARIF_RATE, KOD_PLAN, KOD_ACCOUNT_PARENT, PR_GENER, 
KOD_AV, KOD_REGLAMENT, CUST_RG
 )
 AS 
SELECT
   A.KOD_ACCOUNT , A.KOD_F46 , A.KOD_NUMOBJ , A.KOD_DOG, a.kod_bu
 , A.KOD_NUMOBJ_SA , A.YM , A.DEN, A.NUM_PRIEM , A.DAT_PRIEM , A.RYM, A.DAT_PROMEZHUT
 , A.TARIF , A.KOD_F23 , A.VOLTAGE , A.VID_T , S.NUM_T, A.CUST
 , A.PRICE , A.NACHISL , A.INFO , A.VIST_DAY , a.kod_vid_act, a.kod_sf, a.pr_sf, A.U_M , A.D_M
 , A.PR_HAND , a.raschet, a.kod_bu_akt
 , SUM(B.NAL) AS NAL ,  SUM(DECODE(B.KOD_NAL, 1, B.NAL,0)) AS NDS, A.NACHISL-NVL(SUM(B.NAL),0) AS NACH
 , nvl(A.NACHISL-NVL(SUM(case b.pr_add when 1 then B.NAL else 0 end),0), ROUND(A.CUST*A.PRICE, 2)) AS MONEY
 , S.VID_REAL, d.kod_ist, d.kodp, d.dep, d.podr, d.kod_dog_fin, d.ndog, d.prn_elved, t.prizn_selo, a.vid_avans, a.kod_tar_free, A.kod_dhh, A.kodinterval
 , s.edizm
 , case when d.tep_el=-8 then d.tep_el else s.tep_el end tep_el
 , O.GR_POINT, s.name as vid_t_name, a.KOD_OSN_PERERASCH, a.kod_tarif_rate, a.kod_plan, a.kod_account_parent, a.pr_gener
 , A.KOD_AV, A.KOD_REGLAMENT, a.cust_rg
FROM
   ec_account.nr_account     A
 , ec_account.nr_account_nal B
 , fin_doc.sk_nachisl     S
 , dog.kr_dogovor     D
 
 , tarif.ks_tarif       T
 , dog_object.kr_numobj      O
WHERE
     A.KOD_ACCOUNT = B.KOD_ACCOUNT (+) AND
     A.VID_T = S.VID_T AND
     D.kod_dog = A.kod_dog AND
     A.tarif = T.tarif(+) AND
     A.kod_numobj = O.kod_numobj(+)

group by A.KOD_ACCOUNT , A.KOD_F46 , A.KOD_NUMOBJ , A.KOD_DOG, a.kod_bu
 , A.KOD_NUMOBJ_SA , A.YM , A.DEN, A.NUM_PRIEM , A.DAT_PRIEM , A.RYM, A.DAT_PROMEZHUT
 , A.TARIF , A.KOD_F23 , A.VOLTAGE , A.VID_T , S.NUM_T, A.CUST
 , A.PRICE , A.NACHISL , A.INFO , A.VIST_DAY , a.kod_vid_act, a.kod_sf, a.pr_sf, A.U_M , A.D_M
 , A.PR_HAND, a.raschet, a.kod_bu_akt, S.VID_REAL, d.kod_ist, d.kodp, d.dep, d.podr, d.kod_dog_fin, d.ndog, d.prn_elved
 , t.prizn_selo, a.vid_avans, a.kod_tar_free, A.kod_dhh, A.kodinterval
 , s.edizm, d.tep_el, s.tep_el, O.GR_POINT, s.name, a.KOD_OSN_PERERASCH, a.kod_tarif_rate, a.kod_plan, a.kod_account_parent, a.pr_gener
 , A.kod_av, A.KOD_REGLAMENT, a.cust_rg