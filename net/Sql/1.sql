PACKAGE BODY kg_rep_dog

IS
  -- функция возвращает таблицу с заполненым полем kod_dog.
  -- cnt_kod_dog - кол-во kod_dog в таблице vr_number_array
  -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_dep')
  -- Используется для ТатЭнергоСбыт
   FUNCTION get_tbl_dog(cnt_kod_dog NUMBER default 0 ) RETURN tbl_dog PIPELINED
   IS
     cnt_dep     NUMBER;  -- кол-во выбранных отделений
    BEGIN
      cnt_dep := ng_rep_common.get_count_value_param('p_dep');
      IF cnt_kod_dog = 0 THEN
            FOR curr IN
               ( SELECT kod_dog FROM kr_dogovor d  WHERE cnt_dep = 0 OR (cnt_dep > 0 AND d.dep IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dep')))
            LOOP
               PIPE ROW(curr);
            END LOOP;
            RETURN;
       ELSE
           FOR curr IN
               ( SELECT val as kod_dog FROM vr_number_array where array_id = 'p_kod_dog' )
            LOOP
               PIPE ROW(curr);
            END LOOP;
            RETURN;
       END IF;

   END get_tbl_dog;


  -- Заполняет временную таблицу rr_rep_dog данными по договорам.
  -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_dep')
  -- Если нужно исключить отделения, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_no_dep')
  -- Если нужно отсечь по договорам, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_kod_dog')
  -- Если нужно отсечь по номеру корневой группы потребителей, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_gr_kod_level_1')
  -- Если нужно отсечь по номеру группы потребителей, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_gr_cust_id')
  -- Если нужно исключить номер корневой группы потребителей, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_no_gr_kod_level_1')
  -- Если нужно отсечь по наименованию контрагента, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_kodp')
  -- Если нужно отсечь по inn контрагента, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_inn')
  -- Если нужно отсечь по виду контрагента (частное лицо/организация), то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_is_private')
  -- Если нужно отсечь по наличию или отсутствию протокола разногласия, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_exist_protocol')
  -- Если нужно отсечь по признаку крупного бизнеса, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_is_big_business')
  -- Если нужен фильтр по pr_active из kr_dogovor, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_dog_pr_active')
  -- Если нужен фильтр по иду документа (kod_vdoc), то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_kod_vdoc')
  -- p_condition = 1 - признак наличия фильтра по договорам с мораторной задолженностью (дополнительно нужно заполнить поле note)
  -- p_ym - фильтр для вывода только активных договоров в заданный период
    PROCEDURE dog_gr_customer (p_condition NUMBER default 0, p_ym NUMBER default 0 )
    IS
     cnt_dep     NUMBER;  -- кол-во выбранных отделений
     cnt_no_dep     NUMBER;  -- кол-во выбранных отделений
     cnt_kod_dog    NUMBER;  -- кол-во выбранных договоров
     cnt_gr_cust    NUMBER;  -- кол-во выбранных номеров корневой группы потребителей
     cnt_gr_cust_id NUMBER;  -- кол-во выбранных групп потребителей
     cnt_no_gr_cust NUMBER;  -- кол-во выбранных номеров корневой группы потребителей для исключения из отбора
     cnt_kodp       NUMBER;  -- кол-во выбранных контрагентов
     cnt_inn        NUMBER;  -- кол-во выбранных ИНН контрагента
     cnt_dog_pr_active NUMBER;  -- кол-во записей в vr_number_array с arrey_id = 'p_dog_pr_active'
     cnt_kod_vdoc      NUMBER;  -- кол-во записей в vr_number_array с arrey_id = 'p_kod_vdoc'
     p_is_private      NUMBER;
     p_exist_protocol  NUMBER;
     p_is_big_business NUMBER;

    BEGIN
      cnt_dep := ng_rep_common.get_count_value_param('p_dep');
      cnt_no_dep := ng_rep_common.get_count_value_param('p_no_dep');
      cnt_kod_dog := ng_rep_common.get_count_value_param('p_kod_dog');
      cnt_gr_cust := ng_rep_common.get_count_value_param('p_gr_kod_level_1');
      cnt_gr_cust_id := ng_rep_common.get_count_value_param('p_gr_cust_id');
      cnt_no_gr_cust := ng_rep_common.get_count_value_param('p_no_gr_kod_level_1');
      cnt_kodp := ng_rep_common.get_count_value_param('p_kodp');
      cnt_inn := ng_rep_common.get_count_value_param('p_inn');
      cnt_dog_pr_active := ng_rep_common.get_count_value_param('p_dog_pr_active');
      cnt_kod_vdoc := ng_rep_common.get_count_value_param('p_kod_vdoc');

       -- признак отбора по виду контрагента (частное лицо/организация)( 0 - нет (только организации)); 1-Да (только физ.лица); -1 - не выбрано, т.е. не анализируем (берем все записи))
      SELECT nvl(max(val),-1)
        INTO p_is_private
        FROM vr_number_array WHERE array_id = 'p_is_private';

        -- признак отбора по наличию протокола разногласий по договору( 0 - нет; 1-Да; -1 - не выбрано, т.е. не анализируем)
      SELECT nvl(max(val),-1)
        INTO p_exist_protocol
        FROM vr_number_array WHERE array_id = 'p_exist_protocol';

       -- признак отбора по признаку Крупный бизнес у абонента ( 0 - нет; 1-Да; -1 - не выбрано, т.е. не анализируем)
      SELECT nvl(max(val),-1)
        INTO p_is_big_business
        FROM vr_number_array WHERE array_id = 'p_is_big_business';

      DELETE rr_rep_dog;

      INSERT INTO rr_rep_dog ( kod_dog
                            , ndog
                            , vdog_name       -- вид договора (энергоснабжение/купли-продажи)
                            , vdoc_name       -- вид документа (энергоснабжение/купли-продажи)
                            , payer_id        -- ID потребителя
                            , payer_name      -- наименование потребителя
                            , payer_inn       -- inn потребителя
                            , payer_adr_ur    -- юридический адрес потребителя
                            , payer_adr_p     -- почтовый адрес потребителя
                            , dep_id          -- id филиала (отделения)
                            , dep_name        -- наименование филиала (отделения)
                            , oko_id          -- код участка(офиса клиентского обслуживания (ОКО))
                            , oko_name        -- наименование ОКО
                            , gr_cust_id      -- код группы потребителей АСУСЭ
                            , gr_cust_num     -- номер группы потребителей АСУСЭ. Для сортировки
                            , gr_cust_kod_level_1 -- kod корневой группы потребителя контрагента
                            , gr_cust_level_num   -- Номер уровня группы потребителя
                            , gr_cust_snum    -- номер группы потребителей АСУСЭ для вывода в отчет
                            , gr_cust_name    -- наименование группы потребителей АСУСЭ (КА => группа потребителей)
                            , gr_cust_full_name -- полный путь наименования группы потребителя
                            , is_budget       -- Признак бюджета для группы потребления ФО (1 - бюджет, т.е. группа из источника финансирования. 0 - не бюджет)
                            , gr_cust_name_fo -- наименование группы потребителей ФО (Карточка абонента -> Источник финансирования. При отсутствии: Наименование группы потребителей для ФО)
                            , dog_dat_create  -- Дата создания(регистрации,заключения) договора
                            , dog_dat_beg     -- дата начала действия договора
                            , dog_dat_plan_end -- планируемая дата завершения действия договора
                            , dog_dat_end     -- дата окончания действия договора
                            , terms_pay       -- условия оплаты по договору
                          )
      WITH
           dog as ( SELECT kod_dog
                     FROM TABLE (kg_rep_dog.get_tbl_dog(cnt_kod_dog))
                    )
           , itog_day_zadol as (SELECT d.kod_dog, z.days_bzad || tz.name as itog_day_name
                                FROM dog d
                                  INNER JOIN kr_dogovor dd ON d.kod_dog = dd.kod_dog
                                  LEFT JOIN ss_zadol_day z On dd.kod_bzad = z.kod_bzad
                                  LEFT JOIN kk_type_zadol tz On z.day_type= tz.day_type
                              )
           , avans_day_zadol as (SELECT vd.kod_dog, stragg(perc || '% до: ' || days_bzad || tz.name) as avans_day_name
                                    FROM  kr_vist_day vd
                                      LEFT JOIN ss_zadol_day z On vd.kod_bzad = z.kod_bzad
                                      LEFT JOIN kk_type_zadol tz On z.day_type= tz.day_type
                                  GROUP BY vd.kod_dog
                                )
           , terms as (SELECT d.kod_dog, avans_day_name || '; итоги до: ' || itog_day_name as terms_pay
                         FROM dog d --kr_dogovor d
                           LEFT JOIN avans_day_zadol a On d.kod_dog= a.kod_dog
                           LEFT JOIN itog_day_zadol i On d.kod_dog=i.kod_dog
                       )
           , payer_private as (  -- Добавлено по SD: 76585(1) для отчета № 1073
                                SELECT kod_fs
                                  FROM ks_fs WHERE abbr in ('ФЛ', '5 01 01', '5 01 02', '5 02 01', '5 02 02') AND arhive = 0
                              )
      SELECT d.kod_dog
          , d.ndog
          , CASE WHEN vd.kod_vdoc = 1 AND vd.kod_tipdog = 0 THEN 'энергоснабжения'
                 WHEN vd.kod_vdoc = 5 AND vd.kod_tipdog = 0 THEN 'купли-продажи'
                 ELSE '-' END as vdog_name -- вид договора
          , CASE WHEN vd.kod_vdoc = 1 THEN 'энергоснабжения'
                 WHEN vd.kod_vdoc = 5 THEN 'купли-продажи'
                 ELSE '-' END as vdoc_name -- вид документа
          , p.kodp as payer_id
          , p.name as payer_name
          , p.inn as payer_inn
          , substr(nk_adress.kf_address(6,p.kod_d_ur),0,250) as payer_adr_ur
          , substr(nk_adress.kf_address(6,p.kod_d_p),0,250) as payer_adr_p
          , dep.kodp as dep_id
          , dep.name as dep_name
          , kodp_uch.kodp as oko_id
          , kodp_uch.name as oko
          , gcc.kod_group_cust as gr_cust_id
          , gcc.num_cust as gr_cust_num
          , gcc.kod_level_1 as gr_cust_kod_level_1
          , gcc.level_num as gr_cust_level_num
          , gcc.num_gr as gr_cust_snum
          , gcc.gr_cust_name  as gr_cust_name
          , gcc.fullname as gr_cust_full_name
          , CASE WHEN d.kod_ist in (2,3,4,5)  THEN 1   ELSE 0   END as is_budget
          , CASE WHEN d.kod_ist in (2,3,4,5)
                 THEN f.name
                 ELSE nvl(gcc.fin_name, f.name)
            END as gr_cust_name_fo
          , d.dat_numdog as dat_numdog
          , d.dat_dog as dog_dat_beg
          , d.dat_srok as dog_dat_plan_end
          , d.dat_fin as dog_dat_end
          , terms.terms_pay as terms_pay
     FROM  kr_dogovor d
       INNER JOIN kr_org dep ON d.dep = dep.kodp
       INNER JOIN ks_vdog vd ON d.kod_vdog = vd.kod_vdog
       INNER JOIN terms      ON d.kod_dog = terms.kod_dog
       LEFT JOIN  kr_org kodp_uch   ON d.kodp_uch = kodp_uch.kodp
       LEFT JOIN  kr_dogovor_dop dd ON d.kod_dog = dd.kod_dog
       LEFT JOIN  kv_group_cust_level gcc ON dd.kod_group_cust = gcc.kod_group_cust
       LEFT JOIN  ks_istfin f ON  d.kod_ist = f.kod_ist
       LEFT JOIN  kr_payer p  ON d.kodp = p.kodp
       LEFT JOIN  rr_refprop r  ON d.kod_dog = r.objid AND r.kod_refobject = 2 and r.kod_refcode = 2262 -- наличие протокола разногласий по договору -- добавлено по SD: 76585 Рязань 18.09.2025
       LEFT JOIN  rr_refprop rb ON p.kodp = rb.objid    AND rb.kod_refobject = 1 and rb.kod_refcode = 1501 -- Признак крупного бизнеса. добавлено по SD: 76585 Рязань 18.09.2025
     WHERE  (cnt_dep = 0     OR (cnt_dep > 0     AND d.dep IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dep')))
        AND (cnt_no_dep = 0  OR (cnt_no_dep > 0  AND d.dep not IN (SELECT val FROM vr_number_array WHERE array_id = 'p_no_dep')))
        AND (cnt_kod_dog = 0 OR (cnt_kod_dog > 0 AND d.kod_dog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_dog'))) -- добавлено по заявке 75780(2)
        AND (cnt_gr_cust = 0 OR (cnt_gr_cust > 0 AND gcc.kod_level_1 IN (SELECT val FROM vr_number_array WHERE array_id = 'p_gr_kod_level_1'))) -- добавлено по заявке 75061(1) для Рязани
        AND (cnt_gr_cust_id = 0 OR (cnt_gr_cust_id > 0 AND gcc.kod_group_cust  IN (SELECT val FROM vr_number_array WHERE array_id = 'p_gr_cust_id'))) -- добавлено по заявке 75780(2)для Рязани
        AND (cnt_no_gr_cust = 0 OR (cnt_no_gr_cust > 0 AND gcc.kod_level_1 NOT IN (SELECT val FROM vr_number_array WHERE array_id = 'p_no_gr_kod_level_1'))) -- добавлено по заявке 75061(3) для Рязани
        AND (cnt_kodp = 0 OR (cnt_kodp > 0 AND p.kodp IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kodp'))) -- добавлено по SD: 76585 для Рязани
        AND (cnt_inn = 0  OR (cnt_inn > 0  AND p.inn  IN (SELECT val FROM vr_number_array WHERE array_id = 'p_inn'))) -- добавлено по SD: 76585 для Рязани
        AND (p_is_private = -1  OR (p_is_private = 0 AND p.kod_fs NOT IN (select kod_fs from payer_private))
                                OR (p_is_private = 1 AND p.kod_fs     IN (select kod_fs from payer_private))) -- добавлено по SD: 76585 для Рязани, SD: 76585(1)
        AND (p_exist_protocol = -1  OR (p_exist_protocol = 0 AND r.kod_refcode is null)
                                    OR (p_exist_protocol = 1 AND r.kod_refcode = 2262)) -- добавлено по SD: 76585 для Рязани
        AND (p_is_big_business = -1 OR (p_is_big_business = 0 AND rb.kod_refcode is null)
                                    OR (p_is_big_business = 1 AND rb.kod_refcode = 1501)) -- добавлено по SD: 76585 для Рязани (для отчета № 1073)
        AND (p_ym = 0 OR (p_ym > 0 AND p_ym between  to_number(to_char(d.dat_dog,'YYYYMM'))/100
                                                and COALESCE(to_number(to_char(d.dat_fin,'YYYYMM'))/100,p_ym + 50000))) -- активные договоры по датам. Добавлено 05.06.2025
        AND (cnt_dog_pr_active = 0 OR (cnt_dog_pr_active > 0 AND d.pr_active IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dog_pr_active'))) -- добавлено 31.07.2025 по заявке 75780(2)
        AND (cnt_kod_vdoc = 0 OR (cnt_kod_vdoc > 0 AND vd.kod_vdoc IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_vdoc'))) -- добавлено по SD: 76585(1) для Рязани (отчет № 1073)
     ;

      IF p_condition = 1 THEN
       dog_bankruptcy;
      END IF;

    END dog_gr_customer;

    --------------------------
    -- Заполняет временную таблицу rr_rep_dog краткой информацией по договорам.
    -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_dep')
    -- Если нужно отсечь по договорам, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_kod_dog')
    -- Если нужен фильтр по pr_active из kr_dogovor, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_dog_pr_active')
    -- p_ym - период (если период указан, то добавляем фильтр по периодичности перерасчетов: kod_perper = 0 - "Вручную" для отчетов Рязани)
    PROCEDURE dog_short(p_ym NUMBER default 0)
    IS
      cnt_dep     number;  -- кол-во выбранных отделений
      cnt_kod_dog NUMBER;  -- кол-во выбранных договоров
      cnt_dog_pr_active NUMBER;  -- кол-во записей в vr_number_array с arrey_id = 'p_dog_pr_active'
    BEGIN
      cnt_dep := ng_rep_common.get_count_value_param('p_dep');
      cnt_kod_dog := ng_rep_common.get_count_value_param('p_kod_dog');
      cnt_dog_pr_active := ng_rep_common.get_count_value_param('p_dog_pr_active');
      DELETE rr_rep_dog;

      kg_rep_dog.dog_bankruptcy;
      kg_rep_dog.dog_orem(202501, 1);
      kg_rep_dog.dog_adjustment(202501, 1);
      kg_rep_dog.dog_moratoria_debt;

      INSERT INTO rr_rep_dog ( kod_dog
                            , ndog
                            , payer_id        -- ID потребителя
                            , payer_name      -- наименование потребителя
                            , payer_inn       -- inn потребителя
                            , dep_id          -- id филиала (отделения)
                            , dep_name        -- наименование филиала (отделения)
                            , forma_dog       -- форма договора:  0- пусто (Потребитель); 1 - договор с ИКУ (исполнителями ком.услуг) (Исполнитель); 2 - договор в ЭСО (Покупатель)
                            , kod_perper      -- периодичность перерасчетов: 0 - "Вручную"
                            )
      SELECT d.kod_dog
          , d.ndog
          , p.kodp as payer_id
          , p.name as payer_name
          , p.inn as payer_inn
          , dep.kodp as dep_id
          , dep.name as dep_name
          , v.forma_dog as forma_dog
          , CASE WHEN p_ym > 0 THEN nvl(ng_calc_graph.DogovorReglament(kod_dog, p_ym, 3, 'PERPER'), -10) ELSE -10 END as kod_perper
     FROM kr_dogovor d
       INNER JOIN kr_org dep ON d.dep = dep.kodp
       INNER JOIN ks_vdog v ON d.kod_vdog = v.kod_vdog
       LEFT JOIN kr_payer p ON d.kodp = p.kodp
     WHERE  (cnt_dep = 0 OR (cnt_dep > 0 AND d.dep IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dep')))
        AND (cnt_kod_dog = 0 OR (cnt_kod_dog > 0 AND d.kod_dog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_dog')))
        AND (cnt_dog_pr_active = 0 OR (cnt_dog_pr_active > 0 AND d.pr_active IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dog_pr_active')));

    END dog_short;

   --------------------------
    -- Заполняет временную таблицу rr_rep_dog_obj информацией по объектам.
    -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу   vr_number_array в поле val (array_id = 'p_dep')
    -- Если нужно отсечь по договорам, то предварительно нужно вставить данные во временную таблицу   vr_number_array в поле val (array_id = 'p_kod_dog')
    -- Если нужно отсечь по ТСО на границе сети, то предварительно нужно вставить данные во вр. таблицу vr_number_array в поле val (array_id = 'p_tco_net_kodp')
    -- Если нужно отсечь по статусу договора, то предварительно нужно вставить данные во вр.таблицу   vr_number_array в поле val (array_id = 'p_dog_pr_active')
    -- Если нужно отсечь по статусу объекта, то предварительно нужно вставить данные во вр. таблицу   vr_number_array в поле val (array_id = 'p_obj_pr_active')
    -- Если нужно отсечь по типу объекта (строения) (жилой дом, отдельно стоящий,...), то предварительно нужно вставить данные во вр. таблицу vr_number_array в поле val (array_id = 'p_obj_kod_type')
    -- Если нужно отсечь по коду объекта на договоре, то предварительно нужно вставить данные во вр. таблицу vr_number_array в поле val (array_id = 'p_kod_numobj')
    -- condition:  0 -  все, 1 - добавляем условие на дату начала действия объекта на договоре, 2- добавляем условие на дату прекращения для объекта
    -- p_is_only_active = 1 - берем только активные договоры (по датам на договоре)
    -- используется в Рязани (отчеты № 1063, 1064, 1126, ...)
    PROCEDURE dog_obj(p_date_beg date, p_date_end date, condition NUMBER default 0, p_is_only_active NUMBER default 0)
    IS
      cnt_dep     NUMBER;  -- кол-во выбранных отделений
      cnt_kod_dog NUMBER;  -- кол-во выбранных договоров
      cnt_tco_net_kodp NUMBER;  -- кол-во выбранных ТСО
      cnt_dog_pr_active NUMBER;  -- кол-во записей в vr_number_array с arrey_id = 'p_dog_pr_active'
      cnt_obj_pr_active NUMBER;  -- кол-во записей в vr_number_array с arrey_id = 'p_obj_pr_active'
      cnt_obj_kod_type NUMBER;   -- кол-во записей в vr_number_array с arrey_id = 'p_obj_kod_type'
      cnt_kod_numobj   NUMBER;   -- кол-во записей в vr_number_array с arrey_id = 'p_kod_numobj'  добавлено по SD 76607(1)
    BEGIN
      cnt_dep := ng_rep_common.get_count_value_param('p_dep');
      cnt_kod_dog := ng_rep_common.get_count_value_param('p_kod_dog');
      cnt_tco_net_kodp  := ng_rep_common.get_count_value_param('p_tco_net_kodp');
      cnt_dog_pr_active := ng_rep_common.get_count_value_param('p_dog_pr_active');
      cnt_obj_pr_active := ng_rep_common.get_count_value_param('p_obj_pr_active');
      cnt_obj_kod_type  := ng_rep_common.get_count_value_param('p_obj_kod_type');
      cnt_kod_numobj    := ng_rep_common.get_count_value_param('p_kod_numobj');  -- добавлено по SD 76607(1)
      DELETE rr_rep_dog_obj;

      -- Test procedure calls for parsing
      kg_rep_dog.dog_short(202501);
      kg_rep_dog.dog_gr_customer(1, 202501);
      kg_rep_dog.dog_obj_short(202501);
      kg_rep_dog.dog_obj_dat(SYSDATE);
   

      INSERT INTO rr_rep_dog_obj ( kod_dog,
                                  ndog,
                                  payer_id,
                                  payer_name,
                                  kod_numobj,
                                  num_obj,    -- Номер объекта на договоре.
                                  obj_name,   -- Наименование объекта.
                                  obj_adr_name,   -- Адрес объекта.
                                  obj_dat_create,  -- Дата начала действия в тек. договоре
                                  obj_dat_postav,  -- Дата начала поставки
                                  obj_dat_fin,      -- Дата прекращения действия в тек. договоре
                                  obj_status,       -- Статус объекта на договоре (0-активный, 1 - архивный, 2- новый)
                                  obj_kod_type,     -- Код типа объекта как строения   -- добавлено 14.09.2025 по SD: 75227(2), потеряно и заново добавлено 06.11.2025 по SD 75227(3)
                                  tco_net_kodp,     -- ТСО на границе сети (kodp)
                                  tco_net_name      -- Наименование ТСО на границе сети
                        )
     WITH
       tco as (
                 SELECT n.kod_numobj
                       , net.kodp as tco_net_kodp
                       , max(net.name) as tco_net_name
                       , max(t.kod_type) as obj_kod_type  -- добавлено 14.09.2025 по SD: 75227(2) 06.11.2025 по SD: 75227(3)
                        -- , max(pwrc.name) as tco_pwrc_name -
                   FROM hr_pmax_history h
                     INNER JOIN hr_pmax_detail hd ON (h.kod_pmax_history = hd.kod_pmax_history)
                     INNER JOIN hr_attpoint a ON (hd.kod_attpoint = a.kod_attpoint)
                     INNER JOIN kr_numobj n   ON h.kod_numobj = n.kod_numobj

                     INNER JOIN kr_object ob ON n.kod_obj = ob.kod_obj
                     LEFT JOIN kr_object tt     ON ob.kodd_house = tt.kodd AND tt.tep_el_byt = 1 AND tt.tep_el = 4 -- по адресу объекта на договоре находим адрес объекта как строения добавлено 14.09.2025 по SD: 75227(2)
                     LEFT JOIN tr_har_house t    ON tt.kod_obj = t.kod_obj   -- из объекта как строения будем брать код типа объекта
                   --  INNER JOIN ts_object_type ot ON t.KOD_TYPE = ot.KOD_TYPE  -- справочник типов объектов

                     INNER JOIN kr_dogovor d  ON n.kod_dog = d.kod_dog
                     LEFT JOIN  kr_payer net  ON a.kodp_net = net.kodp
                      -- LEFT JOIN kr_payer pwrc ON a.kodp_pwrcompany = pwrc.kodp

                 WHERE hd.kodnagruzpotreb = a.kodnagruzpotreb
                     AND (cnt_dep = 0 OR (cnt_dep > 0 AND d.dep IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dep')))
                     AND (cnt_kod_dog = 0 OR (cnt_kod_dog > 0 AND d.kod_dog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_dog')))
                     AND (cnt_dog_pr_active = 0 OR (cnt_dog_pr_active > 0 AND d.pr_active IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dog_pr_active')))
                     AND (cnt_obj_pr_active = 0 OR (cnt_obj_pr_active > 0 AND n.pr_active IN (SELECT val FROM vr_number_array WHERE array_id = 'p_obj_pr_active')))  -- добавлено 14.09.2025 по SD: 75227(2)
                     AND (cnt_obj_kod_type = 0  OR (cnt_obj_kod_type > 0  AND t.kod_type IN (SELECT val FROM vr_number_array WHERE array_id = 'p_obj_kod_type')))  -- фильтр по типу строения добавлен по SD 75237(1)
                     AND (cnt_kod_numobj = 0    OR (cnt_kod_numobj   > 0  AND n.kod_numobj IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_numobj')))  -- фильтр по объекту на договоре добавлен по SD 76607(1)
                     AND (cnt_tco_net_kodp = 0  OR (cnt_tco_net_kodp > 0  AND net.kodp   IN (SELECT val FROM vr_number_array WHERE array_id = 'p_tco_net_kodp')))
                     AND (p_is_only_active = 0  OR (p_is_only_active > 0  AND p_date_beg BETWEEN d.dat_dog AND COALESCE(d.dat_fin, SYSDATE + 5000)
                                                                          OR p_date_end BETWEEN d.dat_dog AND COALESCE(d.dat_fin, SYSDATE + 5000)))   -- активные договоры на дату отчета 23.04.2025
                     AND (condition = 0 OR (condition = 1 and n.dat_create BETWEEN p_date_beg AND p_date_end)
                                        OR (condition = 2 and n.dat_fin    BETWEEN p_date_beg AND p_date_end))
                 GROUP BY n.kod_numobj, net.kodp
               )
       SELECT d.kod_dog
                  , d.ndog as ndog
                  , p.kodp as payer_id
                  , p.name as payer_name
                  , n.kod_numobj as kod_numobj
                  , n.num_obj as num_obj
                  , n.name as obj_name
                --  , substr(nk_adress.kf_address(2,ob.kodd),26,250) as obj_adr_name --
                  , substr(nk_adress.kf_address(2,ob.kodd),0,250) as obj_adr_name
                  , n.dat_create as obj_dat_create
                  , n.dat_postav_obj as obj_dat_postav    -- если null, то нужно вывести null !
                  , n.dat_fin as obj_dat_fin
                  , n.pr_active as obj_status
                  , tco.obj_kod_type as obj_kod_type  -- добавлено 14.09.2025 по SD: 75227(2) 06.11.2025 по SD: 75227(3)
                  , tco.tco_net_kodp as tco_net_kodp
                  , tco.tco_net_name as tco_net_name
             FROM tco
               INNER JOIN kr_numobj n ON tco.kod_numobj = n.kod_numobj
               INNER JOIN kr_object ob ON n.kod_obj = ob.kod_obj
               INNER JOIN kr_dogovor d ON d.kod_dog = n.kod_dog
               LEFT JOIN kr_payer p ON d.kodp = p.kodp
         ;
    END dog_obj;


  --------------------------
    -- Заполняет временную таблицу rr_rep_dog краткой информацией по договорам в разрезе объектов на договоре (kod_numobj) и субъектов РФ
    -- с учетом финансового наследования (т.е. по договору получаем все kod_numobj и из родительских договоров).
    -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу   vr_number_array в поле val (array_id = 'p_dep')
    -- Если нужно отсечь по субъекту РФ, то предварительно нужно вставить данные во временную таблицу vr_number_array с arrey_id='p_kod_adr_m'
    -- Если нужно отсечь по договорам, то предварительно нужно вставить данные во временную таблицу   vr_number_array в поле val (array_id = 'p_kod_dog')
    -- Если нужно отсечь по виду договора, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_vdoc')
    -- Если нужно отсечь по типу договора, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_tipdog')
    -- Если нужно отсечь по типу договора, то предварительно нужно вставить данные во временную таблицу vr_number_array в поле val (array_id = 'p_tep_el')
    -- p_ym - фильтр вывода только активных договоров в заданный период
   PROCEDURE dog_obj_short(p_ym NUMBER default 0)
    IS
      cnt_dep     NUMBER;  -- кол-во выбранных отделений
      cnt_adr_m   NUMBER;  -- кол-во выбранных субъектов РФ
      cnt_kod_dog NUMBER;  -- кол-во выбранных договоров
      cnt_vdoc    NUMBER;  -- кол-во выбранных видов договоров
      cnt_tipdog  NUMBER;  -- кол-во выбранных типов договоров
      cnt_tep_el  NUMBER;  -- кол-во выбранных типов договоров (tep_el = 1 - электрический, 7 - услуги )
    BEGIN
      cnt_dep := ng_rep_common.get_count_value_param('p_dep');
      cnt_adr_m   := ng_rep_common.get_count_value_param('p_kod_adr_m');
      cnt_kod_dog := ng_rep_common.get_count_value_param('p_kod_dog');
      cnt_vdoc    := ng_rep_common.get_count_value_param('p_vdoc');
      cnt_tipdog  := ng_rep_common.get_count_value_param('p_tipdog');
      cnt_tep_el  := ng_rep_common.get_count_value_param('p_tep_el');

      DELETE rr_rep_dog;

      INSERT INTO rr_rep_dog ( kod_dog
                            , ndog
                            , dog_dat_beg     -- дата заключения договора
                            , kod_numobj      -- kod объекта на договоре
                            , kod_region      -- kod региона (субъекта РФ)
                            , region_name     -- наименование региона
                            , payer_id        -- ID потребителя
                            , payer_name      -- наименование потребителя
                            , payer_inn       -- inn потребителя
                            , dep_id          -- id филиала (отделения)
                            , dep_name        -- наименование филиала (отделения)
                            , forma_dog       -- форма договора:  0 - пусто (Потребитель); 1 - договор с ИКУ (исполнителями ком.услуг) (Исполнитель); 2 - договор в ЭСО (Покупатель)
                            , gr_cust_id      -- kod группы потребителя.
                          )
      WITH
      pre as (SELECT dp.kod_dog
                  , max(dp.ndog) as ndog
                  , max(dp.dat_numdog) as dog_dat_beg -- дата заключения  (!) договора
                  , n.kod_numobj
                  , nk_adress.kf_get_sf(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END) as kod_region
                  , max(nk_adress.kp_get_region_name(CASE WHEN ob.kodd IS NULL OR ob.kodd = -1 THEN d.kod_d_dog ELSE ob.kodd END)) as kod_region_name
                  , p.kodp as payer_id
                  , max(p.name) as payer_name
                  , max(p.inn) as payer_inn
                  , dep.kodp as dep_id
                  , max(dep.name) as dep_name
                  , max(v.forma_dog) as forma_dog
                  , max(dd.kod_group_cust) as gr_cust_id
             FROM kr_dogovor d
               INNER JOIN kr_dogovor dp ON d.kod_dog_fin=dp.kod_dog  -- учитываем финансовое наследование
               INNER JOIN kr_org dep ON dp.dep = dep.kodp
               INNER JOIN ks_vdog v ON dp.kod_vdog = v.kod_vdog
               LEFT JOIN kr_dogovor_dop dd ON dp.kod_dog = dd.kod_dog

               LEFT JOIN kr_numobj n ON d.kod_dog = n.kod_dog
               LEFT JOIN kr_object ob ON n.kod_obj = ob.kod_obj
               LEFT JOIN kr_payer p ON dp.kodp = p.kodp
             WHERE  (cnt_dep = 0 OR (cnt_dep > 0 AND dp.dep IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dep')))
                AND (cnt_kod_dog = 0 OR (cnt_kod_dog > 0 AND dp.kod_dog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_dog')))
                AND (cnt_vdoc = 0   OR (cnt_vdoc > 0 AND v.kod_vdoc IN (SELECT val FROM vr_number_array WHERE array_id = 'p_vdoc')))
                AND (cnt_tipdog = 0  OR(cnt_tipdog > 0 AND v.kod_tipdog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_tipdog')))
                AND (cnt_tep_el = 0 OR (cnt_tep_el > 0 AND dp.tep_el IN (SELECT val FROM vr_number_array WHERE array_id = 'p_tep_el')))
                AND (p_ym = 0 OR (p_ym > 0 AND p_ym between  to_number(to_char(dp.dat_dog,'YYYYMM'))/100
                                                        and COALESCE(to_number(to_char(dp.dat_fin,'YYYYMM'))/100,p_ym + 50000))) -- активные договоры на дату отчета. Добавлено 01.06.2025
             GROUP BY dp.dep, ob.kodd, d.kod_d_dog, dp.kod_dog, n.kod_obj, n.kod_numobj, p.kodp, dep.kodp
             )
      SELECT kod_dog
          , ndog
          , dog_dat_beg
          , kod_numobj
          , kod_region
          , kod_region_name
          , payer_id
          , payer_name
          , payer_inn
          , dep_id
          , dep_name
          , forma_dog
          , gr_cust_id
      FROM pre
      WHERE cnt_adr_m = 0 OR (cnt_adr_m > 0 AND kod_region IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_adr_m'))
        ;
    END dog_obj_short;

    ---------------------------

   -- Заполняет временную таблицу rr_rep_dog_obj информацией по объектам на дату (пока что только на текущую дату).
   -- Если нужно отсечь по отделению, то предварительно нужно вставить данные во временную таблицу vr_number_array поле 'val' для параметра 'p_dep'
   -- Если нужно отсечь по договорам, то предварительно нужно вставить данные во временную таблицу   vr_number_array в поле val (array_id = 'p_kod_dog')
   -- Если нужно о отсечь по статусу объекта, то предварительно нужно вставить данные во временную таблицу   vr_number_array в поле val (array_id = 'p_obj_status')
   -- Используется в отчетах: № 2005(Рязань.Фиксы), , 1074 (Рязань. ГП)
   PROCEDURE dog_obj_dat(p_date date)
   IS
     cnt_dep     NUMBER;  -- кол-во выбранных отделений
     cnt_kod_dog NUMBER;  -- кол-во выбранных договоров
     cnt_obj_status NUMBER;  -- кол-во выбранных статусов объектов
   BEGIN
     cnt_dep := ng_rep_common.get_count_value_param('p_dep');
     cnt_kod_dog := ng_rep_common.get_count_value_param('p_kod_dog');
     cnt_obj_status := ng_rep_common.get_count_value_param('p_obj_status');

   DELETE rr_rep_dog_obj;
   INSERT INTO rr_rep_dog_obj ( kod_dog,
                                  ndog,
                                  payer_id,
                                  payer_name,
                                  kod_numobj,
                                  num_obj,       -- Номер объекта на договоре.
                                  obj_name,      -- Наименование объекта.
                                  obj_num_full,  -- полное наименование объекта, например "ndog.num_obj"
                                  obj_adr_name,    -- Адрес объекта.
                                  obj_dat_create,  -- Дата начала действия в тек. договоре
                                  is_vkl,          -- Признак неотключаемого объекта (1 - Да, 0 - Нет)
                                  is_act_tb,     -- Наличие акта технологической брони (1 - Да, 0 - Нет)
                                  obj_status,    -- Статус объекта. 0-активный, 1-архивный, 2- новый
                                  obj_rwn  -- Номер строки для сложных группировок
                        )
  WITH
   pre as ( SELECT d.kod_dog
              , d.ndog
              , p.kodp as payer_id
              , p.name as payer_name
              , n.kod_numobj
              , n.num_obj
              , n.name as obj_name
              , d.ndog || '.' || trim(to_char(n.num_obj,'000')) as obj_num_full
              , substr(nk_adress.kf_address(2,ob.kodd),0,250) as obj_adr_name   --
              , n.dat_create as obj_date_beg
              , CASE WHEN n.vkl530 in (1,2) THEN 1 ELSE 0 END as is_vkl   -- признак неотключаемого объекта
              , CASE WHEN hg_bron.getbron_dat(n.kod_numObj, p_date, 2) > 0 THEN 1 ELSE 0 END as is_act_tb
           --   , CASE WHEN p_date >= n.date_excl_rep THEN 1   -- архивный
           --          ELSE (CASE WHEN p_date >= n.dat_create THEN 0  -- активный
           --                    ELSE 2 END) END as obj_status  -- статус "Новый" пока не решено как находить на прошлую дату.
              , n.pr_active as obj_status
    FROM kr_dogovor d
      INNER JOIN kr_numobj n ON d.kod_dog = n.kod_dog
      INNER JOIN kr_object ob ON n.kod_obj = ob.kod_obj
      INNER JOIN kr_payer p ON d.kodp = p.kodp
    WHERE (cnt_dep = 0 OR (cnt_dep > 0 AND d.dep IN (SELECT val FROM vr_number_array WHERE array_id = 'p_dep')))
      AND (cnt_kod_dog = 0 OR (cnt_kod_dog > 0 AND d.kod_dog IN (SELECT val FROM vr_number_array WHERE array_id = 'p_kod_dog')))
    )
  SELECT kod_dog
       , ndog
       , payer_id
       , payer_name
       , kod_numobj
       , num_obj
       , obj_name
       , obj_num_full
       , obj_adr_name
       , obj_date_beg
       , is_vkl   -- признак неотключаемого объекта
       , is_act_tb
       , obj_status
       , ROW_NUMBER() OVER (ORDER BY (lpad(ndog, 10 , '0')), num_obj) as obj_rwn
  FROM pre
  WHERE (cnt_obj_status = 0 OR (cnt_obj_status > 0 AND obj_status IN (SELECT val FROM vr_number_array WHERE array_id = 'p_obj_status')))
  ;
 END;   -- end procedure dog_obj_dat

    ---------------------------
    -- По kod_dog по договорам с мораторной задолженностью добавляется поле в таблицу rr_rep_dog
    -- ("Банкротство" => вкладка "Включение в реестр треб. кредиторов" => поле "Причина исключения/изменения мораторной задолженности)
   PROCEDURE dog_bankruptcy
   IS
   BEGIN

    UPDATE rr_rep_dog r
    SET note = (SELECT max(m.prim_mor_dz)
                  FROM kr_dogovor d
                   LEFT JOIN ur_folders fol ON d.kodp = fol.kodp
                   LEFT JOIN ur_mat m ON fol.kod_folders = m.kod_folders
                WHERE r.kod_dog = d.kod_dog
                GROUP BY d.kod_dog);
   END dog_bankruptcy;

   ------------------------
   -- Заполняем в таблице vr_number_array поле 'val' для параметра 'p_kod_dog'.
   -- p_is_opt = 1 - выбираем договоры ОРЭМ с признаком опт (если хотя бы на одном из объектов есть ГТП с признаком опт, то договор считаается "опт")
   -- p_is_opt = 0 - выбираем договоры РРЭМ (розница) пока не реализован
   PROCEDURE dog_orem (p_ym NUMBER, p_is_opt NUMBER)
   IS
   BEGIN
    INSERT INTO vr_number_array (array_id , val )
    WITH
      opt as (SELECT d.kod_dog
                   , sum(nvl((SELECT 1
                                FROM hs_gtp
                               WHERE kod_gtp = hg_common.get_gtp (n.kod_numobj, p_ym) AND pr_opt = 1),
                        0)) is_opt  -- признак "Опт" регламента (с объекта или договорва в зависимости от "Способа задания графиков")
               FROM kr_dogovor d
                 LEFT JOIN kr_numobj n ON d.kod_dog = n.kod_dog
                 LEFT JOIN kr_object ob ON n.kod_obj = ob.kod_obj
               GROUP BY d.kod_dog
            )
      SELECT 'p_kod_dog'
             ,kod_dog
        FROM opt
       WHERE (p_is_opt = 1 AND is_opt > 0);

   END dog_orem;

    -- Заполняем/Удаляем в таблице vr_number_array поле 'val' для параметра 'p_kod_dog'.
   -- p_ins = 1 - заполняем договорами с корректировкой (т.е. по тем потребителям, которых в начале месяца закрываем предварительно,
   --                                                    а после появления затрат выставляем корректировочные документы. )
   -- p_ins = 2 - delete всех договоров, кроме договоров с корректировкой
   PROCEDURE dog_adjustment (p_ym NUMBER, p_ins NUMBER)
   IS
   BEGIN
    IF p_ins = 1
    THEN
       INSERT INTO vr_number_array (array_id, val )
       WITH
        per as (SELECT  d.kod_dog
                      , nvl(ng_calc_graph.DogovorReglament(kod_dog, p_ym, 3, 'PERPER'), -10) as kod_perper
                 FROM kr_dogovor d
               )
       SELECT 'p_kod_dog'
              ,kod_dog
        FROM per
       WHERE kod_perper = 0;   -- периодичность перерасчетов "вручную": perper = 0
    ELSE
     DELETE FROM vr_number_array
        WHERE EXISTS
           (WITH
            per as (SELECT  d.kod_dog
                          , nvl(ng_calc_graph.DogovorReglament(kod_dog, p_ym, 3, 'PERPER'), -10) as kod_perper
                     FROM kr_dogovor d
                   )
           SELECT 'p_kod_dog'
                  ,kod_dog
            FROM per
           WHERE (kod_perper <> 0) AND array_id = 'p_kod_dog'
           );
    END IF;

   END  dog_adjustment;

   -- Заполняем в таблице vr_number_array поле 'val' для параметра (array_id = )'p_kod_dog'
   -- договорами с мораторной задолженностью (ТатЭнергоСбыт: ndog like '%-51' or ndog like '%-51 %')
   PROCEDURE dog_moratoria_debt
   IS
   BEGIN
      INSERT INTO vr_number_array (array_id, val)
      SELECT  'p_kod_dog'
             , kod_dog
        FROM kr_dogovor
       WHERE ndog like '%-51' or ndog like '%-51 %';

   END  dog_moratoria_debt;

END kg_rep_dog;
