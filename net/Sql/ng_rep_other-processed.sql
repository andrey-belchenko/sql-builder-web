CREATE OR REPLACE PACKAGE BODY ng_rep_other

IS
   -- возвращает данные из отчета ATS (из nr_ats_data, куда данные попадают из биллинга)
   -- параметр p_is_positive_digit = 1 или -1, т.к. не во всех отчетах нужно выводить некоторые данные в отрицательном значении
   -- Если нужно отсечь по субъекту РФ, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kod_adr_m'
   FUNCTION get_tbl_ats_data(p_ym_beg NUMBER, p_ym_end NUMBER, p_coeff NUMBER default -1) RETURN tblAts PIPELINED
   IS
     cnt_kod_adr_m NUMBER;  -- кол-во выбранных субъектов РФ
   BEGIN
    cnt_kod_adr_m := ng_rep_common.get_count_value_param('p_kod_adr_m');

    FOR curr IN
        (
           SELECT   CASE WHEN nvl(g.kod_price_zone,0) in (63,64) THEN 62 ELSE nvl(g.kod_price_zone,0) END  as kod_price_zone -- код ценовой зоны (Хабаровск и Дальний восток сейчас находятся во 2 ЦЗ)
                  , max(nvl((CASE WHEN g.kod_price_zone in (63,64) THEN 'Вторая ценовая зона' ELSE i.name END),'Пустая зона')) as zone_name  -- ценовая зона (наименование)
                  , nvl(g_gp.name,'-') as gtp_gp_name   -- наименование ГТП ГП (по '-' формируем данные ГТП ГП для второго листа отчета)
                  , max(COALESCE(g.kod_rek, g_gp.kod_rek)) as gtp_gp_kod_region   -- код региона (добавлено 20.11.2025). Корректировка 26.12.2025 - Берем с ГТП и только если нет на ГТП берем с ГТП ГП.
                  , max(m.name_p) as gtp_gp_region_name      -- наименование региона (добавлено 20.11.2025)
              , r.kod_gtp as kod_gtp
              , max(r.gtp_id) as kod_gtp_letter
              , max(nvl(g.name,'-')) as gtp_name
              , g.date_admission as date_admission
              , r.ym

              , SUM(CASE WHEN k.name = 'buy_rsv' AND r.value_type = 'E' THEN nvl(value,0) ELSE 0 END) as buy_rsv_value  -- kod_ats_data = 1
              , SUM(CASE WHEN k.name = 'buy_rsv' AND r.value_type = 'E' THEN nvl(money,0) ELSE 0 END) as buy_rsv_money
              , SUM(CASE WHEN k.name = 'sell_rsv' AND r.value_type = 'E' THEN nvl(value,0) ELSE 0 END) * p_coeff as sell_rsv_value  -- kod_ats_data = 2
              , SUM(CASE WHEN k.name = 'sell_rsv' AND r.value_type = 'E' THEN nvl(money,0) ELSE 0 END) * p_coeff as sell_rsv_money
              , SUM(CASE WHEN k.name = 'fine_pwr' AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) * -1 as fine_pwr_money -- kod_ats_data = 3

              , SUM(CASE WHEN k.name = 'buy_pwr'  AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_value  -- kod_ats_data = 4
              , SUM(CASE WHEN k.name = 'buy_pwr'  AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_money
              , SUM(CASE WHEN k.name = 'buy_pwr_frsvr' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_frsvr_value  -- kod_ats_data = 5
              , SUM(CASE WHEN k.name = 'buy_pwr_frsvr' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_frsvr_money

              , SUM(CASE WHEN k.name = 'fine_pwr_not_ready_vr' AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) * -1  as fine_pwr_not_ready_vr_money -- kod_ats_data = 6
              , SUM(CASE WHEN k.name in ('buy_pwr_frsvr', 'fine_pwr_not_ready_vr') AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) * -1  as penalty_pwr_dvr_money -- стоимость штрафов по генераторам для п. 4.2.4

              , SUM(CASE WHEN k.name = 'penalty_pwr_over' AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) as penalty_pwr_over_money  -- kod_ats_data = 8
              , SUM(CASE WHEN k.name = 'fine_pwr_not_ready' AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) as fine_pwr_not_ready_money  -- kod_ats_data = 21
              , SUM(CASE WHEN k.name in ('penalty_pwr_over', 'fine_pwr_not_ready') AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) * -1 as penalty_pwr_kom_money  -- стоимость штрафов по генераторам для п. 4.2.5

              , SUM(CASE WHEN k.name = 'buy_pwr_over' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_over_value  -- kod_ats_data = 7
              , SUM(CASE WHEN k.name = 'buy_pwr_trans' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_trans_money  -- kod_ats_data = 9
              , SUM(CASE WHEN k.name in ('buy_pwr_over', 'buy_pwr_trans') AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_kom_value  -- kod_ats_data = 7+9 для п.4.2.5 for the value
              , SUM(CASE WHEN k.name in ('buy_pwr_over', 'buy_pwr_trans') AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_kom_money  -- kod_ats_data = 7+9 для п.4.2.5 for money

              , SUM(CASE WHEN k.name = 'fine_pwr_kommod' AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END) * -1 as fine_pwr_kommod_money -- kod_ats_data = 12
              , SUM(CASE WHEN k.name = 'buy_pwr_kommod' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_kommod_value  -- kod_ats_data = 13
              , SUM(CASE WHEN k.name = 'buy_pwr_kommod' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_kommod_money

              , SUM(CASE WHEN k.name = 'buy_pwr_con' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_con_value  -- kod_ats_data = 14
              , SUM(CASE WHEN k.name = 'buy_pwr_con' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_con_money
              , SUM(CASE WHEN k.name = 'fine_pwr_con' AND r.value_type = 'P' THEN nvl(penalty,0) ELSE 0 END)  * -1  as fine_pwr_con_money  -- kod_ats_data = 15

              , SUM(CASE WHEN k.name = 'buy_pwr_dpm' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_dpm_value  -- kod_ats_data = 16
              , SUM(CASE WHEN k.name = 'buy_pwr_dpm' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_dpm_money
              , SUM(CASE WHEN k.name = 'buy_pwr_dpmga' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_dpmga_value  -- kod_ats_data = 17
              , SUM(CASE WHEN k.name = 'buy_pwr_dpmga' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_dpmga_money
              , SUM(CASE WHEN k.name = 'buy_br' AND r.value_type = 'E' THEN nvl(value,0) ELSE 0 END) as buy_br_value  -- kod_ats_data = 18
              , SUM(CASE WHEN k.name = 'buy_br' AND r.value_type = 'E' THEN nvl(money,0) ELSE 0 END) as buy_br_money

              , SUM(CASE WHEN k.name = 'sell_br' AND r.value_type = 'E' THEN nvl(value,0) ELSE 0 END) * p_coeff as sell_br_value  -- kod_ats_data = 19
              , SUM(CASE WHEN k.name = 'sell_br' AND r.value_type = 'E' THEN nvl(money,0) ELSE 0 END) * p_coeff as sell_br_money

              , SUM(CASE WHEN k.name = 'rd' AND r.value_type = 'E' THEN nvl(value,0) ELSE 0 END) as rd_value_e  -- kod_ats_data = 20
              , SUM(CASE WHEN k.name = 'rd' AND r.value_type = 'E' THEN nvl(money,0) ELSE 0 END) as rd_money_e
              , SUM(CASE WHEN k.name = 'rd' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as rd_value_p  -- kod_ats_data = 20 with value_type = 'P'
              , SUM(CASE WHEN k.name = 'rd' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as rd_money_p

              -- добавлено 18.09.2024 по SD 72859(1)
              , SUM(CASE WHEN k.name = 'upz_sell_energy' AND r.value_type = 'PE' THEN nvl(value,0) ELSE 0 END) as upz_sell_energy_value_pe  -- kod_atsd_data = 10 with value_type = 'PE'  (плановое значение)
              , SUM(CASE WHEN k.name = 'upz_sell_energy' AND r.value_type = 'E'  THEN nvl(value,0) ELSE 0 END) as upz_sell_energy_value   -- value for kod_atsd_data = 10 with value_type = 'E'  (фактическое значение)
              , SUM(CASE WHEN k.name = 'upz_sell_energy' AND r.value_type = 'E'  THEN nvl(money,0) ELSE 0 END) as upz_sell_energy_money   -- money for kod_atsd_data = 10 with value_type = 'E'  (фактическое значение)
              , SUM(CASE WHEN k.name = 'upz_sell_power'  AND r.value_type = 'P'  THEN nvl(value,0) ELSE 0 END) as upz_sell_power_value       -- value for kod_atsd_data = 11 with value_type = 'P'
              , SUM(CASE WHEN k.name = 'upz_sell_power'  AND r.value_type = 'P'  THEN nvl(money,0) ELSE 0 END) as upz_sell_power_money       -- money for kod_atsd_data = 11 with value_type = 'P'

              -- добавлено 23.10.2025 VeraK
              , SUM(CASE WHEN k.name = 'fact_rd_dv' AND r.value_type = 'E' THEN nvl(value,0) ELSE 0 END) as fact_rd_dv_value_e
              , SUM(CASE WHEN k.name = 'fact_rd_dv' AND r.value_type = 'E' THEN nvl(money,0) ELSE 0 END) as fact_rd_dv_money_e
              , SUM(CASE WHEN k.name = 'rd_dv' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as rd_dv_value_p
              , SUM(CASE WHEN k.name = 'rd_dv' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as rd_dv_money_p
              , SUM(CASE WHEN k.name = 'likom_buy' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as likom_buy_value
              , SUM(CASE WHEN k.name = 'likom_buy' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as likom_buy_money
              , SUM(CASE WHEN k.name = 'buy_pwr_kommod_rd' AND r.value_type = 'P' THEN nvl(value,0) ELSE 0 END) as buy_pwr_kommod_rd_value
              , SUM(CASE WHEN k.name = 'buy_pwr_kommod_rd' AND r.value_type = 'P' THEN nvl(money,0) ELSE 0 END) as buy_pwr_kommod_rd_money
              , SUM(CASE WHEN k.name = 'upr_shrt' THEN nvl(money,0) ELSE 0 END) as upr_shrt_money

        FROM nr_ats_data r
             INNER JOIN nk_ats_data k ON r.kod_ats_data = k.kod_ats_data
             INNER JOIN hs_gtp g ON r.kod_gtp = g.kod_gtp
             LEFT JOIN hs_gtp g_gp ON g.kod_gtp_gp = g_gp.kod_gtp
             LEFT JOIN kk_interval i ON g.kod_price_zone = i.kodinterval
             LEFT JOIN adr_m m ON COALESCE(g.kod_rek, g_gp.kod_rek) = m.kod_m    -- Корректировка 26.12.2025 - Берем с ГТП и только если нет на ГТП берем с ГТП ГП.
       WHERE  r.ym between p_ym_beg AND p_ym_end
         AND ((cnt_kod_adr_m = 0) Or (cnt_kod_adr_m > 0
         AND COALESCE( g.kod_rek, g_gp.kod_rek) in (Select val From vr_number_array Where array_id = 'p_kod_adr_m')))  -- (добавлено 20.11.2025)
       GROUP BY g.kod_price_zone, g_gp.name, r.kod_gtp, g.date_admission, r.ym
       ORDER BY g.kod_price_zone
      )
     LOOP
          PIPE ROW(curr);
      END LOOP;
      RETURN;
   END get_tbl_ats_data;  -- end function get_tbl_ats_data
  ------------------------ ---------------------------------------------------


  -- Возвращает строку всех уникальных телефонов всех сотрудников контрагента
  -- Используется в отчетах 1073 и 20024 Рязань
 FUNCTION get_phone_all(p_kodp NUMBER) return VARCHAR2
   IS
   phone_all VARCHAR2(500);
 BEGIN

 WITH
 pre as ( -- берем все телефоны по каждому сотруднику
  SELECT kodp, trim(REPLACE(e.tel || ',' || e.m_tel || ',' || e.p_tel,';',',')) tel
    FROM kr_employee e
    WHERE e.kodp = p_kodp
    )
 , pre2 as (
         select  kodp, tel
          from pre
         group by kodp, tel
       )
  , pre3 as (
    SELECT kodp, trim(regexp_substr(tel, '[^,]+', 1, lines.column_value)) as  tel
     FROM pre2,
       TABLE (CAST (MULTISET
                       (SELECT LEVEL FROM dual CONNECT BY instr(tel, ',', 1, LEVEL - 1) > 0
                        ) AS sys.odciNumberList )) lines
    )
 , pre4 as (
     SELECT kodp, tel
       FROM pre3
   WHERE tel is not null
      group by kodp, tel
        )
  SELECT -- listagg(trim(tel), ', ' || CHR(13) || CHR(10)) within group(order by kodp) as tel_all
         listagg(trim(tel), ', ') within group(order by kodp) as tel_all
   INTO phone_all
   FROM pre4;

  RETURN phone_all;

 END;

-- Возвращает строку всех уникальных e-mail всех сотрудников контрагента
 FUNCTION get_email_all(p_kodp NUMBER) return VARCHAR2
   IS
   email_all VARCHAR2(500);
 BEGIN

 WITH
 pre as ( -- берем все телефоны по каждому сотруднику
  SELECT kodp, trim(REPLACE(e.e_mail,';',',')) as email
    FROM kr_employee e
    WHERE e.kodp = p_kodp
    )
 , pre2 as (
         select  kodp, email
          from pre
         group by kodp, email
       )
  , pre3 as (
    SELECT kodp, trim(regexp_substr(email, '[^,]+', 1, lines.column_value)) as  email
     FROM pre2,
       TABLE (CAST (MULTISET
                       (SELECT LEVEL FROM dual CONNECT BY instr(email, ',', 1, LEVEL - 1) > 0
                        ) AS sys.odciNumberList )) lines
    )
 , pre4 as (
     SELECT kodp, email
       FROM pre3
   WHERE email is not null
      group by kodp, email
        )
  SELECT -- listagg(trim(email), ',' || CHR(13) || CHR(10)) within group(order by kodp) as email_all
         listagg(trim(email), ', ') within group(order by kodp) as email_all
   INTO email_all
   FROM pre4;

  RETURN email_all;

 END;  -- end FUNCTION get_email_all(p_kodp NUMBER)

   ------------------------------------ ---------------------------------------------------


   -- Используется в Рязани в отчете № 8 (72544.xml).  Заполняет вр. таблицу rr_temp данными о сбытовых надбавках по полугодиям двух лет
   PROCEDURE get_rate_per_part_year(p_year1 NUMBER, p_year2 NUMBER, p_kod_m NUMBER default -10000, p_kodp NUMBER default -10000)
   IS
     year1 VARCHAR2(10);
     year2 VARCHAR2(10);
     is_kod_m NUMBER;
     is_kodp  NUMBER;
     s_data_id     rr_temp.skod%type;
   BEGIN
     year1 := to_char(p_year1,'9999');
     year2 := to_char(p_year2,'9999');
     IF p_kod_m = -10000  THEN is_kod_m := 0; ELSE is_kod_m := 1; END IF;
     IF p_kodp  = -10000  THEN is_kodp  := 0; ELSE is_kodp := 1; END IF;
     s_data_id    :='3d8b60b1-7254-416a-ac16-8c99c9d294fb';

    DELETE FROM rr_temp WHERE skod = s_data_id;

    INSERT INTO rr_temp (skod, n1, s1, n2,s2,n3,s3, n4,n5,n6,n7,n8,n9,n10,n11,n12,n13,n14,n15)
    SELECT s_data_id,
           kod_fo, fo_name,         -- код и наименование федерального округа
           kod_m, region_name,      -- код и наименование региона
           kodp, guarantp_name,     -- код и наименование гарантирующего поставщика
           gr1_part1_year1,         -- ставка тарифа по 1 группе максимальной мощности за первое полугодие первого года
           gr1_part2_year1,         -- ставка тарифа по 1 группе максимальной мощности за второе полугодие первого года
           gr1_part1_year2,         -- ставка тарифа по 1 группе максимальной мощности за первое полугодие второго года
           gr1_part2_year2,         -- ставка тарифа по 1 группе максимальной мощности за второе полугодие второго года
           gr2_part1_year1, gr2_part2_year1, gr2_part1_year2, gr2_part2_year2,
           gr3_part1_year1, gr3_part2_year1, gr3_part1_year2, gr3_part2_year2
          from
              ( SELECT f.kod_fo as kod_fo,
                      max(f.name_fo) as fo_name,
                      a.kod_m,
                      max(a.name_p) as region_name,
                      p.kodp as kodp,
                      max(p.name) as guarantp_name,
                      CASE t.val_from WHEN 670 THEN 2
                                      WHEN 10000 THEN 3
                                      ELSE 1
                      END as gr_max_power,  -- группа максимальной мощности
                      MAX(CASE WHEN to_date(CONCAT('30.06.', year1),'dd.mm.yyyy') between dat_beg AND dat_end THEN COALESCE(rate1,0) ELSE 0 END) as part1_year1,  -- c 01.01.2023 по 30.06.2023
                      MAX(CASE WHEN to_date(CONCAT('31.12.', year1),'dd.mm.yyyy') between dat_beg AND dat_end THEN COALESCE(rate1,0) ELSE 0 END) as part2_year1,
                      MAX(CASE WHEN to_date(CONCAT('30.06.', year2),'dd.mm.yyyy') between dat_beg AND dat_end THEN COALESCE(rate1,0) ELSE 0 END) as part1_year2,
                      MAX(CASE WHEN to_date(CONCAT('31.12.', year2),'dd.mm.yyyy') between dat_beg AND dat_end THEN COALESCE(rate1,0) ELSE 0 END) as part2_year2
               FROM ks_tarif t
                  INNER JOIN adr_m a ON t.prizn_selo = a.kod_m
                  INNER JOIN k_fedo f ON a.kod_fo=f.kod_fo
                  INNER JOIN kr_org p ON t.kodp = p.kodp
                  INNER JOIN ks_tarif_rate tr ON t.tarif = tr.tarif
              WHERE tip_tarif_sost = 5
                  AND tarif_parent is not null
                  AND (val_from > 0 or val_to > 0)
                  AND t.pr_active=0
                  AND (is_kod_m = 0 OR a.kod_m = p_kod_m)
                  AND (is_kodp = 0 OR p.kodp = p_kodp)
              GROUP BY f.kod_fo, a.kod_m, p.kodp, t.val_from
             ) a
         PIVOT
          (  max(part1_year1) as part1_year1, max(part2_year1) as part2_year1, max(part1_year2) as part1_year2, max(part2_year2) as part2_year2
             FOR gr_max_power IN (1 as gr1, 2 as gr2, 3 as gr3)
          ) p;

   END get_rate_per_part_year;


   -- Заполняет временную таблицу rr_rep_po данными по полезному отпуску в разрезе регионов по основному регламенту
   -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_dep'
   -- Если нужно отсечь по договорам, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kod_dog'
   -- Если нужно отсечь по потребителю, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kodp'
   -- Если нужно отсечь по субъекту РФ, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kod_adr_m'
   -- p_no_check_price = 1 если нужна проверка: price is not null, иначе собираем отчет до появления фактических цен текущего месяца
   -- Используется в Рязани. Отчеты: № 15, 32, 1189,...
   PROCEDURE po_per_region(p_ym_beg NUMBER, p_ym_end NUMBER, p_is_only_active NUMBER default 0)
    IS
        cnt_dep     NUMBER;   -- кол-во выбранных отделений
        cnt_kodp    NUMBER;   -- кол-во выбранных потребителей
        cnt_kod_adr_m NUMBER;  -- кол-во выбранных субъектов РФ
        cnt_kod_dog   NUMBER;  -- кол-во выбранных договоров
        p_no_check_price NUMBER; -- не нужна ли проверка: price is not null

    BEGIN
      DELETE rr_rep_po;

      cnt_dep  := ng_rep_common.get_count_value_param('p_dep');
      cnt_kodp := ng_rep_common.get_count_value_param('p_kodp');
      cnt_kod_adr_m := ng_rep_common.get_count_value_param('p_kod_adr_m');
      cnt_kod_dog := ng_rep_common.get_count_value_param('p_kod_dog');

     IF (ng_rep_common.get_count_value_param('p_no_check_price') = 1)   --  p_no_check_price = 1 если нужна проверка: price is not null, иначе собираем отчет до появления фактических цен текущего месяца
       THEN Select COALESCE(val,0) into p_no_check_price  From vr_number_array Where array_id = 'p_no_check_price';
       ELSE p_no_check_price:=0;
     END IF;

     INSERT INTO rr_rep_po ( kod_region, region_name, pr_byt,
                              kod_dog,    ndog,
                              kodp,       payer_name,    -- Код и наименование потребителя
                              kod_okved,
                              kod_numobj,
                              ym,
                              pr_opt,    edizm,
                              voltage,    volt_abbr,
                              cust,       price,
                              nach,       nal,      nachisl,
                              max_power
                            )
      WITH
       pars AS ( SELECT -- 2024.06 as p_ym_beg,
                      -- 2024.06 as p_ym_end
                  p_ym_beg As p_ym_beg,
                  p_ym_end As p_ym_end,     -- для тестирования
                  kg.ym_first_day(p_ym_beg) as p_date_beg,
                  kg.ym_last_day(p_ym_end) as p_date_end
              FROM dual
              )
     , pre as (
              SELECT  nk_adress.kf_get_sf(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as kod_region
                     , nk_adress.kp_get_region_name(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as kod_region_name
                     , Decode(nvl(a.tarif,0), 0, 0, kg_tarif.f_byt_cached(a.tarif)) as pr_byt    -- категория потребителя (население или прочие: признак быт (0- не население, 1 - население))
                     , a.kod_dog
                     , a.ndog as ndog
                     , p.kodp
                     , p.name as payer_name
                     , p.kod_okved as kod_okved
                     , a.kod_numobj as kod_numobj
                     , a.ym  as ym
                     , (SELECT pr_opt FROM hs_gtp  WHERE kod_gtp = hg_common.get_gtp (a.kod_numobj, ym)) as pr_opt
                     , a.edizm
                     , (CASE WHEN vol.voltage IS NULL THEN -9 ELSE vol.voltage END) as voltage
                     , nvl(vol.abbr,'-')  as volt_abbr
                     , a.cust   -- натуральные показатели
                     , a.price
                     , a.nach as nach       -- начислено без НДС
                     , a.nal
                     , a.nachisl
                     , nvl(hg_dogr.get_pmax_numobj(a.kod_numobj, 2, kg.ym_first_day(a.ym), 0, a.voltage),0) AS pmax
                FROM nv_account a
                   INNER JOIN kr_dogovor d ON a.kod_dog = d.kod_dog
                   INNER JOIN kr_dogovor dp ON d.kod_dog_fin = dp.kod_dog
                   LEFT JOIN hk_voltage vol ON a.voltage = vol.voltage
                   LEFT JOIN kr_numobj kr_n ON a.kod_numobj = kr_n.kod_numobj
                   LEFT JOIN kr_object ob ON kr_n.kod_obj = ob.kod_obj
                   LEFT JOIN kr_payer p ON a.kodp = p.kodp
                   Cross Join pars
               Where ((cnt_dep = 0) Or (cnt_dep > 0 And dp.dep in (Select val From vr_number_array Where array_id = 'p_dep')))
                   AND ((cnt_kodp = 0) Or (cnt_kodp > 0 And p.kodp in (Select val From vr_number_array Where array_id = 'p_kodp')))
                   AND (cnt_kod_dog = 0 OR (cnt_kod_dog > 0 AND dp.kod_dog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_dog'))) -- добавлено по заявке 76731
                   AND a.ym BETWEEN pars.p_ym_beg AND pars.p_ym_end
                   AND a.vid_real = 2 AND not (a.vid_t = 22)
                   AND (p_no_check_price = 1 OR (p_no_check_price=0 AND a.price IS not null))  -- добавлено для отчета № 32 по SD: 75956(3) 06.02.2026
                   AND a.rym is null
                   AND ((p_is_only_active = 0  AND dp.pr_active in (0,2))
                         OR (p_is_only_active = 1 AND p_date_beg BETWEEN d.dat_dog AND COALESCE(d.dat_fin, SYSDATE + 5000)
                             OR p_date_end BETWEEN d.dat_dog AND COALESCE(d.dat_fin, SYSDATE + 5000)))   -- активные договоры на дату отчета 23.04.2025
            )
         SELECT   kod_region
                , kod_region_name
                , pr_byt    -- категория потребителя (население или прочие: признак быт (0- не население, 1 - население))
                , kod_dog
                , ndog
                , kodp
                , payer_name
                , kod_okved
                , kod_numobj
                , ym
                , pr_opt
                , edizm
                , voltage
                , volt_abbr
                , cust   -- натуральные показатели
                , price
                , nach as nach       -- начислено без НДС
                , nal
                , nachisl
                , (CASE WHEN pmax < 670 THEN 'до 670 кВт'   -- 'менее 670 кВт'
                             WHEN pmax >= 670 AND pmax < 10000  THEN 'от 670 до 10000 кВт'
                             ELSE 'не менее 10000 кВт'
                         END) as max_power
         FROM pre
        WHERE  (cnt_kod_adr_m = 0) Or (cnt_kod_adr_m > 0 And kod_region in (Select val From vr_number_array Where array_id = 'p_kod_adr_m'))
       ;

   END po_per_region;  -- end procedure po_per_region


   -- Заполняет временную таблицу rr_rep_po данными по полезному отпуску в разрезе регионов по доп. регламентам
   -- p_kod_typevariant - вариант расчета: 0 - продажа (доп. вариант факт); 1- покупка;  2 - цены ГП
   -- p_add_rule_opt - доп. правило определения опт-а ГТП (1 - для отчета 16 в Рязани: для ЦЗ (ценовых зон) все ГТП с установленным флажком "опт".
   --                  для НЦЗ (дальний восток и Коми) без анализа флажка и в случае наличия ГТП ГП.)
   -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_dep'
   -- Если нужно отсечь по потребителю, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kodp'
   -- Если нужно отсечь по субъекту РФ, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kod_adr_m'
   -- ! Используется в Рязани. Отчеты: № 16,...
   PROCEDURE po_per_region_add_regl(p_ym_beg NUMBER, p_ym_end NUMBER, p_kod_typevariant NUMBER default 1, p_add_rule_opt NUMBER default 0)
    IS
        cnt_dep     number;   -- кол-во выбранных отделений
        cnt_kodp    number;   -- кол-во выбранных потребителей
        cnt_kod_adr_m number;  -- кол-во выбранных субъектов РФ
    BEGIN
      DELETE rr_rep_po;

      cnt_dep  := ng_rep_common.get_count_value_param('p_dep');
      cnt_kodp := ng_rep_common.get_count_value_param('p_kodp');
      cnt_kod_adr_m := ng_rep_common.get_count_value_param('p_kod_adr_m');

      INSERT INTO rr_rep_po ( kod_region, region_name, pr_byt,
                              kod_dog,    ndog,
                              kodp,       payer_name,    -- Код и наименование потребителя
                              kod_okved,
                              ym,         pr_opt,    edizm,
                              voltage,    volt_abbr,
                              cust,
                              max_power
                            )
      WITH
       pars AS ( SELECT -- 2024.06 as p_ym_beg,
                      -- 2024.06 as p_ym_end
                  p_ym_beg As p_ym_beg,
                  p_ym_end As p_ym_end     -- для тестирования
              FROM dual
              )
     , pre as (
              SELECT  nk_adress.kf_get_sf(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as kod_region
                     , nk_adress.kp_get_region_name(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as kod_region_name
                     , Decode(nvl(a.tarif,0), 0, 0, kg_tarif.f_byt_cached(a.tarif)) as pr_byt    -- категория потребителя (население или прочие: признак быт (0- не население, 1 - население))
                     , a.kod_dog
                     , a.ndog as ndog
                     , p.kodp
                     , p.name as payer_name
                     , p.kod_okved as kod_okved
                     , a.ym  as ym
                     , (SELECT (CASE WHEN p_add_rule_opt = 1 AND kod_price_zone in (63, 64) AND kod_gtp_gp is not null
                            THEN 1 ELSE pr_opt
                            END) pr_opt
                         FROM hs_gtp  WHERE kod_gtp = hg_common.get_gtp_add_regl(a.kod_numobj, ym, p_kod_typevariant)) as pr_opt
                     , a.edizm
                     , (CASE WHEN vol.voltage IS NULL THEN -9 ELSE vol.voltage END) as voltage
                     , nvl(vol.abbr,'-')  as volt_abbr
                     , a.cust   -- натуральные показатели
                     , nvl(hg_dogr.get_pmax_numobj(a.kod_numobj, 2, kg.ym_first_day(a.ym), 0, a.voltage),0) AS pmax
                FROM nv_account a
                   inner join hr_reglament_pp r ON a.kod_reglament=r.kod_reglament
                   inner join hr_calc_variant v ON v.kod_variant=r.kod_variant

                   INNER JOIN kr_dogovor d ON a.kod_dog = d.kod_dog
                   INNER JOIN kr_dogovor dp ON d.kod_dog_fin = dp.kod_dog
                   LEFT JOIN hk_voltage vol ON a.voltage = vol.voltage
                   LEFT JOIN kr_numobj kr_n ON a.kod_numobj = kr_n.kod_numobj
                   LEFT JOIN kr_object ob ON kr_n.kod_obj = ob.kod_obj
                   LEFT JOIN kr_payer p ON a.kodp = p.kodp
                   Cross Join pars
               Where ((cnt_dep = 0) Or (cnt_dep > 0 And dp.dep in (Select val From vr_number_array Where array_id = 'p_dep')))
                   AND ((cnt_kodp = 0) Or (cnt_kodp > 0 And p.kodp in (Select val From vr_number_array Where array_id = 'p_kodp')))
                   AND a.ym BETWEEN pars.p_ym_beg AND pars.p_ym_end
                   AND v.kod_typevariant = p_kod_typevariant    -- 1 - Покупка
                   AND a.price IS not null
                   AND a.rym is null
                   AND dp.pr_active in (0,2)
            )
         SELECT   kod_region
                , kod_region_name
                , pr_byt    -- категория потребителя (население или прочие: признак быт (0- не население, 1 - население))
                , kod_dog
                , ndog
                , kodp
                , payer_name
                , kod_okved
                , ym
                , pr_opt
                , edizm
                , voltage
                , volt_abbr
                , cust   -- натуральные показатели
                , (CASE WHEN pmax < 670 THEN 'до 670 кВт'   -- 'менее 670 кВт'
                             WHEN pmax >= 670 AND pmax < 10000  THEN 'от 670 до 10000 кВт'
                             ELSE 'не менее 10000 кВт'
                         END) as max_power
         FROM pre
        WHERE  (cnt_kod_adr_m = 0) Or (cnt_kod_adr_m > 0 And kod_region in (Select val From vr_number_array Where array_id = 'p_kod_adr_m'))
       ;

   END po_per_region_add_regl;  -- end procedure po_per_region_add_regl



   -- Заполняет временную таблицу rr_rep_po данными по полезному отпуску в разрезе регионов и составляющих начислений
   -- is_clear (по умолчанию = 1): 0 - будем добавлять записи в таблицу rr_rep_po;   1 - нужно удалить записи из таблицы rr_per_po
   -- p_vid_calc (вид расчета; по умолчанию = 0): 0 - факт; 1 - корректировка; 2 -  перерасчеты (не совсем перерасчеты, а договоры с составляющей = -2)
   -- is_dog - флаг. 1- добавляем фильтр по договорам из таблицы vr_number_array c array_id = 'p_kod_dog'
   -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_dep'
   -- Если нужно отсечь по виду договора, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_vdoc'
   -- Если нужно отсечь по субъекту РФ, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kod_adr_m'
   -- !!! Используется в отчете № 31 у РЯЗАНИ.
   -- НЕ изменять для другого использования!
   PROCEDURE po_per_region_sost(p_ym_beg NUMBER, p_ym_end NUMBER, is_clear NUMBER := 1, p_vid_calc NUMBER := 0, is_dog NUMBER := 0)
   IS
        cnt_dep     number;   -- кол-во выбранных отделений
        cnt_kod_adr_m number;  -- кол-во выбранных субъектов РФ
        cnt_kod_tipdog      number;  -- кол-во видов договора
    BEGIN
      IF is_clear=1 THEN  DELETE rr_rep_po;  END IF;

      cnt_dep  := ng_rep_common.get_count_value_param('p_dep');
      cnt_kod_adr_m := ng_rep_common.get_count_value_param('p_kod_adr_m');
      cnt_kod_tipdog := ng_rep_common.get_count_value_param('p_kod_tipdog');

      INSERT INTO rr_rep_po ( kod_region, region_name,
                              pr_byt,   -- категория потребителя (население или прочие: признак быт (0- не население, 1 - население))
                              vid_calc, -- вид расчета;  0 - факт; 1 - корректировка; 2 -  перерасчеты
                              kod_dog,    ndog,
                              kod_numobj,
                              kodp,       payer_name,    -- Код и наименование потребителя
                              kod_okved,
                              ym,         pr_opt,    edizm,
                              voltage,    volt_abbr,
                              cust,       price,
                              nal,      nachisl,
                              num_precision, -- 'Точность округления (число знаков после запятой)' добавлено 21.12.2025
                              max_power,
                              gr_str_sf,  -- Номер группы строки в СФ     добавлено 21.12.2025
                              pr_hand
                            )
      WITH
       pars AS ( SELECT -- 2024.06 as p_ym_beg,
                      -- 2024.06 as p_ym_end
                  p_ym_beg As p_ym_beg,
                  p_ym_end As p_ym_end     -- для тестирования
              FROM dual
              )
      , pre  AS (
                    SELECT a.kod_account
                          , nk_adress.kf_get_sf(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as kod_region
                          , nk_adress.kp_get_region_name(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as region_name
                          , d.kod_dog
                          , d.ndog as ndog
                          , a.kod_numobj
                          , d.kodp
                          , (SELECT pr_opt FROM hs_gtp  WHERE kod_gtp = hg_common.get_gtp(a.kod_numobj, a.ym)) as pr_opt
                          , nvl(hg_dogr.get_pmax_numobj(a.kod_numobj, 2, kg.ym_first_day(a.ym), 0, a.voltage),0) AS pmax
                    FROM nv_account a
                      INNER JOIN kr_dogovor d ON a.kod_dog_fin = d.kod_dog
                      INNER JOIN ks_vdog vdog ON d.kod_vdog = vdog.kod_vdog
                      LEFT JOIN kr_numobj kr_n ON a.kod_numobj = kr_n.kod_numobj
                      LEFT JOIN kr_object ob ON kr_n.kod_obj = ob.kod_obj
                      LEFT JOIN sk_nachisl n ON a.vid_t = n.vid_t
                      Cross Join pars
                   WHERE  n.vid_real = 2
                     AND a.ym BETWEEN pars.p_ym_beg AND pars.p_ym_end
                     AND a.price IS not null
                     AND ((cnt_dep = 0)  Or (cnt_dep > 0  And d.dep  in (Select val From vr_number_array Where array_id = 'p_dep')))
                     AND ((cnt_kod_tipdog = 0) Or (cnt_kod_tipdog > 0  And vdog.kod_tipdog  in (Select val From vr_number_array Where array_id = 'p_kod_tipdog')))
                     AND ((is_dog = 0)  Or (is_dog = 1  And d.kod_dog in (Select val From vr_number_array Where array_id = 'p_kod_dog')))
                     AND d.pr_active in (0,2)
                  )
   , itog as (
          SELECT  pre.kod_region as kod_region
              , pre.region_name as region_name
              , Decode(nvl(a.tarif,0), 0, 0, kg_tarif.f_byt_cached(a.tarif)) as pr_byt
              , p_vid_calc as vid_calc
              , pre.kod_dog
              , pre.ndog
              , pre.kod_numobj
              , p.kodp
              , p.name as payer_name
              , p.kod_okved as kod_okved
              , (CASE WHEN p_vid_calc = 1 THEN kg.ym_add(a.ym,-1) ELSE a.ym END)  as ym
              , pre.pr_opt
                         --   , (CASE WHEN n.edizm = 23 THEN 4 ELSE n.edizm END) as edizm
             -- , (CASE WHEN n.edizm = 23 or fr.gr_cust = 0 THEN 4 ELSE n.edizm END) as edizm     -- если СФ схлопнутая, то начисления по мощности должны учитываться в э/э
          ,   (CASE WHEN n.edizm =23
                           OR (n.edizm = 3 AND
                             sum(CASE WHEN fvt.size_doc = 2 AND fr.edizm = 3 THEN 1 ELSE 0 END) OVER(PARTITION BY fr.kod_sf, group_min order by fr.kod_sf, group_min)=  -- count_kvt_in_group,
                             sum(CASE WHEN fvt.size_doc = 2  THEN 1 ELSE 0 END) OVER(PARTITION BY fr.kod_sf, group_min order by fr.kod_sf, group_min) -- count_all_in_group
                            )
                     THEN 3 ELSE 4 END) as edizm   -- если в минимальном СФ в group_min ВСЕ строки с edizm=3, то деньги учитываем в мощности, иначе в э/э
              , (CASE WHEN vol.voltage IS NULL THEN -9 ELSE vol.voltage END) as voltage
              , nvl(vol.abbr,'-')  as volt_abbr
              , (CASE WHEN n.edizm = 23 or (fr.gr_cust = 0 and n.edizm = 3) THEN 0 ELSE nvl(sost.cust, a.cust) END) as cust  -- натуральные показатели (fr.gr_cust = 0 - в СФ строки нет)
                        --  , (CASE WHEN p_vid_calc = 0 THEN (CASE WHEN sost.price IS NULL   THEN a.price  ELSE a.price - sost.price     END) ELSE nvl(sost.price, a.price) END)
              , 0 as price  -- пока решение не найдено в шаблоне nach или nachisl делим cust
              , (CASE WHEN p_vid_calc in (0,3) THEN (CASE WHEN sost.nal IS NULL     THEN a.nal     ELSE a.nal - sost.nal         END) ELSE nvl(sost.nal, a.nal) END) as nal
              , (CASE WHEN p_vid_calc in (0,3) THEN (CASE WHEN sost.nachisl IS NULL THEN a.nachisl ELSE a.nachisl - sost.nachisl END) ELSE nvl(sost.nachisl, a.nachisl) END) as nachisl

              , (CASE WHEN fvt.size_doc = 2 THEN kg_billing_settings.getbs(a.kod_dog,'GR_SF_TARIF_PRECISION')
                                            ELSE kg_billing_settings.getbs(a.kod_dog,'PRICE_KOLV_ZN') END) as num_precision -- точность округления (кол-во знаков после запятой (настройка на договоре)) добавлено 21.12.2025
              , (CASE WHEN pre.pmax < 670 THEN 'до 670 кВт'   -- 'менее 670 кВт'
                      WHEN pre.pmax >= 670 AND pre.pmax < 10000  THEN 'от 670 до 10000 кВт'
                      ELSE 'не менее 10000 кВт'
                   END) as max_power
              ,  decode(fvt.size_doc, 0, fr.group_small
                                    , 1, fr.group_big
                                    , 2, fr.group_min
                                    , fr.group_small_rym) as gr_str_sf
              , a.pr_hand
          FROM  pre
             INNER JOIN nv_account a ON pre.kod_account = a.kod_account
             LEFT JOIN ( select  nal.kod_account as kod_account
                                , sum(nal.cust)as cust
                                , max (nal.price) as price /**/
                                , round(sum(nal.nal),2) as nal
                                , round(sum(nal.nachisl),2) as nachisl
                           from  nr_account s
                               inner join nv_account_sost_nal nal ON s.kod_account = nal.kod_account
                               cross join pars
                          where  s.ym BETWEEN pars.p_ym_beg AND pars.p_ym_end
                             AND p_vid_calc in(0,2,3) AND nal.tip_tarif_sost = -2         -- перерасчеты
                          group by nal.kod_account--, nal.price
                      ) sost  ON a.kod_account = sost.kod_account
             LEFT JOIN sr_facras_text frt ON a.kod_account = frt.kod_account   -- добавлено 12.12.2024 по SD: 72943(1)
             LEFT JOIN fin.sr_facras fr ON frt.kod_ras = fr.kod_ras         -- добавлено 12.12.2024
             LEFT join fin.sr_facvip fv ON fr.kod_sf = fv.kod_sf and a.kod_dog = fv.kod_dog and a.ym = fv.ym                -- добавлено 10.07.2025 (по рекомендации Александра Д.)
             LEFT JOIN sr_facvip_text fvt ON fv.kod_sf = fvt.kod_sf         -- добавлено 10.07.2025 по SD: 72943(2)

             LEFT JOIN hk_voltage vol ON a.voltage = vol.voltage
             LEFT JOIN kr_payer p ON pre.kodp = p.kodp
             LEFT JOIN sk_nachisl n ON a.vid_t = n.vid_t
             Cross Join pars
         WHERE   n.vid_real = 2
            AND ((p_vid_calc in (0,3) AND a.rym is null )  -- факт или факт предыдущего месяца
              OR (p_vid_calc = 1 AND a.rym is not null) --a.rym = kg.ym_add(a.ym,-1))      -- корректировки
              OR (p_vid_calc = 2 AND sost.kod_account is not null))
            AND  (cnt_kod_adr_m = 0) Or (cnt_kod_adr_m > 0 And kod_region in (Select val From vr_number_array Where array_id = 'p_kod_adr_m'))
            )

      SELECT  max(i.kod_region) as kod_region
              , max(i.region_name) as region_name
              , i.pr_byt as pr_byt
              , i.vid_calc as vid_calc
              , i.kod_dog as kod_dog
              , max(i.ndog) as ndog
              , max(i.kod_numobj) as kod_numobj
              , max(i.kodp) as kodp
              , max(i.payer_name) as payer_name
              , max(i.kod_okved) as kod_okved
              , i.ym  as ym
              , max(i.pr_opt) as pr_opt
              , i.edizm as edizm   -- если в минимальной СФ в group_min ВСЕ строки с edizm=3, то деньги учитываем в мощности, иначе в э/э
              , i.voltage as voltage
              , max(i.volt_abbr)  as volt_abbr
              , sum(i.cust) as cust  -- натуральные показатели (fr.gr_cust = 0 - в СФ строки нет)
              , max(i.price) as price  -- пока решение не найдено в шаблоне nach или nachisl делим cust
              , sum(i.nal) as nal
              , sum(i.nachisl) as nachisl
              , max(i.num_precision) as num_precision -- кол-во знаков после запятой (настройка на договоре) добавлено 18.12.2025
              , i.max_power as max_power
              , i.gr_str_sf as gr_str_sf
              , i.pr_hand as pr_hand
          FROM  itog i
       GROUP BY  i.pr_byt, i.edizm, i.vid_calc, i.ym, i.kod_dog, i.voltage, i.max_power, i.gr_str_sf, i.pr_hand
       ;
   END po_per_region_sost;  -- end procedure po_per_region_sost


 -- Заполняет временную таблицу rr_rep_emp данными о сотрудниках потребителя
 -- Обязательно нужно сначала заполнить вр. таблицу rr_rep_dog
 -- Используется в отчет № 2004 (Рязань)
 PROCEDURE payer_employee
 IS
 BEGIN
   DELETE rr_rep_emp;
   INSERT INTO rr_rep_emp ( payer_id,
                            director_fio,
                            director_name_fun,
                            director_tel,
                            buh_fio,
                            buh_name_fun,
                            buh_tel,
                            tel_all,
                            email_all
                           )
 SELECT  p.kodp as payer_id
       , CASE WHEN e.kod_dolzhfun = 14 THEN kg.fio(e.fio)    END as director_fio
       , CASE WHEN e.kod_dolzhfun = 14 THEN dolzh.name_fun    END as director_name_fun
       , CASE WHEN e.kod_dolzhfun = 14 THEN  e.tel    END as director_tel
       , CASE WHEN e.kod_namedolzh = 17 THEN kg.fio(e.fio)    END as buh_fio
       , CASE WHEN e.kod_namedolzh = 17 THEN ndolzh.name    END as buh_name_fun
       , CASE WHEN e.kod_namedolzh = 17 THEN e.tel    END as buh_tel
       , get_phone_all(p.kodp) as tel_all
       , get_email_all(p.kodp) as email_all
   FROM rr_rep_dog d   -- предварительно заполненная вр. таблица данными по договорам
    INNER JOIN kr_payer p ON d.payer_id = p.kodp
    LEFT JOIN kr_employee e ON  p.kodp=e.kodp  AND e.pr_active = 0
    LEFT JOIN kk_dolzhfun dolzh ON e.kod_dolzhfun = dolzh.kod_dolzhfun
    LEFT JOIN ks_namedolzh ndolzh ON e.kod_namedolzh = ndolzh.kod_namedolzh
    ;

 END payer_employee;


 -- Заполняет временную таблицу rr_rep_po данными по покупке на розничном рынке.
 -- Используется в Рязани для отчета № 58, 14 (Форма 46-ЭЭ), 55 (Макет 4.41), ...
 PROCEDURE po_on_retail(p_ym_beg NUMBER, p_ym_end NUMBER, p_dep NUMBER)
 IS
 BEGIN
    DELETE rr_rep_po;
    INSERT INTO rr_rep_po ( kod_region, region_name, region_name_sorting,
                           kodp,       payer_name,    -- Код и наименование потребителя
                           kod_dog,    ndog,
                           dog_dat_beg,
                           kod_vdoc,
                           ym,

                           kod_sbit,
                           payer_name_sbit,
                           cust,       cust_kvt,
                           nach,       nach_kvt
                         )
    SELECT
            ( nk_adress.kf_get_sf(d.kod_d_dog)) as kod_region  -- для розницы регион берем из договора
           , max( nk_adress.kp_get_region_name(d.kod_d_dog)) as region_name -- name_p из adr_m
           , max( nk_adress.kf_get_sf_name(nk_adress.kf_get_sf(d.kod_d_dog))) as region_name_sorting   -- получаем name_s из adr_m, чтобы, например, после г.Москва шла Московская область

           , p.kodp as kodp            -- в отчете № 58 поле называется: kod_gtp
           , max(p.name) as payer_name -- в отчете № 58 поле называется: gtp_name (для розницы это наименование контрагента договора э.э.)
           , d.kod_dog
           , max(d.ndog) as ndog
           , max(d.dat_dog) as dog_dat_beg -- в отчете № 58 поле называется: date_admission
           , max(vdogs.kod_vdoc) as kod_vdoc
           , fr.rym as ym

           , ps.kodp as kod_sbit
           , max(ps.name) as payer_name_sbit  -- в отчете № 58 поле называется: kod_gtp_letter -- для розницы это наименование поставщика из доп. регламента

           , sum(CASE WHEN nn.edizm in(4,23) THEN fr.cust ELSE 0 END) as cust
           , sum(CASE WHEN nn.edizm = 3      THEN fr.cust/1000 ELSE 0 END) as cust_kvt
           , round(sum(CASE WHEN nn.edizm in(4,23) THEN fr.nachisl - COALESCE(nal.nal,0) ELSE 0 END),2) as nach
           , round(sum(CASE WHEN nn.edizm in(3)    THEN fr.nachisl - COALESCE(nal.nal,0) ELSE 0 END),2) as nach_kvt

     FROM kr_dogovor ds
         INNER JOIN ks_vdog vdogs ON ds.kod_vdog = vdogs.kod_vdog
         INNER JOIN kr_payer ps ON ds.kodp = ps.kodp
         INNER JOIN fin.sr_facvip fv ON ds.kod_dog = fv.kod_dog
         inner join fin.sr_facras fr ON fv.kod_sf = fr.kod_sf
         INNER JOIN kr_numobj n ON fr.kod_numobj = n.kod_numobj
         INNER JOIN kr_dogovor d ON TO_CHAR(n.num_obj,'FM000') = d.ndog
         INNER JOIN ks_vdog vdog ON d.kod_vdog = vdog.kod_vdog
         INNER JOIN kr_payer p ON d.kodp = p.kodp
         INNER JOIN sk_nachisl nn ON fr.vid_t = nn.vid_t
           LEFT JOIN (SELECT SUM(nvl(nal,0)) as nal, kod_ras FROM sr_facras_nal GROUP BY kod_ras) nal ON fr.kod_ras = nal.kod_ras
    WHERE ds.dep = p_dep      AND d.dep =  p_dep
        AND vdogs.tep_el = 7 AND vdogs.kod_tipdog = 2 --AND vdogs.kod_vdoc = 25 -- AND ds.kod_vdog = 221
        AND fv.vid_real = 5
        AND fr.rym  between p_ym_beg AND p_ym_end
        AND d.tep_el = 1
   GROUP BY  fr.rym, ps.kodp, vdog.kod_vdoc, d.kod_d_dog, p.kodp, d.kod_dog
       ;
 end;

END ng_rep_other;

