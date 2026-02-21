CREATE OR REPLACE PACKAGE BODY sqlb_29814
IS
---Пакет сгенерирован автоматически с помощью Sql.Builder

	PROCEDURE fill_temp
	(
p_date DATE,
p_kod_dog_array_id VARCHAR2,
p_kod_pret_dolg_calc NUMBER
	)
	IS
	BEGIN
		begin
delete from rr_temp where skod = '29814-main';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
d1, --calc_date
n1--kod_pret_dolg_calc
)
--29814-main
--29814-main
select '29814-main' as skod,
'29814-main|' as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select p_date as calc_date, /*date*/
p_kod_pret_dolg_calc as kod_pret_dolg_calc/*number*/
from (
--dual
select dual.dummy as dummy/*string*//*key*/
from dual
dual
--\dual
)
a
--\dual
) mtr
;
--\29814-main
delete from rr_temp where skod = '29814-pre';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_dog
n2, --kod_sf
n3, --kod_deb
s1, --ndog
n4, --ym_vozn
s2, --per_dolg_osn_real
n5, --dolg_osn_real
n6, --dolg_osn_real_deb
n7, --dolg_peni
n8, --dolg_gp
n9, --dolg_peni1
n10, --dolg_sud_peni
n11, --dolg_astr
n12, --dolg_othr
s3, --num_delo
d1, --dat_sud
d2, --dat_calc_peni
d3, --dat_bzad
n13--total_dolg_osn_real
)
--29814-pre
--29814-pre
select '29814-pre' as skod,
'29814-pre|#'||mtr.kod_dog||'#'||mtr.kod_sf||'#'||mtr.kod_deb as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select kod_dog.kod_dog as kod_dog, /*number*//*key*/
kod_sf.kod_sf as kod_sf, /*number*//*key*/
kod_deb.kod_deb as kod_deb, /*number*//*key*/
kod_dog.ndog as ndog, /*Номер договора*//*string*/
kod_sf.ym_vozn as ym_vozn, /*Отчетный период начисления*//*number*/
 case when ( nvl( qube.dolg_osn_real1 ,0)!=0 ) then ( trim(to_char( kod_sf.ym_vozn ,'9999.99')) ) end as per_dolg_osn_real, /*Отчетный период начисления*//*string*/
qube.dolg_osn_real1 as dolg_osn_real, /*Долг по основной реализации*//*number*/
qube.dolg_osn_real2 as dolg_osn_real_deb, /*Долг по основной реализации*//*number*/
qube.sum_peni1 as dolg_peni, /*number*/
qube.dolg_gp1 as dolg_gp, /*Долг ГП*//*number*/
 nvl( qube.dolg_peni1 ,0) -nvl( qube.dolg_sud_peni1 ,0) -nvl( qube.dolg_astr1 ,0) as dolg_peni1, /*Долг пени*//*number*/
qube.dolg_sud_peni1 as dolg_sud_peni, /*Долг взысканные проценты*//*number*/
qube.dolg_astr1 as dolg_astr, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
 nvl( qube.dolg_all1 ,0) -nvl( qube.dolg_osn_real1 ,0) -nvl( qube.dolg_gp1 ,0) -nvl( ( nvl( qube.dolg_peni1 ,0) -nvl( qube.dolg_sud_peni1 ,0) -nvl( qube.dolg_astr1 ,0) ) ,0) -nvl( qube.dolg_sud_peni1 ,0) -nvl( qube.dolg_astr1 ,0) as dolg_othr, /*Иные виды задолженности*//*number*/
qube.num_delo1 as num_delo, /*Номер дела*//*string*/
qube.ur_mat_dat_doc_max1 as dat_sud, /*Дата подачи в суд*//*date*/
qube.dat_calc_peni1 as dat_calc_peni, /*Дата рассчета пени*//*date*/
 case when ( nvl( qube.dolg_osn_real1 ,0)!=0 ) then kod_sf.dat_bzad end as dat_bzad, /*Дата возникновения задолженности*//*date*/
qube.total_dolg_osn_real1 as total_dolg_osn_real/*Дата возникновения обязательства по погашению задолженности*//*number*/
from (
--
select ovr1.kod_dog as kod_dog, /*number*/
ovr1.kod_sf as kod_sf, /*number*/
ovr1.kod_deb as kod_deb, /*number*/
sum(ovr1.dolg_osn_real1) as dolg_osn_real1, /*Долг по основной реализации*//*number*/
sum(ovr1.dolg_osn_real2) as dolg_osn_real2, /*Долг по основной реализации*//*number*/
sum(ovr1.sum_peni1) as sum_peni1, /*number*/
sum(ovr1.dolg_gp1) as dolg_gp1, /*Долг ГП*//*number*/
sum(ovr1.dolg_peni1) as dolg_peni1, /*Долг пени*//*number*/
sum(ovr1.dolg_sud_peni1) as dolg_sud_peni1, /*Долг взысканные проценты*//*number*/
sum(ovr1.dolg_astr1) as dolg_astr1, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
sum(ovr1.dolg_all1) as dolg_all1, /*Начислено*//*number*/
max(ovr1.num_delo1) as num_delo1, /*Номер дела*//*string*/
max(ovr1.ur_mat_dat_doc_max1) as ur_mat_dat_doc_max1, /*Дата подачи в суд*//*date*/
max(ovr1.dat_calc_peni1) as dat_calc_peni1, /*Дата рассчета пени*//*date*/
max(ovr1.total_dolg_osn_real1) as total_dolg_osn_real1/*Дата возникновения обязательства по погашению задолженности*//*number*/
from (
--
select qube.kod_dog as kod_dog, /*number*//*key*/
qube.kod_sf as kod_sf, /*number*//*key*/
qube.kod_deb as kod_deb, /*number*//*key*/
 case when ((qube.dat <= p_date) and (qube.dat_dolg <= p_date) ) then ( case when (vid_real.vid_real in (2) ) then ( nvl( qube.sr_facras_nachisl1 ,0) -nvl( qube.sr_opl_opl_sf1 ,0) ) end ) end as dolg_osn_real1, /*Долг по основной реализации*//*number*/
 case when ((qube.dat <= p_date) and (qube.dat_dolg <= p_date) ) then ( case when (vid_real.vid_real in (2) ) then ( nvl( qube.sr_facras_nachisl2 ,0) -nvl( qube.sr_opl_opl_sf2 ,0) ) end ) end as dolg_osn_real2, /*Долг по основной реализации*//*number*/
 case when (kod_pen.dcalc = p_date) then qube.sr_penni_sum_penni1 end as sum_peni1, /*number*/
 case when ((qube.dat <= p_date) and (vid_real.vid_real in (9) ) ) then ( nvl( qube.sr_facras_nachisl3 ,0) -nvl( qube.sr_opl_opl_sf3 ,0) ) end as dolg_gp1, /*Долг ГП*//*number*/
 case when ((qube.dat <= p_date) and (vid_real.vid_real in (7) ) ) then ( nvl( qube.sr_facras_nachisl3 ,0) -nvl( qube.sr_opl_opl_sf3 ,0) ) end as dolg_peni1, /*Долг пени*//*number*/
 case when ((qube.dat <= p_date) ) then ( nvl( qube.sr_facras_nachisl4 ,0) -nvl( qube.sr_facopl_opl1 ,0) ) end as dolg_sud_peni1, /*Долг взысканные проценты*//*number*/
 case when ((qube.dat <= p_date) ) then ( nvl( qube.sr_facras_nachisl5 ,0) -nvl( qube.sr_facopl_opl2 ,0) ) end as dolg_astr1, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
 nvl( ( case when ((qube.dat <= p_date) and (vid_real.vid_real not in (0 , 2) ) ) then ( nvl( qube.sr_facras_nachisl1 ,0) -nvl( qube.sr_opl_opl_sf1 ,0) ) end ) ,0) +nvl( ( case when ((qube.dat <= p_date) and (vid_real.vid_real = 2) and (qube.dat_dolg <= p_date) ) then ( nvl( qube.sr_facras_nachisl1 ,0) -nvl( qube.sr_opl_opl_sf1 ,0) ) end ) ,0) as dolg_all1, /*Начислено*//*number*/
 last_value( qube.ur_mat_num_delo_max1 ) over( partition by kod_sf.kod_sf order by kod_hist_mat_dec.dat_resh ) as num_delo1, /*Номер дела*//*string*/
qube.ur_mat_dat_doc_max1 as ur_mat_dat_doc_max1, /*Дата подачи в суд*//*date*/
 case when (kod_pen.dcalc = p_date) then qube.sr_penni_dcalc_max1 end as dat_calc_peni1, /*Дата рассчета пени*//*date*/
 sum( ( case when ((qube.dat <= p_date) and (qube.dat_dolg <= p_date) ) then ( case when (vid_real.vid_real in (2) ) then ( nvl( qube.sr_facras_nachisl1 ,0) -nvl( qube.sr_opl_opl_sf1 ,0) ) end ) end ) ) over( partition by kod_dog.kod_dog , kod_sf.rym ) as total_dolg_osn_real1/*Дата возникновения обязательства по погашению задолженности*//*number*/
from (
--
select qube.kod_dog as kod_dog, /*number*/
qube.kod_sf as kod_sf, /*number*/
qube.kod_deb as kod_deb, /*number*/
qube.dat as dat, /*date*//*key*/
qube.dat_dolg as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*/
qube.vid_real as vid_real, /*number*/
qube.kod_pen as kod_pen, /*number*/
qube.kod_hist_mat_dec as kod_hist_mat_dec, /*number*/
qube.sr_facopl_opl2 as sr_facopl_opl2, /*Оплачено*//*number*/
qube.sr_facopl_opl1 as sr_facopl_opl1, /*Оплачено*//*number*/
qube.sr_facras_nachisl1 as sr_facras_nachisl1, /*Начислено*//*number*/
qube.sr_facras_nachisl2 as sr_facras_nachisl2, /*Начислено*//*number*/
qube.sr_facras_nachisl5 as sr_facras_nachisl5, /*Начислено*//*number*/
qube.sr_facras_nachisl4 as sr_facras_nachisl4, /*Начислено*//*number*/
qube.sr_facras_nachisl3 as sr_facras_nachisl3, /*Начислено*//*number*/
qube.sr_opl_opl_sf1 as sr_opl_opl_sf1, /*Оплата начислений*//*number*/
qube.sr_opl_opl_sf2 as sr_opl_opl_sf2, /*Оплата начислений*//*number*/
qube.sr_opl_opl_sf3 as sr_opl_opl_sf3, /*Оплата начислений*//*number*/
qube.sr_penni_dcalc_max1 as sr_penni_dcalc_max1, /*Дата расчета пени*//*date*/
qube.sr_penni_sum_penni1 as sr_penni_sum_penni1, /*number*/
qube.ur_mat_dat_doc_max1 as ur_mat_dat_doc_max1, /*Дата подачи в суд*//*date*/
qube.ur_mat_num_delo_max1 as ur_mat_num_delo_max1/*Номер дела*//*string*/
from (
(
(
--
select qube.dat as dat, /*date*//*key*/
null as dat_dolg,
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
null as kod_hist_mat_dec,
null as kod_pen,
qube.kod_sf as kod_sf, /*number*/
null as vid_real,
qube.sr_facopl_opl2 as sr_facopl_opl2, /*Оплачено*//*number*/
qube.sr_facopl_opl1 as sr_facopl_opl1, /*Оплачено*//*number*/
null as sr_facras_nachisl1, /*number*/
null as sr_facras_nachisl2, /*number*/
qube.sr_facras_nachisl5 as sr_facras_nachisl5, /*Начислено*//*number*/
qube.sr_facras_nachisl4 as sr_facras_nachisl4, /*Начислено*//*number*/
null as sr_facras_nachisl3, /*number*/
null as sr_opl_opl_sf1, /*number*/
null as sr_opl_opl_sf2, /*number*/
null as sr_opl_opl_sf3, /*number*/
null as sr_penni_dcalc_max1, /*date*/
null as sr_penni_sum_penni1, /*number*/
null as ur_mat_dat_doc_max1, /*date*/
null as ur_mat_num_delo_max1/*string*/
from (
--
select un.dat as dat, /*date*//*key*/
un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.kod_sf as kod_sf, /*number*//*key*/
sum(un.sr_facopl_opl2) as sr_facopl_opl2, /*Оплачено*//*number*/
sum(un.sr_facopl_opl1) as sr_facopl_opl1, /*Оплачено*//*number*/
sum(un.sr_facras_nachisl5) as sr_facras_nachisl5, /*Начислено*//*number*/
sum(un.sr_facras_nachisl4) as sr_facras_nachisl4/*Начислено*//*number*/
from (
(
(
--
select sr_facopl_a_d.dat_uch as dat, /*date*//*key*/
kod_sf.kod_deb as kod_deb, /*number*/
kod_sf.kod_dog as kod_dog, /*number*/
kod_ras_a_d.kod_sf as kod_sf, /*number*/
sr_facopl_a_d.opl as sr_facopl_opl2, /*Оплачено*//*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4/*number*/
from (
--sr_facopl
select a.kod_fopl as kod_fopl, /*number*//*key*/
a.kod_ras as kod_ras, /*number*/
a.opl as opl, /*Оплачено*//*number*/
kod_opl.dat_uch as dat_uch/*date*/
from sr_facopl
a
--\sr_facopl
left outer join
(
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.dat_uch as dat_uch/**//*date*/
from sr_opl
a
--\sr_opl
)
kod_opl on a.kod_opl = kod_opl.kod_opl--\sr_opl
)
sr_facopl_a_d
--\sr_facopl
left outer join
(
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.vid_t as vid_t/**//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
kod_ras_a_d on sr_facopl_a_d.kod_ras = kod_ras_a_d.kod_ras--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_ras_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
left outer join
(
--sk_nachisl
select a.vid_t as vid_t/**//*number*//*key*/
from sk_nachisl
a
--\sk_nachisl
)
vid_t on kod_ras_a_d.vid_t = vid_t.vid_t--\sk_nachisl
where
(kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) ) and (vid_t.vid_t in (44) ) )
--\
union all
(
--
select dims.dat as dat, /*date*//*key*/
dims.kod_deb as kod_deb, /*number*/
dims.kod_dog as kod_dog, /*number*/
dims.kod_sf as kod_sf, /*number*/
null as sr_facopl_opl2, /*number*/
sr_facopl.opl as sr_facopl_opl1, /*Оплачено*//*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4/*number*/
from (
--
select sr_facopl_a_d.kod_fopl as kod_fopl_prm, /*number*//*key*/
sr_facopl_a_d.dat_uch as dat, /*date*//*key*/
kod_sf.kod_deb as kod_deb, /*number*//*key*/
kod_sf.kod_dog as kod_dog, /*number*//*key*/
kod_ras_a_d.kod_sf as kod_sf/*number*//*key*/
from (
--sr_facopl
select a.kod_fopl as kod_fopl, /*number*//*key*/
a.kod_ras as kod_ras, /*number*/
kod_opl.dat_uch as dat_uch/*date*/
from sr_facopl
a
--\sr_facopl
left outer join
(
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.dat_uch as dat_uch/**//*date*/
from sr_opl
a
--\sr_opl
)
kod_opl on a.kod_opl = kod_opl.kod_opl--\sr_opl
)
sr_facopl_a_d
--\sr_facopl
left outer join
(
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.vid_t as vid_t/**//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
kod_ras_a_d on sr_facopl_a_d.kod_ras = kod_ras_a_d.kod_ras--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_ras_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
vv_all_deb_sf_a_d1 on vv_all_deb_sf_a_d1.kod_sf = kod_sf.kod_sf--\vv_all_deb_sf
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf/*number*/
from ur_dogplat
a
--\ur_dogplat
)
kod_dogplat_a_d on kod_dogplat_a_d.kod_deb_sf = vv_all_deb_sf_a_d1.kod_deb_sf--\ur_dogplat
left outer join
(
--ur_mat
select a.kod_mat as kod_mat/*number*//*key*/
from ur_mat
a
--\ur_mat
)
kod_mat1_a_d1 on kod_dogplat_a_d.kod_mat = kod_mat1_a_d1.kod_mat--\ur_mat
left outer join
(
--ur_mat_pp
select a.kod_mat as kod_mat, /*number*//*key*/
a.kod_mat as kod_mat_pp/*number*//*key*/
from ur_mat
a
--\ur_mat
left outer join
(
--ur_folders
select a.kod_folders as kod_folders, /*number*//*key*/
a.kod_sdp as kod_sdp/*number*/
from ur_folders
a
--\ur_folders
)
kod_folders on a.kod_folders = kod_folders.kod_folders--\ur_folders
where
kod_folders.kod_sdp = 1)
kod_mat_pp_a_d on kod_mat_pp_a_d.kod_mat = kod_mat1_a_d1.kod_mat--\ur_mat_pp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc, /*number*//*key*/
kod_result.kod_not_edit as kod_not_edit/*number*/
from ur_hist_mat
a
--\ur_hist_mat
left outer join
(
--us_result
select a.kod_result as kod_result, /*number*//*key*/
a.decision as decision, /**//*number*/
a.kod_not_edit as kod_not_edit/*number*/
from us_result
a
--\us_result
)
kod_result on a.kod_result = kod_result.kod_result--\us_result
where
kod_result.decision = 1)
kod_hist_mat_dec on kod_hist_mat_dec.kod_mat = kod_mat_pp_a_d.kod_mat--\ur_hist_mat_dec
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
left outer join
(
--sk_vid_real
select a.vid_real as vid_real/**//*number*//*key*/
from sk_vid_real
a
--\sk_vid_real
)
vid_real on kod_sf.vid_real = vid_real.vid_real--\sk_vid_real
left outer join
(
--sk_nachisl
select a.vid_t as vid_t/**//*number*//*key*/
from sk_nachisl
a
--\sk_nachisl
)
vid_t on kod_ras_a_d.vid_t = vid_t.vid_t--\sk_nachisl
where
(kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) ) and ((vid_real.vid_real in (7) ) and (vid_t.vid_t not in (44) ) and (kod_hist_mat_dec.kod_hist_mat_desc is not null ) and (kod_hist_mat_dec.kod_not_edit = 1) ) group by
sr_facopl_a_d.kod_fopl, /*number*//*key*/
sr_facopl_a_d.dat_uch, /*date*//*key*/
kod_sf.kod_deb, /*number*//*key*/
kod_sf.kod_dog, /*number*//*key*/
kod_ras_a_d.kod_sf/*number*//*key*/
)
dims
--\
left outer join
(
--sr_facopl
select a.kod_fopl as kod_fopl, /*number*//*key*/
a.opl as opl/*Оплачено*//*number*/
from sr_facopl
a
--\sr_facopl
)
sr_facopl on sr_facopl.kod_fopl = dims.kod_fopl_prm--\sr_facopl
)
--\
union all
(
--
select kod_sf.dat_rep as dat, /*Дата документа начисления*//*date*//*key*/
kod_sf.kod_deb as kod_deb, /*number*/
kod_sf.kod_dog as kod_dog, /*number*/
sr_facras_a_d.kod_sf as kod_sf, /*number*/
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
sr_facras_a_d.nachisl as sr_facras_nachisl5, /*Начислено*//*number*/
null as sr_facras_nachisl4/*number*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.vid_t as vid_t, /**//*number*/
a.nachisl as nachisl/*Начислено*//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras_a_d
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
 case when ((( to_number(to_char( a.dat_sf ,'YYYYMM'))/100 ) < a.ym) and (a.vid_real != 0) ) then ( LAST_DAY(to_date( a.ym *100,'YYYYMM')) ) else a.dat_sf end as dat_rep/*Дата документа начисления*//*date*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on sr_facras_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
left outer join
(
--sk_nachisl
select a.vid_t as vid_t/**//*number*//*key*/
from sk_nachisl
a
--\sk_nachisl
)
vid_t on sr_facras_a_d.vid_t = vid_t.vid_t--\sk_nachisl
where
(kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) ) and (vid_t.vid_t in (44) ) )
--\
union all
(
--
select dims.dat as dat, /*Дата документа начисления*//*date*//*key*/
dims.kod_deb as kod_deb, /*number*/
dims.kod_dog as kod_dog, /*number*/
dims.kod_sf as kod_sf, /*number*/
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl5, /*number*/
sr_facras.nachisl as sr_facras_nachisl4/*Начислено*//*number*/
from (
--
select sr_facras_a_d.kod_ras as kod_ras_prm, /*number*//*key*/
kod_sf.dat_rep as dat, /*Дата документа начисления*//*date*//*key*/
kod_sf.kod_deb as kod_deb, /*number*//*key*/
kod_sf.kod_dog as kod_dog, /*number*//*key*/
sr_facras_a_d.kod_sf as kod_sf/*number*//*key*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.vid_t as vid_t/**//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras_a_d
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real, /*number*/
 case when ((( to_number(to_char( a.dat_sf ,'YYYYMM'))/100 ) < a.ym) and (a.vid_real != 0) ) then ( LAST_DAY(to_date( a.ym *100,'YYYYMM')) ) else a.dat_sf end as dat_rep/*Дата документа начисления*//*date*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on sr_facras_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
vv_all_deb_sf_a_d1 on vv_all_deb_sf_a_d1.kod_sf = kod_sf.kod_sf--\vv_all_deb_sf
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf/*number*/
from ur_dogplat
a
--\ur_dogplat
)
kod_dogplat_a_d on kod_dogplat_a_d.kod_deb_sf = vv_all_deb_sf_a_d1.kod_deb_sf--\ur_dogplat
left outer join
(
--ur_mat
select a.kod_mat as kod_mat/*number*//*key*/
from ur_mat
a
--\ur_mat
)
kod_mat1_a_d1 on kod_dogplat_a_d.kod_mat = kod_mat1_a_d1.kod_mat--\ur_mat
left outer join
(
--ur_mat_pp
select a.kod_mat as kod_mat, /*number*//*key*/
a.kod_mat as kod_mat_pp/*number*//*key*/
from ur_mat
a
--\ur_mat
left outer join
(
--ur_folders
select a.kod_folders as kod_folders, /*number*//*key*/
a.kod_sdp as kod_sdp/*number*/
from ur_folders
a
--\ur_folders
)
kod_folders on a.kod_folders = kod_folders.kod_folders--\ur_folders
where
kod_folders.kod_sdp = 1)
kod_mat_pp_a_d on kod_mat_pp_a_d.kod_mat = kod_mat1_a_d1.kod_mat--\ur_mat_pp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc, /*number*//*key*/
kod_result.kod_not_edit as kod_not_edit/*number*/
from ur_hist_mat
a
--\ur_hist_mat
left outer join
(
--us_result
select a.kod_result as kod_result, /*number*//*key*/
a.decision as decision, /**//*number*/
a.kod_not_edit as kod_not_edit/*number*/
from us_result
a
--\us_result
)
kod_result on a.kod_result = kod_result.kod_result--\us_result
where
kod_result.decision = 1)
kod_hist_mat_dec on kod_hist_mat_dec.kod_mat = kod_mat_pp_a_d.kod_mat--\ur_hist_mat_dec
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
left outer join
(
--sk_vid_real
select a.vid_real as vid_real/**//*number*//*key*/
from sk_vid_real
a
--\sk_vid_real
)
vid_real on kod_sf.vid_real = vid_real.vid_real--\sk_vid_real
left outer join
(
--sk_nachisl
select a.vid_t as vid_t/**//*number*//*key*/
from sk_nachisl
a
--\sk_nachisl
)
vid_t on sr_facras_a_d.vid_t = vid_t.vid_t--\sk_nachisl
where
(kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) ) and ((vid_real.vid_real in (7) ) and (vid_t.vid_t not in (44) ) and (kod_hist_mat_dec.kod_hist_mat_desc is not null ) and (kod_hist_mat_dec.kod_not_edit = 1) ) group by
sr_facras_a_d.kod_ras, /*number*//*key*/
kod_sf.dat_rep, /*Дата документа начисления*//*date*//*key*/
kod_sf.kod_deb, /*number*//*key*/
kod_sf.kod_dog, /*number*//*key*/
sr_facras_a_d.kod_sf/*number*//*key*/
)
dims
--\
left outer join
(
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.nachisl as nachisl/*Начислено*//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras on sr_facras.kod_ras = dims.kod_ras_prm--\sr_facras
)
--\
)
--\
)
un
--\
 group by
un.dat, /*date*//*key*/
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.kod_sf/*number*//*key*/
)
qube
--\
)
--\
union all
(
--
select qube.dat as dat, /*Дата документа начисления*//*date*//*key*/
qube.dat_dolg as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*/
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
null as kod_hist_mat_dec,
null as kod_pen,
qube.kod_sf as kod_sf, /*number*/
qube.vid_real as vid_real, /*number*/
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
qube.sr_facras_nachisl1 as sr_facras_nachisl1, /*Начислено*//*number*/
null as sr_facras_nachisl2, /*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4, /*number*/
null as sr_facras_nachisl3, /*number*/
qube.sr_opl_opl_sf1 as sr_opl_opl_sf1, /*Оплата начислений*//*number*/
null as sr_opl_opl_sf2, /*number*/
null as sr_opl_opl_sf3, /*number*/
null as sr_penni_dcalc_max1, /*date*/
null as sr_penni_sum_penni1, /*number*/
null as ur_mat_dat_doc_max1, /*date*/
null as ur_mat_num_delo_max1/*string*/
from (
--
select un.dat as dat, /*Дата документа начисления*//*date*//*key*/
un.dat_dolg as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*//*key*/
un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.kod_sf as kod_sf, /*number*//*key*/
un.vid_real as vid_real, /*number*//*key*/
sum(un.sr_facras_nachisl1) as sr_facras_nachisl1, /*Начислено*//*number*/
sum(un.sr_opl_opl_sf1) as sr_opl_opl_sf1/*Оплата начислений*//*number*/
from (
(
(
--
select kod_sf.dat_rep as dat, /*Дата документа начисления*//*date*//*key*/
kod_sf.dat_bzad as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*/
kod_sf.kod_deb as kod_deb, /*number*/
kod_sf.kod_dog as kod_dog, /*number*/
sr_facras_a_d.kod_sf as kod_sf, /*number*/
kod_sf.vid_real as vid_real, /*number*/
sr_facras_a_d.nachisl as sr_facras_nachisl1, /*Начислено*//*number*/
null as sr_opl_opl_sf1/*number*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.nachisl as nachisl/*Начислено*//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras_a_d
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real, /*number*/
 case when ((( to_number(to_char( a.dat_sf ,'YYYYMM'))/100 ) < a.ym) and (a.vid_real != 0) ) then ( LAST_DAY(to_date( a.ym *100,'YYYYMM')) ) else a.dat_sf end as dat_rep, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_facvip
a
--\sr_facvip
left outer join
(
--sr_debet
select a.kod_deb as kod_deb, /*number*//*key*/
a.dat_bzad as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_debet
a
--\sr_debet
)
kod_deb on a.kod_deb = kod_deb.kod_deb--\sr_debet
where
a.vid_sf not in (2 , 9) )
kod_sf on sr_facras_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) )
--\
union all
(
--
select sr_opl_sf_a_d.dat as dat, /*date*//*key*/
sr_opl_sf_a_d.dat_oper_dolg as dat_dolg, /*Дата операции оплаты задолженности*//*date*/
kod_sf.kod_deb as kod_deb, /*number*/
kod_sf.kod_dog as kod_dog, /*number*/
sr_opl_sf_a_d.kod_sf as kod_sf, /*number*/
kod_sf.vid_real as vid_real, /*number*/
null as sr_facras_nachisl1, /*number*/
sr_opl_sf_a_d.opl_sf as sr_opl_opl_sf1/*Оплата начислений*//*number*/
from (
--sr_opl_sf
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as opl_sf, /*Оплата начислений*//*number*/
 nullif(greatest ( nvl( kod_sf.dat_bzad , to_date('01.01.0001','DD.MM.YYYY')) , nvl( a.dat_uch , to_date('01.01.0001','DD.MM.YYYY')) ), to_date('01.01.0001','DD.MM.YYYY')) as dat_oper_dolg, /*Дата операции оплаты задолженности*//*date*/
a.dat_uch as dat/**//*date*/
from sr_opl
a
--\sr_opl
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_facvip
a
--\sr_facvip
left outer join
(
--sr_debet
select a.kod_deb as kod_deb, /*number*//*key*/
a.dat_bzad as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_debet
a
--\sr_debet
)
kod_deb on a.kod_deb = kod_deb.kod_deb--\sr_debet
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
a.kod_type_opl in (0 , 2 , 3 , 4) )
sr_opl_sf_a_d
--\sr_opl_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on sr_opl_sf_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) )
--\
)
--\
)
un
--\
 group by
un.dat, /*Дата документа начисления*//*date*//*key*/
un.dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*//*key*/
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.kod_sf, /*number*//*key*/
un.vid_real/*number*//*key*/
)
qube
--\
)
--\
union all
(
--
select qube.dat as dat, /*Дата документа начисления*//*date*//*key*/
qube.dat_dolg as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*/
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
null as kod_hist_mat_dec,
null as kod_pen,
null as kod_sf,
qube.vid_real as vid_real, /*number*/
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl1, /*number*/
qube.sr_facras_nachisl2 as sr_facras_nachisl2, /*Начислено*//*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4, /*number*/
null as sr_facras_nachisl3, /*number*/
null as sr_opl_opl_sf1, /*number*/
qube.sr_opl_opl_sf2 as sr_opl_opl_sf2, /*Оплата начислений*//*number*/
null as sr_opl_opl_sf3, /*number*/
null as sr_penni_dcalc_max1, /*date*/
null as sr_penni_sum_penni1, /*number*/
null as ur_mat_dat_doc_max1, /*date*/
null as ur_mat_num_delo_max1/*string*/
from (
--
select un.dat as dat, /*Дата документа начисления*//*date*//*key*/
un.dat_dolg as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*//*key*/
un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.vid_real as vid_real, /*number*//*key*/
sum(un.sr_facras_nachisl2) as sr_facras_nachisl2, /*Начислено*//*number*/
sum(un.sr_opl_opl_sf2) as sr_opl_opl_sf2/*Оплата начислений*//*number*/
from (
(
(
--
select kod_sf_a_d.dat_rep as dat, /*Дата документа начисления*//*date*//*key*/
kod_sf_a_d.dat_bzad as dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*/
kod_sf_a_d.kod_deb as kod_deb, /*number*/
kod_sf_a_d.kod_dog as kod_dog, /*number*/
kod_sf_a_d.vid_real as vid_real, /*number*/
sr_facras_a_d.nachisl as sr_facras_nachisl2, /*Начислено*//*number*/
null as sr_opl_opl_sf2/*number*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.nachisl as nachisl/*Начислено*//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras_a_d
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real, /*number*/
 case when ((( to_number(to_char( a.dat_sf ,'YYYYMM'))/100 ) < a.ym) and (a.vid_real != 0) ) then ( LAST_DAY(to_date( a.ym *100,'YYYYMM')) ) else a.dat_sf end as dat_rep, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_facvip
a
--\sr_facvip
left outer join
(
--sr_debet
select a.kod_deb as kod_deb, /*number*//*key*/
a.dat_bzad as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_debet
a
--\sr_debet
)
kod_deb on a.kod_deb = kod_deb.kod_deb--\sr_debet
where
a.vid_sf not in (2 , 9) )
kod_sf_a_d on sr_facras_a_d.kod_sf = kod_sf_a_d.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf_a_d.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) )
--\
union all
(
--
select sr_opl_sf_a_d.dat as dat, /*date*//*key*/
sr_opl_sf_a_d.dat_oper_dolg as dat_dolg, /*Дата операции оплаты задолженности*//*date*/
kod_sf_a_d.kod_deb as kod_deb, /*number*/
kod_sf_a_d.kod_dog as kod_dog, /*number*/
kod_sf_a_d.vid_real as vid_real, /*number*/
null as sr_facras_nachisl2, /*number*/
sr_opl_sf_a_d.opl_sf as sr_opl_opl_sf2/*Оплата начислений*//*number*/
from (
--sr_opl_sf
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as opl_sf, /*Оплата начислений*//*number*/
 nullif(greatest ( nvl( kod_sf.dat_bzad , to_date('01.01.0001','DD.MM.YYYY')) , nvl( a.dat_uch , to_date('01.01.0001','DD.MM.YYYY')) ), to_date('01.01.0001','DD.MM.YYYY')) as dat_oper_dolg, /*Дата операции оплаты задолженности*//*date*/
a.dat_uch as dat/**//*date*/
from sr_opl
a
--\sr_opl
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_facvip
a
--\sr_facvip
left outer join
(
--sr_debet
select a.kod_deb as kod_deb, /*number*//*key*/
a.dat_bzad as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_debet
a
--\sr_debet
)
kod_deb on a.kod_deb = kod_deb.kod_deb--\sr_debet
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
a.kod_type_opl in (0 , 2 , 3 , 4) )
sr_opl_sf_a_d
--\sr_opl_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf_a_d on sr_opl_sf_a_d.kod_sf = kod_sf_a_d.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf_a_d.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) )
--\
)
--\
)
un
--\
 group by
un.dat, /*Дата документа начисления*//*date*//*key*/
un.dat_dolg, /*Дата возникновения обязательства по погашению задолженности*//*date*//*key*/
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.vid_real/*number*//*key*/
)
qube
--\
)
--\
union all
(
--
select qube.dat as dat, /*Дата документа начисления*//*date*//*key*/
null as dat_dolg,
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
null as kod_hist_mat_dec,
null as kod_pen,
qube.kod_sf as kod_sf, /*number*/
qube.vid_real as vid_real, /*number*/
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl1, /*number*/
null as sr_facras_nachisl2, /*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4, /*number*/
qube.sr_facras_nachisl3 as sr_facras_nachisl3, /*Начислено*//*number*/
null as sr_opl_opl_sf1, /*number*/
null as sr_opl_opl_sf2, /*number*/
qube.sr_opl_opl_sf3 as sr_opl_opl_sf3, /*Оплата начислений*//*number*/
null as sr_penni_dcalc_max1, /*date*/
null as sr_penni_sum_penni1, /*number*/
null as ur_mat_dat_doc_max1, /*date*/
null as ur_mat_num_delo_max1/*string*/
from (
--
select un.dat as dat, /*Дата документа начисления*//*date*//*key*/
un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.kod_sf as kod_sf, /*number*//*key*/
un.vid_real as vid_real, /*number*//*key*/
sum(un.sr_facras_nachisl3) as sr_facras_nachisl3, /*Начислено*//*number*/
sum(un.sr_opl_opl_sf3) as sr_opl_opl_sf3/*Оплата начислений*//*number*/
from (
(
(
--
select kod_sf.dat_rep as dat, /*Дата документа начисления*//*date*//*key*/
kod_sf.kod_deb as kod_deb, /*number*/
kod_sf.kod_dog as kod_dog, /*number*/
sr_facras_a_d.kod_sf as kod_sf, /*number*/
kod_sf.vid_real as vid_real, /*number*/
sr_facras_a_d.nachisl as sr_facras_nachisl3, /*Начислено*//*number*/
null as sr_opl_opl_sf3/*number*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.nachisl as nachisl/*Начислено*//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras_a_d
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real, /*number*/
 case when ((( to_number(to_char( a.dat_sf ,'YYYYMM'))/100 ) < a.ym) and (a.vid_real != 0) ) then ( LAST_DAY(to_date( a.ym *100,'YYYYMM')) ) else a.dat_sf end as dat_rep/*Дата документа начисления*//*date*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on sr_facras_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) )
--\
union all
(
--
select sr_opl_sf_a_d.dat as dat, /*date*//*key*/
kod_sf.kod_deb as kod_deb, /*number*/
kod_sf.kod_dog as kod_dog, /*number*/
sr_opl_sf_a_d.kod_sf as kod_sf, /*number*/
kod_sf.vid_real as vid_real, /*number*/
null as sr_facras_nachisl3, /*number*/
sr_opl_sf_a_d.opl_sf as sr_opl_opl_sf3/*Оплата начислений*//*number*/
from (
--sr_opl_sf
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as opl_sf, /*Оплата начислений*//*number*/
a.dat_uch as dat/**//*date*/
from sr_opl
a
--\sr_opl
where
a.kod_type_opl in (0 , 2 , 3 , 4) )
sr_opl_sf_a_d
--\sr_opl_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb, /*number*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on sr_opl_sf_a_d.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) )
--\
)
--\
)
un
--\
 group by
un.dat, /*Дата документа начисления*//*date*//*key*/
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.kod_sf, /*number*//*key*/
un.vid_real/*number*//*key*/
)
qube
--\
)
--\
union all
(
--
select null as dat, /*key*/
null as dat_dolg,
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
null as kod_hist_mat_dec,
qube.kod_pen as kod_pen, /*number*/
null as kod_sf,
null as vid_real,
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl1, /*number*/
null as sr_facras_nachisl2, /*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4, /*number*/
null as sr_facras_nachisl3, /*number*/
null as sr_opl_opl_sf1, /*number*/
null as sr_opl_opl_sf2, /*number*/
null as sr_opl_opl_sf3, /*number*/
qube.sr_penni_dcalc_max1 as sr_penni_dcalc_max1, /*Дата расчета пени*//*date*/
qube.sr_penni_sum_penni1 as sr_penni_sum_penni1, /*number*/
null as ur_mat_dat_doc_max1, /*date*/
null as ur_mat_num_delo_max1/*string*/
from (
--
select un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.kod_pen as kod_pen, /*number*//*key*/
max(un.sr_penni_dcalc_max1) as sr_penni_dcalc_max1, /*Дата расчета пени*//*date*/
sum(un.sr_penni_sum_penni1) as sr_penni_sum_penni1/*number*/
from (
(
(
--
select kod_pen.kod_deb as kod_deb, /*number*//*key*/
kod_pen.kod_dog as kod_dog, /*number*/
kod_pen.kod_pen as kod_pen, /*number*/
kod_pen.dcalc as sr_penni_dcalc_max1, /*Дата расчета пени*//*date*/
kod_pen.sum_penni as sr_penni_sum_penni1/*number*/
from (
--sr_penni
select a.kod_pen as kod_pen, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.kod_deb as kod_deb, /*number*/
a.dcalc as dcalc, /*Дата расчета пени*//*date*/
a.sum_penni as sum_penni/**//*number*/
from sr_penni
a
--\sr_penni
)
kod_pen
--\sr_penni
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_pen.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
(kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) ) and ((kod_pen.dcalc <= p_date) ) )
--\
)
--\
)
un
--\
 group by
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.kod_pen/*number*//*key*/
)
qube
--\
)
--\
union all
(
--
select null as dat, /*key*/
null as dat_dolg,
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
null as kod_hist_mat_dec,
null as kod_pen,
qube.kod_sf as kod_sf, /*number*/
null as vid_real,
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl1, /*number*/
null as sr_facras_nachisl2, /*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4, /*number*/
null as sr_facras_nachisl3, /*number*/
null as sr_opl_opl_sf1, /*number*/
null as sr_opl_opl_sf2, /*number*/
null as sr_opl_opl_sf3, /*number*/
null as sr_penni_dcalc_max1, /*date*/
null as sr_penni_sum_penni1, /*number*/
qube.ur_mat_dat_doc_max1 as ur_mat_dat_doc_max1, /*Дата подачи в суд*//*date*/
null as ur_mat_num_delo_max1/*string*/
from (
--
select un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.kod_sf as kod_sf, /*number*//*key*/
max(un.ur_mat_dat_doc_max1) as ur_mat_dat_doc_max1/*Дата подачи в суд*//*date*/
from (
(
(
--
select dims.kod_deb as kod_deb, /*number*//*key*/
dims.kod_dog as kod_dog, /*number*/
dims.kod_sf as kod_sf, /*number*/
ur_mat.dat_doc as ur_mat_dat_doc_max1/*Дата подачи в суд*//*date*/
from (
--
select ur_mat_a_d.kod_mat as kod_mat_prm, /*number*//*key*/
kod_sf.kod_deb as kod_deb, /*number*//*key*/
kod_dogplat_a_d.kod_dog as kod_dog, /*number*//*key*/
kod_sf.kod_sf as kod_sf/*number*//*key*/
from (
--ur_mat
select a.kod_mat as kod_mat/*number*//*key*/
from ur_mat
a
--\ur_mat
)
ur_mat_a_d
--\ur_mat
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf/*number*/
from ur_dogplat
a
--\ur_dogplat
)
kod_dogplat_a_d on kod_dogplat_a_d.kod_mat = ur_mat_a_d.kod_mat--\ur_dogplat
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
kod_deb_sf1_a_d1 on kod_dogplat_a_d.kod_deb_sf = kod_deb_sf1_a_d1.kod_deb_sf--\vv_all_deb_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_deb_sf1_a_d1.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_dogplat_a_d.kod_dog = kod_dog.kod_dog--\kr_dogovor
where
kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) group by
ur_mat_a_d.kod_mat, /*number*//*key*/
kod_sf.kod_deb, /*number*//*key*/
kod_dogplat_a_d.kod_dog, /*number*//*key*/
kod_sf.kod_sf/*number*//*key*/
)
dims
--\
left outer join
(
--ur_mat
select a.kod_mat as kod_mat, /*number*//*key*/
a.dat_doc as dat_doc/*Дата подачи в суд*//*date*/
from ur_mat
a
--\ur_mat
)
ur_mat on ur_mat.kod_mat = dims.kod_mat_prm--\ur_mat
)
--\
)
--\
)
un
--\
 group by
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.kod_sf/*number*//*key*/
)
qube
--\
)
--\
union all
(
--
select null as dat, /*key*/
null as dat_dolg,
qube.kod_deb as kod_deb, /*number*/
qube.kod_dog as kod_dog, /*number*/
qube.kod_hist_mat_dec as kod_hist_mat_dec, /*number*/
null as kod_pen,
qube.kod_sf as kod_sf, /*number*/
null as vid_real,
null as sr_facopl_opl2, /*number*/
null as sr_facopl_opl1, /*number*/
null as sr_facras_nachisl1, /*number*/
null as sr_facras_nachisl2, /*number*/
null as sr_facras_nachisl5, /*number*/
null as sr_facras_nachisl4, /*number*/
null as sr_facras_nachisl3, /*number*/
null as sr_opl_opl_sf1, /*number*/
null as sr_opl_opl_sf2, /*number*/
null as sr_opl_opl_sf3, /*number*/
null as sr_penni_dcalc_max1, /*date*/
null as sr_penni_sum_penni1, /*number*/
null as ur_mat_dat_doc_max1, /*date*/
qube.ur_mat_num_delo_max1 as ur_mat_num_delo_max1/*Номер дела*//*string*/
from (
--
select un.kod_deb as kod_deb, /*number*//*key*/
un.kod_dog as kod_dog, /*number*//*key*/
un.kod_hist_mat_dec as kod_hist_mat_dec, /*number*//*key*/
un.kod_sf as kod_sf, /*number*//*key*/
un.ur_mat_num_delo_max1 as ur_mat_num_delo_max1/*Номер дела*//*string*//*key*/
from (
(
(
--
select dims.kod_deb as kod_deb, /*number*//*key*/
dims.kod_dog as kod_dog, /*number*/
dims.kod_hist_mat_dec as kod_hist_mat_dec, /*number*/
dims.kod_sf as kod_sf, /*number*/
ur_mat.num_delo_max as ur_mat_num_delo_max1/*Номер дела*//*string*/
from (
--
select ur_mat_a_d.kod_mat as kod_mat_prm, /*number*//*key*/
kod_sf.kod_deb as kod_deb, /*number*//*key*/
kod_dogplat_a_d.kod_dog as kod_dog, /*number*//*key*/
kod_hist_mat_dec.kod_hist_mat as kod_hist_mat_dec, /*number*//*key*/
kod_sf.kod_sf as kod_sf/*number*//*key*/
from (
--ur_mat
select a.kod_mat as kod_mat/*number*//*key*/
from ur_mat
a
--\ur_mat
)
ur_mat_a_d
--\ur_mat
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf/*number*/
from ur_dogplat
a
--\ur_dogplat
)
kod_dogplat_a_d on kod_dogplat_a_d.kod_mat = ur_mat_a_d.kod_mat--\ur_dogplat
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
kod_deb_sf1_a_d1 on kod_dogplat_a_d.kod_deb_sf = kod_deb_sf1_a_d1.kod_deb_sf--\vv_all_deb_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_deb as kod_deb/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_deb_sf1_a_d1.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_dogplat_a_d.kod_dog = kod_dog.kod_dog--\kr_dogovor
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc, /*number*//*key*/
kod_result.kod_not_edit as kod_not_edit/*number*/
from ur_hist_mat
a
--\ur_hist_mat
left outer join
(
--us_result
select a.kod_result as kod_result, /*number*//*key*/
a.decision as decision, /**//*number*/
a.kod_not_edit as kod_not_edit/*number*/
from us_result
a
--\us_result
)
kod_result on a.kod_result = kod_result.kod_result--\us_result
where
kod_result.decision = 1)
kod_hist_mat_dec on kod_hist_mat_dec.kod_mat = ur_mat_a_d.kod_mat--\ur_hist_mat_dec
where
(kod_dog.kod_dog in (select /*+ dynamic_sampling(a 10)*/ val from vr_number_array a where array_id= p_kod_dog_array_id ) ) and (kod_hist_mat_dec.kod_not_edit = 1) group by
ur_mat_a_d.kod_mat, /*number*//*key*/
kod_sf.kod_deb, /*number*//*key*/
kod_dogplat_a_d.kod_dog, /*number*//*key*/
kod_hist_mat_dec.kod_hist_mat, /*number*//*key*/
kod_sf.kod_sf/*number*//*key*/
)
dims
--\
left outer join
(
--ur_mat
select a.kod_mat as kod_mat, /*number*//*key*/
a.num_delo as num_delo_max/*Номер дела*//*string*/
from ur_mat
a
--\ur_mat
)
ur_mat on ur_mat.kod_mat = dims.kod_mat_prm--\ur_mat
)
--\
)
--\
)
un
--\
 group by
un.kod_deb, /*number*//*key*/
un.kod_dog, /*number*//*key*/
un.kod_hist_mat_dec, /*number*//*key*/
un.kod_sf, /*number*//*key*/
un.ur_mat_num_delo_max1/*Номер дела*//*string*//*key*/
)
qube
--\
)
--\
)
--\
)
qube
--\
)
qube
--\
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_hist_mat as kod_hist_mat_desc, /*number*//*key*/
a.dat_post as dat_resh/*Дата принятия решения*/
from ur_hist_mat
a
--\ur_hist_mat
left outer join
(
--us_result
select a.kod_result as kod_result, /*number*//*key*/
a.decision as decision/**//*number*/
from us_result
a
--\us_result
)
kod_result on a.kod_result = kod_result.kod_result--\us_result
where
kod_result.decision = 1)
kod_hist_mat_dec on kod_hist_mat_dec.kod_hist_mat = qube.kod_hist_mat_dec--\ur_hist_mat_dec
left outer join
(
--sr_penni
select a.kod_pen as kod_pen, /*number*//*key*/
a.dcalc as dcalc/*Дата расчета пени*//*date*/
from sr_penni
a
--\sr_penni
)
kod_pen on kod_pen.kod_pen = qube.kod_pen--\sr_penni
left outer join
(
--sk_vid_real
select a.vid_real as vid_real/**//*number*//*key*/
from sk_vid_real
a
--\sk_vid_real
)
vid_real on vid_real.vid_real = qube.vid_real--\sk_vid_real
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
kod_sf_dop.rym as rym/*number*/
from sr_facvip
a
--\sr_facvip
left outer join
(
--sr_facvip_dop
select sr_facras.kod_sf as kod_sf, /*number*//*key*/
min(sr_facras.rym) as rym/*number*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.rym as rym/**//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras
--\sr_facras
 group by
sr_facras.kod_sf/*number*//*key*/
)
kod_sf_dop on a.kod_sf = kod_sf_dop.kod_sf--\sr_facvip_dop
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_sf.kod_sf = qube.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_dog.kod_dog = qube.kod_dog--\kr_dogovor
)
ovr1
--\
 group by
ovr1.kod_dog, /*number*/
ovr1.kod_sf, /*number*/
ovr1.kod_deb/*number*/
)
qube
--\
left outer join
(
--sr_debet
select a.kod_deb as kod_deb/*number*//*key*/
from sr_debet
a
--\sr_debet
)
kod_deb on kod_deb.kod_deb = qube.kod_deb--\sr_debet
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
 case when (a.vid_real = 0) then kod_sf_dop.rym else ( nvl( kod_sf_old_neg.ym , a.ym ) ) end as ym_vozn/*Отчетный период начисления*//*number*/
from sr_facvip
a
--\sr_facvip
left outer join
(
--sr_facvip_dop
select sr_facras.kod_sf as kod_sf, /*number*//*key*/
min(sr_facras.rym) as rym/*number*/
from (
--sr_facras
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.rym as rym/**//*number*/
from sr_facras
a
--\sr_facras
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.vid_sf as vid_sf/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
kod_sf.vid_sf not in (2 , 9) )
sr_facras
--\sr_facras
 group by
sr_facras.kod_sf/*number*//*key*/
)
kod_sf_dop on a.kod_sf = kod_sf_dop.kod_sf--\sr_facvip_dop
left outer join
(
--sr_debet
select a.kod_deb as kod_deb, /*number*//*key*/
a.dat_bzad as dat_bzad/*Дата возникновения обязательства по погашению задолженности*//*date*/
from sr_debet
a
--\sr_debet
)
kod_deb on a.kod_deb = kod_deb.kod_deb--\sr_debet
left outer join
(
--vv_sr_facvip_rec_info_m
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf_old_neg as kod_sf_old_neg/*number*/
from vv_sr_facvip_rec_info_m
a
--\vv_sr_facvip_rec_info_m
)
kod_sf_rec_info on a.kod_sf = kod_sf_rec_info.kod_sf--\vv_sr_facvip_rec_info_m
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym/*Отчетный период начисления*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf_old_neg on kod_sf_rec_info.kod_sf_old_neg = kod_sf_old_neg.kod_sf--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_sf.kod_sf = qube.kod_sf--\sr_facvip
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.ndog as ndog/*Номер договора*//*string*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_dog.kod_dog = qube.kod_dog--\kr_dogovor
where
(( nvl( qube.total_dolg_osn_real1 ,0)!=0 ) and ( nvl( qube.dolg_osn_real1 ,0)!=0 ) ) or ( nvl( qube.sum_peni1 ,0)!=0 ) or ( nvl( qube.dolg_gp1 ,0)!=0 or nvl( qube.sum_peni1 ,0)!=0 or nvl( qube.dolg_sud_peni1 ,0)!=0 or nvl( qube.dolg_astr1 ,0)!=0 or nvl( ( nvl( qube.dolg_all1 ,0) -nvl( qube.dolg_osn_real1 ,0) -nvl( qube.dolg_gp1 ,0) -nvl( ( nvl( qube.dolg_peni1 ,0) -nvl( qube.dolg_sud_peni1 ,0) -nvl( qube.dolg_astr1 ,0) ) ,0) -nvl( qube.dolg_sud_peni1 ,0) -nvl( qube.dolg_astr1 ,0) ) ,0)!=0 or nvl( qube.dolg_osn_real2 ,0)!=0 ) ) mtr
;
--\29814-pre
delete from rr_temp where skod = '29814-dog';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_dog
s1, --per_dolg_osn_real
n2, --dolg_osn_real
n3, --dolg_gp
n4, --dolg_peni
n5, --dolg_sud_peni
n6, --dolg_astr
s2, --num_delo
n7, --dolg_othr
d1, --dat_calc_peni
s3, --cnt_per_dolg
d2, --dat_sud
d3, --dat_beg_dolg
sparentid--sparentid
)
--29814-dog
--29814-dog
select '29814-dog' as skod,
'29814-dog|#'||mtr.kod_dog as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select dog.kod_dog as kod_dog, /*number*//*key*/
dog.per_dolg_osn_real as per_dolg_osn_real, /*Отчетный период начисления*//*string*/
dog.dolg_osn_real as dolg_osn_real, /*Долг по основной реализации*//*number*/
dog.dolg_gp as dolg_gp, /*Долг ГП*//*number*/
dog.dolg_peni as dolg_peni, /*number*/
dog.dolg_sud_peni as dolg_sud_peni, /*Долг взысканные проценты*//*number*/
dog.dolg_astr as dolg_astr, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
dog.num_delo as num_delo, /*Номер дела*//*string*/
dog.dolg_othr as dolg_othr, /*Иные виды задолженности*//*number*/
dog.dat_calc_peni as dat_calc_peni, /*Дата рассчета пени*//*date*/
dog.cnt_per_dolg as cnt_per_dolg, /*Количество периодов задолженности*//*string*/
dog.dat_sud as dat_sud, /*Дата подачи в суд*//*date*/
dog.dat_beg_dolg as dat_beg_dolg, /*Дата возникновения задолженности*//*date*/
main.sid as sparentid
from (
--29814-dog
select a.kod_dog as kod_dog, /*number*//*key*/
 vg_period.ym_enum_str_to_date_ranges_str( stragg(a.per_dolg_osn_real) ) as per_dolg_osn_real, /*Отчетный период начисления*//*string*/
sum(a.dolg_osn_real) as dolg_osn_real, /*Долг по основной реализации*//*number*/
sum(a.dolg_gp) as dolg_gp, /*Долг ГП*//*number*/
sum(a.dolg_peni) as dolg_peni, /*number*/
sum(a.dolg_sud_peni) as dolg_sud_peni, /*Долг взысканные проценты*//*number*/
sum(a.dolg_astr) as dolg_astr, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
stragg_dist(a.num_delo) as num_delo, /*Номер дела*//*string*/
sum(a.dolg_othr) as dolg_othr, /*Иные виды задолженности*//*number*/
max(a.dat_calc_peni) as dat_calc_peni, /*Дата рассчета пени*//*date*/
 count(distinct a.per_dolg_osn_real) as cnt_per_dolg, /*Количество периодов задолженности*//*string*/
max(a.dat_sud) as dat_sud, /*Дата подачи в суд*//*date*/
min(a.dat_bzad) as dat_beg_dolg/*Дата возникновения задолженности*//*date*/
from (
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_dog,
n2 as kod_sf,
n3 as kod_deb,
s1 as ndog,
n4 as ym_vozn,
s2 as per_dolg_osn_real,
n5 as dolg_osn_real,
n6 as dolg_osn_real_deb,
n7 as dolg_peni,
n8 as dolg_gp,
n9 as dolg_peni1,
n10 as dolg_sud_peni,
n11 as dolg_astr,
n12 as dolg_othr,
s3 as num_delo,
d1 as dat_sud,
d2 as dat_calc_peni,
d3 as dat_bzad,
n13 as total_dolg_osn_real
from rr_temp where skod = '29814-pre'
)
a
--\29814-pre
 group by
a.kod_dog/*number*//*key*/
)
dog
--\29814-dog
inner join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
d1 as calc_date
from rr_temp where skod = '29814-main'
)
main on main.calc_date is not null --\29814-main
) mtr
;
--\29814-dog
delete from rr_temp where skod = '29814-sf';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_dog
n2, --kod_sf
n3, --dolg_osn_real
n4, --dolg_gp
n5, --dolg_peni
n6, --dolg_sud_peni
n7, --dolg_astr
s1, --num_delo
n8, --dolg_othr
d1, --dat_calc_peni
d2, --dat_sud
sparentid--sparentid
)
--29814-sf
--29814-sf
select '29814-sf' as skod,
'29814-sf|#'||mtr.kod_dog||'#'||mtr.kod_sf as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select sf.kod_dog as kod_dog, /*number*//*key*/
sf.kod_sf as kod_sf, /*number*//*key*/
sf.dolg_osn_real as dolg_osn_real, /*Долг по основной реализации*//*number*/
sf.dolg_gp as dolg_gp, /*Долг ГП*//*number*/
sf.dolg_peni as dolg_peni, /*number*/
sf.dolg_sud_peni as dolg_sud_peni, /*Долг взысканные проценты*//*number*/
sf.dolg_astr as dolg_astr, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
sf.num_delo as num_delo, /*Номер дела*//*string*/
sf.dolg_othr as dolg_othr, /*Иные виды задолженности*//*number*/
sf.dat_calc_peni as dat_calc_peni, /*Дата рассчета пени*//*date*/
sf.dat_sud as dat_sud, /*Дата подачи в суд*//*date*/
dog.sid as sparentid
from (
--29814-sf
select a.kod_dog as kod_dog, /*number*//*key*/
a.kod_sf as kod_sf, /*number*//*key*/
sum(a.dolg_osn_real) as dolg_osn_real, /*Долг по основной реализации*//*number*/
sum(a.dolg_gp) as dolg_gp, /*Долг ГП*//*number*/
sum(a.dolg_peni) as dolg_peni, /*number*/
sum(a.dolg_sud_peni) as dolg_sud_peni, /*Долг взысканные проценты*//*number*/
sum(a.dolg_astr) as dolg_astr, /*Долг астрент (проценты за несвоевременное исполнение решения )*//*number*/
max(a.num_delo) as num_delo, /*Номер дела*//*string*/
sum(a.dolg_othr) as dolg_othr, /*Иные виды задолженности*//*number*/
max(a.dat_calc_peni) as dat_calc_peni, /*Дата рассчета пени*//*date*/
max(a.dat_sud) as dat_sud/*Дата подачи в суд*//*date*/
from (
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_dog,
n2 as kod_sf,
n3 as kod_deb,
s1 as ndog,
n4 as ym_vozn,
s2 as per_dolg_osn_real,
n5 as dolg_osn_real,
n6 as dolg_osn_real_deb,
n7 as dolg_peni,
n8 as dolg_gp,
n9 as dolg_peni1,
n10 as dolg_sud_peni,
n11 as dolg_astr,
n12 as dolg_othr,
s3 as num_delo,
d1 as dat_sud,
d2 as dat_calc_peni,
d3 as dat_bzad,
n13 as total_dolg_osn_real
from rr_temp where skod = '29814-pre'
)
a
--\29814-pre
 group by
a.kod_dog, /*number*//*key*/
a.kod_sf/*number*//*key*/
)
sf
--\29814-sf
inner join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_dog
from rr_temp where skod = '29814-dog'
)
dog on dog.kod_dog = sf.kod_dog--\29814-dog
) mtr
;
--\29814-sf
end;
	END fill_temp;
	PROCEDURE fill_table
	(
p_date DATE,
p_kod_dog_array_id VARCHAR2,
p_kod_pret_dolg_calc NUMBER
	)
	IS

 p_key_main number;

 p_key_dog number;

 p_key_sf number;
	BEGIN
fill_temp
	(
p_date,
p_kod_dog_array_id,
p_kod_pret_dolg_calc
	)
	;
for rr_temp_rec in (select
n1,n2,n3,n4,n5,n6,n7,s1,n8,d1,d2,sparentid,sid,s2,s3,d3,skod
from rr_temp
where  skod  in (
'29814-main'
,'29814-dog'
,'29814-sf'
)
connect by prior sid=sparentid
start with  sparentid is null
) loop
if rr_temp_rec.skod ='29814-main' then
p_key_main:=null;
insert into vr_pret_dolg_calc (calc_date,kod_pret_dolg_calc
) values (rr_temp_rec.d1,rr_temp_rec.n1)  returning kod_pret_dolg_calc  into p_key_main;
elsif rr_temp_rec.skod ='29814-dog' then
p_key_dog:=null;
insert into vr_pret_dolg_dog (kod_dog,per_dolg_osn_real,dolg_osn_real,dolg_gp,dolg_peni,dolg_sud_peni,dolg_astr,num_delo,dolg_othr,dat_calc_peni,cnt_per_dolg,dat_sud,dat_beg_dolg,kod_pret_dolg_calc
) values (rr_temp_rec.n1,rr_temp_rec.s1,rr_temp_rec.n2,rr_temp_rec.n3,rr_temp_rec.n4,rr_temp_rec.n5,rr_temp_rec.n6,rr_temp_rec.s2,rr_temp_rec.n7,rr_temp_rec.d1,rr_temp_rec.s3,rr_temp_rec.d2,rr_temp_rec.d3,p_key_main)  returning kod_pret_dolg_dog  into p_key_dog;
elsif rr_temp_rec.skod ='29814-sf' then
p_key_sf:=null;
insert into vr_pret_dolg_sf (kod_dog,kod_sf,dolg_osn_real,dolg_gp,dolg_peni,dolg_sud_peni,dolg_astr,num_delo,dolg_othr,dat_calc_peni,dat_sud,kod_pret_dolg_dog
) values (rr_temp_rec.n1,rr_temp_rec.n2,rr_temp_rec.n3,rr_temp_rec.n4,rr_temp_rec.n5,rr_temp_rec.n6,rr_temp_rec.n7,rr_temp_rec.s1,rr_temp_rec.n8,rr_temp_rec.d1,rr_temp_rec.d2,p_key_dog)  returning kod_pret_dolg_sf  into p_key_sf;
end if;
end loop;
	END fill_table;
END sqlb_29814;

