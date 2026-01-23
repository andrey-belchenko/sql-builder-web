--ipr_fin_body_united_test2 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.year as year, /*number*//*key*/ 
a.period as period, /*number*//*key*/ 
a.ym as ym, /*number*//*key*/ 
a.max_ym as max_ym, /*number*/ 
a.min_ym as min_ym, /*number*/ 
a.kodzatrat as kodzatrat, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_main_titul as kod_main_titul, /*number*//*key*/ 
a.kod_titul_ip_sb as kod_titul_ip_sb, /*number*//*key*/ 
a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*//*key*/ 
a.kod_dog_i as kod_dog_i, /*string*//*key*/ 
a.kod_smet as kod_smet, /*number*//*key*/ 
a.pr_last_smet as pr_last_smet, /*key*/ 
a.kap_sum as kap_sum, /*Плановый объем освоения без НДС*//*number*/ 
a.kap_sum_nds as kap_sum_nds, /*Плановый объем освоения с НДС*//*number*/ 
a.kap_sum_wait as kap_sum_wait, /*number*/ 
a.kap_sum_nds_wait as kap_sum_nds_wait, /*number*/ 
a.fp_sum as fp_sum, /*Плановый объем финансирования без НДС*//*number*/ 
a.fp_sum_nds as fp_sum_nds, /*Плановый объем финансирования с НДС*//*number*/ 
a.fp_sum_wait as fp_sum_wait, /*number*/ 
a.fp_sum_nds_wait as fp_sum_nds_wait, /*number*/ 
a.god_vvod as god_vvod, /*Год ввода в эксплуатацию*//*number*/ 
a.vvod_sum as vvod_sum, /*Плановый ввод в основные фонды без НДС*//*number*/ 
a.vvod_sum_nds as vvod_sum_nds, /*number*/ 
a.vvod_sum_wait as vvod_sum_wait, /*number*/ 
a.vvod_sum_nds_wait as vvod_sum_nds_wait, /*number*/ 
a.pow_km as pow_km, /*Ввод КМ ВЛ план*//*number*/ 
a.pow_mba as pow_mba, /*Ввод МВА план*//*number*/ 
a.pow_cnt as pow_cnt, /*Ввод шт план*//*number*/ 
a.pow_km_del as pow_km_del, /*number*/ 
a.pow_mba_del as pow_mba_del, /*number*/ 
a.pow_km_pr as pow_km_pr, /*Прирост км ВЛ план*//*number*/ 
a.pow_mba_pr as pow_mba_pr, /*Прирост МВА план*//*number*/ 
a.pow_cnt_pr as pow_cnt_pr, /*Прирост шт план*//*number*/ 
a.pow_mba_fact as pow_mba_fact, /*Ввод МВА факт*//*number*/ 
a.pow_km_fact as pow_km_fact, /*Ввод КМ ВЛ факт*//*number*/ 
a.pow_cnt_fact as pow_cnt_fact, /*Ввод шт факт*//*number*/ 
a.pow_km_fact_pr as pow_km_fact_pr, /*Прирост КМ ВЛ факт*//*number*/ 
a.pow_mba_fact_pr as pow_mba_fact_pr, /*Прирост МВА факт*//*number*/ 
a.pow_cnt_fact_pr as pow_cnt_fact_pr, /*Прирост шт*//*number*/ 
a.pow_mba_wait as pow_mba_wait, /*number*/ 
a.pow_km_wait as pow_km_wait, /*number*/ 
a.pow_cnt_wait as pow_cnt_wait, /*number*/ 
a.pow_cnt_proch_wait as pow_cnt_proch_wait, /*number*/ 
a.pow_km_wait_pr as pow_km_wait_pr, /*number*/ 
a.pow_mba_wait_pr as pow_mba_wait_pr, /*number*/ 
a.pow_cnt_wait_pr as pow_cnt_wait_pr, /*number*/ 
a.pow_cnt_proch_wait_pr as pow_cnt_proch_wait_pr, /*number*/ 
a.pow_cnt_proch as pow_cnt_proch, /*number*/ 
a.pow_cnt_proch_pr as pow_cnt_proch_pr, /*number*/ 
a.pow_cnt_proch_fact as pow_cnt_proch_fact, /*number*/ 
a.pow_cnt_proch_fact_pr as pow_cnt_proch_fact_pr, /*number*/ 
a.pow_km_kl as pow_km_kl, /*Ввод КЛ км план*//*number*/ 
a.pow_km_kl_del as pow_km_kl_del, /*number*/ 
a.pow_km_kl_fact as pow_km_kl_fact, /*Ввод КЛ км факт*//*number*/ 
a.pow_km_kl_ekspl as pow_km_kl_ekspl, /*number*/ 
a.pow_km_kl_pr as pow_km_kl_pr, /*Прирост КЛ км план*//*number*/ 
a.pow_km_kl_fact_pr as pow_km_kl_fact_pr, /*Прирост КЛ км факт*//*number*/ 
a.pow_km_kl_wait as pow_km_kl_wait, /*number*/ 
a.pow_km_kl_wait_pr as pow_km_kl_wait_pr, /*number*/ 
a.pow_any as pow_any, /*Ввод мва план*//*number*/ 
a.pow_any_fact as pow_any_fact, /*Ввод мва факт*//*number*/ 
a.pow_km_itog as pow_km_itog, /*Ввод КМ ВЛ план*//*number*/ 
a.pow_mba_itog as pow_mba_itog, /*Ввод МВА план*//*number*/ 
a.pow_cnt_itog as pow_cnt_itog, /*Ввод шт план*//*number*/ 
a.pow_km_del_itog as pow_km_del_itog, /*number*/ 
a.pow_mba_del_itog as pow_mba_del_itog, /*number*/ 
a.pow_km_pr_itog as pow_km_pr_itog, /*Прирост км ВЛ план*//*number*/ 
a.pow_mba_pr_itog as pow_mba_pr_itog, /*Прирост МВА план*//*number*/ 
a.pow_cnt_pr_itog as pow_cnt_pr_itog, /*Прирост шт план*//*number*/ 
a.pow_mba_fact_itog as pow_mba_fact_itog, /*Ввод МВА факт*//*number*/ 
a.pow_km_fact_itog as pow_km_fact_itog, /*Ввод КМ ВЛ факт*//*number*/ 
a.pow_cnt_fact_itog as pow_cnt_fact_itog, /*Ввод шт факт*//*number*/ 
a.pow_km_fact_pr_itog as pow_km_fact_pr_itog, /*Прирост КМ ВЛ факт*//*number*/ 
a.pow_mba_fact_pr_itog as pow_mba_fact_pr_itog, /*Прирост МВА факт*//*number*/ 
a.pow_cnt_fact_pr_itog as pow_cnt_fact_pr_itog, /*Прирост шт*//*number*/ 
a.pow_mba_wait_itog as pow_mba_wait_itog, /*number*/ 
a.pow_km_wait_itog as pow_km_wait_itog, /*number*/ 
a.pow_cnt_wait_itog as pow_cnt_wait_itog, /*number*/ 
a.pow_cnt_proch_wait_itog as pow_cnt_proch_wait_itog, /*number*/ 
a.pow_km_wait_pr_itog as pow_km_wait_pr_itog, /*number*/ 
a.pow_mba_wait_pr_itog as pow_mba_wait_pr_itog, /*number*/ 
a.pow_cnt_wait_pr_itog as pow_cnt_wait_pr_itog, /*number*/ 
a.pow_cnt_proch_wait_pr_itog as pow_cnt_proch_wait_pr_itog, /*number*/ 
a.pow_cnt_proch_itog as pow_cnt_proch_itog, /*number*/ 
a.pow_cnt_proch_pr_itog as pow_cnt_proch_pr_itog, /*number*/ 
a.pow_cnt_proch_fact_itog as pow_cnt_proch_fact_itog, /*number*/ 
a.pow_cnt_proch_fact_pr_itog as pow_cnt_proch_fact_pr_itog, /*number*/ 
a.pow_km_kl_itog as pow_km_kl_itog, /*Ввод КЛ км план*//*number*/ 
a.pow_km_kl_del_itog as pow_km_kl_del_itog, /*number*/ 
a.pow_km_kl_fact_itog as pow_km_kl_fact_itog, /*Ввод КЛ км факт*//*number*/ 
a.pow_km_kl_ekspl_itog as pow_km_kl_ekspl_itog, /*number*/ 
a.pow_km_kl_pr_itog as pow_km_kl_pr_itog, /*Прирост КЛ км план*//*number*/ 
a.pow_km_kl_fact_pr_itog as pow_km_kl_fact_pr_itog, /*Прирост КЛ км факт*//*number*/ 
a.pow_km_kl_wait_itog as pow_km_kl_wait_itog, /*number*/ 
a.pow_km_kl_wait_pr_itog as pow_km_kl_wait_pr_itog, /*number*/ 
a.pow_any_itog as pow_any_itog, /*Ввод мва план*//*number*/ 
a.pow_any_fact_itog as pow_any_fact_itog, /*Ввод мва факт*//*number*/ 
a.kap_sum_nds_fact_a as kap_sum_nds_fact_a, /*Фактический объем освоения c НДС (без перебросок)*//*number*/ 
a.kap_nds_fact_a as kap_nds_fact_a, /*Фактический объем освоения НДС(без перебросок)*//*number*/ 
a.kap_sum_fact_per as kap_sum_fact_per, /*Фактический объем освоения без НДС (передано)*//*number*/ 
a.kap_nds_fact_per as kap_nds_fact_per, /*Фактический объем освоения НДС (передано)*//*number*/ 
a.kap_sum_fact_per_pr as kap_sum_fact_per_pr, /*Фактический объем освоения прочие без НДС (передано)*//*number*/ 
a.kap_nds_fact_per_pr as kap_nds_fact_per_pr, /*Фактический объем освоения  прочие  НДС (передано)*//*number*/ 
a.kap_sum_fact_sp as kap_sum_fact_sp, /*Фактический объем освоения без НДС (списано)*//*number*/ 
a.kap_nds_fact_sp as kap_nds_fact_sp, /*Фактический объем освоения НДС (списано)*//*number*/ 
a.kap_sum_fact_sp_pr as kap_sum_fact_sp_pr, /*Фактический объем освоения прочие без НДС (списано)*//*number*/ 
a.kap_nds_fact_sp_pr as kap_nds_fact_sp_pr, /*Фактический объем освоения  прочие  НДС (списано)*//*number*/ 
a.kap_sum_fact_pri as kap_sum_fact_pri, /*Фактический объем освоения без НДС (принято)*//*number*/ 
a.kap_nds_fact_pri as kap_nds_fact_pri, /*Фактический объем освоения НДС (принято)*//*number*/ 
a.kap_sum_fact_pri_pr as kap_sum_fact_pri_pr, /*Фактический объем освоения прочие без НДС (принято)*//*number*/ 
a.kap_nds_fact_pri_pr as kap_nds_fact_pri_pr, /*Фактический объем освоения прочие НДС (принято)*//*number*/ 
a.fp_fact_sum as fp_fact_sum, /*number*/ 
a.fp_fact_sum_pr as fp_fact_sum_pr, /*Сумма с НДС*//*number*/ 
a.fp_fact_nds as fp_fact_nds, /*number*/ 
a.fp_fact_nds_pr as fp_fact_nds_pr, /*НДС*//*number*/ 
a.fp_sum_fact_per1 as fp_sum_fact_per1, /*number*/ 
a.fp_nds_fact_per1 as fp_nds_fact_per1, /*number*/ 
a.fp_sum_fact_sp1 as fp_sum_fact_sp1, /*number*/ 
a.fp_nds_fact_sp1 as fp_nds_fact_sp1, /*number*/ 
a.fp_sum_fact_pri1 as fp_sum_fact_pri1, /*number*/ 
a.fp_nds_fact_pri1 as fp_nds_fact_pri1, /*number*/ 
a.vvod_sum_fact as vvod_sum_fact, /*Фактический ввод в без НДС*//*number*/ 
a.km as km, /*Протяженность ВЛ, км*//*number*/ 
a.km_kl as km_kl, /*Протяженность КЛ, км*//*number*/ 
a.pow as pow, /*Мощность, МВА*//*number*/ 
a.other as other, /*number*/ 
a.ipr_fin_ipr as ipr_fin_ipr, /*number*/ 
a.summ_s as summ_s, /*Утверждённая сметная стоимость строительства объекта*//*number*/ 
a.sum_psd as sum_psd, /*number*/ 
a.sum_utvpsd as sum_utvpsd, /*number*/ 
a.sum_usr as sum_usr, /*number*/ 
a.summ_smet_tek as summ_smet_tek, /*number*/ 
a.summ_nds_usr as summ_nds_usr, /*Сметная стоимость по УРС с  НДС*//*number*/ 
a.summ_10_nds_usr as summ_10_nds_usr, /*Сметная стоимость по УРС -10% с НДС*//*number*/ 
a.summ_30_nds_usr as summ_30_nds_usr, /*Сметная стоимость по УРС -30% с НДС*//*number*/ 
a.summ_usr as summ_usr, /*Сметная стоимость по УРС без НДС*//*number*/ 
a.summ_10_usr as summ_10_usr, /*Сметная стоимость по УРС -10% без НДС*//*number*/ 
a.summ_30_usr as summ_30_usr, /*Сметная стоимость по УРС -30% без НДС*//*number*/ 
a.summ_nds_psd as summ_nds_psd, /*Сметная стоимость по ПСД с НДС*//*number*/ 
a.summ_nds_psd_10 as summ_nds_psd_10, /*Сметная стоимость по ПСД -10% с НДС*//*number*/ 
a.summ_nds_psd_30 as summ_nds_psd_30, /*Сметная стоимость по ПСД -30% с НДС*//*number*/ 
a.summ_psd as summ_psd, /*Сметная стоимость по ПСД без НДС*//*number*/ 
a.summ_psd_10 as summ_psd_10, /*Сметная стоимость по ПСД -10% без НДС*//*number*/ 
a.summ_psd_30 as summ_psd_30, /*Сметная стоимость по ПСД -30% без НДС*//*number*/ 
a.summ_nds_utvpsd as summ_nds_utvpsd, /*Утвержденная сметная стоимость по ПСД с НДС*//*number*/ 
a.summ_utvpsd as summ_utvpsd, /*Утвержденная сметная стоимость по ПСД без НДС*//*number*/ 
a.mva_psd_30 as mva_psd_30, /*number*/ 
a.mva_30_usr as mva_30_usr, /*number*/ 
a.kl_km_psd_30 as kl_km_psd_30, /*number*/ 
a.kl_km_30_usr as kl_km_30_usr, /*number*/ 
a.vl_km_psd_30 as vl_km_psd_30, /*number*/ 
a.vl_km_30_usr as vl_km_30_usr, /*number*/ 
a.prch_psd_30 as prch_psd_30, /*number*/ 
a.prch_30_usr as prch_30_usr, /*number*/ 
a.summ_nds_usr_tek as summ_nds_usr_tek, /*Сметная стоимость по УРС с  НДС (тек.)*//*number*/ 
a.summ_10_nds_usr_tek as summ_10_nds_usr_tek, /*Сметная стоимость по УРС -10% с НДС (тек.)*//*number*/ 
a.summ_30_nds_usr_tek as summ_30_nds_usr_tek, /*Сметная стоимость по УРС -30% с НДС (тек.)*//*number*/ 
a.summ_usr_tek as summ_usr_tek, /*Сметная стоимость по УРС без НДС (тек.)*//*number*/ 
a.summ_10_usr_tek as summ_10_usr_tek, /*Сметная стоимость по УРС -10% без НДС (тек.)*//*number*/ 
a.summ_30_usr_tek as summ_30_usr_tek, /*Сметная стоимость по УРС -30% без НДС (тек.)*//*number*/ 
a.summ_nds_psd_tek as summ_nds_psd_tek, /*Сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
a.summ_nds_psd_10_tek as summ_nds_psd_10_tek, /*Сметная стоимость по ПСД -10% с НДС (тек.)*//*number*/ 
a.summ_nds_psd_30_tek as summ_nds_psd_30_tek, /*Сметная стоимость по ПСД -30% с НДС (тек.)*//*number*/ 
a.summ_psd_tek as summ_psd_tek, /*Сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
a.summ_psd_10_tek as summ_psd_10_tek, /*Сметная стоимость по ПСД -10% без НДС (тек.)*//*number*/ 
a.summ_psd_30_tek as summ_psd_30_tek, /*Сметная стоимость по ПСД -30% без НДС (тек.)*//*number*/ 
a.summ_nds_utvpsd_tek as summ_nds_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
a.summ_utvpsd_tek as summ_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
a.mva_psd_30_tek as mva_psd_30_tek, /*number*/ 
a.mva_30_usr_tek as mva_30_usr_tek, /*number*/ 
a.kl_km_psd_30_tek as kl_km_psd_30_tek, /*number*/ 
a.kl_km_30_usr_tek as kl_km_30_usr_tek, /*number*/ 
a.vl_km_psd_30_tek as vl_km_psd_30_tek, /*number*/ 
a.vl_km_30_usr_tek as vl_km_30_usr_tek, /*number*/ 
a.prch_psd_30_tek as prch_psd_30_tek, /*number*/ 
a.prch_30_usr_tek as prch_30_usr_tek, /*number*/ 
a.dz as dz, /*Заданная начальная дебиторская задолженность*//*number*/ 
a.kz as kz, /*Заданная начальная кредиторская задолженность*//*number*/ 
a.nzs_saldo as nzs_saldo, /*Заданное НЗС*//*number*/ 
a.fp_sum_nds_pos as fp_sum_nds_pos, /*Плановый объем финансирования с НДС*//*number*/ 
a.kap_sum_nds_pos as kap_sum_nds_pos, /*Плановый объем освоения с НДС*//*number*/ 
a.kap_sum_pos as kap_sum_pos, /*Плановый объем освоения без НДС*//*number*/ 
a.vvod_sum_pos as vvod_sum_pos, /*Плановый ввод в основные фонды без НДС*//*number*/ 
a.dog_plan_cost as dog_plan_cost, /*Сумма договора*//*number*/ 
a.dog_plan_cost_nds as dog_plan_cost_nds, /*Сумма договора с НДС*//*number*/ 
a.ipr_fin_doc as ipr_fin_doc, /*number*/ 
a.eksp_norm_podst as eksp_norm_podst, /*Нормативный срок службы, лет*//*number*/ 
a.eksp_norm_lin as eksp_norm_lin, /*Нормативный срок службы, лет*//*number*/ 
a.phis_coltr as phis_coltr, /*Количествосиловых трансформаторов, шт*//*number*/ 
a.phis_numtr as phis_numtr, /*Марка силовых трансформаторов*//*string*/ 
a.phis_mvatr as phis_mvatr, /*Мощность, МВА*//*number*/ 
a.phis_typel as phis_typel, /*Тип опор*//*string*/ 
a.phis_numl as phis_numl, /*Марка кабеля*//*string*/ 
a.phis_km_vl as phis_km_vl, /*ВЛ,км*//*number*/ 
a.phis_km_kl as phis_km_kl, /*КЛ,км*//*number*/ 
a.phis_other as phis_other, /*Иные объекты (др. единицы измерений)*//*number*/ 
a.kap_lim_sum as kap_lim_sum, /*Лимит освоения*//*number*/ 
a.fp_lim_sum_nds as fp_lim_sum_nds, /*Лимит финансирования*//*number*/ 
a.vvod_lim_sum as vvod_lim_sum/*Лимит ввод в ОФ*//*number*/ 
 
from ( 
--ipr_fin_body_united 
select ovr1.kod_ipr as kod_ipr, /*number*/ 
ovr1.year as year, /*number*/ 
ovr1.period as period, /*number*/ 
ovr1.ym as ym, /*number*/ 
max(ovr1.max_ym)  as max_ym, /*number*/ 
max(ovr1.min_ym)  as min_ym, /*number*/ 
ovr1.kodzatrat as kodzatrat, /*number*/ 
ovr1.kod_titul_ip as kod_titul_ip, /*number*/ 
ovr1.kod_main_titul as kod_main_titul, /*number*/ 
ovr1.kod_titul_ip_sb as kod_titul_ip_sb, /*number*/ 
ovr1.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ovr1.kod_dog as kod_dog, /*number*/ 
ovr1.kod_dog_i as kod_dog_i, /*string*/ 
ovr1.kod_smet as kod_smet, /*number*/ 
ovr1.pr_last_smet as pr_last_smet,  
sum(ovr1.kap_sum)  as kap_sum, /*Плановый объем освоения без НДС*//*number*/ 
sum(ovr1.kap_sum_nds)  as kap_sum_nds, /*Плановый объем освоения с НДС*//*number*/ 
sum(ovr1.kap_sum_wait)  as kap_sum_wait, /*number*/ 
sum(ovr1.kap_sum_nds_wait)  as kap_sum_nds_wait, /*number*/ 
sum(ovr1.fp_sum)  as fp_sum, /*Плановый объем финансирования без НДС*//*number*/ 
sum(ovr1.fp_sum_nds)  as fp_sum_nds, /*Плановый объем финансирования с НДС*//*number*/ 
sum(ovr1.fp_sum_wait)  as fp_sum_wait, /*number*/ 
sum(ovr1.fp_sum_nds_wait)  as fp_sum_nds_wait, /*number*/ 
max(ovr1.god_vvod)  as god_vvod, /*Год ввода в эксплуатацию*//*number*/ 
sum(ovr1.vvod_sum)  as vvod_sum, /*Плановый ввод в основные фонды без НДС*//*number*/ 
sum(ovr1.vvod_sum_nds)  as vvod_sum_nds, /*number*/ 
sum(ovr1.vvod_sum_wait)  as vvod_sum_wait, /*number*/ 
sum(ovr1.vvod_sum_nds_wait)  as vvod_sum_nds_wait, /*number*/ 
sum(ovr1.pow_km)  as pow_km, /*Ввод КМ ВЛ план*//*number*/ 
sum(ovr1.pow_mba)  as pow_mba, /*Ввод МВА план*//*number*/ 
sum(ovr1.pow_cnt)  as pow_cnt, /*Ввод шт план*//*number*/ 
sum(ovr1.pow_km_del)  as pow_km_del, /*number*/ 
sum(ovr1.pow_mba_del)  as pow_mba_del, /*number*/ 
sum(ovr1.pow_km_pr)  as pow_km_pr, /*Прирост км ВЛ план*//*number*/ 
sum(ovr1.pow_mba_pr)  as pow_mba_pr, /*Прирост МВА план*//*number*/ 
sum(ovr1.pow_cnt_pr)  as pow_cnt_pr, /*Прирост шт план*//*number*/ 
sum(ovr1.pow_mba_fact)  as pow_mba_fact, /*Ввод МВА факт*//*number*/ 
sum(ovr1.pow_km_fact)  as pow_km_fact, /*Ввод КМ ВЛ факт*//*number*/ 
sum(ovr1.pow_cnt_fact)  as pow_cnt_fact, /*Ввод шт факт*//*number*/ 
sum(ovr1.pow_km_fact_pr)  as pow_km_fact_pr, /*Прирост КМ ВЛ факт*//*number*/ 
sum(ovr1.pow_mba_fact_pr)  as pow_mba_fact_pr, /*Прирост МВА факт*//*number*/ 
sum(ovr1.pow_cnt_fact_pr)  as pow_cnt_fact_pr, /*Прирост шт*//*number*/ 
sum(ovr1.pow_mba_wait)  as pow_mba_wait, /*number*/ 
sum(ovr1.pow_km_wait)  as pow_km_wait, /*number*/ 
sum(ovr1.pow_cnt_wait)  as pow_cnt_wait, /*number*/ 
sum(ovr1.pow_cnt_proch_wait)  as pow_cnt_proch_wait, /*number*/ 
sum(ovr1.pow_km_wait_pr)  as pow_km_wait_pr, /*number*/ 
sum(ovr1.pow_mba_wait_pr)  as pow_mba_wait_pr, /*number*/ 
sum(ovr1.pow_cnt_wait_pr)  as pow_cnt_wait_pr, /*number*/ 
sum(ovr1.pow_cnt_proch_wait_pr)  as pow_cnt_proch_wait_pr, /*number*/ 
sum(ovr1.pow_cnt_proch)  as pow_cnt_proch, /*number*/ 
sum(ovr1.pow_cnt_proch_pr)  as pow_cnt_proch_pr, /*number*/ 
sum(ovr1.pow_cnt_proch_fact)  as pow_cnt_proch_fact, /*number*/ 
sum(ovr1.pow_cnt_proch_fact_pr)  as pow_cnt_proch_fact_pr, /*number*/ 
sum(ovr1.pow_km_kl)  as pow_km_kl, /*Ввод КЛ км план*//*number*/ 
sum(ovr1.pow_km_kl_del)  as pow_km_kl_del, /*number*/ 
sum(ovr1.pow_km_kl_fact)  as pow_km_kl_fact, /*Ввод КЛ км факт*//*number*/ 
sum(ovr1.pow_km_kl_ekspl)  as pow_km_kl_ekspl, /*number*/ 
sum(ovr1.pow_km_kl_pr)  as pow_km_kl_pr, /*Прирост КЛ км план*//*number*/ 
sum(ovr1.pow_km_kl_fact_pr)  as pow_km_kl_fact_pr, /*Прирост КЛ км факт*//*number*/ 
sum(ovr1.pow_km_kl_wait)  as pow_km_kl_wait, /*number*/ 
sum(ovr1.pow_km_kl_wait_pr)  as pow_km_kl_wait_pr, /*number*/ 
sum(ovr1.pow_any)  as pow_any, /*Ввод мва план*//*number*/ 
sum(ovr1.pow_any_fact)  as pow_any_fact, /*Ввод мва факт*//*number*/ 
sum(ovr1.pow_km_itog)  as pow_km_itog, /*Ввод КМ ВЛ план*//*number*/ 
sum(ovr1.pow_mba_itog)  as pow_mba_itog, /*Ввод МВА план*//*number*/ 
sum(ovr1.pow_cnt_itog)  as pow_cnt_itog, /*Ввод шт план*//*number*/ 
sum(ovr1.pow_km_del_itog)  as pow_km_del_itog, /*number*/ 
sum(ovr1.pow_mba_del_itog)  as pow_mba_del_itog, /*number*/ 
sum(ovr1.pow_km_pr_itog)  as pow_km_pr_itog, /*Прирост км ВЛ план*//*number*/ 
sum(ovr1.pow_mba_pr_itog)  as pow_mba_pr_itog, /*Прирост МВА план*//*number*/ 
sum(ovr1.pow_cnt_pr_itog)  as pow_cnt_pr_itog, /*Прирост шт план*//*number*/ 
sum(ovr1.pow_mba_fact_itog)  as pow_mba_fact_itog, /*Ввод МВА факт*//*number*/ 
sum(ovr1.pow_km_fact_itog)  as pow_km_fact_itog, /*Ввод КМ ВЛ факт*//*number*/ 
sum(ovr1.pow_cnt_fact_itog)  as pow_cnt_fact_itog, /*Ввод шт факт*//*number*/ 
sum(ovr1.pow_km_fact_pr_itog)  as pow_km_fact_pr_itog, /*Прирост КМ ВЛ факт*//*number*/ 
sum(ovr1.pow_mba_fact_pr_itog)  as pow_mba_fact_pr_itog, /*Прирост МВА факт*//*number*/ 
sum(ovr1.pow_cnt_fact_pr_itog)  as pow_cnt_fact_pr_itog, /*Прирост шт*//*number*/ 
sum(ovr1.pow_mba_wait_itog)  as pow_mba_wait_itog, /*number*/ 
sum(ovr1.pow_km_wait_itog)  as pow_km_wait_itog, /*number*/ 
sum(ovr1.pow_cnt_wait_itog)  as pow_cnt_wait_itog, /*number*/ 
sum(ovr1.pow_cnt_proch_wait_itog)  as pow_cnt_proch_wait_itog, /*number*/ 
sum(ovr1.pow_km_wait_pr_itog)  as pow_km_wait_pr_itog, /*number*/ 
sum(ovr1.pow_mba_wait_pr_itog)  as pow_mba_wait_pr_itog, /*number*/ 
sum(ovr1.pow_cnt_wait_pr_itog)  as pow_cnt_wait_pr_itog, /*number*/ 
sum(ovr1.pow_cnt_proch_wait_pr_itog)  as pow_cnt_proch_wait_pr_itog, /*number*/ 
sum(ovr1.pow_cnt_proch_itog)  as pow_cnt_proch_itog, /*number*/ 
sum(ovr1.pow_cnt_proch_pr_itog)  as pow_cnt_proch_pr_itog, /*number*/ 
sum(ovr1.pow_cnt_proch_fact_itog)  as pow_cnt_proch_fact_itog, /*number*/ 
sum(ovr1.pow_cnt_proch_fact_pr_itog)  as pow_cnt_proch_fact_pr_itog, /*number*/ 
sum(ovr1.pow_km_kl_itog)  as pow_km_kl_itog, /*Ввод КЛ км план*//*number*/ 
sum(ovr1.pow_km_kl_del_itog)  as pow_km_kl_del_itog, /*number*/ 
sum(ovr1.pow_km_kl_fact_itog)  as pow_km_kl_fact_itog, /*Ввод КЛ км факт*//*number*/ 
sum(ovr1.pow_km_kl_ekspl_itog)  as pow_km_kl_ekspl_itog, /*number*/ 
sum(ovr1.pow_km_kl_pr_itog)  as pow_km_kl_pr_itog, /*Прирост КЛ км план*//*number*/ 
sum(ovr1.pow_km_kl_fact_pr_itog)  as pow_km_kl_fact_pr_itog, /*Прирост КЛ км факт*//*number*/ 
sum(ovr1.pow_km_kl_wait_itog)  as pow_km_kl_wait_itog, /*number*/ 
sum(ovr1.pow_km_kl_wait_pr_itog)  as pow_km_kl_wait_pr_itog, /*number*/ 
sum(ovr1.pow_any_itog)  as pow_any_itog, /*Ввод мва план*//*number*/ 
sum(ovr1.pow_any_fact_itog)  as pow_any_fact_itog, /*Ввод мва факт*//*number*/ 
sum(ovr1.kap_sum_nds_fact_a)  as kap_sum_nds_fact_a, /*Фактический объем освоения c НДС (без перебросок)*//*number*/ 
sum(ovr1.kap_nds_fact_a)  as kap_nds_fact_a, /*Фактический объем освоения НДС(без перебросок)*//*number*/ 
sum(ovr1.kap_sum_fact_per)  as kap_sum_fact_per, /*Фактический объем освоения без НДС (передано)*//*number*/ 
sum(ovr1.kap_nds_fact_per)  as kap_nds_fact_per, /*Фактический объем освоения НДС (передано)*//*number*/ 
sum(ovr1.kap_sum_fact_per_pr)  as kap_sum_fact_per_pr, /*Фактический объем освоения прочие без НДС (передано)*//*number*/ 
sum(ovr1.kap_nds_fact_per_pr)  as kap_nds_fact_per_pr, /*Фактический объем освоения  прочие  НДС (передано)*//*number*/ 
sum(ovr1.kap_sum_fact_sp)  as kap_sum_fact_sp, /*Фактический объем освоения без НДС (списано)*//*number*/ 
sum(ovr1.kap_nds_fact_sp)  as kap_nds_fact_sp, /*Фактический объем освоения НДС (списано)*//*number*/ 
sum(ovr1.kap_sum_fact_sp_pr)  as kap_sum_fact_sp_pr, /*Фактический объем освоения прочие без НДС (списано)*//*number*/ 
sum(ovr1.kap_nds_fact_sp_pr)  as kap_nds_fact_sp_pr, /*Фактический объем освоения  прочие  НДС (списано)*//*number*/ 
sum(ovr1.kap_sum_fact_pri)  as kap_sum_fact_pri, /*Фактический объем освоения без НДС (принято)*//*number*/ 
sum(ovr1.kap_nds_fact_pri)  as kap_nds_fact_pri, /*Фактический объем освоения НДС (принято)*//*number*/ 
sum(ovr1.kap_sum_fact_pri_pr)  as kap_sum_fact_pri_pr, /*Фактический объем освоения прочие без НДС (принято)*//*number*/ 
sum(ovr1.kap_nds_fact_pri_pr)  as kap_nds_fact_pri_pr, /*Фактический объем освоения прочие НДС (принято)*//*number*/ 
sum(ovr1.fp_fact_sum)  as fp_fact_sum, /*number*/ 
sum(ovr1.fp_fact_sum_pr)  as fp_fact_sum_pr, /*Сумма с НДС*//*number*/ 
sum(ovr1.fp_fact_nds)  as fp_fact_nds, /*number*/ 
sum(ovr1.fp_fact_nds_pr)  as fp_fact_nds_pr, /*НДС*//*number*/ 
sum(ovr1.fp_sum_fact_per1)  as fp_sum_fact_per1, /*number*/ 
sum(ovr1.fp_nds_fact_per1)  as fp_nds_fact_per1, /*number*/ 
sum(ovr1.fp_sum_fact_sp1)  as fp_sum_fact_sp1, /*number*/ 
sum(ovr1.fp_nds_fact_sp1)  as fp_nds_fact_sp1, /*number*/ 
sum(ovr1.fp_sum_fact_pri1)  as fp_sum_fact_pri1, /*number*/ 
sum(ovr1.fp_nds_fact_pri1)  as fp_nds_fact_pri1, /*number*/ 
sum(ovr1.vvod_sum_fact)  as vvod_sum_fact, /*Фактический ввод в без НДС*//*number*/ 
sum(ovr1.km)  as km, /*Протяженность ВЛ, км*//*number*/ 
sum(ovr1.km_kl)  as km_kl, /*Протяженность КЛ, км*//*number*/ 
sum(ovr1.pow)  as pow, /*Мощность, МВА*//*number*/ 
sum(ovr1.other)  as other, /*number*/ 
sum(ovr1.ipr_fin_ipr)  as ipr_fin_ipr, /*number*/ 
sum(ovr1.summ_s)  as summ_s, /*Утверждённая сметная стоимость строительства объекта*//*number*/ 
sum(ovr1.sum_psd)  as sum_psd, /*number*/ 
sum(ovr1.sum_utvpsd)  as sum_utvpsd, /*number*/ 
sum(ovr1.sum_usr)  as sum_usr, /*number*/ 
sum(ovr1.summ_smet_tek)  as summ_smet_tek, /*number*/ 
sum(ovr1.summ_nds_usr)  as summ_nds_usr, /*Сметная стоимость по УРС с  НДС*//*number*/ 
sum(ovr1.summ_10_nds_usr)  as summ_10_nds_usr, /*Сметная стоимость по УРС -10% с НДС*//*number*/ 
sum(ovr1.summ_30_nds_usr)  as summ_30_nds_usr, /*Сметная стоимость по УРС -30% с НДС*//*number*/ 
sum(ovr1.summ_usr)  as summ_usr, /*Сметная стоимость по УРС без НДС*//*number*/ 
sum(ovr1.summ_10_usr)  as summ_10_usr, /*Сметная стоимость по УРС -10% без НДС*//*number*/ 
sum(ovr1.summ_30_usr)  as summ_30_usr, /*Сметная стоимость по УРС -30% без НДС*//*number*/ 
sum(ovr1.summ_nds_psd)  as summ_nds_psd, /*Сметная стоимость по ПСД с НДС*//*number*/ 
sum(ovr1.summ_nds_psd_10)  as summ_nds_psd_10, /*Сметная стоимость по ПСД -10% с НДС*//*number*/ 
sum(ovr1.summ_nds_psd_30)  as summ_nds_psd_30, /*Сметная стоимость по ПСД -30% с НДС*//*number*/ 
sum(ovr1.summ_psd)  as summ_psd, /*Сметная стоимость по ПСД без НДС*//*number*/ 
sum(ovr1.summ_psd_10)  as summ_psd_10, /*Сметная стоимость по ПСД -10% без НДС*//*number*/ 
sum(ovr1.summ_psd_30)  as summ_psd_30, /*Сметная стоимость по ПСД -30% без НДС*//*number*/ 
sum(ovr1.summ_nds_utvpsd)  as summ_nds_utvpsd, /*Утвержденная сметная стоимость по ПСД с НДС*//*number*/ 
sum(ovr1.summ_utvpsd)  as summ_utvpsd, /*Утвержденная сметная стоимость по ПСД без НДС*//*number*/ 
sum(ovr1.mva_psd_30)  as mva_psd_30, /*number*/ 
sum(ovr1.mva_30_usr)  as mva_30_usr, /*number*/ 
sum(ovr1.kl_km_psd_30)  as kl_km_psd_30, /*number*/ 
sum(ovr1.kl_km_30_usr)  as kl_km_30_usr, /*number*/ 
sum(ovr1.vl_km_psd_30)  as vl_km_psd_30, /*number*/ 
sum(ovr1.vl_km_30_usr)  as vl_km_30_usr, /*number*/ 
sum(ovr1.prch_psd_30)  as prch_psd_30, /*number*/ 
sum(ovr1.prch_30_usr)  as prch_30_usr, /*number*/ 
sum(ovr1.summ_nds_usr_tek)  as summ_nds_usr_tek, /*Сметная стоимость по УРС с  НДС (тек.)*//*number*/ 
sum(ovr1.summ_10_nds_usr_tek)  as summ_10_nds_usr_tek, /*Сметная стоимость по УРС -10% с НДС (тек.)*//*number*/ 
sum(ovr1.summ_30_nds_usr_tek)  as summ_30_nds_usr_tek, /*Сметная стоимость по УРС -30% с НДС (тек.)*//*number*/ 
sum(ovr1.summ_usr_tek)  as summ_usr_tek, /*Сметная стоимость по УРС без НДС (тек.)*//*number*/ 
sum(ovr1.summ_10_usr_tek)  as summ_10_usr_tek, /*Сметная стоимость по УРС -10% без НДС (тек.)*//*number*/ 
sum(ovr1.summ_30_usr_tek)  as summ_30_usr_tek, /*Сметная стоимость по УРС -30% без НДС (тек.)*//*number*/ 
sum(ovr1.summ_nds_psd_tek)  as summ_nds_psd_tek, /*Сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
sum(ovr1.summ_nds_psd_10_tek)  as summ_nds_psd_10_tek, /*Сметная стоимость по ПСД -10% с НДС (тек.)*//*number*/ 
sum(ovr1.summ_nds_psd_30_tek)  as summ_nds_psd_30_tek, /*Сметная стоимость по ПСД -30% с НДС (тек.)*//*number*/ 
sum(ovr1.summ_psd_tek)  as summ_psd_tek, /*Сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
sum(ovr1.summ_psd_10_tek)  as summ_psd_10_tek, /*Сметная стоимость по ПСД -10% без НДС (тек.)*//*number*/ 
sum(ovr1.summ_psd_30_tek)  as summ_psd_30_tek, /*Сметная стоимость по ПСД -30% без НДС (тек.)*//*number*/ 
sum(ovr1.summ_nds_utvpsd_tek)  as summ_nds_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
sum(ovr1.summ_utvpsd_tek)  as summ_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
sum(ovr1.mva_psd_30_tek)  as mva_psd_30_tek, /*number*/ 
sum(ovr1.mva_30_usr_tek)  as mva_30_usr_tek, /*number*/ 
sum(ovr1.kl_km_psd_30_tek)  as kl_km_psd_30_tek, /*number*/ 
sum(ovr1.kl_km_30_usr_tek)  as kl_km_30_usr_tek, /*number*/ 
sum(ovr1.vl_km_psd_30_tek)  as vl_km_psd_30_tek, /*number*/ 
sum(ovr1.vl_km_30_usr_tek)  as vl_km_30_usr_tek, /*number*/ 
sum(ovr1.prch_psd_30_tek)  as prch_psd_30_tek, /*number*/ 
sum(ovr1.prch_30_usr_tek)  as prch_30_usr_tek, /*number*/ 
sum(ovr1.dz)  as dz, /*Заданная начальная дебиторская задолженность*//*number*/ 
sum(ovr1.kz)  as kz, /*Заданная начальная кредиторская задолженность*//*number*/ 
sum(ovr1.nzs_saldo)  as nzs_saldo, /*Заданное НЗС*//*number*/ 
sum(        case                  when  ((ovr1.exprzsaldo is  null        )           or  (ovr1.expryear          >=  ovr1.expr5) )           then  ovr1.exprfp_sum_nds        end      )  as fp_sum_nds_pos, /*Плановый объем финансирования с НДС*//*number*/ 
sum(        case                  when  ((ovr1.exprzsaldo is  null        )           or  (ovr1.expryear          >=  ovr1.expr6) )           then  ovr1.exprkap_sum_nds        end      )  as kap_sum_nds_pos, /*Плановый объем освоения с НДС*//*number*/ 
sum(        case                  when  ((ovr1.exprznzs is  null        )           or  (ovr1.expryear          >=  ovr1.expr7) )           then  ovr1.exprkap_sum        end      )  as kap_sum_pos, /*Плановый объем освоения без НДС*//*number*/ 
sum(        case                  when  ((ovr1.exprznzs is  null        )           or  (ovr1.expryear          >=  ovr1.expr8) )           then  ovr1.exprvvod_sum        end      )  as vvod_sum_pos, /*Плановый ввод в основные фонды без НДС*//*number*/ 
sum(ovr1.dog_plan_cost)  as dog_plan_cost, /*Сумма договора*//*number*/ 
sum(ovr1.dog_plan_cost_nds)  as dog_plan_cost_nds, /*Сумма договора с НДС*//*number*/ 
max(ovr1.ipr_fin_doc)  as ipr_fin_doc, /*number*/ 
max(ovr1.eksp_norm_podst)  as eksp_norm_podst, /*Нормативный срок службы, лет*//*number*/ 
max(ovr1.eksp_norm_lin)  as eksp_norm_lin, /*Нормативный срок службы, лет*//*number*/ 
sum(ovr1.phis_coltr)  as phis_coltr, /*Количествосиловых трансформаторов, шт*//*number*/ 
stragg_dist(ovr1.phis_numtr)  as phis_numtr, /*Марка силовых трансформаторов*//*string*/ 
sum(ovr1.phis_mvatr)  as phis_mvatr, /*Мощность, МВА*//*number*/ 
stragg_dist(ovr1.phis_typel)  as phis_typel, /*Тип опор*//*string*/ 
stragg_dist(ovr1.phis_numl)  as phis_numl, /*Марка кабеля*//*string*/ 
sum(ovr1.phis_km_vl)  as phis_km_vl, /*ВЛ,км*//*number*/ 
sum(ovr1.phis_km_kl)  as phis_km_kl, /*КЛ,км*//*number*/ 
sum(ovr1.phis_other)  as phis_other, /*Иные объекты (др. единицы измерений)*//*number*/ 
sum(ovr1.kap_lim_sum)  as kap_lim_sum, /*Лимит освоения*//*number*/ 
sum(ovr1.fp_lim_sum_nds)  as fp_lim_sum_nds, /*Лимит финансирования*//*number*/ 
sum(ovr1.vvod_lim_sum)  as vvod_lim_sum/*Лимит ввод в ОФ*//*number*/ 
 
from ( 
-- 
select backbone.kod_ipr as kod_ipr, /*number*//*key*/ 
backbone.year as year, /*number*//*key*/ 
backbone.period as period, /*number*//*key*/ 
backbone.ym as ym, /*number*//*key*/ 
max(backbone.ym)  as max_ym, /*number*/ 
max(backbone.ym)  as min_ym, /*number*/ 
backbone.kodzatrat as kodzatrat, /*number*//*key*/ 
kod_ipr.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
kod_ipr.kod_main_titul as kod_main_titul, /*number*//*key*/ 
kod_sbor_titul1.kod_titul_ip as kod_titul_ip_sb, /*number*//*key*/ 
backbone.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
backbone.kod_dog as kod_dog, /*number*//*key*/ 
        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end       as kod_dog_i, /*string*//*key*/ 
backbone.kod_smet as kod_smet, /*number*//*key*/ 
backbone.pr_last_smet as pr_last_smet, /*key*/ 
sum(backbone.kap_sum)  as kap_sum, /*Плановый объем освоения без НДС*//*number*/ 
sum(backbone.kap_sum_nds)  as kap_sum_nds, /*Плановый объем освоения с НДС*//*number*/ 
sum(backbone.kap_sum_wait)  as kap_sum_wait, /*number*/ 
sum(backbone.kap_sum_nds_wait)  as kap_sum_nds_wait, /*number*/ 
sum(backbone.fp_sum)  as fp_sum, /*Плановый объем финансирования без НДС*//*number*/ 
sum(backbone.fp_sum_nds)  as fp_sum_nds, /*Плановый объем финансирования с НДС*//*number*/ 
sum(backbone.fp_sum_wait)  as fp_sum_wait, /*number*/ 
sum(backbone.fp_sum_nds_wait)  as fp_sum_nds_wait, /*number*/ 
max(backbone.god_vvod)  as god_vvod, /*Год ввода в эксплуатацию*//*number*/ 
sum(backbone.vvod_sum)  as vvod_sum, /*Плановый ввод в основные фонды без НДС*//*number*/ 
sum(backbone.vvod_sum_nds)  as vvod_sum_nds, /*number*/ 
sum(backbone.vvod_sum_wait)  as vvod_sum_wait, /*number*/ 
sum(backbone.vvod_sum_nds_wait)  as vvod_sum_nds_wait, /*number*/ 
sum(backbone.pow_km)  as pow_km, /*Ввод КМ ВЛ план*//*number*/ 
sum(backbone.pow_mba)  as pow_mba, /*Ввод МВА план*//*number*/ 
sum(backbone.pow_cnt)  as pow_cnt, /*Ввод шт план*//*number*/ 
sum(backbone.pow_km_del)  as pow_km_del, /*number*/ 
sum(backbone.pow_mba_del)  as pow_mba_del, /*number*/ 
sum(backbone.pow_km_pr)  as pow_km_pr, /*Прирост км ВЛ план*//*number*/ 
sum(backbone.pow_mba_pr)  as pow_mba_pr, /*Прирост МВА план*//*number*/ 
sum(backbone.pow_cnt_pr)  as pow_cnt_pr, /*Прирост шт план*//*number*/ 
sum(backbone.pow_mba_fact)  as pow_mba_fact, /*Ввод МВА факт*//*number*/ 
sum(backbone.pow_km_fact)  as pow_km_fact, /*Ввод КМ ВЛ факт*//*number*/ 
sum(backbone.pow_cnt_fact)  as pow_cnt_fact, /*Ввод шт факт*//*number*/ 
sum(backbone.pow_km_fact_pr)  as pow_km_fact_pr, /*Прирост КМ ВЛ факт*//*number*/ 
sum(backbone.pow_mba_fact_pr)  as pow_mba_fact_pr, /*Прирост МВА факт*//*number*/ 
sum(backbone.pow_cnt_fact_pr)  as pow_cnt_fact_pr, /*Прирост шт*//*number*/ 
sum(backbone.pow_mba_wait)  as pow_mba_wait, /*number*/ 
sum(backbone.pow_km_wait)  as pow_km_wait, /*number*/ 
sum(backbone.pow_cnt_wait)  as pow_cnt_wait, /*number*/ 
sum(backbone.pow_cnt_proch_wait)  as pow_cnt_proch_wait, /*number*/ 
sum(backbone.pow_km_wait_pr)  as pow_km_wait_pr, /*number*/ 
sum(backbone.pow_mba_wait_pr)  as pow_mba_wait_pr, /*number*/ 
sum(backbone.pow_cnt_wait_pr)  as pow_cnt_wait_pr, /*number*/ 
sum(backbone.pow_cnt_proch_wait_pr)  as pow_cnt_proch_wait_pr, /*number*/ 
sum(backbone.pow_cnt_proch)  as pow_cnt_proch, /*number*/ 
sum(backbone.pow_cnt_proch_pr)  as pow_cnt_proch_pr, /*number*/ 
sum(backbone.pow_cnt_proch_fact)  as pow_cnt_proch_fact, /*number*/ 
sum(backbone.pow_cnt_proch_fact_pr)  as pow_cnt_proch_fact_pr, /*number*/ 
sum(backbone.pow_km_kl)  as pow_km_kl, /*Ввод КЛ км план*//*number*/ 
sum(backbone.pow_km_kl_del)  as pow_km_kl_del, /*number*/ 
sum(backbone.pow_km_kl_fact)  as pow_km_kl_fact, /*Ввод КЛ км факт*//*number*/ 
sum(backbone.pow_km_kl_ekspl)  as pow_km_kl_ekspl, /*number*/ 
sum(backbone.pow_km_kl_pr)  as pow_km_kl_pr, /*Прирост КЛ км план*//*number*/ 
sum(backbone.pow_km_kl_fact_pr)  as pow_km_kl_fact_pr, /*Прирост КЛ км факт*//*number*/ 
sum(backbone.pow_km_kl_wait)  as pow_km_kl_wait, /*number*/ 
sum(backbone.pow_km_kl_wait_pr)  as pow_km_kl_wait_pr, /*number*/ 
sum(backbone.pow_any)  as pow_any, /*Ввод мва план*//*number*/ 
sum(backbone.pow_any_fact)  as pow_any_fact, /*Ввод мва факт*//*number*/ 
        sum(        sum(backbone.pow_km)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_itog, /*Ввод КМ ВЛ план*//*number*/ 
        sum(        sum(backbone.pow_mba)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_itog, /*Ввод МВА план*//*number*/ 
        sum(        sum(backbone.pow_cnt)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_itog, /*Ввод шт план*//*number*/ 
        sum(        sum(backbone.pow_km_del)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_del_itog, /*number*/ 
        sum(        sum(backbone.pow_mba_del)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_del_itog, /*number*/ 
        sum(        sum(backbone.pow_km_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_pr_itog, /*Прирост км ВЛ план*//*number*/ 
        sum(        sum(backbone.pow_mba_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_pr_itog, /*Прирост МВА план*//*number*/ 
        sum(        sum(backbone.pow_cnt_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_pr_itog, /*Прирост шт план*//*number*/ 
        sum(        sum(backbone.pow_mba_fact)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_fact_itog, /*Ввод МВА факт*//*number*/ 
        sum(        sum(backbone.pow_km_fact)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_fact_itog, /*Ввод КМ ВЛ факт*//*number*/ 
        sum(        sum(backbone.pow_cnt_fact)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_fact_itog, /*Ввод шт факт*//*number*/ 
        sum(        sum(backbone.pow_km_fact_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_fact_pr_itog, /*Прирост КМ ВЛ факт*//*number*/ 
        sum(        sum(backbone.pow_mba_fact_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_fact_pr_itog, /*Прирост МВА факт*//*number*/ 
        sum(        sum(backbone.pow_cnt_fact_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_fact_pr_itog, /*Прирост шт*//*number*/ 
        sum(        sum(backbone.pow_mba_wait)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_wait_itog, /*number*/ 
        sum(        sum(backbone.pow_km_wait)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_wait_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_wait)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_wait_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_proch_wait)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_proch_wait_itog, /*number*/ 
        sum(        sum(backbone.pow_km_wait_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_wait_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_mba_wait_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_mba_wait_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_wait_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_wait_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_proch_wait_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_proch_wait_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_proch)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_proch_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_proch_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_proch_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_proch_fact)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_proch_fact_itog, /*number*/ 
        sum(        sum(backbone.pow_cnt_proch_fact_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_cnt_proch_fact_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_km_kl)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_itog, /*Ввод КЛ км план*//*number*/ 
        sum(        sum(backbone.pow_km_kl_del)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_del_itog, /*number*/ 
        sum(        sum(backbone.pow_km_kl_fact)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_fact_itog, /*Ввод КЛ км факт*//*number*/ 
        sum(        sum(backbone.pow_km_kl_ekspl)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_ekspl_itog, /*number*/ 
        sum(        sum(backbone.pow_km_kl_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_pr_itog, /*Прирост КЛ км план*//*number*/ 
        sum(        sum(backbone.pow_km_kl_fact_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_fact_pr_itog, /*Прирост КЛ км факт*//*number*/ 
        sum(        sum(backbone.pow_km_kl_wait)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_wait_itog, /*number*/ 
        sum(        sum(backbone.pow_km_kl_wait_pr)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_km_kl_wait_pr_itog, /*number*/ 
        sum(        sum(backbone.pow_any)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_any_itog, /*Ввод мва план*//*number*/ 
        sum(        sum(backbone.pow_any_fact)         )                over(         partition by        1          , backbone.kod_ipr          , backbone.kodzatrat          , kod_ipr.kod_titul_ip          , kod_ipr.kod_main_titul          , kod_sbor_titul1.kod_titul_ip          , backbone.kod_ipr_dog          , backbone.kod_dog          , (        case                  when  ((        nullif(        (backbone.kod_dog)           , -1 )      )   is not null        )           then  ('1-'          ||  (backbone.kod_dog) )           else  (        case                  when  ((backbone.kod_ipr_dog)   is not null        )           then  ('2-'          ||  (backbone.kod_ipr_dog) )         end      )         end      )           , backbone.kod_smet          , backbone.pr_last_smet        order by        backbone.ym   nulls first                )       as pow_any_fact_itog, /*Ввод мва факт*//*number*/ 
sum(backbone.kap_sum_nds_fact_a)  as kap_sum_nds_fact_a, /*Фактический объем освоения c НДС (без перебросок)*//*number*/ 
sum(backbone.kap_nds_fact_a)  as kap_nds_fact_a, /*Фактический объем освоения НДС(без перебросок)*//*number*/ 
sum(backbone.kap_sum_fact_per)  as kap_sum_fact_per, /*Фактический объем освоения без НДС (передано)*//*number*/ 
sum(backbone.kap_nds_fact_per)  as kap_nds_fact_per, /*Фактический объем освоения НДС (передано)*//*number*/ 
sum(backbone.kap_sum_fact_per_pr)  as kap_sum_fact_per_pr, /*Фактический объем освоения прочие без НДС (передано)*//*number*/ 
sum(backbone.kap_nds_fact_per_pr)  as kap_nds_fact_per_pr, /*Фактический объем освоения  прочие  НДС (передано)*//*number*/ 
sum(backbone.kap_sum_fact_sp)  as kap_sum_fact_sp, /*Фактический объем освоения без НДС (списано)*//*number*/ 
sum(backbone.kap_nds_fact_sp)  as kap_nds_fact_sp, /*Фактический объем освоения НДС (списано)*//*number*/ 
sum(backbone.kap_sum_fact_sp_pr)  as kap_sum_fact_sp_pr, /*Фактический объем освоения прочие без НДС (списано)*//*number*/ 
sum(backbone.kap_nds_fact_sp_pr)  as kap_nds_fact_sp_pr, /*Фактический объем освоения  прочие  НДС (списано)*//*number*/ 
sum(backbone.kap_sum_fact_pri)  as kap_sum_fact_pri, /*Фактический объем освоения без НДС (принято)*//*number*/ 
sum(backbone.kap_nds_fact_pri)  as kap_nds_fact_pri, /*Фактический объем освоения НДС (принято)*//*number*/ 
sum(backbone.kap_sum_fact_pri_pr)  as kap_sum_fact_pri_pr, /*Фактический объем освоения прочие без НДС (принято)*//*number*/ 
sum(backbone.kap_nds_fact_pri_pr)  as kap_nds_fact_pri_pr, /*Фактический объем освоения прочие НДС (принято)*//*number*/ 
sum(backbone.fp_fact_sum)  as fp_fact_sum, /*number*/ 
sum(backbone.fp_fact_sum_pr)  as fp_fact_sum_pr, /*Сумма с НДС*//*number*/ 
sum(backbone.fp_fact_nds)  as fp_fact_nds, /*number*/ 
sum(backbone.fp_fact_nds_pr)  as fp_fact_nds_pr, /*НДС*//*number*/ 
sum(backbone.fp_sum_fact_per1)  as fp_sum_fact_per1, /*number*/ 
sum(backbone.fp_nds_fact_per1)  as fp_nds_fact_per1, /*number*/ 
sum(backbone.fp_sum_fact_sp1)  as fp_sum_fact_sp1, /*number*/ 
sum(backbone.fp_nds_fact_sp1)  as fp_nds_fact_sp1, /*number*/ 
sum(backbone.fp_sum_fact_pri1)  as fp_sum_fact_pri1, /*number*/ 
sum(backbone.fp_nds_fact_pri1)  as fp_nds_fact_pri1, /*number*/ 
sum(backbone.vvod_sum_fact)  as vvod_sum_fact, /*Фактический ввод в без НДС*//*number*/ 
sum(backbone.km)  as km, /*Протяженность ВЛ, км*//*number*/ 
sum(backbone.km_kl)  as km_kl, /*Протяженность КЛ, км*//*number*/ 
sum(backbone.pow)  as pow, /*Мощность, МВА*//*number*/ 
sum(backbone.other)  as other, /*number*/ 
sum(backbone.ipr_fin_ipr)  as ipr_fin_ipr, /*number*/ 
sum(backbone.summ_s)  as summ_s, /*Утверждённая сметная стоимость строительства объекта*//*number*/ 
sum(backbone.sum_psd)  as sum_psd, /*number*/ 
sum(backbone.sum_utvpsd)  as sum_utvpsd, /*number*/ 
sum(backbone.sum_usr)  as sum_usr, /*number*/ 
sum(backbone.summ_smet_tek)  as summ_smet_tek, /*number*/ 
sum(backbone.summ_nds_usr)  as summ_nds_usr, /*Сметная стоимость по УРС с  НДС*//*number*/ 
sum(backbone.summ_10_nds_usr)  as summ_10_nds_usr, /*Сметная стоимость по УРС -10% с НДС*//*number*/ 
sum(backbone.summ_30_nds_usr)  as summ_30_nds_usr, /*Сметная стоимость по УРС -30% с НДС*//*number*/ 
sum(backbone.summ_usr)  as summ_usr, /*Сметная стоимость по УРС без НДС*//*number*/ 
sum(backbone.summ_10_usr)  as summ_10_usr, /*Сметная стоимость по УРС -10% без НДС*//*number*/ 
sum(backbone.summ_30_usr)  as summ_30_usr, /*Сметная стоимость по УРС -30% без НДС*//*number*/ 
sum(backbone.summ_nds_psd)  as summ_nds_psd, /*Сметная стоимость по ПСД с НДС*//*number*/ 
sum(backbone.summ_nds_psd_10)  as summ_nds_psd_10, /*Сметная стоимость по ПСД -10% с НДС*//*number*/ 
sum(backbone.summ_nds_psd_30)  as summ_nds_psd_30, /*Сметная стоимость по ПСД -30% с НДС*//*number*/ 
sum(backbone.summ_psd)  as summ_psd, /*Сметная стоимость по ПСД без НДС*//*number*/ 
sum(backbone.summ_psd_10)  as summ_psd_10, /*Сметная стоимость по ПСД -10% без НДС*//*number*/ 
sum(backbone.summ_psd_30)  as summ_psd_30, /*Сметная стоимость по ПСД -30% без НДС*//*number*/ 
sum(backbone.summ_nds_utvpsd)  as summ_nds_utvpsd, /*Утвержденная сметная стоимость по ПСД с НДС*//*number*/ 
sum(backbone.summ_utvpsd)  as summ_utvpsd, /*Утвержденная сметная стоимость по ПСД без НДС*//*number*/ 
sum(backbone.mva_psd_30)  as mva_psd_30, /*number*/ 
sum(backbone.mva_30_usr)  as mva_30_usr, /*number*/ 
sum(backbone.kl_km_psd_30)  as kl_km_psd_30, /*number*/ 
sum(backbone.kl_km_30_usr)  as kl_km_30_usr, /*number*/ 
sum(backbone.vl_km_psd_30)  as vl_km_psd_30, /*number*/ 
sum(backbone.vl_km_30_usr)  as vl_km_30_usr, /*number*/ 
sum(backbone.prch_psd_30)  as prch_psd_30, /*number*/ 
sum(backbone.prch_30_usr)  as prch_30_usr, /*number*/ 
sum(backbone.summ_nds_usr_tek)  as summ_nds_usr_tek, /*Сметная стоимость по УРС с  НДС (тек.)*//*number*/ 
sum(backbone.summ_10_nds_usr_tek)  as summ_10_nds_usr_tek, /*Сметная стоимость по УРС -10% с НДС (тек.)*//*number*/ 
sum(backbone.summ_30_nds_usr_tek)  as summ_30_nds_usr_tek, /*Сметная стоимость по УРС -30% с НДС (тек.)*//*number*/ 
sum(backbone.summ_usr_tek)  as summ_usr_tek, /*Сметная стоимость по УРС без НДС (тек.)*//*number*/ 
sum(backbone.summ_10_usr_tek)  as summ_10_usr_tek, /*Сметная стоимость по УРС -10% без НДС (тек.)*//*number*/ 
sum(backbone.summ_30_usr_tek)  as summ_30_usr_tek, /*Сметная стоимость по УРС -30% без НДС (тек.)*//*number*/ 
sum(backbone.summ_nds_psd_tek)  as summ_nds_psd_tek, /*Сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
sum(backbone.summ_nds_psd_10_tek)  as summ_nds_psd_10_tek, /*Сметная стоимость по ПСД -10% с НДС (тек.)*//*number*/ 
sum(backbone.summ_nds_psd_30_tek)  as summ_nds_psd_30_tek, /*Сметная стоимость по ПСД -30% с НДС (тек.)*//*number*/ 
sum(backbone.summ_psd_tek)  as summ_psd_tek, /*Сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
sum(backbone.summ_psd_10_tek)  as summ_psd_10_tek, /*Сметная стоимость по ПСД -10% без НДС (тек.)*//*number*/ 
sum(backbone.summ_psd_30_tek)  as summ_psd_30_tek, /*Сметная стоимость по ПСД -30% без НДС (тек.)*//*number*/ 
sum(backbone.summ_nds_utvpsd_tek)  as summ_nds_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
sum(backbone.summ_utvpsd_tek)  as summ_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
sum(backbone.mva_psd_30_tek)  as mva_psd_30_tek, /*number*/ 
sum(backbone.mva_30_usr_tek)  as mva_30_usr_tek, /*number*/ 
sum(backbone.kl_km_psd_30_tek)  as kl_km_psd_30_tek, /*number*/ 
sum(backbone.kl_km_30_usr_tek)  as kl_km_30_usr_tek, /*number*/ 
sum(backbone.vl_km_psd_30_tek)  as vl_km_psd_30_tek, /*number*/ 
sum(backbone.vl_km_30_usr_tek)  as vl_km_30_usr_tek, /*number*/ 
sum(backbone.prch_psd_30_tek)  as prch_psd_30_tek, /*number*/ 
sum(backbone.prch_30_usr_tek)  as prch_30_usr_tek, /*number*/ 
sum(backbone.dz)  as dz, /*Заданная начальная дебиторская задолженность*//*number*/ 
sum(backbone.kz)  as kz, /*Заданная начальная кредиторская задолженность*//*number*/ 
sum(backbone.nzs_saldo)  as nzs_saldo, /*Заданное НЗС*//*number*/ 
sum(backbone.dog_plan_cost)  as dog_plan_cost, /*Сумма договора*//*number*/ 
sum(backbone.dog_plan_cost_nds)  as dog_plan_cost_nds, /*Сумма договора с НДС*//*number*/ 
max(backbone.ipr_fin_doc)  as ipr_fin_doc, /*number*/ 
max(backbone.eksp_norm_podst)  as eksp_norm_podst, /*Нормативный срок службы, лет*//*number*/ 
max(backbone.eksp_norm_lin)  as eksp_norm_lin, /*Нормативный срок службы, лет*//*number*/ 
sum(backbone.phis_coltr)  as phis_coltr, /*Количествосиловых трансформаторов, шт*//*number*/ 
stragg_dist(backbone.phis_numtr)  as phis_numtr, /*Марка силовых трансформаторов*//*string*/ 
sum(backbone.phis_mvatr)  as phis_mvatr, /*Мощность, МВА*//*number*/ 
stragg_dist(backbone.phis_typel)  as phis_typel, /*Тип опор*//*string*/ 
stragg_dist(backbone.phis_numl)  as phis_numl, /*Марка кабеля*//*string*/ 
sum(backbone.phis_km_vl)  as phis_km_vl, /*ВЛ,км*//*number*/ 
sum(backbone.phis_km_kl)  as phis_km_kl, /*КЛ,км*//*number*/ 
sum(backbone.phis_other)  as phis_other, /*Иные объекты (др. единицы измерений)*//*number*/ 
sum(backbone.kap_lim_sum)  as kap_lim_sum, /*Лимит освоения*//*number*/ 
sum(backbone.fp_lim_sum_nds)  as fp_lim_sum_nds, /*Лимит финансирования*//*number*/ 
sum(backbone.vvod_lim_sum)  as vvod_lim_sum, /*Лимит ввод в ОФ*//*number*/ 
(        sum(        (        case                  when  ((        nvl(        sum(backbone.dz)           , sum(backbone.kz)  )      )  is  null        )           then  null          else  (          nvl( sum(backbone.dz)  ,0)                  -nvl( sum(backbone.kz)  ,0)        )         end      )         )                over(         partition by        backbone.kod_ipr        )      )  as exprzsaldo,  
(        sum(        sum(backbone.nzs_saldo)         )                over(         partition by        backbone.kod_ipr        )      )  as exprznzs,  
backbone.year as expryear, /*key*/ 
max(kod_doc_osn.god_ip)  as expr5,  
sum(backbone.fp_sum_nds)  as exprfp_sum_nds,  
max(kod_doc_osn.god_ip)  as expr6,  
sum(backbone.kap_sum_nds)  as exprkap_sum_nds,  
max(kod_doc_osn.god_ip)  as expr7,  
sum(backbone.kap_sum)  as exprkap_sum,  
max(kod_doc_osn.god_ip)  as expr8,  
sum(backbone.vvod_sum)  as exprvvod_sum 
 
from ( 
( 
( 
-- 
select ipr_fin_kap.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_kap.year as year, /*number*/ 
ipr_fin_kap.period as period, /*number*/ 
ipr_fin_kap.ym as ym, /*number*/ 
ipr_fin_kap.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_kap.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_kap.kod_dog as kod_dog, /*number*/ 
ipr_fin_kap.kod_smet as kod_smet, /*number*/ 
ipr_fin_kap.pr_last_smet as pr_last_smet,  
ipr_fin_kap.plan_summ as kap_sum, /*Плановый объем освоения без НДС*//*number*/ 
ipr_fin_kap.plan_summ_nds as kap_sum_nds, /*Плановый объем освоения с НДС*//*number*/ 
ipr_fin_kap.wfact_summ as kap_sum_wait, /*number*/ 
ipr_fin_kap.wfact_summ_nds as kap_sum_nds_wait, /*number*/ 
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_kap 
select c.kod_ipr_dog as kod_ipr_dog, /*number*/ 
kod_ipr_dog.kod_dog as kod_dog, /*number*/ 
c.kod_ipr as kod_ipr, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vid_zatrat.kodzatrat as kodzatrat, /*number*//*key*/ 
b.year as year, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.plan_summ as plan_summ, /*Плановый объем освоения без НДС*//*number*/ 
a.plan_summ_nds as plan_summ_nds, /*Плановый объем освоения с НДС*//*number*/ 
a.wfact_summ as wfact_summ, /*number*/ 
a.wfact_summ_nds as wfact_summ_nds, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_kapitalvloz_body 
select a.kod_kapitalvloz_body as kod_kapitalvloz_body, /*number*//*key*/ 
a.kod_kapitalvloz_head as kod_kapitalvloz_head, /*number*/ 
a.plan_summ as plan_summ, /*Плановый объем освоения без НДС*//*number*/ 
a.plan_summ_nds as plan_summ_nds, /*Плановый объем освоения с НДС*//*number*/ 
a.period as period, /**//*number*/ 
a.wfact_summ as wfact_summ, /**//*number*/ 
a.wfact_summ_nds as wfact_summ_nds, /**//*number*/ 
          nvl( (head.year          * 100)  ,0)                  +nvl( a.period ,0)         as ym/*number*/ 
 
from ipr_kapitalvloz_body 
a 
--\ipr_kapitalvloz_body 
left outer join 
( 
--ipr_kapitalvloz_head 
select a.kod_kapitalvloz_head as kod_kapitalvloz_head, /*number*//*key*/ 
a.year as year/**//*number*/ 
 
from ipr_kapitalvloz_head 
a 
--\ipr_kapitalvloz_head 
) 
head on a.kod_kapitalvloz_head          =  head.kod_kapitalvloz_head--\ipr_kapitalvloz_head 
) 
a 
--\ipr_kapitalvloz_body 
left outer join 
( 
--ipr_kapitalvloz_head 
select a.kod_kapitalvloz_head as kod_kapitalvloz_head, /*number*//*key*/ 
a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*/ 
a.year as year, /**//*number*/ 
        case                  when  (ipr_v_docs_last.inc is  null        )           then  0          else  1        end       as pr_last/*number*/ 
 
from ipr_kapitalvloz_head 
a 
--\ipr_kapitalvloz_head 
left outer join 
( 
-- 
select ipr_v_docs_last.kod_kapitalvloz_head as kod_kapitalvloz_head, /*number*//*key*/ 
max(ipr_v_docs_last.inc)  as inc/*number*/ 
 
from ( 
--ipr_v_docs_last_okv 
select a.god_ip as god_ip, /**//*number*//*key*/ 
a.kod_kapitalvloz_head as kod_kapitalvloz_head, /*number*/ 
a.inc as inc/**//*number*/ 
 
from ipr_v_docs_last_okv 
a 
--\ipr_v_docs_last_okv 
) 
ipr_v_docs_last 
--\ipr_v_docs_last_okv 
where 
ipr_v_docs_last.inc          =  1 group by 
ipr_v_docs_last.kod_kapitalvloz_head/*number*//*key*/ 
) 
ipr_v_docs_last on ipr_v_docs_last.kod_kapitalvloz_head          =  a.kod_kapitalvloz_head--\ 
) 
b on a.kod_kapitalvloz_head          =  b.kod_kapitalvloz_head--\ipr_kapitalvloz_head 
left outer join 
( 
--ipr_vid_zatrat_titul 
select a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*//*key*/ 
a.kod_vid_zatrat as kod_vid_zatrat, /*number*/ 
a.kod_ipr_dog as kod_ipr_dog, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_vid_zatrat_titul 
a 
--\ipr_vid_zatrat_titul 
) 
c on b.kod_vid_zatrat_titul          =  c.kod_vid_zatrat_titul--\ipr_vid_zatrat_titul 
left outer join 
( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
kod_ipr_dog on c.kod_ipr_dog          =  kod_ipr_dog.kod_ipr_dog--\ipr_dogs 
left outer join 
( 
--ips_vid_zatrat 
select a.kodzatrat as kodzatrat, /**//*number*//*key*/ 
a.kod_smet as kod_smet/*number*/ 
 
from ips_vid_zatrat 
a 
--\ips_vid_zatrat 
) 
kod_vid_zatrat on c.kod_vid_zatrat          =  kod_vid_zatrat.kodzatrat--\ips_vid_zatrat 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vid_zatrat.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
where 
(b.pr_last          =  1)           and  (kod_smet.kod_parent is  null        ) ) 
ipr_fin_kap 
--\ipr_fin_kap 
) 
--\ 
union all 
( 
-- 
select ipr_fin_finplan.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_finplan.year as year, /*number*/ 
ipr_fin_finplan.period as period, /*number*/ 
ipr_fin_finplan.ym as ym, /*number*/ 
ipr_fin_finplan.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_finplan.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_finplan.kod_dog as kod_dog, /*number*/ 
ipr_fin_finplan.kod_smet as kod_smet, /*number*/ 
ipr_fin_finplan.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
ipr_fin_finplan.plan_summ as fp_sum, /*Плановый объем финансирования без НДС*//*number*/ 
ipr_fin_finplan.plan_summ_nds as fp_sum_nds, /*Плановый объем финансирования с НДС*//*number*/ 
ipr_fin_finplan.wfact_summ as fp_sum_wait, /*number*/ 
ipr_fin_finplan.wfact_summ_nds as fp_sum_nds_wait, /*number*/ 
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_finplan 
select c.kod_ipr_dog as kod_ipr_dog, /*number*/ 
kod_ipr_dog.kod_dog as kod_dog, /*number*/ 
c.kod_ipr as kod_ipr, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vid_zatrat.kodzatrat as kodzatrat, /*number*//*key*/ 
b.year as year, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.plan_summ as plan_summ, /*Плановый объем финансирования без НДС*//*number*/ 
a.plan_summ_nds as plan_summ_nds, /*Плановый объем финансирования с НДС*//*number*/ 
a.wfact_summ as wfact_summ, /*number*/ 
a.wfact_summ_nds as wfact_summ_nds, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_finplan_body 
select a.kod_finplan_body as kod_finplan_body, /*number*//*key*/ 
a.kod_finplan_head as kod_finplan_head, /*number*/ 
a.period as period, /**//*number*/ 
a.plan_summ as plan_summ, /*Плановый объем финансирования без НДС*//*number*/ 
a.plan_summ_nds as plan_summ_nds, /*Плановый объем финансирования с НДС*//*number*/ 
a.wfact_summ as wfact_summ, /**//*number*/ 
a.wfact_summ_nds as wfact_summ_nds, /**//*number*/ 
          nvl( (head.year          * 100)  ,0)                  +nvl( a.period ,0)         as ym/*number*/ 
 
from ipr_finplan_body 
a 
--\ipr_finplan_body 
left outer join 
( 
--ipr_finplan_head 
select a.kod_finplan_head as kod_finplan_head, /*number*//*key*/ 
a.year as year/**//*number*/ 
 
from ipr_finplan_head 
a 
--\ipr_finplan_head 
) 
head on a.kod_finplan_head          =  head.kod_finplan_head--\ipr_finplan_head 
) 
a 
--\ipr_finplan_body 
left outer join 
( 
--ipr_finplan_head 
select a.kod_finplan_head as kod_finplan_head, /*number*//*key*/ 
a.year as year, /**//*number*/ 
a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*/ 
        case                  when  (ipr_v_docs_last.inc is  null        )           then  0          else  1        end       as pr_last/*number*/ 
 
from ipr_finplan_head 
a 
--\ipr_finplan_head 
left outer join 
( 
-- 
select ipr_v_docs_last.kod_finplan_head as kod_finplan_head, /*number*//*key*/ 
max(ipr_v_docs_last.inc)  as inc/*number*/ 
 
from ( 
--ipr_v_docs_last_finplan 
select a.god_ip as god_ip, /**//*number*//*key*/ 
a.kod_finplan_head as kod_finplan_head, /*number*/ 
a.inc as inc/**//*number*/ 
 
from ipr_v_docs_last_finplan 
a 
--\ipr_v_docs_last_finplan 
) 
ipr_v_docs_last 
--\ipr_v_docs_last_finplan 
where 
ipr_v_docs_last.inc          =  1 group by 
ipr_v_docs_last.kod_finplan_head/*number*//*key*/ 
) 
ipr_v_docs_last on ipr_v_docs_last.kod_finplan_head          =  a.kod_finplan_head--\ 
) 
b on a.kod_finplan_head          =  b.kod_finplan_head--\ipr_finplan_head 
left outer join 
( 
--ipr_vid_zatrat_titul 
select a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*//*key*/ 
a.kod_vid_zatrat as kod_vid_zatrat, /*number*/ 
a.kod_ipr_dog as kod_ipr_dog, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_vid_zatrat_titul 
a 
--\ipr_vid_zatrat_titul 
) 
c on b.kod_vid_zatrat_titul          =  c.kod_vid_zatrat_titul--\ipr_vid_zatrat_titul 
left outer join 
( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
kod_ipr_dog on c.kod_ipr_dog          =  kod_ipr_dog.kod_ipr_dog--\ipr_dogs 
left outer join 
( 
--ips_vid_zatrat 
select a.kodzatrat as kodzatrat, /**//*number*//*key*/ 
a.kod_smet as kod_smet/*number*/ 
 
from ips_vid_zatrat 
a 
--\ips_vid_zatrat 
) 
kod_vid_zatrat on c.kod_vid_zatrat          =  kod_vid_zatrat.kodzatrat--\ips_vid_zatrat 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vid_zatrat.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
where 
(b.pr_last          =  1)           and  (kod_smet.kod_parent is  null        ) ) 
ipr_fin_finplan 
--\ipr_fin_finplan 
) 
--\ 
union all 
( 
-- 
select ipr_fin_vvod.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_vvod.year as year, /*number*/ 
ipr_fin_vvod.period as period, /*number*/ 
ipr_fin_vvod.ym as ym, /*number*/ 
ipr_fin_vvod.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_vvod.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_vvod.kod_dog as kod_dog, /*number*/ 
ipr_fin_vvod.kod_smet as kod_smet, /*number*/ 
ipr_fin_vvod.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
ipr_fin_vvod.god_vvod as god_vvod, /*Год ввода в эксплуатацию*//*number*/ 
ipr_fin_vvod.plan_summ as vvod_sum, /*Плановый ввод в основные фонды без НДС*//*number*/ 
ipr_fin_vvod.plan_summ_nds as vvod_sum_nds, /*number*/ 
ipr_fin_vvod.wfact_summ as vvod_sum_wait, /*number*/ 
ipr_fin_vvod.wfact_summ_nds as vvod_sum_nds_wait, /*number*/ 
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_vvod 
select c.kod_ipr_dog as kod_ipr_dog, /*number*/ 
kod_ipr_dog.kod_dog as kod_dog, /*number*/ 
c.kod_ipr as kod_ipr, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vid_zatrat.kodzatrat as kodzatrat, /*number*//*key*/ 
b.year as year, /*number*/ 
b.year as god_vvod, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.plan_summ as plan_summ, /*Плановый ввод в основные фонды без НДС*//*number*/ 
a.plan_summ_nds as plan_summ_nds, /*number*/ 
a.wfact_summ as wfact_summ, /*number*/ 
a.wfact_summ_nds as wfact_summ_nds, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_vvod_body 
select a.kod_vvod_body as kod_vvod_body, /*number*//*key*/ 
a.kod_vvod_head as kod_vvod_head, /*number*/ 
a.period as period, /**//*number*/ 
a.plan_summ as plan_summ, /*Плановый ввод в основные фонды без НДС*//*number*/ 
a.plan_summ_nds as plan_summ_nds, /**//*number*/ 
a.wfact_summ as wfact_summ, /**//*number*/ 
a.wfact_summ_nds as wfact_summ_nds, /**//*number*/ 
          nvl( (head.year          * 100)  ,0)                  +nvl( a.period ,0)         as ym/*number*/ 
 
from ipr_vvod_body 
a 
--\ipr_vvod_body 
left outer join 
( 
--ipr_vvod_head 
select a.kod_vvod_head as kod_vvod_head, /*number*//*key*/ 
a.year as year/**//*number*/ 
 
from ipr_vvod_head 
a 
--\ipr_vvod_head 
) 
head on a.kod_vvod_head          =  head.kod_vvod_head--\ipr_vvod_head 
) 
a 
--\ipr_vvod_body 
left outer join 
( 
--ipr_vvod_head 
select a.kod_vvod_head as kod_vvod_head, /*number*//*key*/ 
a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*/ 
a.year as year, /**//*number*/ 
        case                  when  (ipr_v_docs_last.inc is  null        )           then  0          else  1        end       as pr_last/*number*/ 
 
from ipr_vvod_head 
a 
--\ipr_vvod_head 
left outer join 
( 
-- 
select ipr_v_docs_last.kod_vvod_head as kod_vvod_head, /*number*//*key*/ 
max(ipr_v_docs_last.inc)  as inc/*number*/ 
 
from ( 
--ipr_v_docs_last_vvod 
select a.god_ip as god_ip, /**//*number*//*key*/ 
a.kod_vvod_head as kod_vvod_head, /*number*/ 
a.inc as inc/**//*number*/ 
 
from ipr_v_docs_last_vvod 
a 
--\ipr_v_docs_last_vvod 
) 
ipr_v_docs_last 
--\ipr_v_docs_last_vvod 
where 
ipr_v_docs_last.inc          =  1 group by 
ipr_v_docs_last.kod_vvod_head/*number*//*key*/ 
) 
ipr_v_docs_last on ipr_v_docs_last.kod_vvod_head          =  a.kod_vvod_head--\ 
) 
b on a.kod_vvod_head          =  b.kod_vvod_head--\ipr_vvod_head 
left outer join 
( 
--ipr_vid_zatrat_titul 
select a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*//*key*/ 
a.kod_vid_zatrat as kod_vid_zatrat, /*number*/ 
a.kod_ipr_dog as kod_ipr_dog, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_vid_zatrat_titul 
a 
--\ipr_vid_zatrat_titul 
) 
c on b.kod_vid_zatrat_titul          =  c.kod_vid_zatrat_titul--\ipr_vid_zatrat_titul 
left outer join 
( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
kod_ipr_dog on c.kod_ipr_dog          =  kod_ipr_dog.kod_ipr_dog--\ipr_dogs 
left outer join 
( 
--ips_vid_zatrat 
select a.kodzatrat as kodzatrat, /**//*number*//*key*/ 
a.kod_smet as kod_smet/*number*/ 
 
from ips_vid_zatrat 
a 
--\ips_vid_zatrat 
) 
kod_vid_zatrat on c.kod_vid_zatrat          =  kod_vid_zatrat.kodzatrat--\ips_vid_zatrat 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vid_zatrat.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
where 
(b.pr_last          =  1)           and  (kod_smet.kod_parent is  null        ) ) 
ipr_fin_vvod 
--\ipr_fin_vvod 
) 
--\ 
union all 
( 
-- 
select ipr_fin_power.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_power.year as year, /*number*/ 
ipr_fin_power.period as period, /*number*/ 
ipr_fin_power.ym as ym, /*number*/ 
ipr_fin_power.kodzatrat as kodzatrat,  
ipr_fin_power.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_power.kod_dog as kod_dog,  
ipr_fin_power.kod_smet as kod_smet,  
ipr_fin_power.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
ipr_fin_power.km as pow_km, /*Ввод КМ ВЛ план*//*number*/ 
ipr_fin_power.mba as pow_mba, /*Ввод МВА план*//*number*/ 
ipr_fin_power.cnt as pow_cnt, /*Ввод шт план*//*number*/ 
ipr_fin_power.km_del as pow_km_del, /*number*/ 
ipr_fin_power.mba_del as pow_mba_del, /*number*/ 
ipr_fin_power.km_pr as pow_km_pr, /*Прирост км ВЛ план*//*number*/ 
ipr_fin_power.mba_pr as pow_mba_pr, /*Прирост МВА план*//*number*/ 
ipr_fin_power.cnt_pr as pow_cnt_pr, /*Прирост шт план*//*number*/ 
ipr_fin_power.mba_fact as pow_mba_fact, /*Ввод МВА факт*//*number*/ 
ipr_fin_power.km_fact as pow_km_fact, /*Ввод КМ ВЛ факт*//*number*/ 
ipr_fin_power.cnt_fact as pow_cnt_fact, /*Ввод шт факт*//*number*/ 
ipr_fin_power.km_fact_pr as pow_km_fact_pr, /*Прирост КМ ВЛ факт*//*number*/ 
ipr_fin_power.mba_fact_pr as pow_mba_fact_pr, /*Прирост МВА факт*//*number*/ 
ipr_fin_power.cnt_fact_pr as pow_cnt_fact_pr, /*Прирост шт*//*number*/ 
ipr_fin_power.mba_wfact as pow_mba_wait, /*number*/ 
ipr_fin_power.km_wfact as pow_km_wait, /*number*/ 
ipr_fin_power.cnt_wfact as pow_cnt_wait, /*number*/ 
ipr_fin_power.cnt_proch_wfact as pow_cnt_proch_wait, /*number*/ 
ipr_fin_power.km_wfact_pr as pow_km_wait_pr, /*number*/ 
ipr_fin_power.mba_wfact_pr as pow_mba_wait_pr, /*number*/ 
ipr_fin_power.cnt_wfact_pr as pow_cnt_wait_pr, /*number*/ 
ipr_fin_power.cnt_proch_wfact_pr as pow_cnt_proch_wait_pr, /*number*/ 
ipr_fin_power.cnt_proch as pow_cnt_proch, /*number*/ 
ipr_fin_power.cnt_proch_pr as pow_cnt_proch_pr, /*number*/ 
ipr_fin_power.cnt_proch_fact as pow_cnt_proch_fact, /*number*/ 
ipr_fin_power.cnt_proch_fact_pr as pow_cnt_proch_fact_pr, /*number*/ 
ipr_fin_power.km_kl as pow_km_kl, /*Ввод КЛ км план*//*number*/ 
ipr_fin_power.km_kl_del as pow_km_kl_del, /*number*/ 
ipr_fin_power.km_kl_fact as pow_km_kl_fact, /*Ввод КЛ км факт*//*number*/ 
ipr_fin_power.km_kl_ekspl as pow_km_kl_ekspl, /*number*/ 
ipr_fin_power.km_kl_pr as pow_km_kl_pr, /*Прирост КЛ км план*//*number*/ 
ipr_fin_power.km_kl_fact_pr as pow_km_kl_fact_pr, /*Прирост КЛ км факт*//*number*/ 
ipr_fin_power.km_kl_wfact as pow_km_kl_wait, /*number*/ 
ipr_fin_power.km_kl_wfact_pr as pow_km_kl_wait_pr, /*number*/ 
ipr_fin_power.pow_any as pow_any, /*Ввод мва план*//*number*/ 
ipr_fin_power.pow_any_fact as pow_any_fact, /*Ввод мва факт*//*number*/ 
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_power 
select null as kod_smet,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
b.kod_ipr as kod_ipr, /*number*/ 
b.year as year, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.mba as mba, /*Ввод мва план*//*number*/ 
a.km as km, /*Ввод км ВЛ план*//*number*/ 
a.cnt as cnt, /*Ввод шт план*//*number*/ 
a.km_pr as km_pr, /*Прирост км ВЛ план*//*number*/ 
a.mba_pr as mba_pr, /*Прирост мва план*//*number*/ 
a.cnt_pr as cnt_pr, /*Прирост шт план*//*number*/ 
a.mba_del as mba_del, /*number*/ 
a.km_del as km_del, /*number*/ 
a.mba_fact as mba_fact, /*Ввод мва факт*//*number*/ 
a.km_fact as km_fact, /*Ввод км ВЛ факт*//*number*/ 
a.cnt_fact as cnt_fact, /*Ввод шт факт*//*number*/ 
a.km_fact_pr as km_fact_pr, /*Прирост км ВЛ факт*//*number*/ 
a.mba_fact_pr as mba_fact_pr, /*Прирост мва факт*//*number*/ 
a.cnt_fact_pr as cnt_fact_pr, /*Прирост шт*//*number*/ 
a.mba_wfact as mba_wfact, /*number*/ 
a.km_wfact as km_wfact, /*number*/ 
a.cnt_wfact as cnt_wfact, /*number*/ 
a.cnt_proch_wfact as cnt_proch_wfact, /*number*/ 
a.km_wfact_pr as km_wfact_pr, /*number*/ 
a.mba_wfact_pr as mba_wfact_pr, /*number*/ 
a.cnt_wfact_pr as cnt_wfact_pr, /*number*/ 
a.cnt_proch_wfact_pr as cnt_proch_wfact_pr, /*number*/ 
a.cnt_proch as cnt_proch, /*number*/ 
a.cnt_proch_pr as cnt_proch_pr, /*number*/ 
a.cnt_proch_fact as cnt_proch_fact, /*number*/ 
a.cnt_proch_fact_pr as cnt_proch_fact_pr, /*number*/ 
a.km_kl as km_kl, /*Ввод КЛ км план*//*number*/ 
a.km_kl_del as km_kl_del, /*number*/ 
a.km_kl_fact as km_kl_fact, /*Ввод КЛ км факт*//*number*/ 
a.km_kl_ekspl as km_kl_ekspl, /*number*/ 
a.km_kl_pr as km_kl_pr, /*Прирост КЛ км план*//*number*/ 
a.km_kl_fact_pr as km_kl_fact_pr, /*Прирост КЛ км факт*//*number*/ 
a.km_kl_wfact as km_kl_wfact, /*number*/ 
a.km_kl_wfact_pr as km_kl_wfact_pr, /*number*/ 
        coalesce(                  nullif ( a.mba ,0)                  , nullif ( a.km ,0)                  , nullif ( a.km_kl ,0)                  , nullif ( a.cnt ,0)                  , nullif ( a.cnt_proch ,0)                  , nullif ( a.mba_pr ,0)                  , nullif ( a.km_pr ,0)                  , nullif ( a.km_pr ,0)                  , nullif ( a.cnt_pr ,0)                  , nullif ( a.cnt_pr ,0)                )       as pow_any, /*Ввод мва план*//*number*/ 
        coalesce(                  nullif ( a.mba_fact ,0)                  , nullif ( a.km_fact ,0)                  , nullif ( a.km_kl_fact ,0)                  , nullif ( a.cnt_fact ,0)                  , nullif ( a.cnt_proch_fact ,0)                  , nullif ( a.mba_fact_pr ,0)                  , nullif ( a.km_fact_pr ,0)                  , nullif ( a.km_fact_pr ,0)                  , nullif ( a.cnt_fact_pr ,0)                  , nullif ( a.cnt_fact_pr ,0)                )       as pow_any_fact, /*Ввод мва факт*//*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_power_body 
select a.kod_plan_power as kod_plan_power, /*number*//*key*/ 
a.period as period, /**//*number*/ 
a.mba as mba, /*Ввод мва план*//*number*/ 
a.km as km, /*Ввод км ВЛ план*//*number*/ 
a.cnt as cnt, /*Ввод шт план*//*number*/ 
a.kod_power_head as kod_power_head, /*number*/ 
a.mba_fact as mba_fact, /*Ввод мва факт*//*number*/ 
a.km_fact as km_fact, /*Ввод км ВЛ факт*//*number*/ 
a.cnt_fact as cnt_fact, /*Ввод шт факт*//*number*/ 
a.cnt_proch as cnt_proch, /**//*number*/ 
a.cnt_proch_fact as cnt_proch_fact, /**//*number*/ 
a.mba_wfact as mba_wfact, /**//*number*/ 
a.km_wfact as km_wfact, /**//*number*/ 
a.cnt_wfact as cnt_wfact, /**//*number*/ 
a.cnt_proch_wfact as cnt_proch_wfact, /**//*number*/ 
a.mba_wfact_pr as mba_wfact_pr, /**//*number*/ 
a.km_wfact_pr as km_wfact_pr, /**//*number*/ 
a.cnt_wfact_pr as cnt_wfact_pr, /**//*number*/ 
a.cnt_proch_wfact_pr as cnt_proch_wfact_pr, /**//*number*/ 
a.mba_pr as mba_pr, /*Прирост мва план*//*number*/ 
a.km_pr as km_pr, /*Прирост км ВЛ план*//*number*/ 
a.cnt_pr as cnt_pr, /*Прирост шт план*//*number*/ 
a.cnt_proch_pr as cnt_proch_pr, /**//*number*/ 
a.mba_fact_pr as mba_fact_pr, /*Прирост мва факт*//*number*/ 
a.km_fact_pr as km_fact_pr, /*Прирост км ВЛ факт*//*number*/ 
a.cnt_fact_pr as cnt_fact_pr, /*Прирост шт*//*number*/ 
a.cnt_proch_fact_pr as cnt_proch_fact_pr, /**//*number*/ 
a.km_kl as km_kl, /*Ввод КЛ км план*//*number*/ 
a.km_kl_fact as km_kl_fact, /*Ввод КЛ км факт*//*number*/ 
a.km_kl_ekspl as km_kl_ekspl, /**//*number*/ 
a.km_kl_wfact as km_kl_wfact, /**//*number*/ 
a.km_kl_wfact_pr as km_kl_wfact_pr, /**//*number*/ 
a.km_kl_pr as km_kl_pr, /*Прирост КЛ км план*//*number*/ 
a.km_kl_fact_pr as km_kl_fact_pr, /*Прирост КЛ км факт*//*number*/ 
          nvl( a.km ,0)                  -nvl( a.km_pr ,0)         as km_del, /*number*/ 
          nvl( a.km_kl ,0)                  -nvl( a.km_kl_pr ,0)         as km_kl_del, /*number*/ 
          nvl( a.mba ,0)                  -nvl( a.mba_pr ,0)         as mba_del, /*number*/ 
          nvl( (head.year          * 100)  ,0)                  +nvl( a.period ,0)         as ym/*number*/ 
 
from ipr_power_body 
a 
--\ipr_power_body 
left outer join 
( 
--ipr_power_head 
select a.kod_power_head as kod_power_head, /*number*//*key*/ 
a.year as year/**//*number*/ 
 
from ipr_power_head 
a 
--\ipr_power_head 
) 
head on a.kod_power_head          =  head.kod_power_head--\ipr_power_head 
) 
a 
--\ipr_power_body 
left outer join 
( 
--ipr_power_head 
select a.kod_power_head as kod_power_head, /*number*//*key*/ 
a.year as year, /**//*number*/ 
        case                  when  (ipr_v_docs_last.inc is  null        )           then  0          else  1        end       as pr_last, /*number*/ 
ipr_v_docs_last.kod_ipr as kod_ipr/*number*/ 
 
from ipr_power_head 
a 
--\ipr_power_head 
left outer join 
( 
-- 
select ipr_v_docs_last.kod_power_head as kod_power_head, /*number*//*key*/ 
max(ipr_v_docs_last.inc)  as inc, /*number*/ 
max(ipr_v_docs_last.kod_ipr)  as kod_ipr/*number*/ 
 
from ( 
--ipr_v_docs_last_power 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_ipr as kod_ipr, /*number*/ 
a.kod_power_head as kod_power_head, /*number*/ 
a.inc as inc/**//*number*/ 
 
from ipr_v_docs_last_power 
a 
--\ipr_v_docs_last_power 
) 
ipr_v_docs_last 
--\ipr_v_docs_last_power 
where 
ipr_v_docs_last.inc          =  1 group by 
ipr_v_docs_last.kod_power_head/*number*//*key*/ 
) 
ipr_v_docs_last on ipr_v_docs_last.kod_power_head          =  a.kod_power_head--\ 
) 
b on a.kod_power_head          =  b.kod_power_head--\ipr_power_head 
where 
b.pr_last          =  1) 
ipr_fin_power 
--\ipr_fin_power 
) 
--\ 
union all 
( 
-- 
select ipr_fin_kap_fact_dog.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_kap_fact_dog.year as year, /*number*/ 
ipr_fin_kap_fact_dog.period as period, /*number*/ 
ipr_fin_kap_fact_dog.ym as ym, /*number*/ 
ipr_fin_kap_fact_dog.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_kap_fact_dog.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_kap_fact_dog.kod_dog as kod_dog, /*number*/ 
ipr_fin_kap_fact_dog.kod_smet as kod_smet, /*number*/ 
ipr_fin_kap_fact_dog.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
ipr_fin_kap_fact_dog.vsego as kap_sum_nds_fact_a, /*Фактический объем освоения c НДС (без перебросок)*//*number*/ 
ipr_fin_kap_fact_dog.nds as kap_nds_fact_a, /*Фактический объем освоения НДС(без перебросок)*//*number*/ 
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_kap_fact_dog 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
        case                  when  (          nullif( a.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
ipr_ipr_data.kod_ipr as kod_ipr, /*number*/ 
a.kod_dog as kod_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*/ 
a.kodzatrat as kodzatrat, /*number*/ 
a.year as year, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.nds as nds, /*НДС*//*number*/ 
a.vsego as vsego, /*Сумма с НДС*//*number*/ 
null as pr_last_smet 
 
from ( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_ip as kod_ip, /*number*/ 
a.vsego as vsego, /*Сумма с НДС*//*number*/ 
a.nds as nds, /*НДС*//*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet, /*number*/ 
          to_number(to_char( a.dat_doc ,'YYYYMM'))         as ym, /*number*/ 
          to_number(to_char( a.dat_doc ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( a.dat_doc ,'MM'))         as period/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
a 
--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on a.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_ip on a.kod_ip          =  kod_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_ip.kod_titul_ip--\ipr_ipr_data 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_kap_fact_dog 
--\ipr_fin_kap_fact_dog 
) 
--\ 
union all 
( 
-- 
select ipr_fin_kap_fact_mfr.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_kap_fact_mfr.year as year, /*number*/ 
ipr_fin_kap_fact_mfr.period as period, /*number*/ 
ipr_fin_kap_fact_mfr.ym as ym, /*number*/ 
ipr_fin_kap_fact_mfr.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_kap_fact_mfr.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_kap_fact_mfr.kod_dog as kod_dog, /*number*/ 
ipr_fin_kap_fact_mfr.kod_smet as kod_smet, /*number*/ 
ipr_fin_kap_fact_mfr.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
ipr_fin_kap_fact_mfr.summ_per as kap_sum_fact_per, /*Фактический объем освоения без НДС (передано)*//*number*/ 
ipr_fin_kap_fact_mfr.nds_per as kap_nds_fact_per, /*Фактический объем освоения НДС (передано)*//*number*/ 
ipr_fin_kap_fact_mfr.summ_per_pr as kap_sum_fact_per_pr, /*Фактический объем освоения прочие без НДС (передано)*//*number*/ 
ipr_fin_kap_fact_mfr.nds_per_pr as kap_nds_fact_per_pr, /*Фактический объем освоения  прочие  НДС (передано)*//*number*/ 
ipr_fin_kap_fact_mfr.summ_sp as kap_sum_fact_sp, /*Фактический объем освоения без НДС (списано)*//*number*/ 
ipr_fin_kap_fact_mfr.nds_sp as kap_nds_fact_sp, /*Фактический объем освоения НДС (списано)*//*number*/ 
ipr_fin_kap_fact_mfr.summ_sp_pr as kap_sum_fact_sp_pr, /*Фактический объем освоения прочие без НДС (списано)*//*number*/ 
ipr_fin_kap_fact_mfr.nds_sp_pr as kap_nds_fact_sp_pr, /*Фактический объем освоения  прочие  НДС (списано)*//*number*/ 
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_kap_fact_mfr 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
kod_vip_dog.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( kod_vip_dog.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vip_dog.kodzatrat as kodzatrat, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'MM'))         as period, /*number*/ 
(          to_number(to_char( kod_doc.doc_date ,'YYYY'))        )  *100+        (          to_number(to_char( kod_doc.doc_date ,'MM'))        )  as ym, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to  is not null        )           then  mv.summ        end       as summ_per, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to  is not null        )           then  (mv.summ          * (kod_vip_dog.proc_nds          /nullif( 100 ,0)        ) )         end       as nds_per, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to is  null        )           then  mv.summ        end       as summ_sp, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to is  null        )           then  (mv.summ          * (kod_vip_dog.proc_nds          /nullif( 100 ,0)        ) )         end       as nds_sp, /*number*/ 
        case                  when  ((mv.kod_fact_wcompl_to  is not null        )           and  ((        nvl(        kod_vip_dog.kod_dog          , -1 )      )           =  -1) )           then  mv.summ        end       as summ_per_pr, /*number*/ 
        case                  when  ((mv.kod_fact_wcompl_to  is not null        )           and  ((        nvl(        kod_vip_dog.kod_dog          , -1 )      )           =  -1) )           then  (mv.summ          * (kod_vip_dog.proc_nds          /nullif( 100 ,0)        ) )         end       as nds_per_pr, /*number*/ 
        case                  when  ((mv.kod_fact_wcompl_to is  null        )           and  ((        nvl(        kod_vip_dog.kod_dog          , -1 )      )           =  -1) )           then  mv.summ        end       as summ_sp_pr, /*number*/ 
        case                  when  ((mv.kod_fact_wcompl_to is  null        )           and  ((        nvl(        kod_vip_dog.kod_dog          , -1 )      )           =  -1) )           then  (mv.summ          * (kod_vip_dog.proc_nds          /nullif( 100 ,0)        ) )         end       as nds_sp_pr, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_move_wsumm 
select a.kod_move_wsumm as kod_move_wsumm, /*number*//*key*/ 
a.kod_doc as kod_doc, /*number*/ 
a.summ as summ, /**//*number*/ 
a.kod_fact_wcompl_fr as kod_fact_wcompl_fr, /*number*/ 
a.kod_fact_wcompl_to as kod_fact_wcompl_to, /*number*/ 
a.kod_titul_ip_fr as kod_titul_ip_fr/*number*/ 
 
from ipr_move_wsumm 
a 
--\ipr_move_wsumm 
) 
mv 
--\ipr_move_wsumm 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date/**//*date*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on mv.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_fact_wcompl 
select a.kod_fact_wcompl as kod_fact_wcompl, /*number*//*key*/ 
a.kod_vip_dog as kod_vip_dog/*number*/ 
 
from ipr_fact_wcompl 
a 
--\ipr_fact_wcompl 
) 
kod_fact_wcompl_fr on mv.kod_fact_wcompl_fr          =  kod_fact_wcompl_fr.kod_fact_wcompl--\ipr_fact_wcompl 
left outer join 
( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.proc_nds as proc_nds, /**//*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
kod_vip_dog on kod_fact_wcompl_fr.kod_vip_dog          =  kod_vip_dog.kod_vip_dog--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vip_dog.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip_fr on mv.kod_titul_ip_fr          =  kod_titul_ip_fr.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip_fr.kod_titul_ip--\ipr_ipr_data 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_kap_fact_mfr 
--\ipr_fin_kap_fact_mfr 
) 
--\ 
union all 
( 
-- 
select ipr_fin_kap_fact_mto.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_kap_fact_mto.year as year, /*number*/ 
ipr_fin_kap_fact_mto.period as period, /*number*/ 
ipr_fin_kap_fact_mto.ym as ym, /*number*/ 
ipr_fin_kap_fact_mto.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_kap_fact_mto.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_kap_fact_mto.kod_dog as kod_dog, /*number*/ 
ipr_fin_kap_fact_mto.kod_smet as kod_smet, /*number*/ 
ipr_fin_kap_fact_mto.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
ipr_fin_kap_fact_mto.summ as kap_sum_fact_pri, /*Фактический объем освоения без НДС (принято)*//*number*/ 
ipr_fin_kap_fact_mto.nds as kap_nds_fact_pri, /*Фактический объем освоения НДС (принято)*//*number*/ 
ipr_fin_kap_fact_mto.summ_pr as kap_sum_fact_pri_pr, /*Фактический объем освоения прочие без НДС (принято)*//*number*/ 
ipr_fin_kap_fact_mto.nds_pr as kap_nds_fact_pri_pr, /*Фактический объем освоения прочие НДС (принято)*//*number*/ 
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_kap_fact_mto 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
kod_vip_dog.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( kod_vip_dog.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vip_dog.kodzatrat as kodzatrat, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'MM'))         as period, /*number*/ 
(          to_number(to_char( kod_doc.doc_date ,'YYYY'))        )  *100+        (          to_number(to_char( kod_doc.doc_date ,'MM'))        )  as ym, /*number*/ 
mv.summ as summ, /*number*/ 
mv.summ          * (kod_vip_dog.proc_nds          /nullif( 100 ,0)        )  as nds, /*number*/ 
        case                  when  ((        nvl(        kod_vip_dog.kod_dog          , -1 )      )           =  -1)           then  mv.summ        end       as summ_pr, /*number*/ 
        case                  when  ((        nvl(        kod_vip_dog.kod_dog          , -1 )      )           =  -1)           then  (mv.summ          * (kod_vip_dog.proc_nds          /nullif( 100 ,0)        ) )         end       as nds_pr, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_move_wsumm 
select a.kod_move_wsumm as kod_move_wsumm, /*number*//*key*/ 
a.kod_doc as kod_doc, /*number*/ 
a.summ as summ, /**//*number*/ 
a.kod_fact_wcompl_to as kod_fact_wcompl_to, /*number*/ 
a.kod_titul_ip_to as kod_titul_ip_to/*number*/ 
 
from ipr_move_wsumm 
a 
--\ipr_move_wsumm 
) 
mv 
--\ipr_move_wsumm 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date/**//*date*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on mv.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip_to on mv.kod_titul_ip_to          =  kod_titul_ip_to.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip_to.kod_titul_ip--\ipr_ipr_data 
left outer join 
( 
--ipr_fact_wcompl 
select a.kod_fact_wcompl as kod_fact_wcompl, /*number*//*key*/ 
a.kod_vip_dog as kod_vip_dog/*number*/ 
 
from ipr_fact_wcompl 
a 
--\ipr_fact_wcompl 
) 
kod_fact_wcompl_to on mv.kod_fact_wcompl_to          =  kod_fact_wcompl_to.kod_fact_wcompl--\ipr_fact_wcompl 
left outer join 
( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.proc_nds as proc_nds, /**//*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
kod_vip_dog on kod_fact_wcompl_to.kod_vip_dog          =  kod_vip_dog.kod_vip_dog--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vip_dog.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_kap_fact_mto 
--\ipr_fin_kap_fact_mto 
) 
--\ 
union all 
( 
-- 
select ipr_fin_finplan_fact_dog.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_finplan_fact_dog.year as year, /*number*/ 
ipr_fin_finplan_fact_dog.period as period, /*number*/ 
ipr_fin_finplan_fact_dog.ym as ym, /*number*/ 
ipr_fin_finplan_fact_dog.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_finplan_fact_dog.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_finplan_fact_dog.kod_dog as kod_dog, /*number*/ 
ipr_fin_finplan_fact_dog.kod_smet as kod_smet, /*number*/ 
ipr_fin_finplan_fact_dog.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
ipr_fin_finplan_fact_dog.sum_opl as fp_fact_sum, /*number*/ 
null as fp_fact_sum_pr,  
ipr_fin_finplan_fact_dog.nds as fp_fact_nds, /*number*/ 
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_finplan_fact_dog 
select a.kod_opl_period as kod_opl_period, /*number*//*key*/ 
ipr_ipr_data.kod_ipr as kod_ipr, /*number*/ 
a.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( a.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*/ 
a.kodzatrat as kodzatrat, /*number*/ 
a.year as year, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.nds as nds, /*number*/ 
a.sum_opl as sum_opl, /*number*/ 
null as pr_last_smet 
 
from ( 
--isv_ip_opl_for_plan 
select a.kod_opl_period as kod_opl_period, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_ip as kod_ip, /*number*/ 
a.sum_opl as sum_opl, /**//*number*/ 
a.nds as nds, /**//*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
          to_number(to_char( a.dat_opl ,'YYYYMM'))         as ym, /*number*/ 
          to_number(to_char( a.dat_opl ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( a.dat_opl ,'MM'))         as period/*number*/ 
 
from isv_ip_opl 
a 
--\isv_ip_opl 
where 
(          nullif( a.kod_dog ,-1) is  null        )           or  (        exists  ( 
-- 
select 1 as a1 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs 
--\ipr_dogs 
where 
(a.kod_dog          =  ipr_dogs.kod_dog)           and  (a.kod_ip          =  ipr_dogs.kod_titul_ip) ) 
--\ 
) ) 
a 
--\isv_ip_opl_for_plan 
left outer join 
( 
--ips_vid_zatrat 
select a.kodzatrat as kodzatrat, /**//*number*//*key*/ 
a.kod_smet as kod_smet/*number*/ 
 
from ips_vid_zatrat 
a 
--\ips_vid_zatrat 
) 
kodzatrat on a.kodzatrat          =  kodzatrat.kodzatrat--\ips_vid_zatrat 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kodzatrat.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_ip on a.kod_ip          =  kod_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_ip.kod_titul_ip--\ipr_ipr_data 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_finplan_fact_dog 
--\ipr_fin_finplan_fact_dog 
) 
--\ 
union all 
( 
-- 
select ipr_fin_kap_fact_dog_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_kap_fact_dog_pr.year as year, /*number*/ 
ipr_fin_kap_fact_dog_pr.period as period, /*number*/ 
ipr_fin_kap_fact_dog_pr.ym as ym, /*number*/ 
ipr_fin_kap_fact_dog_pr.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_kap_fact_dog_pr.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_kap_fact_dog_pr.kod_dog as kod_dog, /*number*/ 
ipr_fin_kap_fact_dog_pr.kod_smet as kod_smet, /*number*/ 
ipr_fin_kap_fact_dog_pr.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
ipr_fin_kap_fact_dog_pr.vsego as fp_fact_sum_pr, /*Сумма с НДС*//*number*/ 
null as fp_fact_nds,  
ipr_fin_kap_fact_dog_pr.nds as fp_fact_nds_pr, /*НДС*//*number*/ 
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_kap_fact_dog_pr 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
ipr_ipr_data.kod_ipr as kod_ipr, /*number*/ 
a.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( a.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*/ 
a.kodzatrat as kodzatrat, /*number*/ 
a.year as year, /*number*/ 
a.period as period, /*number*/ 
a.ym as ym, /*number*/ 
a.nds as nds, /*НДС*//*number*/ 
a.vsego as vsego, /*Сумма с НДС*//*number*/ 
null as pr_last_smet 
 
from ( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_ip as kod_ip, /*number*/ 
a.vsego as vsego, /*Сумма с НДС*//*number*/ 
a.nds as nds, /*НДС*//*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet, /*number*/ 
          to_number(to_char( a.dat_doc ,'YYYYMM'))         as ym, /*number*/ 
          to_number(to_char( a.dat_doc ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( a.dat_doc ,'MM'))         as period/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
a 
--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on a.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_ip on a.kod_ip          =  kod_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_ip.kod_titul_ip--\ipr_ipr_data 
where 
((        nvl(        a.kod_dog          , -1 )      )           =  -1)           and  (kod_smet.kod_parent is  null        ) ) 
ipr_fin_kap_fact_dog_pr 
--\ipr_fin_kap_fact_dog_pr 
) 
--\ 
union all 
( 
-- 
select ipr_fin_finplan_fact_mfr.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_finplan_fact_mfr.year as year, /*number*/ 
ipr_fin_finplan_fact_mfr.period as period, /*number*/ 
ipr_fin_finplan_fact_mfr.ym as ym, /*number*/ 
ipr_fin_finplan_fact_mfr.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_finplan_fact_mfr.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_finplan_fact_mfr.kod_dog as kod_dog, /*number*/ 
ipr_fin_finplan_fact_mfr.kod_smet as kod_smet, /*number*/ 
ipr_fin_finplan_fact_mfr.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
ipr_fin_finplan_fact_mfr.summ_per as fp_sum_fact_per1, /*number*/ 
ipr_fin_finplan_fact_mfr.nds_per as fp_nds_fact_per1, /*number*/ 
ipr_fin_finplan_fact_mfr.summ_sp as fp_sum_fact_sp1, /*number*/ 
ipr_fin_finplan_fact_mfr.nds_sp as fp_nds_fact_sp1, /*number*/ 
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_finplan_fact_mfr 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
kod_vip_dog.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( kod_vip_dog.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vip_dog.kodzatrat as kodzatrat, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'MM'))         as period, /*number*/ 
(          to_number(to_char( kod_doc.doc_date ,'YYYY'))        )  *100+        (          to_number(to_char( kod_doc.doc_date ,'MM'))        )  as ym, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to  is not null        )           then  mvf.summ        end       as summ_per, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to  is not null        )           then  (          nvl( mvf.summ_nds ,0)                  -nvl( mvf.summ ,0)        )         end       as nds_per, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to is  null        )           then  mvf.summ        end       as summ_sp, /*number*/ 
        case                  when  (mv.kod_fact_wcompl_to is  null        )           then  (          nvl( mvf.summ_nds ,0)                  -nvl( mvf.summ ,0)        )         end       as nds_sp, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_move_wsumm_fin 
select a.kod_move_wsumm_fin as kod_move_wsumm_fin, /*number*//*key*/ 
a.kod_move_wsumm as kod_move_wsumm, /*number*/ 
a.summ as summ, /**//*number*/ 
a.summ_nds as summ_nds/**//*number*/ 
 
from ipr_move_wsumm_fin 
a 
--\ipr_move_wsumm_fin 
) 
mvf 
--\ipr_move_wsumm_fin 
left outer join 
( 
--ipr_move_wsumm 
select a.kod_move_wsumm as kod_move_wsumm, /*number*//*key*/ 
a.kod_doc as kod_doc, /*number*/ 
a.kod_fact_wcompl_fr as kod_fact_wcompl_fr, /*number*/ 
a.kod_fact_wcompl_to as kod_fact_wcompl_to, /*number*/ 
a.kod_titul_ip_fr as kod_titul_ip_fr/*number*/ 
 
from ipr_move_wsumm 
a 
--\ipr_move_wsumm 
) 
mv on mvf.kod_move_wsumm          =  mv.kod_move_wsumm--\ipr_move_wsumm 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date/**//*date*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on mv.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_fact_wcompl 
select a.kod_fact_wcompl as kod_fact_wcompl, /*number*//*key*/ 
a.kod_vip_dog as kod_vip_dog/*number*/ 
 
from ipr_fact_wcompl 
a 
--\ipr_fact_wcompl 
) 
kod_fact_wcompl_fr on mv.kod_fact_wcompl_fr          =  kod_fact_wcompl_fr.kod_fact_wcompl--\ipr_fact_wcompl 
left outer join 
( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
kod_vip_dog on kod_fact_wcompl_fr.kod_vip_dog          =  kod_vip_dog.kod_vip_dog--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vip_dog.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip_fr on mv.kod_titul_ip_fr          =  kod_titul_ip_fr.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip_fr.kod_titul_ip--\ipr_ipr_data 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_finplan_fact_mfr 
--\ipr_fin_finplan_fact_mfr 
) 
--\ 
union all 
( 
-- 
select ipr_fin_finplan_fact_mto.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_finplan_fact_mto.year as year, /*number*/ 
ipr_fin_finplan_fact_mto.period as period, /*number*/ 
ipr_fin_finplan_fact_mto.ym as ym, /*number*/ 
ipr_fin_finplan_fact_mto.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_finplan_fact_mto.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_finplan_fact_mto.kod_dog as kod_dog, /*number*/ 
ipr_fin_finplan_fact_mto.kod_smet as kod_smet, /*number*/ 
ipr_fin_finplan_fact_mto.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
ipr_fin_finplan_fact_mto.summ as fp_sum_fact_pri1, /*number*/ 
ipr_fin_finplan_fact_mto.nds as fp_nds_fact_pri1, /*number*/ 
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_finplan_fact_mto 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
kod_vip_dog.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( kod_vip_dog.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vip_dog.kodzatrat as kodzatrat, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'MM'))         as period, /*number*/ 
(          to_number(to_char( kod_doc.doc_date ,'YYYY'))        )  *100+        (          to_number(to_char( kod_doc.doc_date ,'MM'))        )  as ym, /*number*/ 
mvf.summ as summ, /*number*/ 
          nvl( mvf.summ_nds ,0)                  -nvl( mvf.summ ,0)         as nds, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_move_wsumm_fin 
select a.kod_move_wsumm_fin as kod_move_wsumm_fin, /*number*//*key*/ 
a.kod_move_wsumm as kod_move_wsumm, /*number*/ 
a.summ as summ, /**//*number*/ 
a.summ_nds as summ_nds/**//*number*/ 
 
from ipr_move_wsumm_fin 
a 
--\ipr_move_wsumm_fin 
) 
mvf 
--\ipr_move_wsumm_fin 
left outer join 
( 
--ipr_move_wsumm 
select a.kod_move_wsumm as kod_move_wsumm, /*number*//*key*/ 
a.kod_doc as kod_doc, /*number*/ 
a.kod_fact_wcompl_to as kod_fact_wcompl_to, /*number*/ 
a.kod_titul_ip_to as kod_titul_ip_to/*number*/ 
 
from ipr_move_wsumm 
a 
--\ipr_move_wsumm 
) 
mv on mvf.kod_move_wsumm          =  mv.kod_move_wsumm--\ipr_move_wsumm 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip_to on mv.kod_titul_ip_to          =  kod_titul_ip_to.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip_to.kod_titul_ip--\ipr_ipr_data 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date/**//*date*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on mv.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_fact_wcompl 
select a.kod_fact_wcompl as kod_fact_wcompl, /*number*//*key*/ 
a.kod_vip_dog as kod_vip_dog/*number*/ 
 
from ipr_fact_wcompl 
a 
--\ipr_fact_wcompl 
) 
kod_fact_wcompl_to on mv.kod_fact_wcompl_to          =  kod_fact_wcompl_to.kod_fact_wcompl--\ipr_fact_wcompl 
left outer join 
( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
kod_vip_dog on kod_fact_wcompl_to.kod_vip_dog          =  kod_vip_dog.kod_vip_dog--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vip_dog.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_finplan_fact_mto 
--\ipr_fin_finplan_fact_mto 
) 
--\ 
union all 
( 
-- 
select ipr_fin_vvod_fact_dog.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_vvod_fact_dog.year as year, /*number*/ 
ipr_fin_vvod_fact_dog.period as period, /*number*/ 
ipr_fin_vvod_fact_dog.ym as ym, /*number*/ 
ipr_fin_vvod_fact_dog.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_vvod_fact_dog.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_vvod_fact_dog.kod_dog as kod_dog, /*number*/ 
ipr_fin_vvod_fact_dog.kod_smet as kod_smet, /*number*/ 
ipr_fin_vvod_fact_dog.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
ipr_fin_vvod_fact_dog.summ as vvod_sum_fact, /*Фактический ввод в без НДС*//*number*/ 
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_vvod_fact_dog 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
kod_vip_dog.kod_dog as kod_dog, /*number*/ 
        case                  when  (          nullif( kod_vip_dog.kod_dog ,-1) is  null        )           then  ipr_ipr_data.kod_ipr_dog_pr        end       as kod_ipr_dog, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*//*key*/ 
kod_vip_dog.kodzatrat as kodzatrat, /*number*/ 
ipr_works_vvod.year as year, /*number*/ 
ipr_works_vvod.period as period, /*number*/ 
ipr_works_vvod.ym as ym, /*number*/ 
ipr_works_vvod.summ as summ, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_works_vvod 
select a.kod_work_vvod as kod_work_vvod, /*number*//*key*/ 
a.summ as summ, /**//*number*/ 
a.kod_fact_wcompl as kod_fact_wcompl, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYYMM'))         as ym, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'MM'))         as period/*number*/ 
 
from ipr_works_vvod 
a 
--\ipr_works_vvod 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date/**//*date*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on a.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
) 
ipr_works_vvod 
--\ipr_works_vvod 
left outer join 
( 
--ipr_fact_wcompl 
select a.kod_fact_wcompl as kod_fact_wcompl, /*number*//*key*/ 
a.kod_work_body as kod_work_body, /*number*/ 
a.kod_vip_dog as kod_vip_dog/*number*/ 
 
from ipr_fact_wcompl 
a 
--\ipr_fact_wcompl 
) 
kod_fact_wcompl on ipr_works_vvod.kod_fact_wcompl          =  kod_fact_wcompl.kod_fact_wcompl--\ipr_fact_wcompl 
left outer join 
( 
--isv_ip_vip_dog_for_plan 
select a.kod_vip_dog as kod_vip_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kodzatrat as kodzatrat, /**//*number*/ 
a.kod_smet as kod_smet/*number*/ 
 
from isv_ip_vip_dog 
a 
--\isv_ip_vip_dog 
) 
kod_vip_dog on kod_fact_wcompl.kod_vip_dog          =  kod_vip_dog.kod_vip_dog--\isv_ip_vip_dog_for_plan 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vip_dog.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
left outer join 
( 
--ipr_works_body 
select a.kod_work_body as kod_work_body, /*number*//*key*/ 
a.kod_tituls_object as kod_tituls_object/*number*/ 
 
from ipr_works_body 
a 
--\ipr_works_body 
) 
kod_work_body on kod_fact_wcompl.kod_work_body          =  kod_work_body.kod_work_body--\ipr_works_body 
left outer join 
( 
--ipr_tituls_objects 
select a.kod_tituls_object as kod_tituls_object, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_tituls_objects 
a 
--\ipr_tituls_objects 
) 
kod_tituls_object on kod_work_body.kod_tituls_object          =  kod_tituls_object.kod_tituls_object--\ipr_tituls_objects 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_ip on kod_tituls_object.kod_titul_ip          =  kod_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
ipr_dogs_pr.kod_ipr_dog_pr as kod_ipr_dog_pr/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_dogs_pr.kod_ipr as kod_ipr, /*number*//*key*/ 
max(ipr_dogs_pr.kod_ipr_dog)  as kod_ipr_dog_pr/*number*/ 
 
from ( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog, /*number*/ 
a.kod_dog_type as kod_dog_type, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
ipr_dogs_pr 
--\ipr_dogs 
where 
((        nvl(        ipr_dogs_pr.kod_dog          , -1 )      )           =  -1)           and  (ipr_dogs_pr.kod_dog_type          in  (52          ,  56) )  group by 
ipr_dogs_pr.kod_ipr/*number*//*key*/ 
) 
ipr_dogs_pr on ipr_dogs_pr.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_ip.kod_titul_ip--\ipr_ipr_data 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_vvod_fact_dog 
--\ipr_fin_vvod_fact_dog 
) 
--\ 
union all 
( 
-- 
select ipr_fin_ipr.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_ipr.year as year,  
ipr_fin_ipr.period as period,  
ipr_fin_ipr.ym as ym,  
ipr_fin_ipr.kodzatrat as kodzatrat,  
ipr_fin_ipr.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_ipr.kod_dog as kod_dog,  
ipr_fin_ipr.kod_smet as kod_smet,  
ipr_fin_ipr.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
ipr_fin_ipr.ipr_fin_ipr as ipr_fin_ipr, /*number*/ 
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
ipr_fin_ipr.dz as dz, /*Заданная начальная дебиторская задолженность*//*number*/ 
ipr_fin_ipr.kz as kz, /*Заданная начальная кредиторская задолженность*//*number*/ 
ipr_fin_ipr.saldo as nzs_saldo, /*Заданное НЗС*//*number*/ 
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_ipr 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
1 as ipr_fin_ipr, /*number*/ 
ipr_closed_plans.dz as dz, /*Заданная начальная дебиторская задолженность*//*number*/ 
ipr_closed_plans.kz as kz, /*Заданная начальная кредиторская задолженность*//*number*/ 
ipr_closed_plans.saldo as saldo, /*Заданное НЗС*//*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as kod_smet,  
null as pr_last_smet 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr/*number*//*key*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select ipr_closed_plans.kod_ipr as kod_ipr, /*number*//*key*/ 
sum(ipr_closed_plans.dz)  as dz, /*number*/ 
sum(ipr_closed_plans.kz)  as kz, /*number*/ 
sum(ipr_closed_plans.saldo)  as saldo/*number*/ 
 
from ( 
--ipr_closed_plans 
select a.kod_closed_plan as kod_closed_plan, /*number*//*key*/ 
a.kod_ipr as kod_ipr, /*number*/ 
a.dz as dz, /**//*number*/ 
a.kz as kz, /**//*number*/ 
a.saldo as saldo/**//*number*/ 
 
from ipr_closed_plans 
a 
--\ipr_closed_plans 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
kod_ipr on a.kod_ipr          =  kod_ipr.kod_ipr--\ipr_ipr_data 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.is_head_tit as is_head_tit/**//*number*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip on kod_ipr.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_titul_ip 
where 
(        nvl(        kod_titul_ip.is_head_tit          , 0 )      )           != 1) 
ipr_closed_plans 
--\ipr_closed_plans 
 group by 
ipr_closed_plans.kod_ipr/*number*//*key*/ 
) 
ipr_closed_plans on ipr_closed_plans.kod_ipr          =  a.kod_ipr--\ 
) 
ipr_fin_ipr 
--\ipr_fin_ipr 
) 
--\ 
union all 
( 
-- 
select ipr_fin_ipr100.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_ipr100.year as year,  
ipr_fin_ipr100.period as period,  
ipr_fin_ipr100.ym as ym,  
ipr_fin_ipr100.kodzatrat as kodzatrat,  
ipr_fin_ipr100.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_ipr100.kod_dog as kod_dog,  
ipr_fin_ipr100.kod_smet as kod_smet,  
ipr_fin_ipr100.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
ipr_fin_ipr100.pow as pow, /*Мощность, МВА*//*number*/ 
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_ipr100 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
100 as kod_smet,  
a.pow as pow, /*Мощность, МВА*//*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as pr_last_smet 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
val13.pow as pow/*Мощность, МВА*//*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select val13.kod_ipr as kod_ipr, /*number*//*key*/ 
max(val13.value)  as pow/*string*/ 
 
from ( 
--ipv_phys_param_values 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_ipr as kod_ipr, /*number*/ 
a.kod_phis_param as kod_phis_param, /*number*/ 
a.value as value/**//*string*/ 
 
from ipv_phys_param_values 
a 
--\ipv_phys_param_values 
) 
val13 
--\ipv_phys_param_values 
where 
val13.kod_phis_param          =  13 group by 
val13.kod_ipr/*number*//*key*/ 
) 
val13 on val13.kod_ipr          =  a.kod_ipr--\ 
) 
a 
--\ipr_ipr_data 
) 
ipr_fin_ipr100 
--\ipr_fin_ipr100 
) 
--\ 
union all 
( 
-- 
select ipr_fin_ipr200.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_ipr200.year as year,  
ipr_fin_ipr200.period as period,  
ipr_fin_ipr200.ym as ym,  
ipr_fin_ipr200.kodzatrat as kodzatrat,  
ipr_fin_ipr200.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_ipr200.kod_dog as kod_dog,  
ipr_fin_ipr200.kod_smet as kod_smet,  
ipr_fin_ipr200.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
ipr_fin_ipr200.km as km, /*Протяженность ВЛ, км*//*number*/ 
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_ipr200 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
200 as kod_smet,  
a.km as km, /*Протяженность ВЛ, км*//*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as pr_last_smet 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
val14.km as km/*Протяженность ВЛ, км*//*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select val14.kod_ipr as kod_ipr, /*number*//*key*/ 
max(val14.value)  as km/*string*/ 
 
from ( 
--ipv_phys_param_values 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_ipr as kod_ipr, /*number*/ 
a.kod_phis_param as kod_phis_param, /*number*/ 
a.value as value/**//*string*/ 
 
from ipv_phys_param_values 
a 
--\ipv_phys_param_values 
) 
val14 
--\ipv_phys_param_values 
where 
val14.kod_phis_param          =  14 group by 
val14.kod_ipr/*number*//*key*/ 
) 
val14 on val14.kod_ipr          =  a.kod_ipr--\ 
) 
a 
--\ipr_ipr_data 
) 
ipr_fin_ipr200 
--\ipr_fin_ipr200 
) 
--\ 
union all 
( 
-- 
select ipr_fin_ipr250.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_ipr250.year as year,  
ipr_fin_ipr250.period as period,  
ipr_fin_ipr250.ym as ym,  
ipr_fin_ipr250.kodzatrat as kodzatrat,  
ipr_fin_ipr250.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_ipr250.kod_dog as kod_dog,  
ipr_fin_ipr250.kod_smet as kod_smet,  
ipr_fin_ipr250.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
ipr_fin_ipr250.km_kl as km_kl, /*Протяженность КЛ, км*//*number*/ 
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_ipr250 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
250 as kod_smet,  
a.km_kl as km_kl, /*Протяженность КЛ, км*//*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as pr_last_smet 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
val18.km_kl as km_kl/*Протяженность КЛ, км*//*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
left outer join 
( 
-- 
select val18.kod_ipr as kod_ipr, /*number*//*key*/ 
max(val18.value)  as km_kl/*string*/ 
 
from ( 
--ipv_phys_param_values 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_ipr as kod_ipr, /*number*/ 
a.kod_phis_param as kod_phis_param, /*number*/ 
a.value as value/**//*string*/ 
 
from ipv_phys_param_values 
a 
--\ipv_phys_param_values 
) 
val18 
--\ipv_phys_param_values 
where 
val18.kod_phis_param          =  18 group by 
val18.kod_ipr/*number*//*key*/ 
) 
val18 on val18.kod_ipr          =  a.kod_ipr--\ 
) 
a 
--\ipr_ipr_data 
) 
ipr_fin_ipr250 
--\ipr_fin_ipr250 
) 
--\ 
union all 
( 
-- 
select ipr_fin_ipr300.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_ipr300.year as year,  
ipr_fin_ipr300.period as period,  
ipr_fin_ipr300.ym as ym,  
ipr_fin_ipr300.kodzatrat as kodzatrat,  
ipr_fin_ipr300.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_ipr300.kod_dog as kod_dog,  
ipr_fin_ipr300.kod_smet as kod_smet,  
ipr_fin_ipr300.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
ipr_fin_ipr300.other as other, /*number*/ 
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_ipr300 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
300 as kod_smet,  
null as other, /*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as pr_last_smet 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr/*number*//*key*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
a 
--\ipr_ipr_data 
) 
ipr_fin_ipr300 
--\ipr_fin_ipr300 
) 
--\ 
union all 
( 
-- 
select ipr_fin_zatrat.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_zatrat.year as year,  
ipr_fin_zatrat.period as period,  
ipr_fin_zatrat.ym as ym,  
ipr_fin_zatrat.kodzatrat as kodzatrat, /*number*/ 
ipr_fin_zatrat.kod_ipr_dog as kod_ipr_dog, /*number*/ 
ipr_fin_zatrat.kod_dog as kod_dog, /*number*/ 
ipr_fin_zatrat.kod_smet as kod_smet, /*number*/ 
ipr_fin_zatrat.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
ipr_fin_zatrat.plan_cost as dog_plan_cost, /*Сумма договора*//*number*/ 
ipr_fin_zatrat.plan_cost_nds as dog_plan_cost_nds, /*Сумма договора с НДС*//*number*/ 
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_zatrat 
select a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*//*key*/ 
a.kod_ipr_dog as kod_ipr_dog, /*number*/ 
kod_ipr_dog.kod_dog as kod_dog, /*number*/ 
a.kod_ipr as kod_ipr, /*number*/ 
kod_smet.kod_smet as kod_smet, /*number*/ 
kod_vid_zatrat.kodzatrat as kodzatrat, /*number*/ 
null as year,  
null as period,  
null as ym,  
a.plan_cost as plan_cost, /*Сумма договора*//*number*/ 
a.plan_cost_nds as plan_cost_nds, /*Сумма договора с НДС*//*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_vid_zatrat_titul 
select a.kod_vid_zatrat_titul as kod_vid_zatrat_titul, /*number*//*key*/ 
a.plan_cost as plan_cost, /*Сумма договора*//*number*/ 
a.kod_vid_zatrat as kod_vid_zatrat, /*number*/ 
a.kod_ipr_dog as kod_ipr_dog, /*number*/ 
a.kod_ipr as kod_ipr, /*number*/ 
a.plan_cost_nds as plan_cost_nds/*Сумма договора с НДС*//*number*/ 
 
from ipr_vid_zatrat_titul 
a 
--\ipr_vid_zatrat_titul 
) 
a 
--\ipr_vid_zatrat_titul 
left outer join 
( 
--ipr_dogs 
select a.kod_ipr_dog as kod_ipr_dog, /*number*//*key*/ 
a.kod_dog as kod_dog/*number*/ 
 
from ipr_dogs 
a 
--\ipr_dogs 
) 
kod_ipr_dog on a.kod_ipr_dog          =  kod_ipr_dog.kod_ipr_dog--\ipr_dogs 
left outer join 
( 
--ips_vid_zatrat 
select a.kodzatrat as kodzatrat, /**//*number*//*key*/ 
a.kod_smet as kod_smet/*number*/ 
 
from ips_vid_zatrat 
a 
--\ips_vid_zatrat 
) 
kod_vid_zatrat on a.kod_vid_zatrat          =  kod_vid_zatrat.kodzatrat--\ips_vid_zatrat 
left outer join 
( 
--ips_smet_structure 
select a.kod_smet as kod_smet, /*number*//*key*/ 
a.kod_parent as kod_parent/*number*/ 
 
from ips_smet_structure 
a 
--\ips_smet_structure 
) 
kod_smet on kod_vid_zatrat.kod_smet          =  kod_smet.kod_smet--\ips_smet_structure 
where 
(kod_smet.kod_parent is  null        ) ) 
ipr_fin_zatrat 
--\ipr_fin_zatrat 
) 
--\ 
union all 
( 
-- 
select ipr_fin_smet.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_smet.year as year,  
ipr_fin_smet.period as period,  
ipr_fin_smet.ym as ym,  
ipr_fin_smet.kodzatrat as kodzatrat,  
ipr_fin_smet.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_smet.kod_dog as kod_dog,  
ipr_fin_smet.kod_smet as kod_smet, /*number*/ 
ipr_fin_smet.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
ipr_fin_smet.summ_s as summ_s, /*Утверждённая сметная стоимость строительства объекта*//*number*/ 
ipr_fin_smet.sum_psd as sum_psd, /*number*/ 
ipr_fin_smet.sum_utvpsd as sum_utvpsd, /*number*/ 
ipr_fin_smet.sum_usr as sum_usr, /*number*/ 
null as summ_smet_tek,  
ipr_fin_smet.summ_nds_usr as summ_nds_usr, /*Сметная стоимость по УРС с  НДС*//*number*/ 
ipr_fin_smet.summ_10_nds_usr as summ_10_nds_usr, /*Сметная стоимость по УРС -10% с НДС*//*number*/ 
ipr_fin_smet.summ_30_nds_usr as summ_30_nds_usr, /*Сметная стоимость по УРС -30% с НДС*//*number*/ 
ipr_fin_smet.summ_usr as summ_usr, /*Сметная стоимость по УРС без НДС*//*number*/ 
ipr_fin_smet.summ_10_usr as summ_10_usr, /*Сметная стоимость по УРС -10% без НДС*//*number*/ 
ipr_fin_smet.summ_30_usr as summ_30_usr, /*Сметная стоимость по УРС -30% без НДС*//*number*/ 
ipr_fin_smet.summ_nds_psd as summ_nds_psd, /*Сметная стоимость по ПСД с НДС*//*number*/ 
ipr_fin_smet.summ_nds_psd_10 as summ_nds_psd_10, /*Сметная стоимость по ПСД -10% с НДС*//*number*/ 
ipr_fin_smet.summ_nds_psd_30 as summ_nds_psd_30, /*Сметная стоимость по ПСД -30% с НДС*//*number*/ 
ipr_fin_smet.summ_psd as summ_psd, /*Сметная стоимость по ПСД без НДС*//*number*/ 
ipr_fin_smet.summ_psd_10 as summ_psd_10, /*Сметная стоимость по ПСД -10% без НДС*//*number*/ 
ipr_fin_smet.summ_psd_30 as summ_psd_30, /*Сметная стоимость по ПСД -30% без НДС*//*number*/ 
ipr_fin_smet.summ_nds_utvpsd as summ_nds_utvpsd, /*Утвержденная сметная стоимость по ПСД с НДС*//*number*/ 
ipr_fin_smet.summ_utvpsd as summ_utvpsd, /*Утвержденная сметная стоимость по ПСД без НДС*//*number*/ 
ipr_fin_smet.mva_psd_30 as mva_psd_30, /*number*/ 
ipr_fin_smet.mva_30_usr as mva_30_usr, /*number*/ 
ipr_fin_smet.kl_km_psd_30 as kl_km_psd_30, /*number*/ 
ipr_fin_smet.kl_km_30_usr as kl_km_30_usr, /*number*/ 
ipr_fin_smet.vl_km_psd_30 as vl_km_psd_30, /*number*/ 
ipr_fin_smet.vl_km_30_usr as vl_km_30_usr, /*number*/ 
ipr_fin_smet.prch_psd_30 as prch_psd_30, /*number*/ 
ipr_fin_smet.prch_30_usr as prch_30_usr, /*number*/ 
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_smet 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
        decode(        a.kod_smet          ,  5          ,  4          ,  6          ,  4          ,  7          ,  4          ,  a.kod_smet        )       as kod_smet, /*number*/ 
null as kod_ipr_dog,  
null as kod_dog,  
null as kodzatrat,  
null as year,  
null as period,  
null as ym,  
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1        end       as summ_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_10        end       as summ_10_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_30        end       as summ_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_nds        end       as summ_nds_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_10_nds        end       as summ_10_nds_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_30_nds        end       as summ_30_nds_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.vl_km_30        end       as vl_km_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.kl_km_30        end       as kl_km_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.prch_30        end       as prch_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.mva_30        end       as mva_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd' )        ) )           then  a.summ1        end       as summ_psd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd' )        ) )           then  a.summ1_nds        end       as summ_nds_psd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_10' )        ) )           then  a.summ1        end       as summ_psd_10, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_10' )        ) )           then  a.summ1_nds        end       as summ_nds_psd_10, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.summ1        end       as summ_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.summ1_nds        end       as summ_nds_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.vl_km        end       as vl_km_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.kl_km        end       as kl_km_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.prch        end       as prch_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.mva        end       as mva_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'utvpsd' )        ) )           then  a.summ1        end       as summ_utvpsd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'utvpsd' )        ) )           then  a.summ1_nds        end       as summ_nds_utvpsd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'utvpsd' )        ) )           then  a.summ1_nds        end       as summ_s, /*Утверждённая сметная стоимость строительства объекта*//*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd' )        ) )           then  a.summ1_nds        end       as sum_psd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'utvpsd' )        ) )           then  a.summ1_nds        end       as sum_utvpsd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_nds        end       as sum_usr, /*number*/ 
null as pr_last_smet 
 
from ( 
--ipr_smet_struct_titul 
select a.kod_smet_titul as kod_smet_titul, /*number*//*key*/ 
a.kod_smet as kod_smet, /*number*/ 
a.kod_doc as kod_doc, /*number*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
        case                  when  (a.kod_smet          < (100) )           then  a.summ        end       as summ1, /*number*/ 
        case                  when  (a.kod_smet          < (100) )           then  a.summ_10        end       as summ1_10, /*number*/ 
        case                  when  (a.kod_smet          < (100) )           then  a.summ_30        end       as summ1_30, /*number*/ 
        case                  when  (a.kod_smet          =  6)           then  (        case                  when  (a.kod_smet          < (100) )           then  a.summ        end      )           else  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_nds        end      )         end       as summ1_nds, /*number*/ 
        case                  when  (a.kod_smet          =  6)           then  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_10        end      )           else  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_10_nds        end      )         end       as summ1_10_nds, /*number*/ 
        case                  when  (a.kod_smet          =  6)           then  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_30        end      )           else  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_30_nds        end      )         end       as summ1_30_nds, /*number*/ 
        case                  when  (a.kod_smet          =  (100) )           then  a.summ        end       as mva, /*number*/ 
        case                  when  (a.kod_smet          =  (100) )           then  a.summ_30        end       as mva_30, /*number*/ 
        case                  when  (a.kod_smet          =  (200) )           then  a.summ        end       as vl_km, /*number*/ 
        case                  when  (a.kod_smet          =  (200) )           then  a.summ_30        end       as vl_km_30, /*number*/ 
        case                  when  (a.kod_smet          =  (250) )           then  a.summ        end       as kl_km, /*number*/ 
        case                  when  (a.kod_smet          =  (250) )           then  a.summ_30        end       as kl_km_30, /*number*/ 
        case                  when  (a.kod_smet          =  (300) )           then  a.summ        end       as prch, /*number*/ 
        case                  when  (a.kod_smet          =  (300) )           then  a.summ_30        end       as prch_30/*number*/ 
 
from ipr_smet_struct_titul 
a 
--\ipr_smet_struct_titul 
) 
a 
--\ipr_smet_struct_titul 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc/*number*//*key*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on a.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_fix_smet 
select a.kod_fix_smet as kod_fix_smet, /*number*//*key*/ 
a.kod_doc_osn as kod_doc_osn, /*number*/ 
a.kod_doc_smet as kod_doc_smet, /*number*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
a.doc_target_str as doc_target_str/**//*string*/ 
 
from ipr_fix_smet 
a 
--\ipr_fix_smet 
) 
smet_doc on smet_doc.kod_doc_smet          =  kod_doc.kod_doc--\ipr_fix_smet 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip on a.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
a.kod_doc_osn as kod_doc_osn/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_ipr_data 
where 
(ipr_ipr_data.kod_titul_ip          =  smet_doc.kod_titul_ip)           and  (ipr_ipr_data.kod_doc_osn          =  smet_doc.kod_doc_osn)           and  (a.kod_smet          != 4) ) 
ipr_fin_smet 
--\ipr_fin_smet 
) 
--\ 
union all 
( 
-- 
select ipr_fin_smet_tek.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_smet_tek.year as year,  
ipr_fin_smet_tek.period as period,  
ipr_fin_smet_tek.ym as ym,  
ipr_fin_smet_tek.kodzatrat as kodzatrat,  
ipr_fin_smet_tek.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_smet_tek.kod_dog as kod_dog,  
ipr_fin_smet_tek.kod_smet as kod_smet, /*number*/ 
ipr_fin_smet_tek.pr_last_smet as pr_last_smet, /*number*/ 
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
ipr_fin_smet_tek.summ_smet as summ_smet_tek, /*number*/ 
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
ipr_fin_smet_tek.summ_nds_usr as summ_nds_usr_tek, /*Сметная стоимость по УРС с  НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_10_nds_usr as summ_10_nds_usr_tek, /*Сметная стоимость по УРС -10% с НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_30_nds_usr as summ_30_nds_usr_tek, /*Сметная стоимость по УРС -30% с НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_usr as summ_usr_tek, /*Сметная стоимость по УРС без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_10_usr as summ_10_usr_tek, /*Сметная стоимость по УРС -10% без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_30_usr as summ_30_usr_tek, /*Сметная стоимость по УРС -30% без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_nds_psd as summ_nds_psd_tek, /*Сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_nds_psd_10 as summ_nds_psd_10_tek, /*Сметная стоимость по ПСД -10% с НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_nds_psd_30 as summ_nds_psd_30_tek, /*Сметная стоимость по ПСД -30% с НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_psd as summ_psd_tek, /*Сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_psd_10 as summ_psd_10_tek, /*Сметная стоимость по ПСД -10% без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_psd_30 as summ_psd_30_tek, /*Сметная стоимость по ПСД -30% без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_nds_utvpsd as summ_nds_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД с НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.summ_utvpsd as summ_utvpsd_tek, /*Утвержденная сметная стоимость по ПСД без НДС (тек.)*//*number*/ 
ipr_fin_smet_tek.mva_psd_30 as mva_psd_30_tek, /*number*/ 
ipr_fin_smet_tek.mva_30_usr as mva_30_usr_tek, /*number*/ 
ipr_fin_smet_tek.kl_km_psd_30 as kl_km_psd_30_tek, /*number*/ 
ipr_fin_smet_tek.kl_km_30_usr as kl_km_30_usr_tek, /*number*/ 
ipr_fin_smet_tek.vl_km_psd_30 as vl_km_psd_30_tek, /*number*/ 
ipr_fin_smet_tek.vl_km_30_usr as vl_km_30_usr_tek, /*number*/ 
ipr_fin_smet_tek.prch_psd_30 as prch_psd_30_tek, /*number*/ 
ipr_fin_smet_tek.prch_30_usr as prch_30_usr_tek, /*number*/ 
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_smet_tek 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
smet_doc.pr_last as pr_last_smet, /*number*//*key*/ 
        decode(        a.kod_smet          ,  5          ,  4          ,  6          ,  4          ,  a.kod_smet        )       as kod_smet, /*number*/ 
a.summ1 as summ_smet, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1        end       as summ_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_10        end       as summ_10_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_30        end       as summ_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_nds        end       as summ_nds_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_10_nds        end       as summ_10_nds_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.summ1_30_nds        end       as summ_30_nds_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.vl_km_30        end       as vl_km_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.kl_km_30        end       as kl_km_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.prch_30        end       as prch_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'usr' )        ) )           then  a.mva_30        end       as mva_30_usr, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd' )        ) )           then  a.summ1        end       as summ_psd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd' )        ) )           then  a.summ1_nds        end       as summ_nds_psd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_10' )        ) )           then  a.summ1        end       as summ_psd_10, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_10' )        ) )           then  a.summ1_nds        end       as summ_nds_psd_10, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.summ1        end       as summ_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.summ1_nds        end       as summ_nds_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.vl_km        end       as vl_km_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.kl_km        end       as kl_km_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.prch        end       as prch_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'psd_30' )        ) )           then  a.mva        end       as mva_psd_30, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'utvpsd' )        ) )           then  a.summ1        end       as summ_utvpsd, /*number*/ 
        case                  when  (smet_doc.doc_target_str          =  (          upper( 'utvpsd' )        ) )           then  a.summ1_nds        end       as summ_nds_utvpsd, /*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog 
 
from ( 
--ipr_smet_struct_titul 
select a.kod_smet_titul as kod_smet_titul, /*number*//*key*/ 
a.kod_smet as kod_smet, /*number*/ 
a.kod_doc as kod_doc, /*number*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
        case                  when  (a.kod_smet          < (100) )           then  a.summ        end       as summ1, /*number*/ 
        case                  when  (a.kod_smet          < (100) )           then  a.summ_10        end       as summ1_10, /*number*/ 
        case                  when  (a.kod_smet          < (100) )           then  a.summ_30        end       as summ1_30, /*number*/ 
        case                  when  (a.kod_smet          =  6)           then  (        case                  when  (a.kod_smet          < (100) )           then  a.summ        end      )           else  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_nds        end      )         end       as summ1_nds, /*number*/ 
        case                  when  (a.kod_smet          =  6)           then  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_10        end      )           else  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_10_nds        end      )         end       as summ1_10_nds, /*number*/ 
        case                  when  (a.kod_smet          =  6)           then  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_30        end      )           else  (        case                  when  (a.kod_smet          < (100) )           then  a.summ_30_nds        end      )         end       as summ1_30_nds, /*number*/ 
        case                  when  (a.kod_smet          =  (100) )           then  a.summ        end       as mva, /*number*/ 
        case                  when  (a.kod_smet          =  (100) )           then  a.summ_30        end       as mva_30, /*number*/ 
        case                  when  (a.kod_smet          =  (200) )           then  a.summ        end       as vl_km, /*number*/ 
        case                  when  (a.kod_smet          =  (200) )           then  a.summ_30        end       as vl_km_30, /*number*/ 
        case                  when  (a.kod_smet          =  (250) )           then  a.summ        end       as kl_km, /*number*/ 
        case                  when  (a.kod_smet          =  (250) )           then  a.summ_30        end       as kl_km_30, /*number*/ 
        case                  when  (a.kod_smet          =  (300) )           then  a.summ        end       as prch, /*number*/ 
        case                  when  (a.kod_smet          =  (300) )           then  a.summ_30        end       as prch_30/*number*/ 
 
from ipr_smet_struct_titul 
a 
--\ipr_smet_struct_titul 
) 
a 
--\ipr_smet_struct_titul 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc/*number*//*key*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on a.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_tek_smet 
select kod_ipr.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_doc as kod_doc_smet, /*number*//*key*/ 
kod_doc_target.doc_target_str as doc_target_str, /*string*//*key*/ 
a.pr_last_smet as pr_last/*number*//*key*/ 
 
from ( 
--ipr_doc_by_ipr_by_t 
select a.kod_ipr as kod_ipr, /*number*/ 
a.kod_doc as kod_doc, /*number*/ 
ips_doc_type_target.kod_doc_target as kod_doc_target, /*number*//*key*/ 
        case                  when  (a.kod_doc_by_ipr          =          last_value(        a.kod_doc_by_ipr        )                over(         partition by        a.kod_ipr          , ips_doc_type_target.kod_doc_target        order by        kod_doc.has_smet          , kod_doc.doc_date          , a.kod_doc        ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING              )      )           then  1          else  0        end       as pr_last_smet/*number*/ 
 
from ( 
--ipr_doc_by_ipr 
select a.kod_doc_by_ipr as kod_doc_by_ipr, /*number*//*key*/ 
a.kod_doc as kod_doc, /*number*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_doc_by_ipr 
a 
--\ipr_doc_by_ipr 
) 
a 
--\ipr_doc_by_ipr 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date, /**//*date*/ 
a.kod_doc_type as kod_doc_type, /*number*/ 
        case                  when  (ipr_smet_struct_titul.kod_smet_titul  is not null        )           then  1          else  0        end       as has_smet 
 
from ipr_doc 
a 
--\ipr_doc 
left outer join 
( 
-- 
select ipr_smet_struct_titul.kod_doc as kod_doc, /*number*//*key*/ 
max(ipr_smet_struct_titul.kod_smet_titul)  as kod_smet_titul/*number*/ 
 
from ( 
--ipr_smet_struct_titul 
select a.kod_smet_titul as kod_smet_titul, /*number*//*key*/ 
a.kod_doc as kod_doc/*number*/ 
 
from ipr_smet_struct_titul 
a 
--\ipr_smet_struct_titul 
) 
ipr_smet_struct_titul 
--\ipr_smet_struct_titul 
 group by 
ipr_smet_struct_titul.kod_doc/*number*//*key*/ 
) 
ipr_smet_struct_titul on ipr_smet_struct_titul.kod_doc          =  a.kod_doc--\ 
) 
kod_doc on a.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ips_doc_type 
select a.kod_doc_type as kod_doc_type/*number*//*key*/ 
 
from ips_doc_type 
a 
--\ips_doc_type 
) 
kod_doc_type on kod_doc.kod_doc_type          =  kod_doc_type.kod_doc_type--\ips_doc_type 
left outer join 
( 
--ips_doc_type_target 
select a.kod_doc_type_target as kod_doc_type_target, /*number*//*key*/ 
a.kod_doc_type as kod_doc_type, /*number*/ 
a.kod_doc_target as kod_doc_target/*number*/ 
 
from ips_doc_type_target 
a 
--\ips_doc_type_target 
) 
ips_doc_type_target on ips_doc_type_target.kod_doc_type          =  kod_doc_type.kod_doc_type--\ips_doc_type_target 
) 
a 
--\ipr_doc_by_ipr_by_t 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
kod_ipr on (kod_ipr.kod_ipr          =  a.kod_ipr) --\ipr_ipr_data 
left outer join 
( 
--ips_doc_targets 
select a.kod_target as kod_target, /*number*//*key*/ 
        decode(        a.kod_target          ,  106          ,  'USR'          ,  277          ,  'PSD'          ,  107          ,  'PSD_10'          ,  283          ,  'PSD_30'          ,  278          ,  'UTVPSD'        )       as doc_target_str/*string*/ 
 
from ips_doc_targets 
a 
--\ips_doc_targets 
) 
kod_doc_target on a.kod_doc_target          =  kod_doc_target.kod_target--\ips_doc_targets 
where 
kod_doc_target.doc_target_str  is not null         group by 
kod_ipr.kod_titul_ip, /*number*//*key*/ 
a.kod_doc, /*number*//*key*/ 
kod_doc_target.doc_target_str, /*string*//*key*/ 
a.pr_last_smet/*number*//*key*/ 
) 
smet_doc on smet_doc.kod_doc_smet          =  kod_doc.kod_doc--\ipr_tek_smet 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip on a.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_ipr_data 
where 
(a.kod_titul_ip          =  smet_doc.kod_titul_ip)           and  (a.kod_smet          != 4) ) 
ipr_fin_smet_tek 
--\ipr_fin_smet_tek 
) 
--\ 
union all 
( 
-- 
select ipr_fin_doc.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_doc.year as year,  
ipr_fin_doc.period as period,  
ipr_fin_doc.ym as ym,  
ipr_fin_doc.kodzatrat as kodzatrat,  
ipr_fin_doc.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_doc.kod_dog as kod_dog,  
ipr_fin_doc.kod_smet as kod_smet,  
ipr_fin_doc.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
ipr_fin_doc.ipr_fin_doc as ipr_fin_doc, /*number*/ 
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_doc 
select a.kod_ipr as kod_ipr, /*number*/ 
1 as ipr_fin_doc, /*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as kod_smet,  
null as pr_last_smet 
 
from ( 
--ipr_doc_by_ipr 
select a.kod_doc_by_ipr as kod_doc_by_ipr, /*number*//*key*/ 
a.kod_ipr as kod_ipr/*number*/ 
 
from ipr_doc_by_ipr 
a 
--\ipr_doc_by_ipr 
) 
a 
--\ipr_doc_by_ipr 
) 
ipr_fin_doc 
--\ipr_fin_doc 
) 
--\ 
union all 
( 
-- 
select ipr_fin_tit_objects.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_tit_objects.year as year,  
ipr_fin_tit_objects.period as period,  
ipr_fin_tit_objects.ym as ym,  
ipr_fin_tit_objects.kodzatrat as kodzatrat,  
ipr_fin_tit_objects.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_tit_objects.kod_dog as kod_dog,  
ipr_fin_tit_objects.kod_smet as kod_smet,  
ipr_fin_tit_objects.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
ipr_fin_tit_objects.eksp_norm_podst as eksp_norm_podst, /*Нормативный срок службы, лет*//*number*/ 
ipr_fin_tit_objects.eksp_norm_lin as eksp_norm_lin, /*Нормативный срок службы, лет*//*number*/ 
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_tit_objects 
select ipr_ipr_data.kod_ipr as kod_ipr, /*number*//*key*/ 
        case                  when  (a.kod_vid_object          =  1)           then  a.ekspluat_normativ        end       as eksp_norm_podst, /*Нормативный срок службы, лет*//*number*/ 
        case                  when  (a.kod_vid_object          =  7)           then  a.ekspluat_normativ        end       as eksp_norm_lin, /*Нормативный срок службы, лет*//*number*/ 
null as year,  
null as period,  
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as kod_smet,  
null as pr_last_smet 
 
from ( 
--ipr_tituls_objects 
select a.kod_tituls_object as kod_tituls_object, /*number*//*key*/ 
a.kod_vid_object as kod_vid_object, /*number*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
a.ekspluat_normativ as ekspluat_normativ/*Нормативный срок службы, лет*//*number*/ 
 
from ipr_tituls_objects 
a 
--\ipr_tituls_objects 
) 
a 
--\ipr_tituls_objects 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip on a.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_ipr_data 
) 
ipr_fin_tit_objects 
--\ipr_fin_tit_objects 
) 
--\ 
union all 
( 
-- 
select ipr_fin_phis_param.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_phis_param.year as year, /*number*/ 
ipr_fin_phis_param.period as period, /*number*/ 
ipr_fin_phis_param.ym as ym, /*number*/ 
ipr_fin_phis_param.kodzatrat as kodzatrat,  
ipr_fin_phis_param.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_phis_param.kod_dog as kod_dog,  
ipr_fin_phis_param.kod_smet as kod_smet,  
ipr_fin_phis_param.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
ipr_fin_phis_param.phis_val12 as phis_coltr, /*Количествосиловых трансформаторов, шт*//*number*/ 
ipr_fin_phis_param.phis_val17 as phis_numtr, /*Марка силовых трансформаторов*//*string*/ 
ipr_fin_phis_param.phis_val13 as phis_mvatr, /*Мощность, МВА*//*number*/ 
ipr_fin_phis_param.phis_val1 as phis_typel, /*Тип опор*//*string*/ 
ipr_fin_phis_param.phis_val3 as phis_numl, /*Марка кабеля*//*string*/ 
ipr_fin_phis_param.phis_val14 as phis_km_vl, /*ВЛ,км*//*number*/ 
ipr_fin_phis_param.phis_val18 as phis_km_kl, /*КЛ,км*//*number*/ 
ipr_fin_phis_param.phis_val16 as phis_other, /*Иные объекты (др. единицы измерений)*//*number*/ 
null as kap_lim_sum,  
null as fp_lim_sum_nds,  
null as vvod_lim_sum 
 
from ( 
--ipr_fin_phis_param 
select phis_param.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
ipr_ipr_data.kod_ipr as kod_ipr, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYY'))         as year, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'YYYYMM'))         as ym, /*number*/ 
          to_number(to_char( kod_doc.doc_date ,'MM'))         as period, /*number*/ 
          cut_num( (        case                  when  (phis_param.kod_phis_param          in  12)           then  phis_param.value        end      )  )         as phis_val12, /*Количествосиловых трансформаторов, шт*//*number*/ 
        case                  when  (phis_param.kod_phis_param          in  17)           then  phis_param.value        end       as phis_val17, /*Марка силовых трансформаторов*//*string*/ 
          cut_num( (        case                  when  (phis_param.kod_phis_param          in  13)           then  phis_param.value        end      )  )         as phis_val13, /*Мощность, МВА*//*number*/ 
        case                  when  (phis_param.kod_phis_param          in  1)           then  phis_param.value        end       as phis_val1, /*Тип опор*//*string*/ 
        case                  when  (phis_param.kod_phis_param          in  3)           then  phis_param.value        end       as phis_val3, /*Марка кабеля*//*string*/ 
          cut_num( (        case                  when  (phis_param.kod_phis_param          in  14)           then  phis_param.value        end      )  )         as phis_val14, /*ВЛ,км*//*number*/ 
          cut_num( (        case                  when  (phis_param.kod_phis_param          in  18)           then  phis_param.value        end      )  )         as phis_val18, /*КЛ,км*//*number*/ 
          cut_num( (        case                  when  (phis_param.kod_phis_param          in  16)           then  phis_param.value        end      )  )         as phis_val16, /*Иные объекты (др. единицы измерений)*//*number*/ 
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as kod_smet,  
null as pr_last_smet 
 
from ( 
--ipv_phys_param_values 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
a.kod_doc as kod_doc, /*number*/ 
a.kod_phis_param as kod_phis_param, /*number*/ 
a.value as value/**//*string*/ 
 
from ipv_phys_param_values 
a 
--\ipv_phys_param_values 
) 
phis_param 
--\ipv_phys_param_values 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.doc_date as doc_date/**//*date*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc on phis_param.kod_doc          =  kod_doc.kod_doc--\ipr_doc 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip/*number*//*key*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
) 
kod_titul_ip on phis_param.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  kod_titul_ip.kod_titul_ip--\ipr_ipr_data 
where 
(phis_param.kod_doc  is not null        ) ) 
ipr_fin_phis_param 
--\ipr_fin_phis_param 
) 
--\ 
union all 
( 
-- 
select ipr_fin_lim.kod_ipr as kod_ipr, /*number*//*key*/ 
ipr_fin_lim.year as year, /*number*/ 
ipr_fin_lim.period as period, /*number*/ 
ipr_fin_lim.ym as ym,  
ipr_fin_lim.kodzatrat as kodzatrat,  
ipr_fin_lim.kod_ipr_dog as kod_ipr_dog,  
ipr_fin_lim.kod_dog as kod_dog,  
ipr_fin_lim.kod_smet as kod_smet,  
ipr_fin_lim.pr_last_smet as pr_last_smet,  
null as kap_sum,  
null as kap_sum_nds,  
null as kap_sum_wait,  
null as kap_sum_nds_wait,  
null as fp_sum,  
null as fp_sum_nds,  
null as fp_sum_wait,  
null as fp_sum_nds_wait,  
null as god_vvod,  
null as vvod_sum,  
null as vvod_sum_nds,  
null as vvod_sum_wait,  
null as vvod_sum_nds_wait,  
null as pow_km,  
null as pow_mba,  
null as pow_cnt,  
null as pow_km_del,  
null as pow_mba_del,  
null as pow_km_pr,  
null as pow_mba_pr,  
null as pow_cnt_pr,  
null as pow_mba_fact,  
null as pow_km_fact,  
null as pow_cnt_fact,  
null as pow_km_fact_pr,  
null as pow_mba_fact_pr,  
null as pow_cnt_fact_pr,  
null as pow_mba_wait,  
null as pow_km_wait,  
null as pow_cnt_wait,  
null as pow_cnt_proch_wait,  
null as pow_km_wait_pr,  
null as pow_mba_wait_pr,  
null as pow_cnt_wait_pr,  
null as pow_cnt_proch_wait_pr,  
null as pow_cnt_proch,  
null as pow_cnt_proch_pr,  
null as pow_cnt_proch_fact,  
null as pow_cnt_proch_fact_pr,  
null as pow_km_kl,  
null as pow_km_kl_del,  
null as pow_km_kl_fact,  
null as pow_km_kl_ekspl,  
null as pow_km_kl_pr,  
null as pow_km_kl_fact_pr,  
null as pow_km_kl_wait,  
null as pow_km_kl_wait_pr,  
null as pow_any,  
null as pow_any_fact,  
null as kap_sum_nds_fact_a,  
null as kap_nds_fact_a,  
null as kap_sum_fact_per,  
null as kap_nds_fact_per,  
null as kap_sum_fact_per_pr,  
null as kap_nds_fact_per_pr,  
null as kap_sum_fact_sp,  
null as kap_nds_fact_sp,  
null as kap_sum_fact_sp_pr,  
null as kap_nds_fact_sp_pr,  
null as kap_sum_fact_pri,  
null as kap_nds_fact_pri,  
null as kap_sum_fact_pri_pr,  
null as kap_nds_fact_pri_pr,  
null as fp_fact_sum,  
null as fp_fact_sum_pr,  
null as fp_fact_nds,  
null as fp_fact_nds_pr,  
null as fp_sum_fact_per1,  
null as fp_nds_fact_per1,  
null as fp_sum_fact_sp1,  
null as fp_nds_fact_sp1,  
null as fp_sum_fact_pri1,  
null as fp_nds_fact_pri1,  
null as vvod_sum_fact,  
null as km,  
null as km_kl,  
null as pow,  
null as other,  
null as ipr_fin_ipr,  
null as summ_s,  
null as sum_psd,  
null as sum_utvpsd,  
null as sum_usr,  
null as summ_smet_tek,  
null as summ_nds_usr,  
null as summ_10_nds_usr,  
null as summ_30_nds_usr,  
null as summ_usr,  
null as summ_10_usr,  
null as summ_30_usr,  
null as summ_nds_psd,  
null as summ_nds_psd_10,  
null as summ_nds_psd_30,  
null as summ_psd,  
null as summ_psd_10,  
null as summ_psd_30,  
null as summ_nds_utvpsd,  
null as summ_utvpsd,  
null as mva_psd_30,  
null as mva_30_usr,  
null as kl_km_psd_30,  
null as kl_km_30_usr,  
null as vl_km_psd_30,  
null as vl_km_30_usr,  
null as prch_psd_30,  
null as prch_30_usr,  
null as summ_nds_usr_tek,  
null as summ_10_nds_usr_tek,  
null as summ_30_nds_usr_tek,  
null as summ_usr_tek,  
null as summ_10_usr_tek,  
null as summ_30_usr_tek,  
null as summ_nds_psd_tek,  
null as summ_nds_psd_10_tek,  
null as summ_nds_psd_30_tek,  
null as summ_psd_tek,  
null as summ_psd_10_tek,  
null as summ_psd_30_tek,  
null as summ_nds_utvpsd_tek,  
null as summ_utvpsd_tek,  
null as mva_psd_30_tek,  
null as mva_30_usr_tek,  
null as kl_km_psd_30_tek,  
null as kl_km_30_usr_tek,  
null as vl_km_psd_30_tek,  
null as vl_km_30_usr_tek,  
null as prch_psd_30_tek,  
null as prch_30_usr_tek,  
null as dz,  
null as kz,  
null as nzs_saldo,  
null as dog_plan_cost,  
null as dog_plan_cost_nds,  
null as ipr_fin_doc,  
null as eksp_norm_podst,  
null as eksp_norm_lin,  
null as phis_coltr,  
null as phis_numtr,  
null as phis_mvatr,  
null as phis_typel,  
null as phis_numl,  
null as phis_km_vl,  
null as phis_km_kl,  
null as phis_other,  
ipr_fin_lim.value_osv as kap_lim_sum, /*Лимит освоения*//*number*/ 
ipr_fin_lim.value_fin_nds as fp_lim_sum_nds, /*Лимит финансирования*//*number*/ 
ipr_fin_lim.value_vvod as vvod_lim_sum/*Лимит ввод в ОФ*//*number*/ 
 
from ( 
--ipr_fin_lim 
select a.kod_ipr as kod_ipr, /*number*/ 
a.year as year, /*number*/ 
a.kvartal *3         as period, /*number*/ 
a.value_osv as value_osv, /*number*/ 
a.value_fin_nds as value_fin_nds, /*number*/ 
a.value_vvod as value_vvod, /*number*/ 
null as ym,  
null as kodzatrat,  
null as kod_ipr_dog,  
null as kod_dog,  
null as kod_smet,  
null as pr_last_smet 
 
from ( 
--ipr_finsrc_lim 
select a.kod_ipr as kod_ipr, /*number*/ 
a.kvartal as kvartal, /**//*number*/ 
a.value_osv as value_osv, /**//*number*/ 
a.year as year, /**//*number*/ 
a.value_fin_nds as value_fin_nds, /**//*number*/ 
a.in_titul as in_titul, /**//*number*/ 
a.value_vvod as value_vvod/**//*number*/ 
 
from ipr_finsrc_lim 
a 
--\ipr_finsrc_lim 
) 
a 
--\ipr_finsrc_lim 
where 
(nvl(a.in_titul,0)           =  1) ) 
ipr_fin_lim 
--\ipr_fin_lim 
) 
--\ 
) 
--\ 
) 
backbone 
--\ 
left outer join 
( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
a.kod_doc_osn as kod_doc_osn, /*number*/ 
        nvl(        (        nullif(        a.kod_parent          , -1 )      )           , a.kod_titul_ip )       as kod_main_titul/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
kod_ipr on (kod_ipr.kod_ipr          =  backbone.kod_ipr) --\ipr_ipr_data 
left outer join 
( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
        case                  when  (ipr_ipr_data.kod_child_titul is  null        )           then  ((        nvl(        a.kod_parent_sbor          , a.kod_titul_ip )      )           ||  '-'          ||  ipr_ipr_data.kod_razdel)         end       as kod_sbor_titul/*string*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
left outer join 
( 
-- 
select ipr_ipr_data.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
max(ipr_ipr_data.kod_razdel)  as kod_razdel, /*number*/ 
max(ipr_ipr_data.kod_child_titul)  as kod_child_titul/*number*/ 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
a.kod_razdel as kod_razdel, /*number*/ 
        case                  when  ((        nullif(        a.kod_parent          , -1 )      )   is not null        )           then  a.kod_titul_ip        end       as kod_child_titul/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
ipr_ipr_data 
--\ipr_ipr_data 
 group by 
ipr_ipr_data.kod_titul_ip/*number*//*key*/ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  a.kod_titul_ip--\ 
) 
kod_main_titul1 on kod_ipr.kod_main_titul          =  kod_main_titul1.kod_titul_ip--\ipr_titul_ip 
left outer join 
( 
--ipr_titul_ip_sbor 
select a.kod_sbor_titul as kod_sbor_titul, /*string*//*key*/ 
max(a.kod_parent_sbor_ext)  as kod_titul_ip/*number*/ 
 
from ( 
--ipr_titul_ip 
select a.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
        case                  when  (ipr_ipr_data.kod_child_titul is  null        )           then  ((        nvl(        a.kod_parent_sbor          , a.kod_titul_ip )      )           ||  '-'          ||  ipr_ipr_data.kod_razdel)         end       as kod_sbor_titul, /*string*/ 
        nvl(        a.kod_parent_sbor          , (        case                  when  (a.is_sbor          =  1)           then  a.kod_titul_ip        end      )  )       as kod_parent_sbor_ext/*number*/ 
 
from ipr_titul_ip 
a 
--\ipr_titul_ip 
left outer join 
( 
-- 
select ipr_ipr_data.kod_titul_ip as kod_titul_ip, /*number*//*key*/ 
max(ipr_ipr_data.kod_razdel)  as kod_razdel, /*number*/ 
max(ipr_ipr_data.kod_child_titul)  as kod_child_titul/*number*/ 
 
from ( 
--ipr_ipr_data 
select a.kod_ipr as kod_ipr, /*number*//*key*/ 
a.kod_titul_ip as kod_titul_ip, /*number*/ 
a.kod_razdel as kod_razdel, /*number*/ 
        case                  when  ((        nullif(        a.kod_parent          , -1 )      )   is not null        )           then  a.kod_titul_ip        end       as kod_child_titul/*number*/ 
 
from ipr_ipr_data 
a 
--\ipr_ipr_data 
) 
ipr_ipr_data 
--\ipr_ipr_data 
 group by 
ipr_ipr_data.kod_titul_ip/*number*//*key*/ 
) 
ipr_ipr_data on ipr_ipr_data.kod_titul_ip          =  a.kod_titul_ip--\ 
) 
a 
--\ipr_titul_ip 
 group by 
a.kod_sbor_titul/*string*//*key*/ 
) 
kod_sbor_titul1 on kod_sbor_titul1.kod_sbor_titul          =  kod_main_titul1.kod_sbor_titul--\ipr_titul_ip_sbor 
left outer join 
( 
--ipr_doc 
select a.kod_doc as kod_doc, /*number*//*key*/ 
a.god_ip as god_ip/**//*number*/ 
 
from ipr_doc 
a 
--\ipr_doc 
) 
kod_doc_osn on kod_ipr.kod_doc_osn          =  kod_doc_osn.kod_doc--\ipr_doc 
 group by 
backbone.kod_ipr, /*number*//*key*/ 
backbone.year, /*number*//*key*/ 
backbone.period, /*number*//*key*/ 
backbone.ym, /*number*//*key*/ 
backbone.kodzatrat, /*number*//*key*/ 
kod_ipr.kod_titul_ip, /*number*//*key*/ 
kod_ipr.kod_main_titul, /*number*//*key*/ 
kod_sbor_titul1.kod_titul_ip, /*number*//*key*/ 
backbone.kod_ipr_dog, /*number*//*key*/ 
backbone.kod_dog, /*number*//*key*/ 
        case                  when  ((        nullif(        backbone.kod_dog          , -1 )      )   is not null        )           then  ('1-'          ||  backbone.kod_dog)           else  (        case                  when  (backbone.kod_ipr_dog  is not null        )           then  ('2-'          ||  backbone.kod_ipr_dog)         end      )         end      , /*string*//*key*/ 
backbone.kod_smet, /*number*//*key*/ 
backbone.pr_last_smet/*key*/ 
) 
ovr1 
--\ 
 group by 
ovr1.kod_ipr, /*number*/ 
ovr1.year, /*number*/ 
ovr1.period, /*number*/ 
ovr1.ym, /*number*/ 
ovr1.kodzatrat, /*number*/ 
ovr1.kod_titul_ip, /*number*/ 
ovr1.kod_main_titul, /*number*/ 
ovr1.kod_titul_ip_sb, /*number*/ 
ovr1.kod_ipr_dog, /*number*/ 
ovr1.kod_dog, /*number*/ 
ovr1.kod_dog_i, /*string*/ 
ovr1.kod_smet, /*number*/ 
ovr1.pr_last_smet 
) 
a 
--\ipr_fin_body_united 
--\ipr_fin_body_united_test2 
