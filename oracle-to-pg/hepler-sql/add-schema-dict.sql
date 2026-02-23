with tt as (
    select table_schema || '.' || table_name as full_name,
        table_schema,
        table_name
    from information_schema. "tables" t
    where t.table_type = 'BASE TABLE'
),
ttf as (
    select *
    from tt
    where full_name not in (
            'temp.br_account',
            --'tep_rasch.br_account',
            --'tep_nsi.dk_building_norm',
            'tep_build.dk_building_norm',
            --'tep_nsi.dk_npot_q3_par',
            'tep_dog.dk_npot_q3_par',
            --'tep_nsi.ds_tempgr',
            'tep_dog.ds_tempgr',
            'e_point.hk_voltage',
            --'ec_account.hk_voltage',
            'ec_account.hr_reglament_pp',
            --'dog_tplan.hr_reglament_pp',
            'ec_account.hs_23',
            --'dog_object.hs_23',
            'tep_rasch.kk_pokrit',
            --'tep_nsi.kk_pokrit',
            'fin_fk.kk_tep_el',
            --'globset.kk_tep_el',
            --'dog_tplan.kr_plan',
            'ec_account.kr_plan',
            'dog_tplan.ks_reglament_variant',
            --'fin_nastr.ks_reglament_variant',
            --'ec_calc.nr_account_sost',
            'ec_account.nr_account_sost',
            --'globset.sk_edizm',
            'fin_fk.sk_edizm',
            --'fin_doc.sk_vid_real',
            'fin_fk.sk_vid_real',
            --'fin_doc.sk_vid_sf',
            'fin_fk.sk_vid_sf',
            --'ec_account.sr_avans',
            'ec_avans.sr_avans',
            'ec_account.sr_facvip',
            --'fin_doc.sr_facvip',
            --'fin_peni.ss_penni_proc',
            'fin_fk.ss_penni_proc',
            --'fin_doc.ss_state',
            'fin_fk.ss_state',
            'public.test_load',
            'pres.test_load',
            'temp.texts',
            'search.texts',
            'temp.tmp_amavr',
            'public.tmp_amavr',
            'e_grid.v_json_result',
            'asuse.v_json_result',
            'pres.v_json_result',
            'e_grid.v_result',
            'address.v_result',
            'asuse.v_result'
        )
)
select *
from ttf