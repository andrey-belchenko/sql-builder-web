-- Start of DDL Script for Package ASUSE.VG_TIME
-- Generated 30-окт-2014 20:26:54 from ASUSE@asuse

CREATE OR REPLACE 
PACKAGE vg_time
/* Formatted on 27.05.2014 17:27:58 (QP5 v5.206) */
IS
FUNCTION to_period (p_date IN DATE, p_length IN NUMBER)
        RETURN NUMBER;



    FUNCTION period_start (p_period IN NUMBER)
        RETURN DATE;


    FUNCTION period_length (p_period IN NUMBER)
        RETURN NUMBER;

    FUNCTION period_end (p_period IN NUMBER)
        RETURN DATE;

    FUNCTION is_in_period (p_period IN NUMBER, p_date IN DATE)
        RETURN NUMBER;
END;                                                               -- sg_debet
/



-- End of DDL Script for Package ASUSE.VG_TIME

-- Start of DDL Script for Package Body ASUSE.VG_TIME
-- Generated 30-окт-2014 20:26:54 from ASUSE@asuse

CREATE OR REPLACE 
PACKAGE BODY vg_time
/* Formatted on 30-окт-2014 20:15:01 (QP5 v5.206) */
IS
   FUNCTION to_period (p_date IN DATE, p_length IN NUMBER)
        RETURN NUMBER
    IS
    BEGIN
        RETURN TO_NUMBER (TO_CHAR (p_date, 'J')) + p_length *  (100000 * 10000);
    END;



    FUNCTION period_length (p_period IN NUMBER)
        RETURN NUMBER
    IS
    BEGIN
        RETURN ROUND (p_period / (100000 * 10000));
    END;

    FUNCTION period_start (p_period IN NUMBER)
        RETURN DATE
    IS
    BEGIN
        RETURN TO_DATE (
                   TO_CHAR (p_period - period_length (p_period) *   (100000 * 10000)),
                   'J');
    END;

    FUNCTION period_end (p_period IN NUMBER)
        RETURN DATE
    IS
    BEGIN
        RETURN period_start (p_period) + period_length (p_period);
    END;

    FUNCTION is_in_period (p_period IN NUMBER, p_date IN DATE)
        RETURN NUMBER
    IS
    BEGIN
        IF     p_date >= period_start (p_period)
           AND p_date < period_end (p_period)
        THEN
            RETURN 1;
        ELSE
            RETURN 0;
        END IF;
    END;
END;
/

grant execute on vg_time to public
/
create public synonym vg_time for vg_time;
/

-- End of DDL Script for Package Body ASUSE.VG_TIME

