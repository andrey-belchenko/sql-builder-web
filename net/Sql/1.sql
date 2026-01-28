begin
delete from rr_temp where skod = '24599-dep';
begin
for rec in
(
--24599-dep
--24599-dep
select '24599-dep' as skod,
'24599-dep|#'||mtr.dep as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.rwn1 as rwn1, /*number*/
a.dep as dep, /*number*//*key*/
a.name_dep as name_dep, /*Отделение*//*string*/
a.name_uch as name_uch, /*Участок*//*string*/
a.name_gr as name_gr, /*ИКУ*//*string*/
a.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
a.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
a.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
a.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
a.opl as opl, /*Оплачено за период*//*number*/
a.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
a.nachisl_end_pros as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--
select max(a.rwn1) as rwn1, /*number*/
a.dep as dep, /*number*//*key*/
max(a.name_dep) as name_dep, /*Отделение*//*string*/
max(a.name_uch) as name_uch, /*Участок*//*string*/
max(a.name_gr) as name_gr, /*ИКУ*//*string*/
sum(a.nachisl_do_ymbeg) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
sum(a.zadol_begin) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
sum(a.dolg_self_prosr) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
sum(a.nachisl_ym_spo) as nachisl_ym_spo, /*Начислено за период*//*number*/
sum(a.opl) as opl, /*Оплачено за период*//*number*/
sum(a.nachisl_end) as nachisl_end, /*Начислено последний месяй периода*//*number*/
sum(a.nachisl_end_pros) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--24599
select row_number() over( order by dep.name , kodp_uch.name , kod_gr_potr_nas.name ) as rwn1, /**//*number*/
dog.kod_dog as kod_dog, /*number*//*key*/
dog.dep as dep, /*number*/
dep.name as name_dep, /*Отделение*//*string*/
kodp_uch.name as name_uch, /*Участок*//*string*/
kod_gr_potr_nas.name as name_gr, /*ИКУ*//*string*/
nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
nvl(nachisl_ym_spo.zadol_begin,0) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
nvl(nachisl_ym_spo.dolg_self_prosr,0) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
nvl(nachisl_ym_spo.nachisl_ym_spo,0) as nachisl_ym_spo, /*Начислено за период*//*number*/
nvl(nachisl_ym_spo.opl,0) as opl, /*Оплачено за период*//*number*/
nvl(nachisl_ym_spo.nachisl_end,0) as nachisl_end, /*Начислено последний месяй периода*//*number*/
nvl(nachisl_ym_spo.nachisl_end_pros,0) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.tep_el as tep_el, /**//*number*/
a.dep as dep, /**//*number*/
a.kodp_uch as kodp_uch, /**//*number*/
rr_refprop_dog_103.kod_refcode as kod_gr_potr_nas/*number*/
from kr_dogovor
a
--\kr_dogovor
left outer join
(
--rr_refprop_dog_103
select a.objid as kod_dog, /*number*//*key*/
max(a.kod_refcode) as kod_refcode/*number*/
from (
--rr_refprop
select a.kod_refprop as kod_refprop, /*number*//*key*/
a.kod_refcode as kod_refcode, /*number*/
a.kod_refobject as kod_refobject, /*number*/
a.objid as objid/**//*number*/
from rr_refprop
a
--\rr_refprop
)
a
--\rr_refprop
left outer join
(
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook/*number*/
from rs_refcode
a
--\rs_refcode
)
kod_refcode on a.kod_refcode = kod_refcode.kod_refcode--\rs_refcode
where
(kod_refcode.kod_refbook = 103) and (a.kod_refobject = 2) group by
a.objid/*number*//*key*/
)
rr_refprop_dog_103 on (a.kod_dog = rr_refprop_dog_103.kod_dog) --\rr_refprop_dog_103
)
dog
--\kr_dogovor
left outer join
(
--24599-dolg-ym-spo
select dog.kod_dog as kod_dog, /*number*//*key*/
sr_facvip.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено*//*number*/
sr_facvip.zadol_begin as zadol_begin, /*Остаток (руб)*//*number*/
sr_facvip.dolg_self_prosr as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sr_facvip.nachisl_ym_spo as nachisl_ym_spo, /*Начислено*//*number*/
sr_facvip.opl as opl, /*number*/
sr_facvip.nachisl_end as nachisl_end, /*Начислено*//*number*/
sr_facvip.nachisl_end_pros as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
dog
--\kr_dogovor
left outer join
(
--
select sr_facvip.kod_dog as kod_dog, /*number*//*key*/
sum(sf_nach_ym_1.nachisl) as nachisl_do_ymbeg, /*Начислено*//*number*/
sum(sf_beg.ostatok) as zadol_begin, /*Остаток (руб)*//*number*/
sum(sf_beg.dolg_self) as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sum(sf_nach_spo.nachisl) as nachisl_ym_spo, /*Начислено*//*number*/
sum(opl_spo.opl) as opl, /*number*/
sum(sf_nach_end.nachisl) as nachisl_end, /*Начислено*//*number*/
sum(sf_nach_ym_add1.dolg_self) as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sr_facvip
--\sr_facvip
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 case when (a.dat_bzad <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh8.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh8
--\sr_facras
where
(sr_facras_psh8.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) )
sf_nach_ym_add1 on (sr_facvip.kod_sf = sf_nach_ym_add1.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_end.kod_sf as kod_sf, /*number*//*key*/
sf_nach_end.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh4.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh4
--\sr_facras
where
(sr_facras_psh4.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_end
--\sr_facvip
where
sf_nach_end.ym = 0)
sf_nach_end on (sr_facvip.kod_sf = sf_nach_end.kod_sf) --\
left outer join
(
--
select sf_nach_spo.kod_sf as kod_sf, /*number*//*key*/
sf_nach_spo.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh5.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh5
--\sr_facras
where
(sr_facras_psh5.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_spo
--\sr_facvip
where
sf_nach_spo.ym between 0 and 0)
sf_nach_spo on (sr_facvip.kod_sf = sf_nach_spo.kod_sf) --\
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) as ostatok, /*Остаток (руб)*//*number*/
 case when (a.dat_bzad <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh10.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh10
--\sr_facras
where
(sr_facras_psh10.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) )
sf_beg on (sr_facvip.kod_sf = sf_beg.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_ym_1.kod_sf as kod_sf, /*number*//*key*/
sf_nach_ym_1.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh6.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh6
--\sr_facras
where
(sr_facras_psh6.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_ym_1
--\sr_facvip
where
sf_nach_ym_1.ym = ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) )
sf_nach_ym_1 on (sr_facvip.kod_sf = sf_nach_ym_1.kod_sf) --\
left outer join
(
--
select opl_spo.kod_sf as kod_sf, /*number*//*key*/
sum(opl_spo.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.ym as ym/*Период оплаты*//*number*/
from sr_opl
a
--\sr_opl
)
opl_spo
--\sr_opl
where
opl_spo.ym between 0 and 0 group by
opl_spo.kod_sf/*number*//*key*/
)
opl_spo on opl_spo.kod_sf = sr_facvip.kod_sf--\
where
sr_facvip.vid_real = 2 group by
sr_facvip.kod_dog/*number*//*key*/
)
sr_facvip on sr_facvip.kod_dog = dog.kod_dog--\
)
nachisl_ym_spo on (dog.kod_dog = nachisl_ym_spo.kod_dog) --\24599-dolg-ym-spo
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
kodp_uch on dog.kodp_uch = kodp_uch.kodp--\kr_org
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
dep on dog.dep = dep.kodp--\kr_org
left outer join
(
--p_gr_potr_nas
select a.kod_refcode as kod_gr_potr_nas, /*number*//*key*/
a.name as name, /*Наименование*//*string*/
 case when (a.kod_refcode in (356 , 359 , 354 , 355) ) then 1 else ( case when (a.kod_refcode in (363 , 364 , 361 , 362) ) then 2 else 0 end ) end as pr_iku_rso/*number*/
from (
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook, /*number*/
a.name as name/*Наименование*//*string*/
from rs_refcode
a
--\rs_refcode
)
a
--\rs_refcode
where
a.kod_refbook = 103)
kod_gr_potr_nas on dog.kod_gr_potr_nas = kod_gr_potr_nas.kod_gr_potr_nas--\p_gr_potr_nas
where
(dog.tep_el = 1)  and (dog.dep in (0))   and (dog.kodp_uch in (0))  and ( nvl( nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.zadol_begin,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.dolg_self_prosr,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_ym_spo,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.opl,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end_pros,0) ,0)!=0 ) and (kod_gr_potr_nas.pr_iku_rso = 1) )
a
--\24599
 group by
a.dep/*number*//*key*/
)
a
--\
order by rwn1) mtr
)
loop
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --rwn1
n2, --dep
s1, --name_dep
s2, --name_uch
s3, --name_gr
n3, --nachisl_do_ymbeg
n4, --zadol_begin
n5, --dolg_self_prosr
n6, --nachisl_ym_spo
n7, --opl
n8, --nachisl_end
n9--nachisl_end_pros
)
values (
rec.skod, 
rec.sid, 
rec.rn, 
rec.rwn1, 
rec.dep, 
rec.name_dep, 
rec.name_uch, 
rec.name_gr, 
rec.nachisl_do_ymbeg, 
rec.zadol_begin, 
rec.dolg_self_prosr, 
rec.nachisl_ym_spo, 
rec.opl, 
rec.nachisl_end, 
rec.nachisl_end_pros);
end loop;
end;
--\24599-dep
delete from rr_temp where skod = '24599-uch';
begin
for rec in
(
--24599-uch
--24599-uch
select '24599-uch' as skod,
'24599-uch|#'||mtr.dep||'#'||mtr.kodp_uch as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select b.rwn1 as rwn1, /*number*/
b.dep as dep, /*number*//*key*/
b.kodp_uch as kodp_uch, /*number*//*key*/
b.name_dep as name_dep, /*Отделение*//*string*/
b.name_uch as name_uch, /*Участок*//*string*/
b.name_gr as name_gr, /*ИКУ*//*string*/
b.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
b.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
b.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
b.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
b.opl as opl, /*Оплачено за период*//*number*/
b.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
b.nachisl_end_pros as nachisl_end_pros, /*Просрочено на конец периода*//*number*/
a.sid as sparentid
from (
--24599-uch
select a.rwn1 as rwn1, /*number*/
a.dep as dep, /*number*//*key*/
a.kodp_uch as kodp_uch, /*number*//*key*/
a.name_dep as name_dep, /*Отделение*//*string*/
a.name_uch as name_uch, /*Участок*//*string*/
a.name_gr as name_gr, /*ИКУ*//*string*/
a.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
a.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
a.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
a.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
a.opl as opl, /*Оплачено за период*//*number*/
a.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
a.nachisl_end_pros as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--
select max(a.rwn1) as rwn1, /*number*/
a.dep as dep, /*number*//*key*/
a.kodp_uch as kodp_uch, /*number*//*key*/
max(a.name_dep) as name_dep, /*Отделение*//*string*/
max(a.name_uch) as name_uch, /*Участок*//*string*/
max(a.name_gr) as name_gr, /*ИКУ*//*string*/
sum(a.nachisl_do_ymbeg) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
sum(a.zadol_begin) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
sum(a.dolg_self_prosr) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
sum(a.nachisl_ym_spo) as nachisl_ym_spo, /*Начислено за период*//*number*/
sum(a.opl) as opl, /*Оплачено за период*//*number*/
sum(a.nachisl_end) as nachisl_end, /*Начислено последний месяй периода*//*number*/
sum(a.nachisl_end_pros) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--24599
select row_number() over( order by dep.name , kodp_uch.name , kod_gr_potr_nas.name ) as rwn1, /**//*number*/
dog.kod_dog as kod_dog, /*number*//*key*/
dog.dep as dep, /*number*/
dog.kodp_uch as kodp_uch, /*number*/
dep.name as name_dep, /*Отделение*//*string*/
kodp_uch.name as name_uch, /*Участок*//*string*/
kod_gr_potr_nas.name as name_gr, /*ИКУ*//*string*/
nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
nvl(nachisl_ym_spo.zadol_begin,0) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
nvl(nachisl_ym_spo.dolg_self_prosr,0) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
nvl(nachisl_ym_spo.nachisl_ym_spo,0) as nachisl_ym_spo, /*Начислено за период*//*number*/
nvl(nachisl_ym_spo.opl,0) as opl, /*Оплачено за период*//*number*/
nvl(nachisl_ym_spo.nachisl_end,0) as nachisl_end, /*Начислено последний месяй периода*//*number*/
nvl(nachisl_ym_spo.nachisl_end_pros,0) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.tep_el as tep_el, /**//*number*/
a.dep as dep, /**//*number*/
a.kodp_uch as kodp_uch, /**//*number*/
rr_refprop_dog_103.kod_refcode as kod_gr_potr_nas/*number*/
from kr_dogovor
a
--\kr_dogovor
left outer join
(
--rr_refprop_dog_103
select a.objid as kod_dog, /*number*//*key*/
max(a.kod_refcode) as kod_refcode/*number*/
from (
--rr_refprop
select a.kod_refprop as kod_refprop, /*number*//*key*/
a.kod_refcode as kod_refcode, /*number*/
a.kod_refobject as kod_refobject, /*number*/
a.objid as objid/**//*number*/
from rr_refprop
a
--\rr_refprop
)
a
--\rr_refprop
left outer join
(
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook/*number*/
from rs_refcode
a
--\rs_refcode
)
kod_refcode on a.kod_refcode = kod_refcode.kod_refcode--\rs_refcode
where
(kod_refcode.kod_refbook = 103) and (a.kod_refobject = 2) group by
a.objid/*number*//*key*/
)
rr_refprop_dog_103 on (a.kod_dog = rr_refprop_dog_103.kod_dog) --\rr_refprop_dog_103
)
dog
--\kr_dogovor
left outer join
(
--24599-dolg-ym-spo
select dog.kod_dog as kod_dog, /*number*//*key*/
sr_facvip.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено*//*number*/
sr_facvip.zadol_begin as zadol_begin, /*Остаток (руб)*//*number*/
sr_facvip.dolg_self_prosr as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sr_facvip.nachisl_ym_spo as nachisl_ym_spo, /*Начислено*//*number*/
sr_facvip.opl as opl, /*number*/
sr_facvip.nachisl_end as nachisl_end, /*Начислено*//*number*/
sr_facvip.nachisl_end_pros as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
dog
--\kr_dogovor
left outer join
(
--
select sr_facvip.kod_dog as kod_dog, /*number*//*key*/
sum(sf_nach_ym_1.nachisl) as nachisl_do_ymbeg, /*Начислено*//*number*/
sum(sf_beg.ostatok) as zadol_begin, /*Остаток (руб)*//*number*/
sum(sf_beg.dolg_self) as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sum(sf_nach_spo.nachisl) as nachisl_ym_spo, /*Начислено*//*number*/
sum(opl_spo.opl) as opl, /*number*/
sum(sf_nach_end.nachisl) as nachisl_end, /*Начислено*//*number*/
sum(sf_nach_ym_add1.dolg_self) as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sr_facvip
--\sr_facvip
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 case when (a.dat_bzad <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh8.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh8
--\sr_facras
where
(sr_facras_psh8.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) )
sf_nach_ym_add1 on (sr_facvip.kod_sf = sf_nach_ym_add1.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_end.kod_sf as kod_sf, /*number*//*key*/
sf_nach_end.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh4.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh4
--\sr_facras
where
(sr_facras_psh4.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_end
--\sr_facvip
where
sf_nach_end.ym = 0)
sf_nach_end on (sr_facvip.kod_sf = sf_nach_end.kod_sf) --\
left outer join
(
--
select sf_nach_spo.kod_sf as kod_sf, /*number*//*key*/
sf_nach_spo.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh5.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh5
--\sr_facras
where
(sr_facras_psh5.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_spo
--\sr_facvip
where
sf_nach_spo.ym between 0 and 0)
sf_nach_spo on (sr_facvip.kod_sf = sf_nach_spo.kod_sf) --\
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) as ostatok, /*Остаток (руб)*//*number*/
 case when (a.dat_bzad <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh10.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh10
--\sr_facras
where
(sr_facras_psh10.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) )
sf_beg on (sr_facvip.kod_sf = sf_beg.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_ym_1.kod_sf as kod_sf, /*number*//*key*/
sf_nach_ym_1.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh6.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh6
--\sr_facras
where
(sr_facras_psh6.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_ym_1
--\sr_facvip
where
sf_nach_ym_1.ym = ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) )
sf_nach_ym_1 on (sr_facvip.kod_sf = sf_nach_ym_1.kod_sf) --\
left outer join
(
--
select opl_spo.kod_sf as kod_sf, /*number*//*key*/
sum(opl_spo.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.ym as ym/*Период оплаты*//*number*/
from sr_opl
a
--\sr_opl
)
opl_spo
--\sr_opl
where
opl_spo.ym between 0 and 0 group by
opl_spo.kod_sf/*number*//*key*/
)
opl_spo on opl_spo.kod_sf = sr_facvip.kod_sf--\
where
sr_facvip.vid_real = 2 group by
sr_facvip.kod_dog/*number*//*key*/
)
sr_facvip on sr_facvip.kod_dog = dog.kod_dog--\
)
nachisl_ym_spo on (dog.kod_dog = nachisl_ym_spo.kod_dog) --\24599-dolg-ym-spo
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
kodp_uch on dog.kodp_uch = kodp_uch.kodp--\kr_org
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
dep on dog.dep = dep.kodp--\kr_org
left outer join
(
--p_gr_potr_nas
select a.kod_refcode as kod_gr_potr_nas, /*number*//*key*/
a.name as name, /*Наименование*//*string*/
 case when (a.kod_refcode in (356 , 359 , 354 , 355) ) then 1 else ( case when (a.kod_refcode in (363 , 364 , 361 , 362) ) then 2 else 0 end ) end as pr_iku_rso/*number*/
from (
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook, /*number*/
a.name as name/*Наименование*//*string*/
from rs_refcode
a
--\rs_refcode
)
a
--\rs_refcode
where
a.kod_refbook = 103)
kod_gr_potr_nas on dog.kod_gr_potr_nas = kod_gr_potr_nas.kod_gr_potr_nas--\p_gr_potr_nas
where
(dog.tep_el = 1)  and (dog.dep in (0))   and (dog.kodp_uch in (0))  and ( nvl( nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.zadol_begin,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.dolg_self_prosr,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_ym_spo,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.opl,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end_pros,0) ,0)!=0 ) and (kod_gr_potr_nas.pr_iku_rso = 1) )
a
--\24599
 group by
a.dep, /*number*//*key*/
a.kodp_uch/*number*//*key*/
)
a
--\
order by rwn1)
b
--\24599-uch
inner join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n2 as dep
from rr_temp where skod = '24599-dep'
)
a on a.dep = b.dep--\24599-dep
order by rwn1) mtr
)
loop
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --rwn1
n2, --dep
n3, --kodp_uch
s1, --name_dep
s2, --name_uch
s3, --name_gr
n4, --nachisl_do_ymbeg
n5, --zadol_begin
n6, --dolg_self_prosr
n7, --nachisl_ym_spo
n8, --opl
n9, --nachisl_end
n10, --nachisl_end_pros
sparentid--sparentid
)
values (
rec.skod, 
rec.sid, 
rec.rn, 
rec.rwn1, 
rec.dep, 
rec.kodp_uch, 
rec.name_dep, 
rec.name_uch, 
rec.name_gr, 
rec.nachisl_do_ymbeg, 
rec.zadol_begin, 
rec.dolg_self_prosr, 
rec.nachisl_ym_spo, 
rec.opl, 
rec.nachisl_end, 
rec.nachisl_end_pros, 
rec.sparentid);
end loop;
end;
--\24599-uch
delete from rr_temp where skod = '24599-iku';
begin
for rec in
(
--24599-iku
--24599-iku
select '24599-iku' as skod,
'24599-iku|#'||mtr.dep||'#'||mtr.kodp_uch||'#'||mtr.kod_gr_potr_nas as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select c.rwn as rwn, /*№*//*number*/
c.rwn1 as rwn1, /*number*/
c.dep as dep, /*number*//*key*/
c.kodp_uch as kodp_uch, /*number*//*key*/
c.kod_gr_potr_nas as kod_gr_potr_nas, /*number*//*key*/
c.name_dep as name_dep, /*Отделение*//*string*/
c.name_uch as name_uch, /*Участок*//*string*/
c.name_gr as name_gr, /*ИКУ*//*string*/
c.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
c.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
c.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
c.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
c.opl as opl, /*Оплачено за период*//*number*/
c.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
c.nachisl_end_pros as nachisl_end_pros, /*Просрочено на конец периода*//*number*/
b.sid as sparentid
from (
--24599-iku
select row_number() over( order by a.rwn1 ) as rwn, /*№*//*number*/
a.rwn1 as rwn1, /*number*/
a.dep as dep, /*number*//*key*/
a.kodp_uch as kodp_uch, /*number*//*key*/
a.kod_gr_potr_nas as kod_gr_potr_nas, /*number*//*key*/
a.name_dep as name_dep, /*Отделение*//*string*/
a.name_uch as name_uch, /*Участок*//*string*/
a.name_gr as name_gr, /*ИКУ*//*string*/
a.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
a.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
a.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
a.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
a.opl as opl, /*Оплачено за период*//*number*/
a.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
a.nachisl_end_pros as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--
select max(a.rwn1) as rwn1, /*number*/
a.dep as dep, /*number*//*key*/
a.kodp_uch as kodp_uch, /*number*//*key*/
a.kod_gr_potr_nas as kod_gr_potr_nas, /*number*//*key*/
max(a.name_dep) as name_dep, /*Отделение*//*string*/
max(a.name_uch) as name_uch, /*Участок*//*string*/
max(a.name_gr) as name_gr, /*ИКУ*//*string*/
sum(a.nachisl_do_ymbeg) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
sum(a.zadol_begin) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
sum(a.dolg_self_prosr) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
sum(a.nachisl_ym_spo) as nachisl_ym_spo, /*Начислено за период*//*number*/
sum(a.opl) as opl, /*Оплачено за период*//*number*/
sum(a.nachisl_end) as nachisl_end, /*Начислено последний месяй периода*//*number*/
sum(a.nachisl_end_pros) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--24599
select row_number() over( order by dep.name , kodp_uch.name , kod_gr_potr_nas.name ) as rwn1, /**//*number*/
dog.kod_dog as kod_dog, /*number*//*key*/
dog.dep as dep, /*number*/
dog.kodp_uch as kodp_uch, /*number*/
dog.kod_gr_potr_nas as kod_gr_potr_nas, /*number*/
dep.name as name_dep, /*Отделение*//*string*/
kodp_uch.name as name_uch, /*Участок*//*string*/
kod_gr_potr_nas.name as name_gr, /*ИКУ*//*string*/
nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
nvl(nachisl_ym_spo.zadol_begin,0) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
nvl(nachisl_ym_spo.dolg_self_prosr,0) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
nvl(nachisl_ym_spo.nachisl_ym_spo,0) as nachisl_ym_spo, /*Начислено за период*//*number*/
nvl(nachisl_ym_spo.opl,0) as opl, /*Оплачено за период*//*number*/
nvl(nachisl_ym_spo.nachisl_end,0) as nachisl_end, /*Начислено последний месяй периода*//*number*/
nvl(nachisl_ym_spo.nachisl_end_pros,0) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.tep_el as tep_el, /**//*number*/
a.dep as dep, /**//*number*/
a.kodp_uch as kodp_uch, /**//*number*/
rr_refprop_dog_103.kod_refcode as kod_gr_potr_nas/*number*/
from kr_dogovor
a
--\kr_dogovor
left outer join
(
--rr_refprop_dog_103
select a.objid as kod_dog, /*number*//*key*/
max(a.kod_refcode) as kod_refcode/*number*/
from (
--rr_refprop
select a.kod_refprop as kod_refprop, /*number*//*key*/
a.kod_refcode as kod_refcode, /*number*/
a.kod_refobject as kod_refobject, /*number*/
a.objid as objid/**//*number*/
from rr_refprop
a
--\rr_refprop
)
a
--\rr_refprop
left outer join
(
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook/*number*/
from rs_refcode
a
--\rs_refcode
)
kod_refcode on a.kod_refcode = kod_refcode.kod_refcode--\rs_refcode
where
(kod_refcode.kod_refbook = 103) and (a.kod_refobject = 2) group by
a.objid/*number*//*key*/
)
rr_refprop_dog_103 on (a.kod_dog = rr_refprop_dog_103.kod_dog) --\rr_refprop_dog_103
)
dog
--\kr_dogovor
left outer join
(
--24599-dolg-ym-spo
select dog.kod_dog as kod_dog, /*number*//*key*/
sr_facvip.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено*//*number*/
sr_facvip.zadol_begin as zadol_begin, /*Остаток (руб)*//*number*/
sr_facvip.dolg_self_prosr as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sr_facvip.nachisl_ym_spo as nachisl_ym_spo, /*Начислено*//*number*/
sr_facvip.opl as opl, /*number*/
sr_facvip.nachisl_end as nachisl_end, /*Начислено*//*number*/
sr_facvip.nachisl_end_pros as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
dog
--\kr_dogovor
left outer join
(
--
select sr_facvip.kod_dog as kod_dog, /*number*//*key*/
sum(sf_nach_ym_1.nachisl) as nachisl_do_ymbeg, /*Начислено*//*number*/
sum(sf_beg.ostatok) as zadol_begin, /*Остаток (руб)*//*number*/
sum(sf_beg.dolg_self) as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sum(sf_nach_spo.nachisl) as nachisl_ym_spo, /*Начислено*//*number*/
sum(opl_spo.opl) as opl, /*number*/
sum(sf_nach_end.nachisl) as nachisl_end, /*Начислено*//*number*/
sum(sf_nach_ym_add1.dolg_self) as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sr_facvip
--\sr_facvip
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 case when (a.dat_bzad <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh8.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh8
--\sr_facras
where
(sr_facras_psh8.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) )
sf_nach_ym_add1 on (sr_facvip.kod_sf = sf_nach_ym_add1.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_end.kod_sf as kod_sf, /*number*//*key*/
sf_nach_end.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh4.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh4
--\sr_facras
where
(sr_facras_psh4.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_end
--\sr_facvip
where
sf_nach_end.ym = 0)
sf_nach_end on (sr_facvip.kod_sf = sf_nach_end.kod_sf) --\
left outer join
(
--
select sf_nach_spo.kod_sf as kod_sf, /*number*//*key*/
sf_nach_spo.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh5.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh5
--\sr_facras
where
(sr_facras_psh5.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_spo
--\sr_facvip
where
sf_nach_spo.ym between 0 and 0)
sf_nach_spo on (sr_facvip.kod_sf = sf_nach_spo.kod_sf) --\
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) as ostatok, /*Остаток (руб)*//*number*/
 case when (a.dat_bzad <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh10.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh10
--\sr_facras
where
(sr_facras_psh10.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) )
sf_beg on (sr_facvip.kod_sf = sf_beg.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_ym_1.kod_sf as kod_sf, /*number*//*key*/
sf_nach_ym_1.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh6.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh6
--\sr_facras
where
(sr_facras_psh6.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_ym_1
--\sr_facvip
where
sf_nach_ym_1.ym = ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) )
sf_nach_ym_1 on (sr_facvip.kod_sf = sf_nach_ym_1.kod_sf) --\
left outer join
(
--
select opl_spo.kod_sf as kod_sf, /*number*//*key*/
sum(opl_spo.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.ym as ym/*Период оплаты*//*number*/
from sr_opl
a
--\sr_opl
)
opl_spo
--\sr_opl
where
opl_spo.ym between 0 and 0 group by
opl_spo.kod_sf/*number*//*key*/
)
opl_spo on opl_spo.kod_sf = sr_facvip.kod_sf--\
where
sr_facvip.vid_real = 2 group by
sr_facvip.kod_dog/*number*//*key*/
)
sr_facvip on sr_facvip.kod_dog = dog.kod_dog--\
)
nachisl_ym_spo on (dog.kod_dog = nachisl_ym_spo.kod_dog) --\24599-dolg-ym-spo
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
kodp_uch on dog.kodp_uch = kodp_uch.kodp--\kr_org
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
dep on dog.dep = dep.kodp--\kr_org
left outer join
(
--p_gr_potr_nas
select a.kod_refcode as kod_gr_potr_nas, /*number*//*key*/
a.name as name, /*Наименование*//*string*/
 case when (a.kod_refcode in (356 , 359 , 354 , 355) ) then 1 else ( case when (a.kod_refcode in (363 , 364 , 361 , 362) ) then 2 else 0 end ) end as pr_iku_rso/*number*/
from (
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook, /*number*/
a.name as name/*Наименование*//*string*/
from rs_refcode
a
--\rs_refcode
)
a
--\rs_refcode
where
a.kod_refbook = 103)
kod_gr_potr_nas on dog.kod_gr_potr_nas = kod_gr_potr_nas.kod_gr_potr_nas--\p_gr_potr_nas
where
(dog.tep_el = 1)  and (dog.dep in (0))   and (dog.kodp_uch in (0))  and ( nvl( nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.zadol_begin,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.dolg_self_prosr,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_ym_spo,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.opl,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end_pros,0) ,0)!=0 ) and (kod_gr_potr_nas.pr_iku_rso = 1) )
a
--\24599
 group by
a.dep, /*number*//*key*/
a.kodp_uch, /*number*//*key*/
a.kod_gr_potr_nas/*number*//*key*/
)
a
--\
order by rwn1)
c
--\24599-iku
inner join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n2 as dep,
n3 as kodp_uch
from rr_temp where skod = '24599-uch'
)
b on b.kodp_uch = c.kodp_uch--\24599-uch
order by rwn1) mtr
)
loop
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --rwn
n2, --rwn1
n3, --dep
n4, --kodp_uch
n5, --kod_gr_potr_nas
s1, --name_dep
s2, --name_uch
s3, --name_gr
n6, --nachisl_do_ymbeg
n7, --zadol_begin
n8, --dolg_self_prosr
n9, --nachisl_ym_spo
n10, --opl
n11, --nachisl_end
n12, --nachisl_end_pros
sparentid--sparentid
)
values (
rec.skod, 
rec.sid, 
rec.rn, 
rec.rwn, 
rec.rwn1, 
rec.dep, 
rec.kodp_uch, 
rec.kod_gr_potr_nas, 
rec.name_dep, 
rec.name_uch, 
rec.name_gr, 
rec.nachisl_do_ymbeg, 
rec.zadol_begin, 
rec.dolg_self_prosr, 
rec.nachisl_ym_spo, 
rec.opl, 
rec.nachisl_end, 
rec.nachisl_end_pros, 
rec.sparentid);
end loop;
end;
--\24599-iku
delete from rr_temp where skod = '24599-dog';
begin
for rec in
(
--24599-dog
--24599-dog
select '24599-dog' as skod,
'24599-dog|#'||mtr.kod_dog as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select all_dog.rwn1 as rwn1, /*number*/
all_dog.kod_dog as kod_dog, /*number*//*key*/
all_dog.dep as dep, /*number*/
all_dog.kodp_uch as kodp_uch, /*number*/
all_dog.kod_gr_potr_nas as kod_gr_potr_nas, /*number*/
all_dog.ndog as ndog, /*Номер договора*//*string*/
all_dog.payer_name as payer_name, /*Наименование абонента*//*string*/
all_dog.name_dep as name_dep, /*Отделение*//*string*/
all_dog.name_uch as name_uch, /*Участок*//*string*/
all_dog.name_gr as name_gr, /*ИКУ*//*string*/
all_dog.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
all_dog.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
all_dog.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
all_dog.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
all_dog.opl as opl, /*Оплачено за период*//*number*/
all_dog.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
all_dog.nachisl_end_pros as nachisl_end_pros, /*Просрочено на конец периода*//*number*/
c.sid as sparentid
from (
--24599-dog
select a.rwn1 as rwn1, /*number*/
a.kod_dog as kod_dog, /*number*//*key*/
a.dep as dep, /*number*/
a.kodp_uch as kodp_uch, /*number*/
a.kod_gr_potr_nas as kod_gr_potr_nas, /*number*/
a.ndog as ndog, /*Номер договора*//*string*/
a.payer_name as payer_name, /*Наименование абонента*//*string*/
a.name_dep as name_dep, /*Отделение*//*string*/
a.name_uch as name_uch, /*Участок*//*string*/
a.name_gr as name_gr, /*ИКУ*//*string*/
a.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
a.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
a.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
a.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
a.opl as opl, /*Оплачено за период*//*number*/
a.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
a.nachisl_end_pros as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--
select a.rwn1 as rwn1, /*number*/
a.kod_dog as kod_dog, /*number*//*key*/
a.dep as dep, /*number*/
a.kodp_uch as kodp_uch, /*number*/
a.kod_gr_potr_nas as kod_gr_potr_nas, /*number*/
a.ndog as ndog, /*Номер договора*//*string*/
a.payer_name as payer_name, /*Наименование абонента*//*string*/
a.name_dep as name_dep, /*Отделение*//*string*/
a.name_uch as name_uch, /*Участок*//*string*/
a.name_gr as name_gr, /*ИКУ*//*string*/
a.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
a.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
a.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
a.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
a.opl as opl, /*Оплачено за период*//*number*/
a.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
a.nachisl_end_pros as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--24599
select row_number() over( order by dep.name , kodp_uch.name , kod_gr_potr_nas.name ) as rwn1, /**//*number*/
dog.kod_dog as kod_dog, /*number*//*key*/
dog.dep as dep, /*number*/
dog.kodp_uch as kodp_uch, /*number*/
dog.kod_gr_potr_nas as kod_gr_potr_nas, /*number*/
dog.ndog as ndog, /*Номер договора*//*string*/
kodp.name as payer_name, /*Наименование абонента*//*string*/
dep.name as name_dep, /*Отделение*//*string*/
kodp_uch.name as name_uch, /*Участок*//*string*/
kod_gr_potr_nas.name as name_gr, /*ИКУ*//*string*/
nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
nvl(nachisl_ym_spo.zadol_begin,0) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
nvl(nachisl_ym_spo.dolg_self_prosr,0) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
nvl(nachisl_ym_spo.nachisl_ym_spo,0) as nachisl_ym_spo, /*Начислено за период*//*number*/
nvl(nachisl_ym_spo.opl,0) as opl, /*Оплачено за период*//*number*/
nvl(nachisl_ym_spo.nachisl_end,0) as nachisl_end, /*Начислено последний месяй периода*//*number*/
nvl(nachisl_ym_spo.nachisl_end_pros,0) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.kodp as kodp, /**//*number*/
a.tep_el as tep_el, /**//*number*/
a.ndog as ndog, /*Номер договора*//*string*/
a.dep as dep, /**//*number*/
a.kodp_uch as kodp_uch, /**//*number*/
rr_refprop_dog_103.kod_refcode as kod_gr_potr_nas/*number*/
from kr_dogovor
a
--\kr_dogovor
left outer join
(
--rr_refprop_dog_103
select a.objid as kod_dog, /*number*//*key*/
max(a.kod_refcode) as kod_refcode/*number*/
from (
--rr_refprop
select a.kod_refprop as kod_refprop, /*number*//*key*/
a.kod_refcode as kod_refcode, /*number*/
a.kod_refobject as kod_refobject, /*number*/
a.objid as objid/**//*number*/
from rr_refprop
a
--\rr_refprop
)
a
--\rr_refprop
left outer join
(
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook/*number*/
from rs_refcode
a
--\rs_refcode
)
kod_refcode on a.kod_refcode = kod_refcode.kod_refcode--\rs_refcode
where
(kod_refcode.kod_refbook = 103) and (a.kod_refobject = 2) group by
a.objid/*number*//*key*/
)
rr_refprop_dog_103 on (a.kod_dog = rr_refprop_dog_103.kod_dog) --\rr_refprop_dog_103
)
dog
--\kr_dogovor
left outer join
(
--24599-dolg-ym-spo
select dog.kod_dog as kod_dog, /*number*//*key*/
sr_facvip.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено*//*number*/
sr_facvip.zadol_begin as zadol_begin, /*Остаток (руб)*//*number*/
sr_facvip.dolg_self_prosr as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sr_facvip.nachisl_ym_spo as nachisl_ym_spo, /*Начислено*//*number*/
sr_facvip.opl as opl, /*number*/
sr_facvip.nachisl_end as nachisl_end, /*Начислено*//*number*/
sr_facvip.nachisl_end_pros as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
dog
--\kr_dogovor
left outer join
(
--
select sr_facvip.kod_dog as kod_dog, /*number*//*key*/
sum(sf_nach_ym_1.nachisl) as nachisl_do_ymbeg, /*Начислено*//*number*/
sum(sf_beg.ostatok) as zadol_begin, /*Остаток (руб)*//*number*/
sum(sf_beg.dolg_self) as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sum(sf_nach_spo.nachisl) as nachisl_ym_spo, /*Начислено*//*number*/
sum(opl_spo.opl) as opl, /*number*/
sum(sf_nach_end.nachisl) as nachisl_end, /*Начислено*//*number*/
sum(sf_nach_ym_add1.dolg_self) as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sr_facvip
--\sr_facvip
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 case when (a.dat_bzad <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh8.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh8
--\sr_facras
where
(sr_facras_psh8.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) )
sf_nach_ym_add1 on (sr_facvip.kod_sf = sf_nach_ym_add1.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_end.kod_sf as kod_sf, /*number*//*key*/
sf_nach_end.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh4.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh4
--\sr_facras
where
(sr_facras_psh4.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_end
--\sr_facvip
where
sf_nach_end.ym = 0)
sf_nach_end on (sr_facvip.kod_sf = sf_nach_end.kod_sf) --\
left outer join
(
--
select sf_nach_spo.kod_sf as kod_sf, /*number*//*key*/
sf_nach_spo.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh5.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh5
--\sr_facras
where
(sr_facras_psh5.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_spo
--\sr_facvip
where
sf_nach_spo.ym between 0 and 0)
sf_nach_spo on (sr_facvip.kod_sf = sf_nach_spo.kod_sf) --\
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) as ostatok, /*Остаток (руб)*//*number*/
 case when (a.dat_bzad <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh10.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh10
--\sr_facras
where
(sr_facras_psh10.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) )
sf_beg on (sr_facvip.kod_sf = sf_beg.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_ym_1.kod_sf as kod_sf, /*number*//*key*/
sf_nach_ym_1.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh6.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh6
--\sr_facras
where
(sr_facras_psh6.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_ym_1
--\sr_facvip
where
sf_nach_ym_1.ym = ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) )
sf_nach_ym_1 on (sr_facvip.kod_sf = sf_nach_ym_1.kod_sf) --\
left outer join
(
--
select opl_spo.kod_sf as kod_sf, /*number*//*key*/
sum(opl_spo.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.ym as ym/*Период оплаты*//*number*/
from sr_opl
a
--\sr_opl
)
opl_spo
--\sr_opl
where
opl_spo.ym between 0 and 0 group by
opl_spo.kod_sf/*number*//*key*/
)
opl_spo on opl_spo.kod_sf = sr_facvip.kod_sf--\
where
sr_facvip.vid_real = 2 group by
sr_facvip.kod_dog/*number*//*key*/
)
sr_facvip on sr_facvip.kod_dog = dog.kod_dog--\
)
nachisl_ym_spo on (dog.kod_dog = nachisl_ym_spo.kod_dog) --\24599-dolg-ym-spo
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
kodp_uch on dog.kodp_uch = kodp_uch.kodp--\kr_org
left outer join
(
--kr_payer
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Наименование абонента*//*string*/
from kr_payer
a
--\kr_payer
)
kodp on dog.kodp = kodp.kodp--\kr_payer
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
dep on dog.dep = dep.kodp--\kr_org
left outer join
(
--p_gr_potr_nas
select a.kod_refcode as kod_gr_potr_nas, /*number*//*key*/
a.name as name, /*Наименование*//*string*/
 case when (a.kod_refcode in (356 , 359 , 354 , 355) ) then 1 else ( case when (a.kod_refcode in (363 , 364 , 361 , 362) ) then 2 else 0 end ) end as pr_iku_rso/*number*/
from (
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook, /*number*/
a.name as name/*Наименование*//*string*/
from rs_refcode
a
--\rs_refcode
)
a
--\rs_refcode
where
a.kod_refbook = 103)
kod_gr_potr_nas on dog.kod_gr_potr_nas = kod_gr_potr_nas.kod_gr_potr_nas--\p_gr_potr_nas
where
(dog.tep_el = 1)  and (dog.dep in (0))   and (dog.kodp_uch in (0))  and ( nvl( nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.zadol_begin,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.dolg_self_prosr,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_ym_spo,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.opl,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end_pros,0) ,0)!=0 ) and (kod_gr_potr_nas.pr_iku_rso = 1) )
a
--\24599
)
a
--\
order by rwn1)
all_dog
--\24599-dog
inner join
(
select /*+ dynamic_sampling(rr_temp 10)*/
sid,
sparentid,
rn,
n3 as dep,
n4 as kodp_uch,
n5 as kod_gr_potr_nas
from rr_temp where skod = '24599-iku'
)
c on ( nvl( c.kod_gr_potr_nas ,-1000) = nvl( all_dog.kod_gr_potr_nas ,-1000) ) and (c.kodp_uch = all_dog.kodp_uch) --\24599-iku
order by rwn1) mtr
)
loop
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --rwn1
n2, --kod_dog
n3, --dep
n4, --kodp_uch
n5, --kod_gr_potr_nas
s1, --ndog
s2, --payer_name
s3, --name_dep
s4, --name_uch
s5, --name_gr
n6, --nachisl_do_ymbeg
n7, --zadol_begin
n8, --dolg_self_prosr
n9, --nachisl_ym_spo
n10, --opl
n11, --nachisl_end
n12, --nachisl_end_pros
sparentid--sparentid
)
values (
rec.skod, 
rec.sid, 
rec.rn, 
rec.rwn1, 
rec.kod_dog, 
rec.dep, 
rec.kodp_uch, 
rec.kod_gr_potr_nas, 
rec.ndog, 
rec.payer_name, 
rec.name_dep, 
rec.name_uch, 
rec.name_gr, 
rec.nachisl_do_ymbeg, 
rec.zadol_begin, 
rec.dolg_self_prosr, 
rec.nachisl_ym_spo, 
rec.opl, 
rec.nachisl_end, 
rec.nachisl_end_pros, 
rec.sparentid);
end loop;
end;
--\24599-dog
delete from rr_temp where skod = '24599-itogo';
begin
for rec in
(
--24599-itogo
--24599-itogo
select '24599-itogo' as skod,
'24599-itogo|' as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select a.rwn1 as rwn1, /*number*/
a.name_dep as name_dep, /*Отделение*//*string*/
a.name_uch as name_uch, /*Участок*//*string*/
a.name_gr as name_gr, /*ИКУ*//*string*/
a.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
a.zadol_begin as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
a.dolg_self_prosr as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
a.nachisl_ym_spo as nachisl_ym_spo, /*Начислено за период*//*number*/
a.opl as opl, /*Оплачено за период*//*number*/
a.nachisl_end as nachisl_end, /*Начислено последний месяй периода*//*number*/
a.nachisl_end_pros as nachisl_end_pros, /*Просрочено на конец периода*//*number*/
 to_char( ( (to_date(to_char( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , 1 ) ,'YYYYMM'))/100 ) *10000+01),'YYYYMMDD')) ) ,'DD.MM.YYYY') as dateend, /*string*/
 to_number(to_char( ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ,'YYYY')) as nyear_beg, /*number*/
 decode ( ( mod( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) , 1)*100 ) ,1,'Январь',2,'Февраль',3,'Март',4,'Апрель',5,'Май',6,'Июнь',7,'Июль',8,'Август',9,'Сентябрь',10,'Октябрь',11,'Ноябрь',12,'Декабрь') as name_ym_1, /*string*/
 to_number(to_char( ( (to_date(to_char( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) *10000+01),'YYYYMMDD')) ) ,'YYYY')) as nyear_beg_1, /*number*/
 decode ( ( mod( 0 , 1)*100 ) ,1,'Января',2,'Февраля',3,'Марта',4,'Апреля',5,'Мая',6,'Июня',7,'Июля',8,'Августа',9,'Сентября',10,'Октября',11,'Ноября',12,'Декабря') as name_ym_beg_genitive, /*string*/
 to_number(to_char( ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ,'YYYY')) as nyear_end, /*number*/
 decode ( ( mod( 0 , 1)*100 ) ,1,'Январь',2,'Февраль',3,'Март',4,'Апрель',5,'Май',6,'Июнь',7,'Июль',8,'Август',9,'Сентябрь',10,'Октябрь',11,'Ноябрь',12,'Декабрь') as name_ym_end, /*string*/
 decode ( ( mod( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , 1 ) ,'YYYYMM'))/100 ) , 1)*100 ) ,1,'Января',2,'Февраля',3,'Марта',4,'Апреля',5,'Мая',6,'Июня',7,'Июля',8,'Августа',9,'Сентября',10,'Октября',11,'Ноября',12,'Декабря') as name_ym_end1_genitive, /*string*/
 to_number(to_char( ( (to_date(to_char( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , 1 ) ,'YYYYMM'))/100 ) *10000+01),'YYYYMMDD')) ) ,'YYYY')) as nyear_end1, /*number*/
 MONTHS_BETWEEN( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) , (to_date(to_char( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) *10000+01),'YYYYMMDD')) ) as kol_ym/*number*/
from (
--
select max(a.rwn1) as rwn1, /*number*/
max(a.name_dep) as name_dep, /*Отделение*//*string*/
max(a.name_uch) as name_uch, /*Участок*//*string*/
max(a.name_gr) as name_gr, /*ИКУ*//*string*/
sum(a.nachisl_do_ymbeg) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
sum(a.zadol_begin) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
sum(a.dolg_self_prosr) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
sum(a.nachisl_ym_spo) as nachisl_ym_spo, /*Начислено за период*//*number*/
sum(a.opl) as opl, /*Оплачено за период*//*number*/
sum(a.nachisl_end) as nachisl_end, /*Начислено последний месяй периода*//*number*/
sum(a.nachisl_end_pros) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--24599
select row_number() over( order by dep.name , kodp_uch.name , kod_gr_potr_nas.name ) as rwn1, /**//*number*/
dog.kod_dog as kod_dog, /*number*//*key*/
dep.name as name_dep, /*Отделение*//*string*/
kodp_uch.name as name_uch, /*Участок*//*string*/
kod_gr_potr_nas.name as name_gr, /*ИКУ*//*string*/
nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) as nachisl_do_ymbeg, /*Начислено за предыдущий месяц*//*number*/
nvl(nachisl_ym_spo.zadol_begin,0) as zadol_begin, /*Дебиторская задолженность на начало периода*//*number*/
nvl(nachisl_ym_spo.dolg_self_prosr,0) as dolg_self_prosr, /*Просрочено на начало периода*//*number*/
nvl(nachisl_ym_spo.nachisl_ym_spo,0) as nachisl_ym_spo, /*Начислено за период*//*number*/
nvl(nachisl_ym_spo.opl,0) as opl, /*Оплачено за период*//*number*/
nvl(nachisl_ym_spo.nachisl_end,0) as nachisl_end, /*Начислено последний месяй периода*//*number*/
nvl(nachisl_ym_spo.nachisl_end_pros,0) as nachisl_end_pros/*Просрочено на конец периода*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog, /*number*//*key*/
a.tep_el as tep_el, /**//*number*/
a.dep as dep, /**//*number*/
a.kodp_uch as kodp_uch, /**//*number*/
rr_refprop_dog_103.kod_refcode as kod_gr_potr_nas/*number*/
from kr_dogovor
a
--\kr_dogovor
left outer join
(
--rr_refprop_dog_103
select a.objid as kod_dog, /*number*//*key*/
max(a.kod_refcode) as kod_refcode/*number*/
from (
--rr_refprop
select a.kod_refprop as kod_refprop, /*number*//*key*/
a.kod_refcode as kod_refcode, /*number*/
a.kod_refobject as kod_refobject, /*number*/
a.objid as objid/**//*number*/
from rr_refprop
a
--\rr_refprop
)
a
--\rr_refprop
left outer join
(
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook/*number*/
from rs_refcode
a
--\rs_refcode
)
kod_refcode on a.kod_refcode = kod_refcode.kod_refcode--\rs_refcode
where
(kod_refcode.kod_refbook = 103) and (a.kod_refobject = 2) group by
a.objid/*number*//*key*/
)
rr_refprop_dog_103 on (a.kod_dog = rr_refprop_dog_103.kod_dog) --\rr_refprop_dog_103
)
dog
--\kr_dogovor
left outer join
(
--24599-dolg-ym-spo
select dog.kod_dog as kod_dog, /*number*//*key*/
sr_facvip.nachisl_do_ymbeg as nachisl_do_ymbeg, /*Начислено*//*number*/
sr_facvip.zadol_begin as zadol_begin, /*Остаток (руб)*//*number*/
sr_facvip.dolg_self_prosr as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sr_facvip.nachisl_ym_spo as nachisl_ym_spo, /*Начислено*//*number*/
sr_facvip.opl as opl, /*number*/
sr_facvip.nachisl_end as nachisl_end, /*Начислено*//*number*/
sr_facvip.nachisl_end_pros as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--kr_dogovor
select a.kod_dog as kod_dog/*number*//*key*/
from kr_dogovor
a
--\kr_dogovor
)
dog
--\kr_dogovor
left outer join
(
--
select sr_facvip.kod_dog as kod_dog, /*number*//*key*/
sum(sf_nach_ym_1.nachisl) as nachisl_do_ymbeg, /*Начислено*//*number*/
sum(sf_beg.ostatok) as zadol_begin, /*Остаток (руб)*//*number*/
sum(sf_beg.dolg_self) as dolg_self_prosr, /*Задолженность (руб)*//*number*/
sum(sf_nach_spo.nachisl) as nachisl_ym_spo, /*Начислено*//*number*/
sum(opl_spo.opl) as opl, /*number*/
sum(sf_nach_end.nachisl) as nachisl_end, /*Начислено*//*number*/
sum(sf_nach_ym_add1.dolg_self) as nachisl_end_pros/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.kod_dog as kod_dog, /*number*/
a.vid_real as vid_real/*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sr_facvip
--\sr_facvip
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 case when (a.dat_bzad <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh8.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh8
--\sr_facras
where
(sr_facras_psh8.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( LAST_DAY(to_date( 0 *100,'YYYYMM'))+(1-1/24/60/60) ) )
sf_nach_ym_add1 on (sr_facvip.kod_sf = sf_nach_ym_add1.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_end.kod_sf as kod_sf, /*number*//*key*/
sf_nach_end.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh4.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh4
--\sr_facras
where
(sr_facras_psh4.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_end
--\sr_facvip
where
sf_nach_end.ym = 0)
sf_nach_end on (sr_facvip.kod_sf = sf_nach_end.kod_sf) --\
left outer join
(
--
select sf_nach_spo.kod_sf as kod_sf, /*number*//*key*/
sf_nach_spo.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh5.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh5
--\sr_facras
where
(sr_facras_psh5.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_spo
--\sr_facvip
where
sf_nach_spo.ym between 0 and 0)
sf_nach_spo on (sr_facvip.kod_sf = sf_nach_spo.kod_sf) --\
left outer join
(
--sr_facvip(date)
select a.kod_sf as kod_sf, /*number*//*key*/
 nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) as ostatok, /*Остаток (руб)*//*number*/
 case when (a.dat_bzad <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) ) then ( nvl( a.nachisl ,0) -nvl( sr_opl.opl ,0) ) end as dolg_self/*Задолженность (руб)*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.dat_sf as dat_sf, /*Дата документа начисления*//*date*/
 coalesce( kod_deb.dat_bzad , a.dat_zadol , a.dat_sf ) as dat_bzad, /*Дата возникновения обязательства по погашению задолженности*//*date*/
(
--
select sum(sr_facras_psh10.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh10
--\sr_facras
where
(sr_facras_psh10.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
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
a
--\sr_facvip
left outer join
(
--
select sr_opl.kod_sf as kod_sf, /*number*//*key*/
sum(sr_opl.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.dat_opl as dat_opl/*Дата платежного документа*//*date*/
from sr_opl
a
--\sr_opl
)
sr_opl
--\sr_opl
where
sr_opl.dat_opl < ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) group by
sr_opl.kod_sf/*number*//*key*/
)
sr_opl on sr_opl.kod_sf = a.kod_sf--\
where
a.dat_sf <= ( (to_date(to_char( 0 *10000+01),'YYYYMMDD')) ) )
sf_beg on (sr_facvip.kod_sf = sf_beg.kod_sf) --\sr_facvip(date)
left outer join
(
--
select sf_nach_ym_1.kod_sf as kod_sf, /*number*//*key*/
sf_nach_ym_1.nachisl as nachisl/*Начислено*//*number*/
from (
--sr_facvip
select a.kod_sf as kod_sf, /*number*//*key*/
a.ym as ym, /*Отчетный период начисления*//*number*/
(
--
select sum(sr_facras_psh6.nachisl) as nachisl/*Начислено*//*number*/
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
sr_facras_psh6
--\sr_facras
where
(sr_facras_psh6.kod_sf = a.kod_sf) )
--\
 as nachisl/*Начислено*//*number*/
from sr_facvip
a
--\sr_facvip
where
a.vid_sf not in (2 , 9) )
sf_nach_ym_1
--\sr_facvip
where
sf_nach_ym_1.ym = ( to_number(to_char(ADD_MONTHS( (to_date(to_char( 0 *10000+01),'YYYYMMDD') ) , -1 ) ,'YYYYMM'))/100 ) )
sf_nach_ym_1 on (sr_facvip.kod_sf = sf_nach_ym_1.kod_sf) --\
left outer join
(
--
select opl_spo.kod_sf as kod_sf, /*number*//*key*/
sum(opl_spo.opl) as opl/*number*/
from (
--sr_opl
select a.kod_opl as kod_opl, /*number*//*key*/
a.kod_sf as kod_sf, /*number*/
a.opl as opl, /**//*number*/
a.ym as ym/*Период оплаты*//*number*/
from sr_opl
a
--\sr_opl
)
opl_spo
--\sr_opl
where
opl_spo.ym between 0 and 0 group by
opl_spo.kod_sf/*number*//*key*/
)
opl_spo on opl_spo.kod_sf = sr_facvip.kod_sf--\
where
sr_facvip.vid_real = 2 group by
sr_facvip.kod_dog/*number*//*key*/
)
sr_facvip on sr_facvip.kod_dog = dog.kod_dog--\
)
nachisl_ym_spo on (dog.kod_dog = nachisl_ym_spo.kod_dog) --\24599-dolg-ym-spo
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
kodp_uch on dog.kodp_uch = kodp_uch.kodp--\kr_org
left outer join
(
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
dep on dog.dep = dep.kodp--\kr_org
left outer join
(
--p_gr_potr_nas
select a.kod_refcode as kod_gr_potr_nas, /*number*//*key*/
a.name as name, /*Наименование*//*string*/
 case when (a.kod_refcode in (356 , 359 , 354 , 355) ) then 1 else ( case when (a.kod_refcode in (363 , 364 , 361 , 362) ) then 2 else 0 end ) end as pr_iku_rso/*number*/
from (
--rs_refcode
select a.kod_refcode as kod_refcode, /*number*//*key*/
a.kod_refbook as kod_refbook, /*number*/
a.name as name/*Наименование*//*string*/
from rs_refcode
a
--\rs_refcode
)
a
--\rs_refcode
where
a.kod_refbook = 103)
kod_gr_potr_nas on dog.kod_gr_potr_nas = kod_gr_potr_nas.kod_gr_potr_nas--\p_gr_potr_nas
where
(dog.tep_el = 1)  and (dog.dep in (0))   and (dog.kodp_uch in (0))  and ( nvl( nvl(nachisl_ym_spo.nachisl_do_ymbeg,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.zadol_begin,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.dolg_self_prosr,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_ym_spo,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.opl,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end,0) ,0)!=0 or nvl( nvl(nachisl_ym_spo.nachisl_end_pros,0) ,0)!=0 ) and (kod_gr_potr_nas.pr_iku_rso = 1) )
a
--\24599
)
a
--\
) mtr
)
loop
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
n1, --rwn1
s1, --name_dep
s2, --name_uch
s3, --name_gr
n2, --nachisl_do_ymbeg
n3, --zadol_begin
n4, --dolg_self_prosr
n5, --nachisl_ym_spo
n6, --opl
n7, --nachisl_end
n8, --nachisl_end_pros
s4, --dateend
n9, --nyear_beg
s5, --name_ym_1
n10, --nyear_beg_1
s6, --name_ym_beg_genitive
n11, --nyear_end
s7, --name_ym_end
s8, --name_ym_end1_genitive
n12, --nyear_end1
n13--kol_ym
)
values (
rec.skod, 
rec.sid, 
rec.rn, 
rec.rwn1, 
rec.name_dep, 
rec.name_uch, 
rec.name_gr, 
rec.nachisl_do_ymbeg, 
rec.zadol_begin, 
rec.dolg_self_prosr, 
rec.nachisl_ym_spo, 
rec.opl, 
rec.nachisl_end, 
rec.nachisl_end_pros, 
rec.dateend, 
rec.nyear_beg, 
rec.name_ym_1, 
rec.nyear_beg_1, 
rec.name_ym_beg_genitive, 
rec.nyear_end, 
rec.name_ym_end, 
rec.name_ym_end1_genitive, 
rec.nyear_end1, 
rec.kol_ym);
end loop;
end;
--\24599-itogo
end;