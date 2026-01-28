begin
delete from rr_temp where skod = '52834-pre_astr_nach_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2--nachisl_astr
)
--52834-pre_astr_nach_g
--52834-pre_astr_nach_g
select '52834-pre_astr_nach_g' as skod,
'52834-pre_astr_nach_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.nachisl_astr) as nachisl_astr/*Начислено астрент*//*number*/
from (
--52834-pre_astr_nach
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_facras_astr.kod_ras_astr as kod_ras_astr, /*number*//*key*/
sr_facras_astr.nachisl_astr as nachisl_astr/*Начислено астрент*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--sr_penni_astr
select nvl( kod_sf.kod_sf_first , kod_sf.kod_sf ) as kod_sf_astr, /*number*/
a.kod_delo as kod_delo/*number*/
from (
--sr_penni
select a.kod_pen as kod_pen, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.kod_delo as kod_delo/*number*/
from sr_penni
a
--\sr_penni
)
a
--\sr_penni
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf_first as kod_sf_first/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
a.kod_delo is not null )
sr_penni_astr on sr_penni_astr.kod_delo = kod_mat_pp.kod_mat--\sr_penni_astr
left outer join
(
--sr_facvip_astr
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf as kod_sf_astr/*number*//*key*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf_astr on sr_penni_astr.kod_sf_astr = kod_sf_astr.kod_sf--\sr_facvip_astr
left outer join
(
--sr_facras_astr
select a.kod_ras as kod_ras, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.kod_ras as kod_ras_astr, /*number*//*key*/
a.nachisl as nachisl_astr/*Начислено астрент*//*number*/
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
sr_facras_astr on sr_facras_astr.kod_sf = kod_sf_astr.kod_sf--\sr_facras_astr
 group by
ur_isp.kod_isp, /*number*//*key*/
sr_facras_astr.kod_ras_astr, /*number*//*key*/
sr_facras_astr.nachisl_astr/*Начислено астрент*//*number*//*key*/
)
a
--\52834-pre_astr_nach
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_astr_nach_g
delete from rr_temp where skod = '52834-pre_astr_opl_do_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2--oplf
)
--52834-pre_astr_opl_do_g
--52834-pre_astr_opl_do_g
select '52834-pre_astr_opl_do_g' as skod,
'52834-pre_astr_opl_do_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.oplf) as oplf/*Оплачено*//*number*/
from (
--52834-pre_astr_opl_do
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_opl_astr.kod_opl as kod_opl, /*number*//*key*/
sr_opl_astr.dat_opl as dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_astr.oplf as oplf/*Оплачено*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--sr_penni_astr
select nvl( kod_sf.kod_sf_first , kod_sf.kod_sf ) as kod_sf_astr, /*number*/
a.kod_delo as kod_delo/*number*/
from (
--sr_penni
select a.kod_pen as kod_pen, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.kod_delo as kod_delo/*number*/
from sr_penni
a
--\sr_penni
)
a
--\sr_penni
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf_first as kod_sf_first/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
a.kod_delo is not null )
sr_penni_astr on sr_penni_astr.kod_delo = kod_mat_pp.kod_mat--\sr_penni_astr
left outer join
(
--sr_facvip_astr
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf as kod_sf_astr/*number*//*key*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf_astr on sr_penni_astr.kod_sf_astr = kod_sf_astr.kod_sf--\sr_facvip_astr
left outer join
(
--sr_opl_astr
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.dat_opl as dat_opl, /*Дата платежного документа*//*date*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as oplf/*Оплачено*//*number*/
from sr_opl
a
--\sr_opl
)
sr_opl_astr on sr_opl_astr.kod_sf = kod_sf_astr.kod_sf--\sr_opl_astr
where
sr_opl_astr.dat_opl < ( nvl( ur_isp.dat_doc , DATE'8999-12-31' ) ) group by
ur_isp.kod_isp, /*number*//*key*/
sr_opl_astr.kod_opl, /*number*//*key*/
sr_opl_astr.dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_astr.oplf/*Оплачено*//*number*//*key*/
)
a
--\52834-pre_astr_opl_do
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_astr_opl_do_g
delete from rr_temp where skod = '52834-pre_astr_opl_posl_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
d1, --dat_opl
n2, --oplf
n3--oplf_before_not_done_dat
)
--52834-pre_astr_opl_posl_g
--52834-pre_astr_opl_posl_g
select '52834-pre_astr_opl_posl_g' as skod,
'52834-pre_astr_opl_posl_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
max(a.dat_opl) as dat_opl, /*Дата платежного документа*//*date*/
sum(a.oplf) as oplf, /*Оплачено*//*number*/
sum( case when (( 1=1 )  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_before_not_done_dat/*Оплачено*//*number*/
from (
--52834-pre_astr_opl_posl
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_opl_astr.kod_opl as kod_opl, /*number*//*key*/
sr_opl_astr.dat_opl as dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_astr.oplf as oplf/*Оплачено*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--sr_penni_astr
select nvl( kod_sf.kod_sf_first , kod_sf.kod_sf ) as kod_sf_astr, /*number*/
a.kod_delo as kod_delo/*number*/
from (
--sr_penni
select a.kod_pen as kod_pen, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.kod_delo as kod_delo/*number*/
from sr_penni
a
--\sr_penni
)
a
--\sr_penni
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf_first as kod_sf_first/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on a.kod_sf = kod_sf.kod_sf--\sr_facvip
where
a.kod_delo is not null )
sr_penni_astr on sr_penni_astr.kod_delo = kod_mat_pp.kod_mat--\sr_penni_astr
left outer join
(
--sr_facvip_astr
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_sf as kod_sf_astr/*number*//*key*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf_astr on sr_penni_astr.kod_sf_astr = kod_sf_astr.kod_sf--\sr_facvip_astr
left outer join
(
--sr_opl_astr
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.dat_opl as dat_opl, /*Дата платежного документа*//*date*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as oplf/*Оплачено*//*number*/
from sr_opl
a
--\sr_opl
)
sr_opl_astr on sr_opl_astr.kod_sf = kod_sf_astr.kod_sf--\sr_opl_astr
where
sr_opl_astr.dat_opl >= ur_isp.dat_doc group by
ur_isp.kod_isp, /*number*//*key*/
sr_opl_astr.kod_opl, /*number*//*key*/
sr_opl_astr.dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_astr.oplf/*Оплачено*//*number*//*key*/
)
a
--\52834-pre_astr_opl_posl
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_astr_opl_posl_g
delete from rr_temp where skod = '52834-pre_dogplat_nach_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2, --sum_v
n3, --sum_v_osn
n4, --sum_v_peni
n5, --sum_v_gp
s1, --ndog
n6, --ym_min_priz
n7, --ym_max_priz
s2--peni_per
)
--52834-pre_dogplat_nach_g
--52834-pre_dogplat_nach_g
select '52834-pre_dogplat_nach_g' as skod,
'52834-pre_dogplat_nach_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.sum_v) as sum_v, /*Признано по юр. документу*//*number*/
sum( case when (a.vid_real = 2) then a.sum_v end ) as sum_v_osn, /*Признано по юр. документу*//*number*/
sum( case when (a.vid_real = 7) then a.sum_v end ) as sum_v_peni, /*Признано по юр. документу*//*number*/
sum( case when (a.vid_real = 9) then a.sum_v end ) as sum_v_gp, /*Признано по юр. документу*//*number*/
stragg_dist(a.ndog) as ndog, /*Номер договора*//*string*/
min(a.ym_min_priz) as ym_min_priz, /*Период начисления с (призн.)*//*number*/
max(a.ym_max_priz) as ym_max_priz, /*Период начисления по (призн.)*//*number*/
stragg(a.peni_per) as peni_per/*Период*//*string*/
from (
--52834-pre_dogplat_nach
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
ur_dogplat.kod_dogplat as kod_dogplat, /*number*//*key*/
ur_dogplat.sum_v as sum_v, /*Признано по юр. документу*//*number*//*key*/
ur_dogplat.vid_real as vid_real, /*number*//*key*/
ur_dogplat.ym_min_priz as ym_min_priz, /*Период начисления с (призн.)*//*number*//*key*/
ur_dogplat.ym_max_priz as ym_max_priz, /*Период начисления по (призн.)*//*number*//*key*/
max(kod_dog.ndog) as ndog, /*Номер договора*//*string*/
stragg(vr_peni_period.name) as peni_per/*Период*//*string*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.vid_real as vid_real, /**//*number*/
a.sum_v as sum_v, /*Признано по юр. документу*//*number*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf, /*number*/
 case when ( nvl( a.sum_v ,0)!=0 ) then ( case when (a.vid_real not in (7 , 9) ) then a.ym end ) end as ym_min_priz, /*Период начисления с (призн.)*//*number*/
 case when ( nvl( a.sum_v ,0)!=0 ) then ( case when (a.vid_real not in (7 , 9) ) then a.ym end ) end as ym_max_priz/*Период начисления по (призн.)*//*number*/
from ur_dogplat
a
--\ur_dogplat
)
ur_dogplat on ur_dogplat.kod_mat = kod_mat_pp.kod_mat--\ur_dogplat
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
kod_deb_sf on ur_dogplat.kod_deb_sf = kod_deb_sf.kod_deb_sf--\vv_all_deb_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_deb_sf.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--vr_peni_period
select a.kod_sf as kod_sf, /*с/ф*//*number*/
spr_time_ym.name as name/*Период*//*string*/
from vr_peni_period
a
--\vr_peni_period
left outer join
(
--spr_time_ym
select a.ym as ym, /*number*//*key*/
a.name as name/*Период*//*string*/
from (
--spr_time_ym_pre
select to_number(to_char( a.dat_day ,'YYYYMM'))/100 as ym, /*number*//*key*/
max( trim(to_char( ( to_number(to_char( a.dat_day ,'YYYYMM'))/100 ) ,'9999.99')) ) as name/*Период*//*string*/
from vv_day
a
--\vv_day
 group by
 to_number(to_char( a.dat_day ,'YYYYMM'))/100 /*number*//*key*/
order by ym)
a
--\spr_time_ym_pre
)
spr_time_ym on a.ym = spr_time_ym.ym--\spr_time_ym
)
vr_peni_period on kod_sf.kod_sf = vr_peni_period.kod_sf--\vr_peni_period
left outer join
(
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.ndog as ndog/*Номер договора*//*string*/
from kr_dogovor
a
--\kr_dogovor
)
kod_dog on kod_sf.kod_dog = kod_dog.kod_dog--\kr_dogovor
 group by
ur_isp.kod_isp, /*number*//*key*/
ur_dogplat.kod_dogplat, /*number*//*key*/
ur_dogplat.sum_v, /*Признано по юр. документу*//*number*//*key*/
ur_dogplat.vid_real, /*number*//*key*/
ur_dogplat.ym_min_priz, /*Период начисления с (призн.)*//*number*//*key*/
ur_dogplat.ym_max_priz/*Период начисления по (призн.)*//*number*//*key*/
)
a
--\52834-pre_dogplat_nach
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_dogplat_nach_g
delete from rr_temp where skod = '52834-pre_dogplat_opl_k_do_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2, --oplf
n3, --oplf_osn
n4, --oplf_peni
n5--oplf_gp
)
--52834-pre_dogplat_opl_k_do_g
--52834-pre_dogplat_opl_k_do_g
select '52834-pre_dogplat_opl_k_do_g' as skod,
'52834-pre_dogplat_opl_k_do_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.oplf) as oplf, /*Оплачено*//*number*/
sum( case when (a.vid_real = 2) then a.oplf end ) as oplf_osn, /*Оплачено*//*number*/
sum( case when (a.vid_real = 7) then a.oplf end ) as oplf_peni, /*Оплачено*//*number*/
sum( case when (a.vid_real = 9) then a.oplf end ) as oplf_gp/*Оплачено*//*number*/
from (
--52834-pre_dogplat_opl_k_do
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_opl.kod_opl as kod_opl, /*number*//*key*/
sr_opl.vid_real as vid_real, /*number*//*key*/
sr_opl.dat_opl as dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl.oplf as oplf/*Оплачено*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--sr_opl_bank
select a.kod_link as kod_link, /*number*//*key*/
a.kod_mat as kod_mat/*number*/
from sr_opl_bank
a
--\sr_opl_bank
)
sr_opl_bank on sr_opl_bank.kod_mat = kod_mat_pp.kod_mat--\sr_opl_bank
left outer join
(
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.vid_real as vid_real, /**//*number*/
a.kod_type_opl as kod_type_opl, /*number*/
a.kod_link as kod_link, /*number*/
a.dat_opl as dat_opl, /*Дата платежного документа*//*date*/
a.dat_uch as dat_uch, /**//*date*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as oplf/*Оплачено*//*number*/
from sr_opl
a
--\sr_opl
)
sr_opl on sr_opl.kod_link = sr_opl_bank.kod_link--\sr_opl
where
(sr_opl.kod_type_opl in (1 , 2 , 5 , 6) ) and (sr_opl.dat_uch < ( nvl( ur_isp.dat_doc , DATE'8999-12-31' ) ) ) group by
ur_isp.kod_isp, /*number*//*key*/
sr_opl.kod_opl, /*number*//*key*/
sr_opl.vid_real, /*number*//*key*/
sr_opl.dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl.oplf/*Оплачено*//*number*//*key*/
)
a
--\52834-pre_dogplat_opl_k_do
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_dogplat_opl_k_do_g
delete from rr_temp where skod = '52834-pre_dogplat_opl_k_posl_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2, --oplf
n3, --oplf_osn_before_not_done_dat
n4, --oplf_peni_before_not_done_dat
n5, --oplf_gp_before_not_done_dat
d1, --dat_opl
n6--oplf_before_not_done_dat
)
--52834-pre_dogplat_opl_k_posl_g
--52834-pre_dogplat_opl_k_posl_g
select '52834-pre_dogplat_opl_k_posl_g' as skod,
'52834-pre_dogplat_opl_k_posl_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.oplf) as oplf, /*Оплачено*//*number*/
sum( case when ((a.vid_real = 2)  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_osn_before_not_done_dat, /*Оплачено*//*number*/
sum( case when ((a.vid_real = 7)  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_peni_before_not_done_dat, /*Оплачено*//*number*/
sum( case when ((a.vid_real = 9)  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_gp_before_not_done_dat, /*Оплачено*//*number*/
max(a.dat_opl) as dat_opl, /*Дата платежного документа*//*date*/
sum( case when (( 1=1 )  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_before_not_done_dat/*Оплачено*//*number*/
from (
--52834-pre_dogplat_opl_k_posl
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_opl.kod_opl as kod_opl, /*number*//*key*/
sr_opl.vid_real as vid_real, /*number*//*key*/
sr_opl.dat_opl as dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl.oplf as oplf/*Оплачено*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--sr_opl_bank
select a.kod_link as kod_link, /*number*//*key*/
a.kod_mat as kod_mat/*number*/
from sr_opl_bank
a
--\sr_opl_bank
)
sr_opl_bank on sr_opl_bank.kod_mat = kod_mat_pp.kod_mat--\sr_opl_bank
left outer join
(
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.vid_real as vid_real, /**//*number*/
a.kod_type_opl as kod_type_opl, /*number*/
a.kod_link as kod_link, /*number*/
a.dat_opl as dat_opl, /*Дата платежного документа*//*date*/
a.dat_uch as dat_uch, /**//*date*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as oplf/*Оплачено*//*number*/
from sr_opl
a
--\sr_opl
)
sr_opl on sr_opl.kod_link = sr_opl_bank.kod_link--\sr_opl
where
(sr_opl.kod_type_opl in (1 , 2 , 5 , 6) ) and (sr_opl.dat_uch >= ( nvl( ur_isp.dat_doc , DATE'8999-12-31' ) ) ) group by
ur_isp.kod_isp, /*number*//*key*/
sr_opl.kod_opl, /*number*//*key*/
sr_opl.vid_real, /*number*//*key*/
sr_opl.dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl.oplf/*Оплачено*//*number*//*key*/
)
a
--\52834-pre_dogplat_opl_k_posl
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_dogplat_opl_k_posl_g
delete from rr_temp where skod = '52834-pre_dogplat_opl_sf_do_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2, --oplf
n3, --oplf_osn
n4, --oplf_peni
n5--oplf_gp
)
--52834-pre_dogplat_opl_sf_do_g
--52834-pre_dogplat_opl_sf_do_g
select '52834-pre_dogplat_opl_sf_do_g' as skod,
'52834-pre_dogplat_opl_sf_do_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.oplf) as oplf, /*Оплачено*//*number*/
sum( case when (a.vid_real = 2) then a.oplf end ) as oplf_osn, /*Оплачено*//*number*/
sum( case when (a.vid_real = 7) then a.oplf end ) as oplf_peni, /*Оплачено*//*number*/
sum( case when (a.vid_real = 9) then a.oplf end ) as oplf_gp/*Оплачено*//*number*/
from (
--52834-pre_dogplat_opl_sf_do
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_opl_sf.kod_opl as kod_opl, /*number*//*key*/
sr_opl_sf.vid_real as vid_real, /*number*//*key*/
sr_opl_sf.dat_opl as dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_sf.oplf as oplf/*Оплачено*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.dat_post as dat_post, /*Дата прин. суд. акта*//*date*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.dat_form as dat_form, /*Дата расч. задолж.*//*date*/
a.prizn_konv as prizn_konv, /**//*number*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf/*number*/
from ur_dogplat
a
--\ur_dogplat
)
ur_dogplat on ur_dogplat.kod_mat = kod_mat_pp.kod_mat--\ur_dogplat
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
kod_deb_sf on ur_dogplat.kod_deb_sf = kod_deb_sf.kod_deb_sf--\vv_all_deb_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf/*number*//*key*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_deb_sf.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--sr_opl_sf
select a.kod_opl as kod_opl, /*number*//*key*/
a.vid_real as vid_real, /**//*number*/
a.kod_sf as kod_sf, /*number*/
a.kod_type_opl as kod_type_opl, /*number*/
a.dat_opl as dat_opl, /*Дата платежного документа*//*date*/
a.dat_uch as dat_uch, /**//*date*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as oplf/*Оплачено*//*number*/
from sr_opl
a
--\sr_opl
where
a.kod_type_opl in (0 , 2 , 3 , 4) )
sr_opl_sf on sr_opl_sf.kod_sf = kod_sf.kod_sf--\sr_opl_sf
where
(sr_opl_sf.kod_type_opl in (0 , 2 , 3 , 4) ) and (sr_opl_sf.dat_uch < ( nvl( ur_isp.dat_doc , DATE'8999-12-31' ) ) ) and (sr_opl_sf.dat_uch >= kod_hist_mat_dec.dat_post) and ((ur_dogplat.prizn_konv = 1) or (sr_opl_sf.dat_uch >= ( nullif(least ( nvl( ( nvl( ur_dogplat.dat_form , DATE'8999-12-31' ) ) , to_date('01.01.9999','DD.MM.YYYY')) , nvl( ( nvl( kod_hist_mat_dec.dat_post , DATE'8999-12-31' ) ) , to_date('01.01.9999','DD.MM.YYYY')) ), to_date('01.01.9999','DD.MM.YYYY')) ) ) ) group by
ur_isp.kod_isp, /*number*//*key*/
sr_opl_sf.kod_opl, /*number*//*key*/
sr_opl_sf.vid_real, /*number*//*key*/
sr_opl_sf.dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_sf.oplf/*Оплачено*//*number*//*key*/
)
a
--\52834-pre_dogplat_opl_sf_do
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_dogplat_opl_sf_do_g
delete from rr_temp where skod = '52834-pre_dogplat_opl_sf_posl_g';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --kod_isp
n2, --oplf
n3, --oplf_osn_before_not_done_dat
n4, --oplf_peni_before_not_done_dat
n5, --oplf_gp_before_not_done_dat
d1, --dat_opl
n6--oplf_before_not_done_dat
)
--52834-pre_dogplat_opl_sf_posl_g
--52834-pre_dogplat_opl_sf_posl_g
select '52834-pre_dogplat_opl_sf_posl_g' as skod,
'52834-pre_dogplat_opl_sf_posl_g|#'||mtr.kod_isp as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.kod_isp as kod_isp, /*number*//*key*/
sum(a.oplf) as oplf, /*Оплачено*//*number*/
sum( case when ((a.vid_real = 2)  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_osn_before_not_done_dat, /*Оплачено*//*number*/
sum( case when ((a.vid_real = 7)  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_peni_before_not_done_dat, /*Оплачено*//*number*/
sum( case when ((a.vid_real = 9)  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_gp_before_not_done_dat, /*Оплачено*//*number*/
max(a.dat_opl) as dat_opl, /*Дата платежного документа*//*date*/
sum( case when (( 1=1 )  and (a.dat_opl < :not_done_dat )  ) then a.oplf end ) as oplf_before_not_done_dat/*Оплачено*//*number*/
from (
--52834-pre_dogplat_opl_sf_posl
select ur_isp.kod_isp as kod_isp, /*number*//*key*/
sr_opl_sf.kod_opl as kod_opl, /*number*//*key*/
sr_opl_sf.vid_real as vid_real, /*number*//*key*/
sr_opl_sf.dat_opl as dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_sf.oplf as oplf/*Оплачено*//*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.dat_post as dat_post, /*Дата прин. суд. акта*//*date*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
--ur_dogplat
select a.kod_dogplat as kod_dogplat, /*number*//*key*/
a.dat_form as dat_form, /*Дата расч. задолж.*//*date*/
a.prizn_konv as prizn_konv, /**//*number*/
a.kod_mat as kod_mat, /*number*/
a.kod_deb_sf as kod_deb_sf/*number*/
from ur_dogplat
a
--\ur_dogplat
)
ur_dogplat on ur_dogplat.kod_mat = kod_mat_pp.kod_mat--\ur_dogplat
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
kod_deb_sf on ur_dogplat.kod_deb_sf = kod_deb_sf.kod_deb_sf--\vv_all_deb_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf/*number*//*key*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_deb_sf.kod_sf = kod_sf.kod_sf--\sr_facvip
left outer join
(
--sr_opl_sf
select a.kod_opl as kod_opl, /*number*//*key*/
a.vid_real as vid_real, /**//*number*/
a.kod_sf as kod_sf, /*number*/
a.kod_type_opl as kod_type_opl, /*number*/
a.dat_opl as dat_opl, /*Дата платежного документа*//*date*/
a.dat_uch as dat_uch, /**//*date*/
 nvl( a.opl ,0) +nvl( a.opls ,0) as oplf/*Оплачено*//*number*/
from sr_opl
a
--\sr_opl
where
a.kod_type_opl in (0 , 2 , 3 , 4) )
sr_opl_sf on sr_opl_sf.kod_sf = kod_sf.kod_sf--\sr_opl_sf
where
(sr_opl_sf.kod_type_opl in (0 , 2 , 3 , 4) ) and (sr_opl_sf.dat_uch >= ( nvl( ur_isp.dat_doc , DATE'8999-12-31' ) ) ) and (sr_opl_sf.dat_uch >= kod_hist_mat_dec.dat_post) and ((ur_dogplat.prizn_konv = 1) or (sr_opl_sf.dat_uch >= ( nullif(least ( nvl( ( nvl( ur_dogplat.dat_form , DATE'8999-12-31' ) ) , to_date('01.01.9999','DD.MM.YYYY')) , nvl( ( nvl( kod_hist_mat_dec.dat_post , DATE'8999-12-31' ) ) , to_date('01.01.9999','DD.MM.YYYY')) ), to_date('01.01.9999','DD.MM.YYYY')) ) ) ) group by
ur_isp.kod_isp, /*number*//*key*/
sr_opl_sf.kod_opl, /*number*//*key*/
sr_opl_sf.vid_real, /*number*//*key*/
sr_opl_sf.dat_opl, /*Дата платежного документа*//*date*//*key*/
sr_opl_sf.oplf/*Оплачено*//*number*//*key*/
)
a
--\52834-pre_dogplat_opl_sf_posl
 group by
a.kod_isp/*number*//*key*/
) mtr
;
--\52834-pre_dogplat_opl_sf_posl_g
delete from rr_temp where skod = '52834-main';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
s1, --grsetname
n1, --kod_isp
d1, --dat_last_opl
n2, --dep
n3, --kodp
s2, --ab_dog
s3, --category
s4, --num_delo
n4, --debt_all_do
n5, --debt_osn_do
n6, --debt_peni_do
n7, --debt_gp_do
d2, --dat_vid
d3, --dat_post_upr
d4, --dat_post
n8, --debt_all
n9, --debt_all_before_not_done_dat
n10, --debt_osn_before_not_done_dat
n11, --debt_peni_before_not_done_dat
n12, --debt_gp_before_not_done_dat
n13, --ym_nach_s
n14, --ym_nach_po
s5, --peni_per
s6, --other_work_prim
s7, --dep_sname
s8, --dep_name
d5, --min_dat_vid
n15, --last_month
n16, --gr_rn
s9, --growid
s10, --parent_growid
n17, --groupingid
s11, --grsetid
s12, --origgrsetid
s13, --parent_grsetid
n18--par_groupingid
)
--52834-main
with
mat1 as
(
--
select /*+ materialize*/
grpd.kod_isp as kod_isp, /*Код ИЛ*//*number*//*key*/
grpd.dat_last_opl as dat_last_opl, /*Дата последней оплаты*//*date*//*key*/
grpd.dep as dep, /*Код подразделения*//*number*//*key*/
grpd.kodp as kodp, /*Код абонента*//*number*//*key*/
grpd.ab_dog as ab_dog, /*Абонент и договор*//*string*//*key*/
grpd.category as category, /*string*/
grpd.num_delo as num_delo, /*Номер дела*//*string*//*key*/
grpd.debt_all_do as debt_all_do, /*Долг до выдачи ИЛ*//*number*//*key*/
grpd.debt_osn_do as debt_osn_do, /*Долг осн до выдачи ИЛ*//*number*//*key*/
grpd.debt_peni_do as debt_peni_do, /*Долг пени до выдачи ИЛ*//*number*//*key*/
grpd.debt_gp_do as debt_gp_do, /*Долг ГП до выдачи ИЛ*//*number*//*key*/
grpd.dat_vid as dat_vid, /*Дата выдачи исп. листа*//*date*//*key*/
grpd.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в Упр.*//*date*//*key*/
grpd.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*//*key*/
grpd.debt_all as debt_all, /*Долг*//*number*//*key*/
grpd.debt_all_before_not_done_dat as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
grpd.debt_osn_before_not_done_dat as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
grpd.debt_peni_before_not_done_dat as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
grpd.debt_gp_before_not_done_dat as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
grpd.ym_nach_s as ym_nach_s, /*Период начисления с*//*number*//*key*/
grpd.ym_nach_po as ym_nach_po, /*Период начисления по*//*number*//*key*/
grpd.peni_per as peni_per, /*Период пени*//*string*//*key*/
grpd.other_work_prim as other_work_prim, /*Примечание в других работах*//*string*/
grpd.dep_sname as dep_sname, /*Подразделение*//*string*//*key*/
grpd.dep_name as dep_name, /*Подразделение*//*string*//*key*/
grpd.min_dat_vid as min_dat_vid, /*Дата выдачи исп. листа*//*date*/
grpd.last_month as last_month/*number*/
from (
--
select a.kod_isp as kod_isp, /*Код ИЛ*//*number*//*key*/
a.dat_last_opl as dat_last_opl, /*Дата последней оплаты*//*date*//*key*/
a.dep as dep, /*Код подразделения*//*number*//*key*/
a.kodp as kodp, /*Код абонента*//*number*//*key*/
a.ab_dog as ab_dog, /*Абонент и договор*//*string*//*key*/
cat.abbr as category, /*string*/
a.num_delo as num_delo, /*Номер дела*//*string*//*key*/
a.debt_all_do as debt_all_do, /*Долг до выдачи ИЛ*//*number*//*key*/
a.debt_osn_do as debt_osn_do, /*Долг осн до выдачи ИЛ*//*number*//*key*/
a.debt_peni_do as debt_peni_do, /*Долг пени до выдачи ИЛ*//*number*//*key*/
a.debt_gp_do as debt_gp_do, /*Долг ГП до выдачи ИЛ*//*number*//*key*/
a.dat_vid as dat_vid, /*Дата выдачи исп. листа*//*date*//*key*/
a.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в Упр.*//*date*//*key*/
a.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*//*key*/
a.debt_all as debt_all, /*Долг*//*number*//*key*/
 case when (( 0=1 )  or ((( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl >= :not_done_dat ) ) or ( nvl( a.debt_all ,0)!=0 ) )  ) then a.debt_all_before_not_done_dat end as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
 case when (( 0=1 )  or ((( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl >= :not_done_dat ) ) or ( nvl( a.debt_all ,0)!=0 ) )  ) then a.debt_osn_before_not_done_dat end as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
 case when (( 0=1 )  or ((( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl >= :not_done_dat ) ) or ( nvl( a.debt_all ,0)!=0 ) )  ) then a.debt_peni_before_not_done_dat end as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
 case when (( 0=1 )  or ((( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl >= :not_done_dat ) ) or ( nvl( a.debt_all ,0)!=0 ) )  ) then a.debt_gp_before_not_done_dat end as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
a.ym_nach_s as ym_nach_s, /*Период начисления с*//*number*//*key*/
a.ym_nach_po as ym_nach_po, /*Период начисления по*//*number*//*key*/
a.peni_per as peni_per, /*Период пени*//*string*//*key*/
a.other_work_prim as other_work_prim, /*Примечание в других работах*//*string*/
a.dep_sname as dep_sname, /*Подразделение*//*string*//*key*/
a.dep_name as dep_name, /*Подразделение*//*string*//*key*/
 min( a.dat_vid ) over( partition by a.dep , a.kodp order by 1 ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING ) as min_dat_vid, /*Дата выдачи исп. листа*//*date*/
 case when ((( to_number(to_char( a.dat_vid ,'YYYYMM'))/100 ) = ( to_number(to_char( ( nvl( /*nvlu*/ :vidan_dat2 , ( null ) ) ) ,'YYYYMM'))/100 ) ) or (( to_number(to_char( a.dat_post ,'YYYYMM'))/100 ) = ( to_number(to_char( ( nvl( /*nvlu*/ :postup_dat2 , ( null ) ) ) ,'YYYYMM'))/100 ) ) ) then 1 else 0 end as last_month/*number*/
from (
--52834-pre
select ovr1.kod_isp as kod_isp, /*Код ИЛ*//*number*/
ovr1.is_active as is_active, /*Статус папки*//*number*/
ovr1.kod_category as kod_category, /*Категория потребителя*//*number*/
ovr1.dat_last_opl as dat_last_opl, /*Дата платежного документа*//*date*/
ovr1.dep as dep, /*Код подразделения*//*number*/
ovr1.kodp as kodp, /*Код абонента*//*number*/
ovr1.ab_dog as ab_dog, /*Наименование абонента*//*string*/
ovr1.num_delo as num_delo, /*Номер дела*//*string*/
ovr1.debt_all_do as debt_all_do, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_osn_do as debt_osn_do, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_peni_do as debt_peni_do, /*Признано по юр. документу*//*number*/
ovr1.debt_gp_do as debt_gp_do, /*Признано по юр. документу*//*number*/
ovr1.dat_vid as dat_vid, /*Дата выдачи исп. листа*//*date*/
ovr1.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в управление*//*date*/
ovr1.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*/
ovr1.debt_all as debt_all, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_all_before_not_done_dat as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_osn_before_not_done_dat as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_peni_before_not_done_dat as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
ovr1.debt_gp_before_not_done_dat as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
ovr1.ym_nach_s as ym_nach_s, /*Период начисления с (призн.)*//*number*/
ovr1.ym_nach_po as ym_nach_po, /*Период начисления по (призн.)*//*number*/
ovr1.peni_per as peni_per, /*Период*//*string*/
max(ovr1.other_work_prim) as other_work_prim, /*string*/
ovr1.dep_sname as dep_sname, /*Аббревиатура*//*string*/
ovr1.dep_name as dep_name/*Отделение*//*string*/
from (
--
select ur_isp.kod_isp as kod_isp, /*Код ИЛ*//*number*//*key*/
ur_folders_isp.is_active as is_active, /*Статус папки*//*number*//*key*/
ur_folders_isp.kod_category as kod_category, /*Категория потребителя*//*number*//*key*/
 greatest( ( coalesce( astr_opl_posl.dat_opl , do_opl_k_posl.dat_opl , do_opl_sf_posl.dat_opl ) ) , ( coalesce( do_opl_k_posl.dat_opl , do_opl_sf_posl.dat_opl , astr_opl_posl.dat_opl ) ) , ( coalesce( do_opl_sf_posl.dat_opl , astr_opl_posl.dat_opl , do_opl_k_posl.dat_opl ) ) ) as dat_last_opl, /*Дата платежного документа*//*date*//*key*/
kod_dep.kodp as dep, /*Код подразделения*//*number*//*key*/
kodp.kodp as kodp, /*Код абонента*//*number*//*key*/
kodp.name || ( chr( 13 ) ) || ( chr( 10 ) ) || 'ИНН ' || kodp.inn || ( chr( 13 ) ) || ( chr( 10 ) ) || '(' || dp_nach.ndog || ')' as ab_dog, /*Наименование абонента*//*string*//*key*/
kod_mat_pp.num_delo as num_delo, /*Номер дела*//*string*//*key*/
( nvl( kod_hist_mat_dec.ogr ,0) +nvl( kod_hist_mat_dec.sud_izd ,0) +nvl( astr_nach.nachisl_astr ,0) +nvl( dp_nach.sum_v ,0) ) - ( nvl( astr_opl_do.oplf ,0) +nvl( do_opl_k_do.oplf ,0) +nvl( do_opl_sf_do.oplf ,0) ) as debt_all_do, /*Расходы за введение ограничения режима энергопотребления*//*number*//*key*/
( nvl( kod_hist_mat_dec.ogr ,0) +nvl( dp_nach.sum_v_osn ,0) ) - ( nvl( do_opl_k_do.oplf_osn ,0) +nvl( do_opl_sf_do.oplf_osn ,0) ) as debt_osn_do, /*Расходы за введение ограничения режима энергопотребления*//*number*//*key*/
( nvl( dp_nach.sum_v_peni ,0) ) - ( nvl( do_opl_k_do.oplf_peni ,0) +nvl( do_opl_sf_do.oplf_peni ,0) ) as debt_peni_do, /*Признано по юр. документу*//*number*//*key*/
( nvl( dp_nach.sum_v_gp ,0) ) - ( nvl( do_opl_k_do.oplf_gp ,0) +nvl( do_opl_sf_do.oplf_gp ,0) ) as debt_gp_do, /*Признано по юр. документу*//*number*//*key*/
ur_isp.dat_doc as dat_vid, /*Дата выдачи исп. листа*//*date*//*key*/
ur_isp.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в управление*//*date*//*key*/
ur_isp.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*//*key*/
( nvl( kod_hist_mat_dec.ogr ,0) +nvl( kod_hist_mat_dec.sud_izd ,0) +nvl( astr_nach.nachisl_astr ,0) +nvl( dp_nach.sum_v ,0) ) - ( nvl( astr_opl_do.oplf ,0) +nvl( astr_opl_posl.oplf ,0) +nvl( do_opl_k_do.oplf ,0) +nvl( do_opl_k_posl.oplf ,0) +nvl( do_opl_sf_do.oplf ,0) +nvl( do_opl_sf_posl.oplf ,0) +nvl( kod_hist_mat_dec.opl_ogr ,0) +nvl( kod_hist_mat_dec.opl_sud_izd ,0) ) as debt_all, /*Расходы за введение ограничения режима энергопотребления*//*number*//*key*/
( nvl( kod_hist_mat_dec.ogr ,0) +nvl( kod_hist_mat_dec.sud_izd ,0) +nvl( astr_nach.nachisl_astr ,0) +nvl( dp_nach.sum_v ,0) ) - ( nvl( astr_opl_do.oplf ,0) +nvl( astr_opl_posl.oplf_before_not_done_dat ,0) +nvl( do_opl_k_do.oplf ,0) +nvl( do_opl_k_posl.oplf_before_not_done_dat ,0) +nvl( do_opl_sf_do.oplf ,0) +nvl( do_opl_sf_posl.oplf_before_not_done_dat ,0) +nvl( ( case when (( 1=1 )  and (( nvl( kod_hist_mat_dec.opl_ogr_dat , DATE'8999-12-31' ) ) < :not_done_dat )  ) then kod_hist_mat_dec.opl_ogr end ) ,0) +nvl( ( case when (( 1=1 )  and (( nvl( kod_hist_mat_dec.opl_sud_izd_dat , DATE'8999-12-31' ) ) < :not_done_dat )  ) then kod_hist_mat_dec.opl_sud_izd end ) ,0) ) as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*//*key*/
( nvl( kod_hist_mat_dec.ogr ,0) +nvl( dp_nach.sum_v_osn ,0) ) - ( nvl( do_opl_k_do.oplf_osn ,0) +nvl( do_opl_k_posl.oplf_osn_before_not_done_dat ,0) +nvl( do_opl_sf_do.oplf_osn ,0) +nvl( do_opl_sf_posl.oplf_osn_before_not_done_dat ,0) +nvl( ( case when (( 1=1 )  and (( nvl( kod_hist_mat_dec.opl_ogr_dat , DATE'8999-12-31' ) ) < :not_done_dat )  ) then kod_hist_mat_dec.opl_ogr end ) ,0) ) as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*//*key*/
( nvl( dp_nach.sum_v_peni ,0) ) - ( nvl( do_opl_k_do.oplf_peni ,0) +nvl( do_opl_k_posl.oplf_peni_before_not_done_dat ,0) +nvl( do_opl_sf_do.oplf_peni ,0) +nvl( do_opl_sf_posl.oplf_peni_before_not_done_dat ,0) ) as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*//*key*/
( nvl( dp_nach.sum_v_gp ,0) ) - ( nvl( do_opl_k_do.oplf_gp ,0) +nvl( do_opl_k_posl.oplf_gp_before_not_done_dat ,0) +nvl( do_opl_sf_do.oplf_gp ,0) +nvl( do_opl_sf_posl.oplf_gp_before_not_done_dat ,0) ) as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*//*key*/
dp_nach.ym_min_priz as ym_nach_s, /*Период начисления с (призн.)*//*number*//*key*/
dp_nach.ym_max_priz as ym_nach_po, /*Период начисления по (призн.)*//*number*//*key*/
 vg_period.ym_enum_str_to_ym_ranges_str( dp_nach.peni_per ) as peni_per, /*Период*//*string*//*key*/
 stragg_dist( ur_isp_other_prim.prim ) over( partition by ur_isp.kod_isp order by ur_isp_other_prim.kod_isp_other_prim ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING ) as other_work_prim, /*string*/
kod_dep.sname as dep_sname, /*Аббревиатура*//*string*//*key*/
kod_dep.name as dep_name/*Отделение*//*string*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.dat_doc as dat_doc, /*Дата выдачи исп. листа*//*date*/
a.kod_folders as kod_folders, /*number*/
a.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*/
a.kod_hist_mat as kod_hist_mat, /*number*/
a.dat_post_upr as dat_post_upr/*Дата поступления исп.листа в управление*//*date*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_folders_isp
select a.kod_folders as kod_folders, /*number*//*key*/
a.kod_isp as kod_isp, /*number*/
a.kod_category as kod_category, /*Категория потребителя*//*number*/
 decode ( a.dat_finish ,null,1,0) as is_active, /*Статус папки*//*number*/
a.kod_folders as kod_folders_isp/*number*//*key*/
from ur_folders
a
--\ur_folders
where
a.kod_sdp = 2)
ur_folders_isp on ur_folders_isp.kod_isp = ur_isp.kod_isp--\ur_folders_isp
left outer join
(
--ur_isp_other
select a.kod_isp_other as kod_isp_other, /*number*//*key*/
a.kod_folders as kod_folders/*number*/
from ur_isp_other
a
--\ur_isp_other
)
ur_isp_other_isp on ur_isp_other_isp.kod_folders = ur_folders_isp.kod_folders--\ur_isp_other
left outer join
(
--ur_isp_other_prim
select a.kod_isp_other_prim as kod_isp_other_prim, /*number*//*key*/
a.kod_isp_other as kod_isp_other, /*number*/
a.prim as prim/*string*/
from ur_isp_other_prim
a
--\ur_isp_other_prim
)
ur_isp_other_prim on ur_isp_other_prim.kod_isp_other = ur_isp_other_isp.kod_isp_other--\ur_isp_other_prim
left outer join
(
--ur_folders
select a.kod_folders as kod_folders, /*number*//*key*/
a.kodp as kodp, /**//*number*/
a.kod_podr as kod_podr/*number*/
from ur_folders
a
--\ur_folders
)
kod_folders on ur_isp.kod_folders = kod_folders.kod_folders--\ur_folders
left outer join
(
--kr_payer
select a.kodp as kodp, /**//*number*//*key*/
a.name as name, /*Наименование абонента*//*string*/
a.inn as inn, /*ИНН*//*string*/
a.ogrn as ogrn/*ОГРН*//*string*/
from kr_payer
a
--\kr_payer
)
kodp on kod_folders.kodp = kodp.kodp--\kr_payer
left outer join
(
--kr_dep
select a.kodp as kodp, /*number*//*key*/
kodp.name as name, /*Отделение*//*string*/
kodp.sname as sname/*Аббревиатура*//*string*/
from (
 SELECT LEVEL lvl, kodp
 FROM kr_org a
 WHERE kod_ecls = 4
 AND kod_separator = 2
 AND kodp IN
 (SELECT a.kodp
 FROM kr_org a, kr_org b
 WHERE a.kod_ecls = 4
 AND kg_common.org_area (a.kodp, b.kodp) = 1
 AND b.kod_ecls = 4
 AND kg_common.user_podr_rights (1, b.kodp) > 0)
 CONNECT BY PRIOR kodp = kod_parent
 START WITH kod_parent IS NULL AND kod_ecls = 4 AND kod_separator <= 2
 ORDER SIBLINGS BY kg.cut_num (name), name
 )a
--\kr_dep
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name, /*Отделение*//*string*/
a.sname as sname/**//*string*/
from kr_org
a
--\kr_org
)
kodp on a.kodp = kodp.kodp--\kr_org
)
kod_dep on kod_folders.kod_podr = kod_dep.kodp--\kr_dep
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.sud_izd as sud_izd, /*Судебные издержки*//*number*/
a.ogr as ogr, /*Расходы за введение ограничения режима энергопотребления*//*number*/
a.opl_ogr as opl_ogr, /*Оплачено: расходы за введение ограничения режима энергопотребления*//*number*/
a.opl_sud_izd as opl_sud_izd, /*Оплачено: Судебные издержки*//*number*/
a.opl_ogr_dat as opl_ogr_dat, /**//*date*/
a.opl_sud_izd_dat as opl_sud_izd_dat, /**//*date*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
left outer join
(
--ur_mat_pp
select a.kod_mat as kod_mat, /*number*//*key*/
a.num_delo as num_delo, /*Номер дела*//*string*/
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as nachisl_astr
from rr_temp where skod = '52834-pre_astr_nach_g'
)
astr_nach on ur_isp.kod_isp = astr_nach.kod_isp--\52834-pre_astr_nach_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as oplf
from rr_temp where skod = '52834-pre_astr_opl_do_g'
)
astr_opl_do on ur_isp.kod_isp = astr_opl_do.kod_isp--\52834-pre_astr_opl_do_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
d1 as dat_opl,
n2 as oplf,
n3 as oplf_before_not_done_dat
from rr_temp where skod = '52834-pre_astr_opl_posl_g'
)
astr_opl_posl on ur_isp.kod_isp = astr_opl_posl.kod_isp--\52834-pre_astr_opl_posl_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as sum_v,
n3 as sum_v_osn,
n4 as sum_v_peni,
n5 as sum_v_gp,
s1 as ndog,
n6 as ym_min_priz,
n7 as ym_max_priz,
s2 as peni_per
from rr_temp where skod = '52834-pre_dogplat_nach_g'
)
dp_nach on ur_isp.kod_isp = dp_nach.kod_isp--\52834-pre_dogplat_nach_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as oplf,
n3 as oplf_osn,
n4 as oplf_peni,
n5 as oplf_gp
from rr_temp where skod = '52834-pre_dogplat_opl_k_do_g'
)
do_opl_k_do on ur_isp.kod_isp = do_opl_k_do.kod_isp--\52834-pre_dogplat_opl_k_do_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as oplf,
n3 as oplf_osn_before_not_done_dat,
n4 as oplf_peni_before_not_done_dat,
n5 as oplf_gp_before_not_done_dat,
d1 as dat_opl,
n6 as oplf_before_not_done_dat
from rr_temp where skod = '52834-pre_dogplat_opl_k_posl_g'
)
do_opl_k_posl on ur_isp.kod_isp = do_opl_k_posl.kod_isp--\52834-pre_dogplat_opl_k_posl_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as oplf,
n3 as oplf_osn,
n4 as oplf_peni,
n5 as oplf_gp
from rr_temp where skod = '52834-pre_dogplat_opl_sf_do_g'
)
do_opl_sf_do on ur_isp.kod_isp = do_opl_sf_do.kod_isp--\52834-pre_dogplat_opl_sf_do_g
left outer join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n1 as kod_isp,
n2 as oplf,
n3 as oplf_osn_before_not_done_dat,
n4 as oplf_peni_before_not_done_dat,
n5 as oplf_gp_before_not_done_dat,
d1 as dat_opl,
n6 as oplf_before_not_done_dat
from rr_temp where skod = '52834-pre_dogplat_opl_sf_posl_g'
)
do_opl_sf_posl on ur_isp.kod_isp = do_opl_sf_posl.kod_isp--\52834-pre_dogplat_opl_sf_posl_g
where
((:pr_bankrupt = 0) or ((:pr_bankrupt = 1) and (kodp.ogrn in (
--
select a.ogrn as ogrn/*ОГРН*//*string*//*key*/
from (
--kr_egr_ul
select a.ogrn as ogrn, /*ОГРН*//*string*//*key*/
a.sulst as sulst/**//*string*/
from kr_egr_ul
a
--\kr_egr_ul
)
a
--\kr_egr_ul
where
a.sulst in (114 , 115 , 116 , 117) )
--\
) ) or ((:pr_bankrupt = 2) and ( not exists (
--
select 1
from (
--kr_egr_ul
select a.ogrn as ogrn, /*ОГРН*//*string*//*key*/
a.sulst as sulst/**//*string*/
from kr_egr_ul
a
--\kr_egr_ul
)
a
--\kr_egr_ul
where
(a.ogrn = kodp.ogrn) and (a.sulst in (114 , 115 , 116 , 117) ) )
--\
) ) ) and (dp_nach.ndog not like '%-51 Э') and (dp_nach.ndog not like '%-51 Э-ОДН') )
ovr1
--\
 group by
ovr1.kod_isp, /*Код ИЛ*//*number*/
ovr1.is_active, /*Статус папки*//*number*/
ovr1.kod_category, /*Категория потребителя*//*number*/
ovr1.dat_last_opl, /*Дата платежного документа*//*date*/
ovr1.dep, /*Код подразделения*//*number*/
ovr1.kodp, /*Код абонента*//*number*/
ovr1.ab_dog, /*Наименование абонента*//*string*/
ovr1.num_delo, /*Номер дела*//*string*/
ovr1.debt_all_do, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_osn_do, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_peni_do, /*Признано по юр. документу*//*number*/
ovr1.debt_gp_do, /*Признано по юр. документу*//*number*/
ovr1.dat_vid, /*Дата выдачи исп. листа*//*date*/
ovr1.dat_post_upr, /*Дата поступления исп.листа в управление*//*date*/
ovr1.dat_post, /*Дата поступления исп.листа в отделение*//*date*/
ovr1.debt_all, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
ovr1.debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
ovr1.debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
ovr1.ym_nach_s, /*Период начисления с (призн.)*//*number*/
ovr1.ym_nach_po, /*Период начисления по (призн.)*//*number*/
ovr1.peni_per, /*Период*//*string*/
ovr1.dep_sname, /*Аббревиатура*//*string*/
ovr1.dep_name/*Отделение*//*string*/
)
a
--\52834-pre
inner join
(
--52834-pre_il_gr_cust
select ur_isp.kod_isp as kod_isp/*number*//*key*/
from (
--ur_isp
select a.kod_isp as kod_isp, /*number*//*key*/
a.kod_hist_mat as kod_hist_mat/*number*/
from ur_isp
a
--\ur_isp
)
ur_isp
--\ur_isp
left outer join
(
--ur_hist_mat_dec
select a.kod_hist_mat as kod_hist_mat, /*number*//*key*/
a.kod_mat as kod_mat, /*number*/
a.kod_hist_mat as kod_hist_mat_desc/*number*//*key*/
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
kod_hist_mat_dec on ur_isp.kod_hist_mat = kod_hist_mat_dec.kod_hist_mat--\ur_hist_mat_dec
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
kod_mat_pp on kod_hist_mat_dec.kod_mat = kod_mat_pp.kod_mat--\ur_mat_pp
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
ur_dogplat on ur_dogplat.kod_mat = kod_mat_pp.kod_mat--\ur_dogplat
left outer join
(
--vv_all_deb_sf
select a.kod_deb_sf as kod_deb_sf, /*number*//*key*/
a.kod_sf as kod_sf/*number*/
from vv_all_deb_sf
a
--\vv_all_deb_sf
)
kod_deb_sf on ur_dogplat.kod_deb_sf = kod_deb_sf.kod_deb_sf--\vv_all_deb_sf
left outer join
(
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
kod_sf on kod_deb_sf.kod_sf = kod_sf.kod_sf--\sr_facvip
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
--kr_dogovor_dop
select a.kod_dog as kod_dog, /*number*//*key*/
a.kod_group_cust as kod_group_cust/*number*/
from kr_dogovor_dop
a
--\kr_dogovor_dop
)
kod_dog_dop on kod_dog.kod_dog = kod_dog_dop.kod_dog--\kr_dogovor_dop
left outer join
(
--ks_group_cust
select a.kod_group_cust as kod_group_cust/*number*//*key*/
from kv_group_cust
a
--\kv_group_cust
)
kod_group_cust on kod_dog_dop.kod_group_cust = kod_group_cust.kod_group_cust--\ks_group_cust
where
( 1=1 )  and ((
--
select a.kod_group_cust as kod_group_cust/*number*//*key*/
from (
--ks_group_cust
select a.kod_group_cust as kod_group_cust, /*number*//*key*/
a.kod_gr_parent as kod_gr_parent/*number*/
from kv_group_cust
a
--\kv_group_cust
)
a
--\ks_group_cust
where
a.kod_gr_parent is null connect by nocycle
prior a.kod_gr_parent = a.kod_group_cust
start with a.kod_group_cust = kod_group_cust.kod_group_cust)
--\
 in :kod_group_cust_parent )   and (kod_group_cust.kod_group_cust in :kod_group_cust )  group by
ur_isp.kod_isp/*number*//*key*/
)
b on a.kod_isp = b.kod_isp--\52834-pre_il_gr_cust
left outer join
(
--us_category
select а.kod_category as kod_category, /*number*//*key*/
а.abbr as abbr/*string*/
from us_category
а
--\us_category
)
cat on a.kod_category = cat.kod_category--\us_category
where
( 1=1 ) and (( 0=1 )  or ((( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl >= :not_done_dat ) ) or ( nvl( a.debt_all ,0)!=0 ) )  or ((( 0=1 )  or (:done_dat1 is not null )   or (:done_dat2 is not null )  )  and (( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl >= :done_dat1 ) )   and (( nvl( a.debt_all ,0)=0 ) and (a.dat_last_opl <= :done_dat2 ) )  ) )  and (a.dep in :dep )   and (a.is_active in :prizn_arch_folder )   and (a.dat_vid >= :vidan_dat1 )   and (a.dat_vid <= :vidan_dat2 )   and (( nvl( a.dat_post_upr , a.dat_post ) ) >= :postup_dat1 )   and (( nvl( a.dat_post_upr , a.dat_post ) ) <= :postup_dat2 )  )
grpd
--\
)
--\
--52834-main
select '52834-main' as skod,
'52834-main|#'||mtr.growid as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select decode( p1.groupingid , '0' , '' , '262136' , '' , '262143' , '' ) as grsetname, /*string*/
p1.kod_isp as kod_isp, /*Код ИЛ*//*number*/
p1.dat_last_opl as dat_last_opl, /*Дата последней оплаты*//*date*/
p1.dep as dep, /*Код подразделения*//*number*/
p1.kodp as kodp, /*Код абонента*//*number*/
p1.ab_dog as ab_dog, /*Абонент и договор*//*string*/
p1.category as category, /*string*/
p1.num_delo as num_delo, /*Номер дела*//*string*/
p1.debt_all_do as debt_all_do, /*Долг до выдачи ИЛ*//*number*/
p1.debt_osn_do as debt_osn_do, /*Долг осн до выдачи ИЛ*//*number*/
p1.debt_peni_do as debt_peni_do, /*Долг пени до выдачи ИЛ*//*number*/
p1.debt_gp_do as debt_gp_do, /*Долг ГП до выдачи ИЛ*//*number*/
p1.dat_vid as dat_vid, /*Дата выдачи исп. листа*//*date*/
p1.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в Упр.*//*date*/
p1.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*/
p1.debt_all as debt_all, /*Долг*//*number*/
p1.debt_all_before_not_done_dat as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
p1.debt_osn_before_not_done_dat as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
p1.debt_peni_before_not_done_dat as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
p1.debt_gp_before_not_done_dat as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
p1.ym_nach_s as ym_nach_s, /*Период начисления с*//*number*/
p1.ym_nach_po as ym_nach_po, /*Период начисления по*//*number*/
p1.peni_per as peni_per, /*Период пени*//*string*/
p1.other_work_prim as other_work_prim, /*Примечание в других работах*//*string*/
p1.dep_sname as dep_sname, /*Подразделение*//*string*/
p1.dep_name as dep_name, /*Подразделение*//*string*/
p1.min_dat_vid as min_dat_vid, /*Дата выдачи исп. листа*//*date*/
p1.last_month as last_month, /*number*/
 row_number() over( order by null ) as gr_rn, /*number*/
'#' || ( case when (0 = ( bitand( 1 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dep_name ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 2 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dep_sname ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 4 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dep ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 8 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.last_month ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 16 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.other_work_prim ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 32 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.peni_per ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 64 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.ym_nach_po ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 128 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.ym_nach_s ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 256 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_post ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 512 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_post_upr ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 1024 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_vid ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 2048 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.num_delo ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 4096 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.category ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 8192 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.ab_dog ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 16384 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.kodp ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 32768 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.min_dat_vid ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 65536 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_last_opl ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 131072 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.kod_isp ) ) , ' ' ) ) end ) || '#' as growid, /**//*string*//*key*/
 case when (p1.par_groupingid is null ) then null else ('#' || ( case when (0 = ( bitand( 1 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dep_name ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 2 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dep_sname ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 4 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dep ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 8 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.last_month ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 16 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.other_work_prim ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 32 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.peni_per ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 64 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.ym_nach_po ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 128 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.ym_nach_s ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 256 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_post ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 512 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_post_upr ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 1024 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_vid ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 2048 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.num_delo ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 4096 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.category ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 8192 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.ab_dog ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 16384 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.kodp ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 32768 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.min_dat_vid ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 65536 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.dat_last_opl ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 131072 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.kod_isp ) ) , ' ' ) ) end ) || '#') end as parent_growid, /**//*string*/
p1.groupingid as groupingid, /*Код ИЛ*//*number*/
 decode( p1.groupingid , '0' , 'isp' , '262136' , 'dep' , '262143' , 'itog' ) as grsetid, /**//*string*/
 decode( p1.groupingid , '0' , 'isp' , '262136' , 'dep' , '262143' , 'itog' ) as origgrsetid, /**//*string*/
 decode( p1.groupingid , '0' , 'dep' ) as parent_grsetid, /**//*string*/
p1.par_groupingid as par_groupingid/**//*number*/
from (
--
select grsets_query.kod_isp as kod_isp, /*Код ИЛ*//*number*//*key*/
grsets_query.dat_last_opl as dat_last_opl, /*Дата последней оплаты*//*date*//*key*/
grsets_query.dep as dep, /*Код подразделения*//*number*//*key*/
grsets_query.kodp as kodp, /*Код абонента*//*number*//*key*/
grsets_query.ab_dog as ab_dog, /*Абонент и договор*//*string*//*key*/
grsets_query.category as category, /*string*/
grsets_query.num_delo as num_delo, /*Номер дела*//*string*//*key*/
grsets_query.debt_all_do as debt_all_do, /*Долг до выдачи ИЛ*//*number*//*key*/
grsets_query.debt_osn_do as debt_osn_do, /*Долг осн до выдачи ИЛ*//*number*//*key*/
grsets_query.debt_peni_do as debt_peni_do, /*Долг пени до выдачи ИЛ*//*number*//*key*/
grsets_query.debt_gp_do as debt_gp_do, /*Долг ГП до выдачи ИЛ*//*number*//*key*/
grsets_query.dat_vid as dat_vid, /*Дата выдачи исп. листа*//*date*//*key*/
grsets_query.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в Упр.*//*date*//*key*/
grsets_query.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*//*key*/
grsets_query.debt_all as debt_all, /*Долг*//*number*//*key*/
grsets_query.debt_all_before_not_done_dat as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
grsets_query.debt_osn_before_not_done_dat as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
grsets_query.debt_peni_before_not_done_dat as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
grsets_query.debt_gp_before_not_done_dat as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
grsets_query.ym_nach_s as ym_nach_s, /*Период начисления с*//*number*//*key*/
grsets_query.ym_nach_po as ym_nach_po, /*Период начисления по*//*number*//*key*/
grsets_query.peni_per as peni_per, /*Период пени*//*string*//*key*/
grsets_query.other_work_prim as other_work_prim, /*Примечание в других работах*//*string*/
grsets_query.dep_sname as dep_sname, /*Подразделение*//*string*//*key*/
grsets_query.dep_name as dep_name, /*Подразделение*//*string*//*key*/
grsets_query.min_dat_vid as min_dat_vid, /*Дата выдачи исп. листа*//*date*/
grsets_query.last_month as last_month, /*number*/
grsets_query.groupingid as groupingid, /*Код ИЛ*//*number*/
 decode( grsets_query.groupingid , '0' , '262136' ) as par_groupingid/**//*number*/
from (
--
select grpd.kod_isp as kod_isp, /*Код ИЛ*//*number*//*key*/
grpd.dat_last_opl as dat_last_opl, /*Дата последней оплаты*//*date*//*key*/
grpd.dep as dep, /*Код подразделения*//*number*//*key*/
grpd.kodp as kodp, /*Код абонента*//*number*//*key*/
grpd.ab_dog as ab_dog, /*Абонент и договор*//*string*//*key*/
grpd.category as category, /*string*/
grpd.num_delo as num_delo, /*Номер дела*//*string*//*key*/
sum(grpd.debt_all_do) as debt_all_do, /*Долг до выдачи ИЛ*//*number*//*key*/
sum(grpd.debt_osn_do) as debt_osn_do, /*Долг осн до выдачи ИЛ*//*number*//*key*/
sum(grpd.debt_peni_do) as debt_peni_do, /*Долг пени до выдачи ИЛ*//*number*//*key*/
sum(grpd.debt_gp_do) as debt_gp_do, /*Долг ГП до выдачи ИЛ*//*number*//*key*/
grpd.dat_vid as dat_vid, /*Дата выдачи исп. листа*//*date*//*key*/
grpd.dat_post_upr as dat_post_upr, /*Дата поступления исп.листа в Упр.*//*date*//*key*/
grpd.dat_post as dat_post, /*Дата поступления исп.листа в отделение*//*date*//*key*/
sum(grpd.debt_all) as debt_all, /*Долг*//*number*//*key*/
sum(grpd.debt_all_before_not_done_dat) as debt_all_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
sum(grpd.debt_osn_before_not_done_dat) as debt_osn_before_not_done_dat, /*Расходы за введение ограничения режима энергопотребления*//*number*/
sum(grpd.debt_peni_before_not_done_dat) as debt_peni_before_not_done_dat, /*Признано по юр. документу*//*number*/
sum(grpd.debt_gp_before_not_done_dat) as debt_gp_before_not_done_dat, /*Признано по юр. документу*//*number*/
grpd.ym_nach_s as ym_nach_s, /*Период начисления с*//*number*//*key*/
grpd.ym_nach_po as ym_nach_po, /*Период начисления по*//*number*//*key*/
grpd.peni_per as peni_per, /*Период пени*//*string*//*key*/
grpd.other_work_prim as other_work_prim, /*Примечание в других работах*//*string*/
grpd.dep_sname as dep_sname, /*Подразделение*//*string*//*key*/
grpd.dep_name as dep_name, /*Подразделение*//*string*//*key*/
grpd.min_dat_vid as min_dat_vid, /*Дата выдачи исп. листа*//*date*/
grpd.last_month as last_month, /*number*/
 grouping_id( grpd.kod_isp , grpd.dat_last_opl , grpd.min_dat_vid , grpd.kodp , grpd.ab_dog , grpd.category , grpd.num_delo , grpd.dat_vid , grpd.dat_post_upr , grpd.dat_post , grpd.ym_nach_s , grpd.ym_nach_po , grpd.peni_per , grpd.other_work_prim , grpd.last_month , grpd.dep , grpd.dep_sname , grpd.dep_name ) as groupingid/*Код ИЛ*//*number*/
from mat1
grpd
--\mat1
group by grouping sets ((
grpd.kod_isp, 
grpd.dat_last_opl, 
grpd.min_dat_vid, 
grpd.kodp, 
grpd.ab_dog, 
grpd.category, 
grpd.num_delo, 
grpd.dat_vid, 
grpd.dat_post_upr, 
grpd.dat_post, 
grpd.ym_nach_s, 
grpd.ym_nach_po, 
grpd.peni_per, 
grpd.other_work_prim, 
grpd.last_month, 
grpd.dep, 
grpd.dep_sname, 
grpd.dep_name
),
(
grpd.dep, 
grpd.dep_sname, 
grpd.dep_name
),
(
))
)
grsets_query
--\
)
p1
--\
order by dep_sname,dep nulls first,dep_sname nulls first,dep_name nulls first,min_dat_vid, kodp, dat_vid, dat_post, kod_isp,kod_isp nulls first,dat_last_opl nulls first,min_dat_vid nulls first,kodp nulls first,ab_dog nulls first,category nulls first,num_delo nulls first,dat_vid nulls first,dat_post_upr nulls first,dat_post nulls first,ym_nach_s nulls first,ym_nach_po nulls first,peni_per nulls first,other_work_prim nulls first,last_month nulls first) mtr
;
--\52834-main
delete from rr_temp where skod = '52834-params';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
s1, --dat
s2, --isp_fio
s3, --isp_phone
s4, --podp_fio
s5, --podp_dolg
s6, --not_done_dat
s7, --done_dat1
s8, --done_dat2
s9, --vidan_dat1
s10, --vidan_dat2
s11, --postup_dat1
s12--postup_dat2
)
--52834-params
--52834-params
select '52834-params' as skod,
'52834-params|' as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select to_char( ( sysdate ) ,'DD.MM.YYYY') as dat, /*Дата*//*string*/
isp.fio as isp_fio, /*Фио исполнителя*//*string*/
isp.tel as isp_phone, /*Телефон исполнителя*//*string*/
podp.fio as podp_fio, /*Фио подписанта*//*string*/
podp_dolzh.name as podp_dolg, /*Должность подписанта*//*string*/
 to_char( ( nvl( /*nvlu*/ :not_done_dat , ( null ) ) ) ,'DD.MM.YYYY') as not_done_dat, /*string*/
 to_char( ( nvl( /*nvlu*/ :done_dat1 , ( null ) ) ) ,'DD.MM.YYYY') as done_dat1, /*string*/
 to_char( ( nvl( /*nvlu*/ :done_dat2 , ( null ) ) ) ,'DD.MM.YYYY') as done_dat2, /*string*/
 to_char( ( nvl( /*nvlu*/ :vidan_dat1 , ( null ) ) ) ,'DD.MM.YYYY') as vidan_dat1, /*string*/
 to_char( ( nvl( /*nvlu*/ :vidan_dat2 , ( null ) ) ) ,'DD.MM.YYYY') as vidan_dat2, /*string*/
 to_char( ( nvl( /*nvlu*/ :postup_dat1 , ( null ) ) ) ,'DD.MM.YYYY') as postup_dat1, /*string*/
 to_char( ( nvl( /*nvlu*/ :postup_dat2 , ( null ) ) ) ,'DD.MM.YYYY') as postup_dat2/*string*/
from (
--dual
select dual.dummy as dummy/*string*//*key*/
from dual
dual
--\dual
)
dual
--\dual
left outer join
(
--kr_employee
select a.kod_emp as kod_emp, /*number*//*key*/
a.fio as fio, /*ФИО*//*string*/
a.tel as tel/**//*string*/
from kr_employee
a
--\kr_employee
)
isp on isp.kod_emp = ( nvl( /*nvlu*/ :kod_emp_isp , ( null ) ) ) --\kr_employee
left outer join
(
--kr_employee
select a.kod_emp as kod_emp, /*number*//*key*/
a.kod_namedolzh as kod_namedolzh, /*number*/
a.fio as fio/*ФИО*//*string*/
from kr_employee
a
--\kr_employee
)
podp on podp.kod_emp = ( nvl( /*nvlu*/ :kod_emp_podp , ( null ) ) ) --\kr_employee
left outer join
(
--ks_namedolzh
select a.kod_namedolzh as kod_namedolzh, /*number*//*key*/
a.name as name/*Наименование*//*string*/
from ks_namedolzh
a
--\ks_namedolzh
)
podp_dolzh on podp_dolzh.kod_namedolzh = podp.kod_namedolzh--\ks_namedolzh
) mtr
;
--\52834-params
end;