/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
--select docpack,cnt1 from (select docpack,count(docpack) as cnt1 from [Archive].[dbo].[zao_stg_archive] where del=0 and docpack>0 group by docpack) as a where cnt1>1 




Select distinct a.docpack From (SELECT count(id) as cnt,docpack,id_frm_contr,id_prjcode,id_parent,summ
  FROM [Archive].[dbo].[zao_stg_archive] where del=0 and docpack in
  (select docpack from (select docpack,count(docpack) as cnt1 from [Archive].[dbo].[zao_stg_archive] where del=0 and docpack>0 group by docpack) as a where cnt1>1 ) 
  and date_doc>'2015-01-01 00:00:00.000'
  group by docpack,id_frm_contr,id_prjcode,id_parent,summ) as a where cnt=1 order by docpack




Select distinct a.docpack From (SELECT count(id) as cnt,docpack,id_frm_contr,id_prjcode,id_parent,summ
  FROM [Archive].[dbo].[zao_stg_archive] where del=0 and docpack in
  (select docpack from (select docpack,count(docpack) as cnt1 from [Archive].[dbo].[zao_stg_archive] where del=0 and docpack>0 group by docpack) as a where cnt1>1 ) 
  and date_doc>'2014-11-01 00:00:00.000' and date_doc<'2015-01-01 00:00:00.000'
  group by docpack,id_frm_contr,id_prjcode,id_parent,summ) as a where cnt=1 order by docpack


--Select distinct a.docpack From (SELECT count(id) as cnt,docpack,id_frm_contr,id_prjcode,id_parent,summ
--  FROM [Archive].[dbo].[zao_stg_archive] where del=0 and docpack>0 and date_doc>'2014-11-01 00:00:00.000' and date_doc<'2015-01-01 00:00:00.000'
--  group by docpack,id_frm_contr,id_prjcode,id_parent,summ) as a where cnt=1 order by docpack