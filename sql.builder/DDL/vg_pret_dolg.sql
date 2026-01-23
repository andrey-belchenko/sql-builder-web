CREATE OR REPLACE PACKAGE vg_pret_dolg
IS
    function calculate (p_date DATE, p_kod_dog_array_id VARCHAR2) return number;
END vg_pret_dolg;
/
-- Grants for Package
GRANT EXECUTE ON vg_pret_dolg TO PUBLIC
/

CREATE OR REPLACE PACKAGE BODY vg_pret_dolg
IS
    function calculate (p_date DATE, p_kod_dog_array_id VARCHAR2) return number
    IS
        nkod_pret_dolg_calc number;
    BEGIN
        -- инфо о сессии
        insert into VR_PRET_DOLG_CALC(calc_date) values(sysdate) returning kod_pret_dolg_calc into nkod_pret_dolg_calc;
        
        -- расчет задолженности
        sqlb_29814.fill_table(p_date, p_kod_dog_array_id);
        
        -- сохраняем информацию о задолженности в разрезе договоров
        insert into VR_PRET_DOLG_DOG 
            select null,
                   nkod_pret_dolg_calc,
                   a.*,
                   null,
                   null 
            from sqlb_29814_dog_tbl a;
            
        -- сохраняем информацию о задолженности в разрезе сф    
        insert into VR_PRET_DOLG_SF 
            select null,
                   nkod_pret_dolg_calc,
                   a.*,
                   null,
                   null 
            from sqlb_29814_sf_tbl a;
        
        commit;
        
        return nkod_pret_dolg_calc;
        
        exception 
            when others then
                rollback;
                raise;
    end calculate;
END vg_pret_dolg;
/
