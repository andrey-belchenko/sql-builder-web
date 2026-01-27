 -- Отчет 1126.  SD: 76607.
DECLARE
 p_ym_beg NUMBER;
 p_ym_end NUMBER;
 p_date_beg DATE;
 p_date_end DATE;
BEGIN
  p_ym_beg := :p_ym_beg;
  p_ym_end := :p_ym_beg;
  p_date_beg := kg.ym_first_day(:p_ym_beg);
  p_date_end := kg.ym_last_day(:p_ym_beg);
  delete vr_number_array;
  INSERT INTO vr_number_array (array_id,val)   SELECT 'p_dep', kodp FROM kr_org  WHERE 1=0 OR kodp in :p_dep;
  INSERT INTO vr_number_array (array_id,val)   SELECT 'p_tco_net_kodp', kodp FROM kr_org  WHERE 1=0 ;
  INSERT INTO vr_number_array (array_id,val)   SELECT 'p_kod_dog', kod_dog FROM kr_dogovor WHERE 1=0 ;
  -- выбираем активные договоры на период отчета в рабочую таблицу rr_rep_dog_obj
  -- применяем фильтры по ТСО, отделению, договору
  kg_rep_dog.dog_obj(p_date_beg, p_date_end, 0, 1);
  
   delete rr_rep_po;
   INSERT INTO rr_rep_po (  kod_dog,    ndog,
                              kodp,       payer_name,    -- Код и наименование потребителя   
						      gr_customer_id,  -- Код гр. потребителей 
							  gr_customer_name,	 -- 	Наименование гр. потребителей					  
                              kod_numobj,		 -- код объекта на договоре
							  kod_point,
							  point_num,
							  point_name,
							  tarif_npp,
                              tarif_name,
                              tarif_parent_npp,
                              tarif_parent_name,
                              kodinterval,
                              ym,   
                              rym,
                              voltage,    volt_abbr,
                              cust
                            )
    SELECT   a.kod_dog
             , dp.ndog as ndog
             , dp.payer_id as kodp
             , dp.payer_name as payer_name
			 , a.kod_f23 as gr_customer_id
			 , COALESCE(hs_23.name,'-') as gr_customer_name
             , a.kod_numobj as kod_numobj
             , p.kod_point as kod_point
             , p.nomer as point_num
             , p.name as point_name
             , t.npp as tarif_npp
             , t.sname as tarif_name
             , tt.npp as tarif_parent_npp
             , tt.sname as tarif_parent_name
             , t.kodinterval as kodinterval
             , a.ym  as ym
             , a.rym as rym
             , (CASE WHEN vol.voltage IS NULL THEN -9 ELSE vol.voltage END) as voltage
             , nvl(vol.abbr,'-')  as volt_abbr
             , a.cust   -- натуральные показатели
      FROM nv_account_priem a
          INNER JOIN rr_rep_dog_obj dp ON a.kod_numobj = dp.kod_numobj  -- берем только нужные договоры из рабочей таблицы		 
		 -- INNER JOIN kr_numobj n ON a.kod_numobj = n.kod_numobj
         -- INNER JOIN kr_object ob ON n.kod_obj = ob.kod_obj
          INNER JOIN hk_voltage vol ON a.voltage = vol.voltage
          INNER JOIN sk_nachisl nc ON a.vid_t = nc.vid_t                    
		  INNER JOIN sk_vid_real vr ON nc.vid_real = vr.vid_real    -- основную реализацию берем по pr_osn=1.                 
		  INNER JOIN ks_tarif t ON a.tarif = t.tarif
		  INNER JOIN ks_tarif tt ON t.tarif_parent = tt.tarif
		  LEFT JOIN hs_23 ON a.kod_f23 = hs_23.kod_f23
		  LEFT JOIN nr_priem pr ON a.kod_priem = pr.kod_priem
		  LEFT JOIN hr_point p ON pr.kod_point = p.kod_point
                         
      Where vr.pr_osn = 1
		  AND nc.edizm = 4
		  AND a.ym = p_ym_beg; --BETWEEN p_ym_beg AND p_ym_end;		          
END;
