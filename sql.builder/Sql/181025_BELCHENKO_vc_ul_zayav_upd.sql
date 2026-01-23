update vc_ul_zayav a set num_zayav=(select num_zayav  from c_zayav b where a.kod_zayav=b.kod_zayav) where a.kod_zayav is not null
/
commit
/


