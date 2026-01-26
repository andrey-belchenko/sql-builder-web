using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading;
//using System.Windows.Forms;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.LookAndFeel;
//using DevExpress.Skins;
//using DevExpress.UserSkins;
//using DevExpress.XtraEditors;
using infoenergo.core;
using infoenergo.core.Data;
using infoenergo.sys;
//using infoenergo.ui.win;
//using sql.builder.Controls.Testing;
//using sql.builder.Properties;
using sql.builder.DataApi;
using sql.builder.UI.CommandItems;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
//using infoenergo.framework.Extensions.Oracle;
using System.Collections.Generic;
using System.Text;
using sql.builder.Clean;
using SqlBuilderLib.DevTools;

// Basic usage


namespace sql.builder
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            var tableNames = DevSqlParserAntlr.GetSourceTables(@"begin
delete from rr_temp where skod = '76607_data';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
s1, --grsetname
n1, --kod_dog
n2, --kod_numobj
s2, --ndog
s3, --ndog_obj
s4, --payer_name
n3, --point_num
s5, --point_name
n4, --gr_customer_id
s6, --gr_customer_name
n5, --tarif_parent_npp
s7, --tarif_parent_name
n6, --tarif_npp
s8, --tarif_name
n7, --cust_2
n8, --cust_1
n9, --gr_rn
s9, --growid
s10, --parent_growid
n10, --groupingid
s11, --grsetid
s12, --origgrsetid
s13, --parent_grsetid
n11--par_groupingid
)
--76607_data
with
mat1 as
(
--
select /*+ materialize*/
grpd.kod_dog as kod_dog, /*number*/
grpd.kod_numobj as kod_numobj, /*number*/
grpd.ndog as ndog, /*string*/
grpd.ndog_obj as ndog_obj, /*string*/
grpd.payer_name as payer_name, /*string*/
grpd.point_num as point_num, /*number*/
grpd.point_name as point_name, /*string*/
grpd.gr_customer_id as gr_customer_id, /*number*/
grpd.gr_customer_name as gr_customer_name, /*string*/
grpd.tarif_parent_npp as tarif_parent_npp, /*number*/
grpd.tarif_parent_name as tarif_parent_name, /*string*/
grpd.tarif_npp as tarif_npp, /*number*/
grpd.tarif_name as tarif_name, /*string*/
nvl(grpd.cust_2,0) as cust_2, /*number*/
nvl(grpd.cust_1,0) as cust_1/*number*/
from (
--
select po.kod_dog as kod_dog, /*number*/
po.kod_numobj as kod_numobj, /*number*/
po.ndog as ndog, /*string*/
po.ndog_obj as ndog_obj, /*string*/
po.payer_name as payer_name, /*string*/
po.point_num as point_num, /*number*/
po.point_name as point_name, /*string*/
po.gr_customer_id as gr_customer_id, /*number*/
po.gr_customer_name as gr_customer_name, /*string*/
po.tarif_parent_npp as tarif_parent_npp, /*number*/
po.tarif_parent_name as tarif_parent_name, /*string*/
po.tarif_npp as tarif_npp, /*number*/
po.tarif_name as tarif_name, /*string*/
nvl( case when (po.voltage = 2) then nvl(po.cust,0) end ,0) as cust_2, /*number*/
nvl( case when (po.voltage = 1) then nvl(po.cust,0) end ,0) as cust_1/*number*/
from (
--76607_data_pre
select po.kod_dog as kod_dog, /*number*/
po.kod_numobj as kod_numobj, /*number*/
po.ndog as ndog, /*string*/
po.ndog || '.' || ( TRIM( ( to_char( d.num_obj , '0000' ) ) ) ) as ndog_obj, /*string*/
po.payer_name as payer_name, /*string*/
po.point_num as point_num, /*number*/
po.point_name as point_name, /*string*/
po.gr_customer_id as gr_customer_id, /*number*/
po.gr_customer_name as gr_customer_name, /*string*/
po.tarif_npp as tarif_npp, /*number*/
po.tarif_name as tarif_name, /*string*/
po.tarif_parent_npp as tarif_parent_npp, /*number*/
po.tarif_parent_name as tarif_parent_name, /*number*/
po.voltage as voltage, /*number*/
 case when (po.rym is null ) then po.cust else 0 end as cust, /*number*/
 case when (po.rym is not null ) then po.cust else 0 end as cust_re/*number*/
from (
--rr_rep_po
select a.kod_dog as kod_dog, /*number*/
a.ndog as ndog, /**//*string*/
a.voltage as voltage, /**//*number*/
a.cust as cust, /**//*number*/
a.payer_name as payer_name, /**//*string*/
a.kod_numobj as kod_numobj, /*number*/
a.rym as rym, /**//*number*/
a.gr_customer_id as gr_customer_id, /**//*number*/
a.gr_customer_name as gr_customer_name, /**//*string*/
a.point_num as point_num, /**//*number*/
a.point_name as point_name, /**//*string*/
a.tarif_name as tarif_name, /**//*string*/
a.tarif_parent_name as tarif_parent_name, /**//*string*/
a.tarif_npp as tarif_npp, /**//*number*/
a.tarif_parent_npp as tarif_parent_npp/**//*number*/
from rr_rep_po
a
--\rr_rep_po
)
po
--\rr_rep_po
inner join
(
--rr_rep_dog_obj
select a.kod_numobj as kod_numobj, /*number*/
a.num_obj as num_obj/**//*number*/
from rr_rep_dog_obj
a
--\rr_rep_dog_obj
)
d on po.kod_numobj = d.kod_numobj--\rr_rep_dog_obj
)
po
--\76607_data_pre
)
grpd
--\
)
--\
--76607_data
select '76607_data' as skod,
'76607_data|#'||mtr.growid as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select decode( p1.groupingid , '0' , '' , '16' , '' , '24' , '' , '28' , '' , '30' , '' , '31' , '' ) as grsetname, /*string*/
p1.kod_dog as kod_dog, /*number*/
p1.kod_numobj as kod_numobj, /*number*/
p1.ndog as ndog, /*string*/
p1.ndog_obj as ndog_obj, /*string*/
p1.payer_name as payer_name, /*string*/
p1.point_num as point_num, /*number*/
p1.point_name as point_name, /*string*/
p1.gr_customer_id as gr_customer_id, /*number*/
p1.gr_customer_name as gr_customer_name, /*string*/
p1.tarif_parent_npp as tarif_parent_npp, /*number*/
p1.tarif_parent_name as tarif_parent_name, /*string*/
p1.tarif_npp as tarif_npp, /*number*/
p1.tarif_name as tarif_name, /*string*/
p1.cust_2 as cust_2, /*number*/
p1.cust_1 as cust_1, /*number*/
 row_number() over( order by null ) as gr_rn, /*number*/
'#' || ( case when (0 = ( bitand( 1 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.gr_customer_id ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 2 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.tarif_parent_npp ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 4 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.kod_dog ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 8 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.kod_numobj ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 16 , p1.groupingid ) ) ) then ( coalesce( ( to_char( p1.point_num ) ) , ' ' ) ) end ) || '#' as growid, /**//*string*//*key*/
 case when (p1.par_groupingid is null ) then null else ('#' || ( case when (0 = ( bitand( 1 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.gr_customer_id ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 2 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.tarif_parent_npp ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 4 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.kod_dog ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 8 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.kod_numobj ) ) , ' ' ) ) end ) || '#' || ( case when (0 = ( bitand( 16 , p1.par_groupingid ) ) ) then ( coalesce( ( to_char( p1.point_num ) ) , ' ' ) ) end ) || '#') end as parent_growid, /**//*string*/
p1.groupingid as groupingid, /*number*/
 decode( p1.groupingid , '0' , 'point' , '16' , 'obj' , '24' , 'dog' , '28' , 'tarif_p' , '30' , 'gr' , '31' , 'itog' ) as grsetid, /**//*string*/
 decode( p1.groupingid , '0' , 'point' , '16' , 'obj' , '24' , 'dog' , '28' , 'tarif_p' , '30' , 'gr' , '31' , 'itog' ) as origgrsetid, /**//*string*/
 decode( p1.groupingid , '0' , 'obj' , '16' , 'dog' , '24' , 'tarif_p' , '28' , 'gr' ) as parent_grsetid, /**//*string*/
p1.par_groupingid as par_groupingid/**//*number*/
from (
--
select grsets_query.kod_dog as kod_dog, /*number*/
grsets_query.kod_numobj as kod_numobj, /*number*/
grsets_query.ndog as ndog, /*string*/
grsets_query.ndog_obj as ndog_obj, /*string*/
grsets_query.payer_name as payer_name, /*string*/
grsets_query.point_num as point_num, /*number*/
grsets_query.point_name as point_name, /*string*/
grsets_query.gr_customer_id as gr_customer_id, /*number*/
grsets_query.gr_customer_name as gr_customer_name, /*string*/
grsets_query.tarif_parent_npp as tarif_parent_npp, /*number*/
grsets_query.tarif_parent_name as tarif_parent_name, /*string*/
grsets_query.tarif_npp as tarif_npp, /*number*/
grsets_query.tarif_name as tarif_name, /*string*/
grsets_query.cust_2 as cust_2, /*number*/
grsets_query.cust_1 as cust_1, /*number*/
grsets_query.groupingid as groupingid, /*number*/
 decode( grsets_query.groupingid , '0' , '16' , '16' , '24' , '24' , '28' , '28' , '30' ) as par_groupingid/**//*number*/
from (
--
select grpd.kod_dog as kod_dog, /*number*/
grpd.kod_numobj as kod_numobj, /*number*/
max(grpd.ndog) as ndog, /*string*/
max(grpd.ndog_obj) as ndog_obj, /*string*/
max(grpd.payer_name) as payer_name, /*string*/
grpd.point_num as point_num, /*number*/
max(grpd.point_name) as point_name, /*string*/
grpd.gr_customer_id as gr_customer_id, /*number*/
max(grpd.gr_customer_name) as gr_customer_name, /*string*/
grpd.tarif_parent_npp as tarif_parent_npp, /*number*/
max(grpd.tarif_parent_name) as tarif_parent_name, /*string*/
max(grpd.tarif_npp) as tarif_npp, /*number*/
max(grpd.tarif_name) as tarif_name, /*string*/
sum(grpd.cust_2) as cust_2, /*number*/
sum(grpd.cust_1) as cust_1, /*number*/
 grouping_id( grpd.point_num , grpd.kod_numobj , grpd.kod_dog , grpd.tarif_parent_npp , grpd.gr_customer_id ) as groupingid/*number*/
from mat1
grpd
--\mat1
group by grouping sets ((
grpd.point_num, 
grpd.kod_numobj, 
grpd.kod_dog, 
grpd.tarif_parent_npp, 
grpd.gr_customer_id
),
(
grpd.kod_numobj, 
grpd.kod_dog, 
grpd.tarif_parent_npp, 
grpd.gr_customer_id
),
(
grpd.kod_dog, 
grpd.tarif_parent_npp, 
grpd.gr_customer_id
),
(
grpd.tarif_parent_npp, 
grpd.gr_customer_id
),
(
grpd.gr_customer_id
),
(
))
)
grsets_query
--\
)
p1
--\
order by gr_customer_name,gr_customer_id nulls first,tarif_parent_name,tarif_parent_npp nulls first,lpad(ndog,10,'0'),kod_dog nulls first,ndog_obj,kod_numobj nulls first,point_num,point_num nulls first) mtr
;
--\76607_data
delete from rr_temp where skod = 'title_info';
insert into rr_temp
(
skod, --skod
sid, --sid
rn, --rn
s1, --mes
s2, --mes_pp
n1, --year
s3, --mes_end
n2, --year_end
s4, --ym_beg
s5, --ym_end
s6, --mes_next_end
s7, --year_for_mes_next_end
s8, --first_day_ym_beg
s9, --last_day_ym_end
s10, --p_dep_text
s11, --address_p
s12, --address_rs
s13, --adr_name
s14, --p_okpo
s15, --p_ogrn
s16, --p_inn
d1, --dat
s17, --rs_name
s18, --is_flag
s19--per_or_from_to
)
--title_info
--title_info
select 'title_info' as skod,
'title_info|' as sid,
row_number() over (order by 1) as rn,
mtr.*
from
(
select decode ( ( mod( :p_ym_beg , 1)*100 ) ,1,'январь',2,'февраль',3,'март',4,'апрель',5,'май',6,'июнь',7,'июль',8,'август',9,'сентябрь',10,'октябрь',11,'ноябрь',12,'декабрь') as mes, /*string*/
 lower( ( decode ( ( mod( :p_ym_beg , 1)*100 ) ,1,'Январе',2,'Феврале',3,'Марте',4,'Апреле',5,'Мае',6,'Июне',7,'Июле',8,'Августе',9,'Сентябре',10,'Октябре',11,'Ноябре',12,'Декабре') ) ) as mes_pp, /*Название месяца в предложном падеже*//*string*/
 trunc( :p_ym_beg ) as year, /*number*/
 decode ( ( mod( :p_ym_beg , 1)*100 ) ,1,'январь',2,'февраль',3,'март',4,'апрель',5,'май',6,'июнь',7,'июль',8,'август',9,'сентябрь',10,'октябрь',11,'ноябрь',12,'декабрь') as mes_end, /*string*/
 trunc( :p_ym_beg ) as year_end, /*number*/
( trim(to_char( :p_ym_beg ,'9999.99')) ) as ym_beg, /*string*/
( trim(to_char( :p_ym_beg ,'9999.99')) ) as ym_end, /* *//*string*/
 to_char( ( kg.ym_first_day( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( :p_ym_beg *10000+01),'YYYYMMDD') ) , 1 ) ,'YYYYMM'))/100 ) ) ) , 'DD.MM.YYYY' ) as mes_next_end, /*string*/
 trunc( ( to_number(to_char(ADD_MONTHS( (to_date(to_char( :p_ym_beg *10000+01),'YYYYMMDD') ) , 1 ) ,'YYYYMM'))/100 ) ) as year_for_mes_next_end, /* *//*string*/
 to_char( ( kg.ym_first_day( :p_ym_beg ) ) , 'DD.MM.YYYY' ) as first_day_ym_beg, /*string*/
 to_char( ( kg.ym_last_day( :p_ym_beg ) ) , 'DD.MM.YYYY' ) as last_day_ym_end, /*string*/
 case when (a.name is not null ) then a.name else ' ' end as p_dep_text, /*Отделение*//*string*/
 case when (adr.kf_adress_o is not null ) then adr.kf_adress_o else ' ' end as address_p, /*string*/
 case when (( nk_adress.kf_address( 1 , rs.kodd ) ) is not null ) then ( nk_adress.kf_address( 1 , rs.kodd ) ) else ' ' end as address_rs, /*string*/
 nvl( (
--
select stragg_dist(bb.name_p) as name_p/*key*/
from adr_m
bb
--\adr_m
where
( 0=1 ) )
--\
 , 'все ' ) as adr_name, /*string*/
 case when (p.okpo is not null ) then p.okpo end as p_okpo, /*string*/
 case when (p.ogrn is not null ) then p.ogrn end as p_ogrn, /*ОГРН*//*string*/
 case when (p.inn is not null ) then p.inn end as p_inn, /*ИНН*//*string*/
 sysdate as dat, /*date*/
rs.name as rs_name, /*Название энергосистемы*//*string*/
 case when (( nvl( /*nvlu*/ 0 , 0 ) ) = 1) then 'Да' else 'Нет' end as is_flag, /*string*/
 case when (( nvl( /*nvlu*/ :p_ym_beg , :p_ym_beg ) ) = :p_ym_beg ) then ('за ' || ( decode ( ( mod( :p_ym_beg , 1)*100 ) ,1,'январь',2,'февраль',3,'март',4,'апрель',5,'май',6,'июнь',7,'июль',8,'август',9,'сентябрь',10,'октябрь',11,'ноябрь',12,'декабрь') ) || ' ' || ( trunc( :p_ym_beg ) ) || ' г.') else ('c ' || (( trim(to_char( :p_ym_beg ,'9999.99')) ) ) || ' по ' || (( trim(to_char( :p_ym_beg ,'9999.99')) ) ) ) end as per_or_from_to/*string*/
from (
--rs_esys
select a.kod_esys as kod_esys, /*number*//*key*/
a.name as name, /*Наименование*//*string*/
a.kodd as kodd/**//*number*/
from rs_esys
a
--\rs_esys
)
rs
--\rs_esys
left outer join
(
--kr_org_one
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Наименование*//*string*/
from (
--kr_org
select a.kodp as kodp, /**//*number*//*key*/
a.name as name/*Отделение*//*string*/
from kr_org
a
--\kr_org
)
a
--\kr_org
where
a.kodp = :p_dep )
a on 1 =  1  --\kr_org_one
left outer join
(
--kr_payer
select a.kodp as kodp, /**//*number*//*key*/
a.inn as inn, /*ИНН*//*string*/
a.okpo as okpo, /**//*string*/
a.ogrn as ogrn, /*ОГРН*//*string*/
a.kod_d_p as kod_d_p/*number*/
from kr_payer
a
--\kr_payer
)
p on a.kodp = p.kodp--\kr_payer
left outer join
(
--k_house
select a.kodd as kodd, /**//*number*//*key*/
a.kf_adress_o as kf_adress_o/*Адрес*//*string*/
from k_house
a
--\k_house
)
adr on p.kod_d_p = adr.kodd--\k_house
) mtr
;
--\title_info
end;");

            Console.WriteLine("Extracted source tables:");
            foreach (var tableName in tableNames.OrderBy(t => t))
            {
                Console.WriteLine($"  - {tableName}");
            }
            Console.WriteLine($"Total: {tableNames.Count} tables");
            Console.Write("done");

        }

        public static void Main3(string[] args)
        {
            DevAnalyzer.Enabled = true;
            DevAnalyzer.ClearTempFolder();
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            XmlReports.SetGlobalParValue("dep", 3580m);
            var pars = new Dictionary<string, object>();


            pars.Add("p_dep", 3580m);
            pars.Add("p_ym_beg", 2025.06m);

            var path = CleanSqlBuilder.ExecReportGetPath("ryazan.76607", pars, "76607.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            //var fileUri = new Uri(path).ToString();
            Console.WriteLine(path);
            var tablenames = DevAnalyzer.tableNames;
            Console.WriteLine("done");

        }

        public static void Main2(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            XmlReports.SetGlobalParValue("dep", 3580m);
            var pars = new Dictionary<string, object>();


            pars.Add("p_dep", 3580m);
            pars.Add("p_ym_beg", 2025.06m);

            var path =  CleanSqlBuilder.ExecReportGetPath("ryazan.76607", pars, "76607.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            //var fileUri = new Uri(path).ToString();
            Console.WriteLine(path);
            Console.WriteLine("done");

        }

        public static void Main1(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";
            //var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            var pars = new Dictionary<string, object>();
          

            pars.Add("p_date_s", new DateTime(2020, 1, 8));
            pars.Add("p_date_po", new DateTime(2025, 1, 8));
            pars.Add("p_kodp", new List<int> { 1172, 1210, 1211, 1212, 1214, 1215
                //, 1216, 1217, 1218, 1219
            });

            var path = CleanSqlBuilder.ExecReportGetPath("asuse2.65211", pars, "65211.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            var fileUri = new Uri(path).ToString();
            Console.WriteLine(fileUri); // VS Code will make this clickable
            Console.WriteLine("done");

        }
    }
}
