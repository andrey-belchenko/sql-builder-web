select 
customer,
count(1) as rep_count,
sum(case when exec_1m>0 then 1 else 0 end) as rep_count_1m,
sum(case when exec_3m>0 then 1 else 0 end) as rep_count_3m,
sum(case when exec_12m>0 then 1 else 0 end) as rep_count_12m
from  report_dev_sqlb.report_exec_stat  
group by customer