CREATE OR REPLACE VIEW ASUSE.NV_ACCOUNT_SOST_NAL
 (
 KOD_ACCOUNT_SOST, KOD_ACCOUNT_PRIEM, TIP_TARIF_SOST, CUST, PRICE, KOD_ACCOUNT, 
KOD_NAL, KOD_NALTAX, PR_ADD, TAX, NAL, NACHISL
 )
 AS 
select a.KOD_ACCOUNT_SOST, a.KOD_ACCOUNT_PRIEM, a.TIP_TARIF_SOST, a.CUST,a.PRICE,a.KOD_ACCOUNT, n.kod_nal, n.kod_naltax, n.pr_add,n.tax,
    a.cust*a.price*n.drob nal, decode(n.pr_add, 1,a.cust*a.price+ a.cust*a.price*n.drob,a.cust*a.price) as nachisl
  from (select b.kod_account, n.pr_add, n.kod_nal, n.tax, n.kod_naltax, nvl(sum(n.nal),0) nal,
   case when b.nachisl-nvl(sum(nal),0)=0 then 0 else nvl(sum(n.nal),0)/(nachisl-nvl(sum(nal),0)) end drob from  ec_account.nr_account b, ec_account.nr_account_nal n where n.kod_account(+)=b.kod_account
   group by b.kod_account, n.pr_add, n.kod_nal, n.tax, n.kod_naltax, b.nachisl) n, ec_calc.nr_account_sost a where n.kod_account=a.kod_account
